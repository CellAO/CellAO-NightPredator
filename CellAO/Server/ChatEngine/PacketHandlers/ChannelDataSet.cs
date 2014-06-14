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

    using System;

    using ChatEngine.CoreClient;

    #endregion

    /// <summary>
    /// The channel data set.
    /// </summary>
    public class ChannelDataSet
    {
        #region Fields

        /// <summary>
        /// The channelId.
        /// </summary>
        private byte[] channelId = new byte[] { };

        /// <summary>
        /// The data.
        /// </summary>
        private string data = string.Empty;

        /// <summary>
        /// The mute.
        /// </summary>
        private ushort mute;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Channel Data packet read
        /// </summary>
        /// <param name="client">
        /// Client sending
        /// </param>
        /// <param name="packet">
        /// Packet data
        /// </param>
        public void Read(Client client, byte[] packet)
        {
            PacketReader reader = new PacketReader(ref packet);

            reader.ReadUInt16(); // Packet ID
            reader.ReadUInt16(); // Data length
            this.channelId = reader.ReadBytes(5);
            this.mute = reader.ReadUInt16();
            this.data = reader.ReadString();
            client.Server.Debug(
                client,
                "{0} >> ChannelDataSet: Channel {1} {2} Mute: {3} Data: {4}",
                client.Character.characterName,
                this.channelId[0],
                BitConverter.ToUInt32(this.channelId, 1),
                this.mute,
                this.data);
            reader.Finish();
        }

        #endregion
    }
}