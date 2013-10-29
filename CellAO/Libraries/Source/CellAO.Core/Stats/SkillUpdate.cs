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

namespace CellAO.Core.Stats
{
    #region Usings ...

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public static class SkillUpdate
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="dynel">
        /// </param>
        /// <returns>
        /// </returns>
        public static uint CalculateHitPoints(IStats dynel)
        {
            int[,] tableProfessionHitPoints =
                {
                    { 6, 6, 6, 6, 6, 6, 6, 6, 7, 6, 6, 6, 6, 6 },
                    { 7, 7, 6, 7, 7, 7, 6, 7, 8, 6, 6, 6, 7, 7 },
                    { 8, 7, 6, 7, 7, 8, 7, 7, 9, 6, 6, 6, 8, 8 },
                    { 9, 8, 6, 8, 8, 8, 7, 7, 10, 6, 6, 6, 9, 9 }, 
                    
                    // TitleLevel 4
                    { 10, 9, 6, 9, 8, 9, 8, 8, 11, 6, 6, 6, 10, 9 }, // TitleLevel 5
                    { 11, 12, 6, 10, 9, 9, 9, 9, 12, 6, 6, 6, 11, 10 }, // TitleLevel 6
                    { 12, 13, 7, 11, 10, 10, 9, 9, 13, 6, 6, 6, 11, 10 }, // TitleLevel 7
                    // TitleLevel 3
                    // TitleLevel 2
                    // TitleLevel 1
                    // Sol| MA|ENG|FIX|AGE|ADV|TRA|CRA|ENF|DOC| NT| MP| KEP|SHA   // geprüfte Prof & TL = Soldier, Martial Artist, Engineer, Fixer
                };

            // Sol|Opi|Nan|Tro
            int[] breedBaseHitPoints = { 10, 15, 10, 25 };
            int[] breedMultiplicatorHitPoints = { 3, 3, 2, 4 };
            int[] breedModificatorHitPoints = { 0, -1, -1, 0 };

            uint breed = dynel.Stats["Breed"].BaseValue;
            uint profession = dynel.Stats["Profession"].BaseValue;
            uint titlelevel = dynel.Stats["TitleLevel"].BaseValue;
            uint level = dynel.Stats["Level"].BaseValue;

            // BreedBaseHP+(Level*(TableProfHP+BreedModiHP))+(BodyDevelopment*BreedMultiHP))
            return
                (uint)
                (breedBaseHitPoints[breed - 1]
                 + (dynel.Stats["Level"].Value
                    * (tableProfessionHitPoints[titlelevel - 1, profession - 1] + breedModificatorHitPoints[breed - 1]))
                 + (dynel.Stats["BodyDevelopment"].Value * breedMultiplicatorHitPoints[breed - 1]));
        }

        /// <summary>
        /// </summary>
        /// <param name="dynel">
        /// </param>
        /// <returns>
        /// </returns>
        public static double CalculateIP(IStats dynel)
        {
            double calc = 0;
            uint breed = dynel.Stats["Breed"].BaseValue; // 4 = Breed
            uint profession = dynel.Stats["Profession"].BaseValue - 1; // 60 = Profession

            double[,] skillCosts =
                {
                    { 100, 2.8, 1.6, 2.8, 2.0, 1.6, 2.8, 2.8, 3.0, 1.0, 2.8, 2.8, 1.6, 2.0, 2.0 },
                    { 101, 1.4, 2.5, 4.0, 3.2, 1.0, 4.0, 2.5, 3.2, 2.5, 4.0, 4.0, 1.0, 2.0, 3.2 },
                    { 102, 1.5, 1.6, 3.2, 2.4, 1.0, 2.4, 2.5, 3.2, 2.5, 2.6, 4.0, 4.0, 2.5, 1.8 },
                    { 103, 1.0, 2.0, 4.0, 2.4, 1.0, 3.2, 2.0, 3.2, 2.0, 4.0, 4.0, 4.0, 2.0, 3.2 },
                    { 106, 1.5, 2.5, 4.0, 2.4, 1.0, 3.2, 2.5, 3.2, 2.0, 3.2, 4.0, 1.0, 2.5, 2.5 },
                    { 107, 1.5, 2.5, 4.0, 3.2, 1.4, 3.2, 3.2, 3.2, 2.0, 4.0, 3.2, 4.0, 2.5, 2.5 },
                    { 105, 1.5, 2.5, 4.0, 3.2, 1.0, 3.2, 2.5, 1.0, 2.0, 2.5, 2.5, 4.0, 2.5, 2.5 },
                    { 104, 1.5, 3.2, 4.0, 3.2, 1.8, 4.0, 3.2, 3.2, 3.0, 4.0, 3.2, 4.0, 2.2, 2.5 },
                    { 108, 1.6, 1.2, 3.2, 3.2, 1.6, 3.2, 2.5, 4.0, 1.0, 3.2, 2.4, 1.6, 1.6, 2.4 },
                    { 109, 1.6, 1.6, 4.0, 3.2, 2.5, 2.0, 2.2, 4.0, 2.4, 4.0, 2.4, 4.0, 1.6, 2.4 },
                    { 110, 3.0, 3.0, 4.0, 4.0, 2.5, 2.0, 2.5, 4.0, 4.0, 4.0, 4.0, 4.0, 1.0, 4.0 },
                    { 111, 1.8, 2.0, 4.0, 4.0, 4.0, 4.0, 2.4, 4.0, 1.0, 2.5, 4.0, 4.0, 2.4, 4.0 },
                    { 112, 1.0, 1.8, 1.6, 1.6, 3.0, 1.5, 1.6, 4.0, 3.5, 2.4, 1.6, 4.0, 1.0, 2.0 },
                    { 113, 1.7, 1.3, 4.0, 4.0, 4.0, 4.0, 2.0, 4.0, 4.0, 4.0, 4.0, 4.0, 2.0, 2.8 },
                    { 114, 2.5, 2.5, 3.2, 3.2, 2.5, 3.2, 1.0, 4.0, 3.0, 3.2, 3.2, 4.0, 1.5, 2.4 },
                    { 115, 2.4, 3.2, 3.2, 2.4, 2.5, 3.2, 1.8, 4.0, 4.0, 4.0, 3.0, 4.0, 1.5, 1.5 },
                    { 116, 1.6, 3.0, 4.0, 4.0, 3.5, 3.0, 2.8, 4.0, 4.0, 4.0, 4.5, 4.0, 1.0, 4.0 },
                    { 117, 1.0, 2.4, 2.4, 2.4, 1.6, 1.6, 1.0, 2.4, 2.5, 2.5, 2.4, 3.2, 1.6, 1.4 },
                    { 118, 1.8, 1.6, 4.0, 3.2, 1.0, 3.2, 2.5, 1.0, 2.0, 3.0, 3.5, 1.0, 2.4, 3.2 },
                    { 119, 2.0, 1.6, 3.2, 3.2, 3.0, 3.2, 1.6, 3.8, 2.4, 4.0, 3.0, 4.0, 1.0, 2.5 },
                    { 120, 1.6, 1.6, 2.0, 2.0, 1.6, 3.0, 2.4, 3.8, 1.0, 3.2, 3.2, 3.4, 2.4, 3.2 },
                    { 121, 1.6, 2.0, 4.0, 4.0, 2.0, 3.5, 2.0, 4.0, 1.0, 2.5, 4.0, 4.0, 2.0, 4.0 },
                    { 122, 1.6, 1.6, 1.0, 1.6, 2.5, 2.4, 2.4, 2.4, 1.6, 1.6, 1.0, 1.6, 2.4, 1.8 },
                    { 123, 1.2, 2.0, 2.0, 1.0, 1.6, 2.0, 1.2, 1.2, 1.6, 2.0, 2.0, 2.5, 2.0, 1.6 },
                    { 124, 1.0, 2.0, 2.0, 1.0, 2.0, 1.6, 1.2, 1.8, 2.0, 2.0, 2.0, 1.5, 2.0, 1.6 },
                    { 125, 1.2, 1.5, 1.8, 2.0, 2.0, 1.0, 1.5, 3.2, 2.4, 2.0, 2.0, 3.2, 2.0, 1.2 },
                    { 126, 1.6, 2.0, 2.4, 1.6, 1.8, 1.0, 1.5, 3.2, 3.2, 2.0, 1.6, 3.2, 2.4, 1.0 },
                    { 127, 1.8, 1.2, 1.6, 1.0, 2.5, 1.0, 2.4, 3.2, 2.0, 1.0, 1.0, 3.2, 2.0, 1.6 },
                    { 128, 1.5, 1.6, 1.0, 1.0, 2.5, 2.4, 3.2, 1.8, 1.6, 1.0, 1.0, 1.9, 2.4, 1.8 },
                    { 129, 1.8, 1.6, 1.0, 1.6, 2.5, 2.4, 2.4, 1.6, 2.0, 1.6, 1.0, 1.4, 2.0, 1.5 },
                    { 130, 1.8, 1.4, 1.6, 1.6, 2.5, 1.0, 2.5, 3.2, 2.4, 1.0, 1.0, 3.2, 2.5, 1.5 },
                    { 131, 1.8, 2.4, 1.6, 1.6, 2.5, 1.0, 3.2, 1.4, 1.6, 1.0, 1.0, 1.9, 3.2, 1.5 },
                    { 132, 1.6, 1.2, 1.4, 1.0, 2.0, 1.8, 1.6, 2.2, 1.6, 1.0, 1.0, 2.5, 2.0, 1.2 },
                    { 133, 2.4, 2.5, 4.0, 4.0, 4.0, 3.0, 2.5, 4.0, 4.0, 4.0, 4.0, 4.0, 1.0, 3.0 },
                    { 134, 1.5, 1.8, 4.0, 4.0, 4.0, 4.0, 2.0, 4.0, 4.0, 2.5, 4.0, 4.0, 2.0, 2.5 },
                    { 135, 1.6, 2.0, 2.4, 2.4, 2.4, 1.6, 1.0, 2.4, 2.5, 2.4, 2.4, 1.8, 2.4, 2.4 },
                    { 136, 1.6, 1.0, 1.6, 2.4, 2.4, 2.4, 1.0, 1.2, 1.6, 2.4, 2.4, 2.4, 2.4, 1.4 },
                    { 137, 1.0, 3.0, 2.0, 2.0, 1.5, 2.0, 2.0, 1.8, 1.6, 2.0, 2.0, 1.6, 1.5, 1.4 },
                    { 138, 1.0, 1.6, 2.0, 2.0, 2.0, 2.0, 2.0, 1.8, 1.4, 2.0, 2.0, 1.4, 1.5, 1.5 },
                    { 139, 1.0, 2.4, 2.0, 2.4, 1.6, 1.6, 1.0, 2.4, 3.0, 2.5, 2.4, 3.2, 1.6, 1.4 },
                    { 140, 1.0, 1.6, 2.0, 1.6, 1.6, 1.6, 2.0, 1.2, 2.0, 2.0, 1.6, 2.4, 1.6, 1.3 },
                    { 141, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
                    { 142, 2.4, 2.8, 3.2, 2.8, 1.0, 2.4, 1.8, 2.0, 1.2, 2.8, 2.8, 4.0, 2.0, 2.0 },
                    { 143, 3.2, 3.0, 3.2, 3.2, 1.2, 3.2, 2.4, 1.0, 1.0, 3.2, 2.4, 1.4, 2.4, 3.2 },
                    { 144, 4.0, 1.6, 3.0, 4.0, 4.0, 4.0, 4.0, 1.3, 1.2, 2.5, 2.5, 1.0, 4.0, 4.0 },
                    { 145, 1.5, 1.6, 3.2, 2.4, 1.4, 4.0, 2.4, 1.0, 1.5, 4.0, 3.2, 1.4, 2.5, 2.5 },
                    { 146, 1.5, 1.0, 2.4, 3.2, 2.0, 4.0, 3.9, 4.0, 3.0, 4.0, 3.2, 1.0, 3.0, 4.0 },
                    { 147, 2.0, 2.5, 4.0, 2.4, 1.5, 4.0, 2.5, 1.0, 2.0, 4.0, 3.2, 1.4, 2.4, 3.0 },
                    { 148, 1.8, 3.2, 4.0, 3.0, 3.0, 3.0, 1.5, 4.0, 4.0, 4.0, 4.0, 4.0, 1.5, 3.5 },
                    { 149, 2.0, 1.6, 1.0, 1.0, 2.4, 1.6, 2.4, 3.2, 2.5, 1.0, 1.0, 2.8, 4.0, 1.5 },
                    { 150, 1.0, 3.2, 4.0, 2.4, 3.5, 3.2, 1.6, 4.0, 3.2, 4.0, 4.0, 4.0, 1.0, 2.5 },
                    { 151, 2.2, 1.1, 4.0, 3.2, 3.5, 4.0, 2.5, 4.0, 3.0, 4.0, 3.2, 4.0, 1.8, 2.5 },
                    { 152, 1.2, 2.4, 2.4, 2.0, 1.0, 2.4, 1.8, 1.2, 1.5, 2.4, 2.4, 2.6, 1.1, 2.0 },
                    { 153, 1.6, 1.6, 2.4, 2.4, 2.0, 2.2, 1.0, 1.6, 1.0, 2.4, 2.4, 1.2, 1.8, 1.9 },
                    { 154, 1.6, 2.1, 2.4, 2.4, 2.0, 2.5, 1.0, 1.6, 1.0, 1.6, 2.4, 2.4, 1.5, 1.9 },
                    { 155, 1.8, 2.4, 2.4, 3.2, 1.5, 4.0, 1.6, 1.4, 1.0, 1.6, 3.2, 1.0, 2.0, 1.9 },
                    { 156, 1.0, 1.6, 2.4, 2.4, 2.4, 2.0, 1.0, 2.0, 1.0, 2.4, 2.4, 1.0, 2.0, 1.9 },
                    { 157, 1.6, 2.0, 2.4, 1.6, 3.2, 1.0, 1.5, 3.2, 3.2, 2.4, 1.6, 1.4, 2.4, 1.2 },
                    { 158, 1.6, 4.0, 2.5, 1.5, 1.5, 1.0, 1.3, 2.0, 2.4, 2.5, 3.2, 3.2, 1.5, 1.0 },
                    { 159, 2.4, 1.6, 2.4, 1.0, 1.6, 1.5, 1.5, 3.2, 2.4, 2.0, 2.4, 1.8, 2.0, 1.0 },
                    { 160, 4.0, 2.4, 1.6, 1.6, 2.4, 1.2, 2.0, 3.2, 2.4, 1.0, 1.0, 4.0, 2.0, 1.4 },
                    { 161, 1.6, 1.6, 1.0, 1.0, 1.6, 1.3, 1.0, 2.4, 2.0, 1.0, 1.0, 2.4, 2.0, 1.5 },
                    { 162, 1.6, 1.0, 1.0, 2.3, 1.0, 2.4, 1.5, 1.0, 1.6, 1.6, 2.4, 2.4, 1.5, 1.0 },
                    { 163, 1.6, 1.5, 2.4, 2.0, 1.0, 1.2, 2.0, 3.2, 2.4, 2.0, 2.0, 3.2, 2.4, 1.3 },
                    { 164, 1.7, 1.0, 2.4, 2.4, 2.0, 3.2, 1.5, 3.2, 1.5, 2.5, 2.5, 1.0, 2.0, 1.8 },
                    { 165, 2.0, 1.5, 2.0, 2.0, 2.0, 1.6, 1.0, 2.4, 2.0, 2.4, 2.5, 1.6, 2.0, 1.8 },
                    { 166, 1.0, 2.4, 2.4, 1.6, 1.0, 1.6, 1.0, 2.4, 2.5, 2.5, 2.4, 3.2, 1.0, 1.4 },
                    { 167, 2.4, 4.0, 4.0, 4.0, 3.0, 3.0, 2.2, 4.0, 4.0, 4.0, 5.0, 4.0, 1.5, 3.5 },
                    { 168, 2.4, 1.6, 1.6, 1.2, 2.2, 1.5, 1.6, 1.8, 1.6, 1.6, 1.0, 1.5, 2.2, 1.6 }
                };

            int[] professionMatrix = { 12, 8, 5, 6, 1, 0, 13, 2, 4, 3, 10, 9, -1, 7, 11 };
            double[,] attributeCost =
                {
                    { 16, 2, 2, 3, 1 }, { 17, 2, 1, 3, 2 }, { 18, 2, 3, 2, 1 }, { 19, 2, 2, 1, 3 },
                    { 20, 2, 1, 2, 3 }, { 21, 2, 2, 1, 3 }
                };

            double[,] baseAttributes =
                {
                    { 6, 6, 6, 6, 6, 6 }, { 3, 15, 6, 6, 10, 3 }, { 3, 3, 3, 15, 6, 10 },
                    { 15, 6, 10, 3, 3, 3 }
                };

            int counter = 0;
            int c2;
            int stat = 0;

            // start with attributes...
            for (counter = 0; counter < 6; counter++)
            {
                stat = (Int32)dynel.Stats[Convert.ToInt32(attributeCost[counter, 0])].BaseValue;
                for (c2 = (Int32)baseAttributes[breed - 1, counter]; c2 < stat; c2++)
                {
                    calc += attributeCost[counter, breed] * c2;
                }
            }

            for (counter = 0; counter < 69; counter++)
            {
                stat = (Int32)dynel.Stats[Convert.ToInt32(skillCosts[counter, 0])].BaseValue - 1;
                for (c2 = 5; c2 <= stat; c2++)
                {
                    calc += Math.Floor(skillCosts[counter, professionMatrix[profession] + 1] * c2);
                }
            }

            return calc;
        }

        /// <summary>
        /// </summary>
        /// <param name="dynel">
        /// </param>
        /// <returns>
        /// </returns>
        public static uint CalculateNanoPoints(IStats dynel)
        {
            int[,] tableProfNanoPoints =
                {
                    { 4, 4, 4, 4, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    { 4, 4, 5, 4, 5, 5, 5, 5, 4, 5, 5, 5, 4, 4 },
                    { 4, 4, 6, 4, 6, 5, 5, 5, 4, 6, 6, 6, 4, 4 },
                    { 4, 4, 7, 4, 6, 6, 5, 5, 4, 7, 7, 7, 4, 4 }, 
                    
                    // TitleLevel 4
                    { 4, 4, 8, 4, 7, 6, 6, 6, 4, 8, 8, 8, 4, 4 }, // TitleLevel 5
                    { 4, 4, 9, 4, 7, 7, 7, 7, 4, 10, 10, 10, 4, 4 }, // TitleLevel 6
                    { 5, 5, 10, 5, 8, 8, 7, 7, 5, 10, 10, 10, 4, 4 }, // TitleLevel 7
                    // TitleLevel 3
                    // TitleLevel 2
                    // TitleLevel 1
                    // Sol|MA|ENG|FIX|AGE|ADV|TRA|CRA|ENF|DOC| NT| MP|KEP|SHA  // geprüfte Prof & TL = Soldier, Martial Artist, Engineer, Fixer
                };

            // Sol|Opi|Nan|Tro
            int[] breedBaseNanoPoints = { 10, 10, 15, 8 };
            int[] breedMultiplicatorNanoPoints = { 3, 3, 4, 2 };
            int[] breedModificatorNanoPoints = { 0, -1, 1, -2 };

            uint breed = dynel.Stats["Breed"].BaseValue;
            uint profession = dynel.Stats["Profession"].BaseValue;
            uint titleLevel = dynel.Stats["TitleLevel"].BaseValue;
            uint level = dynel.Stats["Level"].BaseValue;

            // BreedBaseNP+(Level*(TableProfNP+BreedModiNP))+(NanoEnergyPool*BreedMultiNP))
            return
                (uint)
                (breedBaseNanoPoints[breed - 1]
                 + (dynel.Stats["Level"].Value
                    * (tableProfNanoPoints[titleLevel - 1, profession - 1] + breedModificatorNanoPoints[breed - 1]))
                 + (dynel.Stats["NanoEnergyPool"].Value * breedMultiplicatorNanoPoints[breed - 1]));
        }

        #endregion
    }
}