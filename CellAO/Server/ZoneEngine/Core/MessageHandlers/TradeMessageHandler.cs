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

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Network;
    using CellAO.ObjectManager;

    using Dapper;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.All)]
    public class TradeMessageHandler : BaseMessageHandler<TradeMessage, TradeMessageHandler>
    {

        public TradeMessageHandler()
        {
            this.UpdateCharacterStatsOnReceive = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        protected override void Read(TradeMessage message, IZoneClient client)
        {
            Character target = Pool.Instance.GetObject<Character>(client.Controller.Character.Playfield.Identity, message.Target);
            if (target != null)
            {
                target.Controller.Trade(message.Identity);
                /*
            // /!\ THIS IS ONLY A TEST /!\
            // Right click on mob lets them walk away 20m
            // This does NOT work on all mobs tho. (Depends on character flags, vendors wont, like bartender. Leets will)
                InventoryUpdateMessageHandler.Default.Send(
                    client.Controller.Character,
                    target.BaseInventory.Pages[(int)IdentityType.ArmorPage]);
                Vector3 start = new Vector3();
                start.X = target.RawCoordinates.X + 20;
                start.Y = target.RawCoordinates.Y;
                start.Z = target.RawCoordinates.Z;
                FollowTargetMessageHandler.Default.Send(target, target.RawCoordinates, start);
             */
            }
            // TODO: Add shop code here
            // something like
            /*IShop shop = Pool.Instance.GetObject<IShop>(message.Target)
            {
                Identity tempbag = shop.Trade(client.Controller.Character, message.Action, message.Target, message.Container);
                this.Send(character, message.Target, tempbag);
            }*/
        }

        public void Send(ICharacter character, Identity targetIdentity, Identity containerIdentity)
        {
            this.Send(character, this.ShopTrade(character, targetIdentity, containerIdentity));
        }

        private MessageDataFiller ShopTrade(ICharacter character, Identity targetIdentity, Identity containerIdentity)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Container = containerIdentity;
                x.Target = targetIdentity;
                x.Unknown1 = 2;
                x.Action=TradeAction.None;
            };
        }

    }
}