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

namespace CellAO.Database
{
    #region Usings ...

    using System.Data;

    using CellAO.Core.Exceptions;
    using CellAO.Interfaces;

    using Utility.Config;

    #endregion

    /// <summary>
    /// Main hub for database connections
    /// </summary>
    public static class Connector
    {
        #region Static Fields

        /// <summary>
        /// Connection string for ms sql
        /// </summary>
        private static readonly string ConnectionStringMssql = ConfigReadWrite.Instance.CurrentConfig.MsSqlConnection;

        /// <summary>
        /// Connection string for mysql
        /// </summary>
        private static readonly string ConnectionStringMySql = ConfigReadWrite.Instance.CurrentConfig.MysqlConnection;

        /// <summary>
        /// Connection string for Postgresql
        /// </summary>
        private static readonly string ConnectionStringPostGreSql =
            ConfigReadWrite.Instance.CurrentConfig.PostgreConnection;

        /// <summary>
        /// Database connector
        /// </summary>
        private static IDatabaseConnector connector;

        /// <summary>
        /// Type of SQL from config file
        /// </summary>
        private static string sqlType = ConfigReadWrite.Instance.CurrentConfig.SQLType;

        #endregion

        // CONNECTION POOLING IS A MUST!!!
        // TODO: Rewrite needed for config.xml, only providing username, password and database. Create connection string via stringbuilders

        #region Public Methods and Operators

        /// <summary>
        /// Get IDbConnection depending on configuration file
        /// </summary>
        /// <param name="existingConnection">
        /// </param>
        /// <returns>
        /// IDbConnection to the database
        /// </returns>
        /// <exception cref="DatabaseCouldNotBeDeterminedException">
        /// Database could not be determined (check config.xml)
        /// </exception>
        /// <exception cref="ConnectionStringErrorException">
        /// Connection could not be established (check config.xml)
        /// </exception>
        public static IDbConnection GetConnection()
        {
            IDbConnection conn = null;
            if (connector == null)
            {
                if (sqlType == "MySql")
                {
                    connector = new MySQLConnector(ConnectionStringMySql);
                }

                if (sqlType == "MsSql")
                {
                    connector = new MSSqlConnector(ConnectionStringMssql);
                }

                if (sqlType == "PostgreSQL")
                {
                    connector = new NpgsqlConnector(ConnectionStringPostGreSql);
                }
            }

            if (connector == null)
            {
                throw new DatabaseCouldNotBeDeterminedException("Could not determine your database");
            }

            conn = connector.GetConnection();

            if (conn == null)
            {
                throw new ConnectionStringErrorException("ConnectionString error");
            }


            conn.Open();

            return conn;
        }

        #endregion
    }
}