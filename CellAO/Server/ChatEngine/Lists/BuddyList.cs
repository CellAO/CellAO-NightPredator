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

namespace ChatEngine.Lists
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    #endregion

    /// <summary>
    /// Buddy List
    /// </summary>
    public static class BuddyList
    {
        #region Public Methods and Operators

        /// <summary>
        /// The load buddy list.
        /// </summary>
        /// <param name="charId">
        /// The char Id.
        /// </param>
        /// <returns>
        /// Buddy list
        /// </returns>
        public static List<int> LoadBuddyList(int charId)
        {
            return CharacterDao.Instance.Get(charId).GetBuddiesIds();
        }

        // LoadRecentMsgsList unused?

        /// <summary>
        /// The load recent msgs list.
        /// </summary>
        /// <param name="charId">
        /// The char Id.
        /// </param>
        /// <returns>
        /// List of received messages
        /// </returns>
        public static IEnumerable<DBRecentMessages> LoadRecentMsgsList(uint charId)
        {
            return RecentMessagesDao.LoadRecentMessageses((int)charId);
        }

        #endregion
    }
}