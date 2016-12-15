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

namespace CellAO.Communication
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Threading;

    using CellAO.Communication.Server;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public static class ZoneCom
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static bool ClientReconnect = true;

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

        /// <summary>
        /// </summary>
        private static IPAddress serverIP;

        /// <summary>
        /// </summary>
        private static int serverPort;

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        public delegate void OnClientConnectedToServer(object sender, EventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="args">
        /// </param>
        public delegate void OnReceivedMessageFromClient(object sender, OnMessageArgs e);

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        public delegate void OnReceivedMessageFromServer(object sender, OnMessageArgs e);

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public static event OnClientConnectedToServer ClientConnectedToServer;

        /// <summary>
        /// </summary>
        public static event OnReceivedMessageFromClient ReceivedMessageFromClient;

        /// <summary>
        /// </summary>
        public static event OnReceivedMessageFromServer ReceivedMessageFromServer;

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

        /// <summary>
        /// </summary>
        /// <param name="address">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <param name="received">
        /// </param>
        public static void StartClient(IPAddress address, int port, OnReceivedMessageFromServer received)
        {
            if (isServer == true)
            {
                LogUtil.Debug(DebugInfoDetail.Error, "ZoneCom already initialized as Server.");
                return;
            }

            if (isServer == false)
            {
                LogUtil.Debug(DebugInfoDetail.Error, "ZoneCom already initialized as Client.");
                return;
            }

            if (received != null)
            {
                ReceivedMessageFromServer += received;
                serverIP = address;
                serverPort = port;
            }

            client = new ZoneComClient();
            client.OnConnect += ClientToServerConnected;
            client.OnDisconnect += ClientToServerDisconnected;
            client.MessageReceived += ClientMessageReceived;
            try
            {
                client.ConnectToServer(address, port);
            }
            catch (Exception)
            {
                ClientToServerDisconnected(null, EventArgs.Empty);
                return;
            }

            isServer = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="address">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <param name="received">
        /// </param>
        public static void StartServer(IPAddress address, int port, OnReceivedMessageFromClient received)
        {
            if (isServer == true)
            {
                return;
            }

            if (isServer == false)
            {
                Console.WriteLine("Already initialized as Server");
            }

            ReceivedMessageFromClient += received;
            server = new ZoneComServer(address, port);
            server.OnConnect += ServerToClientConnected;
            server.OnDisconnect += ServerToClientDisconnected;
            server.MessageReceived += ServerMessageReceived;
            try
            {
                server.StartServer();
            }
            catch (Exception)
            {
                ServerToClientDisconnected(null, null);
                return;
            }

            isServer = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private static void ClientToServerConnected(object sender, EventArgs e)
        {
            isLinkedWithServer = true;
            ClientConnectedToServer(sender, e);
        }

        /// <summary>
        /// </summary>
        private static void ClientToServerDisconnected(object sender, EventArgs e)
        {
            isLinkedWithServer = false;
            isServer = null;
            Thread.Sleep(1000);
            StartClient(serverIP, serverPort, null);
        }

        /// <summary>
        /// </summary>
        private static void ServerToClientConnected(object sender, EventArgs e)
        {
            isLinkedWithClient = true;
        }

        /// <summary>
        /// </summary>
        private static void ServerToClientDisconnected(object sender, EventArgs e)
        {
            isLinkedWithClient = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="onMessageArgs">
        /// </param>
        private static void ClientMessageReceived(object sender, OnMessageArgs onMessageArgs)
        {
            ReceivedMessageFromServer(sender, onMessageArgs);
        }

        /// <summary>
        /// </summary>
        private static void connectedToServer_r()
        {
            ClientConnectedToServer(null, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="onMessageArgs">
        /// </param>
        private static void ServerMessageReceived(object sender, OnMessageArgs e)
        {
            ReceivedMessageFromClient((HandleClientRequest)sender, e);
        }

        #endregion
    }
}