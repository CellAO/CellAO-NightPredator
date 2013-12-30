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

namespace ZoneEngine.Core.Packets
{
    #region Usings ...

    using CellAO.Core.Entities;
    using CellAO.Core.Vector;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Quaternion = SmokeLounge.AOtomation.Messaging.GameData.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public static class Teleport
    {
        #region Public Methods and Operators

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
        public static N3TeleportMessage Create(
            ICharacter character, 
            Coordinate destination, 
            IQuaternion heading, 
            Identity playfield)
        {
            return new N3TeleportMessage()
                   {
                       Identity = character.Identity, 
                       Destination =
                           new Vector3()
                           {
                               X = destination.x, 
                               Y = destination.y, 
                               Z = destination.z
                           }, 
                       Heading =
                           new Quaternion()
                           {
                               X = heading.xf, 
                               Y = heading.yf, 
                               Z = heading.zf, 
                               W = heading.wf
                           }, 
                       Unknown1 = 0x61, 
                       Playfield =
                           new Identity()
                           {
                               Type = IdentityType.Playfield1, 
                               Instance = playfield.Instance
                           }, 
                       ChangePlayfield =
                           ((playfield.Instance != character.Playfield.Identity.Instance)
                            || (playfield.Type != character.Playfield.Identity.Type))
                               ? new Identity
                                 {
                                     Type = IdentityType.Playfield2, 
                                     Instance = playfield.Instance
                                 }
                               : Identity.None, 
                       Playfield2 =
                           new Identity
                           {
                               Type = IdentityType.Playfield3, 
                               Instance = playfield.Instance
                           }, 
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
        public static void Send(ICharacter character, Coordinate destination, IQuaternion heading, Identity playfield)
        {
            // This needs to be sent immediately!
            character.Client.SendCompressed(Create(character, destination, heading, playfield));
        }

        #endregion
    }
}