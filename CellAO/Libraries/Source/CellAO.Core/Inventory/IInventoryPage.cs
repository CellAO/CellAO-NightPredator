#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-11-01 18:27

#endregion

namespace CellAO.Core.Inventory
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Items;
    using CellAO.Enums;

    #endregion

    public interface IInventoryPage : IEntity
    {
        /// <summary>
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// </summary>
        int MaxSlots { get; set; }

        /// <summary>
        /// </summary>
        int FirstSlotNumber { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        InventoryError Add(int slot, IItem item);

        /// <summary>
        /// </summary>
        /// <param name="slotNum">
        /// </param>
        /// <returns>
        /// </returns>
        IItem Remove(int slotNum);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool Read();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool Write();

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        IItem this[int index] { get; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IDictionary<int, IItem> List();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        int FindFreeSlot();

        bool ValidSlot(int slotNum);
    }
}