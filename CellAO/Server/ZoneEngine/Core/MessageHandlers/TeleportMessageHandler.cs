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
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = CellAO.Core.Vector.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class TeleportMessageHandler : BaseMessageHandler<N3TeleportMessage, TeleportMessageHandler>
    {
        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        public void Send(ICharacter character, Vector3 destination, Quaternion heading, Identity playfield)
        {
            this.Send(character, this.NormalTeleport(character, destination, heading, playfield), false);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        /// <param name="playfieldInstance">
        /// </param>
        /// <param name="GS">
        /// </param>
        /// <param name="SG">
        /// </param>
        /// <param name="destinationidentity">
        /// </param>
        public void SendTeleportProxy(
            ICharacter character, 
            Vector3 destination, 
            Quaternion heading, 
            int playfield, 
            Identity playfieldInstance, 
            int GS, 
            int SG, 
            Identity destinationidentity)
        {
            this.Send(
                character, 
                this.ProxyTeleport(
                    character, 
                    destination, 
                    heading, 
                    playfield, 
                    playfieldInstance, 
                    GS, 
                    SG, 
                    destinationidentity), 
                false);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        /// <param name="playfieldInstance">
        /// </param>
        /// <param name="GS">
        /// </param>
        /// <param name="SG">
        /// </param>
        /// <param name="destinationidentity">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller ProxyTeleport(
            ICharacter character, 
            Vector3 destination, 
            Quaternion heading, 
            int playfield, 
            Identity playfieldInstance, 
            int GS, 
            int SG, 
            Identity destinationidentity)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Destination = new SmokeLounge.AOtomation.Messaging.GameData.Vector3()
                                {
                                    X = (float)destination.x, 
                                    Y = (float)destination.y, 
                                    Z = (float)destination.z
                                };
                x.Heading = new SmokeLounge.AOtomation.Messaging.GameData.Quaternion()
                            {
                                X = (float)heading.x, 
                                Y = (float)heading.y, 
                                Z = (float)heading.z, 
                                W = (float)heading.w
                            };
                x.Unknown1 = 0x61;
                x.Playfield = playfieldInstance;
                x.GameServerId = GS;
                x.SgId = SG;
                x.ChangePlayfield = ((playfield != character.Playfield.Identity.Instance)
                                     || (IdentityType.Playfield != character.Playfield.Identity.Type))
                    ? new Identity { Type = IdentityType.Playfield2, Instance = playfield }
                    : Identity.None;
                x.Playfield2 = destinationidentity;
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller NormalTeleport(
            ICharacter character, 
            Vector3 destination, 
            Quaternion heading, 
            Identity playfield)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Destination = new SmokeLounge.AOtomation.Messaging.GameData.Vector3()
                                {
                                    X = (float)destination.x, 
                                    Y = (float)destination.y, 
                                    Z = (float)destination.z
                                };
                x.Heading = new SmokeLounge.AOtomation.Messaging.GameData.Quaternion()
                            {
                                X = (float)heading.x, 
                                Y = (float)heading.x, 
                                Z = (float)heading.y, 
                                W = (float)heading.w
                            };
                x.Unknown1 = 0x61;
                x.Playfield = new Identity() { Type = IdentityType.Playfield1, Instance = playfield.Instance };
                x.ChangePlayfield = ((playfield.Instance != character.Playfield.Identity.Instance)
                                     || (playfield.Type != character.Playfield.Identity.Type))
                    ? new Identity { Type = IdentityType.Playfield2, Instance = playfield.Instance }
                    : Identity.None;
                x.Playfield2 = new Identity() { Type = IdentityType.Playfield3, Instance = playfield.Instance };
            };
        }
    }
}