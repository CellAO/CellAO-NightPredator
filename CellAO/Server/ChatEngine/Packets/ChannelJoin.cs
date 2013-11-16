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
// Last modified: 2013-11-16 09:36

#endregion

namespace ChatEngine.Packets
{
    #region Usings ...

    using System;

    using ChatEngine.Channels;

    #endregion

    /// <summary>
    /// The channel join.
    /// </summary>
    public static class ChannelJoin
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="channelid">
        /// </param>
        /// <param name="channelname">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <param name="otherdata">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] Create(ulong channelid, string channelname, uint flags, byte[] otherdata)
        {
            PacketWriter writer = new PacketWriter(60);
            writer.Write5of8(channelid);
            writer.WriteString(channelname);
            writer.WriteUInt32(flags);
            writer.WriteBytes(otherdata);
            return writer.Finish();
        }

        /// <summary>
        /// </summary>
        /// <param name="channelType">
        /// </param>
        /// <param name="channelId">
        /// </param>
        /// <param name="channelName">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <param name="otherData">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] Create(
            ChannelType channelType, 
            uint channelId, 
            string channelName, 
            ChannelFlags flags, 
            byte[] otherData)
        {
            PacketWriter writer = new PacketWriter(60);
            writer.WriteByte((byte)channelType);
            writer.WriteUInt32(channelId);
            writer.WriteString(channelName);
            writer.WriteUInt32((UInt32)flags);
            writer.WriteBytes(otherData);
            return writer.Finish();
        }

        #endregion
    }
}