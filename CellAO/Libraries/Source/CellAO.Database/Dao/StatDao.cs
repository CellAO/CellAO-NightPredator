#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-10-30 22:52
// Created:       2013-10-30 17:25

#endregion

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Dapper;

    #endregion

    /// <summary>
    /// </summary>
    public static class StatDao
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static IEnumerable<DBStats> GetAll()
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBStats>(
                        "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<DBStats> GetById(int characterId)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBStats>(
                        "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters where id = @id",
                        new { id = characterId });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="instance">
        /// </param>
        /// <param name="statId">
        /// </param>
        /// <returns>
        /// </returns>
        public static DBStats GetById(int type, int instance, int statId)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBStats>(
                        "SELECT statid, statvalue FROM stats where (type=@type AND instance=@instance AND statid=@statId)",
                        new { type, instance, statId }).First();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="instance">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<DBStats> GetById(int type, int instance)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBStats>(
                        "SELECT statid, statvalue FROM stats where (type=@type AND instance=@instance)",
                        new { type, instance });
            }
        }

        public static void DeleteStats(int type, int instance)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute("DELETE FROM stats WHERE type=@type AND instance=@instance", new { type, instance });
            }
        }

        public static void AddStat(int type, int instance, int num, int value)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute(
                    "REPLACE INTO stats (type, instance, statid, statvalue) VALUES (@t, @i, @statid, @statvalue)",
                    new { t = type, i = instance, statid = num, statvalue = value });
            }
        }

        public static void BulkReplace(List<DBStats> stats)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    conn.Execute(
                        "DELETE FROM stats WHERE type=@type AND instance=@instance",
                        new { stats[0].type, stats[0].instance },
                        transaction: trans);
                    conn.Execute(
                        "REPLACE INTO stats (type, instance, statid, statvalue) VALUES (@type, @instance, @statid, @statvalue)",
                        stats,
                        transaction: trans);
                    trans.Commit();
                }
            }
        }

        public static void DisbandOrganization(int orgId)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute("UPDATE stats SET statvalue=0 WHERE statid=5 AND statvalue=@orgId", new { orgId });
            }
        }
    }
}