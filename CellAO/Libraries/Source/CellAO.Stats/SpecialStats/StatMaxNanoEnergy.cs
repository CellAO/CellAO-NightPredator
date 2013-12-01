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

namespace CellAO.Stats.SpecialStats
{
    #region Usings ...

    using System;

    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public class StatMaxNanoEnergy : Stat
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
        public StatMaxNanoEnergy(
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
        public override uint BaseValue
        {
            get
            {
                int[] breedBaseNanoPoints = { 10, 10, 15, 8, 10, 10, 10 };
                return (uint)breedBaseNanoPoints[this.Stats[StatIds.breed].BaseValue - 1];
            }
        }

        /// <summary>
        /// </summary>
        public override int Modifier
        {
            get
            {
                return base.Modifier;
            }

            set
            {
                base.Modifier = value;
                this.Stats[StatIds.currentnano].Value = Math.Min(this.Value, this.Stats[StatIds.currentnano].Value);
            }
        }

        /// <summary>
        /// </summary>
        public override int Value
        {
            get
            {
                int[,] tableProfessionNanoPoints =
                {
                    { 4, 4, 4, 4, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4 }, 
                    { 4, 4, 5, 4, 5, 5, 5, 5, 4, 5, 5, 5, 4, 4 }, 
                    { 4, 4, 6, 4, 6, 5, 5, 5, 4, 6, 6, 6, 4, 4 }, 
                    { 4, 4, 7, 4, 6, 6, 5, 5, 4, 7, 7, 7, 4, 4 }, 
                    { 4, 4, 8, 4, 7, 6, 6, 6, 4, 8, 8, 8, 4, 4 }, 
                    { 4, 4, 9, 4, 7, 7, 7, 7, 4, 10, 10, 10, 4, 5 }, 
                    { 5, 5, 10, 5, 8, 8, 8, 8, 5, 11, 11, 11, 5, 7 }, 
                };

                int[] breedMultiplicatorNanoPoints = { 3, 3, 4, 2, 3, 3, 3 };
                int[] breedModificatorNanoPoints = { 0, -1, 1, -2, 0, 0, 0 };
                uint breed = this.Stats[StatIds.breed].BaseValue;
                uint profession = this.Stats[StatIds.profession].BaseValue;

                // TODO: Change the tableProfessionNanoPoints array and add the 13th as dummy
                if (profession > 13)
                {
                    profession--;
                }

                uint titleLevel = this.Stats[StatIds.titlelevel].BaseValue;
                uint level = this.Stats[StatIds.level].BaseValue;

                int beforeModifiers =
                    (int)
                        (this.BaseValue
                         + (level
                            * (tableProfessionNanoPoints[titleLevel - 1, profession - 1]
                               + breedModificatorNanoPoints[breed - 1]))
                         + (this.Stats[StatIds.nanoenergypool].Value * breedMultiplicatorNanoPoints[breed - 1]));
                return (int)Math.Floor(
                    (double) // ReSharper disable PossibleLossOfFraction
                        ((beforeModifiers + this.Modifier + this.Trickle) * this.PercentageModifier / 100));

                // ReSharper restore PossibleLossOfFraction
            }

            set
            {
                base.Value = value;
            }
        }

        #endregion
    }
}