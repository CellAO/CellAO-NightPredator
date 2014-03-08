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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Net.Sockets;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.Messages.SystemMessages;

    using Utility.Config;

    #endregion

    /// <summary>
    ///     Chat server info packet writer
    /// </summary>
    // [Export(typeof(IHandleMessage))]
    public class ChatServerInfoMessageHandler : BaseMessageHandler<ChatServerInfoMessage, ChatServerInfoMessageHandler>
    {
        /// <summary>
        /// </summary>
        public ChatServerInfoMessageHandler()
        {
            this.Direction = MessageHandlerDirection.OutboundOnly;
        }

        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public void Send(ICharacter character)
        {
            this.Send(character, Filler(character));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        private static MessageDataFiller Filler(ICharacter character)
        {
            /* get chat settings from config */
            string chatServerIp = string.Empty;
            IPAddress tempIp;
            if (IPAddress.TryParse(ConfigReadWrite.Instance.CurrentConfig.ChatIP, out tempIp))
            {
                chatServerIp = ConfigReadWrite.Instance.CurrentConfig.ChatIP;
            }
            else
            {
                IPHostEntry chatHost = Dns.GetHostEntry(ConfigReadWrite.Instance.CurrentConfig.ChatIP);
                foreach (IPAddress ip in chatHost.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        chatServerIp = ip.ToString();
                        break;
                    }
                }
            }

            int chatPort = Convert.ToInt32(ConfigReadWrite.Instance.CurrentConfig.ChatPort);

            return x =>
            {
                x.HostName = chatServerIp;
                x.Port = chatPort;
            };
        }

        #endregion
    }
}