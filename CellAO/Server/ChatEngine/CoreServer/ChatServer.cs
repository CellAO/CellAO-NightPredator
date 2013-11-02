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
// Last modified: 2013-11-03 00:29

#endregion

namespace ChatEngine.CoreServer
{
    #region Usings ...

    using System;
    using System.Net;

    using Cell.Core;

    using ChatEngine.CoreClient;

    #endregion

    /// <summary>
    /// The server.
    /// </summary>
    public class ChatServer : ServerBase
    {
        #region Public Properties

        /// <summary>
        /// The maximum pending connections.
        /// </summary>
        public override int MaximumPendingConnections
        {
            get
            {
                return base.MaximumPendingConnections;
            }

            set
            {
                base.MaximumPendingConnections = value;
            }
        }

        /// <summary>
        /// The tcp ip.
        /// </summary>
        public override IPAddress TcpIP
        {
            get
            {
                return base.TcpIP;
            }

            set
            {
                base.TcpIP = value;
            }
        }

        /// <summary>
        /// The tcp port.
        /// </summary>
        public override int TcpPort
        {
            get
            {
                return base.TcpPort;
            }

            set
            {
                base.TcpPort = value;
            }
        }

        /// <summary>
        /// The udp ip.
        /// </summary>
        public override IPAddress UdpIP
        {
            get
            {
                return base.UdpIP;
            }

            set
            {
                base.UdpIP = value;
            }
        }

        /// <summary>
        /// The udp port.
        /// </summary>
        public override int UdpPort
        {
            get
            {
                return base.UdpPort;
            }

            set
            {
                base.UdpPort = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The stop.
        /// </summary>
        public override void Stop()
        {
            base.Stop();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create client.
        /// </summary>
        /// <returns>
        /// </returns>
        protected override IClient CreateClient()
        {
            return new Client(this);
        }

        /// <summary>
        /// The on client connected.
        /// </summary>
        /// <param name="client">
        /// </param>
        protected void OnClientConnected(ClientBase client)
        {
            Client client1 = (Client)client;

            byte[] welcomePacket = new byte[]
                                   {
                                       0x00, 0x00, 0x00, 0x22, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                                       // Server Salt (32 Bytes)
                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                   };

            byte[] salt = new byte[0x20];
            Random rand = new Random();

            rand.NextBytes(salt);

            client1.ServerSalt = string.Empty;

            for (int i = 0; i < 32; i++)
            {
                // 0x00 Breaks Things
                if (salt[i] == 0)
                {
                    salt[i] = 42; // So we change it to something nicer
                }

                welcomePacket[6 + i] = salt[i];

                client1.ServerSalt += string.Format("{0:x2}", salt[i]);
            }

            client1.Send(welcomePacket);
        }

        /// <summary>
        /// The on receive udp.
        /// </summary>
        /// <param name="num_bytes">
        /// </param>
        /// <param name="buf">
        /// </param>
        /// <param name="ip">
        /// </param>
        protected override void OnReceiveUDP(int num_bytes, byte[] buf, IPEndPoint ip)
        {
        }

        /// <summary>
        /// The on send to.
        /// </summary>
        /// <param name="clientIP">
        /// </param>
        /// <param name="num_bytes">
        /// </param>
        protected override void OnSendTo(IPEndPoint clientIP, int num_bytes)
        {
        }

        #endregion
    }
}