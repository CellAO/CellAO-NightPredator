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

#region NameSpace

namespace ZoneEngine.Script
{
    #region Usings ...

    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core;

    #endregion

    #endregion

    #region Class AOChatCommand

    /// <summary>
    /// The Class in charge of printing information to our consoles
    /// To add a new chat command refer to the ones already inside Scripts/ChatCommands
    /// Important: Class names of chat command scripts have to be lowercase!
    /// </summary>
    public abstract class AOChatCommand
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="typeList">
        /// </param>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool CheckArgumentHelper(List<Type> typeList, string[] args)
        {
            // Return false if number of args dont match (first argument is Command, so it doesnt count)
            if (args.Length - 1 != typeList.Count)
            {
                return false;
            }

            bool argumentsok = true;
            for (int argcounter = 0; argcounter < typeList.Count; argcounter++)
            {
                if (typeList.ElementAt(argcounter).FullName == typeof(string).FullName)
                {
                    continue;
                }

                if (typeList.ElementAt(argcounter).FullName == typeof(int).FullName)
                {
                    int temp;
                    argumentsok &= int.TryParse(args[argcounter + 1], out temp);
                    continue;
                }

                if (typeList.ElementAt(argcounter).FullName == typeof(Int32).FullName)
                {
                    int temp;
                    argumentsok &= int.TryParse(args[argcounter + 1], out temp);
                    continue;
                }

                if (typeList.ElementAt(argcounter).FullName == typeof(bool).FullName)
                {
                    bool temp;
                    argumentsok &= bool.TryParse(args[argcounter + 1], out temp);
                    continue;
                }

                if (typeList.ElementAt(argcounter).FullName == typeof(uint).FullName)
                {
                    uint temp;
                    argumentsok &= uint.TryParse(args[argcounter + 1], out temp);
                    continue;
                }

                if (typeList.ElementAt(argcounter).FullName == typeof(float).FullName)
                {
                    float temp;
                    argumentsok &= float.TryParse(
                        args[argcounter + 1], 
                        NumberStyles.Any, 
                        CultureInfo.InvariantCulture, 
                        out temp);
                }
            }

            return argumentsok;
        }

        /// <summary>
        /// Checks the command Arguments
        /// </summary>
        /// <param name="args">
        /// True if command arguments are fine
        /// </param>
        /// <returns>
        /// </returns>
        public abstract bool CheckCommandArguments(string[] args);

        /// <summary>
        /// Returns Help for this command
        /// </summary>
        /// <param name="client">
        /// </param>
        public abstract void CommandHelp(ZoneClient client);

        /// <summary>
        /// Execute the chat command
        /// </summary>
        /// <param name="client">
        /// client
        /// </param>
        /// <param name="target">
        /// Target identity
        /// </param>
        /// <param name="args">
        /// command arguments
        /// </param>
        public abstract void ExecuteCommand(ZoneClient client, Identity target, string[] args);

        /// <summary>
        /// Returns the GM Level needed for this command
        /// </summary>
        /// <returns>GMLevel needed</returns>
        public abstract int GMLevelNeeded();

        /// <summary>
        /// Returns a list of commands handled by this class
        /// </summary>
        /// <returns>List of command strings</returns>
        public abstract List<string> ListCommands();

        #endregion
    }

    #endregion Class AOChatCommand
}

#endregion NameSpace