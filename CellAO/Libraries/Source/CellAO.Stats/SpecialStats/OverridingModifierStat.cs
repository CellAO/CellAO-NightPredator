using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Stats.SpecialStats
{
    public class OverridingModifierStat : Stat
    {
        public OverridingModifierStat(Stats statList, int number, uint defaultValue, bool sendBaseValue, bool dontWrite, bool announceToPlayfield)
            : base(statList, number, defaultValue, sendBaseValue, dontWrite, announceToPlayfield)
        {
        }

        public override int Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                this.Modifier = value - (int)this.BaseValue;
            }
        }
    }
}
