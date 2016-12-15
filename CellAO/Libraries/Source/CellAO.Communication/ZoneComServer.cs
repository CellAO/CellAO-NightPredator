#region License

// Copyright (c) 2005-2016, CellAO Team
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

namespace CellAO.Communication.Server
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Net.Sockets;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class ZoneComServer
    {
        #region Fields

        /// <summary>
        /// </summary>
        private IPAddress address;

        /// <summary>
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// </summary>
        private readonly IPEndPoint localEndPoint;

        /// <summary>
        /// </summary>
        private int port;

        /// <summary>
        /// </summary>
        private object streamLockRead = new object();

        /// <summary>
        /// </summary>
        private object streamLockWrite = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="ipAddress">
        /// </param>
        /// <param name="ipPort">
        /// </param>
        public ZoneComServer(IPAddress ipAddress, int ipPort)
        {
            this.address = ipAddress;
            this.port = ipPort;

            this.localEndPoint = new IPEndPoint(this.address, this.port);
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        public delegate void ConnectHandler(object sender, EventArgs e);

        /// <summary>
        /// </summary>
        public delegate void DisconnectHandler(object sender, EventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="onMessageArgs">
        /// </param>
        public delegate void MessageReceivedHandler(object sender, OnMessageArgs e);

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        /// <summary>
        /// </summary>
        public event ConnectHandler OnConnect;

        /// <summary>
        /// </summary>
        public event DisconnectHandler OnDisconnect;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void StartServer()
        {
            this.listener = new TcpListener(this.localEndPoint);
            this.listener.Start();
            this.WaitForClientConnect();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void ClientConnected(object sender, EventArgs e)
        {
            this.OnConnect(sender, e);
        }

        /// <summary>
        /// </summary>
        private void ClientDisconncted(object sender, EventArgs e)
        {
            this.OnDisconnect(sender, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="onMessageArgs">
        /// </param>
        private void ClientMessage(object request, OnMessageArgs onMessageArgs)
        {
            this.MessageReceived(request, onMessageArgs);
        }

        /// <summary>
        /// </summary>
        /// <param name="asyncResult">
        /// </param>
        private void OnClientConnect(IAsyncResult asyncResult)
        {
            try
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = this.listener.EndAcceptTcpClient(asyncResult);
                HandleClientRequest clientRequest = new HandleClientRequest(clientSocket);
                clientRequest.OnConnect += this.ClientConnected;
                clientRequest.OnDisconnect += this.ClientDisconncted;
                clientRequest.MessageReceived += this.ClientMessage;
                clientRequest.StartClient();
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }

            this.WaitForClientConnect();
        }

        /// <summary>
        /// </summary>
        private void WaitForClientConnect()
        {
            this.listener.BeginAcceptTcpClient(new AsyncCallback(this.OnClientConnect), new object());
        }

        #endregion
    }
}