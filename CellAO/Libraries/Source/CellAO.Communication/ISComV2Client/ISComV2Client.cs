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
// Last modified: 2013-11-11 20:45

#endregion

namespace CellAO.Communication.ISComV2Client
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Threading;

    using CellAO.Communication.Messages;

    using MsgPack.Serialization;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class ISComV2Client
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly ISComV2ClientBase clientBase = new ISComV2ClientBase();

        /// <summary>
        /// </summary>
        private IPAddress serverAddress;

        /// <summary>
        /// </summary>
        private int serverPort;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ISComV2Client()
        {
            this.clientBase.ReceivedData += this.clientBase_ReceivedData;
            this.clientBase.Disconnected += this.clientBase_Disconnected;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        public delegate void OnConnectHandler();

        /// <summary>
        /// </summary>
        /// <param name="messageObject">
        /// </param>
        public delegate void OnReceiveDataHandler(DynamicMessage messageObject);

        /// <summary>
        /// Event fired after reconnect tries were unsuccessful
        /// </summary>
        public delegate void ReallyDisconnectedHandler();

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event OnConnectHandler OnConnect;

        /// <summary>
        /// </summary>
        public event OnReceiveDataHandler OnReceiveData;

        /// <summary>
        /// </summary>
        public event ReallyDisconnectedHandler ReallyDisconnected;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="address">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Connect(IPAddress address, int port)
        {
            try
            {
                this.clientBase.Connect(address, port);
                this.serverAddress = address;
                this.serverPort = port;
                if (this.OnConnect != null)
                {
                    this.OnConnect();
                }

                return true;
            }
            catch (Exception e)
            {
                LogUtil.Debug("ISCom Connection to ChatEngine failed");
                LogUtil.ErrorException(e);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dataObject">
        /// </param>
        public void Send(DynamicMessage dataObject)
        {
            MessagePackSerializer<object> serializer = MessagePackSerializer.Create<object>();
            byte[] data = serializer.PackSingleObject(dataObject);
            byte[] header = new byte[8];
            BitConverter.GetBytes(0x00ff55aa).CopyTo(header, 0);
            BitConverter.GetBytes(data.Length).CopyTo(header, 4);
            this.clientBase.Send(header);
            this.clientBase.Send(data);
        }

        /// <summary>
        /// </summary>
        /// <param name="dataObject">
        /// </param>
        public void Send(MessageBase dataObject)
        {
            var temp = new DynamicMessage();
            temp.DataObject = dataObject;
            this.Send(temp);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void clientBase_Disconnected()
        {
            int retries = 0;

            if (this.serverAddress == null)
            {
                LogUtil.Debug("Could not reconnect to ChatEngine (no server address found)");
                return;
            }

            while (retries < 10)
            {
                LogUtil.Debug("Trying to reconnect to ChatEngine");
                if (this.Connect(this.serverAddress, this.serverPort))
                {
                    return;
                }

                Thread.Sleep(2000);
                retries++;
            }

            LogUtil.Debug("Could not reconnect to ChatEngine");
            if (this.ReallyDisconnected != null)
            {
                this.ReallyDisconnected();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dataBytes">
        /// </param>
        private void clientBase_ReceivedData(byte[] dataBytes)
        {
            MessagePackSerializer<DynamicMessage> serializer = MessagePackSerializer.Create<DynamicMessage>();
            DynamicMessage tmp = serializer.UnpackSingleObject(dataBytes);

            // Is the handler set?
            if (this.OnReceiveData != null)
            {
                this.OnReceiveData(tmp);
            }
        }

        #endregion
    }
}