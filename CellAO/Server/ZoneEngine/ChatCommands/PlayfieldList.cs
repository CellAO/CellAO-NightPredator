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

namespace ZoneEngine.ChatCommands
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public class PlayfieldList : AOChatCommand
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
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override void CommandHelp(ICharacter character)
        {
            // No help needed, no arguments can be given
            character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(character, "Lists all playfields and their id's"));
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
            Dictionary<Identity, string> list = ((Playfield)character.Playfield).ListAvailablePlayfields();
            var messList = new List<MessageBody>();
            foreach (KeyValuePair<Identity, string> pf in list)
            {
                messList.Add(
                    ChatTextMessageHandler.Default.Create(
                        character,
                        pf.Key.Instance.ToString().PadLeft(8) + ": " + pf.Value));
            }

            character.Playfield.Publish(Bulk.CreateIM(character.Controller.Client, messList.ToArray()));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GMLevelNeeded()
        {
            return 1;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override List<string> ListCommands()
        {
            var temp = new List<string> { "pflist", "playfieldlist" };
            return temp;
        }

        #endregion
    }
}