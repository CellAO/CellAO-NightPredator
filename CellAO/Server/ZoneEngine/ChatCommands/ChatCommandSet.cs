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

namespace ZoneEngine.ChatCommands
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public class ChatCommandSet : AOChatCommand
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool CheckCommandArguments(string[] args)
        {
            // Two different checks return true: <int> <uint> and <string> <uint>
            List<Type> check = new List<Type>();
            check.Add(typeof(int));
            check.Add(typeof(uint));
            bool check1 = CheckArgumentHelper(check, args);

            check.Clear();
            check.Add(typeof(string));
            check.Add(typeof(uint));
            check1 |= CheckArgumentHelper(check, args);
            return check1;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatText.CreateIM(character, "Syntax: /get <stat name|stat id> <new stat value>"));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="args">
        /// </param>
        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            // Fallback to self if no target is selected
            if (target.Instance == 0)
            {
                target.Type = character.Client.Character.Identity.Type;
                target.Instance = character.Client.Character.Identity.Instance;
            }

            int statId = 1234567890;

            if (!int.TryParse(args[1], out statId))
            {
                try
                {
                    statId = StatNamesDefaults.GetStatNumber(args[1].ToLower());
                }
                catch (Exception)
                {
                }
            }

            if (statId == 1234567890)
            {
                character.Playfield.Publish(ChatText.CreateIM(character, "Unknown Stat name " + args[1]));
                return;
            }

            uint statNewValue = 1234567890;
            try
            {
                statNewValue = (uint)int.Parse(args[2]);
            }
            catch
            {
                try
                {
                    // For values >= 2^31
                    statNewValue = uint.Parse(args[2]);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
            }

            IInstancedEntity tempch = character.Playfield.FindByIdentity(target);

            uint statOldValue;
            try
            {
                statOldValue = tempch.Stats[statId].BaseValue;
                tempch.Stats[statId].Value = (int)statNewValue;
            }
            catch
            {
                character.Playfield.Publish(ChatText.CreateIM(character, "Unknown StatId " + statId));
                return;
            }

            string name = string.Empty;
            if (tempch is INamedEntity)
            {
                name = ((INamedEntity)tempch).Name + " ";
            }

            string response = "Dynel " + name + "(" + target.Type + ":" + target.Instance + "): Stat "
                              + StatNamesDefaults.GetStatName(statId) + " (" + statId + ") =";
            response += " Old: " + statOldValue;
            response += " New: " + statNewValue;
            character.Playfield.Publish(ChatText.CreateIM(character, response));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GMLevelNeeded()
        {
            // Be a GM
            return 1;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override List<string> ListCommands()
        {
            List<string> temp = new List<string>();
            temp.Add("set");
            return temp;
        }

        #endregion
    }
}