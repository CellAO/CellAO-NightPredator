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
    using System.Linq;

    using Dapper;

    using Utility;

    #endregion

    /// <summary>
    /// Data access object for LoginData
    /// </summary>
    public static class LoginDataDao
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get all data from login table
        /// </summary>
        /// <returns>
        /// Collection of DBLoginData
        /// </returns>
        public static IEnumerable<DBLoginData> GetAll()
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return conn.Query<DBLoginData>("SELECT * FROM login");
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Get login data by username
        /// </summary>
        /// <param name="username">
        /// Name of the user
        /// </param>
        /// <returns>
        /// DBLogindata object
        /// </returns>
        public static DBLoginData GetByUsername(string username)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBLoginData>("SELECT * FROM login where Username=@user", new { user = username })
                            .First();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Write login data to table
        /// </summary>
        /// <param name="login">
        /// Login data to write
        /// </param>
        public static void WriteLoginData(DBLoginData login)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "INSERT INTO login (CreationDate, Email, FirstName, LastName, Username, Password, Allowed_Characters, Flags, AccountFlags, Expansions, GM) VALUES (@creationdate, @email, @firstname, @lastname,@username, @password, @allowed_characters, @flags, @accountflags, @expansions, @gm)", 
                        new
                        {
                            creationdate = DateTime.Now, 
                            email = login.Email, 
                            firstname = login.FirstName, 
                            lastname = login.LastName, 
                            username = login.Username, 
                            password = login.Password, 
                            allowed_characters = login.Allowed_Characters, 
                            flags = login.Flags, 
                            accountflags = login.AccountFlags, 
                            expansions = login.Expansions, 
                            gm = login.GM
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
        /// Write new password to table
        /// </summary>
        /// <param name="login">
        /// DBLoginData object
        /// </param>
        public static void WriteNewPassword(DBLoginData login)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "UPDATE login SET password=@pwd WHERE Username=@user", 
                        new { pwd = login.Password, user = login.Username });
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