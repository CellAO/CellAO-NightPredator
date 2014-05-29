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

namespace CellAO.Core.Entities
{
    #region Usings ...

    using System.Linq;
    using System.Runtime.CompilerServices;

    using CellAO.Core.Exceptions;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.ObjectManager;
    using CellAO.Stats;

    using MsgPack.Serialization.EmittingSerializers;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    public class Vendor : Dynel
    {
        public Vendor(Identity parent, Identity id, string templateHash)
            : base(parent, id)
        {
            DBVendorTemplate vendorTemplate =
                VendorTemplateDao.Instance.GetWhere(new { Hash = templateHash }).FirstOrDefault();
            if (vendorTemplate == null)
            {
                throw new DataBaseException("Could not find a vendor template for hash '" + templateHash + "'.");
            }


            this.Stats = new SimpleStatList();
            this.Template = ItemLoader.ItemList[vendorTemplate.ItemTemplate];
            foreach (var s in this.Template.Stats)
            {
                this.Stats[s.Key].Value = s.Value;
            }
            this.TemplateHash = vendorTemplate.Hash;
            this.Name = vendorTemplate.Name;

            this.BaseInventory = new VendorInventory(this);
            this.BaseInventory.Read();
        }

        public ItemTemplate Template { get; private set; }

        public string TemplateHash { get; private set; }
    }
}