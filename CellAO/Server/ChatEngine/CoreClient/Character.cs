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

namespace ChatEngine.CoreClient
{
    #region Usings ...

    using System;

    using CellAO.Database.Dao;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// The character.
    /// </summary>
    public class Character : CharacterBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        private int characterGMLevel = -1;

        /// <summary>
        /// </summary>
        private int characterSide = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class. 
        /// The character.
        /// </summary>
        /// <param name="characterId">
        /// </param>
        /// <param name="client">
        /// </param>
        public Character(uint characterId, Client client)
            : base(characterId)
        {
            this.Client = client;

            if (characterId != 0)
            {
                this.characterName = CharacterDao.Instance.GetCharacterNameById((int)characterId);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Character"/> class. 
        /// The ~ character.
        /// </summary>
        ~Character()
        {
            if (this.CharacterId != 0)
            {
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int CharacterGMLevel
        {
            get
            {
                if (this.characterGMLevel == -1)
                {
                    this.characterGMLevel = LoginDataDao.GetByCharacterId((int)this.CharacterId).GM;
                }

                return this.characterGMLevel;
            }
        }

        /// <summary>
        /// </summary>
        public int CharacterSide
        {
            get
            {
                if (this.characterSide == -1)
                {
                    try
                    {
                        this.characterSide = StatDao.GetById(50000, (int)this.CharacterId, (int)StatIds.side).statvalue;
                    }
                    catch (Exception)
                    {
                        // Has no side in database yet
                        this.characterSide = (int)Side.Neutral;
                        StatDao.AddStat(50000, (int)this.CharacterId, (int)StatIds.side, (int)Side.Neutral);
                    }
                }

                return this.characterSide;
            }
        }

        /// <summary>
        /// The client.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// </summary>
        public int orgId
        {
            get
            {
                return StatDao.GetById(50000, (int)this.CharacterId, 5).statvalue;
            }
        }

        #endregion
    }
}