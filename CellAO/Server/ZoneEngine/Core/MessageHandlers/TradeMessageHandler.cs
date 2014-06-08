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

    using System;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Core.Network;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.Controllers;

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
            IItemContainer target;
            if ((message.Action != TradeAction.End) && (message.Action != TradeAction.Decline))
            {
                target = Pool.Instance.GetObject<IItemContainer>(
                    client.Controller.Character.Playfield.Identity,
                    message.Target);
            }
            switch (message.Action)
            {
                case TradeAction.None:
                {
                    ICharacter isnpc =
                        Pool.Instance.GetObject<ICharacter>(
                            client.Controller.Character.Playfield.Identity,
                            message.Target);
                    if (isnpc != null)
                    {
                        NPCController controller = isnpc.Controller as NPCController;
                        if (controller != null)
                        {
                            controller.Trade(message.Identity);
                        }
                    }
                    break;
                }
                case TradeAction.AddItem:
                {
                    if (client.Controller.Character.ShoppingBag == null)
                    {
                        throw new InvalidOperationException("Characters shopping bag is null?? huh");
                    }

                    IItemContainer issuer =
                        Pool.Instance.GetObject<IItemContainer>(
                            client.Controller.Character.Playfield.Identity,
                            message.Target);

                    if (issuer != null)
                    {
                        IItem item;

                        if (issuer is Vendor)
                        {
                            item = issuer.BaseInventory.GetItemInContainer(
                                (int)IdentityType.Inventory,
                                message.Container.Instance);
                        }
                        else
                        {
                            item = issuer.BaseInventory.GetItemInContainer(
                                (int)message.Container.Type,
                                message.Container.Instance);
                        }
                        if (item != null)
                        {
                            TemporaryBag shoppingBag = client.Controller.Character.ShoppingBag;
                            if (issuer is Vendor)
                            {
                                shoppingBag.Add(
                                    new Identity() { Instance = message.Container.Instance },
                                    issuer.BaseInventory.RemoveItem(
                                        (int)IdentityType.Inventory,
                                        message.Container.Instance));
                            }
                            else
                            {
                                shoppingBag.Add(
                                    message.Target,
                                    issuer.BaseInventory.RemoveItem(
                                        (int)message.Container.Type,
                                        message.Container.Instance));
                            }
                            this.AcknowledgeTradeAction(client.Controller.Character, message);
                        }
                    }
                    break;
                }
                case TradeAction.RemoveItem:
                {
                    if (client.Controller.Character.ShoppingBag == null)
                    {
                        throw new InvalidOperationException("Characters shopping bag is null?? huh");
                    }

                    IItemContainer issuer =
                        Pool.Instance.GetObject<IItemContainer>(
                            client.Controller.Character.Playfield.Identity,
                            message.Target);

                    if (issuer != null)
                    {
                        IItem item;

                        TemporaryBag shoppingBag = client.Controller.Character.ShoppingBag;
                        if (issuer is Vendor)
                        {
                            item = issuer.BaseInventory.GetItemInContainer(
                                (int)IdentityType.Inventory,
                                message.Container.Instance);
                        }
                        else
                        {
                            item = shoppingBag.GetSoldItems()[message.Container.Instance];
                        }
                        if (item != null)
                        {
                            if (issuer is Vendor)
                            {
                                shoppingBag.Remove(
                                    new Identity() { Instance = message.Container.Instance },
                                    message.Container.Instance);

                                this.Send(
                                    client.Controller.Character,
                                    this.AcknowledgeRemove(shoppingBag.Shopper, message));
                                this.Send(
                                    client.Controller.Character,
                                    this.AcknowledgeRemove(shoppingBag.Vendor, message));
                            }
                            else
                            {
                                IItem returnedItem = shoppingBag.Remove(message.Target, message.Container.Instance);
                                if (returnedItem != null)
                                {
                                    InventoryError err =
                                        client.Controller.Character.BaseInventory.AddToPage(
                                            client.Controller.Character.BaseInventory.StandardPage,
                                            client.Controller.Character.BaseInventory.Pages[
                                                client.Controller.Character.BaseInventory.StandardPage].FindFreeSlot(),
                                            returnedItem);

                                    if (err == InventoryError.OK)
                                    {
                                        ContainerAddItemMessageHandler.Default.Send(
                                            client.Controller.Character,
                                            new Identity()
                                            {
                                                Type = IdentityType.KnuBotTradeWindow,
                                                Instance = message.Container.Instance
                                            },
                                            0x6f); // 0x6f = Next free slot in main inventory
                                    }
                                    else
                                    {
                                        // Cant return item code here
                                    }

                                    InventoryUpdatedMessageHandler.Default.Send(
                                        client.Controller.Character,
                                        shoppingBag.Vendor);
                                    InventoryUpdatedMessageHandler.Default.Send(
                                        client.Controller.Character,
                                        client.Controller.Character.Identity);
                                }
                            }
                            this.AcknowledgeTradeAction(client.Controller.Character, message);
                        }
                    }
                    break;
                }
                case TradeAction.End:
                {
                    if (client.Controller.Character.ShoppingBag == null)
                    {
                        throw new InvalidOperationException("Characters shopping bag is null?? huh");
                    }

                    IItemContainer issuer = client.Controller.Character;

                    if (issuer != null)
                    {
                        TemporaryBag shoppingBag = client.Controller.Character.ShoppingBag;
                        if (shoppingBag != null)
                        {
                            IItem[] items = shoppingBag.GetBoughtItems();
                            double CLFactor = 1
                                              - (double)
                                                  (client.Controller.Character.Stats[StatIds.computerliteracy].Value
                                                   / 4000.0f);
                            int cash = 0;
                            foreach (IItem item in items)
                            {
                                int nextSlot = issuer.BaseInventory[issuer.BaseInventory.StandardPage].FindFreeSlot();
                                if (nextSlot != -1)
                                {
                                    issuer.BaseInventory[issuer.BaseInventory.StandardPage].Add(nextSlot, item);
                                    AddTemplateMessageHandler.Default.Send(client.Controller.Character, (Item)item);
                                    cash += (int)(CLFactor * item.GetAttribute(74));
                                }
                            }

                            items = shoppingBag.GetSoldItems();
                            CLFactor = 0.04
                                       + ((double)client.Controller.Character.Stats[StatIds.computerliteracy].Value
                                          / 100000.0f);
                            foreach (IItem item in items)
                            {
                                cash -= (int)(CLFactor * item.GetAttribute(74));
                            }

                            // TODO: DEDUCT CREDITS HERE!
                            client.Controller.Character.Stats[StatIds.cash].Value -= cash;

                            this.Send(
                                client.Controller.Character,
                                TradeAction.Unknown,
                                shoppingBag.Vendor,
                                shoppingBag.Vendor);
                            shoppingBag.Dispose();
                        }
                    }
                    break;
                }
                case TradeAction.Decline:
                {
                    IItemContainer issuer = client.Controller.Character;
                    TemporaryBag shoppingBag = client.Controller.Character.ShoppingBag;

                    if (shoppingBag != null)
                    {
                        IItem[] items = shoppingBag.GetSoldItems();
                        foreach (IItem item in items)
                        {
                            int nextSlot = issuer.BaseInventory[issuer.BaseInventory.StandardPage].FindFreeSlot();
                            if (nextSlot != -1)
                            {
                                issuer.BaseInventory[issuer.BaseInventory.StandardPage].Add(nextSlot, item);
                            }
                        }

                        this.Send(client.Controller.Character, TradeAction.Decline, Identity.None, Identity.None);
                        shoppingBag.Dispose();
                    }

                    break;
                }
            }
        }

        private MessageDataFiller AcknowledgeRemove(Identity identity, TradeMessage message)
        {
            return x =>
            {
                x.Identity = identity;
                x.Unknown = 0;
                x.Unknown1 = message.Unknown1;
                x.Action = message.Action;
                x.Target = message.Target;
                x.Container = message.Container;
            };
        }

        private void Send(ICharacter character, TradeAction tradeAction, Identity identity1, Identity identity2)
        {
            this.Send(character, this.EndTrade(character, tradeAction, identity1, identity2));
        }

        private MessageDataFiller EndTrade(
            ICharacter character,
            TradeAction tradeAction,
            Identity identity1,
            Identity identity2)
        {
            return x =>
            {
                x.Action = tradeAction;
                x.Container = identity2;
                x.Target = identity1;
                x.Identity = character.Identity;
                x.Unknown1 = 2;
            };
        }

        private void AcknowledgeTradeAction(ICharacter character, TradeMessage message)
        {
            this.Send(character, this.AcknowledgeFiller(message));
        }

        private MessageDataFiller AcknowledgeFiller(TradeMessage message)
        {
            return x =>
            {
                x.Target = message.Target;
                x.Action = message.Action;
                x.Container = message.Container;
                x.Unknown1 = message.Unknown1;
                x.Unknown = message.Unknown;
                x.Identity = message.Identity;
            };
        }

        public void Send(ICharacter character, TemporaryBag tempBag)
        {
            this.Send(character, this.TemporaryBagHandle(character, tempBag.Shopper, tempBag.Vendor, tempBag.Identity));
            this.Send(character, this.TemporaryBagHandle(character, tempBag.Vendor, tempBag.Shopper, tempBag.Identity));
        }

        private MessageDataFiller TemporaryBagHandle(
            ICharacter character,
            Identity identity1,
            Identity identity2,
            Identity bagIdentity)
        {
            return x =>
            {
                x.Identity = identity1;
                x.Unknown = 0;
                x.Unknown1 = 2;
                x.Action = TradeAction.None;
                x.Target = identity2;
                x.Container = bagIdentity;
            };
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
                x.Action = TradeAction.None;
            };
        }
    }
}