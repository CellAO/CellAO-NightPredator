#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Vector;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.Playfields;

    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class PlayfieldAnarchyFMessageHandler :
        BaseMessageHandler<PlayfieldAnarchyFMessage, PlayfieldAnarchyFMessageHandler>
    {
        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public void Send(ICharacter character)
        {
            this.Send(character, Filler(character));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        private static MessageDataFiller Filler(ICharacter character)
        {
            return x =>
            {
                x.Identity = new Identity
                             {
                                 Type = IdentityType.Playfield2,
                                 Instance = character.Playfield.Identity.Instance
                             };
                Coordinate temp = character.Coordinates();
                x.CharacterCoordinates = new Vector3 { X = temp.x, Y = temp.y, Z = temp.z, };
                x.PlayfieldId1 = new Identity
                                 {
                                     Type = IdentityType.Playfield1,
                                     Instance = character.Playfield.Identity.Instance
                                 };
                x.PlayfieldId2 = new Identity
                                 {
                                     Type = IdentityType.Playfield2,
                                     Instance = character.Playfield.Identity.Instance
                                 };
                x.PlayfieldX = Playfields.GetPlayfieldX(character.Playfield.Identity.Instance);
                x.PlayfieldZ = Playfields.GetPlayfieldZ(character.Playfield.Identity.Instance);

                IEnumerable<Vendor> vendors = Pool.Instance.GetAll<Vendor>(
                    character.Playfield.Identity,
                    (int)IdentityType.VendingMachine);

                /*                if (vendors.Any())
                {
                    x.PlayfieldVendorInfo = new PlayfieldVendorInfo()
                                            {
                                                VendorCount = vendors.Count(),
                                                FirstVendorId =
                                                    vendors.ElementAt(0).Identity.Instance
                                            };
                }*/
            };

            // TODO: Add the VendorHandler again
            /* var vendorcount = VendorHandler.GetNumberofVendorsinPlayfield(client.Character.PlayField);
            if (vendorcount > 0)
            {
                var firstVendorId = VendorHandler.GetFirstVendor(client.Character.PlayField);
                message.PlayfieldVendorInfo = new PlayfieldVendorInfo
                                                  {
                                                      VendorCount = vendorcount, 
                                                      FirstVendorId = firstVendorId
                                                  };
            }
            */
        }

        #endregion
    }
}