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
// Last modified: 2013-11-16 10:27

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
    public static class StatDao
    {
        #region Public Methods and Operators

        /// <summary>
        /// Add a Stat to table
        /// </summary>
        /// <param name="type">
        /// Type id of the owner
        /// </param>
        /// <param name="instance">
        /// Instance of the owner
        /// </param>
        /// <param name="num">
        /// Stat id number
        /// </param>
        /// <param name="value">
        /// Value of the stat
        /// </param>
        public static void AddStat(int type, int instance, int num, int value)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "REPLACE INTO stats (type, instance, statid, statvalue) VALUES (@t, @i, @statid, @statvalue)", 
                        new { t = type, i = instance, statid = num, statvalue = value });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Bulk insert/replace of stats
        /// </summary>
        /// <param name="stats">
        /// List of DBStats
        /// </param>
        public static void BulkReplace(List<DBStats> stats)
        {
            try
            {
                // Delete all stats before writing
                DeleteStats(stats[0].type, stats[0].instance);

                using (IDbConnection conn = Connector.GetConnection())
                {
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        conn.Execute(
                            "INSERT INTO stats (type, instance, statid, statvalue) VALUES (@type, @instance, @statid, @statvalue)", 
                            stats, 
                            transaction: trans);
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

        /// <summary>
        /// Delete stats
        /// </summary>
        /// <param name="type">
        /// Type id of the owner
        /// </param>
        /// <param name="instance">
        /// Instance of the owner
        /// </param>
        public static void DeleteStats(int type, int instance)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute("DELETE FROM stats WHERE type=@type AND instance=@instance", new { type, instance });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
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
        /// Get all Stats
        /// </summary>
        /// <returns>
        /// Collection of DBStats
        /// </returns>
        public static IEnumerable<DBStats> GetAll()
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBStats>(
                            "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters");
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
        public static DBStats GetById(int type, int instance, int statId)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBStats>(
                            "SELECT statid, statvalue FROM stats where (type=@type AND instance=@instance AND statid=@statId)", 
                            new { type, instance, statId }).First();
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                // Log and return an empty stat entry
                // TODO: Get a DEFAULT value for the stat
                return new DBStats { type = type, instance = instance, statid = statId, statvalue = 0 };
            }
        }

        /// <summary>
        /// Get list of stats by character type/instance
        /// </summary>
        /// <param name="type">
        /// Type id of the character
        /// </param>
        /// <param name="instance">
        /// Instance of the character
        /// </param>
        /// <returns>
        /// Collection of DBStats
        /// </returns>
        public static IEnumerable<DBStats> GetById(int type, int instance)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBStats>(
                            "SELECT statid, statvalue FROM stats where (type=@type AND instance=@instance)", 
                            new { type, instance });
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