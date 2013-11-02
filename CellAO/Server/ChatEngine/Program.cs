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
// Last modified: 2013-11-03 00:29

#endregion

namespace ChatEngine
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Threading.Tasks;

    using CellAO.Core.Components;

    using ChatEngine.CoreServer;

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
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool Initialize()
        {
            try
            {
                chatServer = Container.GetInstance<ChatServer>();

                if (!InitializeLogAndBug())
                {
                    return false;
                }

                if (!InitializeTCP())
                {
                    return false;
                }
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
                SettingsOverride.LoadCustomSettings("NBug.ChatEngine.Config");
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
        private static bool InitializeTCP()
        {
            int Port = Convert.ToInt32(Config.Instance.CurrentConfig.ChatPort);
            try
            {
                if (Config.Instance.CurrentConfig.ISCommLocalIP == "0.0.0.0")
                {
                    chatServer.TcpEndPoint = new IPEndPoint(IPAddress.Any, Port);
                }
                else
                {
                    chatServer.TcpIP = IPAddress.Parse(Config.Instance.CurrentConfig.ListenIP);
                }

                chatServer.MaximumPendingConnections = 100;
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

            ct.TextRead("main.txt");

            if (!Initialize())
            {
                return;
            }
        }

        #endregion
    }
}