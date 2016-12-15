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

namespace Utility
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using locales;

    #endregion

    /// <summary>
    /// </summary>
    public class ServerConsoleCommands
    {
        #region Fields

        /// <summary>
        /// </summary>
        public string Engine = string.Empty;

        /// <summary>
        /// </summary>
        private readonly List<ServerConsoleCommandEntry> entries = new List<ServerConsoleCommandEntry>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="commandString">
        /// </param>
        /// <param name="command">
        /// </param>
        public void AddEntry(string commandString, Action<string[]> command)
        {
            this.entries.Add(new ServerConsoleCommandEntry() { Command = command, CommandString = commandString });
        }

        /// <summary>
        /// </summary>
        /// <param name="commandString">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Execute(string commandString)
        {
            // First lets remove double spaces
            while (commandString.IndexOf("  ") > -1)
            {
                commandString = commandString.Replace("  ", " ");
            }

            // Then leading or trailing spaces
            commandString = commandString.Trim();

            // Empty command string -> no command at all
            if (commandString == string.Empty)
            {
                return false;
            }

            string[] parts = commandString.Split(' ');
            if (this.entries.Any(x => x.CommandString == parts[0]))
            {
                this.entries.Single(x => x.CommandString == parts[0]).Command(parts);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string HelpAll()
        {
            int maxLength = this.MaxCommandLength();

            string output = string.Empty;

            foreach (ServerConsoleCommandEntry serverConsoleCommandEntry in this.entries)
            {
                output += output == string.Empty ? string.Empty : Environment.NewLine;
                output += serverConsoleCommandEntry.CommandString.PadRight(maxLength + 1)
                          + this.HelpText(serverConsoleCommandEntry.CommandString);
            }

            return output;
        }

        /// <summary>
        /// </summary>
        /// <param name="commandString">
        /// </param>
        /// <returns>
        /// </returns>
        public string HelpText(string commandString)
        {
            if (this.entries.Any(x => x.CommandString == commandString))
            {
                if (locales.ResourceManager.GetObject("ServerConsoleCommandHelp" + this.Engine + "_" + commandString)
                    != null)
                {
                    return
                        (string)
                            locales.ResourceManager.GetObject(
                                "ServerConsoleCommandHelp" + this.Engine + "_" + commandString);
                }

                // If no specific output found for this engine, then fall back to normal
                if (locales.ResourceManager.GetObject("ServerConsoleCommandHelp_" + commandString) != null)
                {
                    return (string)locales.ResourceManager.GetObject("ServerConsoleCommandHelp" + "_" + commandString);
                }
            }

            return "No help available for command '" + commandString + "'";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the length of the longest console command (for padding the output)
        /// </summary>
        /// <returns>
        /// Length of the longest console command
        /// </returns>
        private int MaxCommandLength()
        {
            if (this.entries.Count > 0)
            {
                return this.entries.Max(x => x.CommandString.Length);
            }
            return 0;
        }

        #endregion
    }
}