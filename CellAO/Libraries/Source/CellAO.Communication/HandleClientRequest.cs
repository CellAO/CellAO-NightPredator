using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Communication
{
    using System.IO;
    using System.Net.Sockets;

    using MsgPack.Serialization;

    using Utility;

    public class HandleClientRequest
    {
        private TcpClient clientSocket;

        private NetworkStream networkStream = null;

        public HandleClientRequest(TcpClient clientConnected)
        {
            this.clientSocket = clientConnected;
        }

        private object streamLockWrite = new object();

        private object streamLockRead = new object();


        public delegate void MessageReceivedHandler(HandleClientRequest request, OnMessageArgs onMessageArgs);

        public delegate void ConnectHandler();

        public delegate void DisconnectHandler();

        public event MessageReceivedHandler MessageReceived;
        public event ConnectHandler OnConnect;
        public event DisconnectHandler OnDisconnect;

        public void StartClient()
        {
            networkStream = clientSocket.GetStream();
            WaitForRequest();
            OnConnect();
        }

        public void WaitForRequest()
        {
            if (clientSocket.Connected)
            {
                byte[] buffer = new byte[clientSocket.ReceiveBufferSize];
                networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallBack, buffer);
            }
            else
            {
                clientSocket.Close();
                networkStream.Close();
                OnDisconnect();
            }
        }

        public void ReadCallBack(IAsyncResult asyncResult)
        {
            lock (streamLockRead)
            {
                try
                {
                    NetworkStream network = clientSocket.GetStream();
                    int read = network.EndRead(asyncResult);
                    if (read == 0)
                    {
                        network.Close();
                        clientSocket.Close();
                        OnDisconnect();
                        return;
                    }

                    MemoryStream memoryStream = new MemoryStream(asyncResult.AsyncState as byte[]);

                    MessagePackSerializer<OnMessageArgs> messagePackSerializer =
                        MessagePackSerializer.Create<OnMessageArgs>();

                    OnMessageArgs args = messagePackSerializer.Unpack(memoryStream);

                    MessageReceived(this, args);

                }
                catch (Exception e)
                {
                    LogUtil.ErrorException(e);
                    return;
                }
            }
            this.WaitForRequest();
        }

        public void SendData(OnMessageArgs args)
        {
            lock (streamLockWrite)
            {
                MessagePackSerializer<OnMessageArgs> messagePackSerializer =
                    MessagePackSerializer.Create<OnMessageArgs>();

                byte[] data = messagePackSerializer.PackSingleObject(args);

                if (networkStream == null)
                {
                    throw new NullReferenceException("Network stream gone away!");
                }
                if (networkStream.CanWrite)
                {
                    networkStream.Write(data, 0, data.Length);
                    networkStream.Flush();
                }
            }
        }
    }
}
