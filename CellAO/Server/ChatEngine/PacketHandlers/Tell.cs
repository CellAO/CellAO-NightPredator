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

namespace ChatEngine.PacketHandlers
{
    #region Usings ...

    using CellAO.Database.Dao;

    using ChatEngine.CoreClient;
    using ChatEngine.Packets;

    #endregion

    /// <summary>
    /// The tell.
    /// </summary>
    public static class Tell
    {
        #region Public Methods and Operators

        /// <summary>
        /// Read and process Tell message
        /// </summary>
        /// <param name="client">
        /// Client sending
        /// </param>
        /// <param name="packet">
        /// Packet data
        /// </param>
        public static void Read(Client client, byte[] packet)
        {
            PacketReader reader = new PacketReader(ref packet);

            reader.ReadUInt16();
            reader.ReadUInt16();
            uint playerId = reader.ReadUInt32();
            string message = reader.ReadString();
            client.Server.Debug(client, "{0} >> Tell: PlayerId: {1}", client.Character.characterName, playerId);
            reader.Finish();
            if (client.ChatServer().ConnectedClients.ContainsKey(playerId))
            {
                Client tellClient = (Client)client.ChatServer().ConnectedClients[playerId];
                if (!tellClient.KnownClients.Contains(client.Character.CharacterId))
                {
                    byte[] pname = PlayerName.Create(client, client.Character.CharacterId);
                    tellClient.Send(pname);
                    tellClient.KnownClients.Add(client.Character.CharacterId);

                    // TODO: Check if status bytes are correct even for offline chars
                    client.Send(
                        BuddyOnlineStatus.Create(
                            (uint)tellClient.Character.CharacterId,
                            (uint)CharacterDao.Instance.IsOnline((int)tellClient.Character.CharacterId),
                            new byte[] { 0x00, 0x01, 0x00 }));
                }

                byte[] pgroup = MsgPrivateGroup.Create(client.Character.CharacterId, message, string.Empty);
                tellClient.Send(pgroup);
            }
            else
            {
                byte[] sysmsg = MsgSystem.Create("Player not online.");
                client.Send(sysmsg);
            }
        }

        #endregion
    }
}