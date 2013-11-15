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
// Last modified: 2013-11-16 00:16

#endregion

namespace ChatEngine.Packets
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    #endregion

    /// <summary>
    /// The account character list.
    /// </summary>
    public static class AccountCharacterList
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="username">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] Create(string username)
        {
            PacketWriter writer = new PacketWriter(0x07);
            IEnumerable<DBCharacter> chars = CharacterDao.GetAllForUser(username);

            byte[] numberOfCharacters = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)chars.Count()));
            writer.WriteBytes(numberOfCharacters);
            foreach (DBCharacter character in chars)
            {
                writer.WriteUInt32((UInt32)character.Id);
            }

            writer.WriteBytes(numberOfCharacters);
            foreach (DBCharacter character in chars)
            {
                writer.WriteString(character.Name);
            }

            writer.WriteBytes(numberOfCharacters);
            foreach (DBCharacter character in chars)
            {
                writer.WriteUInt32((UInt32)StatDao.GetById(50000, character.Id, 54).statvalue);
            }

            writer.WriteBytes(numberOfCharacters);
            foreach (DBCharacter character in chars)
            {
                // TODO: Find out what to put in here, for now lets go with 0x00000000
                writer.WriteUInt32(0);
            }

            return writer.Finish();
        }

        #endregion
    }
}