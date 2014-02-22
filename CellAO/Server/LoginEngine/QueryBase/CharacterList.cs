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

namespace LoginEngine.QueryBase
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    using LoginEngine.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public static class CharacterList
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="accountName">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CharacterEntry> LoadCharacters(string accountName)
        {
            var characters = new List<CharacterEntry>();

            foreach (DBCharacter ch in CharacterDao.Instance.GetAllForUser(accountName))
            {
                var charentry = new CharacterEntry();
                charentry.Id = ch.Id;
                charentry.Name = ch.Name;
                charentry.Playfield = ch.Playfield;
                charentry.Level = StatDao.GetById(50000, ch.Id, 54).statvalue; // 54 = Level
                charentry.Breed = StatDao.GetById(50000, ch.Id, 4).statvalue; // 4 = Breed
                charentry.Gender = StatDao.GetById(50000, ch.Id, 59).statvalue; // 59 = Sex
                charentry.Profession = StatDao.GetById(50000, ch.Id, 60).statvalue; // 60 = Profession
                characters.Add(charentry);
            }

            return characters;
        }

        #endregion
    }
}