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
    using System.Data;
    using System.Linq;

    using Dapper;

    using Utility;

    #endregion

    /// <summary>
    /// Data access object to check online status of characters
    /// </summary>
    public static class OnlineDao
    {
        #region Public Methods and Operators

        /// <summary>
        /// Check if character (id) is online
        /// </summary>
        /// <param name="id">
        /// Id of the character
        /// </param>
        /// <returns>
        /// DBOnline object
        /// </returns>
        public static DBOnline IsOnline(int id)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBOnline>("SELECT Online from characters WHERE id=@charid", new { charid = id })
                            .First();
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }

            return new DBOnline { Online = 0 };
        }

        /// <summary>
        /// Set online flag in table
        /// </summary>
        /// <param name="id">
        /// Id of the character
        /// </param>
        public static void SetOnline(int id)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute("UPDATE characters SET online=1 WHERE id=@charid", new { charid = id });
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