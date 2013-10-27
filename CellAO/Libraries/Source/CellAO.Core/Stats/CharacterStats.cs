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
// Last modified: 2013-10-27 11:37
// Created:       2013-10-27 07:58

#endregion

namespace CellAO.Core.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion

    /// <summary>
    /// </summary>
    internal class OrgMisc
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="orgId">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<int> GetOrgMembers(uint orgId)
        {
            return GetOrgMembers(orgId, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="orgId">
        /// </param>
        /// <param name="excludePresident">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<int> GetOrgMembers(uint orgId, bool excludePresident)
        {
            // Stat #5 == Clan == OrgID
            // Stat #48 == ClanLevel == Org Rank (0 is president)
            SqlWrapper mySql = new SqlWrapper();
            List<int> orgMembers = new List<int>();
            string pres = string.Empty;

            if (excludePresident)
            {
                pres = " AND `ID` NOT IN (SELECT `ID` FROM `characters_stats` WHERE `Stat` = '48' AND `Value` = '0')";
            }

            DataTable dt =
                mySql.ReadDatatable(
                    "SELECT `ID` FROM `characters_stats` WHERE `Stat` = '5' AND `Value` = '" + orgId + "'" + pres);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    orgMembers.Add((Int32)row[0]);
                }
            }

            return orgMembers;
        }

        #endregion
    }
}