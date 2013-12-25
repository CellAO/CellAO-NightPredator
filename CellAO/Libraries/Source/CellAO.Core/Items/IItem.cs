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

namespace CellAO.Core.Items
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Actions;
    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// Item Interface
    /// </summary>
    public interface IItem
    {
        #region Public Properties

        /// <summary>
        /// Item's Flags
        /// </summary>
        int Flags { get; }

        /// <summary>
        /// HighId of the item template
        /// </summary>
        int HighID { get; }

        /// <summary>
        /// Identity of the item (if it is instanced)
        /// </summary>
        Identity Identity { get; }

        /// <summary>
        /// </summary>
        List<Actions> ItemActions { get; }

        /// <summary>
        /// </summary>
        List<Events> ItemEvents { get; }

        /// <summary>
        /// LowId of the item template
        /// </summary>
        int LowID { get; }

        /// <summary>
        /// Stacked count of the item
        /// </summary>
        int MultipleCount { get; set; }

        /// <summary>
        /// We Dont Know (TM)
        /// </summary>
        int Nothing { get; }

        /// <summary>
        /// Quality level of the item
        /// </summary>
        int Quality { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="actionType">
        /// </param>
        /// <param name="entity">
        /// </param>
        /// <returns>
        /// </returns>
        bool CheckRequirements(ActionType actionType, ITargetingEntity entity);

        /// <summary>
        /// Get item attribute
        /// </summary>
        /// <param name="attributeId">
        /// Id of the attribute
        /// </param>
        /// <returns>
        /// Stored item attribute value
        /// </returns>
        int GetAttribute(int attributeId);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        byte[] GetItemAttributes();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool IsInstanced();

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="eventType">
        /// </param>
        /// <param name="itemSlot">
        /// </param>
        void PerformAction(ICharacter character, EventType eventType, int itemSlot);

        /// <summary>
        /// Set an item attribute
        /// </summary>
        /// <param name="attributeId">
        /// Id of the attribute
        /// </param>
        /// <param name="newValue">
        /// The new value of the item attribute
        /// </param>
        void SetAttribute(int attributeId, int newValue);

        /// <summary>
        /// Write item to database
        /// </summary>
        void WriteToDatabase();

        #endregion
    }
}