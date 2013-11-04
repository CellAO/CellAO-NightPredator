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
// Last modified: 2013-11-04 3:39 PM

#endregion

namespace CellAO.Relay.Common
{
    using IrcDotNet;

    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    // Provides access to basic commands for controlling an IRC bot.
    public abstract class BasicIrcBot : IrcBot
    {
        #region Constructors and Destructors

        public BasicIrcBot()
            : base()
        {
        }

        #endregion

        #region Public Properties

        public abstract IrcRegistrationInfo RegistrationInfo { get; }

        #endregion

        #region Methods

        protected override void InitializeChatCommandProcessors()
        {
            this.ChatCommandProcessors.Add("help", this.ProcessChatCommandHelp);
        }

        protected override void InitializeCommandProcessors()
        {
            this.CommandProcessors.Add("exit", this.ProcessCommandExit);
            this.CommandProcessors.Add("connect", this.ProcessCommandConnect);
            this.CommandProcessors.Add("c", this.ProcessCommandConnect);
            this.CommandProcessors.Add("disconnect", this.ProcessCommandDisconnect);
            this.CommandProcessors.Add("d", this.ProcessCommandDisconnect);
            this.CommandProcessors.Add("join", this.ProcessCommandJoin);
            this.CommandProcessors.Add("j", this.ProcessCommandJoin);
            this.CommandProcessors.Add("leave", this.ProcessCommandLeave);
            this.CommandProcessors.Add("l", this.ProcessCommandLeave);
            this.CommandProcessors.Add("list", this.ProcessCommandList);
        }

        private void ProcessChatCommandHelp(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            if (parameters.Count != 0)
            {
                throw new InvalidCommandParametersException(0);
            }

            // List all commands recognized by this bot.
            var replyTarget = this.GetDefaultReplyTarget(client, source, targets);
            client.LocalUser.SendMessage(replyTarget, "Commands recognized by bot:");
            client.LocalUser.SendMessage(
                replyTarget,
                string.Join(", ", this.ChatCommandProcessors.Select(kvPair => kvPair.Key)));
        }

        private void ProcessCommandConnect(string command, IList<string> parameters)
        {
            if (parameters.Count < 1)
            {
                throw new ArgumentException(Properties.Resources.MessageNotEnoughArgs);
            }

            this.Connect(parameters[0], this.RegistrationInfo);
        }

        private void ProcessCommandDisconnect(string command, IList<string> parameters)
        {
            if (parameters.Count < 1)
            {
                throw new ArgumentException(Properties.Resources.MessageNotEnoughArgs);
            }

            this.Disconnect(parameters[0]);
        }

        private void ProcessCommandExit(string command, IList<string> parameters)
        {
            this.Stop();
        }

        private void ProcessCommandJoin(string command, IList<string> parameters)
        {
            if (parameters.Count < 2)
            {
                throw new ArgumentException(Properties.Resources.MessageNotEnoughArgs);
            }

            // Join given channel on given server.
            var client = this.GetClientFromServerNameMask(parameters[0]);
            string channelName = parameters[1];
            client.Channels.Join(channelName);
        }

        private void ProcessCommandLeave(string command, IList<string> parameters)
        {
            if (parameters.Count < 2)
            {
                throw new ArgumentException(Properties.Resources.MessageNotEnoughArgs);
            }

            // Leave given channel on the given server.
            var client = this.GetClientFromServerNameMask(parameters[0]);
            string channelName = parameters[1];
            client.Channels.Leave(channelName);
        }

        private void ProcessCommandList(string command, IList<string> parameters)
        {
            // List all active server connections and channels of which local user is currently member.
            foreach (var client in this.Clients)
            {
                Console.Out.WriteLine("Server: {0}", client.ServerName ?? "(unknown)");
                foreach (var channel in client.Channels)
                {
                    if (channel.Users.Any(u => u.User == client.LocalUser))
                    {
                        Console.Out.WriteLine(" * {0}", channel.Name);
                    }
                }
            }
        }

        #endregion
    }
}