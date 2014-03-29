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

namespace ZoneEngine.Core.Controllers
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Core.Nanos;
    using CellAO.Core.Network;
    using CellAO.Core.Vector;
    using CellAO.Enums;
    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.MessageHandlers;

    using Quaternion = CellAO.Core.Vector.Quaternion;

    #endregion

    /// <summary>
    /// </summary>
    public class PlayerController : IController
    {        
        // All functions return true if reply should be sent, false if no reply needed
        
        /// <summary>
        /// </summary>
        private WeakReference<ICharacter> character;

        /// <summary>
        /// </summary>
        public ICharacter Character
        {
            get
            {
                return this.character.Target;
            }

            set
            {
                if (value == null)
                {
                    throw new Exception("Dont try to weak reference null");
                }

                this.character = new WeakReference<ICharacter>(value);
            }
        }

        #region Generic character actions

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool LookAt(Identity target)
        {
            this.Character.SetTarget(target);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="nanoId">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool CastNano(int nanoId, Identity target)
        {
            // Procedure:
            // 1. Check if nano can be casted (criteria to Use (3))
            // 2. Lock nanocasting ability
            // 3. Wait for cast attack delay
            // 4. Check target's restance to the nano
            // 5. Execute nanos gamefunctions
            // 6. Wait for nano recharge delay
            // 7. Unlock nano casting

            NanoFormula nano = NanoLoader.NanoList[nanoId];
            int strain = nano.NanoStrain();

            // ...

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Search()
        {
            // Procedure:
            // 1. Gather stealthed entities inside range
            // 2. Check against each entities concealment skill
            // 3. Unhide successful found entities
            // 4. Lock search action for ?? seconds

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Sneak()
        {
            // Procedure: 
            // 1. Gather surrounding mobs/players
            // 2. Check concealment against their perception skill
            // 3. Vanish for successful rolled chars/mobs

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="visualFlag">
        /// </param>
        /// <returns>
        /// </returns>
        public bool ChangeVisualFlag(int visualFlag)
        {
            // Procedure:
            // 1. Set visualFlags stat
            // 2. Send AppearanceUpdate
            this.Character.Stats[StatIds.visualflags].Value = visualFlag;

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="moveType">
        /// </param>
        /// <param name="newCoordinates">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Move(int moveType, Coordinate newCoordinates, Quaternion heading)
        {
            // Procedure:
            // 1. Check if new coordinates are plausible (in range of runspeed since last update)
            // 2. Set coordinates & heading

            // give it a bit uncertainty (2.0f)
            if (newCoordinates.Distance2D(this.Character.Coordinates) < 2.0f)
            {
                this.Character.SetCoordinates(newCoordinates, heading);
                this.Character.UpdateMoveType((byte)moveType);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceContainerType">
        /// </param>
        /// <param name="sourcePlacement">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="targetPlacement">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool ContainerAddItem(int sourceContainerType, int sourcePlacement, Identity target, int targetPlacement)
        {
            // Procedure:
            // 1. Check if source location has item
            // 2. Check if target container exists
            // 3. Switch source with target

            // Source container exists
            if (this.Character.BaseInventory.Pages.ContainsKey(sourceContainerType))
            {
                IInventoryPage sourcePage = this.Character.BaseInventory.Pages[sourceContainerType];

                // Source is not null
                if (sourcePage[sourcePlacement] != null)
                {
                    if (this.Character.Identity == target)
                    {
                        IInventoryPage targetPage = this.Character.BaseInventory.PageFromSlot(targetPlacement);
                        if (targetPage != null)
                        {
                            IItem itemSource = sourcePage.Remove(sourcePlacement);
                            IItem itemTarget = targetPage.Remove(targetPlacement);
                            if (itemTarget != null)
                            {
                                sourcePage.Add(sourcePlacement, itemTarget);
                            }

                            if (itemSource != null)
                            {
                                targetPage.Add(targetPlacement, itemSource);
                            }
                        }
                    }
                    else
                    {
                        // Put it into the other players/npcs trade window?
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Follow(Identity target)
        {
            // Procedure:
            // 1. Check if target is still ingame
            // 2. Find a path to target and head accordingly
            // 3. Start movement (if not already)
            // 4. Start Pathfinding loop

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Stand()
        {
            // Procedure:
            // 1. Update characters move mode
            // 2. Announce the action to the playfield (or range)
            // 3. If logout timer pending, cancel pending logout timer

            if (this.Character.InLogoutTimerPeriod())
            {
                this.Character.StopLogoutTimer();
            }

            this.Character.UpdateMoveType(37); // Magic number -> Stand
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="parameter1">
        /// </param>
        /// <param name="parameter2">
        /// </param>
        /// <param name="parameter3">
        /// </param>
        /// <param name="parameter4">
        /// </param>
        /// <param name="parameter5">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool SocialAction(
            SocialAction action, 
            byte parameter1, 
            byte parameter2, 
            byte parameter3, 
            byte parameter4, 
            int parameter5)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Trade(Identity target)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Player specific actions

        /// <summary>
        /// </summary>
        /// <param name="itemPosition">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool UseItem(Identity itemPosition)
        {
            // Procedure:
            // 1. Check if item exists at this position
            // 2. Check if item is usable and/or consumable
            // 2. Decrease stack if consumable
            // 3. Delete consumable item if stack==0
            // 4. Send the TemplateAction to client
            // 5. Execute the item's gamefunctions

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="container">
        /// </param>
        /// <param name="slotNumber">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool DeleteItem(int container, int slotNumber)
        {
            // Procedure:
            // 1. Check container id (only bags and main inventory are valid for deleting)
            // 2. Remove item from inventory/bag

            if (this.Character.BaseInventory.Pages.ContainsKey(container))
            {
                this.Character.BaseInventory.Pages[container].Remove(slotNumber);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="targetItem">
        /// </param>
        /// <param name="stackCount">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool SplitItemStack(Identity targetItem, int stackCount)
        {
            // Procedure:
            // 1. Check if Item exists
            // 2. Check if stackCount<item's stack - 1
            // 3. Create new item from old item with stack=stackCount
            // 4. Decrease old item's stack
            // 5. Add new item to inventory

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceItem">
        /// </param>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool JoinItemStack(Identity sourceItem, Identity targetItem)
        {
            // Procedure:
            // 1. Check if items are the same itemid's
            // 2. Add sourceItem stack to targetItem
            // 3. Delete sourceItem

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceItem">
        /// </param>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool CombineItems(Identity sourceItem, Identity targetItem)
        {
            // Procedure: 
            // See TradeSkillReceiver.TradeSkillBuildPressed

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="inventoryPageId">
        /// </param>
        /// <param name="slotNumber">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TradeSkillSourceChanged(int inventoryPageId, int slotNumber)
        {
            // Procedure see TradeSkillReceiver.TradeSkillSourceChanged

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="inventoryPageId">
        /// </param>
        /// <param name="slotNumber">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TradeSkillTargetChanged(int inventoryPageId, int slotNumber)
        {
            // Procedure see TradeSkillReceiver.TradeSkillTargetChanged

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TradeSkillBuildPressed(Identity targetItem)
        {
            // Procedure see TradeSkillReceiver.TradeSkillBuildPressed

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool ChatCommand(string command, Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Logout()
        {
            // Procedure: 
            // 1. Sit down (if not already)
            // 2. Check if we are a GM
            // 2.1. Save character and logout immediately
            // 3. Start logout timer
            // 4. Save character
            // 5. Logout

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Login()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool StopLogout()
        {
            // Procedure:
            // 1. Stop pending logout timer
            // 2. Go back to previous move mode (dunno if really needed)

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool GetTargetInfo(Identity target)
        {
            // Procedure:
            // 1. Gather data
            // 2. Send to client

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamInvite(Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamKickMember(Identity target)
        {
            // Procedure:
            // 1. Kick Team member
            // 2. Send Team update message

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamLeave()
        {
            // Procedure:
            // 1. Leave the team
            // 2. Send Team update message

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TransferTeamLeadership(Identity target)
        {
            // Procedure:
            // 1. Transfer Leadership
            // 2. Send Team update message

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamJoinRequest(Identity target)
        {
            // Procedure:
            // 1. Send target the invite

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="accept">
        /// </param>
        /// <param name="requester">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamJoinReply(bool accept, Identity requester)
        {
            // Procedure:
            // 1. If accept==true
            // 2.    Call requester's TeamJoinAccepted
            // 3. else
            // 4.    Call requester's TeamJoinRejected

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="newTeamMember">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamJoinAccepted(Identity newTeamMember)
        {
            // Procedure:
            // 1. If on team exists yet, create one
            // 2. Add yourself as TeamLeader
            // 3. Add newTeamMember
            // 4. Send out TeamMemberInfo etc. to all team members

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="rejectingIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool TeamJoinRejected(Identity rejectingIdentity)
        {
            // Procedure: 
            // 1. Send back negative reply

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        public void SendChangedStats(IZoneClient client)
        {

            Dictionary<int, uint> toPlayfield = new Dictionary<int, uint>();
            Dictionary<int, uint> toPlayer = new Dictionary<int, uint>();

            client.Controller.Character.Stats.GetChangedStats(toPlayer, toPlayfield);
            
            StatMessageHandler.Default.SendBulk(client.Controller.Character, toPlayer, toPlayfield);
        }

        #endregion


    }
}