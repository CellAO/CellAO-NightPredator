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
// Last modified: 2013-11-03 10:58

#endregion

namespace ChatEngine.PacketHandlers
{
    #region Usings ...

    using System;
    using System.IO;

    using ChatEngine.CoreClient;
    using ChatEngine.Lists;
    using ChatEngine.Packets;

    #endregion

    /// <summary>
    /// The channel message.
    /// </summary>
    internal static class ChannelMessage
    {
        #region Public Methods and Operators

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="packet">
        /// </param>
        public static void Read(Client client, byte[] packet)
        {
            // TODO: Fix this mess.
            ushort data_length = BitConverter.ToUInt16(new[] { packet[3], packet[2] }, 0);
            byte[] sender_ID = BitConverter.GetBytes(client.Character.CharacterId);
            Array.Reverse(sender_ID);
            MemoryStream m_stream = new MemoryStream();
            m_stream.Write(packet, 0, 9);
            m_stream.Write(sender_ID, 0, 4);
            m_stream.Write(packet, 9, packet.Length - 9);
            m_stream.Capacity = (int)m_stream.Length;
            byte[] message = m_stream.GetBuffer();
            byte[] new_length = BitConverter.GetBytes(message.Length - 4);
            message[2] = new_length[1];
            message[3] = new_length[0];
            m_stream.Close();
            m_stream.Dispose();

            foreach (Client m_client in client.ChatServer().ConnectedClients.Values)
            {
                if (!m_client.KnownClients.Contains(client.Character.CharacterId)
                    && (m_client.Character.CharacterId != client.Character.CharacterId))
                {
                    byte[] pname = PlayerName.New(client, client.Character.CharacterId);
                    m_client.Send(pname);
                    m_client.KnownClients.Add(client.Character.CharacterId);
                }

                m_client.Send(message);
            }

            PacketReader reader = new PacketReader(ref packet);
            reader.ReadUInt16();
            reader.ReadUInt16();
            reader.ReadUInt16();
            reader.ReadUInt16();
            reader.ReadByte();
            string text = reader.ReadString();
            string channelName = ChatChannels.GetChannel(packet).Name;
            ChatLogger.WriteString(channelName, text, client.Character.characterName);
        }

        #endregion
    }
}