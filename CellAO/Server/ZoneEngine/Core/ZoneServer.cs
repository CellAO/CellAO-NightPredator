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

namespace ZoneEngine.Core
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Net;

    using Cell.Core;

    using CellAO.Core.Playfields;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Component;

    #endregion

    /// <summary>
    /// </summary>
    [Export]
    public class ZoneServer : ServerBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public HashSet<ZoneClient> Clients = new HashSet<ZoneClient>();

        /// <summary>
        /// </summary>
        public int Id;

        /// <summary>
        /// </summary>
        private readonly ClientFactory clientFactory;

        /// <summary>
        /// </summary>
        private readonly List<IPlayfield> playfields = new List<IPlayfield>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="clientFactory">
        /// </param>
        [ImportingConstructor]
        public ZoneServer(ClientFactory clientFactory)
        {
            // TODO: Get the Server id from chatengine or config file
            this.Id = 0x356;
            this.clientFactory = clientFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        public IPlayfield PlayfieldById(int id)
        {
            // TODO: This needs to be changed to check for whole Identity
            foreach (IPlayfield pf in this.playfields)
            {
                if (pf.Identity.Instance == id)
                {
                    return pf;
                }
            }

            this.CreatePlayfield(new Identity { Instance = id });
            return this.PlayfieldById(id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override IClient CreateClient()
        {
            return this.clientFactory.Create(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="playfieldIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        protected IPlayfield CreatePlayfield(Identity playfieldIdentity)
        {
            var temp = new Playfield(this, playfieldIdentity);
            this.playfields.Add(temp);
            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="num_bytes">
        /// </param>
        /// <param name="buf">
        /// </param>
        /// <param name="ip">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void OnReceiveUDP(int num_bytes, byte[] buf, IPEndPoint ip)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="clientIP">
        /// </param>
        /// <param name="num_bytes">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void OnSendTo(IPEndPoint clientIP, int num_bytes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}