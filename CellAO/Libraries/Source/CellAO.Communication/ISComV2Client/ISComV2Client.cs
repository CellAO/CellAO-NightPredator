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
    public class ISComV2Client : IDisposable
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly ISComV2ClientBase clientBase = new ISComV2ClientBase();

        /// <summary>
        /// </summary>
        private bool closing = false;

        /// <summary>
        /// </summary>
        private Thread connectorThread;

        /// <summary>
        /// </summary>
        private IPAddress serverAddress;

        /// <summary>
        /// </summary>
        private int serverPort;

        private bool disposed = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ISComV2Client()
        {
            this.clientBase.ReceivedData += this.ClientBaseReceivedData;
            this.clientBase.Disconnected += this.ClientBaseDisconnected;

            this.connectorThread = new Thread(new ThreadStart(this.Connector));
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        public delegate void OnConnectHandler(object sender, EventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        public delegate void OnReceiveDataHandler(object sender, DynamicMessage e);

        /// <summary>
        /// Event fired after reconnect tries were unsuccessful
        /// </summary>
        public delegate void ReallyDisconnectedHandler(object sender, EventArgs e);

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
                this.serverAddress = address;
                this.serverPort = port;
                this.connectorThread = new Thread(new ThreadStart(this.Connector));
                this.connectorThread.Start();

                return true;
            }
            catch (Exception e)
            {
                LogUtil.Debug(DebugInfoDetail.Engine, "ISCom Connection to ChatEngine failed");
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
            byte[] finalData = new byte[8 + data.Length];
            BitConverter.GetBytes(0x00ff55aa).CopyTo(finalData, 0);
            BitConverter.GetBytes(data.Length).CopyTo(finalData, 4);
            Array.Copy(data, 0, finalData, 8, data.Length);
            this.clientBase.Send(finalData);
        }

        /// <summary>
        /// </summary>
        /// <param name="dataObject">
        /// </param>
        public void Send(MessageBase dataObject)
        {
            var temp = new DynamicMessage()
            {
                DataObject = dataObject
            };
            this.Send(temp);
        }

        /// <summary>
        /// </summary>
        public void ShutDown()
        {
            LogUtil.Debug(DebugInfoDetail.Engine, "Shutting down ISCom");
            this.closing = true;
            while (this.connectorThread.IsAlive)
            {
                Thread.Sleep(100);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void Connector()
        {
            Ping ping = new Ping();
            while (!this.closing)
            {
                if (this.serverAddress == null)
                {
                    continue;
                }

                if (!this.clientBase.IsConnected)
                {
                    LogUtil.Debug(DebugInfoDetail.ISComm, "Trying to connect to ChatEngine...");

                    // this.Connect(serverAddress, serverPort);
                    try
                    {
                        this.clientBase.Connect(this.serverAddress, this.serverPort);
                        if (this.OnConnect != null)
                        {
                            OnConnect(this, EventArgs.Empty);
                        }
                    }
                    catch
                    {
                    }
                }
                if (!this.closing)
                {
                    Thread.Sleep(5000);
                    this.Send(ping);
                }
            }
        }

        /// <summary>
        /// </summary>
        private void ClientBaseDisconnected(object sender, EventArgs e)
        {
            if (this.serverAddress == null)
            {
                LogUtil.Debug(DebugInfoDetail.Error, "Could not reconnect to ChatEngine (no server address found)");
                return;
            }

            LogUtil.Debug(DebugInfoDetail.ISComm, "Trying to reconnect to ChatEngine");
        }

        /// <summary>
        /// </summary>
        /// <param name="dataBytes">
        /// </param>
        private void ClientBaseReceivedData(object sender, OnDataReceivedArgs e)
        {
            MessagePackSerializer<DynamicMessage> serializer = MessagePackSerializer.Create<DynamicMessage>();
            DynamicMessage tmp = serializer.UnpackSingleObject(e.dataBytes);

            // Is the handler set?
            if (this.OnReceiveData != null)
            {
                OnReceiveData(this, tmp);
            }
        }

        #endregion

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    this.clientBase.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}