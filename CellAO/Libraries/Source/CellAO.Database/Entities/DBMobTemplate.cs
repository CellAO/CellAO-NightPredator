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

using System.Data.Linq;
namespace CellAO.Database.Dao
{
    using CellAO.Database.Entities;

    /// <summary>
    /// Data object for mobTemplate data
    /// </summary>
    [Tablename("mobtemplate")]
    public class DBMobTemplate:IDBEntity
    {
        /// <summary>
        /// Primary Key, 4 letter code
        /// </summary>
        public string Hash { get; set; }

        public int MinLvl { get; set; }

        public int MaxLvl { get; set; }

        public int Side { get; set; }

        public int Fatness { get; set; }

        public int Breed { get; set; }

        public int Sex { get; set; }

        public int Race { get; set; }

        public string Name { get; set; }

        public int Flags { get; set; }

        public int NPCFamily { get; set; }

        public int Health { get; set; }

        public int MonsterData { get; set; }

        public int MonsterScale { get; set; }

        public int TextureHands { get; set; }

        public int TextureBody { get; set; }

        public int TextureFeet { get; set; }

        public int TextureArms { get; set; }

        public int TextureLegs { get; set; }

        public int HeadMesh { get; set; }

        public Binary MobMeshs { get; set; }

        public Binary AdditionalMeshs { get; set; }

        /// <summary>
        /// Comma-delimited item hashes from mobdroptable. Hashes can be added to form a union of items. e.g. HASH01+HASH02, HASH03+HASH04, HASH05
        /// </summary>
        public string DropHashes { get; set; }

        /// <summary>
        /// Comma-delimited slot for each item hash, must have same number of commas as above
        /// </summary>
        public string DropSlots { get; set; }

        /// <summary>
        /// Comma-delimited % * 100 for each item hash. 10000 = 100.0%, 1250 = 12.5% etc., must have same number of commas as above
        /// </summary>
        public string DropRates { get; set; }

        public int Id { get; set; }
    }
}