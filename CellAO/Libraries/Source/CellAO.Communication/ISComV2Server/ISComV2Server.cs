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
// Last modified: 2013-11-16 19:01

#endregion

namespace CellAO.Communication.ISComV2Server
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Net;

    using Cell.Core;

    using CellAO.Communication.Messages;

    using MemBus;
    using MemBus.Configurators;

    using MsgPack.Serialization;

    #endregion

    /// <summary>
    /// </summary>
    public class ISComV2Server : ServerBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public int lastClientNumber = 0;

        /// <summary>
        /// </summary>
        private readonly IBus bus;

        /// <summary>
        /// </summary>
        private Dictionary<int, IClient> clientDictionary = new Dictionary<int, IClient>();

        /// <summary>
        /// </summary>
        private HashSet<IClient> clients = new HashSet<IClient>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ISComV2Server()
        {
            this.bus = BusSetup.StartWith<AsyncConfiguration>().Construct();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="messageObject">
        /// </param>
        public delegate void DataReceivedHandler(IClient client, DynamicMessage messageObject);

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event DataReceivedHandler DataReceived;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        protected override IClient CreateClient()
        {
            IClient temp;
            lock (this.clients)
            {
                this.lastClientNumber++;
                temp = new ISComV2ClientHandler(this, this.bus, this.lastClientNumber);
                this.clients.Add(temp);
                ((ISComV2ClientHandler)temp).DataReceived += this.ISComV2Server_DataReceived;
            }

            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="num_bytes">
        /// </param>
        /// <param name="buf">
        /// </param>
        /// <param name="ip">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void OnReceiveUDP(int num_bytes, byte[] buf, IPEndPoint ip)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="clientIP">
        /// </param>
        /// <param name="num_bytes">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void OnSendTo(IPEndPoint clientIP, int num_bytes)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="dataBytes">
        /// </param>
        private void ISComV2Server_DataReceived(ISComV2ClientHandler client, byte[] dataBytes)
        {
            if (this.DataReceived != null)
            {
                MessagePackSerializer<DynamicMessage> ser = MessagePackSerializer.Create<DynamicMessage>();
                DynamicMessage result = ser.UnpackSingleObject(dataBytes);

                this.DataReceived(client, result);
            }
        }

        #endregion
    }
}