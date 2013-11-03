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
// Last modified: 2013-11-03 5:55 AM

#endregion

namespace ChatEngine.IRCRelay.Common
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    #endregion

    // Provides core functionality for an IRC bot that operates via multiple clients.
    public abstract class IrcBot : IDisposable
    {
        #region Constants

        private const int clientQuitTimeout = 1000;

        #endregion

        // Regex for splitting space-separated list of command parts until first parameter that begins with '/'.

        #region Static Fields

        private static readonly Regex commandPartsSplitRegex = new Regex("(?<! /.*) ", RegexOptions.None);

        #endregion

        // Dictionary of all chat command processors, keyed by name.

        // Internal and exposable collection of all clients that communicate individually with servers.

        #region Fields

        private Collection<IrcClient> allClients;

        private ReadOnlyCollection<IrcClient> allClientsReadOnly;

        private IDictionary<string, ChatCommandProcessor> chatCommandProcessors;

        // Dictionary of all command processors, keyed by name.
        private IDictionary<string, CommandProcessor> commandProcessors;

        // True if the read loop is currently active, false if ready to terminate.

        private bool isDisposed = false;

        private bool isRunning;

        #endregion

        #region Constructors and Destructors

        public IrcBot()
        {
            this.isRunning = false;
            this.commandProcessors = new Dictionary<string, CommandProcessor>(StringComparer.InvariantCultureIgnoreCase);
            this.InitializeCommandProcessors();

            this.allClients = new Collection<IrcClient>();
            this.allClientsReadOnly = new ReadOnlyCollection<IrcClient>(this.allClients);
            this.chatCommandProcessors =
                new Dictionary<string, ChatCommandProcessor>(StringComparer.InvariantCultureIgnoreCase);
            this.InitializeChatCommandProcessors();
        }

        ~IrcBot()
        {
            this.Dispose(false);
        }

        #endregion

        #region Delegates

        protected delegate void ChatCommandProcessor(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters);

        protected delegate void CommandProcessor(string command, IList<string> parameters);

        #endregion

        #region Public Properties

        public ReadOnlyCollection<IrcClient> Clients
        {
            get
            {
                return this.allClientsReadOnly;
            }
        }

        public virtual string QuitMessage
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Properties

        protected IDictionary<string, ChatCommandProcessor> ChatCommandProcessors
        {
            get
            {
                return this.chatCommandProcessors;
            }
        }

        protected IDictionary<string, CommandProcessor> CommandProcessors
        {
            get
            {
                return this.commandProcessors;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Disconnect(string server)
        {
            // Disconnect IRC client that is connected to given server.
            var client = this.GetClientFromServerNameMask(server);
            var serverName = client.ServerName;
            client.Quit(clientQuitTimeout, this.QuitMessage);
            client.Dispose();

            // Remove client from connection.
            this.allClients.Remove(client);

            Console.Out.WriteLine("Disconnected from '{0}'.", serverName);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run()
        {
            // Read commands from stdin until bot terminates.
            this.isRunning = true;
            while (this.isRunning)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (line.Length == 0)
                {
                    continue;
                }

                string[] parts = line.Split(' ');
                string command = parts[0].ToLower();
                string[] parameters = parts.Skip(1).ToArray();
                this.ReadCommand(command, parameters);
            }
        }

        public void Stop()
        {
            this.isRunning = false;
        }

        #endregion

        #region Methods

        protected void Connect(string server, IrcRegistrationInfo registrationInfo)
        {
            // Create new IRC client and connect to given server.
            var client = new IrcClient();
            client.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
            client.Connected += this.IrcClient_Connected;
            client.Disconnected += this.IrcClient_Disconnected;
            client.Registered += this.IrcClient_Registered;

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

        protected void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // Disconnect each client gracefully.
                    foreach (var client in this.allClients)
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

        protected IrcClient GetClientFromServerNameMask(string serverNameMask)
        {
            var client =
                this.Clients.SingleOrDefault(
                    c => c.ServerName != null && Regex.IsMatch(c.ServerName, serverNameMask, RegexOptions.IgnoreCase));
            if (client == null)
            {
                throw new IrcBotException(
                    IrcBotExceptionType.NoConnection,
                    string.Format(Properties.Resources.MessageBotNoConnection, serverNameMask));
            }
            return client;
        }

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

        protected abstract void InitializeChatCommandProcessors();

        protected abstract void InitializeCommandProcessors();

        protected abstract void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e);

        protected abstract void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e);

        protected abstract void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e);

        protected abstract void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e);

        protected abstract void OnClientConnect(IrcClient client);

        protected abstract void OnClientDisconnect(IrcClient client);

        protected abstract void OnClientRegistered(IrcClient client);

        protected abstract void OnLocalUserJoinedChannel(IrcLocalUser localUser, IrcChannelEventArgs e);

        protected abstract void OnLocalUserLeftChannel(IrcLocalUser localUser, IrcChannelEventArgs e);

        protected abstract void OnLocalUserMessageReceived(IrcLocalUser localUser, IrcMessageEventArgs e);

        protected abstract void OnLocalUserNoticeReceived(IrcLocalUser localUser, IrcMessageEventArgs e);

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

        private void IrcClient_Channel_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var channel = (IrcChannel)sender;

            this.OnChannelNoticeReceived(channel, e);
        }

        private void IrcClient_Channel_UserJoined(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;

            this.OnChannelUserLeft(channel, e);
        }

        private void IrcClient_Channel_UserLeft(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;

            this.OnChannelUserJoined(channel, e);
        }

        private void IrcClient_Connected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            this.OnClientConnect(client);
        }

        private void IrcClient_Disconnected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            this.OnClientDisconnect(client);
        }

        private void IrcClient_LocalUser_JoinedChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined += this.IrcClient_Channel_UserJoined;
            e.Channel.UserLeft += this.IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived += this.IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived += this.IrcClient_Channel_NoticeReceived;

            this.OnLocalUserJoinedChannel(localUser, e);
        }

        private void IrcClient_LocalUser_LeftChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined -= this.IrcClient_Channel_UserJoined;
            e.Channel.UserLeft -= this.IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived -= this.IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived -= this.IrcClient_Channel_NoticeReceived;

            this.OnLocalUserJoinedChannel(localUser, e);
        }

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

        private void IrcClient_LocalUser_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            this.OnLocalUserNoticeReceived(localUser, e);
        }

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

        private bool ReadChatCommand(IrcClient client, IrcMessageEventArgs eventArgs)
        {
            // Check if given message represents chat command.
            var line = eventArgs.Text;
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

        private void ReadChatCommand(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            string[] parameters)
        {
            var defaultReplyTarget = this.GetDefaultReplyTarget(client, source, targets);

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
    }
}