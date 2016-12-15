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
    /// Data access object for Stats
    /// </summary>
    public class StatDao : Dao<DBStats, StatDao>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Bulk insert/replace of stats
        /// </summary>
        /// <param name="stats">
        /// List of DBStats
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        public void BulkReplace(List<DBStats> stats, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            IDbConnection conn = connection;
            try
            {
                conn = conn ?? Connector.GetConnection();
                IDbTransaction trans = transaction;
                try
                {
                    trans = trans ?? conn.BeginTransaction();

                    // Do it in one transaction, so no stats can be lost
                    this.Delete(new { Type = stats[0].Type, Instance = stats[0].Instance }, conn, trans);
                    foreach (DBStats stat in stats)
                    {
                        this.Add(stat, conn, trans);
                    }
                }
                finally
                {
                    if (transaction == null)
                    {
                        if (trans != null)
                        {
                            trans.Commit();
                            trans.Dispose();
                        }
                    }
                }
            }
            finally
            {
                if (connection == null)
                {
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Disband an organization
        /// </summary>
        /// <param name="orgId">
        /// Id of the organization
        /// </param>
        public static void DisbandOrganization(int orgId)
        {
            // This only takes care of the offline characters. Characters currently online have to be notified as well (setting their stats + message)
            // This also does not belong here

            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute("UPDATE stats SET statvalue=0 WHERE statid=5 AND statvalue=@orgId", new { orgId });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Get one stat from a particular character
        /// </summary>
        /// <param name="type">
        /// Type id of the character
        /// </param>
        /// <param name="instance">
        /// instance of the character
        /// </param>
        /// <param name="statId">
        /// Stat number
        /// </param>
        /// <returns>
        /// DBStats object
        /// </returns>
        public DBStats GetById(int type, int instance, int statId)
        {
            // Return stat or new DBStat with value of 0
            return Instance.GetAll(new { Type = type, Instance = instance, StatId = statId }).FirstOrDefault()
                   ?? new DBStats { Type = type, Instance = instance, StatId = statId, StatValue = 0 };
        }

        #endregion
    }
}