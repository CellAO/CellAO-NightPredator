using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Database.Dao
{
    using CellAO.Database.Entities;

    public class MobSpawnStatDao : Dao<DBMobSpawnStat, MobSpawnStatDao>
    {
        public int Add(DBMobSpawnStat entity)
        {
            return Add(entity, null, null, false);
        }

    }
}
