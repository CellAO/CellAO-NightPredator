    using System;
    using System.Net;
    using System.Threading.Tasks;

    using NBug;
    using NBug.Properties;

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

        private static bool InitializeConsoleCommands()
        {
            consoleCommands.Engine = "Login";

            consoleCommands.AddEntry("start", StartServer);
            consoleCommands.AddEntry("running", IsServerRunning);
            consoleCommands.AddEntry("stop", StopServer);
            consoleCommands.AddEntry("exit", ShutDownServer);
            consoleCommands.AddEntry("quit", ShutDownServer);
            consoleCommands.AddEntry("debugnetwork", SetDebugNetwork);
            consoleCommands.AddEntry("checkphp", CheckPHP);
            consoleCommands.AddEntry("checkWebCore", CheckWebCore);
            return true;
        }


        static void Main(string[] args)
        {

        }
    }
}
