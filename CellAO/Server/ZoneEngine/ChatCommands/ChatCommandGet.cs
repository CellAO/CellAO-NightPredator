#region License

// Copyright (c) 2005-2014, CellAO Team
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
    using CellAO.Core.Exceptions;
    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public class ChatCommandGet : AOChatCommand
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
            List<Type> check = new List<Type> { typeof(int) };
            bool check1 = CheckArgumentHelper(check, args);

            check.Clear();
            check.Add(typeof(string));

            check1 |= CheckArgumentHelper(check, args);
            return check1;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Syntax: /get <stat name|stat id>"));
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
                target = character.Identity;
            }

            if (target.Type != IdentityType.CanbeAffected)
            {
                character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Target must be player/monster/NPC"));
                return;
            }

            Dynel targetDynel = (Dynel)character.Playfield.FindByIdentity(target);
            if (targetDynel != null)
            {
                Character targetCharacter = (Character)targetDynel;

                // May be obsolete in the future, let it stay in comment yet
                // ch.CalculateSkills();  

                // Check for numbers too, not only for names
                int statId;
                if (!int.TryParse(args[1], out statId))
                {
                    try
                    {
                        statId = StatNamesDefaults.GetStatNumber(args[1]);
                    }
                    catch (Exception)
                    {
                        statId = 1234567890;
                    }
                }

                if (statId == 1234567890)
                {
                    character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Unknown Stat name " + args[1]));
                    return;
                }

                uint statValue;
                int effectiveValue;
                int trickle;
                int mod;
                int perc;
                try
                {
                    statValue = targetCharacter.Stats[statId].BaseValue;
                    effectiveValue = targetCharacter.Stats[statId].Value;
                    trickle = targetCharacter.Stats[statId].Trickle;
                    mod = targetCharacter.Stats[statId].Modifier;
                    perc = targetCharacter.Stats[statId].PercentageModifier;
                }
                catch (StatDoesNotExistException)
                {
                    character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Unknown Stat Id " + statId));
                    return;
                }

                string response = "Character " + targetCharacter.Name + " (" + targetCharacter.Identity.Instance
                                  + "): Stat " + StatNamesDefaults.GetStatName(statId) + " (" + statId + ") = "
                                  + statValue;

                if (statValue != targetCharacter.Stats[statId].Value)
                {
                    response += "\r\nEffective value Stat " + StatNamesDefaults.GetStatName(statId) + " (" + statId
                                + ") = " + effectiveValue;
                }

                response += "\r\nTrickle: " + trickle + " Modificator: " + mod + " Percentage: " + perc;
                character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, response));
            }
            else
            {
                // Shouldnt be happen again (fallback to self)
                character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Unable to find target."));
            }
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
            List<string> temp = new List<string> { "get" };
            return temp;
        }

        #endregion
    }
}