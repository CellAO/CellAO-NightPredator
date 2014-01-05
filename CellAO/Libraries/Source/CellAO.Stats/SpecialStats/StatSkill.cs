#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace CellAO.Stats.SpecialStats
{
    #region Usings ...

    using System;

    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public class StatSkill : Stat
    {
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
        public StatSkill(
            Stats statList, 
            int number, 
            uint defaultValue, 
            bool sendBaseValue, 
            bool dontWrite, 
            bool announceToPlayfield)
            : base(statList, number, defaultValue, sendBaseValue, dontWrite, announceToPlayfield)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public override int Trickle
        {
            get
            {
                double strengthTrickle = SkillTrickleTable.table[this.StatId - 100, 1];
                double agilityTrickle = SkillTrickleTable.table[this.StatId - 100, 2];
                double staminaTrickle = SkillTrickleTable.table[this.StatId - 100, 3];
                double intelligenceTrickle = SkillTrickleTable.table[this.StatId - 100, 4];
                double senseTrickle = SkillTrickleTable.table[this.StatId - 100, 5];
                double psychicTrickle = SkillTrickleTable.table[this.StatId - 100, 6];

                return
                    Convert.ToInt32(
                        Math.Floor(
                            (strengthTrickle * this.Stats[StatIds.strength].Value
                             + staminaTrickle * this.Stats[StatIds.stamina].Value
                             + senseTrickle * this.Stats[StatIds.sense].Value
                             + agilityTrickle * this.Stats[StatIds.agility].Value
                             + intelligenceTrickle * this.Stats[StatIds.intelligence].Value
                             + psychicTrickle * this.Stats[StatIds.psychic].Value) / 4));
            }

            set
            {
                this.ReCalculate = true;
                base.Trickle = value;
            }
        }

        #endregion
    }
}