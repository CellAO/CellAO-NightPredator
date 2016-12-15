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

namespace ChatEngine.Relay.Common
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    using ChatEngine.Channels;
    using ChatEngine.Properties;

    using IrcDotNet;

    using Config = Utility.Config.ConfigReadWrite;

    #endregion

    // Provides core functionality for an IRC bot that operates via multiple clients.
    /// <summary>
    /// </summary>
    public abstract class IrcBot : IDisposable
    {
        #region Constants

        /// <summary>
        /// </summary>
        private const int clientQuitTimeout = 1000;

        #endregion

        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly Regex commandPartsSplitRegex = new Regex("(?<! /.*) ", RegexOptions.None);

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        public ChannelBase RelayedChannel = null;

        // Dictionary of all chat command processors, keyed by name.

        // Internal and exposable collection of all clients that communicate individually with servers.

        /// <summary>
        /// </summary>
        private readonly Collection<IrcClient> allClients;

        /// <summary>
        /// </summary>
        private readonly ReadOnlyCollection<IrcClient> allClientsReadOnly;

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, ChatCommandProcessor> chatCommandProcessors;

        // Dictionary of all command processors, keyed by name.
        /// <summary>
        /// </summary>
        private readonly IDictionary<string, CommandProcessor> commandProcessors;

        // True if the read loop is currently active, false if ready to terminate.

        /// <summary>
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// </summary>
        private IrcClient client;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public IrcBot()
        {
            this.isRunning = false;
            this.commandProcessors = new Dictionary<string, CommandProcessor>(StringComparer.InvariantCultureIgnoreCase);

            this.allClients = new Collection<IrcClient>();
            this.allClientsReadOnly = new ReadOnlyCollection<IrcClient>(this.allClients);
            this.chatCommandProcessors =
                new Dictionary<string, ChatCommandProcessor>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// </summary>
        ~IrcBot()
        {
            this.Dispose(false);
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="targets">
        /// </param>
        /// <param name="command">
        /// </param>
        /// <param name="parameters">
        /// </param>
        protected delegate void ChatCommandProcessor(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters);

        /// <summary>
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <param name="parameters">
        /// </param>
        protected delegate void CommandProcessor(string command, IList<string> parameters);

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public ReadOnlyCollection<IrcClient> Clients
        {
            get
            {
                return this.allClientsReadOnly;
            }
        }

        /// <summary>
        /// </summary>
        public virtual string QuitMessage
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected IDictionary<string, ChatCommandProcessor> ChatCommandProcessors
        {
            get
            {
                return this.chatCommandProcessors;
            }
        }

        /// <summary>
        /// </summary>
        protected IDictionary<string, CommandProcessor> CommandProcessors
        {
            get
            {
                return this.commandProcessors;
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
        /// <param name="server">
        /// </param>
        public void Disconnect(string server)
        {
            // Disconnect IRC client that is connected to given server.
            IrcClient client = this.GetClientFromServerNameMask(server);
            string serverName = client.ServerName;
            client.Quit(clientQuitTimeout, this.QuitMessage);
            client.Dispose();

            // Remove client from connection.
            this.allClients.Remove(client);

            Console.Out.WriteLine("Disconnected from '{0}'.", serverName);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public virtual void IrcClient_ProtocolError(object sender, IrcProtocolErrorEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        public virtual void Run()
        {
            this.isRunning = true;
        }

        /// <summary>
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="server">
        /// </param>
        /// <param name="registrationInfo">
        /// </param>
        protected void Connect(string server, IrcRegistrationInfo registrationInfo)
        {
            // Create new IRC client and connect to given server.
            //var client = new IrcClient();
            client = new IrcClient();
            client.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
            client.Connected += this.IrcClient_Connected;
            client.Disconnected += this.IrcClient_Disconnected;
            client.Registered += this.IrcClient_Registered;
            client.ProtocolError += this.IrcClient_ProtocolError;
            client.ChannelListReceived += this.client_ChannelListReceived;

            // Wait until connection has succeeded or timed out.
            using (var connectedEvent = new ManualResetEventSlim(false))
            {
                client.Connected += (sender2, e2) => connectedEvent.Set();
                client.Connect(server, false, registrationInfo);
                if (!connectedEvent.Wait(10000))
                {
                    client.Dispose();
                    ConsoleUtilities.WriteError("Connection to '{0}' timed out.", server);
                    return;
                }
            }

            // Add new client to collection.
            this.allClients.Add(client);

            Console.Out.WriteLine("Now connected to '{0}'.", server);
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // Disconnect each client gracefully.
                    foreach (IrcClient client in this.allClients)
                    {
                        if (client != null)
                        {
                            client.Quit(clientQuitTimeout, this.QuitMessage);
                            client.Dispose();
                        }
                    }
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="serverNameMask">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="IrcBotException">
        /// </exception>
        protected IrcClient GetClientFromServerNameMask(string serverNameMask)
        {
            IrcClient client =
                this.Clients.SingleOrDefault(
                    c => c.ServerName != null && Regex.IsMatch(c.ServerName, serverNameMask, RegexOptions.IgnoreCase));
            if (client == null)
            {
                throw new IrcBotException(
                    IrcBotExceptionType.NoConnection,
                    string.Format(Resources.MessageBotNoConnection, serverNameMask));
            }

            return client;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="targets">
        /// </param>
        /// <returns>
        /// </returns>
        protected IList<IIrcMessageTarget> GetDefaultReplyTarget(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets)
        {
            if (targets.Contains(client.LocalUser) && source is IIrcMessageTarget)
            {
                return new[] { (IIrcMessageTarget)source };
            }
            else
            {
                return targets;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected abstract void OnClientConnect(IrcClient client);

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected abstract void OnClientDisconnect(IrcClient client);

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected abstract void OnClientRegistered(IrcClient client);

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnLocalUserJoinedChannel(IrcLocalUser localUser, IrcChannelEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnLocalUserLeftChannel(IrcLocalUser localUser, IrcChannelEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnLocalUserMessageReceived(IrcLocalUser localUser, IrcMessageEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void OnLocalUserNoticeReceived(IrcLocalUser localUser, IrcMessageEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected abstract void client_ChannelListReceived(object sender, IrcChannelListReceivedEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Channel_MessageReceived(object sender, IrcMessageEventArgs e)
        {
            var channel = (IrcChannel)sender;

            if (e.Source is IrcUser)
            {
                // Read message and process if it is chat command.
                if (this.ReadChatCommand(channel.Client, e))
                {
                    return;
                }
            }

            this.OnChannelMessageReceived(channel, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Channel_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var channel = (IrcChannel)sender;

            this.OnChannelNoticeReceived(channel, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Channel_UserJoined(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;

            this.OnChannelUserLeft(channel, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Channel_UserLeft(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;

            this.OnChannelUserJoined(channel, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Connected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            this.OnClientConnect(client);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Disconnected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            this.OnClientDisconnect(client);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_LocalUser_JoinedChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined += this.IrcClient_Channel_UserJoined;
            e.Channel.UserLeft += this.IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived += this.IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived += this.IrcClient_Channel_NoticeReceived;

            this.OnLocalUserJoinedChannel(localUser, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_LocalUser_LeftChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined -= this.IrcClient_Channel_UserJoined;
            e.Channel.UserLeft -= this.IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived -= this.IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived -= this.IrcClient_Channel_NoticeReceived;

            this.OnLocalUserJoinedChannel(localUser, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_LocalUser_MessageReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            if (e.Source is IrcUser)
            {
                // Read message and process if it is chat command.
                if (this.ReadChatCommand(localUser.Client, e))
                {
                    return;
                }
            }

            this.OnLocalUserMessageReceived(localUser, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_LocalUser_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            this.OnLocalUserNoticeReceived(localUser, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void IrcClient_Registered(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            client.LocalUser.NoticeReceived += this.IrcClient_LocalUser_NoticeReceived;
            client.LocalUser.MessageReceived += this.IrcClient_LocalUser_MessageReceived;
            client.LocalUser.JoinedChannel += this.IrcClient_LocalUser_JoinedChannel;
            client.LocalUser.LeftChannel += this.IrcClient_LocalUser_LeftChannel;

            Console.Beep();

            this.OnClientRegistered(client);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="eventArgs">
        /// </param>
        /// <returns>
        /// </returns>
        private bool ReadChatCommand(IrcClient client, IrcMessageEventArgs eventArgs)
        {
            // Check if given message represents chat command.
            string line = eventArgs.Text;
            if (line.Length > 1 && line.StartsWith("."))
            {
                // Process command.
                string[] parts = commandPartsSplitRegex.Split(line.Substring(1)).Select(p => p.TrimStart('/')).ToArray();
                string command = parts.First();
                string[] parameters = parts.Skip(1).ToArray();
                this.ReadChatCommand(client, eventArgs.Source, eventArgs.Targets, command, parameters);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="targets">
        /// </param>
        /// <param name="command">
        /// </param>
        /// <param name="parameters">
        /// </param>
        private void ReadChatCommand(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            string[] parameters)
        {
            IList<IIrcMessageTarget> defaultReplyTarget = this.GetDefaultReplyTarget(client, source, targets);

            ChatCommandProcessor processor;
            if (this.chatCommandProcessors.TryGetValue(command, out processor))
            {
                try
                {
                    processor(client, source, targets, command, parameters);
                }
                catch (InvalidCommandParametersException exInvalidCommandParameters)
                {
                    client.LocalUser.SendNotice(defaultReplyTarget, exInvalidCommandParameters.GetMessage(command));
                }
                catch (Exception ex)
                {
                    if (source is IIrcMessageTarget)
                    {
                        client.LocalUser.SendNotice(
                            defaultReplyTarget,
                            "Error processing '{0}' command: {1}",
                            command,
                            ex.Message);
                    }
                }
            }
            else
            {
                if (source is IIrcMessageTarget)
                {
                    client.LocalUser.SendNotice(defaultReplyTarget, "Command '{0}' not recognized.", command);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <param name="parameters">
        /// </param>
        private void ReadCommand(string command, IList<string> parameters)
        {
            CommandProcessor processor;
            if (this.commandProcessors.TryGetValue(command, out processor))
            {
                try
                {
                    processor(command, parameters);
                }
                catch (Exception ex)
                {
                    ConsoleUtilities.WriteError("Error: {0}", ex.Message);
                }
            }
            else
            {
                ConsoleUtilities.WriteError("Command '{0}' not recognized.", command);
            }
        }

        #endregion

        // Regex for splitting space-separated list of command parts until first parameter that begins with '/'.
    }
}