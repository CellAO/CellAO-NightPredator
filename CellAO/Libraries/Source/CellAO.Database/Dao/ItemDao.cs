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
    using System.Runtime.CompilerServices;

    using Dapper;

    using Utility;

    #endregion

    /// <summary>
    /// Data access object for Items (not instanced)
    /// </summary>
    public class ItemDao : Dao<DBItem>
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public static ItemDao Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemDao();
                    _instance.TableName = getTablename();
                }

                return (ItemDao)_instance;
            }
        }


        #endregion

        #region Public Methods and Operators

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
        public DBItem ReadItem(int containerType, int containerInstance, int containerPlacement)
        {
            return
                Instance.GetAll(

                        new
                        {
                            containertype = containerType,
                            containerinstance = containerInstance,
                            containerplacement = containerPlacement
                        }).FirstOrDefault();
        }

        /// <summary>
        /// Remove item from table
        /// </summary>
        /// <param name="containerType">
        /// </param>
        /// <param name="containerInstance">
        /// </param>
        /// <param name="containerPlacement">
        /// </param>
        public void RemoveItem(int containerType, int containerInstance, int containerPlacement)
        {
            int rowsAffected =
                Instance.Delete(
                        new
                        {
                            containertype = containerType,
                            containerinstance = containerInstance,
                            containerplacement = containerPlacement
                        });
        }

        /// <summary>
        /// Insert one DBItem into table
        /// </summary>
        /// <param name="item">
        /// DBItem to write
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        public void Save(DBItem item, IDbConnection connection, IDbTransaction transaction)
        {
            int affectedRows = ItemDao.Instance.Save(item, null, connection, transaction);
            // Check for 0 rows?
        }

        /// <summary>
        /// Write List of DBItem into table
        /// </summary>
        /// <param name="items">
        /// List of DBItem
        /// </param>
        public void Save(List<DBItem> items, IDbConnection connection, IDbTransaction transaction)
        {
            if (items.Count > 0)
            {

                using (IDbConnection conn = Connector.GetConnection(connection))
                {
                    using (IDbTransaction trans = transaction ?? conn.BeginTransaction())
                    {
                        foreach (DBItem item in items)
                        {
                            Instance.Delete(
                                    new
                                    {
                                        items[0].containertype,
                                        items[0].containerinstance,
                                        items[0].Id
                                    },
                                connection,
                                transaction);
                            Instance.Save(item, new { item.containertype, item.containerinstance, item.Id }, connection, transaction);
                        }

                        trans.Commit();
                    }
                }
            }
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
        public IEnumerable<DBItem> GetAllInContainer(int containerType, int containerInstance)
        {
            return
                Instance.GetAll(
                    new { containertype = containerType, containerinstance = containerInstance });
        }

        #endregion
    }
}