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

namespace CellAO.Database
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using CellAO.Database.Entities;

    using Dapper;

    using Utility.Config;

    #endregion

    public static class SqlMapperUtil
    {
        private static string scopeIdentity = string.Empty;

        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities) where T : class, new()
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                int records = 0;

                foreach (T entity in entities)
                {
                    records += conn.Execute(sql, entity);
                }

                return records;
            }
        }

        public static DynamicParameters GetParametersFromObject(
            object obj,
            string[] propertyNamesToIgnore,
            bool removeForeignKeys)
        {
            if (propertyNamesToIgnore == null)
            {
                propertyNamesToIgnore = new string[] { String.Empty };
            }
            DynamicParameters p = new DynamicParameters();
            PropertyInfo[] properties =
                obj.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(
                        x => (!x.GetCustomAttributes(typeof(ForeignKeyAttribute), false).Any() || !removeForeignKeys))
                    .ToArray();

            foreach (PropertyInfo prop in properties)
            {
                if (!propertyNamesToIgnore.Contains(prop.Name))
                {
                    p.Add("@" + prop.Name, prop.GetValue(obj, null));
                }
            }
            return p;
        }

        public static void SetIdentity<T>(IDbConnection connection, Action<T> setId, IDbTransaction transaction = null)
        {
            if (scopeIdentity == string.Empty)
            {
                scopeIdentity = ConfigReadWrite.Instance.CurrentConfig.SQLType.ToLower() == "mysql"
                    ? "LAST_INSERT_ID()"
                    : ConfigReadWrite.Instance.CurrentConfig.SQLType.ToLower() == "mssql"
                        ? "@@SCOPE_IDENTITY"
                        : "LASTVAL()";
            }

            dynamic identity =
                connection.Query(string.Concat("SELECT ", scopeIdentity, " AS Id"), transaction: transaction).Single();
            T newId = (T)identity.Id;
            setId(newId);
        }

        public static object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object theValue = null;
            foreach (PropertyInfo prop in properties)
            {
                if (string.Compare(prop.Name, propertyName, true) == 0)
                {
                    theValue = prop.GetValue(target, null);
                }
            }
            return theValue;
        }

        public static void SetPropertyValue(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
            {
                return;
            }
            if (!info.CanWrite)
            {
                return;
            }
            info.SetValue(p, value, null);
        }

        /// <summary>
        /// Stored proc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> StoredProcWithParams<T>(string procname, dynamic parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return conn.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Stored proc insert with ID.
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <typeparam name="U">The Type of the ID</typeparam>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parms">instance of DynamicParameters class. This should include a defined output parameter</param>
        /// <returns>U - the @@Identity value from output parameter</returns>
        public static U StoredProcInsertWithID<T, U>(string procName, DynamicParameters parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                int x = conn.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);

                return parms.Get<U>("@ID");
            }
        }

        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> SqlWithParams<T>(string sql, dynamic parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return conn.Query<T>(sql, (object)parms).ToList();
            }
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql(string sql, dynamic parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return conn.Execute(sql, (object)parms);
            }
        }

        /// <summary>
        /// Insert update or delete stored proc.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteStoredProc(string procName, dynamic parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return conn.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }

        public static T SqlWithParamsSingle<T>(string sql, dynamic parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return conn.Query<T>(sql, (object)parms).FirstOrDefault();
            }
        }

        public static T StoredProcWithParamsSingle<T>(string procname, dynamic parms)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                T x = conn.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).SingleOrDefault();

                return x;
            }
        }

        #region Custom for CellAO DAO

        public static string CreateUpdateSQL(string tablename, object parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Cannot create Update SQL statement without parameters");
            }

            StringBuilder sb = new StringBuilder(string.Concat("UPDATE ", tablename, " SET "));
            foreach (string pname in GetParametersFromObject(parameters, null, true).ParameterNames)
            {
                if (pname.ToLower() != "id")
                {
                    sb.AppendFormat("{0} = @{0},", pname);
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE id=@id ");
            return sb.ToString();
        }

        public static string CreateInsertSQL(string tablename, object parameters, bool dontUseId = true)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Cannot create Insert SQL statement without parameters");
            }

            StringBuilder sb = new StringBuilder(string.Concat("INSERT INTO ", tablename, " ( "));
            foreach (string pname in GetParametersFromObject(parameters, null, false).ParameterNames)
            {
                if ((pname.ToLower() != "id") || !dontUseId)
                {
                    sb.AppendFormat("{0},", pname);
                }
            }
            sb.Remove(sb.Length - 1, 1); //  remove last ','
            sb.Append(" ) VALUES ( ");
            foreach (string pname in GetParametersFromObject(parameters, null, false).ParameterNames)
            {
                if ((pname.ToLower() != "id") || !dontUseId)
                {
                    sb.AppendFormat("@{0},", pname);
                }
            }
            sb.Remove(sb.Length - 1, 1); //  remove last ','
            sb.Append(" ) ");
            return sb.ToString();
        }

        public static string CreateDeleteSQL(string tablename, object whereParameters = null)
        {
            StringBuilder sb = new StringBuilder(string.Format("DELETE FROM {0}", tablename));
            if (whereParameters == null)
            {
                sb.Append(" WHERE Id = @Id ");
            }
            else
            {
                sb.Append(" WHERE ");
                foreach (string pname in GetParametersFromObject(whereParameters, null, false).ParameterNames)
                {
                    sb.AppendFormat(" ( {0} = @{0} ) AND", pname);
                    // AND *NO* WE WONT DO THE OR, XOR OR WHATEVER OTHER OPERATOR, SO DO NOT ASK :)
                }
                sb.Remove(sb.Length - 3, 3); //  remove trailing 'AND'
            }
            return sb.ToString();
        }

        public static string CreateGetSQL(string tablename, object whereParameters = null)
        {
            StringBuilder sb =
                new StringBuilder(
                    string.Concat("SELECT * FROM ", tablename, (whereParameters != null) ? " WHERE " : String.Empty));
            if (whereParameters != null)
            {
                foreach (string pname in GetParametersFromObject(whereParameters, null, false).ParameterNames)
                {
                    sb.AppendFormat(" ( {0} = @{0} ) AND", pname);
                    // AND *NO* WE WONT DO THE OR, XOR OR WHATEVER OTHER OPERATOR, SO DO NOT ASK :)
                }
                sb.Remove(sb.Length - 3, 3); //  remove trailing 'AND'
            }
            return sb.ToString();
        }

        public static string CreateCountSQL(string tablename, object whereParameters = null)
        {
            StringBuilder sb =
                new StringBuilder(
                    string.Concat(
                        "SELECT COUNT(*) FROM ",
                        tablename,
                        (whereParameters != null) ? " WHERE " : String.Empty));
            if (whereParameters != null)
            {
                foreach (string pname in GetParametersFromObject(whereParameters, null, false).ParameterNames)
                {
                    sb.AppendFormat(" ( {0} = @{0} ) AND", pname);
                    // AND *NO* WE WONT DO THE OR, XOR OR WHATEVER OTHER OPERATOR, SO DO NOT ASK :)
                }
                sb.Remove(sb.Length - 3, 3); //  remove trailing 'AND'
            }
            return sb.ToString();
        }

        #endregion
    }
}