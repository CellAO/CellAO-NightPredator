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
    using System.Linq;
    using System.Net;
    using System.Reflection;

    using Cell.Core;

    using CellAO.Communication.Messages;
    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;

    using MemBus;
    using MemBus.Configurators;
    using MemBus.Support;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.SystemMessages;

    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Script;

    using IBus = MemBus.IBus;

    #endregion

    /// <summary>
    /// </summary>
    public class ZoneServer : ServerBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public HashSet<ZoneClient> Clients = new HashSet<ZoneClient>();

        /// <summary>
        /// </summary>
        public int Id;

        /// <summary>
        /// </summary>
        private readonly List<IPlayfield> playfields = new List<IPlayfield>();

        private readonly DisposeContainer memBusDisposeContainer = new DisposeContainer();

        private readonly IBus zoneBus;

        private readonly MessageSerializer messageSerializer = new MessageSerializer();

        #endregion

        #region Constructors and Destructors

        private readonly List<Type> subscribedMessageHandlers = new List<Type>();

        /// <summary>
        /// </summary>
        public ZoneServer()
        {
            // TODO: Get the Server id from chatengine or config file
            this.Id = 0x356;
            this.ClientDisconnected += this.ZoneServerClientDisconnected;

            // New Bus initialization
            this.zoneBus = BusSetup.StartWith<AsyncConfiguration>().Construct();

            this.subscribedMessageHandlers.Clear();

            this.SubscribeMessage<CharacterActionMessageHandler, CharacterActionMessage>();
            this.SubscribeMessage<CharDCMoveMessageHandler, CharDCMoveMessage>();
            this.SubscribeMessage<CharInPlayMessageHandler, CharInPlayMessage>();
            this.SubscribeMessage<ChatCmdMessageHandler, ChatCmdMessage>();
            this.SubscribeMessage<ContainerAddItemMessageHandler, ContainerAddItemMessage>();
            this.SubscribeMessage<FollowTargetMessageHandler, FollowTargetMessage>();
            this.SubscribeMessage<GenericCmdMessageHandler, GenericCmdMessage>();
            this.SubscribeMessage<LookAtMessageHandler, LookAtMessage>();
            this.SubscribeMessage<SkillMessageHandler, SkillMessage>();
            this.SubscribeMessage<SocialActionCmdMessageHandler, SocialActionCmdMessage>();
            this.SubscribeMessage<VicinityChatMessageHandler, TextMessage>();
            this.SubscribeMessage<TradeMessageHandler, TradeMessage>();
            this.SubscribeMessage<ZoneLoginMessageHandler, ZoneLoginMessage>();

            this.SubscribeMessage<KnuBotAnswerMessageHandler, KnuBotAnswerMessage>();
            this.SubscribeMessage<KnuBotCloseChatWindowMessageHandler, KnuBotCloseChatWindowMessage>();
            this.SubscribeMessage<KnuBotFinishTradeMessageHandler, KnuBotFinishTradeMessage>();
            this.SubscribeMessage<KnuBotTradeMessageHandler, KnuBotTradeMessage>();
            this.CheckSubscribedMessageHandlers();
        }

        private void SubscribeMessage<T, TU>() where T : AbstractMessageHandler<TU> where TU : MessageBody, new()
        {
            T def =
                (T)
                    typeof(T).GetProperty(
                        "Default",
                        BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
            this.memBusDisposeContainer.Add(this.zoneBus.Subscribe<MessageWrapper<TU>>(def.Receive));
            this.subscribedMessageHandlers.Add(typeof(TU));
        }

        private void CheckSubscribedMessageHandlers()
        {
            bool warned = false;
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in
                assembly.GetTypes()
                    .Where(x => x.IsClass && (x.GetCustomAttributes(typeof(MessageHandlerAttribute), true).Any())))
            {
                if (type.BaseType != null)
                {
                    Type genericArgument = type.BaseType.GetGenericArguments()[0];
                    MessageHandlerAttribute handlerAttribute =
                        (MessageHandlerAttribute)
                            type.GetCustomAttributes(typeof(MessageHandlerAttribute), true).FirstOrDefault();

                    if (handlerAttribute.Direction == MessageHandlerDirection.None)
                    {
                        Console.WriteLine(
                            "Warning: '" + type.Name
                            + "' has no Direction defined (MessageHandlerAttribute missing in declaration?)");
                    }
                    else
                    {
                        if (handlerAttribute.Direction != MessageHandlerDirection.OutboundOnly)
                        {
                            if (!this.subscribedMessageHandlers.Contains(genericArgument))
                            {
                                // Found a Messagehandler which is not subscribed
                                if (!warned)
                                {
                                    Console.WriteLine("Warning! Following Messagehandlers have not been subscribed!");
                                    warned = true;
                                }
                                Console.WriteLine("Missing: " + type.Name);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void DisconnectAllClients()
        {
            foreach (Playfield pf in this.playfields)
            {
                pf.DisconnectAllClients();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="global">
        /// </param>
        /// <returns>
        /// </returns>
        public Dictionary<Identity, string> ListAvailablePlayfields(bool global = true)
        {
            Dictionary<Identity, string> temp = new Dictionary<Identity, string>();
            Dictionary<int, string> names = Playfields.Playfields.PlayfieldNames();

            if (!global)
            {
                foreach (Playfield pf in this.playfields)
                {
                    temp.Add(pf.Identity, names[pf.Identity.Instance]);
                }
            }
            else
            {
                foreach (KeyValuePair<int, string> pf in names)
                {
                    temp.Add(new Identity() { Type = IdentityType.Playfield, Instance = pf.Key }, pf.Value);
                }
            }

            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <returns>
        /// </returns>
        public IPlayfield PlayfieldById(Identity id)
        {
            // TODO: This needs to be changed to check for whole Identity
            foreach (IPlayfield pf in this.playfields)
            {
                if (pf.Identity == id)
                {
                    return pf;
                }
            }

            this.CreatePlayfield(id);
            return this.PlayfieldById(id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="messageobject">
        /// </param>
        internal void ProcessISComMessage(DynamicMessage messageobject)
        {
            // Switch to handlers
            if (messageobject.DataObject is ChatCommand)
            {
                this.HandleChatCommand((ChatCommand)messageobject.DataObject);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override IClient CreateClient()
        {
            return new ZoneClient(this, this.messageSerializer, this.zoneBus);
        }

        /// <summary>
        /// </summary>
        /// <param name="playfieldIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        protected IPlayfield CreatePlayfield(Identity playfieldIdentity)
        {
            var temp = new Playfield(this, playfieldIdentity);
            this.playfields.Add(temp);
            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="num_bytes">
        /// </param>
        /// <param name="buf">
        /// </param>
        /// <param name="ip">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void OnReceiveUDP(int num_bytes, byte[] buf, IPEndPoint ip)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="clientIP">
        /// </param>
        /// <param name="num_bytes">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected override void OnSendTo(IPEndPoint clientIP, int num_bytes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="chatCommand">
        /// </param>
        private void HandleChatCommand(ChatCommand chatCommand)
        {
            foreach (Playfield playfield in this.playfields)
            {
                ICharacter character =
                    playfield.FindByIdentity<Character>(
                        new Identity { Type = IdentityType.CanbeAffected, Instance = chatCommand.CharacterId });
                if (character != null)
                {
                    string fullArgs = chatCommand.ChatCommandString.TrimEnd(char.MinValue).TrimStart('.');

                    string temp = string.Empty;
                    do
                    {
                        temp = fullArgs;
                        fullArgs = fullArgs.Replace("  ", " ");
                    }
                    while (temp != fullArgs);

                    string[] cmdArgs = fullArgs.Trim().Split(' ');

                    ScriptCompiler.Instance.CallChatCommand(
                        cmdArgs[0].ToLower(),
                        character.Controller.Client,
                        character.Identity,
                        cmdArgs);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="forced">
        /// </param>
        private void ZoneServerClientDisconnected(IClient client, bool forced)
        {
            ZoneClient cli = (ZoneClient)client;
            if (cli != null)
            {
                cli.Dispose();
            }
        }

        public override void Stop()
        {
            foreach (Playfield pf in this.playfields)
            {
                pf.Dispose();
            }
            base.Stop();
        }

        #endregion
    }
}