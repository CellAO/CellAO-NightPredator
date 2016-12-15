#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace CellAO.Core.Inventory
{
    #region Usings ...

    using System;
    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Exceptions;
    using CellAO.Core.Items;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    #endregion

    public class VendorInventoryPage : BaseInventoryPage, IItemSlotHandler, IItemHotSwapHandler, IEquipmentPage
    {
        private readonly Utility.WeakReference<Vendor> ownerReference;

        public VendorInventoryPage(Identity ownerInstance)
            : base((int)IdentityType.Inventory, 250, 0, ownerInstance)
        {
            this.ownerReference = new Utility.WeakReference<Vendor>(Pool.Instance.GetObject<Vendor>(ownerInstance));
        }

        public int Stat(int statId)
        {
            if (this.ownerReference == null)
            {
                throw new Exception("Vendor still not initialized? Or is it gone already...");
            }

            return this.ownerReference.Target.Stats[statId].Value;
        }

        public override bool Read()
        {
            Random rnd = new Random(Environment.TickCount);

            string templateHash = this.ownerReference.Target.TemplateHash;
            if (!string.IsNullOrEmpty(templateHash))
            {
                DBVendorTemplate vendorTemplate =
                    VendorTemplateDao.Instance.GetWhere(new { Hash = templateHash }).FirstOrDefault();
                if (vendorTemplate == null)
                {
                    throw new DataBaseException(
                        "No vendor found for Template " + templateHash + "." + Environment.NewLine
                        + "Please fill in the database :)");
                }

                int slotNumber = 0;
                DBShopInventoryTemplate[] inventoryEntries =
                    ShopInventoryTemplateDao.Instance.GetWhere(new { Hash = vendorTemplate.ShopInvHash }).ToArray();
                foreach (DBShopInventoryTemplate inventoryEntry in inventoryEntries)
                {
                    // Skip entries which are out of vendortemplate's QL range
                    if ((inventoryEntry.MinQl > vendorTemplate.MaxQl) || (inventoryEntry.MaxQl < vendorTemplate.MinQl))
                    {
                        continue;
                    }

                    // narrow down the range to least amount of QL
                    int minQl = inventoryEntry.MinQl > vendorTemplate.MinQl
                        ? inventoryEntry.MinQl
                        : vendorTemplate.MinQl;
                    int maxQl = inventoryEntry.MaxQl < vendorTemplate.MaxQl
                        ? inventoryEntry.MaxQl
                        : vendorTemplate.MaxQl;

                    int itemQl = minQl + rnd.Next(maxQl - minQl);

                    Item item = new Item(itemQl, inventoryEntry.LowId, inventoryEntry.HighId);
                    this.Add(slotNumber, item);
                    slotNumber++;
                    if (slotNumber >= this.MaxSlots)
                    {
                        break;
                    }
                }
            }
            return true;
        }

        public new IItem Remove(int slotNum)
        {
            return this[slotNum];
        }

        public override bool Write()
        {
            return true;
        }
    }
}