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

namespace ZoneEngine.Core
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Database.Entities;

    #endregion

    /// <summary>
    /// </summary>
    public class TradeSkillEntry
    {
        #region Fields

        /// <summary>
        /// </summary>
        public int DeleteFlag;

        /// <summary>
        /// </summary>
        public int ID1;

        /// <summary>
        /// </summary>
        public int ID2;

        /// <summary>
        /// </summary>
        public bool IsImplant;

        /// <summary>
        /// </summary>
        public int MaxBump;

        /// <summary>
        /// </summary>
        public int MaxXP;

        /// <summary>
        /// </summary>
        public int MinTargetQL;

        /// <summary>
        /// </summary>
        public int MinXP;

        /// <summary>
        /// </summary>
        public int QLRangePercent;

        /// <summary>
        /// </summary>
        public int ResultHighId;

        /// <summary>
        /// </summary>
        public int ResultLowId;

        /// <summary>
        /// </summary>
        public List<TradeSkillSkill> Skills = new List<TradeSkillSkill>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="ts">
        /// </param>
        /// <returns>
        /// </returns>
        public static TradeSkillEntry ConvertFromDB(DBTradeSkill ts)
        {
            TradeSkillEntry tse = new TradeSkillEntry();
            tse.ID1 = ts.ID1;
            tse.ID2 = ts.ID2;
            tse.IsImplant = ts.IsImplant > 0;
            tse.MaxBump = ts.MaxBump;
            tse.MaxXP = ts.MaxXP;
            tse.MinTargetQL = ts.MinTarget;
            tse.MinXP = ts.MinXP;
            tse.ResultLowId = int.Parse(ts.ResultIDS.Split(',')[0].Trim());
            tse.ResultHighId = int.Parse(ts.ResultIDS.Split(',')[1].Trim());
            tse.QLRangePercent = ts.QLRangePercent;
            tse.DeleteFlag = ts.DeleteFlag;

            string[] skillStrings = ts.Skill.Split(',');
            string[] skillPercents = ts.SkillPercent.Split(',');
            string[] skillPerBumps = ts.SkillPerBump.Split(',');

            int skillcount = skillStrings.Length;

            for (int i = 0; i < skillcount; i++)
            {
                TradeSkillSkill tss = new TradeSkillSkill();
                tss.StatId = int.Parse(skillStrings[i].Trim());
                tss.SkillPerBump = int.Parse(skillPerBumps[i].Trim());
                tss.Percent = int.Parse(skillPercents[i].Trim());
                tse.Skills.Add(tss);
            }

            return tse;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="sourceQL">
        /// </param>
        /// <param name="targetQL">
        /// </param>
        /// <returns>
        /// </returns>
        internal bool ValidateRange(int sourceQL, int targetQL)
        {
            if (this.QLRangePercent != 0)
            {
                if (this.QLRangePercent == 1)
                {
                    return sourceQL >= targetQL;
                }

                return (targetQL - (decimal)sourceQL) / targetQL <= this.QLRangePercent / 100M;
            }

            return true;
        }

        #endregion
    }
}