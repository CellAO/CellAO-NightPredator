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

namespace ChatEngine
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Utility.Config;

    #endregion

    /// <summary>
    /// The chat log.
    /// </summary>
    internal struct ChatLog
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets CreationDate.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets Stream.
        /// </summary>
        public StreamWriter Stream { get; set; }

        #endregion
    }

    /// <summary>
    /// The chat logger.
    /// </summary>
    public static class ChatLogger
    {
        #region Static Fields

        /// <summary>
        /// Handles to log files for various channels.
        /// </summary>
        private static readonly Dictionary<string, ChatLog> LogFiles = new Dictionary<string, ChatLog>();

        /// <summary>
        /// The m_b enabled.
        /// </summary>
        private static readonly bool m_bEnabled;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ChatLogger"/> class.
        /// </summary>
        static ChatLogger()
        {
            if (ConfigReadWrite.Instance.CurrentConfig.LogChat)
            {
                m_bEnabled = true;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Write a string into LogFile
        /// </summary>
        /// <param name="channel">
        /// The channel.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="sender">
        /// The sender.
        /// </param>
        public static void WriteString(string channel, string data, string sender)
        {
            // Make sure the config has chat logging enabled.
            if (!m_bEnabled)
            {
                return;
            }

            /*
            //Check if this is the first entry for this channel.
            if (!m_LogFiles.ContainsKey(Channel))
            {
                LoadLog(Channel);
            }
            */

            // Check if it's a new day and we must close current log
            // for channel, and open a new one.
            if (CheckLogRefresh(channel))
            {
                LoadLog(channel);
            }

            LoadLog(channel);
            ChatLog logFile = LogFiles[channel];
            DateTime timestamp = DateTime.Now;
            StringBuilder logEntry = new StringBuilder();
            logEntry.Append("[");
            logEntry.Append(timestamp.ToString("hh:mm"));
            logEntry.Append("] ");
            logEntry.Append(sender);
            logEntry.Append(": ");
            logEntry.Append(data);

            logFile.Stream.WriteLine(logEntry.ToString());
            logFile.Stream.Close();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The check log refresh.
        /// </summary>
        /// <param name="Channel">
        /// The channel.
        /// </param>
        /// <returns>
        /// The check log refresh.
        /// </returns>
        private static bool CheckLogRefresh(string Channel)
        {
            if (LogFiles.ContainsKey(Channel))
            {
                if (DateTime.Now.Day != LogFiles[Channel].CreationDate.Day)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The get log string.
        /// </summary>
        /// <param name="Channel">
        /// The channel.
        /// </param>
        /// <param name="Time">
        /// The time.
        /// </param>
        /// <returns>
        /// The get log string.
        /// </returns>
        private static string GetLogString(string Channel, DateTime Time)
        {
            StringBuilder Sb = new StringBuilder();
            Sb.Append(@"Logs\");
            Sb.Append(Channel);
            Sb.Append("-");
            Sb.Append(Time.Year.ToString());
            Sb.Append("-");
            Sb.Append(Time.Month.ToString().PadLeft(2, '0'));
            Sb.Append("-");
            Sb.Append(Time.Day.ToString().PadLeft(2, '0'));
            Sb.Append(".log");

            if (!Directory.Exists("Logs"))
            {
                try
                {
                    Directory.CreateDirectory("Logs");
                }
                catch
                {
                }
            }

            return Sb.ToString();
        }

        /// <summary>
        /// The load log.
        /// </summary>
        /// <param name="Channel">
        /// The channel.
        /// </param>
        private static void LoadLog(string Channel)
        {
            if (LogFiles.ContainsKey(Channel))
            {
                LogFiles[Channel].Stream.Close();
            }

            DateTime newLogDate = DateTime.Now;
            ChatLog newLog = new ChatLog();
            newLog.CreationDate = newLogDate;
            newLog.Stream = null;
            string logFilename = GetLogString(Channel, newLogDate);
            if (!File.Exists(logFilename))
            {
                newLog.Stream = new StreamWriter(logFilename, false);
            }
            else
            {
                newLog.Stream = new StreamWriter(logFilename, true);
            }

            LogFiles[Channel] = newLog;
        }

        #endregion
    }
}