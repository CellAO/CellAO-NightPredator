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

namespace CellAO.Core.Entities
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using CellAO.Core.Inventory;
    using CellAO.Core.Nanos;
    using CellAO.Core.Network;
    using CellAO.Core.Textures;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;
    using CellAO.Interfaces;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public class Character : Dynel, ICharacter
    {
        #region Fields

        /// <summary>
        /// </summary>
        public Dictionary<int, int> SocialTab = new Dictionary<int, int>();

        /// <summary>
        /// </summary>
        public List<AOTextures> Textures = new List<AOTextures>();

        /// <summary>
        /// </summary>
        private Timer logoutTimer = null;

        /// <summary>
        /// Caching Mesh layer structure
        /// </summary>
        private MeshLayers meshLayer = new MeshLayers();

        /// <summary>
        /// </summary>
        private MoveDirections moveDirection;

        /// <summary>
        /// </summary>
        private DateTime predictionTime;

        /// <summary>
        /// Caching Mesh layer for social tab items
        /// </summary>
        private MeshLayers socialMeshLayer = new MeshLayers();

        /// <summary>
        /// </summary>
        private SpinOrStrafeDirections spinDirection;

        /// <summary>
        /// </summary>
        private SpinOrStrafeDirections strafeDirection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="pooledIn">
        /// </param>
        /// <param name="identity">
        /// </param>
        /// <param name="zoneClient">
        /// </param>
        public Character(Pool pooledIn, Identity identity, IZoneClient zoneClient)
            : base(pooledIn, identity)
        {
            this.DoNotDoTimers = true;
            this.Client = zoneClient;
            this.ActiveNanos = new List<IActiveNano>();

            this.UploadedNanos = new List<IUploadedNanos>();

            this.BaseInventory = new PlayerInventory(this);

            this.SocialTab = new Dictionary<int, int>
                             {
                                 { 0, 0 }, 
                                 { 1, 0 }, 
                                 { 2, 0 }, 
                                 { 3, 0 }, 
                                 { 4, 0 }, 
                                 { 38, 0 }, 
                                 { 1004, 0 }, 
                                 { 1005, 0 }, 
                                 { 64, 0 }, 
                                 { 32, 0 }, 
                                 { 1006, 0 }, 
                                 { 1007, 0 }
                             };

            this.Read();

            this.meshLayer.AddMesh(0, this.Stats[StatIds.headmesh].Value, 0, 4);
            this.socialMeshLayer.AddMesh(0, this.Stats[StatIds.headmesh].Value, 0, 4);

            this.DoNotDoTimers = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public List<IActiveNano> ActiveNanos { get; private set; }

        /// <summary>
        /// </summary>
        public bool ChangedAppearance { get; set; }

        /// <summary>
        /// </summary>
        public IZoneClient Client { get; set; }

        /// <summary>
        /// </summary>
        public Coordinate Coordinates
        {
            get
            {
                if ((this.moveDirection == MoveDirections.None) && (this.strafeDirection == SpinOrStrafeDirections.None))
                {
                    return new Coordinate(this.RawCoordinates);
                }
                else if (this.spinDirection == SpinOrStrafeDirections.None)
                {
                    Vector.Vector3 moveVector = this.calculateMoveVector();

                    moveVector = moveVector * this.PredictionDuration.TotalSeconds;

                    return new Coordinate(this.RawCoordinates + moveVector);
                }
                else
                {
                    Vector.Vector3 moveVector;
                    Vector.Vector3 positionFromCentreOfTurningCircle;
                    double turnArcAngle;
                    double y;
                    double duration;

                    duration = this.PredictionDuration.TotalSeconds;

                    moveVector = this.calculateMoveVector();
                    turnArcAngle = this.calculateTurnArcAngle();

                    // This is calculated seperately as height is unaffected by turning
                    y = this.RawCoordinates.Y + (moveVector.y * duration);

                    if (this.spinDirection == SpinOrStrafeDirections.Left)
                    {
                        positionFromCentreOfTurningCircle = new Vector.Vector3(moveVector.z, y, -moveVector.x);
                    }
                    else
                    {
                        positionFromCentreOfTurningCircle = new Vector.Vector3(-moveVector.z, y, moveVector.x);
                    }

                    return
                        new Coordinate(
                            new Vector.Vector3(this.RawCoordinates.X, this.RawCoordinates.Y, this.RawCoordinates.Z)
                            + (Vector.Vector3)
                                Quaternion.RotateVector3(
                                    new Quaternion(Vector.Vector3.AxisY, turnArcAngle), 
                                    positionFromCentreOfTurningCircle) - positionFromCentreOfTurningCircle);
                }
            }

            set
            {
                this.RawCoordinates = new Vector3() { X = value.x, Y = value.y, Z = value.z };
            }
        }

        /// <summary>
        /// </summary>
        public Identity FightingTarget { get; set; }

        /// <summary>
        /// Heading as Quaternion
        /// </summary>
        public Quaternion Heading
        {
            get
            {
                if (this.spinDirection == SpinOrStrafeDirections.None)
                {
                    return this.RawHeading;
                }
                else
                {
                    double turnArcAngle;
                    Quaternion turnQuaterion;
                    Quaternion newHeading;

                    turnArcAngle = this.calculateTurnArcAngle();
                    turnQuaterion = new Quaternion(Vector.Vector3.AxisY, turnArcAngle);

                    newHeading = Quaternion.Hamilton(turnQuaterion, this.RawHeading);
                    newHeading.Normalize();

                    return newHeading;
                }
            }

            set
            {
            }
        }

        /// <summary>
        /// </summary>
        public IInventoryPage MainInventory { get; private set; }

        /// <summary>
        /// </summary>
        public MeshLayers MeshLayer
        {
            get
            {
                return this.meshLayer;
            }

            private set
            {
                this.meshLayer = value;
            }
        }

        /// <summary>
        /// </summary>
        public MoveModes MoveMode { get; set; }

        /// <summary>
        /// </summary>
        public string OrganizationName
        {
            get
            {
                try
                {
                    return OrganizationDao.GetOrganizationData(this.Stats[StatIds.clan].Value).Name;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// </summary>
        public TimeSpan PredictionDuration { get; private set; }

        /// <summary>
        /// </summary>
        public MoveModes PreviousMoveMode { get; set; }

        /// <summary>
        /// </summary>
        public Vector3 RawCoordinates { get; set; }

        /// <summary>
        /// </summary>
        public Quaternion RawHeading { get; set; }

        /// <summary>
        /// </summary>
        public Identity SelectedTarget { get; set; }

        /// <summary>
        /// </summary>
        public MeshLayers SocialMeshLayer
        {
            get
            {
                return this.socialMeshLayer;
            }

            private set
            {
                this.socialMeshLayer = value;
            }
        }

        /// <summary>
        /// </summary>
        public TradeSkillInfo TradeSkillSource { get; set; }

        /// <summary>
        /// </summary>
        public TradeSkillInfo TradeSkillTarget { get; set; }

        /// <summary>
        /// </summary>
        public List<IUploadedNanos> UploadedNanos { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        private MoveModes moveMode
        {
            get
            {
                return (MoveModes)this.Stats[StatIds.currentmovementmode].Value;
            }

            set
            {
                this.Stats[StatIds.currentmovementmode].Value = (int)value;
            }
        }

        /// <summary>
        /// </summary>
        private MoveModes previousMoveMode
        {
            get
            {
                return (MoveModes)this.Stats[StatIds.prevmovementmode].Value;
            }

            set
            {
                this.Stats[StatIds.prevmovementmode].Value = (int)value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CalculateSkills()
        {
            // TODO: Reintroduce skill calculation (walk inventory and active nanos)

            // First, walk inventory and get buffs from there
            this.DoNotDoTimers = true;
            this.Stats.ClearModifiers();
            this.Textures.Clear();
            this.meshLayer.Clear();
            this.socialMeshLayer.Clear();
            this.meshLayer.AddMesh(0, (Int32)this.Stats[StatIds.headmesh].BaseValue, 0, 4);
            this.socialMeshLayer.AddMesh(0, (Int32)this.Stats[StatIds.headmesh].BaseValue, 0, 4);

            this.SocialTab = new Dictionary<int, int>
                             {
                                 { 0, 0 }, 
                                 { 1, 0 }, 
                                 { 2, 0 }, 
                                 { 3, 0 }, 
                                 { 4, 0 }, 
                                 { 38, 0 }, 
                                 { 1004, 0 }, 
                                 { 1005, 0 }, 
                                 { 64, 0 }, 
                                 { 32, 0 }, 
                                 { 1006, 0 }, 
                                 { 1007, 0 }
                             };

            this.BaseInventory.CalculateModifiers(this);

            if (this.ChangedAppearance)
            {
                this.Playfield.AnnounceAppearanceUpdate(this);
                this.ChangedAppearance = false;
            }

            this.DoNotDoTimers = false;
        }

        /// <summary>
        /// </summary>
        public override void Dispose()
        {
            this.DoNotDoTimers = true;
            this.Save();
            this.DoNotDoTimers = true;
            if (this.Client != null)
            {
                this.Client.Server.DisconnectClient(this.Client);
                this.Client.Character = null;
            }

            this.Client = null;
            OnlineDao.SetOffline(this.Identity.Instance);
            this.Playfield.Despawn(this.Identity);
            base.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="nanoId">
        /// </param>
        /// <returns>
        /// </returns>
        public bool HasNano(int nanoId)
        {
            return this.UploadedNanos.Any(x => x.NanoId == nanoId);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool InLogoutTimerPeriod()
        {
            return this.logoutTimer != null;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        public void LogoutTimerCallback(object sender)
        {
            if (this.logoutTimer == null)
            {
                // Logout Timer has been cancelled
                return;
            }

            this.logoutTimer = null;
            this.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override bool Read()
        {
            this.DoNotDoTimers = true;
            DBCharacter daochar = CharacterDao.GetById(this.Identity.Instance).FirstOrDefault();
            if (daochar != null)
            {
                this.Name = daochar.Name;
                this.LastName = daochar.LastName;
                this.FirstName = daochar.FirstName;
                this.RawCoordinates = new Vector3 { X = daochar.X, Y = daochar.Y, Z = daochar.Z };
                this.RawHeading = new Quaternion(daochar.HeadingX, daochar.HeadingY, daochar.HeadingZ, daochar.HeadingW);
            }

            foreach (int nano in UploadedNanosDao.ReadNanos(this.Identity.Instance))
            {
                this.UploadedNanos.Add(new UploadedNano() { NanoId = nano });
            }

            this.BaseInventory.Read();
            base.Read();
            this.DoNotDoTimers = false;

            // Implement error checking
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        public void Reconnect(IZoneClient client)
        {
            this.Client = client;
        }

        /// <summary>
        /// </summary>
        public void Save()
        {
            this.Write();
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Send(MessageBody messageBody, bool announceToPlayfield)
        {
            if (!announceToPlayfield)
            {
                this.Send(messageBody);
                return;
            }

            this.Playfield.Announce(messageBody);
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public void Send(SystemMessage message)
        {
            this.Playfield.Send(this.Client, message);
        }

        /// <summary>
        /// </summary>
        public void SendChangedStats()
        {
            var message = new StatMessage() { Identity = this.Identity, };
            message.Stats = this.Stats.ChangedAnnouncingStats;
            if (message.Stats.Length > 0)
            {
                this.Playfield.AnnounceOthers(message, this.Identity);
            }

            message.Stats = this.Stats.ChangedStats;
            if (message.Stats.Length > 0)
            {
                this.Playfield.Send(this.Client, message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool SetFightingTarget(Identity identity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public bool SetTarget(Identity identity)
        {
            this.SelectedTarget = identity;
            return true;
        }

        /// <summary>
        /// </summary>
        public void StartLogoutTimer()
        {
            this.logoutTimer = new Timer(this.LogoutTimerCallback, null, 30000, 0);
        }

        /// <summary>
        /// </summary>
        public void StopLogoutTimer()
        {
            this.logoutTimer = null;
        }

        /// <summary>
        /// </summary>
        public void StopMovement()
        {
            // This should be used to stop the interpolating and save last interpolated value of movement before teleporting
            this.RawCoordinates.X = this.Coordinates.x;
            this.RawCoordinates.Y = this.Coordinates.y;
            this.RawCoordinates.Z = this.Coordinates.z;
            this.RawHeading = this.Heading;

            this.spinDirection = SpinOrStrafeDirections.None;
            this.strafeDirection = SpinOrStrafeDirections.None;
            this.moveDirection = MoveDirections.None;
        }

        /// <summary>
        /// </summary>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        public override void Teleport(Coordinate destination, IQuaternion heading, Identity playfield)
        {
            this.Playfield.Teleport(this, destination, heading, playfield);
        }

        /// <summary>
        /// </summary>
        /// <param name="moveType">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void UpdateMoveType(byte moveType)
        {
            this.predictionTime = DateTime.Now;

            /*
             * NV: Would be nice to have all other possible values filled out for this at some point... *Looks at Suiv*
             * More specifically, 10, 13 and 22 - 10 and 13 seem to be tied to spinning with mouse. 22 seems random (ping mabe?)
             * Also TODO: Tie this with CurrentMovementMode stat and persistance (ie, log off walking, log back on and still walking)
             * Values of CurrentMovementMode and their effects:
                0: slow moving feet not animating
                1: rooted cant sit
                2: walk
                3: run
                4: swim
                5: crawl
                6: sneak
                7: flying
                8: sitting
                9: rooted can sit
                10: same as 0
                11: sleeping
                12: lounging
                13: same as 0
                14: same as 0
                15: same as 0
                16: same as 0
             */
            switch (moveType)
            {
                case 1: // Forward Start
                    this.moveDirection = MoveDirections.Forwards;
                    break;
                case 2: // Forward Stop
                    this.moveDirection = MoveDirections.None;
                    break;

                case 3: // Reverse Start
                    this.moveDirection = MoveDirections.Backwards;
                    break;
                case 4: // Reverse Stop
                    this.moveDirection = MoveDirections.None;
                    break;

                case 5: // Strafe Right Start
                    this.strafeDirection = SpinOrStrafeDirections.Right;
                    break;
                case 6: // Strafe Stop (Right)
                    this.strafeDirection = SpinOrStrafeDirections.None;
                    break;

                case 7: // Strafe Left Start
                    this.strafeDirection = SpinOrStrafeDirections.Left;
                    break;
                case 8: // Strafe Stop (Left)
                    this.strafeDirection = SpinOrStrafeDirections.None;
                    break;

                case 9: // Turn Right Start
                    this.spinDirection = SpinOrStrafeDirections.Right;
                    break;
                case 10: // Mouse Turn Right Start
                    break;
                case 11: // Turn Stop (Right)
                    this.spinDirection = SpinOrStrafeDirections.None;
                    break;

                case 12: // Turn Left Start
                    this.spinDirection = SpinOrStrafeDirections.Left;
                    break;
                case 13: // Mouse Turn Left Start
                    break;
                case 14: // Turn Stop (Left)
                    this.spinDirection = SpinOrStrafeDirections.None;
                    break;

                case 15: // Jump Start

                    // NV: TODO: This!
                    break;
                case 16: // Jump Stop
                    break;

                case 17: // Elevate Up Start
                    break;
                case 18: // Elevate Up Stop
                    break;

                case 19: // ? 19 = 20 = 22 = 31 = 32
                    break;
                case 20: // ? 19 = 20 = 22 = 31 = 32
                    break;

                case 21: // Full Stop
                    break;

                case 22: // ? 19 = 20 = 22 = 31 = 32
                    break;

                case 23: // Switch To Frozen Mode
                    break;
                case 24: // Switch To Walk Mode
                    this.moveMode = MoveModes.Walk;
                    break;
                case 25: // Switch To Run Mode
                    this.moveMode = MoveModes.Run;
                    break;
                case 26: // Switch To Swim Mode
                    break;
                case 27: // Switch To Crawl Mode
                    this.previousMoveMode = this.moveMode;
                    this.moveMode = MoveModes.Crawl;
                    break;
                case 28: // Switch To Sneak Mode
                    this.previousMoveMode = this.moveMode;
                    this.moveMode = MoveModes.Sneak;
                    break;
                case 29: // Switch To Fly Mode
                    break;
                case 30: // Switch To Sit Ground Mode
                    this.previousMoveMode = this.moveMode;
                    this.moveMode = MoveModes.Sit;
                    break;

                case 31: // ? 19 = 20 = 22 = 31 = 32
                    break;
                case 32: // ? 19 = 20 = 22 = 31 = 32
                    break;

                case 33: // Switch To Sleep Mode
                    this.moveMode = MoveModes.Sleep;
                    break;
                case 34: // Switch To Lounge Mode
                    this.moveMode = MoveModes.Lounge;
                    break;

                case 35: // Leave Swim Mode
                    break;
                case 36: // Leave Sneak Mode
                    this.moveMode = this.previousMoveMode;
                    break;
                case 37: // Leave Sit Mode
                    this.moveMode = this.previousMoveMode;
                    break;
                case 38: // Leave Frozen Mode
                    break;
                case 39: // Leave Fly Mode
                    break;
                case 40: // Leave Crawl Mode
                    this.moveMode = this.previousMoveMode;
                    break;
                case 41: // Leave Sleep Mode
                    break;
                case 42: // Leave Lounge Mode
                    break;
                default:

                    // Console.WriteLine("Unknown MoveType: " + moveType);
                    break;
            }

            // Console.WriteLine((moveDirection != 0 ? moveMode.ToString() : "Stand") + "ing in the direction " + moveDirection.ToString() + (spinDirection != 0 ? " while spinning " + spinDirection.ToString() : "") + (strafeDirection != 0 ? " and strafing " + strafeDirection.ToString() : ""));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override bool Write()
        {
            this.BaseInventory.Write();
            CharacterDao.UpdatePosition(this.GetDBCharacter());
            CharacterDao.SetPlayfield(
                this.Identity.Instance, 
                (int)this.Playfield.Identity.Type, 
                this.Playfield.Identity.Instance);
            return base.Write();
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void WriteStats()
        {
            this.Stats.Write();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal DBCharacter GetDBCharacter()
        {
            DBCharacter temp = new DBCharacter();
            temp.FirstName = this.FirstName;
            temp.LastName = this.LastName;

            temp.HeadingW = this.RawHeading.wf;
            temp.HeadingX = this.RawHeading.xf;
            temp.HeadingY = this.RawHeading.yf;
            temp.HeadingZ = this.RawHeading.zf;
            temp.X = this.RawCoordinates.X;
            temp.Y = this.RawCoordinates.Y;
            temp.Z = this.RawCoordinates.Z;

            temp.Id = this.Identity.Instance;
            temp.Name = this.Name;
            temp.Online = 1;
            temp.Playfield = this.Playfield.Identity.Instance;
            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        internal void Send(MessageBody messageBody)
        {
            this.Playfield.Send(this.Client, messageBody);
        }

        /// <summary>
        /// Can Character move?
        /// </summary>
        /// <returns>Can move=true</returns>
        private bool CanMove()
        {
            if ((this.moveMode == MoveModes.Run) || (this.moveMode == MoveModes.Walk)
                || (this.moveMode == MoveModes.Swim) || (this.moveMode == MoveModes.Crawl)
                || (this.moveMode == MoveModes.Sneak) || (this.moveMode == MoveModes.Fly))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculate the effective run speed (run, walk, sneak etc)
        /// </summary>
        /// <returns>Effective run speed</returns>
        private int calculateEffectiveRunSpeed()
        {
            int effectiveRunSpeed;

            switch (this.moveMode)
            {
                case MoveModes.Run:
                    effectiveRunSpeed = this.Stats[StatIds.runspeed].Value; // Stat #156 = RunSpeed
                    break;

                case MoveModes.Walk:
                    effectiveRunSpeed = -500;
                    break;

                case MoveModes.Swim:

                    // Swim speed is calculated the same as Run Speed except is half as effective
                    effectiveRunSpeed = this.Stats[StatIds.swim].Value >> 1; // Stat #138 = Swim
                    break;

                case MoveModes.Crawl:
                    effectiveRunSpeed = -600;
                    break;

                case MoveModes.Sneak:
                    effectiveRunSpeed = -500;
                    break;

                case MoveModes.Fly:
                    effectiveRunSpeed = 2200; // NV: TODO: Propper calc for this!
                    break;

                default:

                    // All other movement modes, sitting, sleeping, lounging, rooted, etc have a speed of 0
                    // As there is no way to 'force' that this way, we just default to 0 and hope that canMove() has been called to properly check.
                    effectiveRunSpeed = 0;
                    break;
            }

            return effectiveRunSpeed;
        }

        /// <summary>
        /// Calculate forward speed
        /// </summary>
        /// <returns>forward speed</returns>
        private double calculateForwardSpeed()
        {
            double speed;
            int effectiveRunSpeed;

            if ((this.moveDirection == MoveDirections.None) || (!this.CanMove()))
            {
                return 0;
            }

            effectiveRunSpeed = this.calculateEffectiveRunSpeed();

            if (this.moveDirection == MoveDirections.Forwards)
            {
                // NV: TODO: Verify this more. Especially with uber-low runspeeds (negative)
                speed = Math.Max(0, (effectiveRunSpeed * 0.005) + 4);

                if (this.moveMode != MoveModes.Swim)
                {
                    speed = Math.Min(15, speed); // Forward speed is capped at 15 units/sec for non-swimming
                }
            }
            else
            {
                // NV: TODO: Verify this more. Especially with uber-low runspeeds (negative)
                speed = -Math.Max(0, (effectiveRunSpeed * 0.0035) + 4);

                if (this.moveMode != MoveModes.Swim)
                {
                    speed = Math.Max(-15, speed); // Backwards speed is capped at 15 units/sec for non-swimming
                }
            }

            return speed;
        }

        /// <summary>
        /// Calculate move vector
        /// </summary>
        /// <returns>Movevector</returns>
        private Vector.Vector3 calculateMoveVector()
        {
            double forwardSpeed;
            double strafeSpeed;
            Vector.Vector3 forwardMove;
            Vector.Vector3 strafeMove;

            if (!this.CanMove())
            {
                return Vector.Vector3.Origin;
            }

            forwardSpeed = this.calculateForwardSpeed();
            strafeSpeed = this.calculateStrafeSpeed();

            if ((forwardSpeed == 0) && (strafeSpeed == 0))
            {
                return Vector.Vector3.Origin;
            }

            if (forwardSpeed != 0)
            {
                forwardMove = (Vector.Vector3)Quaternion.RotateVector3(this.RawHeading, Vector.Vector3.AxisZ);
                forwardMove.Magnitude = Math.Abs(forwardSpeed);
                if (forwardSpeed < 0)
                {
                    forwardMove = -forwardMove;
                }
            }
            else
            {
                forwardMove = Vector.Vector3.Origin;
            }

            if (strafeSpeed != 0)
            {
                strafeMove = (Vector.Vector3)Quaternion.RotateVector3(this.RawHeading, Vector.Vector3.AxisX);
                strafeMove.Magnitude = Math.Abs(strafeSpeed);
                if (strafeSpeed < 0)
                {
                    strafeMove = -strafeMove;
                }
            }
            else
            {
                strafeMove = Vector.Vector3.Origin;
            }

            return forwardMove + strafeMove;
        }

        /// <summary>
        /// Calculate strafe speed
        /// </summary>
        /// <returns>Strafe speed</returns>
        private double calculateStrafeSpeed()
        {
            double speed;
            int effectiveRunSpeed;

            // Note, you can not strafe while swimming or crawling
            if ((this.strafeDirection == SpinOrStrafeDirections.None) || (this.moveMode == MoveModes.Swim)
                || (this.moveMode == MoveModes.Crawl) || (!this.CanMove()))
            {
                return 0;
            }

            effectiveRunSpeed = this.calculateEffectiveRunSpeed();

            // NV: TODO: Update this based off Forward runspeed when that is checked (strafe effective run speed = effective run speed / 2)
            speed = ((effectiveRunSpeed / 2) * 0.005) + 4;

            if (this.strafeDirection == SpinOrStrafeDirections.Left)
            {
                speed = -speed;
            }

            return speed;
        }

        /// <summary>
        /// Calculate Turnangle
        /// </summary>
        /// <returns>Turnangle</returns>
        private double calculateTurnArcAngle()
        {
            double turnTime;
            double angle;
            double modifiedDuration;

            turnTime = this.calculateTurnTime();

            modifiedDuration = this.PredictionDuration.TotalSeconds % turnTime;

            angle = 2 * Math.PI * modifiedDuration / turnTime;

            return angle;
        }

        /// <summary>
        /// Calculate Turn time
        /// </summary>
        /// <returns>Turn time</returns>
        private double calculateTurnTime()
        {
            int turnSpeed;
            double turnTime;

            turnSpeed = this.Stats[StatIds.turnspeed].Value; // Stat #267 TurnSpeed

            if (turnSpeed == 0)
            {
                turnSpeed = 40000;
            }

            turnTime = 70000 / turnSpeed;

            return turnTime;
        }

        #endregion
    }
}