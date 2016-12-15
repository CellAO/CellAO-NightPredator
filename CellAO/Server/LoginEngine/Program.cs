#region License

// Copyright (c) 2005-2016, CellAO Team
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

namespace LoginEngine
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AO.Core.Encryption;

    using CellAO.Core.Components;
    using CellAO.Database.Dao;

    using locales;

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
        private static readonly IContainer Container = new MefContainer();

        /// <summary>
        /// </summary>
        private static readonly ServerConsoleCommands consoleCommands = new ServerConsoleCommands();

        /// <summary>
        /// </summary>
        private static ConsoleText ct;

        /// <summary>
        /// </summary>
        private static bool exited = false;

        /// <summary>
        /// </summary>
        private static LoginServer loginServer;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public static void StartTheServer()
        {
            loginServer.Start(true, false);
        }

        #endregion

        #region Methods

        private static bool CheckUsername(string username)
        {
            return !LoginDataDao.Instance.GetWhere(new { Username = username }).Any();
        }

        private static bool IsNumber(string number)
        {
            int temp = 0;
            return Int32.TryParse(number, out temp);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void AddUser(string[] obj)
        {
            if (obj.Length == 1)
            {
                List<string> temp = new List<string>();
                temp.Add("adduser");

                while (true)
                {
                    Console.Write("Username: ");
                    string test = Console.ReadLine();
                    if ((string.IsNullOrWhiteSpace(test)) || (test.Length < 6))
                    {
                        Console.WriteLine("Please enter a username (at least 6 chars)...");
                        continue;
                    }
                    if (CheckUsername(test))
                    {
                        temp.Add(test);
                        break;
                    }
                }

                while (true)
                {
                    Console.Write("Password: ");
                    string test = Console.ReadLine();
                    if ((string.IsNullOrWhiteSpace(test)) || (test.Length < 6))
                    {
                        Console.WriteLine("Please enter a password (at least 6 chars) for your safety...");
                        continue;
                    }
                    temp.Add(test);
                    break;
                }

                while (true)
                {
                    Console.WriteLine("Number of character slots: ");
                    string test = Console.ReadLine();
                    if (IsNumber(test))
                    {
                        temp.Add(test);
                        break;
                    }
                }

                Console.WriteLine("Expansions: Enter 2047 for all expansions (i know you want that)");
                while (true)
                {
                    Console.Write("Expansions: ");
                    string test = Console.ReadLine();
                    if (IsNumber(test))
                    {
                        temp.Add(test);
                        break;
                    }
                }

                Console.WriteLine(
                    "GM-Level: Anything above 0 is GM, but there are differences. Full Client GM = 1 (using keyboard shortcuts) but for some items you have to be GM Level 511");
                while (true)
                {
                    Console.Write("GM-Level: ");
                    string test = Console.ReadLine();
                    if (IsNumber(test))
                    {
                        temp.Add(test);
                        break;
                    }
                }

                while (true)
                {
                    Console.WriteLine("E-Mail: ");
                    string test = Console.ReadLine();
                    if (TestEmailRegex.TestEmail(test))
                    {
                        temp.Add(test);
                        break;
                    }
                }

                Console.Write("First name: ");
                temp.Add(Console.ReadLine());

                Console.Write("Last name: ");
                temp.Add(Console.ReadLine());

                obj = temp.ToArray();
            }

            Colouring.Push(ConsoleColor.Red);
            bool argsOk = CheckAddUserParameters(obj);
            Colouring.Pop();

            if (!argsOk)
            {
                return;
            }

            DBLoginData login = new DBLoginData
                                {
                                    Username = obj[1],
                                    AccountFlags = 0,
                                    AllowedCharacters = int.Parse(obj[3]),
                                    CreationDate = DateTime.Now,
                                    Email = obj[6],
                                    Expansions = int.Parse(obj[4]),
                                    FirstName = obj[7],
                                    LastName = obj[8],
                                    GM = int.Parse(obj[5]),
                                    Flags = 0,
                                    Password = new LoginEncryption().GeneratePasswordHash(obj[2])
                                };

            try
            {
                LoginDataDao.WriteLoginData(login);
            }
            catch (Exception ex)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(
                    "An error occured while trying to add a new user account:" + Environment.NewLine + "{0}",
                    ex.Message);
                Colouring.Pop();
                return;
            }

            Colouring.Push(ConsoleColor.Green);
            Console.WriteLine("User added successfully.");
            Colouring.Pop();
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool CheckAddUserParameters(string[] obj)
        {
            if (obj.Length != 9)
            {
                Console.WriteLine(
                    "Wrong number of parameters supplied. <username> <password> <number of characters> <expansions> <gm level> <email> <first name> <last name>");
                return false;
            }

            bool failed = false;
            int temp = 0;
            if (!int.TryParse(obj[3], out temp))
            {
                Console.WriteLine("Error: <number of characters> must be a number (duh!)");
                failed = true;
            }

            if (!int.TryParse(obj[4], out temp))
            {
                Console.WriteLine("Error: <expansions> must be a number between 0 and 2047!");
                failed = true;
            }
            else
            {
                if ((temp < 0) || (temp > 2047))
                {
                    Console.WriteLine("Error: <expansions> must be a number between 0 and 2047!");
                    failed = true;
                }
            }

            if (!int.TryParse(obj[5], out temp))
            {
                Console.WriteLine("Error: <GM Level> must be number between 0 and 511");
                failed = true;
            }
            else
            {
                if ((temp < 0) || (temp > 511))
                {
                    Console.WriteLine("Error: <GM Level> must be number between 0 and 511");
                    failed = true;
                }
            }

            if (!TestEmailRegex.TestEmail(obj[6]))
            {
                Console.WriteLine("Error: <Email> You must supply an valid email address for this account");
                failed = true;
            }

            return !failed;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool CheckDatabase()
        {
            bool result = true;
            try
            {
                LoginDataDao.Instance.GetAll();
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
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
        /// <returns>
        /// </returns>
        private static bool Initialize()
        {
            Console.WriteLine();
            Colouring.Push(ConsoleColor.Green);

            if (!InitializeLogAndBug())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingNLogNBug);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!InitializeServerInstance())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingEngine);
                Colouring.Pop();
                Colouring.Pop();
                return false;
            }

            if (!CheckDatabase())
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ErrorInitializingDatabase);
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

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeConsoleCommands()
        {
            consoleCommands.Engine = "Login";

            consoleCommands.AddEntry("start", StartServer);
            consoleCommands.AddEntry("running", IsServerRunning);
            consoleCommands.AddEntry("stop", StopServer);
            consoleCommands.AddEntry("exit", ShutDownServer);
            consoleCommands.AddEntry("quit", ShutDownServer);
            consoleCommands.AddEntry("debug", SetDebug);
            consoleCommands.AddEntry("adduser", AddUser);
            consoleCommands.AddEntry("hash", SetHash);
            consoleCommands.AddEntry("setgm", SetGMLevel);
            consoleCommands.AddEntry("logoffchars", LogoffCharacters);
            consoleCommands.AddEntry("setpass", SetPassword);
            return true;
        }

        private static void SetDebug(string[] obj)
        {
            if (obj.Length == 1)
            {
                LogUtil.Toggle("");
            }
            else
            {
                for (int i = 1; i < obj.Length; i++)
                {
                    LogUtil.Toggle(obj[i]);
                }
            }
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
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool InitializeServerInstance()
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
                    loginServer.TcpEndPoint = new IPEndPoint(IPAddress.Parse(Config.Instance.CurrentConfig.ListenIP), Port);
                }

                loginServer.MaximumPendingConnections = 100;
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
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void IsServerRunning(string[] obj)
        {
            Colouring.Push(ConsoleColor.White);
            if (loginServer.IsRunning)
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
        /// <param name="obj">
        /// </param>
        private static void LogoffCharacters(string[] obj)
        {
            if (obj.Length != 2)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Syntax: logoffchars <username>");
                Colouring.Pop();
            }
            else
            {
                LoginDataDao.Instance.LogoffChars(obj[1]);
            }
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
        /// <param name="obj">
        /// </param>
        private static void SetGMLevel(string[] obj)
        {
            int gmlevel = 0;
            if ((obj.Length != 3) || (!int.TryParse(obj[2], out gmlevel)))
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Syntax: setgm <username> <gmlevel>");
                Console.WriteLine("gmlevel range: 0 - 511");
                Colouring.Pop();
            }
            else
            {
                LoginDataDao.SetGM(obj[1], gmlevel);
                Colouring.Push(ConsoleColor.Green);
                Console.WriteLine("Successfully set GM Level " + gmlevel + " to account " + obj[1]);
                Colouring.Pop();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void SetHash(string[] obj)
        {
            Colouring.Push(ConsoleColor.Red);

            if (obj.Length != 2)
            {
                Console.WriteLine("The Syntax for this command is \"hash <String to hash>\" alphanumeric no spaces");
                Colouring.Pop();
                return;
            }

            string pass = obj[1];
            var le = new LoginEncryption();
            string hashed = le.GeneratePasswordHash(pass);
            Colouring.Pop();
            Console.Write("The Hash for password '");
            Colouring.Push(ConsoleColor.Green);
            Console.Write(obj[1]);
            Colouring.Pop();
            Console.Write("' is '");
            Colouring.Push(ConsoleColor.Green);
            Console.Write(hashed);
            Colouring.Pop();
            Console.WriteLine("'");
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void SetPassword(string[] obj)
        {
            string Syntax =
                "The syntax for this command is \"setpass <account username> <newpass>\" where newpass is alpha numeric no spaces";
            if (obj.Length != 3)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(Syntax);
                Colouring.Pop();
            }
            else
            {
                string username = obj[1];
                string newpass = obj[2];
                var le = new LoginEncryption();
                string hashed = le.GeneratePasswordHash(newpass);
                int affected =
                    LoginDataDao.WriteNewPassword(new DBLoginData() { Username = username, Password = hashed });
                if (affected == 0)
                {
                    Colouring.Push(ConsoleColor.Red);
                    Console.WriteLine("Could not set new password. Maybe username is wrong?");
                    Colouring.Pop();
                }
                else
                {
                    Colouring.Push(ConsoleColor.Green);
                    Console.WriteLine("New password is set.");
                    Colouring.Pop();
                }
            }
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
        /// <param name="obj">
        /// </param>
        private static void ShutDownServer(string[] obj)
        {
            if (loginServer.IsRunning)
            {
                loginServer.Stop();
            }

            exited = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="parts">
        /// </param>
        private static void StartServer(string[] parts)
        {
            if (loginServer.IsRunning)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ServerConsoleServerIsRunning);
                Colouring.Pop();
            }
            else
            {
                // TODO: Add Sql Check.
                StartTheServer();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void StopServer(string[] obj)
        {
            if (!loginServer.IsRunning)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine(locales.ServerConsoleServerIsNotRunning);
                Colouring.Pop();
            }
            else
            {
                loginServer.Stop();
            }
        }

        #endregion
    }
}