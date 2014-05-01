using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Database.Entities
{
    using CellAO.Database.Dao;

    [Tablename("mobspawnswaypoints")]
    public class DBMobSpawnWaypoints:IDBEntity
    {
        public int Id { get; set; }
        public int Identity { get; set; }
        public int WalkMode { get; set; }
        public int Playfield { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

    }
}
