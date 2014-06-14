﻿#region License

// Copyright (c) 2005-2014, CellAO Team
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

#endregion

namespace ChatEngine.Relay.Common
{
    #region Usings ...

    using System.Collections.Generic;

    using IrcDotNet;

    #endregion

    /// <summary>
    /// </summary>
    public static class IrcBotUtilities
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="format">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void SendMessage(
            this IrcLocalUser localUser,
            IIrcMessageTarget target,
            string format,
            params object[] args)
        {
            SendMessage(localUser, new[] { target }, format, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="targets">
        /// </param>
        /// <param name="format">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void SendMessage(
            this IrcLocalUser localUser,
            IList<IIrcMessageTarget> targets,
            string format,
            params object[] args)
        {
            localUser.SendMessage(targets, string.Format(format, args));
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="format">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void SendNotice(
            this IrcLocalUser localUser,
            IIrcMessageTarget target,
            string format,
            params object[] args)
        {
            SendNotice(localUser, new[] { target }, format, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="targets">
        /// </param>
        /// <param name="format">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void SendNotice(
            this IrcLocalUser localUser,
            IList<IIrcMessageTarget> targets,
            string format,
            params object[] args)
        {
            localUser.SendNotice(targets, string.Format(format, args));
        }

        #endregion
    }
}