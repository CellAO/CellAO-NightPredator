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

namespace LoginEngine
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AO.Core.Encryption;

    using CellAO.Core.Components;
    using CellAO.Database.Dao;

    using LoginEngine.CoreServer;

    using NBug;
    using NBug.Properties;

    using NLog;

    using Utility;

    using Config = Utility.Config.ConfigReadWrite;

    #endregion

    /// <summary>
    /// Program class for LoginEngine
    /// </summary>
    internal class Program
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static bool DebugNetwork;

        /// <summary>
        /// </summary>
        private static readonly IContainer Container = new MefContainer();

        /// <summary>
        /// </summary>
        private static ConsoleText ct;

        /// <summary>
        /// </summary>
        private static LoginServer loginServer;

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
            ct.TextRead("login_consolecommands.txt");
            while (true)
            {
                if (!processedargs)
                {
                    if (args.Length == 1)
                    {
                        if (args[0].ToLower() == "/autostart")
                        {
                            ct.TextRead("autostart.txt");
                            loginServer.Start(TCPEnable, UDPEnable);
                        }
                    }

                    processedargs = true;
                }

                Console.Write(Environment.NewLine + "Server Command >>");

                consoleCommand = Console.ReadLine();

                while (consoleCommand.IndexOf("  ") > -1)
                {
                    consoleCommand = consoleCommand.Replace("  ", " ");
                }

                consoleCommand = consoleCommand.Trim();
                switch (consoleCommand.ToLower())
                {
                    case "start":
                        if (loginServer.IsRunning)
                        {
                            Colouring.Push(ConsoleColor.Red);
                            ct.TextRead("loginisrunning.txt");
                            Colouring.Pop();
                            break;
                        }

                        loginServer.Start(TCPEnable, UDPEnable);
                        break;
                    case "stop":
                        if (!loginServer.IsRunning)
                        {
                            Colouring.Push(ConsoleColor.Red);
                            ct.TextRead("loginisnotrunning.txt");
                            Colouring.Pop();
                            break;
                        }

                        loginServer.Stop();
                        break;
                    case "exit":
                        Process.GetCurrentProcess().Kill();
                        break;
                    case "running":
                        if (loginServer.IsRunning)
                        {
                            // Console.WriteLine("Login Server is running");
                            ct.TextRead("loginisrunning.txt");
                            break;
                        }

                        // Console.WriteLine("Login Server not running");
                        ct.TextRead("loginisnotrunning.txt");
                        break;

                    case "help":
                        ct.TextRead("logincmdhelp.txt");
                        break;
                    case "help start":
                        ct.TextRead("helpstart.txt");
                        break;
                    case "help exit":
                        ct.TextRead("helpstop.txt");
                        break;
                    case "help running":
                        ct.TextRead("loginhelpcmdrunning.txt");
                        break;
                    case "help Adduser":
                        ct.TextRead("logincmdadduserhelp.txt");
                        break;
                    case "help setpass":
                        ct.TextRead("logincmdhelpsetpass.txt");
                        break;
                    case "debugnetwork":
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
                        break;

                    default:
                        if (consoleCommand.ToLower().StartsWith("setgm"))
                        {
                            string[] parts = consoleCommand.Split(' ');
                            int gmlevel = 0;
                            if ((parts.Count() != 3) || (!Int32.TryParse(parts[2], out gmlevel)))
                            {
                                Console.WriteLine("Usage: logoffchars <username>");
                            }
                            else
                            {
                                LoginDataDao.SetGM(parts[1], gmlevel);
                                break;
                            }
                        }
                        if (consoleCommand.ToLower().StartsWith("logoffchars"))
                        {
                            string[] parts = consoleCommand.Split(' ');
                            if (parts.Count() != 2)
                            {
                                Console.WriteLine("Usage: logoffchars <username>");
                            }
                            else
                            {
                                LoginDataDao.LogoffChars(parts[1]);
                                break;
                            }
                        }
                        else

                            // This section handles the command for adding a user to the database
                            if (consoleCommand.ToLower().StartsWith("adduser"))
                            {
                                string[] parts = consoleCommand.Split(' ');
                                if (parts.Length < 9)
                                {
                                    Console.WriteLine(
                                        "Invalid command syntax." + Environment.NewLine + "Please use:"
                                        + Environment.NewLine
                                        + "Adduser <username> <password> <number of characters> <expansion> <gm level> <email> <FirstName> <LastName>");
                                    break;
                                }

                                string username = parts[1];
                                string password = parts[2];
                                int numChars = 0;
                                try
                                {
                                    numChars = int.Parse(parts[3]);
                                }
                                catch
                                {
                                    Console.WriteLine("Error: <number of characters> must be a number (duh!)");
                                    break;
                                }

                                int expansions = 0;
                                try
                                {
                                    expansions = int.Parse(parts[4]);
                                }
                                catch
                                {
                                    Console.WriteLine("Error: <expansions> must be a number between 0 and 2047!");
                                    break;
                                }

                                if (expansions < 0 || expansions > 2047)
                                {
                                    Console.WriteLine("Error: <expansions> must be a number between 0 and 2047!");
                                    break;
                                }

                                int gm = 0;
                                try
                                {
                                    gm = int.Parse(parts[5]);
                                }
                                catch
                                {
                                    Console.WriteLine("Error: <GM Level> must be number (duh!)");
                                    break;
                                }

                                string email = parts[6];
                                if (email == null)
                                {
                                    email = string.Empty;
                                }

                                if (!TestEmailRegex.TestEmail(email))
                                {
                                    Console.WriteLine(
                                        "Error: <Email> You must supply an email address for this account");
                                    break;
                                }

                                string firstname = parts[7];
                                try
                                {
                                    if (firstname == null)
                                    {
                                        throw new ArgumentNullException();
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Error: <FirstName> You must supply a first name for this accout");
                                    break;
                                }

                                string lastname = parts[8];
                                try
                                {
                                    if (lastname == null)
                                    {
                                        throw new ArgumentNullException();
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Error: <LastName> You must supply a last name for this account");
                                    break;
                                }

                                DBLoginData login = new DBLoginData
                                                    {
                                                        Username = username,
                                                        AccountFlags = 0,
                                                        Allowed_Characters = numChars,
                                                        CreationDate = DateTime.Now,
                                                        Email = email,
                                                        Expansions = expansions,
                                                        FirstName = firstname,
                                                        LastName = lastname,
                                                        GM = gm,
                                                        Flags = 0,
                                                        Password =
                                                            new LoginEncryption().GeneratePasswordHash(
                                                                password)
                                                    };
                                try
                                {
                                    LoginDataDao.WriteLoginData(login);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(
                                        "An error occured while trying to add a new user account:" + Environment.NewLine
                                        + "{0}",
                                        ex.Message);
                                    break;
                                }

                                Console.WriteLine("User added successfully.");
                                break;
                            }

                        // This function just hashes the string you enter using the loginencryption method
                        if (consoleCommand.ToLower().StartsWith("hash"))
                        {
                            string Syntax =
                                "The Syntax for this command is \"hash <String to hash>\" alphanumeric no spaces";
                            string[] parts = consoleCommand.Split(' ');
                            if (parts.Length != 2)
                            {
                                Console.WriteLine(Syntax);
                                break;
                            }

                            string pass = parts[1];
                            var le = new LoginEncryption();
                            string hashed = le.GeneratePasswordHash(pass);
                            Console.WriteLine(hashed);
                            break;
                        }

                        // sets the password for the given username
                        // Added by Andyzweb
                        // Still TODO add exception and error handling
                        if (consoleCommand.ToLower().StartsWith("setpass"))
                        {
                            string Syntax =
                                "The syntax for this command is \"setpass <account username> <newpass>\" where newpass is alpha numeric no spaces";
                            string[] parts = consoleCommand.Split(' ');
                            if (parts.Length != 3)
                            {
                                Console.WriteLine(Syntax);
                                break;
                            }

                            string username = parts[1];
                            string newpass = parts[2];
                            var le = new LoginEncryption();
                            string hashed = le.GeneratePasswordHash(newpass);

                            try
                            {
                                LoginDataDao.WriteNewPassword(
                                    new DBLoginData { Username = username, Password = hashed });
                            }

                                // yeah this part here, some kind of exception handling for mysql errors
                            catch (Exception ex)
                            {
                                Console.WriteLine("Could not set new Password" + Environment.NewLine + ex.Message);
                                LogUtil.ErrorException(ex);
                            }
                        }

                        ct.TextRead("login_consolecmdsdefault.txt");
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool Initialize()
        {
            loginServer = Container.GetInstance<LoginServer>();

            int Port = Convert.ToInt32(Config.Instance.CurrentConfig.LoginPort);
            try
            {
                if (Config.Instance.CurrentConfig.ListenIP == "0.0.0.0")
                {
                    loginServer.TcpEndPoint = new IPEndPoint(IPAddress.Any, Port);
                }
                else
                {
                    loginServer.TcpIP = IPAddress.Parse(Config.Instance.CurrentConfig.ListenIP);
                }

                loginServer.MaximumPendingConnections = 100;
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
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Error occured while initalizing NLog/NBug");
                Console.WriteLine(e.Message);
                Colouring.Pop();
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

            OnScreenBanner.PrintCellAOBanner(ConsoleColor.Red);

            Console.WriteLine();

            ct.TextRead("main.txt");

            if (!InitializeLogAndBug())
            {
                return;
            }

            if (!Initialize())
            {
                return;
            }

            CommandLoop(args);

            // NLog<->Mono lockup fix
            LogManager.Configuration = null;
        }

        #endregion
    }
}