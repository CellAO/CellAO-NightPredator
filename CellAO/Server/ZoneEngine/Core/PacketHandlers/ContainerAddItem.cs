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
             * DEAD Trade Window (incoming)
             */
            var fromContainerID = (int)message.SourceContainer.Type;
            int fromPlacement = message.SourceContainer.Instance;
            Identity toIdentity = message.Target;
            int toPlacement = message.TargetPlacement;

            // Where and what does need to be moved/added?
            IInventoryPage sendingPage = cli.Character.BaseInventory.Pages[fromContainerID];
            IItem itemFrom = sendingPage[fromPlacement];

            // Receiver of the item (IInstancedEntity, can be mostly all from NPC, Character or Bag, later even playfields)
            IItemContainer itemReceiver = cli.Playfield.FindByIdentity(toIdentity) as IItemContainer;
            if (itemReceiver == null)
            {
                throw new ArgumentOutOfRangeException(
                    "No Entity found: " + message.Target.Type.ToString() + ":" + message.Target.Instance);
            }

            // On which inventorypage should the item be added?
            IInventoryPage receivingPage = itemReceiver.BaseInventory.PageFromSlot(toPlacement);

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
            int delay = 0;

            if (itemFrom != null)
            {
                delay += itemFrom.GetAttribute(211);
            }

            if (itemTo != null)
            {
                delay += itemTo.GetAttribute(211);
            }

            if (delay == 0)
            {
                delay = 200;
            }

            int counter;

            cli.Character.DoNotDoTimers = true;
            IItemSlotHandler equipTo = receivingPage as IItemSlotHandler;
            IItemSlotHandler unequipFrom = sendingPage as IItemSlotHandler;

            if (equipTo != null)
            {
                if (itemTo != null)
                {
                    if (receivingPage.NeedsItemCheck)
                    {
                        Actions action = GetAction(receivingPage, itemFrom);

                        if (action.CheckRequirements(cli.Character))
                        {
                            UnEquip.Send(cli, receivingPage, toPlacement);
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
                    unequipFrom.Unequip(fromPlacement, receivingPage, toPlacement);
                    UnEquip.Send(cli, sendingPage, fromPlacement);
                }
            }

            /*
            switch (fromContainerID)
            {
                case 0x68:

                    // from Inventory
                    if (toPlacement <= 30)
                    {
                        // to Weaponspage or Armorpage
                        // TODO: Send some animation
                        if (itemTo != null)
                        {
                            cli.Character.UnequipItem(itemTo, cli.Character, false, fromPlacement);

                            // send interpolated item
                            Unequip.Send(cli, itemTo, InventoryPage(toPlacement), toPlacement);

                            // client takes care of hotswap
                            cli.Character.EquipItem(itemFrom, cli.Character, false, toPlacement);
                            Equip.Send(cli, itemFrom, InventoryPage(toPlacement), toPlacement);
                        }
                        else
                        {
                            cli.Character.EquipItem(itemFrom, cli.Character, false, toPlacement);
                            Equip.Send(cli, itemFrom, InventoryPage(toPlacement), toPlacement);
                        }
                    }
                    else
                    {
                        if (toPlacement < 46)
                        {
                            if (itemTo == null)
                            {
                                cli.Character.EquipItem(itemFrom, cli.Character, false, toPlacement);
                                Equip.Send(cli, itemFrom, InventoryPage(toPlacement), toPlacement);
                            }
                        }

                        // Equiping to social page
                        if ((toPlacement >= 49) && (toPlacement <= 63))
                        {
                            if (itemTo != null)
                            {
                                cli.Character.UnequipItem(itemTo, cli.Character, true, fromPlacement);

                                // send interpolated item
                                cli.Character.EquipItem(itemFrom, cli.Character, true, toPlacement);
                            }
                            else
                            {
                                cli.Character.EquipItem(itemFrom, cli.Character, true, toPlacement);
                            }

                            // cli.Character.switchItems(cli, fromplacement, toplacement);
                        }
                    }

                    cli.Character.SwitchItems(fromPlacement, toPlacement);
                    cli.Character.CalculateSkills();
                    noAppearanceUpdate = false;
                    break;
                case 0x66:

                    // from Armorpage
                    cli.Character.UnequipItem(itemFrom, cli.Character, false, fromPlacement);

                    // send interpolated item
                    Unequip.Send(cli, itemFrom, InventoryPage(fromPlacement), fromPlacement);
                    cli.Character.SwitchItems(fromPlacement, toPlacement);
                    cli.Character.CalculateSkills();
                    noAppearanceUpdate = false;
                    break;
                case 0x65:

                    // from Weaponspage
                    cli.Character.UnequipItem(itemFrom, cli.Character, false, fromPlacement);

                    // send interpolated item
                    Unequip.Send(cli, itemFrom, InventoryPage(fromPlacement), fromPlacement);
                    cli.Character.SwitchItems(fromPlacement, toPlacement);
                    cli.Character.CalculateSkills();
                    noAppearanceUpdate = false;
                    break;
                case 0x67:

                    // from Implantpage
                    cli.Character.UnequipItem(itemFrom, cli.Character, false, fromPlacement);

                    // send interpolated item
                    Unequip.Send(cli, itemFrom, InventoryPage(fromPlacement), fromPlacement);
                    cli.Character.SwitchItems(fromPlacement, toPlacement);
                    cli.Character.CalculateSkills();
                    noAppearanceUpdate = true;
                    break;
                case 0x73:
                    cli.Character.UnequipItem(itemFrom, cli.Character, true, fromPlacement);

                    cli.Character.SwitchItems(fromPlacement, toPlacement);
                    cli.Character.CalculateSkills();
                    break;
                case 0x69:
                    cli.Character.TransferItemfromBank(fromPlacement, toPlacement);
                    toPlacement = 0x6f; // setting back to 0x6f for packet reply
                    noAppearanceUpdate = true;
                    break;
                case 0x6c:

                    // KnuBot Trade Window
                    cli.Character.TransferItemfromKnuBotTrade(fromPlacement, toPlacement);
                    break;
                default:
                    break;
            }
        }*/

            cli.Character.DoNotDoTimers = false;
            if ((equipTo != null) || (unequipFrom != null))
            {
                // Equipmentpages need delays
                // Delay when equipping/unequipping
                // has to be redone, jumping breaks the equiping/unequiping 
                // and other messages have to be done too
                // like heartbeat timer, damage from environment and such
                Thread.Sleep(delay);
            }
            else
            {
                Thread.Sleep(200); // social has to wait for 0.2 secs too (for helmet update)
            }

            /*
            SwitchItem.Send(
                cli,
                fromContainerID,
                fromPlacement,
                new Identity { Type = toIdentity.Type, Instance = toIdentity.Instance },
                toPlacement);
             */
            cli.Character.Stats.ClearChangedFlags();
            if (!noAppearanceUpdate)
            {
                throw new NotImplementedException("TODO");

                // cli.Character.AppearanceUpdate();
            }

            itemFrom = null;
            itemTo = null;
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
                action = item.ItemActions.SingleOrDefault(x => x.ActionType == (int)ActionType.ToWear);
            }

            if (page is WeaponInventoryPage)
            {
                action = item.ItemActions.SingleOrDefault(x => x.ActionType == (int)ActionType.ToWield);
            }

            if (action == null)
            {
                throw new NotSupportedException(
                    "No suitable action found for equipping to this page: " + page.GetType());
            }

            return action;
        }

        /// <summary>
        /// </summary>
        /// <param name="placement">
        /// </param>
        /// <returns>
        /// </returns>
        public static int InventoryPage(int placement)
        {
            if (placement < 16)
            {
                return 0x65;
            }

            if (placement < 32)
            {
                return 0x66;
            }

            if (placement < 48)
            {
                return 0x67;
            }

            if (placement < 64)
            {
                return 0x73;
            }

            return -1;
        }

        #endregion
    }
}