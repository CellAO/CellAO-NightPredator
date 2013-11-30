using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Stats.SpecialStats
{
    using CellAO.Enums;

    public class StatNanoDelta : Stat
    {
        public StatNanoDelta(
            int number,
            uint defaultValue,
            bool sendBaseValue,
            bool dontWrite,
            bool announceToPlayfield)
            : base(number, defaultValue, sendBaseValue, dontWrite, announceToPlayfield)
        {
        }

        public override void CalcTrickle()
        {
            this.DoNotDontWriteToSql = true;
            int value = this.Value;
            uint[] nanodelta = { 3, 3, 4, 2, 12, 15, 20 };
            this.BaseValue = nanodelta[this.Stats[StatIds.breed].Value - 1];
            this.Trickle = (int)Math.Floor((double)(this.Stats[StatIds.nanoenergypool].Value / 100));
            if (value != this.Value)
            {
                this.OnAfterStatChangedEvent(
                    new StatChangedEventArgs(this, (uint)value, (uint)this.Value, this.AnnounceToPlayfield));
            }
            this.DoNotDontWriteToSql = false;
        }
    }
}
