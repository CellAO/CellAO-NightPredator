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

namespace ZoneEngine.Core
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Items;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    #endregion

    /// <summary>
    /// </summary>
    public class TradeSkill
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static TradeSkill instance;

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        public Dictionary<int, string> ItemNames = new Dictionary<int, string>();

        /// <summary>
        /// </summary>
        private readonly List<TradeSkillEntry> tradeSkillList = new List<TradeSkillEntry>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        static TradeSkill()
        {
        }

        /// <summary>
        /// </summary>
        private TradeSkill()
        {
            this.CacheItemNames();
            Console.WriteLine("Cached " + this.ItemNames.Count + " item names");
            this.CacheTradeSkills();
            Console.WriteLine("\rCached " + this.tradeSkillList.Count + " trade skill entries");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public static TradeSkill Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TradeSkill();
                }

                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="lid">
        /// </param>
        /// <param name="hid">
        /// </param>
        /// <param name="ql">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetItemName(int lid, int hid, int ql)
        {
            try
            {
                string lName = this.ItemNames[lid];
                string hName = this.ItemNames[hid];

                int lQL = ItemLoader.ItemList[lid].Quality;
                int hQL = ItemLoader.ItemList[hid].Quality;

                if (ql > (hQL - lQL) / 2 + lQL)
                {
                    return hName;
                }
                else
                {
                    return lName;
                }
            }
            catch (Exception)
            {
                return "NoName";
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="id1">
        /// </param>
        /// <param name="id2">
        /// </param>
        /// <returns>
        /// </returns>
        public TradeSkillEntry GetTradeSkillEntry(int id1, int id2)
        {
            return this.tradeSkillList.FirstOrDefault(x => (x.ID1 == id1) && (x.ID2 == id2));
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        public int SourceProcessesCount(int id)
        {
            return this.tradeSkillList.Count(x => x.ID1 == id);
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        public int TargetProcessesCount(int id)
        {
            return this.tradeSkillList.Count(x => x.ID2 == id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void CacheItemNames()
        {
            foreach (DBItemName itemName in ItemNamesDao.Instance.GetAll())
            {
                this.ItemNames.Add(itemName.Id, itemName.Name);
            }
        }

        /// <summary>
        /// </summary>
        private void CacheTradeSkills()
        {
            int i = 0;
            this.tradeSkillList.Clear();
            foreach (DBTradeSkill tradeSkill in TradeSkillDao.Instance.GetAll())
            {
                this.tradeSkillList.Add(TradeSkillEntry.ConvertFromDB(tradeSkill));
                i++;
                if ((i % 1000) == 0)
                {
                    Console.Write("\rCached {0} trade skill entries", i);
                }
            }
        }

        #endregion
    }
}