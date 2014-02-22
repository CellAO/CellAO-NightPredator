using CellAO.Database.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using CellAO.Core.Exceptions;

namespace CellAO.Database.Dao
{
    public class Dao<T> : IDao<T> where T : IDBEntity, new()
    {
        public string TableName { get; set; }

        #region Singleton

        static Dao<T> instance = default(Dao<T>);
        
        public static Dao<T> Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dao<T>();

                return instance;
            }
        }

        #endregion

        #region Reflection Cache

        private static Dictionary<string, PropertyInfo> cachedProperties = null;
        protected static Dictionary<string, PropertyInfo> CachedProperties
        {
            get
            {
                if (cachedProperties == null)
                {
                    lock (cachedProperties)
                    {

                    }
                }
                return cachedProperties;
            }
        }

        DynamicParameters getAllParameters()
        {
            DynamicParameters parameters = new DynamicParameters(this);
            foreach (string propertyName in CachedProperties.Keys)
            {
                parameters.Add(propertyName, CachedProperties[propertyName].GetValue(this, null));
            }
            return parameters;
        }

        #endregion

        #region CRUD

        public int Add(T entity)
        {
            int rowsAffected = 0;

            using (IDbConnection conn = Connector.GetConnection())
            {
                rowsAffected = conn.Execute(
                    SqlMapperUtil.CreateInsertSQL(this.TableName, getAllParameters())
                 );

                // we must retrive the Id anyway here. we need to standardise the id as we started to do.
                if (rowsAffected == 1)
                    SqlMapperUtil.SetIdentity<int>(conn, id => entity.Id = id);
                else
                    throw new DataBaseException(string.Format("Failed to create new record on table '{0}'", this.TableName));
            }

            return rowsAffected;
        }

        public int Save(T entity, DynamicParameters parameters = null)
        {
            int rowsAffected = 0;

            using (IDbConnection conn = Connector.GetConnection())
            {
                rowsAffected = conn.Execute(SqlMapperUtil.CreateUpdateSQL(this.TableName, (parameters != null) ? parameters : getAllParameters()));

            }

            return rowsAffected;
        }

        public bool Exists(int entityId)
        {
            bool exists = false;
            using (IDbConnection conn = Connector.GetConnection())
            {
                exists = conn.Query<int>(string.Format("SELECT ID FROM {0} where ID = @id", this.TableName), new { id = entityId }).Count() == 1;
            }
            return exists;
        }

        public void Delete(int entityId)
        {

            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute(SqlMapperUtil.CreateDeleteSQL(this.TableName), new { id = entityId });
            }

        }

        /// <summary>
        /// Load a Character by id
        /// </summary>
        /// <param name="entityId">
        /// Id of the Character
        /// </param>
        /// <returns>
        /// DBCharacter object
        /// </returns>
        public T Get(int entityId)
        {
            T entity = default(T);
            using (IDbConnection conn = Connector.GetConnection())
            {
                DynamicParameters whereParameters = new DynamicParameters();
                whereParameters.Add("Id", entityId);
                entity =
                    conn.Query<T>(
                        SqlMapperUtil.CreateGetSQL(this.TableName, whereParameters),
                        whereParameters).SingleOrDefault();
            }
            return entity;
        }

        /// <summary>
        /// Load all Character data
        /// </summary>
        /// <returns>
        /// Collection of DBCharacter
        /// </returns>
        public IEnumerable<T> GetAll(DynamicParameters parameters = null)
        {
            IEnumerable<T> entities = null;
            using (IDbConnection conn = Connector.GetConnection())
            {
                entities =
                    conn.Query<T>(
                        SqlMapperUtil.CreateGetSQL(this.TableName, parameters),
                        (parameters != null ? parameters : null)
                    );
            }
            return entities;
        }

        #endregion

    }
}
