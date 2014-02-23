﻿#region License

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

namespace ChatEngine.CoreClient
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    #endregion

    /// <summary>
    /// </summary>
    public class CharacterBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public uint CharacterId;

        /// <summary>
        /// </summary>
        public string characterFirstName;

        /// <summary>
        /// </summary>
        public string characterLastName;

        /// <summary>
        /// </summary>
        public string characterName;

        /// <summary>
        /// </summary>
        public string orgName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        public CharacterBase(uint characterId)
        {
            this.CharacterId = characterId;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool ReadNames()
        {
            bool success = false;
            DBCharacter character = CharacterDao.Instance.Get((int)this.CharacterId);
            if (character != null)
            {
                this.characterName = character.Name;
                this.characterFirstName = character.FirstName;
                this.characterLastName = character.LastName;

                DBStats clan = StatDao.Instance.GetById(50000, (int)this.CharacterId, 5);
                if (clan != null)
                {
                    DBOrganization org = OrganizationDao.GetOrganizationData(clan.StatValue);
                    this.orgName = org.Name;
                }

                success = true;
            }
           
            return success;
        }

        #endregion
    }
}