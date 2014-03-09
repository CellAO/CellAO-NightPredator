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

    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Playfields;
    using CellAO.Core.Requirements;
    using CellAO.Core.Statels;
    using CellAO.Core.Vector;
    using CellAO.Enums;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Core.Packets;
    using ZoneEngine.Core.Playfields;

    #endregion

    /// <summary>
    /// </summary>
    public class ChatCommandShowStatel : AOChatCommand
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
            // Always true, only string arguments
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Usage: /command showstatel"));
            return;
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
            List<MessageBody> replies = new List<MessageBody>();
            string reply = "Looking up for statel in playfield " + character.Playfield.Identity.Instance;
            replies.Add(ChatTextMessageHandler.Default.Create(character, reply));
            StatelData o = null;
            if (!PlayfieldLoader.PFData.ContainsKey(character.Playfield.Identity.Instance))
            {
                reply = "Could not find data for playfield " + character.Playfield.Identity.Instance;
                replies.Add(ChatTextMessageHandler.Default.Create(character, reply));
            }
            else
            {
                PlayfieldData pfData = PlayfieldLoader.PFData[character.Playfield.Identity.Instance];
                foreach (StatelData s in pfData.Statels)
                {
                    if (o == null)
                    {
                        o = s;
                    }
                    else
                    {
                        if (Coordinate.Distance2D(character.Coordinates, s.Coord())
                            < Coordinate.Distance2D(character.Coordinates, o.Coord()))
                        {
                            o = s;
                        }
                    }
                }

                if (o == null)
                {
                    replies.Add(
                        ChatTextMessageHandler.Default.Create(character, "No statel on this playfield... Very odd, where exactly are you???"));
                }
                else
                {
                    replies.Add(
                        ChatTextMessageHandler.Default.Create(
                            character, 
                            o.StatelIdentity.Type.ToString() + " " + ((int)o.StatelIdentity.Type).ToString("X8") + ":"
                            + o.StatelIdentity.Instance.ToString("X8")));
                    replies.Add(ChatTextMessageHandler.Default.Create(character, "Item Template Id: " + o.TemplateId));
                    foreach (Event se in o.Events)
                    {
                        replies.Add(
                            ChatTextMessageHandler.Default.Create(
                                character, 
                                "Event: " + se.EventType.ToString() + " # of Functions: "
                                + se.Functions.Count.ToString()));

                        foreach (Function sf in se.Functions)
                        {
                            string Fargs = string.Empty;
                            foreach (MessagePackObject obj in sf.Arguments.Values)
                            {
                                if (Fargs.Length > 0)
                                {
                                    Fargs = Fargs + ", ";
                                }

                                Fargs = Fargs + obj.ToString();
                            }

                            replies.Add(
                                ChatTextMessageHandler.Default.Create(
                                    character, 
                                    "    Fn: " + ((FunctionType)sf.FunctionType).ToString() + "("
                                    + sf.FunctionType.ToString() + "), # of Args: "
                                    + sf.Arguments.Values.Count.ToString()));
                            replies.Add(ChatTextMessageHandler.Default.Create(character, "    Args: " + Fargs));

                            foreach (Requirement sfr in sf.Requirements)
                            {
                                string req;
                                req = "Attr: " + sfr.Statnumber.ToString() + " Value: " + sfr.Value.ToString()
                                      + " Target: " + sfr.Target.ToString() + " Op: " + sfr.Operator.ToString();
                                replies.Add(ChatTextMessageHandler.Default.Create(character, req));
                            }
                        }
                    }
                }
            }

            character.Playfield.Publish(Bulk.CreateIM(character.Client, replies.ToArray()));
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
            List<string> temp = new List<string>();
            temp.Add("showstatel");
            return temp;
        }

        #endregion
    }
}