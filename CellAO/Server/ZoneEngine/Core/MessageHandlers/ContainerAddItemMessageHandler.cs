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

    // TODO: Change Actions to something more suitable (maybe EntityAction?)
    using System;
    using System.Linq;
    using System.Threading;

    using CellAO.Core.Actions;
    using CellAO.Core.Components;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Core.Network;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.InboundOnly)]
    public class ContainerAddItemMessageHandler :
        BaseMessageHandler<ContainerAddItemMessage, ContainerAddItemMessageHandler>
    {
        #region Inbound

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        protected override void Read(ContainerAddItemMessage message, IZoneClient client)
        {
            bool noAppearanceUpdate = false;

            /* Container ID's:
             * 0065 Weaponpage
             * 0066 Armorpage
             * 0067 Implantpage
             * 0068 Inventory (places 64-93)
             * 0069 Bank
             * 006B Backpack
             * 006C KnuBot Trade Window
             * 006E Overflow window
             * 006F Trade Window
             * 0073 Socialpage
             * 0767 Shop Inventory
             * 0790 Playershop Inventory
             * DEAD Trade Window (incoming) It's bank now (when you put something into the bank)
             */

            IInventoryPage sendingPage =
                Pool.Instance.GetObject<IInventoryPage>(message.Identity,
                    new Identity()
                    {
                        Type = (IdentityType)message.Identity.Instance,
                        Instance = (int)message.SourceContainer.Type
                    });
            int fromPlacement = message.SourceContainer.Instance;
            Identity toIdentity = message.Target;
            int toPlacement = message.TargetPlacement;

            // Where and what does need to be moved/added?
            IItem itemFrom = sendingPage[fromPlacement];

            // Receiver of the item (IInstancedEntity, can be mostly all from NPC, Character or Bag, later even playfields)
            // Turn 0xDEAD into C350 if instance is the same
            if (toIdentity.Type == IdentityType.IncomingTradeWindow)
            {
                toIdentity.Type = IdentityType.CanbeAffected;
            }

            IItemContainer itemReceiver =
                client.Controller.Character.Playfield.FindByIdentity(toIdentity) as IItemContainer;
            if (itemReceiver == null)
            {
                throw new ArgumentOutOfRangeException(
                    "No Entity found: " + message.Target.Type.ToString() + ":" + message.Target.Instance);
            }

            // On which inventorypage should the item be added?
            IInventoryPage receivingPage;
            if ((toPlacement == 0x6f) && (message.Target.Type == IdentityType.IncomingTradeWindow))
            {
                receivingPage = itemReceiver.BaseInventory.Pages[(int)IdentityType.Bank];
            }
            else
            {
                receivingPage = itemReceiver.BaseInventory.PageFromSlot(toPlacement);
            }

            // Get standard page if toplacement cant be found (0x6F for next free slot)
            // TODO: If Entities are not the same (other player, bag etc) then always add to the standard page
            if ((receivingPage == null) || (itemReceiver.GetType() != client.Controller.Character.GetType()))
            {
                receivingPage = itemReceiver.BaseInventory.Pages[itemReceiver.BaseInventory.StandardPage];
            }

            if (receivingPage == null)
            {
                throw new ArgumentOutOfRangeException("No inventorypage found.");
            }

            if (toPlacement == 0x6f)
            {
                toPlacement = receivingPage.FindFreeSlot();
            }

            // Is there already a item?
            IItem itemTo;
            try
            {
                itemTo = receivingPage[toPlacement];
            }
            catch (Exception)
            {
                itemTo = null;
            }

            // Calculating delay for equip/unequip/switch gear
            int delay = 20;

            client.Controller.Character.DoNotDoTimers = true;
            IItemSlotHandler equipTo = receivingPage as IItemSlotHandler;
            IItemSlotHandler unequipFrom = sendingPage as IItemSlotHandler;

            noAppearanceUpdate =
                !((equipTo is WeaponInventoryPage) || (equipTo is ArmorInventoryPage)
                  || (equipTo is SocialArmorInventoryPage));
            noAppearanceUpdate &=
                !((unequipFrom is WeaponInventoryPage) || (unequipFrom is ArmorInventoryPage)
                  || (unequipFrom is SocialArmorInventoryPage));

            if (equipTo != null)
            {
                if (itemTo != null)
                {
                    if (receivingPage.NeedsItemCheck)
                    {
                        AOAction action = this.getAction(sendingPage, itemFrom);

                        if (action.CheckRequirements(client.Controller.Character))
                        {
                            UnEquip.Send(client, receivingPage, toPlacement);
                            if (!noAppearanceUpdate)
                            {
                                // Equipmentpages need delays
                                // Delay when equipping/unequipping
                                // has to be redone, jumping breaks the equiping/unequiping 
                                // and other messages have to be done too
                                // like heartbeat timer, damage from environment and such

                                delay = (itemFrom.GetAttribute(211) == 1234567890 ? 20 : itemFrom.GetAttribute(211))
                                        + (itemTo.GetAttribute(211) == 1234567890 ? 20 : itemTo.GetAttribute(211));
                            }

                            Thread.Sleep(delay * 10); // social has to wait for 0.2 secs too (for helmet update)

                            client.Controller.Character.Send(message);
                            equipTo.HotSwap(sendingPage, fromPlacement, toPlacement);
                            Equip.Send(client, receivingPage, toPlacement);
                        }
                    }
                }
                else
                {
                    if (receivingPage.NeedsItemCheck)
                    {
                        if (itemFrom == null)
                        {
                            throw new NullReferenceException("itemFrom can not be null, possible inventory error");
                        }

                        AOAction action = this.getAction(receivingPage, itemFrom);

                        if (action.CheckRequirements(client.Controller.Character))
                        {
                            if (!noAppearanceUpdate)
                            {
                                // Equipmentpages need delays
                                // Delay when equipping/unequipping
                                // has to be redone, jumping breaks the equiping/unequiping 
                                // and other messages have to be done too
                                // like heartbeat timer, damage from environment and such

                                delay = itemFrom.GetAttribute(211);
                                if ((equipTo is SocialArmorInventoryPage) || (delay == 1234567890))
                                {
                                    delay = 20;
                                }

                                Thread.Sleep(delay * 10);
                            }

                            if (sendingPage == receivingPage)
                            {
                                // Switch rings for example
                                UnEquip.Send(client, sendingPage, fromPlacement);
                            }

                            client.Controller.Character.Send(message);
                            equipTo.Equip(sendingPage, fromPlacement, toPlacement);
                            Equip.Send(client, receivingPage, toPlacement);
                        }
                    }
                }
            }
            else
            {
                if (unequipFrom != null)
                {
                    // Send to client first
                    if (!noAppearanceUpdate)
                    {
                        // Equipmentpages need delays
                        // Delay when equipping/unequipping
                        // has to be redone, jumping breaks the equiping/unequiping 
                        // and other messages have to be done too
                        // like heartbeat timer, damage from environment and such

                        delay = itemFrom.GetAttribute(211);
                        if ((unequipFrom is SocialArmorInventoryPage) || (delay == 1234567890))
                        {
                            delay = 20;
                        }

                        Thread.Sleep(delay * 10);
                    }

                    UnEquip.Send(client, sendingPage, fromPlacement);
                    unequipFrom.Unequip(fromPlacement, receivingPage, toPlacement);
                    client.Controller.Character.Send(message);
                }
                else
                {
                    // No equipment page involved, just send ContainerAddItemMessage back
                    message.TargetPlacement = receivingPage.FindFreeSlot();
                    IItem item = sendingPage.Remove(fromPlacement);
                    receivingPage.Add(message.TargetPlacement, item);
                    client.Controller.Character.Send(message);
                }
            }

            client.Controller.Character.DoNotDoTimers = false;

            client.Controller.Character.Stats.ClearChangedFlags();

            // Apply item functions before sending the appearanceupdate message
            client.Controller.Character.CalculateSkills();

            if (!noAppearanceUpdate)
            {
                AppearanceUpdateMessageHandler.Default.Send(client.Controller.Character);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="page">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        private AOAction getAction(IInventoryPage page, IItem item)
        {
            AOAction action = null;

            // TODO: Add special check for social page
            if ((page is ArmorInventoryPage) || (page is ImplantInventoryPage))
            {
                action = item.ItemActions.SingleOrDefault(x => x.ActionType == ActionType.ToWear);
                if (action == null)
                {
                    return new AOAction();
                }
            }

            if (page is WeaponInventoryPage)
            {
                action = item.ItemActions.SingleOrDefault(x => x.ActionType == ActionType.ToWield);
                if (action == null)
                {
                    return new AOAction();
                }
            }

            if (page is PlayerInventoryPage)
            {
                // No checks needed for unequipping
                return new AOAction();
            }

            if (page is SocialArmorInventoryPage)
            {
                // TODO: Check for side, sex, breed conditionals
                return new AOAction();
            }

            if (action == null)
            {
                throw new NotSupportedException(
                    "No suitable action found for equipping to this page: " + page.GetType());
            }

            return action;
        }

        #endregion
    }
}