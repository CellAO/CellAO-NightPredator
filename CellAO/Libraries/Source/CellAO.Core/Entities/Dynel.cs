#region License

// Copyright (c) 2005-2016, CellAO Team
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

namespace CellAO.Core.Entities
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Components;
    using CellAO.Core.Inventory;
    using CellAO.Core.Playfields;
    using CellAO.Core.Textures;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Enums;
    using CellAO.Interfaces;
    using CellAO.ObjectManager;
    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    using Utility;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public partial class Dynel : PooledObject, IDynel
    {
        #region Fields and Properties

        /// <summary>
        /// </summary>
        private Identity playfieldIdentity;

        /// <summary>
        /// </summary>
        private Utility.WeakReference<IController> controller;

        /// <summary>
        /// </summary>
        protected SpinOrStrafeDirections spinDirection;

        /// <summary>
        /// Caching Mesh layer structure
        /// </summary>
        protected MeshLayers meshLayer = new MeshLayers();

        /// <summary>
        /// </summary>
        public List<AOTextures> Textures = new List<AOTextures>();

        /// <summary>
        /// </summary>
        protected DateTime PredictionTime;

        public bool IsTeleporting = false;

        private bool disposed = false;

        /// <summary>
        /// </summary>
        public IController Controller { get; set; }

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public IPlayfield Playfield
        {
            get
            {
                if (this.playfieldIdentity.Equals(Identity.None))
                {
                    return null;
                }
                return Pool.Instance.GetObject<IPlayfield>(Identity.None, this.playfieldIdentity);
            }

            set
            {
                if (value == null)
                {
                    this.playfieldIdentity = Identity.None;
                }
                else
                {
                    this.playfieldIdentity = value.Identity;
                }
            }
        }

        private bool doNotDoTimers = true;

        /// <summary>
        /// </summary>
        public bool DoNotDoTimers
        {
            get
            {
                return this.doNotDoTimers;
            }
            set
            {
                this.doNotDoTimers = value;
                if (this.Stats != null)
                {
                    this.Stats.ClearChangedFlags();
                }
            }
        }

        /// <summary>
        /// </summary>
        public bool Starting { get; set; }

        /// <summary>
        /// </summary>
        public virtual Coordinate Coordinates()
        {
            return new Coordinate(this.RawCoordinates);
        }

        public virtual void Coordinates(Vector3 position)
        {
            this.RawCoordinates = position;
        }

        public virtual void Coordinates(Vector.Vector3 position)
        {
            this.RawCoordinates = position;
        }

        public virtual void Coordinates(Coordinate position)
        {
            this.RawCoordinates = position.coordinate;
        }

        /// <summary>
        /// </summary>
        public TimeSpan PredictionDuration
        {
            get
            {
                return DateTime.UtcNow - this.PredictionTime;
            }
        }

        /// <summary>
        /// </summary>
        public virtual Vector3 RawCoordinates { get; set; }

        /// <summary>
        /// </summary>
        public Quaternion RawHeading { get; set; }

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
                this.RawHeading = new Quaternion(value.xf, value.yf, value.zf, value.wf);
            }
        }

        /// <summary>
        /// </summary>
        public IInventoryPages BaseInventory { get; set; }

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
        public string OrganizationName
        {
            get
            {
                try
                {
                    return OrganizationDao.Instance.Get(this.Stats[StatIds.clan].Value).Name;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// </summary>
        public bool ChangedAppearance { get; set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        public Dynel(Identity parent, Identity id)
            : base(parent, id)
        {
            this.Starting = true;
            this.DoNotDoTimers = true;

            this.Stats = new Stats(this.Identity);
            this.InitializeStats();

            // The subclasses will initialize their own BaseInventory
            // this.BaseInventory = new UnitInventory(this, pooledIn);

            this.Starting = false;
        }

        #endregion

        #region Positioning

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

        /// <summary>
        /// Calculate Turnangle
        /// </summary>
        /// <returns>Turnangle</returns>
        protected double calculateTurnArcAngle()
        {
            double turnTime;
            double angle;
            double modifiedDuration;

            turnTime = this.calculateTurnTime();

            modifiedDuration = this.PredictionDuration.TotalSeconds % turnTime;

            angle = 2 * Math.PI * modifiedDuration / turnTime;

            return angle;
        }

        #endregion

        #region Object

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public virtual bool Read()
        {
            bool oldtimers = this.doNotDoTimers;
            this.DoNotDoTimers = true;
            this.Stats.Read();

            this.BaseInventory.Read();

            // base.Read();
            if (!oldtimers)
            {
                this.DoNotDoTimers = false;
            }
            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public virtual bool Write()
        {
            this.DoNotDoTimers = true;
            this.Stats.Write();

            // write dynel properties to database ??

            this.DoNotDoTimers = false;
            return true;
        }

        /// <summary>
        /// Wrapper for this.Write()
        /// </summary>
        public void Save()
        {
            this.Write();
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    // Write stats to database
                    this.Write();
                    if (!this.IsTeleporting)
                    {
                        if (this is Character)
                        {
                            this.Controller.LogoffCharacter();
                        }
                    }
                }
            }
            this.disposed = true;
            base.Dispose(disposing);
        }

        #endregion

        #region Statistics

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void WriteStats()
        {
            this.Stats.Write();
        }

        /// <summary>
        /// </summary>
        public void SendChangedStats()
        {
            this.Controller.SendChangedStats();
        }

        #endregion

        #region Teleporting

        /// <summary>
        /// </summary>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        public void Teleport(Coordinate destination, IQuaternion heading, Identity playfield)
        {
            this.IsTeleporting = false;
            this.Playfield.Teleport(this, destination, heading, playfield);
        }

        #endregion

        #region Messaging

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public void Send(SystemMessage message)
        {
            this.Controller.Client.SendCompressed(message);
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
        /// <param name="messageBody">
        /// </param>
        public void Send(MessageBody messageBody)
        {
            if (this.Controller != null)
            {
                if (this.Controller.Client != null)
                {
                    this.Controller.Client.SendCompressed(messageBody);
                }
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public bool InPlayfield(Identity identity)
        {
            bool result = false;
            if (this.Playfield != null)
            {
                result = this.Playfield.Identity == identity;
            }

            return result;
        }
    }
}