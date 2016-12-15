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

    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;

    #endregion

    public class WalkingTest : AOChatCommand
    {
        public override bool CheckCommandArguments(string[] args)
        {
            return true;
        }

        public override void CommandHelp(ICharacter character)
        {
        }

        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            if (args[0].ToLower() == "walktest")
            {
                ICharacter npc = Pool.Instance.GetObject<ICharacter>(character.Playfield.Identity, character.SelectedTarget);
                if (npc != null)
                {
                    Vector3 newcoords = new Vector3();
                    newcoords.X = npc.RawCoordinates.X;
                    newcoords.Y = npc.RawCoordinates.Y;
                    newcoords.Z = npc.RawCoordinates.Z;
                    newcoords.X += 20;
                    npc.Controller.MoveTo(newcoords);
                }
            }
            if (args[0].ToLower() == "walkback")
            {
                ICharacter npc = Pool.Instance.GetObject<ICharacter>(character.Playfield.Identity, character.SelectedTarget);
                if (npc != null)
                {
                    Vector3 newcoords = new Vector3();
                    newcoords.X = npc.RawCoordinates.X;
                    newcoords.Y = npc.RawCoordinates.Y;
                    newcoords.Z = npc.RawCoordinates.Z;
                    newcoords.X -= 20;
                    npc.Controller.MoveTo(newcoords);
                }
            }
            if (args[0].ToLower() == "followtest")
            {
                ICharacter npc = Pool.Instance.GetObject<ICharacter>(character.Playfield.Identity, character.SelectedTarget);
                if (npc != null)
                {
                    npc.Controller.Follow(character.Identity);
                }
            }
            if (args[0].ToLower() == "showcoords")
            {
                ICharacter npc = Pool.Instance.GetObject<ICharacter>(character.Playfield.Identity, character.SelectedTarget);
                if (npc != null)
                {
                    character.Playfield.Publish(
                        ChatTextMessageHandler.Default.CreateIM(
                            character,
                            "Coordinates of " + character.SelectedTarget.ToString(true) + ": "
                            + npc.Coordinates().ToString()));
                    character.Playfield.Publish(
                        ChatTextMessageHandler.Default.CreateIM(
                            character,
                            "Heading of " + character.SelectedTarget.ToString(true) + ": " + npc.Heading.ToString()));
                }
            }
            if (args[0].ToLower() == "addwp")
            {
                CellAO.Core.Vector.Vector3 v = character.Coordinates().coordinate;
                bool running = character.MoveMode == MoveModes.Run;
                ICharacter npc = Pool.Instance.GetObject<ICharacter>(character.Playfield.Identity, character.SelectedTarget);
                if (npc != null)
                {
                    npc.AddWaypoint(v, running);
                    character.Playfield.Publish(
                        ChatTextMessageHandler.Default.CreateIM(
                            character,
                            "Waypoint added: " + character.SelectedTarget.ToString(true) + ": "
                            + character.Coordinates().ToString()));
                }
            }
        }

        public override int GMLevelNeeded()
        {
            return 0;
        }

        public override List<string> ListCommands()
        {
            return new List<string>(new[] { "walktest", "followtest", "walkback", "showcoords", "addwp" });
        }
    }
}