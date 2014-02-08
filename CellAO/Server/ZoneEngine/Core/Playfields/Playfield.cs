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

namespace CellAO.Core.Playfields
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Network;
    using CellAO.Core.Statels;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Enums;
    using CellAO.Interfaces;
    using CellAO.ObjectManager;

    using MemBus;
    using MemBus.Configurators;
    using MemBus.Support;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.SystemMessages;

    using Utility;

    using ZoneEngine;
    using ZoneEngine.Core;
    using ZoneEngine.Core.Functions;
    using ZoneEngine.Core.InternalMessages;
    using ZoneEngine.Core.Packets;
    using ZoneEngine.Core.Playfields;

    using Config = Utility.Config.ConfigReadWrite;
    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public class Playfield : IPlayfield
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly DisposeContainer memBusDisposeContainer = new DisposeContainer();

        /// <summary>
        /// </summary>
        private readonly IBus playfieldBus;

        /// <summary>
        /// </summary>
        private readonly ZoneServer server;

        /// <summary>
        /// </summary>
        private List<PlayfieldDistrict> districts = new List<PlayfieldDistrict>();

        /// <summary>
        /// </summary>
        private Timer heartBeat;

        /// <summary>
        /// </summary>
        private List<StatelData> statels = new List<StatelData>();

        /// <summary>
        /// </summary>
        private float x;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="zoneServer">
        /// </param>
        /// <param name="playfieldIdentity">
        /// </param>
        public Playfield(ZoneServer zoneServer, Identity playfieldIdentity)
            : this(zoneServer)
        {
            this.Identity = playfieldIdentity;
            this.statels = PlayfieldLoader.PFData[this.Identity.Instance].Statels;
        }

        /// <summary>
        /// </summary>
        /// <param name="zoneServer">
        /// </param>
        private Playfield(ZoneServer zoneServer)
        {
            this.server = zoneServer;

            this.playfieldBus = BusSetup.StartWith<AsyncConfiguration>().Construct();
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtomationMessageToClient>(SendAOtomationMessageToClient));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtomationMessageToPlayfield>(this.SendAOtomationMessageToPlayfield));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtomationMessageToPlayfieldOthers>(
                    this.SendAOtomationMessageToPlayfieldOthers));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtomationMessageBodyToClient>(this.SendAOtomationMessageBodyToClient));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtomationMessageBodiesToClient>(
                    this.SendAOtomationMessageBodiesToClient));
            this.memBusDisposeContainer.Add(this.playfieldBus.Subscribe<IMSendPlayerSCFUs>(this.SendSCFUsToClient));
            this.memBusDisposeContainer.Add(this.playfieldBus.Subscribe<IMExecuteFunction>(this.ExecuteFunction));
            this.Entities = new Pool();
            this.heartBeat = new Timer(this.HeartBeatTimer, null, 10, 0);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public List<PlayfieldDistrict> Districts
        {
            get
            {
                return this.districts;
            }

            private set
            {
                this.districts = value;
            }
        }

        /// <summary>
        /// </summary>
        public Pool Entities { get; private set; }

        /// <summary>
        /// </summary>
        public List<Functions> EnvironmentFunctions { get; private set; }

        /// <summary>
        /// </summary>
        public Expansions Expansion { get; set; }

        /// <summary>
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        /// </summary>
        public IBus PlayfieldBus { get; set; }

        /// <summary>
        /// </summary>
        public float X
        {
            get
            {
                return this.X;
            }

            set
            {
                this.x = value;
            }
        }

        /// <summary>
        /// </summary>
        public float XScale { get; set; }

        /// <summary>
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// </summary>
        public float ZScale { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="clientMessage">
        /// </param>
        public static void SendAOtomationMessageToClient(IMSendAOtomationMessageToClient clientMessage)
        {
            LogUtil.Debug(clientMessage.message.Body.GetType().ToString());
            clientMessage.client.SendCompressed(clientMessage.message.Body);
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Announce(Message message)
        {
            Announce(message.Body);
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        public void Announce(MessageBody messageBody)
        {
            foreach (IEntity entity in this.Entities.GetAll((int)IdentityType.CanbeAffected))
            {
                var character = entity as Character;

                if (character != null)
                {
                    // Make this whole thing unblocking with publishing single internal messages
                    this.Publish(
                        new IMSendAOtomationMessageBodyToClient() { client = character.Client, Body = messageBody });
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public void AnnounceAppearanceUpdate(ICharacter character)
        {
            AppearanceUpdate.AnnounceAppearanceUpdate(character);
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        /// <param name="dontSend">
        /// </param>
        public void AnnounceOthers(MessageBody messageBody, Identity dontSend)
        {
            foreach (IInstancedEntity entity in this.Entities.GetAll((int)IdentityType.CanbeAffected))
            {
                var character = entity as Character;
                if (character != null)
                {
                    if (character.Identity != dontSend)
                    {
                        // Make this whole thing unblocking with publishing single internal messages
                        this.Publish(
                            new IMSendAOtomationMessageBodyToClient() { client = character.Client, Body = messageBody });
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        public void Despawn(Identity identity)
        {
            this.Announce(ZoneEngine.Core.Packets.Despawn.Create(identity));
        }

        /// <summary>
        /// </summary>
        public void DisconnectAllClients()
        {
            IEnumerable<IEntity> templist =
                this.Entities.GetAll((int)IdentityType.CanbeAffected).Where(et => et is Character).ToList();
            for (int i = templist.Count() - 1; i >= 0; i--)
            {
                IEntity entity = templist.ElementAt(i);
                if ((entity as Character) != null)
                {
                    this.server.DisconnectClient((entity as Character).Client);
                    (entity as Character).Dispose();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="entity">
        /// </param>
        public void DisconnectClient(IInstancedEntity entity)
        {
            this.Entities.RemoveObject(entity);
        }

        /// <summary>
        /// </summary>
        /// <param name="imExecuteFunction">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void ExecuteFunction(IMExecuteFunction imExecuteFunction)
        {
            var user = (ITargetingEntity)this.FindNamedEntityByIdentity(imExecuteFunction.User);
            INamedEntity target;

            // TODO: Go over the targets, they can return item templates, inventory entries etc too
            switch (imExecuteFunction.Function.Target)
            {
                case 1:
                    target = (INamedEntity)user;
                    break;
                case 2:
                    throw new NotImplementedException("Target Wearer not implemented yet");
                case 3:
                    target = this.FindNamedEntityByIdentity(user.SelectedTarget);
                    break;
                case 14:
                    target = this.FindNamedEntityByIdentity(user.FightingTarget);
                    break;
                case 19: // Perhaps (if issued from a item) its the item itself
                    target = (INamedEntity)user;
                    break;
                case 23:
                    target = this.FindNamedEntityByIdentity(user.SelectedTarget);
                    break;
                case 26:
                    target = (INamedEntity)user;
                    break;
                case 100:
                    target = (INamedEntity)user;
                    break;
                default:
                    throw new NotImplementedException(
                        "Unknown target encountered: Target#:" + imExecuteFunction.Function.Target);
            }

            if (target == null)
            {
                var temp = user as Character;
                if (temp != null)
                {
                    temp.Client.SendCompressed(
                        new ChatTextMessage { Identity = temp.Identity, Text = "No valid target found" });
                    return;
                }
            }

            FunctionCollection.Instance.CallFunction(
                imExecuteFunction.Function.FunctionType, 
                (INamedEntity)user, 
                (INamedEntity)user, 
                target, 
                imExecuteFunction.Function.Arguments.Values.ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public IInstancedEntity FindByIdentity(Identity identity)
        {
            return this.Entities.GetObject<IInstancedEntity>(identity);
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public T FindByIdentity<T>(Identity identity) where T : Character
        {
            return this.Entities.GetObject<T>(identity);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="range">
        /// </param>
        /// <returns>
        /// </returns>
        public List<Character> FindInRange(ICharacter character, float range)
        {
            List<Character> temp = new List<Character>();
            Coordinate coord = character.Coordinates;
            foreach (IInstancedEntity entity in this.Entities.GetAll((int)IdentityType.CanbeAffected))
            {
                if (entity == character)
                {
                    continue;
                }
                if (((Character)entity).Coordinates.Distance2D(coord) <= range)
                {
                    temp.Add((Character)entity);
                }
            }

            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public INamedEntity FindNamedEntityByIdentity(Identity identity)
        {
            return this.Entities.GetObject<INamedEntity>(identity);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsInstancedPlayfield()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="global">
        /// </param>
        /// <returns>
        /// </returns>
        public Dictionary<Identity, string> ListAvailablePlayfields(bool global = true)
        {
            return this.server.ListAvailablePlayfields(global);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int NumberOfDynels()
        {
            return this.Entities.GetAll((int)IdentityType.CanbeAffected).Count();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int NumberOfPlayers()
        {
            return this.Entities.GetAll((int)IdentityType.CanbeAffected).Count(x => x is Character);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        public void Publish(object obj)
        {
            this.playfieldBus.Publish(obj);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="body">
        /// </param>
        public void Send(IZoneClient client, MessageBody body)
        {
            this.Publish(new IMSendAOtomationMessageBodyToClient() { client = client, Body = body });
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="message">
        /// </param>
        public void Send(IZoneClient client, Message message)
        {
            this.Publish(new IMSendAOtomationMessageToClient() { client = client, message = message });
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public void SendAOtMessageBodyToClient(IMSendAOtomationMessageBodyToClient msg)
        {
            msg.client.SendCompressed(msg.Body);
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public void SendAOtomationMessageBodiesToClient(IMSendAOtomationMessageBodiesToClient msg)
        {
            foreach (MessageBody mb in msg.Bodies)
            {
                msg.client.SendCompressed(mb);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public void SendAOtomationMessageBodyToClient(IMSendAOtomationMessageBodyToClient msg)
        {
            if (msg.client != null)
            {
                msg.client.SendCompressed(msg.Body);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="clientMessage">
        /// </param>
        public void SendAOtomationMessageToPlayfield(IMSendAOtomationMessageToPlayfield clientMessage)
        {
            this.Announce(clientMessage.Body);
        }

        /// <summary>
        /// </summary>
        /// <param name="clientMessage">
        /// </param>
        public void SendAOtomationMessageToPlayfieldOthers(IMSendAOtomationMessageToPlayfieldOthers clientMessage)
        {
            this.AnnounceOthers(clientMessage.Body, clientMessage.Identity);
        }

        /// <summary>
        /// </summary>
        /// <param name="sendSCFUs">
        /// </param>
        public void SendSCFUsToClient(IMSendPlayerSCFUs sendSCFUs)
        {
            Identity dontSendTo = sendSCFUs.toClient.Character.Identity;
            lock (this.Entities)
            {
                foreach (IEntity entity in
                    this.Entities.GetAll((int)IdentityType.CanbeAffected).Where(x => x is IPacketReceivingEntity))
                {
                    if (entity.Identity != dontSendTo)
                    {
                        var temp = entity as Character;
                        if (temp != null)
                        {
                            // TODO: make it NPC-safe
                            SimpleCharFullUpdateMessage simpleCharFullUpdate =
                                SimpleCharFullUpdate.ConstructMessage(temp);
                            sendSCFUs.toClient.SendCompressed(simpleCharFullUpdate);

                            var charInPlay = new CharInPlayMessage { Identity = temp.Identity, Unknown = 0x00 };
                            sendSCFUs.toClient.SendCompressed(charInPlay);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="destination">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <param name="playfield">
        /// </param>
        public void Teleport(Character character, Coordinate destination, IQuaternion heading, Identity playfield)
        {
            // Prevent client from entering this again
            if (character.DoNotDoTimers)
            {
                return;
            }

            character.DoNotDoTimers = true;

            // Teleport to another playfield
            ZoneEngine.Core.Packets.Teleport.Send(character, destination, heading, playfield);

            // Send packet, disconnect, and other playfield waits for connect

            DespawnMessage despawnMessage = ZoneEngine.Core.Packets.Despawn.Create(character.Identity);
            this.AnnounceOthers(despawnMessage, character.Identity);
            character.RawCoordinates = new Vector3() { X = destination.x, Y = destination.y, Z = destination.z };
            character.RawHeading = new Quaternion(heading.xf, heading.yf, heading.zf, heading.wf);
            character.Save();
            CharacterDao.SetPlayfield(character.Identity.Instance, (int)playfield.Type, playfield.Instance);

            // TODO: Get new server ip from chatengine (which has to log all zoneengine's playfields)
            // for now, just transmit our ip and port

            IPAddress tempIp;
            if (IPAddress.TryParse(Config.Instance.CurrentConfig.ZoneIP, out tempIp) == false)
            {
                IPHostEntry zoneHost = Dns.GetHostEntry(Config.Instance.CurrentConfig.ZoneIP);
                foreach (IPAddress ip in zoneHost.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        tempIp = ip;
                        break;
                    }
                }
            }

            var redirect = new ZoneRedirectionMessage
                           {
                               ServerIpAddress = tempIp, 
                               ServerPort = (ushort)this.server.TcpEndPoint.Port
                           };
            character.Client.SendCompressed(redirect);
            character.DoNotDoTimers = false;

            // character.Client.Server.DisconnectClient(character.Client);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="c">
        /// </param>
        private void CheckStatelCollision(ICharacter c)
        {
            foreach (StatelData sd in this.statels)
            {
                foreach (Events ev in
                    sd.Events.Where(
                        x => (x.EventType == (int)EventType.OnCollide) || (x.EventType == (int)EventType.OnEnter)))
                {
                    if (sd.Coord().Distance3D(c.Coordinates) < 2.0)
                    {
                        ev.Perform(c, c);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="c">
        /// </param>
        private void CheckWallCollision(ICharacter c)
        {
            WallCollisionResult wcr = WallCollision.CheckCollision(c.Coordinates, c.Playfield.Identity.Instance);
            if (wcr != null)
            {
                int destPlayfield = wcr.SecondWall.DestinationPlayfield;
                if (destPlayfield > 0)
                {
                    if (Program.DebugZoning)
                    {
                        LogUtil.Debug(wcr.ToString());
                    }

                    PlayfieldDestination dest =
                        PlayfieldLoader.PFData[destPlayfield].Destinations[wcr.SecondWall.DestinationIndex];

                    if (Program.DebugZoning)
                    {
                        LogUtil.Debug(dest.ToString());
                    }

                    float newX = (dest.EndX - dest.StartX) * wcr.Factor + dest.StartX;
                    float newZ = (dest.EndZ - dest.StartZ) * wcr.Factor + dest.StartZ;
                    float dist = WallCollision.Distance(dest.StartX, dest.StartZ, dest.EndX, dest.EndZ);
                    float headDistX = (dest.EndX - dest.StartX) / dist;
                    float headDistZ = (dest.EndZ - dest.StartZ) / dist;
                    newX -= headDistZ * 8;
                    newZ += headDistX * 8;

                    Coordinate destinationCoordinate = new Coordinate(newX, c.RawCoordinates.Y, newZ);

                    this.Teleport(
                        (Character)c, 
                        destinationCoordinate, 
                        c.RawHeading, 
                        new Identity() { Type = IdentityType.Playfield, Instance = destPlayfield });
                    return;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        private void HeartBeatTimer(object sender)
        {
            IEnumerable<IEntity> temp = null;
            lock (this.Entities)
            {
                temp = this.Entities.GetAll((int)IdentityType.CanbeAffected).Where(x => x is ICharacter);
            }

            foreach (ICharacter c in temp)
            {
                if (c != null)
                {
                    if (c.DoNotDoTimers || c.Starting)
                    {
                        continue;
                    }

                    this.CheckWallCollision(c);
                    this.CheckStatelCollision(c);
                }
            }

            this.heartBeat.Change(10, 0);
        }

        #endregion
    }
}