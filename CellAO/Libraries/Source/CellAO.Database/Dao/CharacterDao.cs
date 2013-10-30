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
// Last modified: 2013-10-30 22:52
// Created:       2013-10-30 17:25

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
    /// </summary>
    public static class CharacterDao
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static IEnumerable<DBCharacter> GetAll()
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBCharacter>(
                        "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="username">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<DBCharacter> GetAllForUser(string username)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBCharacter>(
                        "SELECT ID, Username, Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters WHERE Username=@username",
                        new { username });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<DBCharacter> GetById(int characterId)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                return
                    conn.Query<DBCharacter>(
                        "SELECT Name, FirstName, LastName, Textures0,Textures1,Textures2,Textures3,Textures4,playfield as Playfield, "
                        + "X,Y,Z,HeadingX,HeadingY,HeadingZ,HeadingW FROM characters where id = @id",
                        new { id = characterId });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <returns>
        /// </returns>
        public static int CharExists(string name)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                int temp =
                    conn.Query<int>("SELECT ID FROM characters where Name = @charname", new { charname = name }).Count();
                return temp;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public static void AddCharacter(DBCharacter character)
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

        public static void UpdatePosition(DBCharacter db)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute(
                    "UPDATE characters SET playfield = @Playfield, X = @X, Y = @Y, Z = @Z WHERE id=@Id",
                    new { db.Playfield, db.X, db.Y, db.Z, db.Id });
            }
        }

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
    }
}