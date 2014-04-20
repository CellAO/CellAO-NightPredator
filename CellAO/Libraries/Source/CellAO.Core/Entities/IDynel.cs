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

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    using ZoneEngine.Core;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public interface IDynel : IPacketReceivingEntity, INamedEntity, IItemContainer
    {
        /// <summary>
        /// </summary>
        IController Controller { get; }

        /// <summary>
        /// </summary>
        bool ChangedAppearance { get; set; }

        /// <summary>
        /// </summary>
        Coordinate Coordinates { get; set; }

        /// <summary>
        /// </summary>
        Quaternion Heading { get; set; }

        /// <summary>
        /// </summary>
        IInventoryPage MainInventory { get; }

        /// <summary>
        /// Caching Mesh layer structure
        /// </summary>
        MeshLayers MeshLayer { get; }

        /// <summary>
        /// </summary>
        TimeSpan PredictionDuration { get; }

        /// <summary>
        /// </summary>
        Vector3 RawCoordinates { get; set; }

        /// <summary>
        /// </summary>
        Quaternion RawHeading { get; set; }

        /// <summary>
        /// </summary>
        string OrganizationName { get; }



        /// <summary>
        /// </summary>
        void Save();

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        void Send(MessageBody messageBody, bool announceToPlayfield = false);

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        void Send(SystemMessage message);

        /// <summary>
        /// </summary>
        void SendChangedStats();

        /// <summary>
        /// </summary>
        void WriteStats();

        bool InPlayfield(Identity identity);
    }
}
