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

    using CellAO.Core.Inventory;
    using CellAO.Core.Network;
    using CellAO.Core.Textures;
    using CellAO.Core.Vector;
    using CellAO.Enums;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.Messages;

    using ZoneEngine.Core;

    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;
    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public interface ICharacter : IDynel, ISummoner, ITargetingEntity
    {
        /// <summary>
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        void Teleport(Coordinate destination, IQuaternion heading, Identity playfield);

        /// <summary>
        /// Active Nanos list
        /// </summary>
        Dictionary<int, IActiveNano> ActiveNanos { get; }

        /// <summary>
        /// Caching Mesh layer for social tab items
        /// </summary>
        MeshLayers SocialMeshLayer { get; }

        /// <summary>
        /// Uploaded Nanos list
        /// </summary>
        List<IUploadedNanos> UploadedNanos { get; }


        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        void CalculateSkills();

        /// <summary>
        /// </summary>
        TradeSkillInfo TradeSkillSource { get; set; }

        /// <summary>
        /// </summary>
        TradeSkillInfo TradeSkillTarget { get; set; }

        /// <summary>
        /// </summary>
        MoveModes MoveMode { get; set; }

        /// <summary>
        /// </summary>
        MoveModes PreviousMoveMode { get; set; }

        /// <summary>
        /// </summary>
        void UpdateMoveType(byte moveType);


        /// <summary>
        /// </summary>
        bool InLogoutTimerPeriod();

        void StopLogoutTimer();

        void SetCoordinates(Coordinate newCoordinates, Vector.Quaternion heading);

        void StartLogoutTimer();

        void Reconnect(IZoneClient zoneClient);

        int CalculateNanoAttackTime(Nanos.NanoFormula nano);
    }
}