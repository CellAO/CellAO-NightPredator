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

namespace CellAO.Communication.Client
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Security.Permissions;

    using MsgPack.Serialization;

    using Utility;

    public class Client
    {
        TcpClient clientSocket = new TcpClient();

        private NetworkStream serverStream;

        public void ConnectToServer(IPAddress address, int port)
        {
            clientSocket.Connect(address, port);
            serverStream = clientSocket.GetStream();
            WaitForRequest();
            OnConnect();
        }

        public void WaitForRequest()
        {
            byte[] buffer = new byte[clientSocket.ReceiveBufferSize];
            serverStream.BeginRead(buffer, 0, buffer.Length, ReadCallBack, buffer);
        }
        public void SendData(OnMessageArgs arg)
        {
            MessagePackSerializer<OnMessageArgs> messagePackSerializer = MessagePackSerializer.Create<OnMessageArgs>();

            byte[] buffer = messagePackSerializer.PackSingleObject(arg);
            NetworkStream serStream = clientSocket.GetStream();
            serStream.Write(buffer, 0, buffer.Length);
            serStream.Flush();

        }

        public void CloseConnection()
        {
            clientSocket.Close();
            OnDisconnect();
        }

        public delegate void MessageReceivedHandler(OnMessageArgs onMessageArgs);

        public delegate void ConnectHandler();

        public delegate void DisconnectHandler();

        public event MessageReceivedHandler MessageReceived;
        public event ConnectHandler OnConnect;
        public event DisconnectHandler OnDisconnect;

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            NetworkStream netstream = clientSocket.GetStream();
            try
            {
                int read = netstream.EndRead(asyncResult);
                if (read == 0)
                {
                    serverStream.Close();
                    OnDisconnect();
                    clientSocket.Close();
                    return;
                }

                MemoryStream memoryStream = new MemoryStream(asyncResult.AsyncState as byte[]);

                MessagePackSerializer<OnMessageArgs> messagePackSerializer =
                    MessagePackSerializer.Create<OnMessageArgs>();

                OnMessageArgs args = messagePackSerializer.Unpack(memoryStream);

                MessageReceived(args);


            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                throw;
            }
            this.WaitForRequest();

        }

    }
}