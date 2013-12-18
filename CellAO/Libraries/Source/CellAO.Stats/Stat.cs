#region License

// Copyright (c) 2005-2013, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
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

#endregion

namespace CellAO.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    /// Event Arguments for changed stats
    /// </summary>
    public class StatChangedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="changedStat">
        /// </param>
        /// <param name="valueBeforeChange">
        /// </param>
        /// <param name="valueAfterChange">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        public StatChangedEventArgs(
            Stat changedStat,
            uint valueBeforeChange,
            uint valueAfterChange,
            bool announceToPlayfield)
        {
            this.Stat = changedStat;
            this.OldValue = valueBeforeChange;
            this.NewValue = valueAfterChange;
            this.AnnounceToPlayfield = announceToPlayfield;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public bool AnnounceToPlayfield { get; set; }

        /// <summary>
        /// </summary>
        public uint NewValue { get; set; }

        /// <summary>
        /// </summary>
        public uint OldValue { get; set; }

        /// <summary>
        /// </summary>
        public Stat Stat { get; private set; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class Stat : IStat
    {
        #region Fields

        /// <summary>
        /// </summary>
        internal int LastCalculatedValue = -1;

        /// <summary>
        /// </summary>
        public bool ReCalculate { get; set; }

        /// <summary>
        /// </summary>
        private readonly List<int> affects = new List<int>();

        /// <summary>
        /// </summary>
        private bool announceToPlayfield = true;

        /// <summary>
        /// </summary>
        private uint baseValue = 1234567890;

        /// <summary>
        /// </summary>
        private int modifier = 0;

        /// <summary>
        /// </summary>
        private int percentageModifier = 100; // From Items/Perks/Nanos

        /// <summary>
        /// </summary>
        private bool sendBaseValue = true;

        /// <summary>
        /// </summary>
        private int trickle = 0;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="statList">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        /// <param name="sendBaseValue">
        /// </param>
        /// <param name="dontWrite">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        public Stat(
            Stats statList,
            int number,
            uint defaultValue,
            bool sendBaseValue,
            bool dontWrite,
            bool announceToPlayfield)
        {
            this.Stats = statList;
            this.StatId = number;
            this.DefaultValue = defaultValue;
            this.BaseValue = defaultValue;
            this.sendBaseValue = sendBaseValue;
            this.DoNotDontWriteToSql = dontWrite;
            this.announceToPlayfield = announceToPlayfield;
            this.ReCalculate = true;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event EventHandler<StatChangedEventArgs> AfterStatChangedEvent;

        /// <summary>
        /// </summary>
        public event EventHandler<StatChangedEventArgs> BeforeStatChangedEvent;

        /// <summary>
        /// </summary>
        public event EventHandler<StatChangedEventArgs> CalculateStatEvent;

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public List<int> Affects
        {
            get
            {
                return this.affects;
            }
        }

        /// <summary>
        /// </summary>
        public bool AnnounceToPlayfield
        {
            get
            {
                return this.announceToPlayfield;
            }

            set
            {
                this.announceToPlayfield = value;
            }
        }

        /// <summary>
        /// </summary>
        public uint BaseValue
        {
            get
            {
                return this.GetBaseValue;
            }

            set
            {
                this.ReCalculate = true;
                this.baseValue = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool Changed { get; set; }

        /// <summary>
        /// </summary>
        public uint DefaultValue { get; set; }

        /// <summary>
        /// </summary>
        public bool DoNotDontWriteToSql { get; set; }

        /// <summary>
        /// </summary>
        public virtual uint GetBaseValue
        {
            get
            {
                return this.baseValue;
            }
        }

        /// <summary>
        /// </summary>
        public virtual int GetValue
        {
            get
            {
                this.LastCalculatedValue = (int)Math.Floor(
                    (double) // ReSharper disable PossibleLossOfFraction
                        ((this.BaseValue + this.Modifier + this.Trickle) * this.PercentageModifier / 100));
                return this.LastCalculatedValue;
            }
        }

        /// <summary>
        /// </summary>
        public virtual int Modifier
        {
            get
            {
                return this.modifier;
            }

            set
            {
                int oldValue = this.LastCalculatedValue;
                int oldModifier = this.modifier;
                this.modifier = value;
                if (value != oldModifier)
                {
                    this.ReCalculate = true;
                    var temp = this.Value;
                }
            }
        }

        /// <summary>
        /// </summary>
        public virtual int PercentageModifier
        {
            get
            {
                return this.percentageModifier;
            }

            set
            {
                int oldPercentageModifier = this.percentageModifier;
                this.percentageModifier = value;
                if (value != oldPercentageModifier)
                {
                    this.ReCalculate = true;
                    var temp = this.Value;
                }
            }
        }

        /// <summary>
        /// </summary>
        public bool SendBaseValue
        {
            get
            {
                return this.sendBaseValue;
            }

            set
            {
                this.sendBaseValue = value;
            }
        }

        /// <summary>
        /// </summary>
        public int StatId { get; set; }

        /// <summary>
        /// </summary>
        public IStatList Stats { get; private set; }

        /// <summary>
        /// </summary>
        public virtual int Trickle
        {
            get
            {
                return this.trickle;
            }

            set
            {
                int oldTrickle = this.trickle;
                this.trickle = value;
                if (value != oldTrickle)
                {
                    this.ReCalculate = true;
                    var temp = this.Value;
                }
            }
        }

        /// <summary>
        /// </summary>
        public int Value
        {
            get
            {
                if (this.ReCalculate)
                {
                    int temp = this.GetValue;
                    this.Changed |= temp != this.LastCalculatedValue;
                    this.ReCalculate = false;

                    this.LastCalculatedValue = temp;
                }

                return this.LastCalculatedValue;
            }

            set
            {
                this.ReCalculate = true;
                this.Set(value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public virtual void CalcTrickle()
        {
            this.ReCalculate = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="val">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual uint GetMaxValue(uint val)
        {
            return val;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="starting">
        /// </param>
        public void Set(uint value, bool starting = false)
        {
            if (starting)
            {
                this.BaseValue = value;
                return;
            }

            if (value != this.BaseValue)
            {
                this.Changed = true;
                uint max = this.GetMaxValue(value);
                this.BaseValue = max;

                if (this.affects.Any())
                {
                    foreach (int affectedStat in this.affects)
                    {

                        this.Stats[affectedStat].ReCalculate = true;
                        // This recalculates values and sets the changed flag so it can be sent to the client if needed (value has changed)
                        int temp = this.Stats[affectedStat].Value;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="starting">
        /// </param>
        public virtual void Set(int value, bool starting = false)
        {
            this.Set((uint)value, starting);
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        public virtual void SetBaseValue(uint value)
        {
            this.Changed = this.baseValue != value;
            this.baseValue = value;
            this.ReCalculate = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="stats">
        /// </param>
        public void SetStats(Stats stats)
        {
            // Set the owning list
            this.Stats = stats;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        public virtual void SetValue(int value)
        {
            this.SetBaseValue((uint)value);
        }

        #endregion

    }
}