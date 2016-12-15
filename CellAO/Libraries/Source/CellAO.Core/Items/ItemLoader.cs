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

namespace CellAO.Core.Items
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using locales;

    using Utility;

    #endregion

    /// <summary>
    /// Item template Loader/Cache class
    /// </summary>
    public static class ItemLoader
    {
        #region Static Fields

        /// <summary>
        /// Cache of all item templates
        /// </summary>
        public static Dictionary<int, ItemTemplate> ItemList = new Dictionary<int, ItemTemplate>(130000);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Cache all item templates
        /// </summary>
        /// <returns>number of cached items</returns>
        public static int CacheAllItems()
        {
            return CacheAllItems("items.dat");
        }

        /// <summary>
        /// Cache all item templates
        /// </summary>
        /// <param name="fname">
        /// File to load from
        /// </param>
        /// <returns>
        /// Number of cached items
        /// </returns>
        public static int CacheAllItems(string fname)
        {
            Contract.Requires(!string.IsNullOrEmpty(fname));
            DateTime _now = DateTime.UtcNow;

            MessagePackZip.UncompressData<ItemTemplate>(fname).ForEach(x => ItemList.Add(x.ID, x));

            Console.WriteLine(
                locales.ItemLoaderLoadedItems + " - {1}\r",
                new object[] { ItemList.Count, new DateTime((DateTime.UtcNow - _now).Ticks).ToString("mm:ss.ff") });

            GC.Collect();
            return ItemList.Count;
        }

        #endregion
    }
}