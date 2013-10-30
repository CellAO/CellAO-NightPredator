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
// Last modified: 2013-10-29 22:26
// Created:       2013-10-29 19:57

#endregion

namespace CellAO.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Interfaces;

    #endregion

    /// <summary>
    /// List of StatelStats
    /// </summary>
    public class StatelStats : IStatList
    {
        /// <summary>
        /// List of Stats (StatelStat) for non-modifiable Stats
        /// </summary>
        private readonly Dictionary<int, StatelStat> statList = new Dictionary<int, StatelStat>();

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        IStat IStatList.this[int index]
        {
            get
            {
                if (!this.statList.ContainsKey(index))
                {
                    this.statList.Add(index, new StatelStat(index));
                }

                return this.statList[index];
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <returns>
        /// </returns>
        IStat IStatList.this[string name]
        {
            get
            {
                int index = StatNamesDefaults.GetStatNumber(name);
                if (!this.statList.ContainsKey(index))
                {
                    this.statList.Add(index, new StatelStat(index));
                }

                return this.statList[index];
            }
        }

        /// <summary>
        /// Nothing to do here, they're always 0
        /// </summary>
        public void ClearModifiers()
        {
            return;
        }

        public void ClearChangedFlags()
        {
            // Empty, statels dont have changing stats
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Read()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Write()
        {
            throw new NotImplementedException();
        }
    }
}