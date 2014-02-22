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

using System.Collections.Generic;
namespace CellAO.Database.Entities
{
    /// <summary>
    /// Data object for Character DAO
    /// </summary>
    public class DBCharacter : IDBEntity
    {
        #region Public Properties

        public int Id { get; set; }

        /// <summary>
        /// Coordinates (X)
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Coordinates (Y)
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Coordinates (Z)
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Heading (W)
        /// </summary>
        public float HeadingW { get; set; }

        /// <summary>
        /// Heading (X)
        /// </summary>
        public float HeadingX { get; set; }

        /// <summary>
        /// Heading (Y)
        /// </summary>
        public float HeadingY { get; set; }

        /// <summary>
        /// Heading (Z)
        /// </summary>
        public float HeadingZ { get; set; }


        /// <summary>
        /// Username of the character
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Name of the character
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// First name of the character
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the character
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 0 by default (int)
        /// </summary>
        public int Online { get; set; }

        /// <summary>
        /// Playfield
        /// </summary>
        public int Playfield { get; set; }

        /// <summary>
        /// Texture #0
        /// </summary>
        public int Textures0 { get; set; }

        /// <summary>
        /// Texture #1
        /// </summary>
        public int Textures1 { get; set; }

        /// <summary>
        /// Texture #2
        /// </summary>
        public int Textures2 { get; set; }

        /// <summary>
        /// Texture #3
        /// </summary>
        public int Textures3 { get; set; }

        /// <summary>
        /// Texture #4
        /// </summary>
        public int Textures4 { get; set; }

        /// <summary>
        /// CSV list of players ID that are buddies of meeee
        /// </summary>
        public string BuddyList { get; set; }

        #endregion

        #region Buddies intelligence - cos my buddies are intelligent otherwise they wouldnt be my buddies :)

        public List<int> GetBuddiesIds()
        {
            List<int> buddiesIds = new List<int>();
            foreach (string strId in this.BuddyList.Split(','))
                buddiesIds.Add(int.Parse(strId));
            return buddiesIds;
        }

        public void AddBuddy(int buddyId)
        {
            List<string> buddies = new List<string>();
            buddies.AddRange(this.BuddyList.Split(','));
            if (!buddies.Contains(buddyId.ToString()))
                buddies.Add(buddyId.ToString());

            this.BuddyList = string.Join(",", buddies);
        }

        public void RemoveBuddy(int buddyId)
        {
            List<string> buddies = new List<string>();
            buddies.AddRange(this.BuddyList.Split(','));
            if (buddies.Contains(buddyId.ToString()))
                buddies.Remove(buddyId.ToString());

            this.BuddyList = string.Join(",", buddies);
        }

        #endregion

    }
}