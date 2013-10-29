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
// Last modified: 2013-10-29 21:42
// Created:       2013-10-29 19:57

#endregion

namespace CellAO.Core.Stats.SpecialStats
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public class StatNanoInterval : DynelStat
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        /// <param name="sendBaseValue">
        /// </param>
        /// <param name="doNotWrite">
        /// </param>
        /// <param name="announceToPlayfield">
        /// </param>
        public StatNanoInterval(
            int number, uint defaultValue, bool sendBaseValue, bool doNotWrite, bool announceToPlayfield)
        {
            this.StatId = number;
            this.DefaultValue = defaultValue;

            this.BaseValue = this.DefaultValue;
            this.SendBaseValue = sendBaseValue;
            this.DoNotDontWriteToSql = doNotWrite;
            this.AnnounceToPlayfield = announceToPlayfield;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public override void CalcTrickle()
        {
            if (this.Parent is IMoving)
            {
                IMoving character = (IMoving)this.Parent;

                // calculating Nano and Heal Delta and interval
                int nanoInterval = 28
                                   - (Math.Min(
                                       (int)Math.Floor(Convert.ToDouble(character.Stats["Psychic"].Value) / 60), 13) * 2);
                character.Stats["NanoInterval"].BaseValue = (uint)nanoInterval; // Healinterval

                /* TODO Add Proper Timers w/e is in the other file too
                character.PurgeTimer(1);
                AOTimers at = new AOTimers();
                at.Strain = 1;
                */
                int nanoDelta = character.Stats["NanoDelta"].Value;
                if (character.MoveMode == MoveModes.Sit)
                {
                    int nanoDelta2 = nanoDelta >> 1;
                    nanoDelta = nanoDelta + nanoDelta2;
                }

                /*
                at.Timestamp = DateTime.Now + TimeSpan.FromSeconds(character.Stats.NanoInterval.Value);
                at.Function.Target = this.Parent.Identity.Instance; // changed from ItemHandler.itemtarget_self;
                at.Function.TickCount = -2;
                at.Function.TickInterval = (uint)(character.Stats.NanoInterval.Value * 1000);
                at.Function.FunctionType = Constants.FunctiontypeHit;
                at.Function.Arguments.Values.Add(214);
                at.Function.Arguments.Values.Add(nanoDelta);
                at.Function.Arguments.Values.Add(nanoDelta);
                at.Function.Arguments.Values.Add(0);
                character.Timers.Add(at);
                  */
                if (!this.Parent.Starting)
                {
                    this.AffectStats();
                }
            }
        }

        #endregion
    }
}