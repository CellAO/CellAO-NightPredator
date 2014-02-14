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

namespace ZoneEngine.Core
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Net;

    using Cell.Core;

    using CellAO.Communication.Messages;
    using CellAO.Core.Entities;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;
    using CellAO.Database.Dao;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Component;
    using ZoneEngine.Core.PacketHandlers;

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
            this.ClientDisconnected += this.ZoneServerClientDisconnected;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void DisconnectAllClients()
        {
            foreach (Playfield pf in this.playfields)
            {
                pf.DisconnectAllClients();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="global">
        /// </param>
        /// <returns>
        /// </returns>
        public Dictionary<Identity, string> ListAvailablePlayfields(bool global = true)
        {
            Dictionary<Identity, string> temp = new Dictionary<Identity, string>();
            Dictionary<int, string> names = Playfields.Playfields.PlayfieldNames();

            if (!global)
            {
                foreach (Playfield pf in this.playfields)
                {
                    temp.Add(pf.Identity, names[pf.Identity.Instance]);
                }
            }
            else
            {
                foreach (KeyValuePair<int, string> pf in names)
                {
                    temp.Add(new Identity() { Type = IdentityType.Playfield, Instance = pf.Key }, pf.Value);
                }
            }

            return temp;
        }

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
        /// <param name="messageobject">
        /// </param>
        internal void ProcessISComMessage(DynamicMessage messageobject)
        {
            // Switch to handlers
            if (messageobject.DataObject is ChatCommand)
            {
                this.HandleChatCommand((ChatCommand)messageobject.DataObject);
            }
        }

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

        /// <summary>
        /// </summary>
        /// <param name="chatCommand">
        /// </param>
        private void HandleChatCommand(ChatCommand chatCommand)
        {
            foreach (Playfield playfield in this.playfields)
            {
                IInstancedEntity character =
                    playfield.FindByIdentity(
                        new Identity { Type = IdentityType.CanbeAffected, Instance = chatCommand.CharacterId });
                if (character != null)
                {
                    ChatCommandHandler.Read(
                        chatCommand.ChatCommandString.TrimStart('.'), 
                        (ZoneClient)((Character)character).Client);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="forced">
        /// </param>
        private void ZoneServerClientDisconnected(IClient client, bool forced)
        {
            ZoneClient cli = (ZoneClient)client;
            if (cli.Character != null)
            {
                ((Character)cli.Character).StartLogoutTimer();
                OnlineDao.SetOffline(((IZoneClient)client).Character.Identity.Instance);

                // Will be saved at character dispose too, but just to be sure...
                ((IZoneClient)client).Character.Save();
            }

            cli.Dispose();
        }

        #endregion
    }
}