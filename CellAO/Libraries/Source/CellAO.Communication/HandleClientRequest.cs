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

namespace CellAO.Communication
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Net.Sockets;

    using MsgPack.Serialization;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class HandleClientRequest
    {
        #region Fields

        /// <summary>
        /// </summary>
        private TcpClient clientSocket;

        /// <summary>
        /// </summary>
        private NetworkStream networkStream = null;

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
        /// <param name="clientConnected">
        /// </param>
        public HandleClientRequest(TcpClient clientConnected)
        {
            this.clientSocket = clientConnected;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        public delegate void ConnectHandler();

        /// <summary>
        /// </summary>
        public delegate void DisconnectHandler();

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="onMessageArgs">
        /// </param>
        public delegate void MessageReceivedHandler(HandleClientRequest request, OnMessageArgs onMessageArgs);

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
        /// <param name="asyncResult">
        /// </param>
        public void ReadCallBack(IAsyncResult asyncResult)
        {
            lock (this.streamLockRead)
            {
                try
                {
                    NetworkStream network = this.clientSocket.GetStream();
                    int read = network.EndRead(asyncResult);
                    if (read == 0)
                    {
                        network.Close();
                        this.clientSocket.Close();
                        this.OnDisconnect();
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
                    return;
                }
            }

            this.WaitForRequest();
        }

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public void SendData(OnMessageArgs args)
        {
            lock (this.streamLockWrite)
            {
                MessagePackSerializer<OnMessageArgs> messagePackSerializer =
                    MessagePackSerializer.Create<OnMessageArgs>();

                byte[] data = messagePackSerializer.PackSingleObject(args);

                if (this.networkStream == null)
                {
                    throw new NullReferenceException("Network stream gone away!");
                }

                if (this.networkStream.CanWrite)
                {
                    this.networkStream.Write(data, 0, data.Length);
                    this.networkStream.Flush();
                }
            }
        }

        /// <summary>
        /// </summary>
        public void StartClient()
        {
            this.networkStream = this.clientSocket.GetStream();
            this.WaitForRequest();
            this.OnConnect();
        }

        /// <summary>
        /// </summary>
        public void WaitForRequest()
        {
            if (this.clientSocket.Connected)
            {
                byte[] buffer = new byte[this.clientSocket.ReceiveBufferSize];
                this.networkStream.BeginRead(buffer, 0, buffer.Length, this.ReadCallBack, buffer);
            }
            else
            {
                this.clientSocket.Close();
                this.networkStream.Close();
                this.OnDisconnect();
            }
        }

        #endregion
    }
}