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
// Last modified: 2013-11-01 21:05

#endregion

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    using Utility;

    #endregion

    /// <summary>
    /// Data access object for instanced items
    /// </summary>
    public static class InstancedItemDao
    {
        #region Public Methods and Operators

        /// <summary>
        /// Load all instanced items
        /// </summary>
        /// <returns>
        /// Collection of DBInstancedItem
        /// </returns>
        public static IEnumerable<DBInstancedItem> GetAll()
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return conn.Query<DBInstancedItem>("SELECT * FROM instanceditems");
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Load all instanced items in a specific container
        /// </summary>
        /// <param name="containertype">
        /// Type of the container
        /// </param>
        /// <param name="containerinstance">
        /// Instance of the container
        /// </param>
        /// <returns>
        /// Collection of DBInstancedItem
        /// </returns>
        public static IEnumerable<DBInstancedItem> GetAllInContainer(int containertype, int containerinstance)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBInstancedItem>(
                            "SELECT * FROM instanceditems WHERE containertype=@containertype AND containerinstance=@containerinstance", 
                            new { containertype, containerinstance });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Load instanced item by id
        /// </summary>
        /// <param name="itemid">
        /// id of the item to load
        /// </param>
        /// <returns>
        /// Collection of DBInstancedItem
        /// </returns>
        public static IEnumerable<DBInstancedItem> GetById(int itemid)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return conn.Query<DBInstancedItem>(
                        "SELECT * FROM instanceditems where id = @id", 
                        new { id = itemid });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
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
        /// slot in the container
        /// </param>
        public static void RemoveItem(int containertype, int containerinstance, int containerplacement)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "DELETE FROM instanceditems WHERE containertype=@containertype AND containerinstance=@containerinstance AND containerplacement=@containerplacement", 
                        new { containertype, containerinstance, containerplacement });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Insert a DBInstancedItem into table
        /// </summary>
        /// <param name="item">
        /// Item to write
        /// </param>
        public static void Save(DBInstancedItem item)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "INSERT INTO instanceditems (containertype,containerinstance,containerplacement,itemtype,iteminstance"
                        + ",lowid,highid,quality,multiplecount,x,y,z,headingx,headingy,headingz,headingw,stats) VALUES (@conttype,"
                        + " @continstance, @contplacement, @itype, @iinstance, @low, @high, @ql, @mc, @ix, @iy, @iz, @hx, @hy, @hz, @hw, @st)", 
                        new
                        {
                            conttype = item.containertype, 
                            continstance = item.containerinstance, 
                            contplacement = item.containerplacement, 
                            itype = item.itemtype, 
                            iinstance = item.iteminstance, 
                            low = item.lowid, 
                            high = item.highid, 
                            ql = item.quality, 
                            mc = item.multiplecount, 
                            ix = item.x, 
                            iy = item.y, 
                            iz = item.z, 
                            hx = item.headingx, 
                            hy = item.headingy, 
                            hz = item.headingz, 
                            hw = item.headingw, 
                            st = item.stats
                        });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Insert a list of DBInstancedItems to table
        /// </summary>
        /// <param name="items">
        /// List of DBInstancedItems
        /// </param>
        public static void Save(List<DBInstancedItem> items)
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
                            "DELETE FROM instanceditems WHERE containertype=@containertype AND containerinstance=@containerinstance", 
                            new { items[0].containertype, items[0].containerinstance }, 
                            transaction: trans);
                        foreach (DBInstancedItem item in items)
                        {
                            conn.Execute(
                                "INSERT INTO instanceditems (containertype,containerinstance,containerplacement,itemtype,iteminstance"
                                + ",lowid,highid,quality,multiplecount,x,y,z,headingx,headingy,headingz,headingw,stats) VALUES (@conttype,"
                                + " @continstance, @contplacement, @itype, @iinstance, @low, @high, @ql, @mc, @ix, @iy, @iz, @hx, @hy, @hz, @hw, @st)", 
                                new
                                {
                                    conttype = item.containertype, 
                                    continstance = item.containerinstance, 
                                    contplacement = item.containerplacement, 
                                    itype = item.itemtype, 
                                    iinstance = item.iteminstance, 
                                    low = item.lowid, 
                                    high = item.highid, 
                                    ql = item.quality, 
                                    mc = item.multiplecount, 
                                    ix = item.x, 
                                    iy = item.y, 
                                    iz = item.z, 
                                    hx = item.headingx, 
                                    hy = item.headingy, 
                                    hz = item.headingz, 
                                    hw = item.headingw, 
                                    st = item.stats
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
                throw;
            }
        }

        #endregion
    }
}