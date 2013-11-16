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
// Last modified: 2013-11-16 15:14

#endregion

namespace CellAO.Core.Entities
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Inventory;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;
    using CellAO.Enums;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    #endregion

    /// <summary>
    /// </summary>
    public class Character : Dynel, ICharacter
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="zoneClient">
        /// </param>
        /// <param name="identity">
        /// </param>
        public Character(IZoneClient zoneClient, Identity identity)
            : base(identity)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public List<IActiveNano> ActiveNanos { get; private set; }

        /// <summary>
        /// </summary>
        public IZoneClient Client { get; private set; }

        /// <summary>
        /// </summary>
        public ICoordinate Coordinates { get; set; }

        /// <summary>
        /// </summary>
        public Identity FightingTarget { get; set; }

        /// <summary>
        /// </summary>
        public Quaternion Heading { get; set; }

        /// <summary>
        /// </summary>
        public IInventoryPage MainInventory { get; private set; }

        /// <summary>
        /// </summary>
        public IMeshLayers MeshLayer { get; private set; }

        /// <summary>
        /// </summary>
        public MoveModes MoveMode { get; set; }

        /// <summary>
        /// </summary>
        public IPlayfield Playfield { get; set; }

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
        public IMeshLayers SocialMeshLayer { get; private set; }

        /// <summary>
        /// </summary>
        public List<IUploadedNanos> UploadedNanos { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CalculateSkills()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
        /// <param name="moveType">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void UpdateMoveType(byte moveType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void WriteStats()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}