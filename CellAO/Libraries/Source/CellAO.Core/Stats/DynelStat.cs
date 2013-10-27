#region License

// Copyright (c) 2005-2013, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
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
// 
// Last modified: 2013-10-26 22:26
// Created:       2013-10-26 22:17

#endregion

namespace CellAO.Core.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.InstancedEntities;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    #region StatChangedEventArgs

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
            DynelStat changedStat, uint valueBeforeChange, uint valueAfterChange, bool announceToPlayfield)
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
        public IInstancedEntity Parent
        {
            get
            {
                return this.Stat.Parent;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Stat { get; private set; }

        #endregion
    }

    #endregion

    #region ClassStat  class for one stat

    /// <summary>
    /// </summary>
    public class DynelStat : IStat
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
        private bool sendBaseValue = true;

        /// <summary>
        /// </summary>
        private int percentageModifier = 100; // From Items/Perks/Nanos

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
        public DynelStat(int number, uint defaultValue, bool sendBaseValue, bool dontWrite, bool announceToPlayfield)
        {
            this.StatId = number;
            this.DefaultValue = defaultValue;
            this.BaseValue = defaultValue;
            this.sendBaseValue = sendBaseValue;
            this.DoNotDontWriteToSql = dontWrite;
            this.announceToPlayfield = announceToPlayfield;
        }

        /// <summary>
        /// </summary>
        public DynelStat()
        {
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event EventHandler<StatChangedEventArgs> RaiseAfterStatChangedEvent;

        /// <summary>
        /// </summary>
        public event EventHandler<StatChangedEventArgs> RaiseBeforeStatChangedEvent;

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
        public bool Changed { get; set; }

        /// <summary>
        /// </summary>
        public bool DoNotDontWriteToSql { get; set; }

        /// <summary>
        /// </summary>
        public IInstancedEntity Parent { get; private set; }

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
        private uint baseValue;

        /// <summary>
        /// </summary>
        public uint BaseValue
        {
            get
            {
                return this.baseValue;
            }

            set
            {
                bool sendit = value != this.baseValue;
                this.baseValue = value;
                if (sendit)
                {
                    Stat.Send(this.Parent, this.StatId, value, this.announceToPlayfield);
                }
            }
        }

        /// <summary>
        /// </summary>
        public uint DefaultValue { get; set; }

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
        public int Trickle { get; set; }

        /// <summary>
        /// </summary>
        public int Modifier { get; set; }

        /// <summary>
        /// </summary>
        public int StatId { get; set; }

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
        public void AffectStats()
        {
            if (!(this.Parent is Character) && !(this.Parent is NonPlayerCharacter))
            {
                return;
            }

            foreach (int c in this.affects)
            {
                this.Parent.Stats[c].CalcTrickle();
            }
        }

        /// <summary>
        /// Calculate trickle value (prototype)
        /// </summary>
        public virtual void CalcTrickle()
        {
            if (!this.Parent.Starting)
            {
                this.AffectStats();
            }
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
        public void Set(uint value)
        {
            if ((this.Parent == null) || this.Parent.Starting)
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
                this.WriteStatToSql();

                if (!this.Parent.Starting)
                {
                    this.AffectStats();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        public void Set(int value)
        {
            this.Set((uint)value);
        }

        /// <summary>
        /// </summary>
        /// <param name="parent">
        /// </param>
        public void SetParent(IInstancedEntity parent)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Write Stat to Sql
        /// </summary>
        public void WriteStatToSql()
        {
            this.WriteStatToSql(this.Changed);
        }

        /// <summary>
        /// Write Stat to Sql
        /// </summary>
        /// <param name="doit">
        /// </param>
        public void WriteStatToSql(bool doit)
        {
            if (this.DoNotDontWriteToSql)
            {
                return;
            }

            if (doit)
            {
                Identity id = this.Parent.Identity;
                StatDao.AddStat((int)id.Type, id.Instance, this.StatId, (int)this.baseValue);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        private void OnAfterStatChangedEvent(StatChangedEventArgs e)
        {
            EventHandler<StatChangedEventArgs> handler = this.RaiseAfterStatChangedEvent;
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
            EventHandler<StatChangedEventArgs> handler = this.RaiseBeforeStatChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }

    #endregion
}