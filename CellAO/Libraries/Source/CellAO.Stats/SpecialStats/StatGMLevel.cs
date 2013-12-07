using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Stats.SpecialStats
{
    using CellAO.Database.Dao;

    public class StatGmLevel : Stat
    {
        public StatGmLevel(Stats statList, int number, uint defaultValue, bool sendBaseValue, bool dontWrite, bool announceToPlayfield)
            : base(statList, number, defaultValue, sendBaseValue, dontWrite, announceToPlayfield)
        {
        }

        public override int Value
        {
            get
            {
                return LoginDataDao.GetByCharacterId(this.Stats.Owner.Instance).GM;
            }
            set
            {
                base.Value = value;
            }
        }
    }
}
