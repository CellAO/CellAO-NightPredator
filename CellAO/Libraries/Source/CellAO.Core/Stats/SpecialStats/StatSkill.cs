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

    #endregion

    /// <summary>
    /// Combining all Skills (MM/BM/1HE etc pp)
    /// </summary>
    public class StatSkill : DynelStat
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
        public StatSkill(int number, uint defaultValue, bool sendBaseValue, bool doNotWrite, bool announceToPlayfield)
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
            double strengthTrickle = SkillTrickleTable.table[this.StatId - 100, 1];
            double agilityTrickle = SkillTrickleTable.table[this.StatId - 100, 2];
            double staminaTrickle = SkillTrickleTable.table[this.StatId - 100, 3];
            double intelligenceTrickle = SkillTrickleTable.table[this.StatId - 100, 4];
            double senseTrickle = SkillTrickleTable.table[this.StatId - 100, 5];
            double psychicTrickle = SkillTrickleTable.table[this.StatId - 100, 6];

            this.Trickle =
                Convert.ToInt32(
                    Math.Floor(
                        (strengthTrickle * this.Parent.Stats["Strength"].Value
                         + staminaTrickle * this.Parent.Stats["Stamina"].Value
                         + senseTrickle * this.Parent.Stats["Sense"].Value
                         + agilityTrickle * this.Parent.Stats["Agility"].Value
                         + intelligenceTrickle * this.Parent.Stats["Intelligence"].Value
                         + psychicTrickle * this.Parent.Stats["Psychic"].Value) / 4));

            if (!this.Parent.Starting)
            {
                this.AffectStats();
            }
        }

        #endregion
    }
}