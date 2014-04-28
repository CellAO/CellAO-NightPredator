#region License

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

namespace ChatEngine.CoreServer
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Cell.Core;

    using CellAO.Communication.Messages;
    using CellAO.Database.Dao;

    using ChatEngine.Channels;
    using ChatEngine.CoreClient;
    using ChatEngine.Packets;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility.Config;

    #endregion

    /// <summary>
    /// The server.
    /// </summary>
    public class ChatServer : ServerBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public HashSet<ChannelBase> Channels = new HashSet<ChannelBase>();

        /// <summary>
        /// </summary>
        public Dictionary<uint, Client> ConnectedClients = new Dictionary<uint, Client>();

        /// <summary>
        /// </summary>
        public string MessageOfTheDay = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ChatServer()
        {
            // Global channel
            this.Channels.Add(new GlobalChannel(ChannelFlags.None, ChannelType.General, 1, "Global"));

            // Shopping channels (at the moment just level restricted, no sides)
            this.Channels.Add(new LevelRestrictedChannel(1, 1, 50));
            this.Channels.Add(new LevelRestrictedChannel(2, 51, 150));
            this.Channels.Add(new LevelRestrictedChannel(3, 151, 220));

            // Restricted channels (GM, sided channels)
            this.Channels.Add(new RestrictedChannel(Side.Gm, ChannelFlags.None, ChannelType.GM));
            this.Channels.Add(new RestrictedChannel(Side.Clan, ChannelFlags.None, ChannelType.General));
            this.Channels.Add(new RestrictedChannel(Side.Omni, ChannelFlags.None, ChannelType.General));
            this.Channels.Add(new RestrictedChannel(Side.Neutral, ChannelFlags.None, ChannelType.General));

            // Add a relay channel if needed
            if (ConfigReadWrite.Instance.CurrentConfig.UseIRCRelay)
            {
                this.Channels.Add(
                    new GlobalChannel(
                        ChannelFlags.None,
                        ChannelType.General,
                        5,
                        ConfigReadWrite.Instance.CurrentConfig.RelayIngameChannel));
            }

            this.ClientConnected += this.ClientConnectedToChat;
            this.ClientDisconnected += this.OnClientDisconnect;

            // server welcome message
            this.MessageOfTheDay = ConfigReadWrite.Instance.CurrentConfig.Motd;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public List<ChannelBase> ChannelsByType<T>()
        {
            return this.Channels.Where(x => x is T).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="forced">
        /// </param>
        public void OnClientDisconnect(IClient client, bool forced)
        {
            Client cl = (Client)client;
            if (cl.Character.CharacterId != 0)
            {
                CharacterDao.Instance.SetOffline((int)cl.Character.CharacterId);
                this.ConnectedClients.Remove(cl.Character.CharacterId);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        internal void AddClientToChannels(Client client)
        {
            // Automatically add client to its appropriate channels
            foreach (ChannelBase channel in this.ChannelsByType<GlobalChannel>())
            {
                channel.AddClient(client);
            }

            foreach (ChannelBase channel in this.ChannelsByType<RestrictedChannel>())
            {
                channel.AddClient(client);
            }

            foreach (ChannelBase channel in this.ChannelsByType<LevelRestrictedChannel>())
            {
                channel.AddClient(client);
            }

            foreach (ChannelBase channel in this.ChannelsByType<TeamChannel>())
            {
                channel.AddClient(client);
            }

            foreach (ChannelBase channel in this.ChannelsByType<OrganizationChannel>())
            {
                channel.AddClient(client);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="packet">
        /// </param>
        /// <returns>
        /// </returns>
        internal ChannelBase GetChannel(byte[] packet)
        {
            byte channelType = packet[4];
            uint chanid = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(packet, 5));

            foreach (ChannelBase ce in this.Channels)
            {
                if ((ce.ChannelId == chanid) && ((byte)ce.channelType == channelType))
                {
                    return ce;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="messageObject">
        /// </param>
        internal void ISComDataReceived(object sender, DynamicMessage messageObject)
        {
            var message = messageObject.DataObject as VicinityChatMessage;
            if (message != null)
            {
                this.DistributeVicinityChat(message);
            }
        }

        /// <summary>
        /// The on client connected.
        /// </summary>
        /// <param name="client">
        /// </param>
        protected void ClientConnectedToChat(IClient client)
        {
            Client client1 = (Client)client;

            byte[] welcomePacket = new byte[]
                                   {
                                       0x00, 0x00, 0x00, 0x22, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                                       // Server Salt (32 Bytes)
                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                   };

            byte[] salt = new byte[0x20];
            Random rand = new Random();

            rand.NextBytes(salt);

            client1.ServerSalt = string.Empty;

            for (int i = 0; i < 32; i++)
            {
                // 0x00 Breaks Things
                if (salt[i] == 0)
                {
                    salt[i] = 42; // So we change it to something nicer
                }

                welcomePacket[6 + i] = salt[i];

                client1.ServerSalt += string.Format("{0:x2}", salt[i]);
            }

            client1.Send(welcomePacket);
        }

        /// <summary>
        /// The create client.
        /// </summary>
        /// <returns>
        /// </returns>
        protected override IClient CreateClient()
        {
            return new Client(this);
        }

        /// <summary>
        /// The on receive udp.
        /// </summary>
        /// <param name="num_bytes">
        /// </param>
        /// <param name="buf">
        /// </param>
        /// <param name="ip">
        /// </param>
        protected override void OnReceiveUDP(int num_bytes, byte[] buf, IPEndPoint ip)
        {
        }

        /// <summary>
        /// The on send to.
        /// </summary>
        /// <param name="clientIP">
        /// </param>
        /// <param name="num_bytes">
        /// </param>
        protected override void OnSendTo(IPEndPoint clientIP, int num_bytes)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="vicinityChatMessage">
        /// </param>
        private void DistributeVicinityChat(VicinityChatMessage vicinityChatMessage)
        {
            byte[] packet = MsgVicinity.Create(
                (uint)vicinityChatMessage.SenderId,
                vicinityChatMessage.Text,
                (byte)vicinityChatMessage.MessageType);

            string lookup = CharacterDao.Instance.GetCharacterNameById(vicinityChatMessage.SenderId);
            byte[] nameLookup = NameLookupResult.Create((uint)vicinityChatMessage.SenderId, lookup);

            foreach (int charId in vicinityChatMessage.CharacterIds)
            {
                foreach (Client cli in this.ConnectedClients.Values)
                {
                    if (cli.Character.CharacterId == charId)
                    {
                        if (!cli.KnownClients.Contains((uint)vicinityChatMessage.SenderId))
                        {
                            // Name lookup
                            cli.Send(nameLookup);
                            cli.KnownClients.Add((uint)vicinityChatMessage.SenderId);
                        }

                        cli.Send(packet);
                    }
                }
            }
        }

        #endregion
    }
}