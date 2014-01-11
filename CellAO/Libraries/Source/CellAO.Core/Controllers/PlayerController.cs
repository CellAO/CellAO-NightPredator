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

namespace CellAO.Core.Controllers
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;

    #endregion

    /// <summary>
    /// </summary>
    internal class PlayerController : IController
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public ICharacter Character { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="nanoId">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool CastNano(int nanoId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="page">
        /// </param>
        /// <param name="slot">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool DeleteItem(IInventoryPage page, int slot)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="pageFrom">
        /// </param>
        /// <param name="fromSlot">
        /// </param>
        /// <param name="pageTo">
        /// </param>
        /// <param name="toSlot">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool EquipItem(IInventoryPage pageFrom, int fromSlot, IInventoryPage pageTo, int toSlot)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="pageFrom">
        /// </param>
        /// <param name="fromSlot">
        /// </param>
        /// <param name="pageTo">
        /// </param>
        /// <param name="toSlot">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool UnequipItem(IInventoryPage pageFrom, int fromSlot, IInventoryPage pageTo, int toSlot)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool UseItem(Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="item1Page">
        /// </param>
        /// <param name="item1Slot">
        /// </param>
        /// <param name="item2Page">
        /// </param>
        /// <param name="item2Slot">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool UseItemOnItem(IInventoryPage item1Page, int item1Slot, IInventoryPage item2Page, int item2Slot)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}