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
// Last modified: 2013-11-16 19:02

#endregion

namespace CellAO.Core.Inventory
{
    #region Usings ...

    using System;

    using CellAO.Core.Items;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class SocialArmorInventoryPage : BaseInventoryPage, IItemSlotHandler
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="ownerInstance">
        /// </param>
        public SocialArmorInventoryPage(int ownerInstance)
            : base((int)IdentityType.SocialPage, 15, 0x31, ownerInstance)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Added(ItemTemplate item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <param name="err">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CheckAdd(int slot, ItemTemplate item, ref InventoryError err)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="templ">
        /// </param>
        /// <param name="err">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CheckRemove(int slot, ItemTemplate templ, ref InventoryError err)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="sendingPage">
        /// </param>
        /// <param name="fromPlacement">
        /// </param>
        /// <param name="toPlacement">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Equip(IInventoryPage sendingPage, int fromPlacement, int toPlacement)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="sendingPage">
        /// </param>
        /// <param name="fromPlacement">
        /// </param>
        /// <param name="toPlacement">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void HotSwap(IInventoryPage sendingPage, int fromPlacement, int toPlacement)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Removed(int slot, ItemTemplate item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="fromPlacement">
        /// </param>
        /// <param name="receivingPage">
        /// </param>
        /// <param name="toPlacement">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Unequip(int fromPlacement, IInventoryPage receivingPage, int toPlacement)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}