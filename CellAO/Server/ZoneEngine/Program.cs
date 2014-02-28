#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

namespace ZoneEngine
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using CellAO.Communication.ISComV2Client;
    using CellAO.Communication.Messages;
    using CellAO.Core.Components;
    using CellAO.Core.Items;
    using CellAO.Core.Nanos;
    using CellAO.Database;

    using locales;

    using NBug;
    using NBug.Properties;

    using NLog;

    using Utility;
    using Utility.Config;

    using ZoneEngine.Core;
    using ZoneEngine.Core.Functions;
    using ZoneEngine.Core.Playfields;
    using ZoneEngine.Script;

    #endregion

    /// <summary>
    /// Program Class for ZoneEngine
    /// </summary>
    internal class Program
    {
        // TODO: Find out why the MEFs are not working under MONO 2.10

        #region Static Fields

        /// <summary>
        /// </summary>
        public static readonly IContainer Container = new MefContainer();

        /// <summary>
        /// </summary>
        public static bool DebugGameFunctions;

        /// <summary>
        /// </summary>
        public static bool DebugNetwork;

        /// <summary>
        /// </summary>
        public static bool DebugZoning;

        /// <summary>
        /// </summary>
        public static ISComV2Client ISComClient;

        /// <summary>
        /// </summary>
        public static ScriptCompiler csc;

        /// <summary>
        /// </summary>
        public static ZoneServer zoneServer;

        /// <summary>
        /// </summary>
        private static ServerConsoleCommands consoleCommands = new ServerConsoleCommands();

        /// <summary>
        /// </summary>
        private static bool exited = false;

        #endregion

        #region Methods

        /// <summary>
        /// Check the database
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void CheckDatabase(string[] parts)
        {
            Misc.CheckDatabase();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool CheckZoneServerCreation()
        {
            try
            {
                zoneServer = Container.GetInstance<ZoneServer>();
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void CommandLoop(string[] args)
        {
            bool processedargs = false;
            Console.WriteLine(locales.ZoneEngineConsoleCommands);

            while (!exited)
            {
                if (!processedargs)
                {
                    if (args.Length == 1)
                    {
                        if (args[0].ToLower() == "/autostart")
                        {
                            Console.WriteLine(locales.ServerConsoleAutostart);
                            csc.Compile(false);
                            StartTheServer();
                        }
                    }

                    processedargs = true;
                }

                Console.Write(Environment.NewLine + "{0} >>", locales.ServerConsoleCommand);
                string consoleCommand = Console.ReadLine();

                if (consoleCommand != null)
                {
                    if (!consoleCommands.Execute(consoleCommand))
                    {
                        ShowCommandHelp();
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (zoneServer != null)
            {
                exited = true;
                zoneServer.DisconnectAllClients();
                LogUtil.Debug("Shutting down ZoneEngine hard");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="messageobject">
        /// </param>
        private static void ISComClientOnReceiveData(DynamicMessage messageobject)
        {
            zoneServer.ProcessISComMessage(messageobject);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool ISComInitialization()
        {
            int port;
            IPAddress chatEngineIp;
            try
            {
                ISComClient = new ISComV2Client();
                chatEngineIp = IPAddress.Parse(ConfigReadWrite.Instance.CurrentConfig.ChatIP);
                port = ConfigReadWrite.Instance.CurrentConfig.CommPort;
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                return false;
            }

            try
            {
                ISComClient.OnReceiveData += ISComClientOnReceiveData;
                ISComClient.Connect(chatEngineIp, port);
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                return true;
            }

            return true;
        }

        /// <summary>
        /// Initializing methods go here
        /// </summary>
        /// <returns>
        /// true if ok
        /// </returns>
        private static bool Initialize()
        {
            Console.WriteLine();
            Colouring.Push(ConsoleColor.Green);

            if (!InitializeGameFunctions())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingGamefunctions);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!InitializeLogAndBug())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingNLogNBug);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!CheckZoneServerCreation())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorCreatingZoneServerInstance);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!ISComInitialization())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingISCom);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!InizializeTCPIP())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorTCPIPSetup);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!InitializeScriptCompiler())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorCreatingScriptCompilerInstance);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!Misc.CheckDatabase())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingDatabase);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            Colouring.Push(ConsoleColor.Green);
            if (!LoadItemsAndNanos())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorLoadingItemsNanos);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();

            Colouring.Push(ConsoleColor.Green);
            if (!LoadTradeSkills())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("No locale yet: Error reading trade skills");
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();

            if (!InitializeConsoleCommands())
            {
                return false;
            }

            Colouring.Pop();

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeConsoleCommands()
        {
            consoleCommands.Engine = "Zone";

            consoleCommands.AddEntry("start", StartServer);
            consoleCommands.AddEntry("startm", StartServerMultipleScriptDlls);
            consoleCommands.AddEntry("running", IsServerRunning);
            consoleCommands.AddEntry("ping", PingChatServer);

            consoleCommands.AddEntry("stop", StopServer);

            consoleCommands.AddEntry("exit", ShutDownServer);
            consoleCommands.AddEntry("quit", ShutDownServer);

            consoleCommands.AddEntry("check", CheckDatabase);
            consoleCommands.AddEntry("updatedb", CheckDatabase);

            consoleCommands.AddEntry("online", ShowOnlineCharacters);
            consoleCommands.AddEntry("ls", ListAvailableScripts);

            consoleCommands.AddEntry("debuggamefunctions", SetDebugGameFunctions);

            consoleCommands.AddEntry("debugnetwork", SetDebugNetwork);

            consoleCommands.AddEntry("debugzoning", SetDebugZoning);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeGameFunctions()
        {
            try
            {
                Colouring.Push(ConsoleColor.Green);
                Console.WriteLine(
                    "{0} Game functions loaded", 
                    FunctionCollection.Instance.NumberofRegisteredFunctions());
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();
            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeLogAndBug()
        {
            try
            {
                // Setup and enable NLog logging to file
                LogUtil.SetupConsoleLogging(LogLevel.Debug);
                LogUtil.SetupFileLogging("${basedir}/ZoneEngineLog.txt", LogLevel.Trace);

                // NBug initialization
                SettingsOverride.LoadCustomSettings("NBug.ZoneEngine.config");
                Settings.WriteLogToDisk = true;
                AppDomain.CurrentDomain.UnhandledException += Handler.UnhandledException;
                TaskScheduler.UnobservedTaskException += Handler.UnobservedTaskException;
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingNLogNBug);
                Console.WriteLine(e.Message);
                Colouring.Pop();
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeScriptCompiler()
        {
            try
            {
                csc = new ScriptCompiler();
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InizializeTCPIP()
        {
            int Port = Convert.ToInt32(ConfigReadWrite.Instance.CurrentConfig.ZonePort);
            try
            {
                if (ConfigReadWrite.Instance.CurrentConfig.ListenIP == "0.0.0.0")
                {
                    zoneServer.TcpEndPoint = new IPEndPoint(IPAddress.Any, Port);
                }
                else
                {
                    zoneServer.TcpIP = IPAddress.Parse(ConfigReadWrite.Instance.CurrentConfig.ListenIP);
                }

                zoneServer.MaximumPendingConnections = 100;
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e); 
                
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorIPAddressParseFailed);
                Console.Write(e.Message);
                Colouring.Pop();
                Console.ReadKey();
                
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void IsServerRunning(string[] parts)
        {
            Colouring.Push(ConsoleColor.White);
            if (zoneServer.IsRunning)
            {
                Console.WriteLine(locales.ServerConsoleServerIsRunning);
            }
            else
            {
                Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
            }

            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void ListAvailableScripts(string[] parts)
        {
            // list all available scripts, dont remove it since it does what it should
            Colouring.Push(ConsoleColor.White);
            Console.WriteLine(locales.ServerConsoleAvailableScripts + ":");

            string[] files = Directory.GetFiles(
                "Scripts" + Path.DirectorySeparatorChar, 
                "*.cs", 
                SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Console.WriteLine(locales.ServerConsoleNoScriptsFound);
                return;
            }

            Colouring.Push(ConsoleColor.Green);
            foreach (string s in files)
            {
                Console.WriteLine(s);
            }

            Colouring.Pop();
        }

        /// <summary>
        /// Load items and Nanos into static lists
        /// </summary>
        /// <returns>
        /// true if ok
        /// </returns>
        private static bool LoadItemsAndNanos()
        {
            Colouring.Push(ConsoleColor.Green);
            try
            {
                Console.WriteLine(locales.ItemLoaderLoadedItems, ItemLoader.CacheAllItems());
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                Colouring.Pop();
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorReadingItemsFile);
                Console.WriteLine(e.Message);
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();

            Colouring.Push(ConsoleColor.Green);
            try
            {
                Console.WriteLine(locales.NanoLoaderLoadedNanos, NanoLoader.CacheAllNanos());
                Console.WriteLine();
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                Colouring.Pop();
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorReadingNanosFile);
                Console.WriteLine(e.Message);
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();

            Colouring.Push(ConsoleColor.Green);
            try
            {
                Console.WriteLine("Loaded {0} Playfields", PlayfieldLoader.CacheAllPlayfieldData());
                Console.WriteLine();
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                Colouring.Pop();
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Error reading statels.dat");
                Console.WriteLine(e.Message);
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool LoadTradeSkills()
        {
            try
            {
                int temp = TradeSkill.Instance.ItemNames.Count;
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">
        /// Command line parameters
        /// </param>
        private static void Main(string[] args)
        {
            Console.CancelKeyPress += ConsoleCancelKeyPress;

            OnScreenBanner.PrintCellAOBanner(ConsoleColor.Green);

            Console.WriteLine();
            Console.WriteLine(locales.ServerConsoleMainText, DateTime.Now.Year);

            if (!Initialize())
            {
                Console.WriteLine(locales.ErrorInitializingEngine);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
            else
            {
#if DEBUG
                StartTheServer();
#endif
                CommandLoop(args);
            }

            // NLog<->Mono lockup fix
            LogManager.Configuration = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void PingChatServer(string[] parts)
        {
            // ChatCom.Server.Ping();
            Console.WriteLine("Ping is disabled till we can do it");
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void SetDebugGameFunctions(string[] parts)
        {
            DebugGameFunctions = !DebugGameFunctions;
            Colouring.Push(ConsoleColor.Green);
            if (DebugGameFunctions)
            {
                Console.WriteLine("Debugging Game functions enabled");
            }
            else
            {
                Console.WriteLine("Debugging Game functions disabled");
            }

            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void SetDebugNetwork(string[] parts)
        {
            DebugNetwork = !DebugNetwork;
            Colouring.Push(ConsoleColor.Green);
            if (DebugNetwork)
            {
                Console.WriteLine("Debugging of network traffic enabled");
            }
            else
            {
                Console.WriteLine("Debugging of network traffic disabled");
            }

            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void SetDebugZoning(string[] parts)
        {
            DebugZoning = !DebugZoning;
            Colouring.Push(ConsoleColor.Green);
            if (DebugZoning)
            {
                Console.WriteLine("Debugging of zoning enabled");
            }
            else
            {
                Console.WriteLine("Debugging of zoning disabled");
            }

            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        private static void ShowCommandHelp()
        {
            Colouring.Push(ConsoleColor.White);
            Console.WriteLine(locales.ServerConsoleAvailableCommands);
            Console.WriteLine("---------------------------");
            Console.WriteLine(consoleCommands.HelpAll());
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void ShowOnlineCharacters(string[] parts)
        {
            if (zoneServer.IsRunning)
            {
                Colouring.Push(ConsoleColor.White);

                // TODO: Check all clients inside playfields
                lock (zoneServer.Clients)
                {
                    foreach (ZoneClient c in zoneServer.Clients)
                    {
                        Console.WriteLine(
                            "Character " + c.Character.Name + " online in PF " + c.Character.Playfield.Identity.Instance);
                    }
                }

                Colouring.Pop();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void ShutDownServer(string[] parts)
        {
            if (zoneServer.IsRunning)
            {
                zoneServer.Stop();
            }

            ISComClient.ShutDown();
            exited = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void StartServer(string[] parts)
        {
            if (zoneServer.IsRunning)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ServerConsoleServerIsRunning);
                Colouring.Pop();
            }
            else
            {
                // TODO: Add Sql Check.
                csc.Compile(false);
                StartTheServer();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void StartServerMultipleScriptDlls(string[] parts)
        {
            // Multiple dll compile
            if (zoneServer.IsRunning)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ServerConsoleServerIsRunning);
                Colouring.Pop();
            }
            else
            {
                // TODO: Add Sql Check.
                csc.Compile(true);
                StartTheServer();
            }
        }

        /// <summary>
        /// </summary>
        private static void StartTheServer()
        {
            // TODO: Read playfield data, check which playfields have to be created, and create them
            // TODO: Cache neccessary Spawns and Mobs
            // TODO: Cache neccessary Doors 
            // TODO: Cache neccessary statels
            // TODO: Cache Vendors

            // Console.WriteLine(Core.Playfields.Playfields.Instance.playfields[0].name);

            Console.WriteLine(csc.AddScriptMembers() + " chat commands loaded");
            zoneServer.Start(true, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void StopServer(string[] parts)
        {
            if (!zoneServer.IsRunning)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
                Colouring.Pop();
            }
            else
            {
                zoneServer.Stop();
            }
        }

        #endregion
    }
}