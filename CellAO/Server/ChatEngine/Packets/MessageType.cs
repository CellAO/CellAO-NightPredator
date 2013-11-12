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
// Last modified: 2013-11-12 22:18

#endregion

namespace ChatEngine.Packets
{
    /// <summary>
    /// </summary>
    public enum MessageType : short
    {
        // Outgoing packets

        /// <summary>
        /// </summary>
        LoginSeed = 0, 

        /// <summary>
        /// </summary>
        LoginOk = 5, 

        /// <summary>
        /// </summary>
        LoginError = 6, 

        /// <summary>
        /// </summary>
        LoginCharacterList = 7, 

        /// <summary>
        /// </summary>
        CharacterUnknown = 10, 

        /// <summary>
        /// </summary>
        CharacterName = 20, 

        /// <summary>
        /// </summary>
        VicinityMessage = 34, 

        /// <summary>
        /// </summary>
        AnonymousMessage = 35, 

        /// <summary>
        /// </summary>
        SystemMessage = 36, 

        /// <summary>
        /// </summary>
        MessageSystem = 37, 

        /// <summary>
        /// </summary>
        FriendStatus = 40, 

        /// <summary>
        /// </summary>
        FriendRemoved = 41, 

        /// <summary>
        /// </summary>
        PrivateChannelJoin = 52, 

        /// <summary>
        /// </summary>
        PrivateChannelPart = 53, 

        /// <summary>
        /// </summary>
        PrivateChannelKickAll = 54, 

        /// <summary>
        /// </summary>
        PrivateChannelClientJoin = 55, 

        /// <summary>
        /// </summary>
        PrivateChannelClientPart = 56, 

        /// <summary>
        /// </summary>
        ChannelStatus = 60, 

        /// <summary>
        /// </summary>
        ChannelPart = 61, 

        /// <summary>
        /// </summary>
        Pong = 100, 

        /// <summary>
        /// </summary>
        Forward = 110, 

        /// <summary>
        /// </summary>
        AmdMuxInfo = 1100, 

        // Incoming Packets

        /// <summary>
        /// </summary>
        LoginResponse = 2, 

        /// <summary>
        /// </summary>
        LoginSelectChar = 3, 

        /// <summary>
        /// </summary>
        FriendAdd = 40, 

        /// <summary>
        /// </summary>
        FriendRemove = 41, 

        /// <summary>
        /// </summary>
        ChannelUpdate = 64, 

        /// <summary>
        /// </summary>
        ChannelCliMode = 66, 

        /// <summary>
        /// </summary>
        PrivateChannelInvite = 50, 

        /// <summary>
        /// </summary>
        PrivateChannelKick = 51, 

        /// <summary>
        /// </summary>
        ClientModeGet = 70, 

        /// <summary>
        /// </summary>
        ClientModeSet = 71, 

        /// <summary>
        /// </summary>
        Ping = 100, 

        /// <summary>
        /// </summary>
        ChatCommand = 120, 

        /// <summary>
        /// </summary>
        NameLookup = 21, 

        /// <summary>
        /// </summary>
        PrivateGroupMessage = 57, 

        /// <summary>
        /// </summary>
        ChannelMessage = 65, 

        /// <summary>
        /// </summary>
        PrivateMessage = 30
    }
}