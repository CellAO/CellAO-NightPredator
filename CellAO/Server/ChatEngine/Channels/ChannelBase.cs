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
// Last modified: 2013-11-12 22:19

#endregion

namespace ChatEngine.Channels
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using Cell.Core;

    using ChatEngine.CoreClient;

    #endregion

    /// <summary>
    /// </summary>
    public class ChannelBase : IChannelBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public uint ChannelId { get; private set; }

        /// <summary>
        /// </summary>
        private ChannelFlags channelFlags;

        /// <summary>
        /// </summary>
        private string channelName;

        /// <summary>
        /// </summary>
        private HashSet<IClient> clients = new HashSet<IClient>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="flags">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="id">
        /// </param>
        /// <param name="name">
        /// </param>
        public ChannelBase(ChannelFlags flags, ChannelType type, uint id, string name = "")
        {
            this.channelFlags = flags;
            this.channelType = type;
            this.ChannelId = id;
            this.channelName = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public ChannelType channelType { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual bool AddClient(IClient client)
        {
            if (!this.clients.Contains(client))
            {
                this.clients.Add(client);

                // TODO: Send feedback to client?
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceClient">
        /// </param>
        /// <param name="text">
        /// </param>
        /// <param name="blob">
        /// </param>
        public void ChannelMessage(Client sourceClient, string text, string blob = "")
        {
            byte[] channelMessageBytes = Packets.ChannelMessage.Create(
                this, 
                sourceClient.Character.CharacterId, 
                text, 
                blob);
            foreach (IClient client in this.clients)
            {
                client.Send(channelMessageBytes);
            }
        }

        /// <summary>
        /// </summary>
        public void CloseChannel()
        {
            foreach (IClient client in this.clients)
            {
                // TODO: Send feedback to client
            }

            this.clients.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        public bool RemoveClient(IClient client)
        {
            if (this.clients.Contains(client))
            {
                this.clients.Remove(client);

                // TODO: Send feedback to client
                return true;
            }

            return false;
        }

        #endregion
    }
}