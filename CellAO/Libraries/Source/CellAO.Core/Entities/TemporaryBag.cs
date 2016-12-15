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

    using System.Linq;

    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    #endregion

    public class TemporaryBag : PooledObject
    {
        public static IdentityType Type = IdentityType.TempBag;

        private readonly OutgoingTradeInventoryPage charactersBag;

        private readonly KnuBotTradeInventoryPage vendorsBag;

        public TemporaryBag(Identity parent, Identity id, Identity shopper, Identity vendor, int vendorSlots = 255)
            : base(parent, id)
        {
            this.Shopper = shopper;
            this.Vendor = vendor;
            this.charactersBag = new OutgoingTradeInventoryPage(id, vendorSlots);
            this.vendorsBag = new KnuBotTradeInventoryPage(id);
        }

        public Identity Shopper { get; set; }

        public Identity Vendor { get; set; }

        public bool Add(Identity from, IItem item)
        {
            if (from.Equals(this.Shopper))
            {
                this.vendorsBag.Add(this.vendorsBag.FindFreeSlot(), item);
                LogUtil.Debug(DebugInfoDetail.Shopping, "Added Item from character " + from.ToString(true));
            }
            else
            {
                this.charactersBag.Add(from.Instance);
                LogUtil.Debug(DebugInfoDetail.Shopping, "Added Item from shop on position " + from.ToString(true));
            }

            // For now no invalid trades
            return true;
        }

        public IItem Remove(Identity from, int slot)
        {
            if (from.Equals(this.Shopper))
            {
                LogUtil.Debug(DebugInfoDetail.Shopping, "Removed Item from character in shopbag from slot " + slot);
                return this.vendorsBag.Remove(slot);
            }
            this.charactersBag.Remove(slot);
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Remove linkage from character object first
                ICharacter character = Pool.Instance.GetObject<ICharacter>(this.Shopper);
                if (character != null)
                {
                    if (character.ShoppingBag == this)
                    {
                        character.ShoppingBag = null;
                    }
                }

                // Dispose the internal inventory pages
                this.charactersBag.Dispose();
                this.vendorsBag.Dispose();
            }

            base.Dispose(disposing);
        }

        public IItem[] GetBoughtItems()
        {
            IItemContainer seller = Pool.Instance.GetObject<IItemContainer>(this.Vendor);
            return charactersBag.GetItems(seller.BaseInventory[seller.BaseInventory.StandardPage]);
        }

        public IItem[] GetSoldItems()
        {
            return vendorsBag.List().Values.ToArray();
        }
    }
}