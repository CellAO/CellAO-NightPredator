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

    using System;

    using ChatEngine.Channels;
    using ChatEngine.CoreClient;
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
            byte[] senderId = BitConverter.GetBytes(client.Character.CharacterId);
            Array.Reverse(senderId);

            byte[] newpacket = new byte[packet.Length + 4];

            Array.Copy(packet, 0, newpacket, 0, 9);
            Array.Copy(senderId, 0, newpacket, 9, 4);
            Array.Copy(packet, 9, newpacket, 13, packet.Length - 9);
            newpacket[2] = (byte)(packet.Length >> 8);
            newpacket[3] = (byte)packet.Length;

            ChannelBase channel = client.ChatServer().GetChannel(packet);

            foreach (Client recipient in client.ChatServer().ConnectedClients.Values)
            {
                if (recipient.Channels.Contains(channel))
                {
                    if (!recipient.KnownClients.Contains(client.Character.CharacterId)
                        && (recipient.Character.CharacterId != client.Character.CharacterId))
                    {
                        byte[] pname = PlayerName.Create(client, client.Character.CharacterId);
                        recipient.Send(pname);
                        recipient.KnownClients.Add(client.Character.CharacterId);
                    }

                    recipient.Send(newpacket);
                }
            }

            PacketReader reader = new PacketReader(ref packet);
            reader.ReadUInt16();
            reader.ReadUInt16();
            reader.ReadUInt16();
            reader.ReadUInt16();
            reader.ReadByte();
            string text = reader.ReadString();
            string channelName = channel.ChannelName;

            client.ChannelMessageReceived(channel, client.Character.characterName, text);
            ChatLogger.WriteString(channelName, text, client.Character.characterName);
        }

        #endregion
    }
}