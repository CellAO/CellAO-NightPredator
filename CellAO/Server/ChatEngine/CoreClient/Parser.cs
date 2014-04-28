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

namespace ChatEngine
{
    #region Usings ...

    using ChatEngine.CoreClient;
    using ChatEngine.PacketHandlers;

    #endregion

    /// <summary>
    /// The parser.
    /// </summary>
    public class Parser
    {
        #region Public Methods and Operators

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="packet">
        /// </param>
        /// <param name="messageNumber">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Parse(Client client, byte[] packet, ushort messageNumber)
        {
            switch (messageNumber)
            {
                case 0:
                    Authenticate.Read(client, ref packet);
                    break;
                case 2:

                    // Chat bot wants to authenticate
                    AuthenticateBot.Read(client, packet);
                    break;
                case 3:
                    LoginCharacter.Read(client, packet);
                    break;
                case 21:
                    PlayerNameLookup.Read(client, packet);
                    break;
                case 30:
                    Tell.Read(client, packet);
                    break;
                case 40:
                    BuddyAdd.Read(client, packet);
                    break;
                case 41:
                    new BuddyRemove().Read(client, packet);
                    break;
                case 42:
                    new OnlineStatus().Read(client, packet);
                    break;
                case 50:
                    new PrivateGroupInvitePlayer().Read(client, packet);
                    break;
                case 51:
                    new PrivateGroupKickPlayer().Read(client, packet);
                    break;
                case 52:
                    new PrivateGroupJoin().Read(client, packet);
                    break;
                case 53:
                    new PrivateGroupLeave().Read(client, packet);
                    break;
                case 54:

                    // this packet should have no data to read
                    PrivateGroupKickEveryone.Read(client, ref packet);
                    break;
                case 57:
                    new PrivateGroupMessage().Read(client, packet);
                    break;
                case 64:
                    new ChannelDataSet().Read(client, packet);
                    break;
                case 65:
                    ChannelMessage.Read(client, packet);
                    break;
                case 66:
                    new ChannelMode().Read(client, packet);
                    break;
                case 70:
                case 71:

                    // should never get these messages (ClimodeGet and ClimodeSet)
                    break;
                case 100:

                    // Ping
                    client.Send(packet);
                    break;
                case 110:
                    break;
                case 120:
                    new ChatCommand().Read(client, packet);
                    break;
                default:
                    client.Server.Warning(client, "Client sent unknown message {0}", messageNumber.ToString());
                    return false;
            }

            return true;
        }

        #endregion
    }
}