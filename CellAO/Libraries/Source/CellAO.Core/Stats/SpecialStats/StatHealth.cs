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

namespace CellAO.Core.Stats.SpecialStats
{
    using CellAO.Interfaces;

    /// <summary>
    /// </summary>
    public class StatHealth : DynelStat
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
        public StatHealth(int number, uint defaultValue, bool sendBaseValue, bool doNotWrite, bool announceToPlayfield)
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
            int[,] tableProfessionHitPoints =
                {
                    { 6, 6, 6, 6, 6, 6, 6, 6, 7, 6, 6, 6, 6, 6 },
                    { 7, 7, 6, 7, 7, 7, 6, 7, 8, 6, 6, 6, 7, 7 },
                    { 8, 7, 6, 7, 7, 8, 7, 7, 9, 6, 6, 6, 8, 7 },
                    { 9, 8, 6, 8, 8, 8, 7, 7, 10, 6, 6, 6, 9, 8 },
                    { 10, 9, 6, 9, 8, 9, 8, 8, 11, 6, 6, 6, 10, 9 },
                    { 11, 12, 6, 10, 9, 9, 9, 9, 12, 6, 6, 6, 11, 10 },
                    { 12, 13, 7, 11, 10, 10, 10, 10, 13, 7, 7, 7, 12, 11 }, // TitleLevel 7
                    // TitleLevel 6
                    // TitleLevel 5
                    // TitleLevel 4
                    // TitleLevel 3
                    // TitleLevel 2
                    // TitleLevel 1
                    // Sol| MA|ENG|FIX|AGE|ADV|TRA|CRA|ENF|DOC| NT| MP| KEP|SHA   // geprüfte Prof & TL = Soldier, Martial Artist, Engineer, Fixer, Agent, Advy, Trader, Crat
                };

            // Sol|Opi|Nan|Tro
            int[] breedBaseHitPoints = { 10, 15, 10, 25, 30, 30, 30 };
            int[] breedMultiplicatorHitPoints = { 3, 3, 2, 4, 8, 8, 10 };
            int[] breedModificatorHitPoints = { 0, -1, -1, 0, 0, 0, 0 };

            if ((this.Parent is ICharacter) || (this.Parent is INonPlayerCharacter))
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

                // BreedBaseHP+(Level*(TableProfHP+BreedModiHP))+(BodyDevelopment*BreedMultiHP))
                if (this.Parent is NonPlayerCharacter)
                {
                    // TODO: correct calculation of mob HP
                    Set(
                        breedBaseHitPoints[breed - 1]
                        + (character.Stats["Level"].Value * tableProfessionHitPoints[6, 8])
                        + (character.Stats["BodyDevelopment"].Value + breedMultiplicatorHitPoints[breed - 1]));
                }
                else
                {
                    Set(
                        breedBaseHitPoints[breed - 1]
                        + (character.Stats["Level"].Value
                           * (tableProfessionHitPoints[titleLevel - 1, profession - 1]
                              + breedModificatorHitPoints[breed - 1]))
                        + (character.Stats["BodyDevelopment"].Value * breedMultiplicatorHitPoints[breed - 1]));
                }

                // ch.Stats.Health.StatBaseValue = (UInt32)Math.Min(ch.Stats.Health.Value, StatBaseValue);
            }

            if (!this.Parent.Starting)
            {
                this.AffectStats();
            }
        }

        #endregion
    }
}