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

namespace CellAO.Communication
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    using MsgPack.Serialization;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class ZoneComClient : IDisposable
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly TcpClient clientSocket = new TcpClient();

        /// <summary>
        /// </summary>
        private NetworkStream serverStream;

        /// <summary>
        /// </summary>
        private readonly object streamLockRead = new object();

        /// <summary>
        /// </summary>
        private readonly object streamLockWrite = new object();

        private bool disposed = false;

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
        public void CloseConnection()
        {
            this.clientSocket.Close();
            this.OnDisconnect(this, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="address">
        /// </param>
        /// <param name="port">
        /// </param>
        public void ConnectToServer(IPAddress address, int port)
        {
            this.clientSocket.Connect(address, port);
            this.serverStream = this.clientSocket.GetStream();
            this.WaitForRequest();
            this.OnConnect(this, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="arg">
        /// </param>
        public void SendData(OnMessageArgs arg)
        {
            lock (this.streamLockWrite)
            {
                MessagePackSerializer<OnMessageArgs> messagePackSerializer =
                    MessagePackSerializer.Create<OnMessageArgs>();

                byte[] buffer = messagePackSerializer.PackSingleObject(arg);
                NetworkStream serStream = this.clientSocket.GetStream();
                serStream.Write(buffer, 0, buffer.Length);
                serStream.Flush();
            }
        }

        /// <summary>
        /// </summary>
        public void WaitForRequest()
        {
            byte[] buffer = new byte[this.clientSocket.ReceiveBufferSize];
            this.serverStream.BeginRead(buffer, 0, buffer.Length, this.ReadCallBack, buffer);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="asyncResult">
        /// </param>
        private void ReadCallBack(IAsyncResult asyncResult)
        {
            lock (this.streamLockRead)
            {
                try
                {
                    NetworkStream netstream = this.clientSocket.GetStream();
                    int read = netstream.EndRead(asyncResult);
                    if (read == 0)
                    {
                        this.serverStream.Close();
                        this.OnDisconnect(this, EventArgs.Empty);
                        this.clientSocket.Close();
                        return;
                    }

                    MemoryStream memoryStream = new MemoryStream(asyncResult.AsyncState as byte[]);

                    MessagePackSerializer<OnMessageArgs> messagePackSerializer =
                        MessagePackSerializer.Create<OnMessageArgs>();

                    OnMessageArgs args = messagePackSerializer.Unpack(memoryStream);

                    this.MessageReceived(this, args);
                }
                catch (Exception e)
                {
                    LogUtil.ErrorException(e);
                    this.serverStream.Close();
                    this.clientSocket.Close();
                    this.OnDisconnect(this, EventArgs.Empty);
                    return;
                }
            }

            this.WaitForRequest();
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
                    this.clientSocket.Close();
                }
            }
            this.disposed = true;
        }
    }
}