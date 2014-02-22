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

namespace ChatEngine.PacketHandlers
{
    #region Usings ...

    using System.Linq;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    using ChatEngine.Channels;
    using ChatEngine.CoreClient;
    using ChatEngine.Packets;

    using Utility;

    #endregion

    /// <summary>
    /// Login Character
    /// </summary>
    public static class LoginCharacter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Read Login Character packet
        /// </summary>
        /// <param name="client">
        /// Client sending
        /// </param>
        /// <param name="packet">
        /// packet data
        /// </param>
        public static void Read(Client client, byte[] packet)
        {
            PacketReader reader = new PacketReader(ref packet);

            reader.ReadUInt16(); // Packet ID
            reader.ReadUInt16(); // Data length
            uint playerId = reader.ReadUInt32();
            client.Server.Debug(
                client, 
                "{0} >> LoginCharacter: PlayerID: {1}", 
                client.Character.characterName, 
                playerId);
            reader.Finish();

            if (client.IsBot)
            {
                OnlineDao.SetOnline((int)playerId);
            }

            DBCharacter character = CharacterDao.Instance.Get((int)playerId);

            client.Character.CharacterId = playerId;
            client.Character.characterName = character.Name;
            client.Character.characterFirstName = character.FirstName;
            client.Character.characterLastName = character.LastName;

            client.ChatServer().AddClientToChannels(client);

            if (client.IsBot)
            {
                // and give client its own name lookup
                byte[] pname = PlayerName.Create(client, client.Character.CharacterId);
                client.Send(pname);

                // send server welcome message to client
                byte[] anonv = MsgAnonymousVicinity.Create(
                    string.Empty, 
                    string.Format(
                        client.ChatServer().MessageOfTheDay, 
                        AssemblyInfoclass.RevisionName + " " + AssemblyInfoclass.AssemblyVersion), 
                    string.Empty);
                client.Send(anonv);

                // TODO: Add Buddies List/BuddyOnlineStatus messages

                foreach (ChannelBase channel in client.Channels)
                {
                    byte[] channelJoin = ChannelJoin.Create(
                        channel.channelType, 
                        channel.ChannelId, 
                        channel.ChannelName, 
                        channel.channelFlags, 
                        new byte[] { 0x00, 0x00 });
                    client.Send(channelJoin);
                }

                if (!client.ChatServer().ConnectedClients.ContainsKey(client.Character.CharacterId))
                {
                    client.ChatServer().ConnectedClients.Add(client.Character.CharacterId, client);
                }

                // add yourself to that list
                client.KnownClients.Add(client.Character.CharacterId);
            }
        }

        #endregion
    }
}