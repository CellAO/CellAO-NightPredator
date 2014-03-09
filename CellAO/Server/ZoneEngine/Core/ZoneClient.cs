#region License

// Copyright (c) 2005-2014, CellAO Team
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Sockets;

    using Cell.Core;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.EventHandlers.Events;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    using zlib;

    using ZoneEngine.Core.Functions;

    #endregion

    /// <summary>
    /// </summary>
    public class ZoneClient : ClientBase, IZoneClient
    {
        #region Fields

        /// <summary>
        /// </summary>
        public IPlayfield Playfield;

        /// <summary>
        /// </summary>
        private readonly ZoneServer server;

        /// <summary>
        /// </summary>
        private IBus bus;

        /// <summary>
        /// </summary>
        private Character character;

        /// <summary>
        /// </summary>
        private IMessageSerializer messageSerializer;

        /// <summary>
        /// </summary>
        private NetworkStream netStream;

        /// <summary>
        /// </summary>
        private short packetNumber = 0;

        /// <summary>
        /// </summary>
        private ZOutputStream zStream;

        /// <summary>
        /// </summary>
        private bool zStreamSetup;

        #endregion

        #region Constructors and Destructors

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

        #region Public Properties

        /// <summary>
        /// </summary>
        public Character Character
        {
            get
            {
                return this.character;
            }

            set
            {
                this.character = (Character)value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="functions">
        /// </param>
        public void CallFunction(CellAO.Core.Functions.Function functions)
        {
            // TODO: Make it more versatile, not just applying stuff on yourself
            FunctionCollection.Instance.CallFunction(
                functions.FunctionType, 
                this.character, 
                this.character, 
                this.character, 
                functions.Arguments.Values.ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="charId">
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        public void CreateCharacter(int charId)
        {
            DBCharacter character = CharacterDao.Instance.Get(charId);
            if (character == null)
            {
                throw new Exception("Character " + charId + " not found.");
            }

            IPlayfield pf = this.server.PlayfieldById(character.Playfield);

            if (pf.Entities.GetObject<Character>(
                new Identity() { Type = IdentityType.CanbeAffected, Instance = charId }) == null)
            {
                this.character = new Character(
                    pf.Entities, 
                    new Identity { Type = IdentityType.CanbeAffected, Instance = charId }, 
                    this);
            }
            else
            {
                this.character =
                    pf.Entities.GetObject<Character>(
                        new Identity() { Type = IdentityType.CanbeAffected, Instance = charId });
                this.character.Reconnect(this);
                LogUtil.Debug("Reconnected to Character " + charId);
            }

            // Stop pending logouts
            this.character.StopLogoutTimer();

            this.character.Playfield = pf;
            this.Playfield = pf;
            this.character.Stats.Read();
        }

        /// <summary>
        /// </summary>
        public new void Dispose()
        {
            this.Dispose(true);
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

            if (Program.DebugNetwork)
            {
                LogUtil.Debug(messageBody.GetType().ToString());
            }

            this.SendCompressed(buffer);
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        public void SendCompressed(byte[] buffer)
        {
            // We can not be multithreaded here. packet numbers would be jumbled
            lock (this.netStream)
            {
                // Discard the packet for now, if we can not write to the stream
                if (this.netStream.CanWrite)
                {
                    byte[] pn = BitConverter.GetBytes(this.packetNumber++);
                    buffer[0] = pn[1];
                    buffer[1] = pn[0];

                    try
                    {
                        this.zStream.Write(buffer, 0, buffer.Length);
                        this.zStream.Flush();
                    }
                    catch (Exception e)
                    {
                        LogUtil.Debug("Error writing to zStream");
                        LogUtil.ErrorException(e);
                        this.server.DisconnectClient(this);
                    }
                }
            }

            if (Program.DebugNetwork)
            {
                LogUtil.Debug(HexOutput.Output(buffer));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        public void SendInitiateCompressionMessage(MessageBody messageBody)
        {
            // TODO: Investigate if reciever is a timestamp
            var message = new Message
                          {
                              Body = messageBody, 
                              Header =
                                  new Header
                                  {
                                      MessageId = 0xdfdf, 
                                      PacketType = messageBody.PacketType, 
                                      Unknown = 0x0001, 

                                      // TODO: Make compression choosable in config.xml
                                      Sender = 0x01000000, 

                                      // 01000000 = uncompressed, 03000000 = compressed
                                      Receiver = 0 // this.character.Identity.Instance 
                                  }
                          };
            byte[] buffer = this.messageSerializer.Serialize(message);

            if (Program.DebugNetwork)
            {
                LogUtil.Debug(HexOutput.Output(buffer));
            }

            this.packetNumber = 1;

            this.Send(buffer);

            // Now create the compressed stream
            try
            {
                if (!this.zStreamSetup)
                {
                    // CreateIM the zStream
                    this.netStream = new NetworkStream(this.TcpSocket);
                    this.zStream = new ZOutputStream(this.netStream, zlibConst.Z_BEST_SPEED);
                    this.zStream.FlushMode = zlibConst.Z_SYNC_FLUSH;
                    this.zStreamSetup = true;
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <returns>
        /// </returns>
        public bool SendChatText(string text)
        {
            // TODO: remove it here, transfer it to Character class and let it publish it on playfield bus
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

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // Remove reference of character
            if (this.character != null)
            {
                this.character.StartLogoutTimer();
                this.character.Client = null;
            }

            this.character = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// </summary>
        /// <param name="segment">
        /// </param>
        /// <returns>
        /// </returns>
        protected uint GetMessageNumber(BufferSegment segment)
        {
            var messageNumberArray = new byte[4];
            messageNumberArray[3] = segment.SegmentData[16];
            messageNumberArray[2] = segment.SegmentData[17];
            messageNumberArray[1] = segment.SegmentData[18];
            messageNumberArray[0] = segment.SegmentData[19];
            uint reply = BitConverter.ToUInt32(messageNumberArray, 0);
            return reply;
        }

        /// <summary>
        /// </summary>
        /// <param name="segment">
        /// </param>
        /// <returns>
        /// </returns>
        protected uint GetMessageNumber(byte[] segment)
        {
            var messageNumberArray = new byte[4];
            messageNumberArray[3] = segment[16];
            messageNumberArray[2] = segment[17];
            messageNumberArray[1] = segment[18];
            messageNumberArray[0] = segment[19];
            uint reply = BitConverter.ToUInt32(messageNumberArray, 0);
            return reply;
        }

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
            Message message = null;

            var packet = new byte[this._remainingLength];
            Array.Copy(buffer.SegmentData, packet, this._remainingLength);

            if (Program.DebugNetwork)
            {
                LogUtil.Debug("\r\nReceived: \r\n" + HexOutput.Output(packet));
            }

            this._remainingLength = 0;
            try
            {
                message = this.messageSerializer.Deserialize(packet);
            }
            catch (Exception)
            {
                uint messageNumber = this.GetMessageNumber(packet);
                this.Server.Warning(
                    this, 
                    "Client sent malformed message {0}", 
                    messageNumber.ToString(CultureInfo.InvariantCulture));
                LogUtil.Debug(HexOutput.Output(packet));
                return false;
            }

            buffer.IncrementUsage();

            if (message == null)
            {
                uint messageNumber = this.GetMessageNumber(packet);
                this.Server.Warning(
                    this, 
                    "Client sent unknown message {0}", 
                    messageNumber.ToString(CultureInfo.InvariantCulture));
                return false;
            }

            this.bus.Publish(new MessageReceivedEvent(this, message));

            return true;
        }

        #endregion
    }
}