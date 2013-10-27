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
// Last modified: 2013-10-27 11:38
// Created:       2013-10-27 07:58

#endregion

namespace CellAO.Database
{
    #region Usings ...

    using System;
    using System.Data;

    using CellAO.Interfaces;

    using Utility.Config;

    #endregion

    /// <summary>
    /// </summary>
    public static class Connector
    {
        /// <summary>
        /// </summary>
        public static string Sqltype = ConfigReadWrite.Instance.CurrentConfig.SQLType;

        /// <summary>
        /// only needed once to read this
        /// </summary>
        private static readonly string ConnectionString_MySQL = ConfigReadWrite.Instance.CurrentConfig.MysqlConnection;

        /// <summary>
        /// </summary>
        private static readonly string ConnectionString_MSSQL = ConfigReadWrite.Instance.CurrentConfig.MsSqlConnection;

        /// <summary>
        /// </summary>
        private static readonly string ConnectionString_PostGreSQL =
            ConfigReadWrite.Instance.CurrentConfig.PostgreConnection;

        private static IDatabaseConnector connector;

        // CONNECTION POOLING IS A MUST!!!
        // TODO: Rewrite needed for config.xml, only providing username, password and database. Create connection string via stringbuilders
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static IDbConnection GetConnection()
        {
            IDbConnection conn = null;
            if (connector == null)
            {
                if (Sqltype == "MySql")
                {
                    connector = new MySQLConnector(ConnectionString_MySQL);
                }

                if (Sqltype == "MsSql")
                {
                    connector = new MSSqlConnector(ConnectionString_MSSQL);
                }

                if (Sqltype == "PostgreSQL")
                {
                    connector = new NpgsqlConnector(ConnectionString_PostGreSQL);
                }
            }

            if (connector == null)
            {
                throw new Exception("Could not determine your database");
            }

            conn = connector.GetConnection();

            if (conn == null)
            {
                throw new Exception("ConnectionString error");
            }

            conn.Open();
            return conn;
        }
    }
}