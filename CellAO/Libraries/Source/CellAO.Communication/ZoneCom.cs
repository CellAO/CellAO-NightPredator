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
// Last modified: 2013-11-04 21:31

#endregion

namespace CellAO.Communication
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Threading;

    using CellAO.Communication.Server;

    #endregion

    /// <summary>
    /// </summary>
    public static class ZoneCom
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static ZoneComClient client = null;

        /// <summary>
        /// </summary>
        private static bool isLinkedWithClient = false;

        /// <summary>
        /// </summary>
        private static bool isLinkedWithServer = false;

        /// <summary>
        /// </summary>
        private static bool? isServer;

        /// <summary>
        /// </summary>
        private static ZoneComServer server = null;

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="args">
        /// </param>
        public delegate void OnServerMessageReceived(HandleClientRequest request, OnMessageArgs args);

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        public delegate void OnClientMessageReceived(OnMessageArgs args);

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public static event OnClientMessageReceived ClientMessageReceived;

        /// <summary>
        /// </summary>
        public static event OnServerMessageReceived ServerMessageReceived;

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public static bool Linked
        {
            get
            {
                return isLinkedWithClient || isLinkedWithServer;
            }
        }

        /// <summary>
        /// </summary>
        public static bool LinkedWithClient
        {
            get
            {
                return isLinkedWithClient;
            }
        }

        /// <summary>
        /// </summary>
        public static bool LinkedWithServer
        {
            get
            {
                return isLinkedWithServer;
            }
        }

        #endregion

        public static bool ClientReconnect = true;

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="ID">
        /// </param>
        /// <param name="data">
        /// </param>
        public static void SendMessage(byte ID, byte[] data)
        {
            OnMessageArgs args = new OnMessageArgs();
            args.ID = ID;
            args.Data = data;
            client.SendData(args);
        }

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="ID">
        /// </param>
        /// <param name="data">
        /// </param>
        public static void SendReply(HandleClientRequest request, byte ID, byte[] data)
        {
            OnMessageArgs args = new OnMessageArgs();
            args.ID = ID;
            args.Data = data;
            request.SendData(args);
        }

        public delegate void OnConnectedToServer();

        public static event OnConnectedToServer ConnectedToServer;

        /// <summary>
        /// </summary>
        /// <param name="address">
        /// </param>
        /// <param name="port">
        /// </param>
        public static void StartClient(IPAddress address, int port, OnClientMessageReceived received)
        {
            if (isServer == true)
            {
                return;
            }

            if (isServer == false)
            {
                Console.WriteLine("Already initialized as Client");
            }
            if (received != null)
            {
                ClientMessageReceived += received;
                serverIP = address;
                serverPort = port;
            }


            client = new ZoneComClient();
            client.OnConnect += ServerConnected;
            client.OnDisconnect += ServerDisconnected;
            client.MessageReceived += client_MessageReceived;
            try
            {
                client.ConnectToServer(address, port);
            }
            catch (Exception)
            {
                ServerDisconnected();
            }
            isServer = false;
        }

        private static void connectedToServer_r()
        {
            ConnectedToServer();
        }

        static void client_MessageReceived(OnMessageArgs onMessageArgs)
        {
            ClientMessageReceived(onMessageArgs);
        }

        /// <summary>
        /// </summary>
        /// <param name="address">
        /// </param>
        /// <param name="port">
        /// </param>
        public static void StartServer(IPAddress address, int port, OnServerMessageReceived received)
        {
            if (isServer == true)
            {
                return;
            }

            if (isServer == false)
            {
                Console.WriteLine("Already initialized as Server");
            }
            ServerMessageReceived += received;
            server = new ZoneComServer(address, port);
            server.OnConnect += ClientConnected;
            server.OnDisconnect += ClientDisconnected;
            server.MessageReceived += server_MessageReceived;

            server.StartServer();
            isServer = true;
        }

        static void server_MessageReceived(HandleClientRequest request, OnMessageArgs onMessageArgs)
        {
            ServerMessageReceived(request, onMessageArgs);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private static void ClientConnected()
        {
            isLinkedWithClient = true;
        }

        /// <summary>
        /// </summary>
        private static void ClientDisconnected()
        {
            isLinkedWithClient = false;
        }

        /// <summary>
        /// </summary>
        private static void ServerConnected()
        {
            isLinkedWithServer = true;
            ConnectedToServer();
        }

        private static IPAddress serverIP;

        private static int serverPort;

        /// <summary>
        /// </summary>
        private static void ServerDisconnected()
        {
            isLinkedWithServer = false;
            isServer = null;
            Thread.Sleep(1000);
            StartClient(serverIP,serverPort, null);
        }

        #endregion
    }
}