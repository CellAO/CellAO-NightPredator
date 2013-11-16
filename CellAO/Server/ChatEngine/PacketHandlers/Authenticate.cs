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

namespace ChatEngine.PacketHandlers
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    using AO.Core.Encryption;

    using CellAO.Communication;

    using ChatEngine.CoreClient;
    using ChatEngine.Packets;

    using Utility;

    #endregion

    /// <summary>
    /// The authenticate.
    /// </summary>
    internal static class Authenticate
    {
        #region Public Methods and Operators

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="packet">
        /// </param>
        public static void Read(Client client, ref byte[] packet)
        {
            MemoryStream m_stream = new MemoryStream(packet);
            BinaryReader m_reader = new BinaryReader(m_stream);

            // now we should do password check and then send OK or Error
            // sending OK now
            m_stream.Position = 12;

            short userNameLength = IPAddress.NetworkToHostOrder(m_reader.ReadInt16());
            string userName = Encoding.ASCII.GetString(m_reader.ReadBytes(userNameLength));
            short loginKeyLength = IPAddress.NetworkToHostOrder(m_reader.ReadInt16());
            string loginKey = Encoding.ASCII.GetString(m_reader.ReadBytes(loginKeyLength));

            uint characterId = BitConverter.ToUInt32(new[] { packet[11], packet[10], packet[9], packet[8] }, 0);

            LoginEncryption loginEncryption = new LoginEncryption();

            if (loginEncryption.IsValidLogin(loginKey, client.ServerSalt, userName)
                && loginEncryption.IsCharacterOnAccount(userName, characterId))
            {
                byte[] loginok = LoginOk.Create();
                client.Send(loginok);
            }
            else
            {
                byte[] loginerr = LoginError.Create();
                client.Send(loginerr);
                client.Server.DisconnectClient(client);
                byte[] invalid = BitConverter.GetBytes(characterId);

                ZoneCom.SendMessage(99, invalid);
                return;
            }

            // save characters ID in client - note, this is usually 0 if it is a chat client connecting
            client.Character = new Character(characterId, client);

            // add client to connected clients list
            if (!client.ChatServer().ConnectedClients.ContainsKey(client.Character.CharacterId))
            {
                client.ChatServer().ConnectedClients.Add(client.Character.CharacterId, client);
            }

            // add yourself to that list
            client.KnownClients.Add(client.Character.CharacterId);

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

            client.ChatServer().AddClientToChannels(client);
        }

        #endregion
    }
}