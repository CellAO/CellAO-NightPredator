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

    using System.Collections.Generic;

    using CellAO.Core.Items;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class OutgoingTradeInventoryPage : BaseInventoryPage
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="ownerInstance">
        /// </param>
        public OutgoingTradeInventoryPage(Identity ownerInstance, int maxSlots)
            : base((int)IdentityType.TradeWindow, maxSlots, 0, ownerInstance)
        {
        }

        private Dictionary<int, int> Content = new Dictionary<int, int>();

        public void Add(int slot)
        {
            if (Content.ContainsKey(slot))
            {
                Content[slot]++;
            }
            else
            {
                Content.Add(slot, 1);
            }
        }

        public new void Remove(int slot)
        {
            if (Content.ContainsKey(slot))
            {
                Content[slot]--;
                if (Content[slot] == 0)
                {
                    Content.Remove(slot);
                }
            }
        }

        public IItem[] GetItems(IInventoryPage page)
        {
            List<IItem> result = new List<IItem>();
            foreach (KeyValuePair<int, int> kv in this.Content)
            {
                IItem item = page[kv.Key];
                if (ItemLoader.ItemList[item.LowID].IsStackable())
                {
                    item.MultipleCount *= kv.Value;
                    result.Add(item);
                }
                else
                {
                    for (int i = 0; i < kv.Value; i++)
                    {
                        result.Add(item);
                    }
                }
            }
            return result.ToArray();
        }

        #endregion
    }
}