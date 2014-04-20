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
    using System.Diagnostics;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Functions;
    using CellAO.Core.Network;
    using CellAO.Core.Vector;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    using ZoneEngine.Core.Functions;
    using ZoneEngine.Core.MessageHandlers;

    using Quaternion = CellAO.Core.Vector.Quaternion;

    #endregion

    /// <summary>
    /// </summary>
    internal class NPCController : IController
    {
        /// <summary>
        /// </summary>
        public ICharacter Character { get; set; }

        // Always null here
        public IZoneClient Client
        {
            get
            {
                return null;
            }
            set { throw new Exception("NPC's dont have a client. Faulty code tries to use it!!"); }
        }

        ~NPCController()

        {
            LogUtil.Debug("NPC Controller finished");
            LogUtil.Debug(new StackTrace().ToString());
        }

        public void Dispose()
        {
            if (this.Client != null)
            {
                this.Client = null;
            }
            // this.Character.Dispose();
        }

        public bool LookAt(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool UseStatel(Identity identity)
        {
            throw new NotImplementedException();
        }

        public void SendChatText(string text)
        {
            throw new NotImplementedException();
        }

        public bool CastNano(int nanoId, Identity target)
        {
            throw new NotImplementedException();
        }

        public bool Search()
        {
            throw new NotImplementedException();
        }

        public bool Sneak()
        {
            throw new NotImplementedException();
        }

        public bool ChangeVisualFlag(int visualFlag)
        {
            throw new NotImplementedException();
        }

        public bool Move(int moveType, Coordinate newCoordinates, SmokeLounge.AOtomation.Messaging.GameData.Quaternion heading)
        {
            throw new NotImplementedException();
        }

        public bool Move(int moveType, Coordinate newCoordinates, Quaternion heading)
        {
            throw new NotImplementedException();
        }

        public bool ContainerAddItem(int sourceContainerType, int sourcePlacement, Identity target, int targetPlacement)
        {
            throw new NotImplementedException();
        }

        public bool Follow(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool Stand()
        {
            throw new NotImplementedException();
        }

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

        public bool Trade(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool UseItem(Identity itemPosition)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(int container, int slotNumber)
        {
            throw new NotImplementedException();
        }

        public bool SplitItemStack(Identity targetItem, int stackCount)
        {
            throw new NotImplementedException();
        }

        public bool JoinItemStack(Identity sourceItem, Identity targetItem)
        {
            throw new NotImplementedException();
        }

        public bool CombineItems(Identity sourceItem, Identity targetItem)
        {
            throw new NotImplementedException();
        }

        public bool TradeSkillSourceChanged(int inventoryPageId, int slotNumber)
        {
            throw new NotImplementedException();
        }

        public bool TradeSkillTargetChanged(int inventoryPageId, int slotNumber)
        {
            throw new NotImplementedException();
        }

        public bool TradeSkillBuildPressed(Identity targetItem)
        {
            throw new NotImplementedException();
        }

        public bool ChatCommand(string command, Identity target)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        public bool Login()
        {
            throw new NotImplementedException();
        }

        public bool StopLogout()
        {
            throw new NotImplementedException();
        }

        public bool GetTargetInfo(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool TeamInvite(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool TeamKickMember(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool TeamLeave()
        {
            throw new NotImplementedException();
        }

        public bool TransferTeamLeadership(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool TeamJoinRequest(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool TeamJoinReply(bool accept, Identity requester)
        {
            throw new NotImplementedException();
        }

        public bool TeamJoinAccepted(Identity newTeamMember)
        {
            throw new NotImplementedException();
        }

        public bool TeamJoinRejected(Identity rejectingIdentity)
        {
            throw new NotImplementedException();
        }

        public void SendChangedStats()
        {
            Dictionary<int, uint> toPlayfield = new Dictionary<int, uint>();
            Dictionary<int, uint> toPlayer = new Dictionary<int, uint>();

            this.Character.Stats.GetChangedStats(toPlayer, toPlayfield);
            toPlayer.Clear();
            StatMessageHandler.Default.SendBulk(this.Character, toPlayer, toPlayfield);
        }

        public void CallFunction(Function function)
        {
            FunctionCollection.Instance.CallFunction(
                function.FunctionType,
                this.Character,
                this.Character,
                this.Character,
                function.Arguments.Values.ToArray());
        }
    }
}