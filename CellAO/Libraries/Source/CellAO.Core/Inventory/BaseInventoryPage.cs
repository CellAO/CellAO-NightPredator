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

namespace CellAO.Core.Inventory
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Items;
    using CellAO.Database.Dao;
    using CellAO.Enums;

    using Dapper;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public abstract class BaseInventoryPage : IInventoryPage
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, IItem> Content;

        /// <summary>
        /// </summary>
        private Identity identity;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="pagenum">
        /// </param>
        /// <param name="maxslots">
        /// </param>
        /// <param name="firstslotnumber">
        /// </param>
        /// <param name="ownerInstance">
        /// </param>
        public BaseInventoryPage(int pagenum, int maxslots, int firstslotnumber, int ownerInstance)
            : this()
        {
            this.identity.Type = (IdentityType)pagenum;
            this.identity.Instance = ownerInstance;
            this.MaxSlots = maxslots;
            this.FirstSlotNumber = firstslotnumber;
        }

        /// <summary>
        /// </summary>
        private BaseInventoryPage()
        {
            this.Identity = new Identity();
            this.Content = new Dictionary<int, IItem>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int FirstSlotNumber { get; set; }

        /// <summary>
        /// </summary>
        public Identity Identity
        {
            get
            {
                return this.identity;
            }

            set
            {
                this.identity = value;
            }
        }

        /// <summary>
        /// </summary>
        public int MaxSlots { get; set; }

        /// <summary>
        /// </summary>
        public bool NeedsItemCheck { get; set; }

        /// <summary>
        /// </summary>
        public virtual int Page
        {
            get
            {
                return (int)this.identity.Type;
            }

            set
            {
                this.identity.Type = (IdentityType)value;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public IItem this[int index]
        {
            get
            {
                if (this.Content.ContainsKey(index))
                {
                    return this.Content[index];
                }

                return null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="slot">
        /// </param>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public virtual InventoryError Add(int slot, IItem item)
        {
            if (this.Content.ContainsKey(slot))
            {
                throw new ArgumentException(
                    "Already item in slot " + slot + " of container " + this.Identity.Type + ":"
                    + this.Identity.Instance);
            }

            if ((slot < this.FirstSlotNumber) || (slot > this.FirstSlotNumber + this.MaxSlots))
            {
                throw new ArgumentOutOfRangeException(
                    "Slot out of range: " + slot + " not in " + this.FirstSlotNumber + " to "
                    + (this.FirstSlotNumber + this.MaxSlots));
            }

            this.Content.Add(slot, item);
            return InventoryError.OK;
        }

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
        /// <param name="character">
        /// </param>
        public virtual void CalculateModifiers(Character character)
        {
            // Do nothing
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
        public void CheckAdd(int slot, ItemTemplate item, InventoryError err)
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
        public void CheckRemove(int slot, ItemTemplate templ, InventoryError err)
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
            IItem toEquip = sendingPage[fromPlacement];

            // First: Check if the item can be worn
            bool canBeWornCheck = (toEquip.GetAttribute(30) & (int)CanFlags.Wear) == (int)CanFlags.Wear;

            if (canBeWornCheck)
            {
                this.Add(toPlacement, toEquip);
                sendingPage.Remove(fromPlacement);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int FindFreeSlot()
        {
            int slot = this.FirstSlotNumber;
            while (slot < this.FirstSlotNumber + this.MaxSlots)
            {
                if (!this.Content.ContainsKey(slot))
                {
                    return slot;
                }

                slot++;
            }

            return -1;
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
            IItem toEquip = sendingPage[fromPlacement];
            IItem hotSwapItem = this[toPlacement];

            sendingPage.Remove(fromPlacement);
            this.Remove(toPlacement);

            sendingPage.Add(fromPlacement, hotSwapItem);
            this.Add(toPlacement, toEquip);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IDictionary<int, IItem> List()
        {
            return this.Content;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Read()
        {
            int containerType = (int)this.Identity.Type;

            foreach (DBItem item in ItemDao.Instance.GetAllInContainer(containerType, this.Identity.Instance))
            {
                Item newItem = new Item(item.quality, item.lowid, item.highid);
                newItem.SetAttribute(412, item.multiplecount);
                this.Content.Add(item.containerplacement, newItem);

                // Make item visible
                // TODO: Other flags must be set too
                newItem.Flags |= 0x1;
            }

            foreach (DBInstancedItem item in InstancedItemDao.Instance.GetAll(new DynamicParameters(new { containertype = containerType, containerinstance = this.identity.Instance })))
            {
                Item newItem = new Item(item.quality, item.lowid, item.highid);
                newItem.SetAttribute(412, item.multiplecount);
                Identity temp = new Identity();
                temp.Type = (IdentityType)item.itemtype;
                temp.Instance = item.Id;
                newItem.Identity = temp;

                byte[] binaryStats = item.stats.ToArray();
                for (int i = 0; i < binaryStats.Length / 8; i++)
                {
                    int statid = BitConverter.ToInt32(binaryStats, i * 8);
                    int statvalue = BitConverter.ToInt32(binaryStats, i * 8 + 4);
                    newItem.SetAttribute(statid, statvalue);
                }

                // Make item visible
                // TODO: Other flags must be set too
                // Anything ->    =0x01
                // Containers ->  =0x02
                // ????? ->       |0x20
                // ????? ->       |0x80 (maybe unique)

                // Found online: 0xa1 for nano instruction disc
                // 0x02 for any bag
                // 0x81 for unique totw rings
                newItem.Flags |= 0x1;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="slotNum">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IItem Remove(int slotNum)
        {
            // TODO: Item placement switches could cause items to disappear when zoneengine crashes at that moment
            if (!this.Content.ContainsKey(slotNum))
            {
                throw new ArgumentOutOfRangeException(
                    "No item in slot " + slotNum + " of container " + this.Identity.Type + ":" + this.Identity.Instance);
            }

            IItem temp = this.Content[slotNum];
            int containerType = (int)this.Identity.Type;

            if (temp.Identity.Type == IdentityType.None)
            {
                ItemDao.Instance.Delete(new DynamicParameters(new { containertype = containerType, containerinstance = this.identity.Instance, containerplacement = slotNum }));
            }
            else
            {
                InstancedItemDao.Instance.Delete(new DynamicParameters(new { containertype = containerType, containerinstance = this.identity.Instance, containerplacement = slotNum }));
            }

            this.Content.Remove(slotNum);
            return temp;
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
        /// <returns>
        /// </returns>
        public BankSlot[] ToInventoryArray()
        {
            List<BankSlot> slots = new List<BankSlot>();
            foreach (KeyValuePair<int, IItem> kv in this.List())
            {
                short flags = 0;
                if (kv.Value.IsInstanced())
                {
                    flags = 0xa0;
                }

                flags |= (short)((kv.Value.LowID == kv.Value.HighID) ? 2 : 1);
                var slot = new BankSlot();
                slot.Flags = flags;
                slot.Count = (short)kv.Value.MultipleCount;
                slot.Identity = kv.Value.Identity;
                slot.ItemLowId = kv.Value.LowID;
                slot.ItemHighId = kv.Value.HighID;
                slot.Quality = kv.Value.Quality;
                slot.ItemFlags = 0;
                slots.Add(slot);
            }

            return slots.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="slotFrom">
        /// </param>
        /// <param name="slotTo">
        /// </param>
        /// <param name="err">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void TryHotSwap(int slotFrom, int slotTo, InventoryError err)
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
            IItem toUnEquip = this[fromPlacement];
            receivingPage.Add(toPlacement, toUnEquip);
            this.Remove(fromPlacement);
        }

        /// <summary>
        /// </summary>
        /// <param name="slotNum">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool ValidSlot(int slotNum)
        {
            return (this.FirstSlotNumber <= slotNum) && (slotNum < this.FirstSlotNumber + this.MaxSlots);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Write()
        {
            List<DBInstancedItem> DBinstanced = new List<DBInstancedItem>();
            List<DBItem> DBuninstanced = new List<DBItem>();
            foreach (KeyValuePair<int, IItem> kv in this.Content)
            {
                if (kv.Value.Identity.Type != IdentityType.None)
                {
                    DBInstancedItem dbi = new DBInstancedItem
                                          {
                                              containerinstance = this.Identity.Instance,
                                              containertype = (int)this.Identity.Type,
                                              containerplacement = kv.Key,
                                              itemtype = (int)kv.Value.Identity.Type,
                                              Id = kv.Value.Identity.Instance,
                                              lowid = kv.Value.LowID,
                                              highid = kv.Value.HighID,
                                              quality = kv.Value.Quality,
                                              multiplecount = kv.Value.MultipleCount,
                                              stats = new Binary(kv.Value.GetItemAttributes())
                                          };

                    DBinstanced.Add(dbi);
                }
                else
                {
                    DBItem dbi = new DBItem
                                 {
                                     containerinstance = this.Identity.Instance,
                                     containertype = (int)this.Identity.Type,
                                     containerplacement = kv.Key,
                                     lowid = kv.Value.LowID,
                                     highid = kv.Value.HighID,
                                     quality = kv.Value.Quality,
                                     multiplecount = kv.Value.MultipleCount
                                 };

                    DBuninstanced.Add(dbi);
                }
            }

            ItemDao.Instance.Save(DBuninstanced);
            InstancedItemDao.Instance.Save(DBinstanced);
            return true;
        }

        #endregion
    }
}