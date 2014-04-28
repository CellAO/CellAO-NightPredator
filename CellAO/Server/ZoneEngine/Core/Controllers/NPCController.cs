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

namespace ZoneEngine.Core.Controllers
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using CellAO.Core.Entities;
    using CellAO.Core.Functions;
    using CellAO.Core.Network;
    using CellAO.Core.Vector;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    using ZoneEngine.Core.Functions;
    using ZoneEngine.Core.MessageHandlers;

    using Quaternion = SmokeLounge.AOtomation.Messaging.GameData.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    internal class NPCController : IController
    {
        private double lastDistance = 0.0f;

        private Identity followIdentity = Identity.None;

        private CellAO.Core.Vector.Vector3 followCoordinates = new CellAO.Core.Vector.Vector3();

        public CharacterState State { get; private set; }

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
            set
            {
                throw new Exception("NPC's dont have a client. Faulty code tries to use it!!");
            }
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

        public bool Move(int moveType, Coordinate newCoordinates, CellAO.Core.Vector.Quaternion heading)
        {
            throw new NotImplementedException();
        }

        public bool ContainerAddItem(int sourceContainerType, int sourcePlacement, Identity target, int targetPlacement)
        {
            throw new NotImplementedException();
        }

        public bool Follow(Identity target)
        {
            this.followIdentity = target;
            return true;
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

        public void MoveTo(Vector3 destination)
        {
            FollowTargetMessageHandler.Default.Send(this.Character, this.Character.RawCoordinates, destination);
            CellAO.Core.Vector.Vector3 dest = destination;
            CellAO.Core.Vector.Vector3 start = this.Character.RawCoordinates;
            dest = start - dest;
            dest = dest.Normalize();
            this.Character.Heading =
                (CellAO.Core.Vector.Quaternion)this.Character.Heading.GenerateRotationFromDirectionVector(dest);
            this.Run();

            Coordinate c = new Coordinate(destination);
            bool arrived = false;
            double lastDistance = double.MaxValue;
            while (!arrived)
            {
                Coordinate temp = this.Character.Coordinates;
                double distance = this.Character.Coordinates.Distance2D(c);
                arrived = (distance < 0.2f) || (lastDistance < distance);
                lastDistance = distance;
                // LogUtil.Debug(DebugInfoDetail.Movement,"Moving...");
                Thread.Sleep(100);
            }
            this.StopMovement();
        }

        public void DoFollow()
        {
            Coordinate sourceCoord = this.Character.Coordinates;
            CellAO.Core.Vector.Vector3 targetPosition = this.followCoordinates;
            if (!this.followIdentity.Equals(Identity.None))
            {
                ICharacter targetChar = Pool.Instance.GetObject<ICharacter>(this.followIdentity);
                if (targetChar == null)
                {
                    // If target does not longer exist (death or zone or logoff) then stop following
                    this.followIdentity = Identity.None;
                    this.followCoordinates = new CellAO.Core.Vector.Vector3();
                    return;
                }

                targetPosition = targetChar.Coordinates.coordinate;
            }

            // Do we have coordinates to follow?
            if (targetPosition.Distance2D(new CellAO.Core.Vector.Vector3()) < 0.01f)
            {
                return;
            }

            // /!\ If target flies away, there has to be some kind of adjustment
            CellAO.Core.Vector.Vector3 start = sourceCoord.coordinate;
            CellAO.Core.Vector.Vector3 dest = targetPosition;

            // Check if we have arrived
            if (start.Distance2D(dest) < 1.0f)
            {
                this.StopMovement();
                this.Character.RawCoordinates = dest;
                this.followCoordinates = new CellAO.Core.Vector.Vector3();
                return;
            }

            LogUtil.Debug(DebugInfoDetail.Movement, "Distance to target: " + start.Distance2D(dest).ToString());

            // If target moved or first call, then issue a new follow
            if (this.followCoordinates.Distance2D(dest) > 1.0f)
            {
                this.Character.Coordinates = sourceCoord;
                CellAO.Core.Vector.Vector3 temp = dest - start;
                temp.y = 0;
                this.Character.Heading =
                    (CellAO.Core.Vector.Quaternion)
                        this.Character.Heading.GenerateRotationFromDirectionVector(temp.Normalize());
                this.followCoordinates = dest;
                FollowTargetMessageHandler.Default.Send(this.Character, start, dest);
                this.Run();
            }
        }

        public bool IsFollowing()
        {
            return ((!this.followIdentity.Equals(Identity.None)) || (this.followCoordinates.x != 0.0f)
                    || (this.followCoordinates.y != 0.0f) || (this.followCoordinates.z != 0.0f));
        }

        public void Run()
        {
            this.Character.UpdateMoveType(25); // Magic number: Switch to run
            this.Character.UpdateMoveType(1); // Magic number: Forward start
        }

        public void StopMovement()
        {
            this.Character.UpdateMoveType(2); // Magic numer: Forward stop
        }

        public void Walk()
        {
            this.Character.UpdateMoveType(24); // Magic number: Switch to walk
            this.Character.UpdateMoveType(1); // Magic number: Forward start
        }

        public bool SaveToDatabase
        {
            get
            {
                return false;
            }
        }

        ~NPCController()
        {
            LogUtil.Debug(DebugInfoDetail.Memory, "NPC Controller finished");
            LogUtil.Debug(DebugInfoDetail.Memory, new StackTrace().ToString());
        }

        public bool Move(int moveType, Coordinate newCoordinates, Quaternion heading)
        {
            return false;
        }

        public void Move()
        {
        }

        public void StopFollow()
        {
            this.followIdentity = Identity.None;
            lock (this.followCoordinates)
            {
                this.followCoordinates = new CellAO.Core.Vector.Vector3();
            }
        }
    }
}