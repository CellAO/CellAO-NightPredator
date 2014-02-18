    using System;
    using System.Net;
    using System.Threading.Tasks;

    using NBug;
    using NBug.Properties;

    using NLog;

    using Utility;

    using Config = Utility.Config.ConfigReadWrite;

namespace WebEngine
{
    using System.ComponentModel;

    internal class Program
    {
        #region Static Fields

        public static bool DebugNetwork;


        private static ServerConsoleCommands consoleCommands = new ServerConsoleCommands();

        private static bool exited = false;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public static void StartTheServer()
        {
            var myServer = default(HttpServer);
            myServer = new HttpServer();
        }

        #endregion


        private static bool Initialize()
        {
            Console.WriteLine();
            Colouring.Push(ConsoleColor.Green);

            if (!InitializeLogAndBug())
            {
                Colouring.Push(ConsoleColor.Red);
                //Console.WriteLine(locales.ErrorInitializingNLogNBug);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            //if (!InitializeServerInstance())
            //{
            //    Colouring.Push(ConsoleColor.Red);
            //    //Console.WriteLine(locales.ErrorInitializingEngine);
            //    Colouring.Pop();
            //    Colouring.Pop();
            //    return false;
            //}

            if (!CheckDatabase())
            {
                Colouring.Push(ConsoleColor.Red);
                // Console.WriteLine(locales.ErrorInitializingDatabase);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!InitializeConsoleCommands())
            {
                Colouring.Pop();
                return false;
            }

            Colouring.Pop();
            return true;
        }


        private static bool InitializeConsoleCommands()
        {
            consoleCommands.Engine = "Web";

            consoleCommands.AddEntry("start", StartServer);
            consoleCommands.AddEntry("running", IsServerRunning);
            consoleCommands.AddEntry("stop", StopServer);
            consoleCommands.AddEntry("exit", ShutDownServer);
            consoleCommands.AddEntry("quit", ShutDownServer);
            consoleCommands.AddEntry("debugnetwork", SetDebugNetwork);
           // consoleCommands.AddEntry("checkphp", CheckPHP);
            //consoleCommands.AddEntry("checkWebCore", CheckWebCore);
            return true;
        }

        private static bool InitializeLogAndBug()
        {
            try
            {
                // Setup and enable NLog logging to file
                LogUtil.SetupConsoleLogging(LogLevel.Debug);
                LogUtil.SetupFileLogging("${basedir}/WebEngineLog.txt", LogLevel.Trace);

                // NBug initialization
                SettingsOverride.LoadCustomSettings("NBug.WebEngine.config");
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

        private static void IsServerRunning(string[] obj)
        {

            //TODO: Write Code to check if Server is Running


            //   Colouring.Push(ConsoleColor.White);
            //if (WebEngine.HttpServer.IsRunning)
            //  {
            //     Console.WriteLine(locales.ServerConsoleServerIsRunning);
            // }
            // else
            //  {
            //     Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
            // }

            //  Colouring.Pop();
        }

        private static bool CheckDatabase()
        {
            bool result = true;
            try
            {
                //LoginDataDao.GetAll();
                //TODO: Add code to load WebCore DB
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        private static void Main(string[] args)
        {
            OnScreenBanner.PrintCellAOBanner(ConsoleColor.Red);

            Console.WriteLine();
            //Console.WriteLine(locales.ServerConsoleMainText, DateTime.Now.Year);

            if (!Initialize())
            {
                // Console.WriteLine(locales.ErrorInitializingEngine);
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

        private static void CommandLoop(string[] args)
        {
            bool processedargs = false;
           // Console.WriteLine(locales.ZoneEngineConsoleCommands);

            while (!exited)
            {
                if (!processedargs)
                {
                    if (args.Length == 1)
                    {
                        if (args[0].ToLower() == "/autostart")
                        {
                           // Console.WriteLine(locales.ServerConsoleAutostart);
                            StartTheServer();
                        }
                    }

                    processedargs = true;
                }

                //Console.Write(Environment.NewLine + "{0} >>", locales.ServerConsoleCommand);
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
        private static void ShowCommandHelp()
        {
            Colouring.Push(ConsoleColor.White);
           // Console.WriteLine(locales.ServerConsoleAvailableCommands);
            Console.WriteLine("---------------------------");
            Console.WriteLine(consoleCommands.HelpAll());
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void ShutDownServer(string[] obj)
        {
            //if (loginServer.IsRunning)
            //{
            //    loginServer.Stop();
            //}

            //exited = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void StartServer(string[] parts)
        {
            //if (loginServer.IsRunning)
            //{
            //    Colouring.Push(ConsoleColor.Red);
            //    Console.WriteLine(locales.ServerConsoleServerIsRunning);
            //    Colouring.Pop();
            //}
            //else
            //{
            //    // TODO: Add Sql Check.
            //    StartTheServer();
            //}
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void StopServer(string[] obj)
        {
            //if (!loginServer.IsRunning)
            // {
            //    Colouring.Push(ConsoleColor.Red);
            //     Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
            //    Colouring.Pop();
            // }
            // else
            // {
            //loginServer.Stop();
            //}
        }

        private static void SetDebugNetwork(string[] obj)
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
    }
}
