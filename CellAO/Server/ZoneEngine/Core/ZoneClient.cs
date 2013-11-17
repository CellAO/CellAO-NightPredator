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

#endregion

namespace ZoneEngine.Core
{
    #region Usings ...

    using System;

    using Cell.Core;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;

    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    public class ZoneClient : ClientBase, IZoneClient
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly ZoneServer server;

        public Playfield Playfield;

        /// <summary>
        /// </summary>
        private IBus bus;

        /// <summary>
        /// </summary>
        private IMessageSerializer messageSerializer;

        /// <summary>
        /// </summary>
        private short packetNumber = 0;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="server">
        /// </param>
        public ZoneClient(ServerBase server)
            : base(server)
        {
            this.server = (ZoneServer)server;
        }

        /// <summary>
        /// </summary>
        /// <param name="server">
        /// </param>
        /// <param name="messageSerializer">
        /// </param>
        /// <param name="bus">
        /// </param>
        public ZoneClient(ZoneServer server, IMessageSerializer messageSerializer, IBus bus)
            : base(server)
        {
            this.server = server;
            this.messageSerializer = messageSerializer;
            this.bus = bus;
        }

        #endregion

        #region Public Methods and Operators

        public ICharacter Character { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <returns>
        /// </returns>
        public bool SendChatText(string text)
        {
            var message = new ChatTextMessage
                          {
                              Identity = this.Character.Identity, 
                              Unknown = 0x00, 
                              Text = text, 
                              Unknown1 = 0x1000, 
                              Unknown2 = 0x00000000
                          };

            this.SendCompressed(message);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        public void SendCompressed(MessageBody messageBody)
        {
            var message = new Message
                          {
                              Body = messageBody, 
                              Header =
                                  new Header
                                  {
                                      MessageId = BitConverter.ToUInt16(new byte[] { 0xDF, 0xDF }, 0), 
                                      PacketType = messageBody.PacketType, 
                                      Unknown = 0x0001, 
                                      Sender = this.server.Id, 
                                      Receiver = this.Character.Identity.Instance
                                  }
                          };

            byte[] buffer = this.messageSerializer.Serialize(message);
            this.SendCompressed(buffer);
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        public void SendCompressed(byte[] buffer)
        {
            byte[] pn = BitConverter.GetBytes(this.packetNumber++);
            buffer[0] = pn[1];
            buffer[1] = pn[0];

            this.Send(buffer);
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
            throw new NotImplementedException();
        }

        #endregion
    }
}