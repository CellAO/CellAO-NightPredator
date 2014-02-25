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

    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using CellAO.Database.Entities;

    using Dapper;

    #endregion

    /// <summary>
    /// Character Data Access Object
    /// </summary>
    public class CharacterDao : Dao<DBCharacter>
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public static CharacterDao Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharacterDao();
                    _instance.TableName = getTablename();
                }

                return (CharacterDao)_instance;
            }
        }
        
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        /// <param name="buddyId">
        /// </param>
        public void AddBuddy(int characterId, int buddyId)
        {
            DBCharacter character = this.Get(characterId);
            if (character != null)
            {
                // add the buddy to the character 
                character.AddBuddy(buddyId);

                // saves to the database
                // DynamicParameters parameters = new DynamicParameters(character);  new{character.BuddyList should do it too
                // parameters.Add("BuddyList", character.BuddyList); not needed, AddBuddy already adds the id to the CSV string
                // this.Save(character, new { character.BuddyList });

                // New: (we need to pass the id as parameter here)
                this.Save(character, new { character.BuddyList, character.Id });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        public new void Delete(int id, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            // NEW AND FUUUUUCK YOU VS
            using (IDbConnection conn = connection ?? Connector.GetConnection())
            {
                using (IDbTransaction trans = transaction ?? conn.BeginTransaction())
                {
                    // TODO : move these two to their own DAOs

                    // remove this character from organisations
                    conn.Execute("DELETE FROM `organizations` WHERE ID = @id", new { id = id }, trans);

                    // empty this characters inventory
                    conn.Execute("DELETE FROM `inventory` WHERE ID = @id", new { id = id }, trans);

                    // deletes this character
                    base.Delete(id, conn, trans);

                    // TODO: refactor StatDao
                    // delete characters stats
                    StatDao.Instance.Delete(new { type = 50000, Id = id });
                    if (transaction == null)
                    {
                        trans.Commit();
                    }
                }
            }
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
            return this.GetByCharName(name) != null;
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
            return Instance.GetAll(new { username = username });
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
            return Instance.GetAll(new { Name = name }).FirstOrDefault();
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
                name = conn.Query<string>(SQL, new { characterId }).FirstOrDefault();
            }

            if (name == null)
            {
                name = string.Empty;
            }

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
                result = conn.Query<int>(SQL, new { userName, characterId }).Count() == 1;
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        /// <param name="buddyId">
        /// </param>
        public void RemoveBuddy(int characterId, int buddyId)
        {
            DBCharacter character = this.Get(characterId);
            if (character != null)
            {
                // remove the buddy from the character 
                character.RemoveBuddy(buddyId);

                // saves to the database
                // parameters.Add("BuddyList", character.BuddyList); Obsolete, RemoveBuddy removes from character object already

                // CAUTION
                // This could lead to a nasty multithreading issue
                // RemoveBuddy reads, char logs (and saves) out and RemoveBuddy saves over it again
                // this.Save(character, character);

                // New:
                this.Save(character, new { character.BuddyList, character.Id });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="charId">
        /// </param>
        /// <param name="pfType">
        /// </param>
        /// <param name="pfNum">
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        public void SetPlayfield(
            int charId, 
            int pfType, 
            int pfNum, 
            IDbConnection connection = null, 
            IDbTransaction transaction = null)
        {
            // TODO: extend character table for GameServerId, SgId and playfield type
            int rowsAffected = Instance.Save(
                new DBCharacter(), 
                // completely empty one is enough here, parameters have higher priority
                new { Playfield = pfNum, Id = charId });
                
                // Needed to add charId here too, else it cant be passed as a parameter value. not nice

            // should ensure that rowsAffected == 1 otherwise ???
        }

        /// <summary>
        /// Check if character (id) is online
        /// </summary>
        /// <param name="id">
        /// Id of the character
        /// </param>
        /// <returns>
        /// </returns>
        public int IsOnline(int id)
        {
            return this.Get(id).Online;
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        public void SetOffline(int id)
        {
            this.Save(new DBCharacter() { Id = id, Online = 1 }, new { Id = id });
        }

        /// <summary>
        /// Set online flag in table
        /// </summary>
        /// <param name="id">
        /// Id of the character
        /// </param>
        public void SetOnline(int id)
        {
            this.Save(new DBCharacter() { Id = id, Online = 1 }, new { Id = id });
        }
    }
}