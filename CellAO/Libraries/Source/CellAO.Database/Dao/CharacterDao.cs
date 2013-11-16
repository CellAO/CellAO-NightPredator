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

    #endregion

    /// <summary>
    /// Character Data Access Object
    /// </summary>
    public static class CharacterDao
    {
        #region Public Methods and Operators

        /// <summary>
        /// Insert a new character
        /// </summary>
        /// <param name="character">
        /// The DBCharacter object to store
        /// </param>
        public static void AddCharacter(DBCharacter character)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "INSERT INTO characters (Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4"
                        + ",playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW,Username) VALUES (@Name, @FirstName, "
                        + "@LastName, @Textures0, @Textures1, @Textures3, @Textures4, @Playfield, @X, @Y, @Z, @HeadingX, @HeadingY, "
                        + "@HeadingZ, @HeadingW, @Online,@username)", 
                        new
                        {
                            character.Name, 
                            character.FirstName, 
                            character.LastName, 
                            character.Textures0, 
                            character.Textures1, 
                            character.Textures2, 
                            character.Textures3, 
                            character.Textures4, 
                            character.Playfield, 
                            character.X, 
                            character.Y, 
                            character.Z, 
                            character.HeadingX, 
                            character.HeadingY, 
                            character.HeadingZ, 
                            character.HeadingW, 
                            Online = 0, 
                            username = character.Username
                        });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
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
        public static int CharExists(string name)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    int temp =
                        conn.Query<int>("SELECT ID FROM characters where Name = @charname", new { charname = name })
                            .Count();
                    return temp;
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="charid">
        /// </param>
        public static void DeleteCharacter(int charid)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("charid", charid);
                    conn.Execute("DELETE FROM `characters` WHERE ID = @charid", p);
                    conn.Execute("DELETE FROM `organizations` WHERE ID = @charid", p);
                    conn.Execute("DELETE FROM `inventory` WHERE ID = @charid", p);
                }

                StatDao.DeleteStats(50000, charid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Load all Character data
        /// </summary>
        /// <returns>
        /// Collection of DBCharacter
        /// </returns>
        public static IEnumerable<DBCharacter> GetAll()
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBCharacter>(
                            "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters");
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
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
        public static IEnumerable<DBCharacter> GetAllForUser(string username)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBCharacter>(
                            "SELECT ID, Username, Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters WHERE Username=@username", 
                            new { username });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
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
        public static DBCharacter GetByCharName(string name)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return conn.Query<DBCharacter>("SELECT * FROM characters WHERE Name=@name", new { name }).First();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Load a Character by id
        /// </summary>
        /// <param name="characterId">
        /// Id of the Character
        /// </param>
        /// <returns>
        /// DBCharacter object
        /// </returns>
        public static IEnumerable<DBCharacter> GetById(int characterId)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<DBCharacter>(
                            "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, "
                            + "X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW, UserName FROM characters where id = @id", 
                            new { id = characterId });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
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
        public static string GetCharacterNameById(int characterId)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    return
                        conn.Query<string>("SELECT Name FROM characters WHERE ID=@characterId", new { characterId })
                            .Single();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userName">
        /// </param>
        /// <param name="characterId">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsCharacterOnAccount(string userName, uint characterId)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("userName", userName);
                    p.Add("characterId", characterId);
                    return
                        conn.Query<int>("SELECT id FROM characters where username=@userName AND id=@characterId", p)
                            .Count() == 1;
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        /// <summary>
        /// Write back the position of the Characer
        /// </summary>
        /// <param name="character">
        /// DBCharacte object
        /// </param>
        public static void UpdatePosition(DBCharacter character)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute(
                        "UPDATE characters SET playfield = @Playfield, X = @X, Y = @Y, Z = @Z WHERE id=@Id", 
                        new { character.Playfield, character.X, character.Y, character.Z, character.Id });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
        }

        #endregion
    }
}