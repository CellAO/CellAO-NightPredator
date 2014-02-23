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

namespace ChatEngine.Relay
{
    #region Usings ...

    using System.Data.SqlClient;
    using System.Diagnostics;

    using CellAO.Database.Dao;

    using IrcDotNet;

    #endregion

    /// <summary>
    /// </summary>
    public class CellAoBotUser
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="ircUser">
        /// </param>
        public CellAoBotUser(IrcUser ircUser)
        {
            Debug.Assert(ircUser != null);
            this.IrcUser = ircUser;
        }

        #endregion

        // public CellAoUser CellAoUser
        // {
        // get;
        // private set;
        // }

        #region Public Properties

        /// <summary>
        /// </summary>
        public IrcUser IrcUser { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        #endregion

        // TODO: Figure out how to get ChatServe to gather some information about Users?

        // public TwitterUser TwitterUser
        // {
        // get;
        // private set;
        // }

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="username">
        /// </param>
        /// <param name="password">
        /// </param>
        /// <returns>
        /// </returns>
        public bool LogIn(string username, string password)
        {
            try
            {
                string dUser = LoginDataDao.Instance.GetByUsername(username).Username;

                if (dUser != username)
                {
                    return false;
                }

                this.IsAuthenticated = true;

                return true;
            }
            catch (SqlException)
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        public void LogOut()
        {
            this.IsAuthenticated = false;
        }

        #endregion
    }
}