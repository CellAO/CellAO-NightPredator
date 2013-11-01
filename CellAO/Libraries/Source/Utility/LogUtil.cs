#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-11-01 16:46
// Created:       2013-11-01 16:29

#endregion

namespace Utility
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.Threading;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    #endregion

    public static class LogUtil
    {
        private static bool init;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static Action<Action<string>> SystemInfoLogger;

        public static event Action<string, Exception> ExceptionRaised;

        public static void SetupLogging()
        {
            if (init)
            {
                return;
            }

            init = true;
            LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget
                                    {
                                        Layout =
                                            "${processtime} [${level}] ${message} ${exception:format=tostring}"
                                    };
            config.AddTarget("console", consoleTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            LogManager.Configuration = config;
            LogManager.EnableLogging();
        }

        #region Exceptions

        public static void ErrorException(Exception e)
        {
            ErrorException(e, false);
        }

        public static void ErrorException(Exception e, bool addSystemInfo)
        {
            ErrorException(e, addSystemInfo, "");
        }

        public static void ErrorException(string msg, params object[] format)
        {
            ErrorException(false, msg, format);
        }

        public static void ErrorException(bool addSystemInfo, string msg, params object[] format)
        {
            LogException(log.Error, null, addSystemInfo, msg, format);
        }

        public static void ErrorException(Exception e, string msg, params object[] format)
        {
            ErrorException(e, true, msg, format);
        }

        public static void ErrorException(Exception e, bool addSystemInfo, string msg, params object[] format)
        {
            LogException(log.Error, e, addSystemInfo, msg, format);
        }

        public static void WarnException(Exception e)
        {
            WarnException(e, false);
        }

        public static void WarnException(Exception e, bool addSystemInfo)
        {
            WarnException(e, addSystemInfo, "");
        }

        public static void WarnException(string msg, params object[] format)
        {
            WarnException(false, msg, format);
        }

        public static void WarnException(bool addSystemInfo, string msg, params object[] format)
        {
            LogException(log.Warn, null, addSystemInfo, msg, format);
        }

        public static void WarnException(Exception e, string msg, params object[] format)
        {
            WarnException(e, true, msg, format);
        }

        public static void WarnException(Exception e, bool addSystemInfo, string msg, params object[] format)
        {
            LogException(log.Warn, e, addSystemInfo, msg, format);
        }

        public static void FatalException(Exception e, string msg, params object[] format)
        {
            FatalException(e, true, msg, format);
        }

        public static void FatalException(Exception e, bool addSystemInfo)
        {
            FatalException(e, addSystemInfo, "");
        }

        public static void FatalException(Exception e, bool addSystemInfo, string msg, params object[] format)
        {
            LogException(log.Fatal, e, addSystemInfo, msg, format);
        }

        public static void LogException(
            Action<string> logger, Exception e, bool addSystemInfo, string msg, params object[] format)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                msg = string.Format(msg, format);
                logger(msg);
            }

            if (e != null)
            {
                LogStacktrace(logger);
                logger("");
                logger(e.ToString());
            }

            if (addSystemInfo)
            {
                logger("");
                if (SystemInfoLogger != null)
                {
                    SystemInfoLogger(logger);
                }
                else
                {
                    LogSystemInfo(logger);
                }
            }

            Action<string, Exception> evt = ExceptionRaised;
            if (evt != null)
            {
                evt(msg, e);
            }
        }

        #endregion

        public static void LogStacktrace(Action<string> logger)
        {
            StackTrace stackTrace = new StackTrace(Thread.CurrentThread, true);
            string temp = "";
            foreach (StackFrame stackFrame in stackTrace.GetFrames())
            {
                temp += stackFrame.ToString().Trim() + "\r\n\t";
            }
            logger(temp);
        }

        private static void LogSystemInfo(Action<string> logger)
        {
            string title = "CellAO component";
#if DEBUG
            title += " - Debug";
#else
			title += " - Release";
#endif
            logger(title);
            logger(string.Format("OS: {0} - CLR: {1}", Environment.OSVersion, Environment.Version));
        }
    }
}

