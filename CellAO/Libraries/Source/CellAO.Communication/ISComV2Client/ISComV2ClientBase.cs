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

namespace CellAO.Communication.ISComV2Client
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using Cell.Core;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class ISComV2ClientBase : IDisposable
    {
        #region Constants

        /// <summary>
        /// </summary>
        public const int BufferSize = CellDef.MAX_PBUF_SEGMENT_SIZE;

        #endregion

        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly BufferManager Buffers = BufferManager.Default;

        /// <summary>
        /// </summary>
        private static long _totalBytesReceived;

        /// <summary>
        /// </summary>
        private static long _totalBytesSent;

        #endregion

        #region Fields

        /// <summary>
        /// The buffer containing the data received.
        /// </summary>
        protected BufferSegment _bufferSegment;

        /// <summary>
        /// The offset in the buffer to write at.
        /// </summary>
        protected int _offset;

        /// <summary>
        /// The offset in the buffer to write at.
        /// </summary>
        protected int _remainingLength;

        /// <summary>
        /// The socket containing the TCP connection this client is using.
        /// </summary>
        protected Socket _tcpSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Number of bytes that have been received by this client.
        /// </summary>
        private uint _bytesReceived;

        /// <summary>
        /// Number of bytes that have been sent by this client.
        /// </summary>
        private uint _bytesSent;

        private bool disposed = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ISComV2ClientBase()
        {
            this._bufferSegment = Buffers.CheckOut();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="dataBytes">
        /// </param>
        public delegate void OnDataReceived(object sender, OnDataReceivedArgs e);

        /// <summary>
        /// </summary>
        public delegate void OnDisconnect(object sender, EventArgs e);

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event OnDisconnect Disconnected;

        /// <summary>
        /// </summary>
        public event OnDataReceived ReceivedData;

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this._tcpSock != null && this._tcpSock.Connected;
            }
        }

        /// <summary>
        /// Gets the port the server is communicating on.
        /// </summary>
        public int Port
        {
            get
            {
                return (this._tcpSock != null && this._tcpSock.RemoteEndPoint != null)
                    ? ((IPEndPoint)this._tcpSock.RemoteEndPoint).Port
                    : -1;
            }
        }

        /// <summary>
        /// </summary>
        public uint ReceivedBytes
        {
            get
            {
                return this._bytesReceived;
            }
        }

        /// <summary>
        /// </summary>
        public uint SentBytes
        {
            get
            {
                return this._bytesSent;
            }
        }

        /// <summary>
        /// Gets the IP address of the server.
        /// </summary>
        public IPAddress ServerAddress
        {
            get
            {
                return (this._tcpSock != null && this._tcpSock.RemoteEndPoint != null)
                    ? ((IPEndPoint)this._tcpSock.RemoteEndPoint).Address
                    : null;
            }
        }

        /// <summary>
        /// Gets/Sets the socket this client is using for TCP communication.
        /// </summary>
        public Socket TcpSocket
        {
            get
            {
                return this._tcpSock;
            }

            set
            {
                if (this._tcpSock != null && this._tcpSock.Connected)
                {
                    this._tcpSock.Shutdown(SocketShutdown.Both);
                    this._tcpSock.Close();
                }

                if (value != null)
                {
                    this._tcpSock = value;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// </summary>
        public void BeginReceive()
        {
            this.ResumeReceive();
        }

        /// <summary>
        /// </summary>
        /// <param name="host">
        /// </param>
        /// <param name="port">
        /// </param>
        public void Connect(string host, int port)
        {
            this.Connect(IPAddress.Parse(host), port);
        }

        /// <summary>
        /// </summary>
        /// <param name="addr">
        /// </param>
        /// <param name="port">
        /// </param>
        public void Connect(IPAddress addr, int port)
        {
            if (this._tcpSock != null)
            {
                if (this._tcpSock.Connected)
                {
                    this._tcpSock.Disconnect(true);
                }

                this._tcpSock.Connect(addr, port);

                this.BeginReceive();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="packet">
        /// </param>
        public void Send(byte[] packet)
        {
            this.Send(packet, 0, packet.Length);
        }

        /// <summary>
        /// </summary>
        /// <param name="segment">
        /// </param>
        /// <param name="length">
        /// </param>
        public void Send(BufferSegment segment, int length)
        {
            this.Send(segment.Buffer.Array, segment.Offset, length);
        }

        /// <summary>
        /// </summary>
        /// <param name="packet">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="length">
        /// </param>
        public virtual void Send(byte[] packet, int offset, int length)
        {
            if (this._tcpSock != null && this._tcpSock.Connected)
            {
                SocketAsyncEventArgs args = SocketHelpers.AcquireSocketArg();
                if (args != null)
                {
                    args.Completed += SendAsyncComplete;
                    args.SetBuffer(packet, offset, length);
                    args.UserToken = this;
                    this._tcpSock.SendAsync(args);

                    unchecked
                    {
                        this._bytesSent += (uint)length;
                    }

                    Interlocked.Add(ref _totalBytesSent, length);
                }
                else
                {
                    Console.WriteLine("Client {0}'s SocketArgs are null", this);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="packet">
        /// </param>
        public void SendCopy(byte[] packet)
        {
            var copy = new byte[packet.Length];
            Array.Copy(packet, copy, packet.Length);
            this.Send(copy, 0, copy.Length);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return
                (this.TcpSocket == null || !this.TcpSocket.Connected
                    ? "<disconnected client>"
                    : (this.TcpSocket.RemoteEndPoint ?? (object)"<unknown client>")).ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="dataBytes">
        /// </param>
        protected void DataReceived(byte[] dataBytes)
        {
            // Call the Event
            if (this.ReceivedData != null)
            {
                this.ReceivedData(this, new OnDataReceivedArgs() { dataBytes = dataBytes });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    if (this._tcpSock != null && this._tcpSock.Connected)
                    {
                        try
                        {
                            this._bufferSegment.DecrementUsage();
                            this._tcpSock.Shutdown(SocketShutdown.Both);
                            this._tcpSock.Close();
                            this._tcpSock = null;
                        }
                        catch (SocketException /* exception*/)
                        {
                            // TODO: Check what exceptions we need to handle
                        }
                    }
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// </summary>
        protected void EnsureBuffer()
        {
            {
                // (int size)
                // if (size > BufferSize - _offset)
                // not enough space left in buffer: Copy to new buffer
                BufferSegment newSegment = Buffers.CheckOut();
                Array.Copy(
                    this._bufferSegment.Buffer.Array,
                    this._bufferSegment.Offset + this._offset,
                    newSegment.Buffer.Array,
                    newSegment.Offset,
                    this._remainingLength);
                this._bufferSegment.DecrementUsage();
                this._bufferSegment = newSegment;
                this._offset = 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        protected bool OnReceive(BufferSegment buffer)
        {
            // return false, if header cannot be complete (00FF55AA, <Length of packet>)

            // Loop if more than one packet frame received
            while (true)
            {
                if (this._remainingLength == 0)
                {
                    return true;
                }

                if (this._remainingLength < 8)
                {
                    return false;
                }

                int expectedLength = this.CheckData(buffer);
                if (expectedLength == -1)
                {
                    // MALFORMED PACKET RECEIVED !!!
                    LogUtil.Debug(DebugInfoDetail.Error, "Malformed packet received: ");
                    byte[] data = new byte[this._remainingLength];
                    buffer.SegmentData.CopyTo(data, this._remainingLength);
                    LogUtil.Debug(DebugInfoDetail.Error, HexOutput.Output(data));
                    this._remainingLength = 0;
                    this._offset = 0;

                    // Lets clear the buffer and try this again, no need to drop the connection
                    return true;
                }

                if (expectedLength + 8 > this._remainingLength)
                {
                    return false;
                }

                if (this._remainingLength >= expectedLength + 8)
                {
                    // Handle packet payload here

                    byte[] dataBytes = new byte[expectedLength];
                    Array.Copy(buffer.SegmentData, 8 + this._offset, dataBytes, 0, expectedLength);

                    this.DataReceived(dataBytes);
                }

                if (expectedLength + 8 <= this._remainingLength)
                {
                    // If we have received a full packet frame
                    // then move the remaining data to a new buffer (with offset 0)
                    // only adjusting offset and length here
                    // Then do the whole thing again
                    this._remainingLength -= expectedLength + 8;
                    this._offset += expectedLength + 8;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="args">
        /// </param>
        private static void SendAsyncComplete(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= SendAsyncComplete;
            SocketHelpers.ReleaseSocketArg(args);
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        private int CheckData(BufferSegment buffer)
        {
            if (BitConverter.ToInt32(buffer.SegmentData, this._offset) != 0x00FF55AA)
            {
                Console.WriteLine("Invalid packet header");
                return -1;
            }

            return BitConverter.ToInt32(buffer.SegmentData, this._offset + 4);
        }

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private void ProcessRecieve(SocketAsyncEventArgs args)
        {
            try
            {
                int bytesReceived = args.BytesTransferred;

                if (bytesReceived == 0)
                {
                    // if (args.SocketError != SocketError.Success)
                    this.ServerDisconnected();
                }
                else
                {
                    // increment our counters
                    unchecked
                    {
                        this._bytesReceived += (uint)bytesReceived;
                    }

                    Interlocked.Add(ref _totalBytesReceived, bytesReceived);

                    this._remainingLength += bytesReceived;

                    if (this.OnReceive(this._bufferSegment))
                    {
                        // packet processed entirely
                        this._offset = 0;
                        this._remainingLength = 0;
                        this._bufferSegment.DecrementUsage();
                        this._bufferSegment = Buffers.CheckOut();
                    }
                    else
                    {
                        this.EnsureBuffer();
                    }

                    this.ResumeReceive();
                }
            }
            catch (ObjectDisposedException)
            {
                this.ServerDisconnected();
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);

                // Error occurred, need proper handler
            }
            finally
            {
                args.Completed -= this.ReceiveAsyncComplete;
                SocketHelpers.ReleaseSocketArg(args);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="args">
        /// </param>
        private void ReceiveAsyncComplete(object sender, SocketAsyncEventArgs args)
        {
            this.ProcessRecieve(args);
        }

        /// <summary>
        /// Resumes asynchronous TCP receiving from the server.
        /// </summary>
        private void ResumeReceive()
        {
            if (this._tcpSock != null && this._tcpSock.Connected)
            {
                SocketAsyncEventArgs socketArgs = SocketHelpers.AcquireSocketArg();
                int offset = this._offset + this._remainingLength;

                socketArgs.SetBuffer(
                    this._bufferSegment.Buffer.Array,
                    this._bufferSegment.Offset + offset,
                    BufferSize - offset);
                socketArgs.UserToken = this;
                socketArgs.Completed += this.ReceiveAsyncComplete;

                bool willRaiseEvent = this._tcpSock.ReceiveAsync(socketArgs);
                if (!willRaiseEvent)
                {
                    this.ProcessRecieve(socketArgs);
                }
            }
        }

        /// <summary>
        /// </summary>
        private void ServerDisconnected()
        {
            if (this.Disconnected != null)
            {
                this.Disconnected(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}