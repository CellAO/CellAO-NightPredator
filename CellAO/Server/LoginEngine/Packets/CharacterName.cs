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

namespace LoginEngine.Packets
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Text;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class CharacterName
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static string mandatoryVowel = "aiueo"; /* 5 chars */

        /// <summary>
        /// </summary>
        private static string optionalOrdCon = "vybcfghjqktdnpmrlws"; /* 19 chars */

        /// <summary>
        /// </summary>
        private static string optionalOrdEnd = "nmrlstyzx"; /* 9 chars */

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// </summary>
        public int Breed { get; set; }

        /// <summary>
        /// </summary>
        public int Fatness { get; set; }

        /// <summary>
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// </summary>
        public int HeadMesh { get; set; }

        /// <summary>
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// </summary>
        public int MonsterScale { get; set; }

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public int Profession { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        private int[] Abis { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int CheckAgainstDatabase()
        {
            /* name in use */
            if (CharacterDao.CharExists(this.Name) > 0)
            {
                return 0;
            }

            return this.CreateNewChar();
        }

        /// <summary>
        /// </summary>
        /// <param name="charid">
        /// </param>
        public void DeleteChar(int charid)
        {
            try
            {
                CharacterDao.DeleteCharacter(charid);
            }
            catch (Exception e)
            {
                Console.WriteLine(this.Name + e.Message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="profession">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetRandomName(Profession profession)
        {
            var random = new Random();
            byte randomNameLength = 0;
            var randomLength = (byte)random.Next(3, 8);
            var sb = new StringBuilder();
            while (randomNameLength <= randomLength)
            {
                if (random.Next(14) > 4)
                {
                    sb.Append(optionalOrdCon.Substring(random.Next(0, 18), 1));
                    randomNameLength++;
                }

                sb.Append(mandatoryVowel.Substring(random.Next(0, 4), 1));
                randomNameLength++;

                if (random.Next(14) <= 4)
                {
                    continue;
                }

                sb.Append(optionalOrdEnd.Substring(random.Next(0, 8), 1));
                randomNameLength++;
            }

            string name = sb.ToString();
            name = char.ToUpper(name[0]) + name.Substring(1);
            return name;
        }

        /// <summary>
        /// </summary>
        /// <param name="startInSL">
        /// </param>
        /// <param name="charid">
        /// </param>
        public void SendNameToStartPlayfield(bool startInSL, int charid)
        {
            DBCharacter dbCharacter = new DBCharacter { Id = charid, Playfield = 4001, X = 850, Y = 43, Z = 565 };
            if (!startInSL)
            {
                dbCharacter.Playfield = 4582;
                dbCharacter.X = 939;
                dbCharacter.Y = 20;
                dbCharacter.Z = 732;
            }

            CharacterDao.UpdatePosition(dbCharacter);
            CharacterDao.SetPlayfield(dbCharacter.Id, (int)IdentityType.Playfield, dbCharacter.Playfield);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private int CreateNewChar()
        {
            int charID = 0;
            switch (this.Breed)
            {
                case 0x1: /* solitus */
                    this.Abis = new[] { 6, 6, 6, 6, 6, 6 };
                    break;
                case 0x2: /* opifex */
                    this.Abis = new[] { 3, 3, 10, 6, 6, 15 };
                    break;
                case 0x3: /* nanomage */
                    this.Abis = new[] { 3, 10, 6, 15, 3, 3 };
                    break;
                case 0x4: /* atrox */
                    this.Abis = new[] { 15, 3, 3, 3, 10, 6 };
                    break;
                default:
                    Console.WriteLine("unknown breed: ", this.Breed);
                    break;
            }

            /*
             * Note, all default values are not specified here as defaults are handled
             * in the CharacterStats Class for us automatically. Also minimises SQL
             * usage for default stats that are never changed from their default value
             *           ~NV
             */
            // Delete orphaned stats for charID
            StatDao.DeleteStats(50000, charID);
            try
            {
                CharacterDao.AddCharacter(
                    new DBCharacter
                    {
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        Name = this.Name,
                        Username = this.AccountName,
                    });
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                return 0;
            }

            try
            {
                /* select new char id */
                charID = CharacterDao.GetByCharName(this.Name).Id;
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                return 0;
            }

            List<DBStats> stats = new List<DBStats>();

            // Transmit GM level into stats table
            stats.Add(
                new DBStats
                {
                    type = 50000,
                    instance = charID,
                    statid = 215,
                    statvalue = LoginDataDao.GetByUsername(this.AccountName).GM
                });

            // Flags
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 0, statvalue = 20 });

            // Level
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 54, statvalue = 1 });

            // SEXXX
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 59, statvalue = this.Gender });

            // Headmesh
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 64, statvalue = this.HeadMesh });

            // MonsterScale
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 360, statvalue = this.MonsterScale });

            // Visual Sex (even better ^^)
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 369, statvalue = this.Gender });

            // Breed
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 4, statvalue = this.Breed });

            // Visual Breed
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 367, statvalue = this.Breed });

            // Profession / 60
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 60, statvalue = this.Profession });

            // VisualProfession / 368
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 368, statvalue = this.Profession });

            // Fatness / 47
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 47, statvalue = this.Fatness });

            // Strength / 16
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 16, statvalue = this.Abis[0] });

            // Psychic / 21
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 21, statvalue = this.Abis[1] });

            // Sense / 20
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 20, statvalue = this.Abis[2] });

            // Intelligence / 19
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 19, statvalue = this.Abis[3] });

            // Stamina / 18
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 18, statvalue = this.Abis[4] });

            // Agility / 17
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 17, statvalue = this.Abis[5] });

            // Set HP and NP auf 1
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 1, statvalue = 1 });
            stats.Add(new DBStats { type = 50000, instance = charID, statid = 214, statvalue = 1 });

            stats.Add(new DBStats { type = 50000, instance = charID, statid = 389, statvalue = LoginDataDao.GetByUsername(this.AccountName).Expansions });

            StatDao.BulkReplace(stats);

            return charID;
        }

        #endregion
    }
}