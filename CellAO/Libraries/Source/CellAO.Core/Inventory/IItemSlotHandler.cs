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

namespace CellAO.Core.Inventory
{
    #region Usings ...

    #region Usings ...

    using CellAO.Core.Items;
    using CellAO.Enums;

    #endregion

    #endregion

    /// <summary>
    /// </summary>
    public interface IItemSlotHandler
    {
        #region Public Methods and Operators

        /// <summary>
        /// Is called after the given item is added to the given slot
        /// </summary>
        /// <param name="item">
        /// </param>
        void Added(ItemTemplate item);

        /// <summary>
        /// Is called before adding to check whether the item may be added to the corresponding slot 
        /// (given the case that the corresponding slot is valid and unoccupied)
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <param name="err">
        /// </param>
        void CheckAdd(int slot, ItemTemplate item, ref InventoryError err);

        /// <summary>
        /// Is called before removing the given item to check whether it may actually be removed
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="templ">
        /// </param>
        /// <param name="err">
        /// </param>
        void CheckRemove(int slot, ItemTemplate templ, ref InventoryError err);

        /// <summary>
        /// </summary>
        /// <param name="sendingPage">
        /// </param>
        /// <param name="fromPlacement">
        /// </param>
        /// <param name="toPlacement">
        /// </param>
        void Equip(IInventoryPage sendingPage, int fromPlacement, int toPlacement);

        /// <summary>
        /// </summary>
        /// <param name="sendingPage">
        /// </param>
        /// <param name="fromPlacement">
        /// </param>
        /// <param name="toPlacement">
        /// </param>
        void HotSwap(IInventoryPage sendingPage, int fromPlacement, int toPlacement);

        /// <summary>
        /// Is called after the given item is removed from the given slot
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="item">
        /// </param>
        void Removed(int slot, ItemTemplate item);

        /// <summary>
        /// </summary>
        /// <param name="fromPlacement">
        /// </param>
        /// <param name="receivingPage">
        /// </param>
        /// <param name="toPlacement">
        /// </param>
        void Unequip(int fromPlacement, IInventoryPage receivingPage, int toPlacement);

        #endregion
    }
}