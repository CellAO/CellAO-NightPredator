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

namespace CellAO.Core.Stats.SpecialStats
{
    using CellAO.Core.Stats;

    /// <summary>
    /// </summary>
    public sealed class StatMaxNanoEnergy : DynelStat
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
        public StatMaxNanoEnergy(
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
            int[,] tableProfessionNanoPoints =
                {
                    { 4, 4, 4, 4, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    { 4, 4, 5, 4, 5, 5, 5, 5, 4, 5, 5, 5, 4, 4 },
                    { 4, 4, 6, 4, 6, 5, 5, 5, 4, 6, 6, 6, 4, 4 },
                    { 4, 4, 7, 4, 6, 6, 5, 5, 4, 7, 7, 7, 4, 4 },
                    { 4, 4, 8, 4, 7, 6, 6, 6, 4, 8, 8, 8, 4, 4 },
                    { 4, 4, 9, 4, 7, 7, 7, 7, 4, 10, 10, 10, 4, 5 },
                    { 5, 5, 10, 5, 8, 8, 8, 8, 5, 11, 11, 11, 5, 7 }, 
                    
                    
                    
                    // TitleLevel 7
                    // TitleLevel 6
                    // TitleLevel 5
                    // TitleLevel 4
                    // TitleLevel 3
                    // TitleLevel 2
                    // TitleLevel 1
                    // Sol|MA|ENG|FIX|AGE|ADV|TRA|CRA|ENF|DOC| NT| MP|KEP|SHA  // geprüfte Prof & TL = Soldier, Martial Artist, Engineer, Fixer, Agent, Advy, Trader, Doc
                };

            // Sol|Opi|Nan|Tro
            int[] breedBaseNanoPoints = { 10, 10, 15, 8, 10, 10, 10 };
            int[] breedMultiplicatorNanoPoints = { 3, 3, 4, 2, 3, 3, 3 };
            int[] breedModificatorNanoPoints = { 0, -1, 1, -2, 0, 0, 0 };

            if ((this.Parent is Character) || (this.Parent is NonPlayerCharacter))
            {
                // This condition could be obsolete
                Character character = (Character)this.Parent;
                uint breed = character.Stats["Breed"].BaseValue;
                uint profession = character.Stats["Profession"].BaseValue;
                if (profession > 13)
                {
                    profession--;
                }

                uint titleLevel = character.Stats["TitleLevel"].BaseValue;
                uint level = character.Stats["Level"].BaseValue;

                // BreedBaseNP+(Level*(TableProfNP+BreedModiNP))+(NanoEnergyPool*BreedMultiNP))
                if (this.Parent is NonPlayerCharacter)
                {
                    // TODO: correct calculation of mob NP
                    this.Set(
                        (uint)
                        (breedBaseNanoPoints[breed - 1]
                         + (character.Stats["Level"].Value
                            * (tableProfessionNanoPoints[6, 8] + breedModificatorNanoPoints[breed - 1]))
                         + (character.Stats["NanoEnergyPool"].Value * breedMultiplicatorNanoPoints[breed - 1])));
                }
                else
                {
                    this.Set(
                        (uint)
                        (breedBaseNanoPoints[breed - 1]
                         + (character.Stats["Level"].Value
                            * (tableProfessionNanoPoints[titleLevel - 1, profession - 1]
                               + breedModificatorNanoPoints[breed - 1]))
                         + (character.Stats["NanoEnergyPool"].Value * breedMultiplicatorNanoPoints[breed - 1])));
                }
            }

            if (!this.Parent.Starting)
            {
                this.AffectStats();
            }
        }

        #endregion
    }
}