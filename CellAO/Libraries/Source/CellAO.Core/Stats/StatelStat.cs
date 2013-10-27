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
    /// <summary>
    /// </summary>
    public class StatelStat : IStat
    {
        /// <summary>
        /// Stat's value
        /// </summary>
        private int value;

        /// <summary>
        /// Stat's Id
        /// </summary>
        public int StatId { get; private set; }

        /// <summary>
        /// Stat's value
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Always 'value' for Statels
        /// </summary>
        public uint BaseValue
        {
            get
            {
                return (uint)this.value;
            }

            set
            {
                this.value = (int)value;
            }
        }

        /// <summary>
        /// Always 0 for Statels
        /// </summary>
        public int Trickle
        {
            get
            {
                return 0;
            }

            set
            {
            }
        }

        /// <summary>
        /// Always 0 for Statels
        /// </summary>
        public int Modifier
        {
            get
            {
                return 0;
            }

            set
            {
            }
        }

        /// <summary>
        /// Always 100%
        /// </summary>
        public int PercentageModifier
        {
            get
            {
                return 100;
            }

            set
            {
            }
        }

        /// <summary>
        /// </summary>
        public bool AnnounceToPlayfield { get; set; }

        /// <summary>
        /// Nothing to do here, no trickles in statels
        /// </summary>
        public void CalcTrickle()
        {
            return;
        }

        /// <summary>
        /// Never called on statels
        /// </summary>
        public void AffectStats()
        {
        }

        /// <summary>
        /// Never called on Statels
        /// </summary>
        /// <param name="old">
        /// </param>
        /// old (actual) value
        /// <returns>
        /// </returns>
        public uint GetMaxValue(uint old)
        {
            return 0;
        }

        /// <summary>
        /// Create new Stat and fill with default value
        /// </summary>
        /// <param name="statNumber">
        /// Stat's id
        /// </param>
        public StatelStat(int statNumber)
        {
            this.StatId = statNumber;
            this.value = StatNamesDefaults.GetDefault(statNumber);
        }

        /// <summary>
        /// Create Stat and fill with designated value
        /// </summary>
        /// <param name="statNumber">
        /// Stat's Id
        /// </param>
        /// <param name="statValue">
        /// Stat's initial value
        /// </param>
        public StatelStat(int statNumber, int statValue)
            : this(statNumber)
        {
            this.value = statValue;
        }
    }
}