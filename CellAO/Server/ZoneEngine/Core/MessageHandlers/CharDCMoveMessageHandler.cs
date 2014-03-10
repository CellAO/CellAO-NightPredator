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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using CellAO.Core.Components;
    using CellAO.Core.Network;
    using CellAO.Core.Vector;

    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.InternalMessages;

    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public class CharDCMoveMessageHandler : BaseMessageHandler<CharDCMoveMessage, CharDCMoveMessageHandler>
    {
        /// <summary>
        /// </summary>
        public CharDCMoveMessageHandler()
        {
            // Only inbound? Maybe we need outbound too, because of Fear nanos - Algorithman
            this.Direction = MessageHandlerDirection.InboundOnly;
            this.UpdateCharacterStatsOnReceive = true;

        }

        #region Inbound

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        protected override void Read(CharDCMoveMessage message, IZoneClient client)
        {
            if (client.Character.DoNotDoTimers)
            {
                return;
            }
            byte moveType = message.MoveType;
            var heading = new Quaternion(message.Heading.X, message.Heading.Y, message.Heading.Z, message.Heading.W);
            Coordinate coordinates = new Coordinate(message.Coordinates);

            // TODO: Find out what these (tmpInt) are and name them
            int tmpInt1 = message.Unknown1;
            int tmpInt2 = message.Unknown2;
            int tmpInt3 = message.Unknown3;

            /*
            if (!client.Character.DoNotDoTimers)
            {
                var teleportPlayfield = WallCollision.WallCollisionCheck(
                    coordinates.x, coordinates.z, client.Character.PlayField);
                if (teleportPlayfield.ZoneToPlayfield >= 1)
                {
                    var coordHeading = WallCollision.GetCoord(
                        teleportPlayfield, coordinates.x, coordinates.z, coordinates);
                    if (teleportPlayfield.Flags != 1337 && client.Character.PlayField != 152
                        || Math.Abs(client.Character.Coordinates.y - teleportPlayfield.Y) <= 2
                        || teleportPlayfield.Flags == 1337
                        && Math.Abs(client.Character.Coordinates.y - teleportPlayfield.Y) <= 6)
                    {
                        client.Teleport(
                            coordHeading.Coordinates, coordHeading.Heading, teleportPlayfield.ZoneToPlayfield);
                        Program.zoneServer.Clients.Remove(client);
                    }

                    return;
                }

                if (client.Character.Stats.LastConcretePlayfieldInstance.Value != 0)
                {
                    var correspondingDoor = DoorHandler.DoorinRange(
                        client.Character.PlayField, client.Character.Coordinates, 1.0f);
                    if (correspondingDoor != null)
                    {
                        correspondingDoor = DoorHandler.FindCorrespondingDoor(correspondingDoor, client.Character);
                        client.Character.Stats.LastConcretePlayfieldInstance.Value = 0;
                        var aoc = correspondingDoor.Coordinates;
                        aoc.x += correspondingDoor.hX * 3;
                        aoc.y += correspondingDoor.hY * 3;
                        aoc.z += correspondingDoor.hZ * 3;
                        client.Teleport(aoc, client.Character.Heading, correspondingDoor.playfield);
                        Program.zoneServer.Clients.Remove(client);
                        return;
                    }
                }
            }
            */

            // Is this correct? Shouldnt the client input be compared to the prediction and then be overridden to prevent teleportation exploits? 
            // - Algorithman

            client.Character.RawCoordinates = coordinates.coordinate;
            client.Character.RawHeading = heading;
            client.Character.UpdateMoveType(moveType);

            /* Start NV Heading Testing Code
             * Yaw: 0 to 360 Degrees (North turning clockwise to a complete revolution)
             * Roll: Not sure, but is always 0 cause we can't roll in AO
             * Pitch: 90 to -90 Degrees (90 is nose in the air, 0 is level, -90 is nose to the ground)
             */
            /* Comment this line with a '//' to enable heading testing
            client.SendChatText("Raw Headings: X: " + client.Character.heading.x + " Y: " + client.Character.heading.y + " Z:" + client.Character.heading.z);
            
            client.SendChatText("Yaw:  " + Math.Round(180 * client.Character.heading.yaw / Math.PI) + " Degrees");
            client.SendChatText("Roll: " + Math.Round(180 * client.Character.heading.roll / Math.PI) + " Degrees");
            client.SendChatText("Pitch:   " + Math.Round(180 * client.Character.heading.pitch / Math.PI) + " Degrees");
            /* End NV Heading testing code */

            /* start of packet */
            var reply = new CharDCMoveMessage
                        {
                            Identity = client.Character.Identity, 
                            Unknown = 0x00, 
                            MoveType = moveType, 
                            Heading =
                                new SmokeLounge.AOtomation.Messaging.GameData.Quaternion
                                {
                                    X =
                                        heading
                                        .xf, 
                                    Y =
                                        heading
                                        .yf, 
                                    Z =
                                        heading
                                        .zf, 
                                    W =
                                        heading
                                        .wf
                                }, 
                            Coordinates =
                                new Vector3
                                {
                                    X = coordinates.x, 
                                    Y = coordinates.y, 
                                    Z = coordinates.z
                                }, 
                            Unknown1 = tmpInt1, 
                            Unknown2 = tmpInt2, 
                            Unknown3 = tmpInt3
                        };
            client.Character.Playfield.Publish(new IMSendAOtomationMessageToPlayfield { Body = reply });

            // TODO: rewrite statelscheck
            /*
            if (Statels.StatelppfonEnter.ContainsKey(client.Character.PlayField))
            {
                foreach (var s in Statels.StatelppfonEnter[client.Character.PlayField])
                {
                    if (s.onEnter(client))
                    {
                        return;
                    }

                    if (s.onTargetinVicinity(client))
                    {
                        return;
                    }
                }
            }
             */
        }

        #endregion
    }
}