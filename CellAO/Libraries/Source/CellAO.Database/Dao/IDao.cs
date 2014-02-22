using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CellAO.Database.Entities;
using Dapper;

namespace CellAO.Database.Dao
{
    public interface IDao<T> where T : IDBEntity
    {
        int Add(T dbentity);

        int Save(T dbentity, DynamicParameters parameters = null);

        bool Exists(int entityId);

        void Delete(int entityId); 

        T Get(int entityId);

        IEnumerable<T> GetAll(DynamicParameters parameters = null);

    }
}
