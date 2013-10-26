using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Database
{
    #region Usings ...

    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Utility.Config;

    using MySql.Data.MySqlClient;

    using Npgsql;

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
            if (Sqltype == "MySql")
            {
                conn = new MySqlConnection(ConnectionString_MySQL);
            }

            if (Sqltype == "MsSql")
            {
                conn = new SqlConnection(ConnectionString_MSSQL);
            }

            if (Sqltype == "PostgreSQL")
            {
                conn = new NpgsqlConnection(ConnectionString_PostGreSQL);
            }

            if (conn == null)
            {
                throw new Exception("ConnectionString error");
            }

            conn.Open();
            return conn;
        }
    }
}