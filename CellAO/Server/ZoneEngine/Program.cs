#region License

// Copyright (c) 2005-2013, CellAO Team
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
    using CellAO.Core.Components;
    using CellAO.Core.Items;
    using CellAO.Core.Nanos;
    using CellAO.Database;

    using NBug;
    using NBug.Properties;

    using NLog;

    using Utility;
    using Utility.Config;

    using ZoneEngine.Core;
    using ZoneEngine.Core.Functions;
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
        private static ConsoleText ct;

        /// <summary>
        /// </summary>
        private static bool exited = false;

        #endregion

        #region Methods

        /// <summary>
        /// Check the database
        /// </summary>
        private static void CheckDatabase()
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
            ct.TextRead("zone_consolecommands.txt");
            while (!exited)
            {
                if (!processedargs)
                {
                    if (args.Length == 1)
                    {
                        if (args[0].ToLower() == "/autostart")
                        {
                            ct.TextRead("autostart.txt");
                            csc.Compile(false);
                            StartTheServer();
                        }
                    }

                    processedargs = true;
                }

                Console.Write("\nServer Command >>");
                string consoleCommand = Console.ReadLine();

                if (!consoleCommands.Execute(consoleCommand))
                {
                    ct.TextRead("zone_consolecmdsdefault.txt");
                }
            }
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
            catch (Exception)
            {
                return false;
            }

            try
            {
                ISComClient.Connect(chatEngineIp, port);
            }
            catch
            {
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
            Console.ForegroundColor = ConsoleColor.Green;

            if (!InitializeGameFunctions())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error in Gamefunction initialization");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!InitializeLogAndBug())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while initializing NLog/NBug");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!CheckZoneServerCreation())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while creating ZoneServer instance");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!ISComInitialization())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error initializing ISCom");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!InizializeTCPIP())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error initializing TCP/IP setup");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!InitializeScriptCompiler())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while creating ScriptCompiler instance");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!Misc.CheckDatabase())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while initializing database");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            if (!LoadItemsAndNanos())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while loading Items/Nanos");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            if (!InitializeConsoleCommands())
            {
                return false;
            }

            Console.ForegroundColor = ConsoleColor.White;

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeConsoleCommands()
        {
            consoleCommands.AddEntry("start", StartServer);
            consoleCommands.AddEntry("exit", ShutDownServer);
            consoleCommands.AddEntry("quit", ShutDownServer);
            consoleCommands.AddEntry("online", ShowOnlineCharacters);
            consoleCommands.AddEntry("running", IsServerRunning);
            consoleCommands.AddEntry("ls", ListAvailableScripts);
            consoleCommands.AddEntry("check", CheckDatabase);
            consoleCommands.AddEntry("updatedb", CheckDatabase);
            consoleCommands.AddEntry("ping", PingChatServer);
            consoleCommands.AddEntry("stop", StopServer);
            consoleCommands.AddEntry("startm", StartServerMultipleScriptDlls);

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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    "{0} Game functions loaded", 
                    FunctionCollection.Instance.NumberofRegisteredFunctions());
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
        private static bool InitializeLogAndBug()
        {
            try
            {
                // Setup and enable NLog logging to file
                LogUtil.SetupConsoleLogging(LogLevel.Debug);
                LogUtil.SetupFileLogging("${basedir}/LoginEngineLog.txt", LogLevel.Trace);

                // NBug initialization
                SettingsOverride.LoadCustomSettings("NBug.LoginEngine.config");
                Settings.WriteLogToDisk = true;
                AppDomain.CurrentDomain.UnhandledException += Handler.UnhandledException;
                TaskScheduler.UnobservedTaskException += Handler.UnobservedTaskException;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while initalizing NLog/NBug");
                Console.WriteLine(e.Message);
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
            catch (Exception)
            {
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
                ct.TextRead("ip_config_parse_error.txt");
                Console.Write(e.Message);
                Console.ReadKey();
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        private static void IsServerRunning()
        {
            if (zoneServer.IsRunning)
            {
                Console.WriteLine("Zone Server is Running");
            }
            else
            {
                Console.WriteLine("Zone Server not Running");
            }
        }

        /// <summary>
        /// </summary>
        private static void ListAvailableScripts()
        {
            // list all available scripts, dont remove it since it does what it should
            Console.WriteLine("Available scripts");

            string[] files = Directory.GetFiles(
                "Scripts" + Path.DirectorySeparatorChar, 
                "*.cs", 
                SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Console.WriteLine("No scripts were found.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (string s in files)
            {
                Console.WriteLine(s);
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Load items and Nanos into static lists
        /// </summary>
        /// <returns>
        /// true if ok
        /// </returns>
        private static bool LoadItemsAndNanos()
        {
            try
            {
                Console.WriteLine("Loaded {0} items", ItemLoader.CacheAllItems());
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured while loading the items.dat.");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            try
            {
                Console.WriteLine("Loaded {0} nanos", NanoLoader.CacheAllNanos());
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured while loading the nanos.dat.");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
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
            ct = new ConsoleText();

            OnScreenBanner.PrintCellAOBanner(ConsoleColor.Green);

            Console.WriteLine();
            ct.TextRead("main.txt");

            if (!Initialize())
            {
                Console.WriteLine("Error occurred while initializing. Please check the log file.");
                Console.ReadLine();
            }
            else
            {
                CommandLoop(args);
            }

            // NLog<->Mono lockup fix
            LogManager.Configuration = null;
        }

        /// <summary>
        /// </summary>
        private static void PingChatServer()
        {
            // ChatCom.Server.Ping();
            Console.WriteLine("Ping is disabled till we can fix it");
        }

        /// <summary>
        /// </summary>
        private static void ShowOnlineCharacters()
        {
            if (zoneServer.IsRunning)
            {
                Console.ForegroundColor = ConsoleColor.White;

                // TODO: Check all clients inside playfields
                lock (zoneServer.Clients)
                {
                    foreach (ZoneClient c in zoneServer.Clients)
                    {
                        Console.WriteLine("Character " + c.Character.Name + " online");
                    }
                }

                Console.ResetColor();
            }
        }

        /// <summary>
        /// </summary>
        private static void ShutDownServer()
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
        private static void StartServer()
        {
            if (zoneServer.IsRunning)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Zone Server is already running");
                Console.ResetColor();
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
        private static void StartServerMultipleScriptDlls()
        {
            // Multiple dll compile
            if (zoneServer.IsRunning)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Zone Server is already running");
                Console.ResetColor();
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

            csc.AddScriptMembers();
            zoneServer.Start(true, false);
        }

        /// <summary>
        /// </summary>
        private static void StopServer()
        {
            if (!zoneServer.IsRunning)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Zone Server is not running");
                Console.ResetColor();
            }
            else
            {
                zoneServer.Stop();
            }
        }

        #endregion
    }
}