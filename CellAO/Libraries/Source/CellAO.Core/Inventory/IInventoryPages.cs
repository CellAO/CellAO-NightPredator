﻿#region License

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

namespace CellAO.Core.Inventory
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Items;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public interface IInventoryPages
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        IItemContainer Owner { get; }

        /// <summary>
        /// </summary>
        IDictionary<int, IInventoryPage> Pages { get; }

        /// <summary>
        /// </summary>
        int StandardPage { get; set; }

        #endregion

        #region Public Indexers

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        IInventoryPage this[int index] { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="pageNum">
        /// </param>
        /// <param name="slotNum">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        InventoryError AddToPage(int pageNum, int slotNum, IItem item);

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        void CalculateModifiers(Character character);

        /// <summary>
        /// </summary>
        /// <param name="targetLocation">
        /// </param>
        /// <returns>
        /// </returns>
        Item GetItemAt(int targetLocation);

        /// <summary>
        /// </summary>
        /// <param name="container">
        /// </param>
        /// <param name="placement">
        /// </param>
        /// <returns>
        /// </returns>
        Item GetItemInContainer(int container, int placement);

        /// <summary>
        /// </summary>
        /// <param name="slotNum">
        /// </param>
        /// <returns>
        /// </returns>
        IInventoryPage PageFromSlot(int slotNum);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool Read();

        /// <summary>
        /// </summary>
        /// <param name="pageNum">
        /// </param>
        /// <param name="slotNum">
        /// </param>
        /// <returns>
        /// </returns>
        IItem RemoveItem(int pageNum, int slotNum);

        /// <summary>
        /// </summary>
        /// <param name="statId">
        /// </param>
        /// <returns>
        /// </returns>
        int Stat(int statId);

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        InventoryError TryAdd(IItem item);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool Write();

        #endregion
    }
}