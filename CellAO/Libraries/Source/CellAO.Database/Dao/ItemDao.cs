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

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Dapper;

    using Utility;

    #endregion

    /// <summary>
    /// Data access object for Items (not instanced)
    /// </summary>
    public static class ItemDao
    {
        #region Public Methods and Operators

        /// <summary>
        /// Load all items from table
        /// </summary>
        /// <returns>
        /// Collection of DBItem
        /// </returns>
        public static IEnumerable<DBItem> GetAll()
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return conn.Query<DBItem>("SELECT * FROM items");
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }

            return new List<DBItem>();
        }

        /// <summary>
        /// Load all items of a specific container
        /// </summary>
        /// <param name="containerType">
        /// Type of the container
        /// </param>
        /// <param name="containerInstance">
        /// Instance of the container
        /// </param>
        /// <returns>
        /// Collection of DBItem
        /// </returns>
        public static IEnumerable<DBItem> GetAllInContainer(int containerType, int containerInstance)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBItem>(
                            "SELECT * FROM items WHERE containertype=@containerType AND containerinstance=@containerInstance", 
                            new { containerType, containerInstance });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Load a specific item
        /// </summary>
        /// <param name="containerType">
        /// Type of the container
        /// </param>
        /// <param name="containerInstance">
        /// Instance of the container
        /// </param>
        /// <param name="containerPlacement">
        /// Slot of the item
        /// </param>
        /// <returns>
        /// DBItem object
        /// </returns>
        public static DBItem ReadItem(int containerType, int containerInstance, int containerPlacement)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBItem>(
                            "SELECT * FROM items WHERE containertype=@containerType AND containerinstance=@containerInstance AND containerplacement=@containerPlacement", 
                            new { containerType, containerInstance, containerPlacement }).Single();
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }

            return null;
        }

        /// <summary>
        /// Remove item from table
        /// </summary>
        /// <param name="containertype">
        /// Type of the container
        /// </param>
        /// <param name="containerinstance">
        /// Instance of the container
        /// </param>
        /// <param name="containerplacement">
        /// Slot of the item
        /// </param>
        public static void RemoveItem(int containertype, int containerinstance, int containerplacement)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "DELETE FROM items WHERE containertype=@containertype AND containerinstance=@containerinstance AND containerplacement=@containerplacement", 
                        new { containertype, containerinstance, containerplacement });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        /// <summary>
        /// Insert one DBItem into tabl
        /// </summary>
        /// <param name="item">
        /// DBItem to write
        /// </param>
        public static void Save(DBItem item)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "REPLACE INTO items VALUES (@containerType, @containerInstance, @containerPlacement, @lowid, @highid, @quality, @multiplecount)", 
                        new { item });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        /// <summary>
        /// Write List of DBItem into table
        /// </summary>
        /// <param name="items">
        /// List of DBItem
        /// </param>
        public static void Save(List<DBItem> items)
        {
            if (items.Count == 0)
            {
                return;
            }

            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        conn.Execute(
                            "DELETE FROM items WHERE containertype=@containertype AND containerinstance=@containerinstance", 
                            new { items[0].containertype, items[0].containerinstance }, 
                            transaction: trans);
                        foreach (DBItem item in items)
                        {
                            conn.Execute(
                                "INSERT INTO items (containertype,containerinstance,containerplacement"
                                + ",lowid,highid,quality,multiplecount) VALUES (@conttype,"
                                + " @continstance, @contplacement, @low, @high, @ql, @mc)", 
                                new
                                {
                                    conttype = item.containertype, 
                                    continstance = item.containerinstance, 
                                    contplacement = item.containerplacement, 
                                    low = item.lowid, 
                                    high = item.highid, 
                                    ql = item.quality, 
                                    mc = item.multiplecount, 
                                }, 
                                transaction: trans);
                        }

                        trans.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        #endregion
    }
}