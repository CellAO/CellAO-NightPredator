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

namespace ZoneEngine.Core
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Sockets;
    using System.Threading;

    using Cell.Core;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using Ionic.Zlib;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    using Utility;

    using IBus = MemBus.IBus;

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
        private readonly IBus bus;

        /// <summary>
        /// </summary>
        private IController controller;

        /// <summary>
        /// </summary>
        private readonly IMessageSerializer messageSerializer;

        /// <summary>
        /// </summary>
        private NetworkStream netStream;

        private readonly object locker = new object();

        /// <summary>
        /// </summary>
        private short packetNumber = 0;

        /// <summary>
        /// </summary>
        private ZlibStream zStream;

        /// <summary>
        /// </summary>
        private bool zStreamSetup;

        private bool disposed = false;

        private readonly Queue<byte[]> sendQueue = new Queue<byte[]>();

        private Thread dispatcherThread;

        private bool stopDispatcher = false;

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
            this.dispatcherThread = new Thread(this.DispatchMessages);
            this.dispatcherThread.Start();
        }

        #endregion

        #region Public Properties

        public IController Controller
        {
            get
            {
                return this.controller;
            }
            set
            {
                this.controller = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        public void SendCompressed(MessageBody messageBody)
        {
            if ((this.controller == null) || (this.controller.Character == null))
            {
                return;
            }
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
                                      Receiver = this.Controller.Character.Identity.Instance
                                  }
                          };

            byte[] buffer = this.messageSerializer.Serialize(message);

            lock (this.sendQueue)
            {
                this.sendQueue.Enqueue(buffer);
            }
            LogUtil.Debug(DebugInfoDetail.AoTomation, messageBody.GetType().ToString());
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

            // TODO: Save playfield type into Character table and use it accordingly
            IPlayfield pf =
                this.server.PlayfieldById(
                    new Identity() { Type = IdentityType.Playfield, Instance = character.Playfield });

            if (
                Pool.Instance.GetObject<Character>(
                    new Identity() { Type = IdentityType.CanbeAffected, Instance = charId }) == null)
            {
                this.Controller.Character = new Character(
                    pf.Identity,
                    new Identity { Type = IdentityType.CanbeAffected, Instance = charId },
                    this.Controller);
                this.controller.Character.Read();
            }
            else
            {
                this.Controller.Character =
                    Pool.Instance.GetObject<Character>(
                        new Identity() { Type = IdentityType.CanbeAffected, Instance = charId });
                this.Controller.Character.Reconnect(this);
                LogUtil.Debug(DebugInfoDetail.Engine, "Reconnected to Character " + charId);
            }

            // Stop pending logouts
            this.Controller.Character.StopLogoutTimer();

            this.Controller.Character.Playfield = pf;
            this.Playfield = pf;
            this.Controller.Character.Stats.Read();
            this.controller.Character.Stats[StatIds.visualprofession].BaseValue = (uint)this.controller.Character.Stats[StatIds.profession].Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        public void SendCompressed(byte[] buffer)
        {
            // We can not be multithreaded here. packet numbers would be jumbled
            lock (this.locker)
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
                        LogUtil.Debug(DebugInfoDetail.Error, "Error writing to zStream");
                        LogUtil.ErrorException(e);
                        this.server.DisconnectClient(this);
                    }
                }
            }

            LogUtil.Debug(DebugInfoDetail.Network, HexOutput.Output(buffer));
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        public void SendInitiateCompressionMessage(MessageBody messageBody)
        {

            // IMPORTANT!!!!
            // DO NOT mess with this packet unless you're 9000% sure you know what you're doing.
            // This is NOT N3 message, but a special message type.
            // This is NOT fire and forget packet.
            // This is a negotiating packet which means that client and server have to agree on values.
            // out of sync = no go
            // What is hardcoded here is a working version. Changing this may break things.
            // ~Midian

            var comressionNegotiatePacket = new byte[]
                                            {
                                                0xdf, 0xdf,
                                                0x7f, 0x00,
                                                0x00, 0x01,
                                                0x00, 0x10,
                                                0x01, 0x00, // RecvCompression 0x01,0x00 Yes/0x00,0x00 No
                                                0x00, 0x00, // SendCompression 0x01,0x00 Yes/0x00,0x00 No
                                                0x00, 0x00, 0x00, 0x00
                                            };
            this.Send(comressionNegotiatePacket);
            this.packetNumber = 1;
            // TODO: Make compression choosable in config.xml
            
            /* var message = new Message
                          {
                              Body = messageBody,
                              Header =
                                  new Header
                                  {
                                      MessageId = 0xdfdf,
                                      PacketType = messageBody.PacketType,
                                      Unknown = 0x0001,

                                      
                                      Sender = 0x01000000,

                                      // 01000000 = uncompressed, 03000000 = compressed
                                      Receiver = 0 // this.character.Identity.Instance 
                                  }
                          };
            byte[] buffer = this.messageSerializer.Serialize(message);

            LogUtil.Debug(DebugInfoDetail.Network, HexOutput.Output(buffer));

            this.Send(buffer); */

            // Now create the compressed stream
            try
            {
                if (!this.zStreamSetup)
                {
                    // CreateIM the zStream
                    this.netStream = new NetworkStream(this.TcpSocket);
                    this.zStream = new ZlibStream(this.netStream, CompressionMode.Compress, CompressionLevel.BestSpeed);
                    this.zStream.FlushMode = FlushType.Sync;
                    this.zStreamSetup = true;
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    this.stopDispatcher = true;

                    while (this.stopDispatcher)
                    {
                        Thread.Sleep(10);
                    }

                    // Remove reference of character
                    if (this.Controller.Character != null)
                    {
                        // Commenting this for now, since no logouttimer should occur on zoning, only on a network disconnect (like a client crash)
                        // only how should i find out..... - Algorithman
                        /*
                    if (this.character.Stats[StatIds.gmlevel].Value == 0)
                    {
                        this.character.StartLogoutTimer();
                    }
                     */
                        //if (this == this.character.Client)
                        // {
                        //this.character.Client = null;
                        // }
                    }
                    if (this.zStream != null)
                    {
                        this.zStream.Close();
                    }
                    if (this.netStream != null)
                    {
                        this.netStream.Close();
                    }
                    this.controller = null;
                }
            }
            this.disposed = true;

            // Not needed anymore, since controller.character is a weakreference now and only lives in the Pool now
            // this.Controller.Character = null;

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

            LogUtil.Debug(DebugInfoDetail.Network, "\r\nReceived: \r\n" + HexOutput.Output(packet));

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
                LogUtil.Debug(DebugInfoDetail.Error, HexOutput.Output(packet));
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

            // FUUUUUGLY

            Type wrapperType = typeof(MessageWrapper<>);
            Type genericWrapperType = wrapperType.MakeGenericType(message.Body.GetType());

            object wrapped = Activator.CreateInstance(genericWrapperType);
            wrapped.GetType().GetProperty("Client").SetValue(wrapped, (IZoneClient)this, null);
            wrapped.GetType().GetProperty("Message").SetValue(wrapped, message, null);
            wrapped.GetType().GetProperty("MessageBody").SetValue(wrapped, message.Body, null);

            this.bus.Publish(wrapped);

            return true;
        }

        private void DispatchMessages()
        {
            while (!this.stopDispatcher)
            {
                byte[] data = null;
                lock (this.sendQueue)
                {
                    if (this.sendQueue.Count > 0)
                    {
                        data = this.sendQueue.Dequeue();
                    }
                }
                if (data != null)
                {
                    this.SendCompressed(data);
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
            this.stopDispatcher = false;
        }

        #endregion
    }
}