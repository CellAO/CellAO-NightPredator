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
// Last modified: 2013-11-11 20:39

#endregion

namespace CellAO.Communication.ISComV2Server
{
    #region Usings ...

    using System;

    using Cell.Core;

    using CellAO.Communication.Messages;

    using MemBus;

    using MsgPack.Serialization;

    using NiceHexOutput;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class ISComV2ClientHandler : ClientBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly int ID;

        /// <summary>
        /// </summary>
        private readonly IBus bus;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="server">
        /// </param>
        public ISComV2ClientHandler(ServerBase server)
            : base(server)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="server">
        /// </param>
        /// <param name="messageBus">
        /// </param>
        /// <param name="clientNumber">
        /// </param>
        public ISComV2ClientHandler(ServerBase server, IBus messageBus, int clientNumber)
            : base(server)
        {
            this.ID = clientNumber;
            this.bus = messageBus;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="dataBytes">
        /// </param>
        public delegate void DataReceivedHandler(ISComV2ClientHandler client, byte[] dataBytes);

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event DataReceivedHandler DataReceived;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetID()
        {
            return this.ID;
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
            this.Send(header);
            this.Send(data);
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public void Send(MessageBase message)
        {
            var temp = new DynamicMessage();
            temp.DataObject = message;
            this.Send(temp);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override bool OnReceive(BufferSegment buffer)
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
                    LogUtil.Debug("Malformed packet received: ");
                    byte[] data = new byte[this._remainingLength];
                    buffer.SegmentData.CopyTo(data, this._remainingLength);
                    LogUtil.Debug(NiceHexOutput.Output(data));
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
                    if (this.DataReceived != null)
                    {
                        this.DataReceived(this, dataBytes);
                    }
                    else
                    {
                        LogUtil.Debug("No DataReceived event fired due to missing method");
                    }
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

        #endregion
    }
}