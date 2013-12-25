#region License
// Copyright (c) 2005-2012, CellAO Team
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

#region Usings...
#endregion

namespace ZoneEngine.ChatCommands
{
    using System.Collections.Generic;
    using System.Linq;

    using AO.Core;

    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;
    using CellAO.Core.Requirements;
    using CellAO.Core.Statels;
    using CellAO.Core.Vector;
    using CellAO.Enums;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.Packets;
    using ZoneEngine.Core.Playfields;
    using ZoneEngine.Script;

    public class ChatCommandShowStatel : AOChatCommand
    {
        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            List<MessageBody> replies = new List<MessageBody>();
            string reply = "Looking up for statel in playfield " + character.Playfield.Identity.Instance;
            replies.Add(ChatText.Create(character, reply));
            StatelData o = null;
            if (!PlayfieldLoader.PFData.ContainsKey(character.Playfield.Identity.Instance))
            {
                reply = "Could not find data for playfield " + character.Playfield.Identity.Instance;
                replies.Add(ChatText.Create(character, reply));
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
                        ChatText.Create(character, "No statel on this playfield... Very odd, where exactly are you???"));
                }
                else
                {
                    replies.Add(
                        ChatText.Create(
                            character,
                            o.StatelIdentity.Type.ToString() + " " + ((int)o.StatelIdentity.Type).ToString("X8") + ":"
                            + o.StatelIdentity.Instance.ToString("X8")));
                    foreach (Events se in o.Events)
                    {
                        replies.Add(
                            ChatText.Create(
                                character,
                                "Event: " + se.EventType.ToString() + " # of Functions: "
                                + se.Functions.Count.ToString()));

                        foreach (Functions sf in se.Functions)
                        {
                            string Fargs = "";
                            foreach (MessagePackObject obj in sf.Arguments.Values)
                            {
                                if (Fargs.Length > 0)
                                {
                                    Fargs = Fargs + ", ";
                                }
                                Fargs = Fargs + obj.ToString();
                            }
                            replies.Add(
                                ChatText.Create(
                                    character,
                                    "    Fn: " + ((FunctionType)sf.FunctionType).ToString() + "("+sf.FunctionType.ToString()+ "), # of Args: "
                                    + sf.Arguments.Values.Count.ToString()));
                            replies.Add(ChatText.Create(character, "    Args: " + Fargs));

                            foreach (Requirements sfr in sf.Requirements)
                            {
                                string req;
                                req = "Attr: " + sfr.Statnumber.ToString() + " Value: " + sfr.Value.ToString()
                                      + " Target: " + sfr.Target.ToString() + " Op: " + sfr.Operator.ToString();
                                replies.Add(ChatText.Create(character, req));
                            }
                        }

                    }
                }
            }
            character.Playfield.Publish(Bulk.CreateIM(character.Client, replies.ToArray()));
        }

        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(ChatText.CreateIM(character, "Usage: /command showstatel"));
            return;
        }

        public override bool CheckCommandArguments(string[] args)
        {
            // Always true, only string arguments
            return true;
        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            List<string> temp = new List<string>();
            temp.Add("showstatel");
            return temp;
        }
    }
}
