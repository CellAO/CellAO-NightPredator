#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-10-27 10:00
// Created:       2013-10-27 07:58

#endregion

namespace CellAO.Core.Playfields
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using Cell.Core;

    using CellAO.Enums;
    using CellAO.Interfaces;

    using MemBus;
    using MemBus.Configurators;
    using MemBus.Support;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    public class Playfield : IPlayfield
    {
        /// <summary>
        /// </summary>
        public IBus PlayfieldBus { get; set; }

        /// <summary>
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        /// </summary>
        private readonly ServerBase server;

        /// <summary>
        /// </summary>
        private List<IPlayfieldDistrict> districts = new List<IPlayfieldDistrict>();

        /// <summary>
        /// </summary>
        public HashSet<IInstancedEntity> Entities { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int NumberOfPlayers()
        {
            int count = 0;
            foreach (IInstancedEntity instancedEntity in this.Entities)
            {
                if ((instancedEntity as IPacketReceivingEntity) != null)
                {
                    count++;
                }
            }

            return count++;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int NumberOfDynels()
        {
            return this.Entities.Count;
        }

        /// <summary>
        /// </summary>
        public List<IFunctions> EnvironmentFunctions { get; private set; }

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
        public List<IPlayfieldDistrict> Districts
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
        private float x;

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
        public float Z { get; set; }

        /// <summary>
        /// </summary>
        public float XScale { get; set; }

        /// <summary>
        /// </summary>
        public float ZScale { get; set; }

        /// <summary>
        /// </summary>
        public Expansions Expansion { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public IInstancedEntity FindByIdentity(Identity identity)
        {
            foreach (IInstancedEntity entity in this.Entities)
            {
                if ((entity.Identity.Instance == identity.Instance) && (entity.Identity.Type == identity.Type))
                {
                    return entity;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public INamedEntity FindNamedEntityByIdentity(Identity identity)
        {
            foreach (IInstancedEntity entity in this.Entities)
            {
                if ((entity.Identity.Instance == identity.Instance) && (entity.Identity.Type == identity.Type))
                {
                    INamedEntity temp = entity as INamedEntity;
                    return temp;
                }
            }

            return null;
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
            foreach (IInstancedEntity entity in this.Entities)
            {
                var character = entity as Character;

                if (character != null)
                {
                    character.Send(messageBody);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        /// <param name="dontSend">
        /// </param>
        public void AnnounceOthers(MessageBody messageBody, Identity dontSend)
        {
            foreach (IInstancedEntity entity in this.Entities)
            {
                var character = entity as Character;
                if (character != null)
                {
                    if (character.Identity != dontSend)
                    {
                        character.Send(messageBody);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public void DisconnectAllClients()
        {
            foreach (IInstancedEntity entity in this.Entities)
            {
                if ((entity as Character) != null)
                {
                    (entity as Character).Client.Server.DisconnectClient((entity as Character).Client);
                }
            }
        }

        /// <summary>
        /// </summary>
        private readonly IBus playfieldBus;

        /// <summary>
        /// </summary>
        private readonly DisposeContainer memBusDisposeContainer = new DisposeContainer();

        /// <summary>
        /// </summary>
        public Playfield()
        {
            this.playfieldBus = BusSetup.StartWith<AsyncConfiguration>().Construct();
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtMessageToClient>(SendAOtMessageToClient));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtMessageToPlayfield>(this.SendAOtMessageToPlayfield));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtMessageToPlayfieldOthers>(this.SendAOtMessageToPlayfieldOthers));
            this.memBusDisposeContainer.Add(
                this.playfieldBus.Subscribe<IMSendAOtMessageBodyToClient>(this.SendAOtMessageBodyToClient));
            this.memBusDisposeContainer.Add(this.playfieldBus.Subscribe<IMSendPlayerSCFUs>(this.SendSCFUsToClient));
            this.memBusDisposeContainer.Add(this.playfieldBus.Subscribe<IMExecuteFunction>(this.ExecuteFunction));
            this.Entities = new HashSet<IInstancedEntity>();
        }

        /// <summary>
        /// </summary>
        /// <param name="playfieldIdentity">
        /// </param>
        public Playfield(Identity playfieldIdentity)
            : this()
        {
            this.Identity = playfieldIdentity;
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public void HandlePlayfieldMessage(InternalMessage message)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="clientMessage">
        /// </param>
        public static void SendAOtMessageToClient(IMSendAOtMessageToClient clientMessage)
        {
            LogUtil.Debug(clientMessage.message.Body.GetType().ToString());
            clientMessage.client.SendCompressed(clientMessage.message.Body);
        }

        /// <summary>
        /// </summary>
        /// <param name="clientMessage">
        /// </param>
        public void SendAOtMessageToPlayfield(IMSendAOtMessageToPlayfield clientMessage)
        {
            this.Announce(clientMessage.Body);
        }

        /// <summary>
        /// </summary>
        /// <param name="clientMessage">
        /// </param>
        public void SendAOtMessageToPlayfieldOthers(IMSendAOtMessageToPlayfieldOthers clientMessage)
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
            foreach (IEntity entity in this.Entities)
            {
                if (entity.Identity != dontSendTo)
                {
                    if (entity.Identity.Type == IdentityType.CanbeAffected)
                    {
                        var temp = entity as INamedEntity;
                        if (temp != null)
                        {
                            // TODO: make it NPC-safe
                            SimpleCharFullUpdateMessage simpleCharFullUpdate =
                                SimpleCharFullUpdate.ConstructMessage((Character)temp);
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
        /// <param name="msg">
        /// </param>
        public void SendAOtMessageBodyToClient(IMSendAOtMessageBodyToClient msg)
        {
            msg.client.SendCompressed(msg.Body);
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
                    break;
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

            Program.FunctionC.CallFunction(
                imExecuteFunction.Function.FunctionType,
                (INamedEntity)user,
                (INamedEntity)user,
                target,
                imExecuteFunction.Function.Arguments.Values.ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        public void Publish(object obj)
        {
            this.playfieldBus.Publish(obj);
        }
    }
}