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
// Last modified: 2013-11-16 19:03

#endregion

namespace CellAO.Stats
{
    #region Usings ...

    #region Usings ...

    using System;
    using System.Collections.Generic;

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

    #endregion

    #region Stat class for one stat

    /// <summary>
    /// </summary>
    public class Stat : IStat
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly List<int> affects = new List<int>();

        /// <summary>
        /// </summary>
        private bool announceToPlayfield = true;

        /// <summary>
        /// </summary>
        private int percentageModifier = 100; // From Items/Perks/Nanos

        /// <summary>
        /// </summary>
        private bool sendBaseValue = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
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
        public Stat(int number, uint defaultValue, bool sendBaseValue, bool dontWrite, bool announceToPlayfield)
        {
            this.StatId = number;
            this.DefaultValue = defaultValue;
            this.BaseValue = defaultValue;
            this.sendBaseValue = sendBaseValue;
            this.DoNotDontWriteToSql = dontWrite;
            this.announceToPlayfield = announceToPlayfield;
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
        public uint BaseValue { get; set; }

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
        public int Modifier { get; set; }

        /// <summary>
        /// </summary>
        public int PercentageModifier
        {
            get
            {
                return this.percentageModifier;
            }

            set
            {
                this.percentageModifier = value;
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
        public int Trickle { get; set; }

        /// <summary>
        /// </summary>
        public virtual int Value
        {
            get
            {
                return (int)Math.Floor(
                    (double) // ReSharper disable PossibleLossOfFraction
                        ((this.BaseValue + this.Modifier + this.Trickle) * this.PercentageModifier / 100));

                // ReSharper restore PossibleLossOfFraction
            }

            set
            {
                this.Set(value);
            }
        }

        #endregion

        #region Public Methods and Operators

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
                uint oldvalue = (uint)this.Value;
                uint max = this.GetMaxValue(value);
                this.OnBeforeStatChangedEvent(new StatChangedEventArgs(this, oldvalue, max, this.announceToPlayfield));
                this.BaseValue = max;
                this.OnAfterStatChangedEvent(new StatChangedEventArgs(this, oldvalue, max, this.announceToPlayfield));
                this.Changed = true;

                this.OnCalculateStat(new StatChangedEventArgs(this, oldvalue, max, this.announceToPlayfield));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="starting">
        /// </param>
        public void Set(int value, bool starting = false)
        {
            this.Set((uint)value, starting);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        private void OnAfterStatChangedEvent(StatChangedEventArgs e)
        {
            EventHandler<StatChangedEventArgs> handler = this.AfterStatChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        private void OnBeforeStatChangedEvent(StatChangedEventArgs e)
        {
            EventHandler<StatChangedEventArgs> handler = this.BeforeStatChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        private void OnCalculateStat(StatChangedEventArgs e)
        {
            EventHandler<StatChangedEventArgs> handler = this.CalculateStatEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }

    #endregion
}