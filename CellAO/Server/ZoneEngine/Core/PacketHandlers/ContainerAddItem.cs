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

namespace ZoneEngine.Core.PacketHandlers
{
    #region Usings ...

    using System;
    using System.Linq;
    using System.Threading;

    using CellAO.Core.Actions;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public static class ContainerAddItem
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="cli">
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void AddItemToContainer(ContainerAddItemMessage message, ZoneClient cli)
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
            var fromContainerID = (int)message.SourceContainer.Type;
            int fromPlacement = message.SourceContainer.Instance;
            Identity toIdentity = message.Target;
            int toPlacement = message.TargetPlacement;

            // Where and what does need to be moved/added?
            IInventoryPage sendingPage = cli.Character.BaseInventory.Pages[fromContainerID];
            IItem itemFrom = sendingPage[fromPlacement];

            // Receiver of the item (IInstancedEntity, can be mostly all from NPC, Character or Bag, later even playfields)
            // Turn 0xDEAD into C350 if instance is the same
            if (toIdentity.Type == IdentityType.IncomingTradeWindow)
            {
                toIdentity.Type = IdentityType.CanbeAffected;
            }

            IItemContainer itemReceiver = cli.Playfield.FindByIdentity(toIdentity) as IItemContainer;
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
            if ((receivingPage == null) || (itemReceiver.GetType() != cli.Character.GetType()))
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

            cli.Character.DoNotDoTimers = true;
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
                        Actions action = GetAction(sendingPage, itemFrom);

                        if (action.CheckRequirements(cli.Character))
                        {
                            UnEquip.Send(cli, receivingPage, toPlacement);
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

                            cli.Character.Send(message);
                            equipTo.HotSwap(sendingPage, fromPlacement, toPlacement);
                            Equip.Send(cli, receivingPage, toPlacement);
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

                        Actions action = GetAction(receivingPage, itemFrom);

                        if (action.CheckRequirements(cli.Character))
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
                                UnEquip.Send(cli, sendingPage, fromPlacement);
                            }

                            cli.Character.Send(message);
                            equipTo.Equip(sendingPage, fromPlacement, toPlacement);
                            Equip.Send(cli, receivingPage, toPlacement);
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

                    UnEquip.Send(cli, sendingPage, fromPlacement);
                    unequipFrom.Unequip(fromPlacement, receivingPage, toPlacement);
                    cli.Character.Send(message);
                }
                else
                {
                    // No equipment page involved, just send ContainerAddItemMessage back
                    message.TargetPlacement = receivingPage.FindFreeSlot();
                    IItem item = sendingPage.Remove(fromPlacement);
                    receivingPage.Add(message.TargetPlacement, item);
                    cli.Character.Send(message);
                }
            }

            cli.Character.DoNotDoTimers = false;

            cli.Character.Stats.ClearChangedFlags();

            // Apply item functions before sending the appearanceupdate message
            cli.Character.CalculateSkills();

            if (!noAppearanceUpdate)
            {
                AppearanceUpdateMessageHandler.Default.Send(cli.Character);
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
        /// <exception cref="NotSupportedException">
        /// </exception>
        public static Actions GetAction(IInventoryPage page, IItem item)
        {
            Actions action = null;

            // TODO: Add special check for social page
            if ((page is ArmorInventoryPage) || (page is ImplantInventoryPage))
            {
                action = item.ItemActions.SingleOrDefault(x => x.ActionType == ActionType.ToWear);
                if (action == null)
                {
                    return new Actions();
                }
            }

            if (page is WeaponInventoryPage)
            {
                action = item.ItemActions.SingleOrDefault(x => x.ActionType == ActionType.ToWield);
                if (action == null)
                {
                    return new Actions();
                }
            }

            if (page is PlayerInventoryPage)
            {
                // No checks needed for unequipping
                return new Actions();
            }

            if (page is SocialArmorInventoryPage)
            {
                // TODO: Check for side, sex, breed conditionals
                return new Actions();
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