using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Stats.SpecialStats
{
    public class StatVisualProfession : OverridingModifierStat
    {
        public StatVisualProfession(Stats statList, int number, uint defaultValue, bool sendBaseValue, bool dontWrite, bool announceToPlayfield)
            : base(statList, number, defaultValue, sendBaseValue, dontWrite, announceToPlayfield)
        {
        }

        public override void Set(int value, bool starting = false)
        {
            this.Modifier = value - (int)this.BaseValue;
            this.ReCalculate = true;
            this.Changed = true;

        }
    }
}
