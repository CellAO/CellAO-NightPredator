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

namespace LoginEngine
{
    #region Usings ...

    using System;
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

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        private static void AddUser(string[] obj)
        {
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

        /*       /// <summary>
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
                                       Console.WriteLine("Usage: setgm <username> <gmlevel>");
                                       Console.WriteLine("gmlevel range: 0 - 511");
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
                                                               AllowedCharacters = numChars,
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
               }*/

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
                    loginServer.TcpIP = IPAddress.Parse(Config.Instance.CurrentConfig.ListenIP);
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