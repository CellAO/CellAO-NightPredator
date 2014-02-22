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

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using CellAO.Database.Entities;

    using Dapper;

    using Utility;
    using System.Text;

    #endregion

    /// <summary>
    /// Character Data Access Object
    /// </summary>
    public class CharacterDao : Dao<DBCharacter> // , IDao<DBCharacter> // WTF
    {

        #region Required

        public static new CharacterDao Instance // NEW AND FUCK U LOL
        {
            get
            {
                return (CharacterDao)Dao<DBCharacter>.Instance; // THIS IS VICIOUS
            }
        }

        public CharacterDao() {
            this.TableName = "characters"; //  a bit annoying, maybe move to class attribute at one point in time....
        } 

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        public new void Delete(int id) // NEW AND FUUUUUCK YOU VS
        {

            using (IDbConnection conn = Connector.GetConnection())
            {
                // TODO : move these two to their own DAOs

                // remove this character from organisations
                conn.Execute("DELETE FROM `organizations` WHERE ID = @id", new { id = id });

                // empty this characters inventory
                conn.Execute("DELETE FROM `inventory` WHERE ID = @id", new { id = id });
            }

            // deletes this character
            base.Delete(id);

            // delete characters stats
            StatDao.DeleteStats(50000, id);

        }

         /// <summary>
        /// Does the Character exist
        /// </summary>
        /// <param name="name">
        /// Name of the Character
        /// </param>
        /// <returns>
        /// returns 1 if it exists
        /// </returns>
        public bool ExistsByName(string name)
        {
            return GetByCharName(name) != null; 
        }

        /// <summary>
        /// Load all characters for a specific user
        /// </summary>
        /// <param name="username">
        /// Name of the user
        /// </param>
        /// <returns>
        /// Collection of DBCharacter
        /// </returns>
        public IEnumerable<DBCharacter> GetAllForUser(string username)
        {
            return CharacterDao.Instance.GetAll(new DynamicParameters(new { username = username }));
        }

        /// <summary>
        /// Load a specific Character by name
        /// </summary>
        /// <param name="name">
        /// Name of the Character
        /// </param>
        /// <returns>
        /// DBCharacter object or null
        /// </returns>
        public DBCharacter GetByCharName(string name)
        {
            return CharacterDao.Instance.GetAll(new DynamicParameters(new { name = name })).FirstOrDefault();
        }

       

        /// <summary>
        /// Get the name of a character by id
        /// </summary>
        /// <param name="characterId">
        /// Id of the Character
        /// </param>
        /// <returns>
        /// Name of the Character or string.Empty
        /// </returns>
        public string GetCharacterNameById(int characterId)
        {
            const string SQL = "SELECT Name FROM characters WHERE ID=@characterId";
            string name = null;
            using (IDbConnection conn = Connector.GetConnection())
            {
                name =
                    conn.Query<string>(SQL, new { characterId })
                        .FirstOrDefault();
            }
            if (name == null) 
                name = string.Empty;
            return name;
        }

        /// <summary>
        /// </summary>
        /// <param name="userName">
        /// </param>
        /// <param name="characterId">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsCharacterOnAccount(string userName, uint characterId)
        {
            const string SQL = "SELECT id FROM characters where username=@userName AND id=@characterId";
            bool result;
            using (IDbConnection conn = Connector.GetConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("userName", userName);
                p.Add("characterId", characterId);
                result = conn.Query<int>(SQL, p).Count() == 1;
            }
           return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="charId">
        /// </param>
        /// <param name="pfType">
        /// </param>
        /// <param name="pfNum">
        /// </param>
        public void SetPlayfield(int charId, int pfType, int pfNum)
        {
            // TODO: Use CharacterDao.Instance.Save instead....

            const string SQL = "UPDATE characters SET playfield=@PF WHERE ID=@characterId";
            int rowsAffected = 0;
            using (IDbConnection conn = Connector.GetConnection())
            {
                // TODO: extend character table for GameServerId, SgId and playfield type
                rowsAffected = conn.Execute(
                    SQL, 
                    new { PF = pfNum, characterId = charId });

                // should ensure that rowsAffected == 1 otherwise ???
            }
           
        }

        #region Buddies

        /// <summary>
        /// </summary>
        /// <param name="charId">
        /// </param>
        /// <param name="buddyId">
        /// </param>
        public void AddBuddy(int characterId, int buddyId)
        {
            DBCharacter character = Get(characterId);
            if (character != null)
            {
                // add the buddy to the character 
                character.AddBuddy(buddyId);

                // saves to the database
                DynamicParameters parameters = new DynamicParameters(character);
                parameters.Add("BuddyList", character.BuddyList);
                this.Save( character, parameters );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="charId">
        /// </param>
        /// <param name="buddyId">
        /// </param>
        public void RemoveBuddy(int characterId, int buddyId)
        {
            DBCharacter character = Get(characterId);
            if (character != null)
            {
                // add the buddy to the character 
                character.RemoveBuddy(buddyId);

                // saves to the database
                DynamicParameters parameters = new DynamicParameters(character);
                parameters.Add("BuddyList", character.BuddyList);
                this.Save( character, parameters );
            }
        }

        #endregion
    }
}