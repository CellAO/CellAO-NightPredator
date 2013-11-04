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
// Last modified: 2013-11-03 12:45

#endregion

namespace CellAO.Communication.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using Utility;

    public class ZoneComServer
    {
        private IPAddress address;

        private int port;

        private IPEndPoint localEndPoint;

        private TcpListener listener;

        public ZoneComServer(IPAddress ipAddress, int ipPort)
        {
            this.address = ipAddress;
            this.port = ipPort;

            this.localEndPoint = new IPEndPoint(this.address, this.port);
        }

        public void StartServer()
        {
            this.listener = new TcpListener(localEndPoint);
            this.listener.Start();
            WaitForClientConnect();
        }

        private void WaitForClientConnect()
        {
            listener.BeginAcceptTcpClient(new System.AsyncCallback(OnClientConnect), new object());
        }

        public delegate void MessageReceivedHandler(HandleClientRequest request, OnMessageArgs onMessageArgs);

        public delegate void ConnectHandler();

        public delegate void DisconnectHandler();

        public event MessageReceivedHandler MessageReceived;
        public event ConnectHandler OnConnect;
        public event DisconnectHandler OnDisconnect;

        private object streamLockWrite = new object();

        private object streamLockRead = new object();



        private void OnClientConnect(IAsyncResult asyncResult)
        {
            try
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = listener.EndAcceptTcpClient(asyncResult);
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

        private void ClientConnected()
        {
            this.OnConnect();
        }

        private void ClientDisconncted()
        {
            this.OnDisconnect();
        }

        private void ClientMessage(HandleClientRequest request, OnMessageArgs onMessageArgs)
        {
            this.MessageReceived(request, onMessageArgs);
        }
    }

}