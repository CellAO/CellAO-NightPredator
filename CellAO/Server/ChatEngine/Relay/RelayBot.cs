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

namespace Chatengine.Relay
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChatEngine;
    using ChatEngine.Channels;
    using ChatEngine.CoreServer;
    using ChatEngine.Relay;
    using ChatEngine.Relay.Common;

    using IrcDotNet;

    using Utility;

    using Config = Utility.Config.ConfigReadWrite;

    #endregion

    /// <summary>
    /// </summary>
    public class RelayBot : BasicIrcBot
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly string ircchannel = Config.Instance.CurrentConfig.IRCChannel;

        /// <summary>
        /// </summary>
        private readonly string nickname = Config.Instance.CurrentConfig.RelayBotNick;

        /// <summary>
        /// </summary>
        private readonly string realname = Config.Instance.CurrentConfig.RelayBotNick;

        /// <summary>
        /// </summary>
        private readonly string username = Config.Instance.CurrentConfig.RelayBotIdent;

        /// <summary>
        /// 
        /// </summary>
        private readonly List<CellAoBotUser> cellAoBotUsers;

        /// <summary>
        /// </summary>
        private IrcClient client = null;

        private int GMlevel = 0;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public RelayBot()
            : base()
        {
            this.cellAoBotUsers = new List<CellAoBotUser>();
            this.InitializeRelayChatCommandProcessors();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public override IrcRegistrationInfo RegistrationInfo
        {
            get
            {
                return new IrcUserRegistrationInfo()
                       {
                           NickName = this.nickname,
                           UserName = this.username,
                           RealName = this.realname
                       };
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public override void IrcClient_ProtocolError(object sender, IrcProtocolErrorEventArgs e)
        {
            // on Protocol Error 433 (Nickname in Use) change to an alternate one
            if (e.Code == 433)
            {
                IrcClient client = (IrcClient)sender;
                string actualName = client.LocalUser.NickName;
                int temp = -1;
                if (actualName.Replace(Config.Instance.CurrentConfig.RelayBotNick, string.Empty) != string.Empty)
                {
                    int.TryParse(actualName.Replace(Config.Instance.CurrentConfig.RelayBotNick, string.Empty), out temp);
                    temp++;
                }
                else
                {
                    temp = 1;
                }

                actualName = Config.Instance.CurrentConfig.RelayBotNick + temp;
                client.LocalUser.SetNickName(actualName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="chatServer">
        /// </param>
        public void Run(ChatServer chatServer)
        {
            this.Run();
            if (Config.Instance.CurrentConfig.UseIRCRelay)
            {
                // Find ingame channel to relay
                string temp = Config.Instance.CurrentConfig.RelayIngameChannel;
                this.RelayedChannel = chatServer.Channels.FirstOrDefault(x => x.ChannelName == temp);
                if (this.RelayedChannel == null)
                {
                    LogUtil.Debug(DebugInfoDetail.Engine, "Could not find ChatEngine Channel '" + temp + "'");
                    return;
                }

                this.RelayedChannel.OnChannelMessage += this.RelayedChannel_OnChannelMessage;

                // Found ingame channel, now connect to IRC
                this.Connect(Config.Instance.CurrentConfig.IRCServer, this.RegistrationInfo);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        protected void InitializeRelayChatCommandProcessors()
        {
            this.ChatCommandProcessors.Add("lusers", this.ProcessChatCommandListUsers);
            this.ChatCommandProcessors.Add("login", this.ProcessChatCommandLogIn);
            this.ChatCommandProcessors.Add("logout", this.ProcessChatCommandLogOut);
            this.ChatCommandProcessors.Add("send", this.ProcessChatCommandSend);
            this.ChatCommandProcessors.Add("home", this.ProcessChatCommandHome);
            this.ChatCommandProcessors.Add("mentions", this.ProcessChatCommandMentions);
            this.ChatCommandProcessors.Add("serverinfo", this.ProcessChatCommandServerInfo);
            this.ChatCommandProcessors.Add("zoneinfo", this.ProcessChatCommandZoneInfo);
        }

        private void ProcessChatCommandZoneInfo(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            int numberOfZoneEnginesConnected = Program.ISCom.ClientCount;
            List<string> addressList = Program.ISCom.GetZoneEngineIds();
            StringBuilder sb = new StringBuilder();
            var sourceUser = (IrcUser)source;
            IList<IIrcMessageTarget> replyTargets = this.GetDefaultReplyTarget(client, sourceUser, targets);

            client.LocalUser.SendMessage(
                replyTargets,
                "{0}",
                "Number of ZoneEngines connected: " + numberOfZoneEnginesConnected);
            sb.AppendLine("Number of ZoneEngines connected: " + numberOfZoneEnginesConnected);
            foreach (string s in addressList)
            {
                client.LocalUser.SendMessage(replyTargets, "{0}", s);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            this.RelayedChannel.ChannelMessage(this.nickname + "-" + e.Source.Name, e.Text);
        }

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            this.SendGreeting(channel.Client.LocalUser, e.ChannelUser.User);
        }

        /// <summary>
        /// </summary>
        /// <param name="channel">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected override void OnClientConnect(IrcClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected override void OnClientDisconnect(IrcClient client)
        {
            this.client = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected override void OnClientRegistered(IrcClient client)
        {
            client.ListChannels(Config.Instance.CurrentConfig.IRCChannel);
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnLocalUserJoinedChannel(IrcLocalUser localUser, IrcChannelEventArgs e)
        {
            this.SendGreeting(localUser, e.Channel);
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnLocalUserLeftChannel(IrcLocalUser localUser, IrcChannelEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnLocalUserMessageReceived(IrcLocalUser localUser, IrcMessageEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void OnLocalUserNoticeReceived(IrcLocalUser localUser, IrcMessageEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected override void client_ChannelListReceived(object sender, IrcChannelListReceivedEventArgs e)
        {
            if (e.Channels.Count > 0)
            {
                foreach (IrcChannelInfo channelInfo in e.Channels)
                {
                    this.client.Channels.Join(new[] { channelInfo.Name });
                }
            }
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
        private void ProcessChatCommandHome(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            // var sourceUser = (IrcUser)source;
            // var twitterBotUser = GetTwitterBotUser(sourceUser);

            // if (parameters.Count != 0)
            // throw new InvalidCommandParametersException(1);

            //// List tweets on Home timeline of user.
            // var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            // client.LocalUser.SendMessage(replyTargets, "Recent tweets on home timeline of '{0}':",
            // twitterBotUser.TwitterUser.ScreenName);
            // foreach (var tweet in twitterBotUser.ListTweetsOnHomeTimeline())
            // {
            // SendTweetInfo(client, replyTargets, tweet);
            // }
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
        private void ProcessChatCommandListUsers(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            // var sourceUser = (IrcUser)source;

            // if (parameters.Count != 0)
            // throw new InvalidCommandParametersException(1);

            //// List all currently logged-in twitter users.
            // var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            // client.LocalUser.SendMessage(replyTargets, "Currently logged-in Twitter users ({0}):",
            // this.cellaoUsers.Count);
            // foreach (var tu in this.cellaoUsers)
            // {
            // //client.LocalUser.SendMessage(replyTargets, "{0} / {1} ({2} @ {3})",
            // // tu.TwitterUser.ScreenName, tu.TwitterUser.Name, tu.IrcUser.NickName, tu.IrcUser.Client.ServerName);
            // }
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
        private void ProcessChatCommandLogIn(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            var sourceUser = (IrcUser)source;
            CellAoBotUser CellAOUser = this.cellAoBotUsers.SingleOrDefault(tu => tu.IrcUser == sourceUser);

            if (CellAOUser != null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "User '{0}' is already logged in to CellAO as {1}.",
                        sourceUser,
                        CellAOUser.IrcUser.NickName));
            }
            if (parameters.Count != 2)
            {
                throw new InvalidCommandParametersException(1);
            }

            // // Create new CellAO user and log in to service.
            var cellaoBotUser = new CellAoBotUser(sourceUser);
           // bool success = cellaoBotUser.LogIn(parameters[0], parameters[1]);
            cellaoBotUser.LogIn(parameters[0], parameters[1]);
            
            IList<IIrcMessageTarget> replyTargets = this.GetDefaultReplyTarget(client, sourceUser, targets);
            if (cellaoBotUser.IsAuthenticated)
            {
                this.cellAoBotUsers.Add(cellaoBotUser);
                client.LocalUser.SendMessage(
                    replyTargets,
                    "You are now logged in as {0} / '{1}'.",
                    cellaoBotUser.IrcUser.NickName,
                    cellaoBotUser.IrcUser.NickName);
                GMlevel = cellaoBotUser.GMLevel(username);
            }
            else
            {
                client.LocalUser.SendMessage(replyTargets, "Login failed");
            }
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
        private void ProcessChatCommandLogOut(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            var sourceUser = (IrcUser)source;
            var cellaoBotUser = GetCellAOBotUser(sourceUser);
            if (parameters.Count != 0)
                throw new InvalidCommandParametersException(1);

            // Log out CellAO user.
            cellaoBotUser.LogOut();

            this.cellAoBotUsers.Remove(cellaoBotUser);
            IList<IIrcMessageTarget> replyTargets = this.GetDefaultReplyTarget(client, sourceUser, targets);
            client.LocalUser.SendMessage(replyTargets, "You are now logged out.");
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
        private void ProcessChatCommandMentions(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            // var sourceUser = (IrcUser)source;
            // var twitterBotUser = GetTwitterBotUser(sourceUser);

            // if (parameters.Count != 0)
            // throw new InvalidCommandParametersException(1);

            //// List tweets on Home timeline of user.
            // var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            // client.LocalUser.SendMessage(replyTargets, "Recent tweets mentioning '{0}':",
            // twitterBotUser.TwitterUser.ScreenName);
            // foreach (var tweet in twitterBotUser.ListTweetsMentioningMe())
            // {
            // SendTweetInfo(client, replyTargets, tweet);
            // }
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
        private void ProcessChatCommandSend(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            // var sourceUser = (IrcUser)source;
            // var twitterBotUser = GetTwitterBotUser(sourceUser);

            // if (parameters.Count != 1)
            // throw new InvalidCommandParametersException(1);

            //// Send tweet from user.
            // var tweetStatus = twitterBotUser.SendTweet(parameters[0].TrimStart());
            // var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            // client.LocalUser.SendMessage(replyTargets, "Tweet sent by {0} at {1}.", tweetStatus.User.ScreenName,
            // tweetStatus.CreatedDate.ToLongTimeString());
        }

        /// <summary>
        /// Need to change Some Information in this code. Probably a Seperate MOTD for IRC or something to have the bot read and send for
        /// connection Info.
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
        private void ProcessChatCommandServerInfo(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            IList<IIrcMessageTarget> replyTarget = this.GetDefaultReplyTarget(client, source, targets);

            client.LocalUser.SendMessage(replyTarget, "A How to Setup Connection to CellAO can be found here: TBA");
            client.LocalUser.SendMessage(
                replyTarget,
                "This is the address for the server: " + Config.Instance.CurrentConfig.ListenIP + "  On Port: "
                + Config.Instance.CurrentConfig.LoginPort);
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName">
        /// </param>
        /// <param name="text">
        /// </param>
        private void RelayedChannel_OnChannelMessage(object sender, ChannelMessageEventArgs e)
        {
            this.client.LocalUser.SendMessage(this.ircchannel, "[" + e.PlayerName + "] " + e.Text);
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        private string SanitizeTextForIrc(string value)
        {
            var sb = new StringBuilder(value);
            sb.Replace('\r', ' ');
            sb.Replace('\n', ' ');
            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="localUser">
        /// </param>
        /// <param name="target">
        /// </param>
        private void SendGreeting(IrcLocalUser localUser, IIrcMessageTarget target)
        {
            localUser.SendNotice(target, "This is the {0}, welcome.", ProgramInfo.AssemblyTitle);
            localUser.SendNotice(target, "Message me with '.help' for instructions on how to use me.");

            localUser.SendNotice(target, "Remember to log in via a private message and not via the channel.");
        }

        #endregion

        // List of all currently logged-in Twitter users.
        // private List<CellAOUsers> cellAoUserses;

        private CellAoBotUser GetCellAOBotUser(IrcUser ircUSer)
        {
            CellAoBotUser CellAOUser = this.cellAoBotUsers.SingleOrDefault(tu => tu.IrcUser == ircUSer);
            if (CellAOUser == null)
            {
                throw new InvalidOperationException(
                    string.Format("User '{0}' is not logged in to Cellao.", ircUSer.NickName));
            }
            return CellAOUser;
        }
    }
}