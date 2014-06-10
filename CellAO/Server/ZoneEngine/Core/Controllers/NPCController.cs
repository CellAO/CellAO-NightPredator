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

    using CellAO.Core.Entities;
    using CellAO.Core.Functions;
    using CellAO.Core.Network;
    using CellAO.Core.Vector;
    using CellAO.Enums;
    using CellAO.Interfaces;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    using ZoneEngine.Core.Functions;
    using ZoneEngine.Core.KnuBot;
    using ZoneEngine.Core.MessageHandlers;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = CellAO.Core.Vector.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public class NPCController : IController
    {
        public BaseKnuBot KnuBot = null;

        private double lastDistance = double.MaxValue;

        private Identity followIdentity = Identity.None;

        private Vector3 followCoordinates = new Vector3();

        private CharacterState state = CharacterState.Idle;

        public CharacterState State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool LookAt(Identity target)
        {
            throw new NotImplementedException();
        }

        public bool UseStatel(Identity identity, EventType eventType = EventType.OnUse)
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
            this.followIdentity = target;
            ICharacter npc = Pool.Instance.GetObject<ICharacter>(this.Character.Playfield.Identity, target);
            if (npc != null)
            {
                Vector3 temp = npc.Coordinates().coordinate - this.Character.Coordinates().coordinate;
                temp.y = 0;
                this.Character.Heading = (Quaternion)Quaternion.GenerateRotationFromDirectionVector(temp).Normalize();
                FollowTargetMessageHandler.Default.Send(
                    this.Character,
                    this.Character.Coordinates().coordinate,
                    npc.Coordinates().coordinate);
                this.Run();
                this.StartMovement();
            }
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
            // Do we have a attached KnuBot?
            if ((this.KnuBot != null) && (this.KnuBot.Character.Target==null))
            {
                return this.KnuBot.StartDialog(Pool.Instance.GetObject<ICharacter>(Character.Playfield.Identity, target));
            }
            return false;
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

        public void CallFunction(Function function, IEntity caller)
        {
            FunctionCollection.Instance.CallFunction(
                function.FunctionType,
                this.Character,
                caller,
                this.Character,
                function.Arguments.Values.ToArray());
        }

        public void MoveTo(SmokeLounge.AOtomation.Messaging.GameData.Vector3 destination)
        {
            FollowTargetMessageHandler.Default.Send(this.Character, this.Character.RawCoordinates, destination);
            Vector3 dest = destination;
            Vector3 start = this.Character.RawCoordinates;
            dest = start - dest;
            dest = dest.Normalize();
            this.Character.Heading = (Quaternion)Quaternion.GenerateRotationFromDirectionVector(dest);
            this.Run();

            Coordinate c = new Coordinate(destination);
            this.followCoordinates = c.coordinate;
            /*bool arrived = false;
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
            LogUtil.Debug(DebugInfoDetail.Movement, "Arrived at "+this.Character.Coordinates.ToString());
            this.StopMovement();*/
        }

        public void DoFollow()
        {
            Coordinate sourceCoord = this.Character.Coordinates();
            Vector3 targetPosition = this.followCoordinates;
            if (!this.followIdentity.Equals(Identity.None))
            {
                ICharacter targetChar = Pool.Instance.GetObject<ICharacter>(Character.Playfield.Identity, this.followIdentity);
                if (targetChar == null)
                {
                    // If target does not longer exist (death or zone or logoff) then stop following
                    this.followIdentity = Identity.None;
                    this.followCoordinates = new Vector3();
                    return;
                }

                targetPosition = targetChar.Coordinates().coordinate;
            }

            // Do we have coordinates to follow?
            if (targetPosition.Distance2D(new Vector3()) < 0.01f)
            {
                return;
            }

            // /!\ If target flies away, there has to be some kind of adjustment
            Vector3 start = sourceCoord.coordinate;
            Vector3 dest = targetPosition;

            // Check if we have arrived
            if (start.Distance2D(dest) < 0.3f)
            {
                this.StopMovement();
                this.Character.RawCoordinates = dest;
                FollowTargetMessageHandler.Default.Send(this.Character, dest);
                this.followCoordinates = new Vector3();
                return;
            }

            LogUtil.Debug(DebugInfoDetail.Movement, "Distance to target: " + start.Distance2D(dest).ToString());

            // If target moved or first call, then issue a new follow
            if (targetPosition.Distance2D(this.followCoordinates) > 2.0f)
            {
                this.StopMovement();
                this.Character.Coordinates(start);
                FollowTargetMessageHandler.Default.Send(this.Character, start);
                Vector3 temp = start - dest;
                temp.y = 0;
                this.Character.Heading = (Quaternion)Quaternion.GenerateRotationFromDirectionVector(temp);
                this.followCoordinates = dest;
                FollowTargetMessageHandler.Default.Send(this.Character, start, dest);
                this.StartMovement();
            }
        }

        public void StartPatrolling()
        {
            Waypoint next = this.FindNextWaypoint();

            // If a suitable waypoint is found
            if (next != null)
            {
                if (next.Running)
                {
                    this.Run();
                }
                else
                {
                    this.Walk();
                }
                this.followCoordinates = next.Position;
                Vector3 temp = this.Character.Coordinates().coordinate - next.Position;
                temp.y = 0;
                this.Character.Heading = (Quaternion)Quaternion.GenerateRotationFromDirectionVector(temp).Normalize();
                LogUtil.Debug(DebugInfoDetail.Movement, "Direction: " + this.Character.Heading.ToString());
                FollowTargetMessageHandler.Default.Send(
                    this.Character,
                    this.Character.Coordinates().coordinate,
                    next.Position);
                this.StartMovement();
                LogUtil.Debug(DebugInfoDetail.Movement, "Walking to: " + this.followCoordinates);
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
        }

        public void StopMovement()
        {
            this.Character.UpdateMoveType(2); // Magic number: Forward stop
        }

        public void Walk()
        {
            this.Character.UpdateMoveType(24); // Magic number: Switch to walk
        }

        public bool SaveToDatabase
        {
            get
            {
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Client != null)
                {
                    this.Client = null;
                }
            }
        }

        private int activeWaypoint = 0;

        private Waypoint FindNextWaypoint()
        {
            Waypoint result = null;
            if (this.Character.Waypoints.Count <= 2)
            {
                return null;
            }
            if (this.Character.Waypoints.Count <= activeWaypoint)
            {
                activeWaypoint = 0;
            }
            int len = this.Character.Waypoints.Count;
            do
            {
                activeWaypoint = (activeWaypoint + 1) % len;
                result = this.Character.Waypoints[activeWaypoint];
            }
            while (result.Position.Distance2D(this.Character.Coordinates().coordinate)<0.2f);
            return result;
        }

        public void StartMovement()
        {
            this.Character.UpdateMoveType(1); // Magic number: Forward start
        }

        ~NPCController()
        {
            LogUtil.Debug(DebugInfoDetail.Memory, "NPC Controller finished");
            LogUtil.Debug(DebugInfoDetail.Memory, new StackTrace().ToString());
            this.Dispose(false);
        }

        public bool Move(
            int moveType,
            Coordinate newCoordinates,
            SmokeLounge.AOtomation.Messaging.GameData.Quaternion heading)
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
                this.followCoordinates = new Vector3();
            }
        }

        public void SetKnuBot(BaseKnuBot knubot)
        {
            this.KnuBot = knubot;
        }
    }
}