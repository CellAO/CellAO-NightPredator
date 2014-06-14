#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
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
// 

#endregion

namespace ChatEngine
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Threading.Tasks;

    using CellAO.Communication.ISComV2Server;
    using CellAO.Core.Components;

    using ChatEngine.CoreServer;

    using Chatengine.Relay;

    using ChatEngine.Relay.Common;

    using locales;

    using NBug;
    using NBug.Properties;

    using NLog;

    using Utility;

    using Config = Utility.Config.ConfigReadWrite;

    #endregion

    /// <summary>
    /// Program class for ChatEngine
    /// </summary>
    internal class Program
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static ISComV2Server ISCom;

        /// <summary>
        /// </summary>
        private static readonly IContainer Container = new MefContainer();

        /// <summary>
        /// </summary>
        private static ChatServer chatServer;

        /// <summary>
        /// </summary>
        private static ConsoleText ct = new ConsoleText();

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        private static void CommandLoop(string[] args)
        {
            // Hard coded, because we only use TCP connections
            const bool TCPEnable = true;
            const bool UDPEnable = false;

            bool processedargs = false;

            string consoleCommand;

            while (true)
            {
                if (!processedargs)
                {
                    if (args.Length == 1)
                    {
                        if (args[0].ToLower() == "/autostart")
                        {
                            Console.WriteLine(locales.ServerConsoleAutostart);
                            StartRelayBot();
                            chatServer.Start(TCPEnable, UDPEnable);
                        }

                        processedargs = true;
                    }
                }

                Console.Write(Environment.NewLine + locales.ServerConsoleCommand + ">>");

                consoleCommand = Console.ReadLine();

                while (consoleCommand.IndexOf("  ") > -1)
                {
                    consoleCommand = consoleCommand.Replace("  ", " ");
                }

                consoleCommand = consoleCommand.Trim();
                switch (consoleCommand.ToLower())
                {
                    case "start":
                        if (chatServer.IsRunning)
                        {
                            Colouring.Push(ConsoleColor.Red);
                            Console.WriteLine(locales.ServerConsoleServerIsRunning);
                            Colouring.Pop();
                            break;
                        }

                        StartRelayBot();
                        chatServer.Start(TCPEnable, UDPEnable);
                        break;
                    case "stop":
                        if (!chatServer.IsRunning)
                        {
                            Colouring.Push(ConsoleColor.Red);
                            Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
                            Colouring.Pop();
                            break;
                        }

                        chatServer.Stop();
                        break;
                    case "exit":
                        return;
                    case "running":
                        if (chatServer.IsRunning)
                        {
                            Console.WriteLine(locales.ServerConsoleServerIsRunning);
                            break;
                        }

                        Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
                        break;

                    case "help":
                        ct.TextRead("chatcmdhelp.txt");
                        break;
                    case "help start":
                        Console.WriteLine(locales.ServerConsoleCommandHelp_start);
                        break;
                    case "help exit":
                        Console.WriteLine(locales.ServerConsoleCommandHelp_stop);
                        break;
                    case "help running":
                        ct.TextRead("chathelpcmdrunning.txt");
                        break;
                    case "debugnetwork":
                        LogUtil.Toggle(DebugInfoDetail.Network);
                        break;
                }
            }
        }

        private static void StartRelayBot()
        {
            if (Config.Instance.CurrentConfig.UseIRCRelay == true)
            {
                Console.WriteLine("Starting RelayBot. Version {0}", ProgramInfo.AssemblyVersion);

                // Call the IRC Bot stuff here..
                RelayBot ircbot = new RelayBot();

                ircbot.Run(chatServer);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool Initialize()
        {
            try
            {
                chatServer = new ChatServer();

                if (!InitializeLogAndBug())
                {
                    return false;
                }

                if (!InitializeTCP())
                {
                    return false;
                }

                if (!InitializeISCom())
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeISCom()
        {
            try
            {
                ISCom = new ISComV2Server();
                ISCom.DataReceived += chatServer.ISComDataReceived;
                if (Config.Instance.CurrentConfig.ListenIP == "0.0.0.0")
                {
                    ISCom.TcpEndPoint = new IPEndPoint(IPAddress.Any, Config.Instance.CurrentConfig.CommPort);
                }
                else
                {
                    ISCom.TcpEndPoint = new IPEndPoint(
                        IPAddress.Parse(Config.Instance.CurrentConfig.ListenIP),
                        Config.Instance.CurrentConfig.CommPort);
                }

                ISCom.Start(true, false);
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
        private static bool InitializeLogAndBug()
        {
            try
            {
                // Setup and enable NLog logging to file
                LogUtil.SetupConsoleLogging(LogLevel.Debug);
                LogUtil.SetupFileLogging("${basedir}/ChatEngineLog.txt", LogLevel.Trace);

                // NBug initialization
                SettingsOverride.LoadCustomSettings("NBug.ChatEngine.config");
                Settings.WriteLogToDisk = true;
                AppDomain.CurrentDomain.UnhandledException += Handler.UnhandledException;
                TaskScheduler.UnobservedTaskException += Handler.UnobservedTaskException;
            }
            catch (Exception e)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Error occured while initalizing NLog/NBug");
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
        private static bool InitializeTCP()
        {
            int Port = Convert.ToInt32(Config.Instance.CurrentConfig.ChatPort);
            try
            {
                if (Config.Instance.CurrentConfig.ListenIP == "0.0.0.0")
                {
                    chatServer.TcpEndPoint = new IPEndPoint(IPAddress.Any, Port);
                }
                else
                {
                    chatServer.TcpEndPoint = new IPEndPoint(
                        IPAddress.Parse(Config.Instance.CurrentConfig.ListenIP),
                        Port);
                }

                chatServer.MaximumPendingConnections = 100;
            }
            catch (Exception e)
            {
                Console.WriteLine(locales.ErrorIPAddressParseFailed);
                Console.Write(e.Message);
                Console.ReadKey();
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

            OnScreenBanner.PrintCellAOBanner(ConsoleColor.Yellow);

            Console.WriteLine();

            Console.WriteLine(locales.ServerConsoleMainText, DateTime.Now.Year);

            if (!Initialize())
            {
                Console.WriteLine("Error occured while initilizing. Please check in log.");
                return;
            }

            CommandLoop(args);
            LogManager.Configuration = null;
        }

        #endregion
    }
}