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
// Last modified: 2013-11-04 3:42 PM

#endregion

namespace CellAO.Relay
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Text;

    using CellAO.Relay.Common;

    using Config = Utility.Config.ConfigReadWrite;

    using IrcDotNet;

    #endregion

    public class RelayBot : BasicIrcBot
    {
        // List of all currently logged-in Twitter users.
        //private List<CellAOUsers> cellaoUsers;

        #region Fields

        private readonly string nickname = Config.Instance.CurrentConfig.RelayBotNick;

        private readonly string realname = Config.Instance.CurrentConfig.RelayBotNick;

        private readonly string username = Config.Instance.CurrentConfig.RelayBotIdent;

        #endregion

        #region Constructors and Destructors

        public RelayBot()
            : base()
        {
            //this.cellaoUsers = new List<CellAOUsers>();
        }

        #endregion

        #region Public Properties

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

        #region Methods

        protected override void InitializeChatCommandProcessors()
        {
            base.InitializeChatCommandProcessors();

            this.ChatCommandProcessors.Add("lusers", ProcessChatCommandListUsers);
            this.ChatCommandProcessors.Add("login", ProcessChatCommandLogIn);
            this.ChatCommandProcessors.Add("logout", ProcessChatCommandLogOut);
            this.ChatCommandProcessors.Add("send", ProcessChatCommandSend);
            //this.ChatCommandProcessors.Add("home", ProcessChatCommandHome);
            //this.ChatCommandProcessors.Add("mentions", ProcessChatCommandMentions);
        }

        protected override void InitializeCommandProcessors()
        {
             base.InitializeCommandProcessors();
        }

        protected override void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            //
        }

        protected override void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e)
        {
            //
        }

        protected override void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            this.SendGreeting(channel.Client.LocalUser, e.ChannelUser.User);
        }

        protected override void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e)
        {
            //
        }

        protected override void OnClientConnect(IrcClient client)
        {
            //
        }

        protected override void OnClientDisconnect(IrcClient client)
        {
            //
        }

        protected override void OnClientRegistered(IrcClient client)
        {
            //
        }

        protected override void OnLocalUserJoinedChannel(IrcLocalUser localUser, IrcChannelEventArgs e)
        {
            this.SendGreeting(localUser, e.Channel);
        }

        protected override void OnLocalUserLeftChannel(IrcLocalUser localUser, IrcChannelEventArgs e)
        {
            //
        }

        protected override void OnLocalUserMessageReceived(IrcLocalUser localUser, IrcMessageEventArgs e)
        {
            //
        }

        protected override void OnLocalUserNoticeReceived(IrcLocalUser localUser, IrcMessageEventArgs e)
        {
            //
        }

        //private TwitterBotUser GetTwitterBotUser(IrcUser ircUser)
       // {
            //var twitterUser = this.twitterUsers.SingleOrDefault(tu => tu.IrcUser == ircUser);
            //if (twitterUser == null)
            //    throw new InvalidOperationException(string.Format(
            //        "User '{0}' is not logged in to Twitter.", ircUser.NickName));
            //return twitterUser;
        //}

        private void ProcessChatCommandHome(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            //var sourceUser = (IrcUser)source;
            //var twitterBotUser = GetTwitterBotUser(sourceUser);

            //if (parameters.Count != 0)
            //    throw new InvalidCommandParametersException(1);

            //// List tweets on Home timeline of user.
            //var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            //client.LocalUser.SendMessage(replyTargets, "Recent tweets on home timeline of '{0}':",
            //    twitterBotUser.TwitterUser.ScreenName);
            //foreach (var tweet in twitterBotUser.ListTweetsOnHomeTimeline())
            //{
            //    SendTweetInfo(client, replyTargets, tweet);
            //}
        }

        private void ProcessChatCommandListUsers(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            //var sourceUser = (IrcUser)source;

            //if (parameters.Count != 0)
            //    throw new InvalidCommandParametersException(1);

            //// List all currently logged-in twitter users.
            //var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            //client.LocalUser.SendMessage(replyTargets, "Currently logged-in Twitter users ({0}):",
            //    this.cellaoUsers.Count);
            //foreach (var tu in this.cellaoUsers)
            //{
            //    //client.LocalUser.SendMessage(replyTargets, "{0} / {1} ({2} @ {3})",
            //       // tu.TwitterUser.ScreenName, tu.TwitterUser.Name, tu.IrcUser.NickName, tu.IrcUser.Client.ServerName);
            //}
        }

        private void ProcessChatCommandLogIn(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            //    var sourceUser = (IrcUser)source;
            //    var twitterUser = this.twitterUsers.SingleOrDefault(tu => tu.IrcUser == sourceUser);
            //    if (twitterUser != null)
            //        throw new InvalidOperationException(string.Format(
            //            "User '{0}' is already logged in to Twitter as {1}.", sourceUser.NickName,
            //            twitterUser.TwitterUser.ScreenName));

            //    if (parameters.Count != 2)
            //        throw new InvalidCommandParametersException(1);

            //    // Create new Twitter user and log in to service.
            //    var twitterBotUser = new TwitterBotUser(sourceUser);
            //    var success = twitterBotUser.LogIn(parameters[0], parameters[1]);

            //    var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            //    if (success)
            //    {
            //        // Log-in succeeded.

            //        this.twitterUsers.Add(twitterBotUser);

            //        client.LocalUser.SendMessage(replyTargets, "You are now logged in as {0} / '{1}'.",
            //            twitterBotUser.TwitterUser.ScreenName, twitterBotUser.TwitterUser.Name);
            //    }
            //    else
            //    {
            //        // Log-in failed.

            //        client.LocalUser.SendMessage(replyTargets, "Invalid log-in username/password.");
            //    }
        }

        private void ProcessChatCommandLogOut(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            //var sourceUser = (IrcUser)source;
            //var twitterBotUser = GetTwitterBotUser(sourceUser);

            //if (parameters.Count != 0)
            //    throw new InvalidCommandParametersException(1);

            //// Log out Twitter user.
            //twitterBotUser.LogOut();
            //this.twitterUsers.Remove(twitterBotUser);

            //var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            //client.LocalUser.SendMessage(replyTargets, "You are now logged out.");
        }

        private void ProcessChatCommandMentions(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            //var sourceUser = (IrcUser)source;
            //var twitterBotUser = GetTwitterBotUser(sourceUser);

            //if (parameters.Count != 0)
            //    throw new InvalidCommandParametersException(1);

            //// List tweets on Home timeline of user.
            //var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            //client.LocalUser.SendMessage(replyTargets, "Recent tweets mentioning '{0}':",
            //    twitterBotUser.TwitterUser.ScreenName);
            //foreach (var tweet in twitterBotUser.ListTweetsMentioningMe())
            //{
            //    SendTweetInfo(client, replyTargets, tweet);
            //}
        }

        private void ProcessChatCommandSend(
            IrcClient client,
            IIrcMessageSource source,
            IList<IIrcMessageTarget> targets,
            string command,
            IList<string> parameters)
        {
            //var sourceUser = (IrcUser)source;
            //var twitterBotUser = GetTwitterBotUser(sourceUser);

            //if (parameters.Count != 1)
            //    throw new InvalidCommandParametersException(1);

            //// Send tweet from user.
            //var tweetStatus = twitterBotUser.SendTweet(parameters[0].TrimStart());
            //var replyTargets = GetDefaultReplyTarget(client, sourceUser, targets);
            //client.LocalUser.SendMessage(replyTargets, "Tweet sent by {0} at {1}.", tweetStatus.User.ScreenName,
            //    tweetStatus.CreatedDate.ToLongTimeString());
        }

        private string SanitizeTextForIrc(string value)
        {
            var sb = new StringBuilder(value);
            sb.Replace('\r', ' ');
            sb.Replace('\n', ' ');
            return sb.ToString();
        }

        private void SendGreeting(IrcLocalUser localUser, IIrcMessageTarget target)
        {
            localUser.SendNotice(target, "This is the {0}, welcome.", ProgramInfo.AssemblyTitle);
            localUser.SendNotice(target, "Message me with '.help' for instructions on how to use me.");
            //localUser.SendNotice(target, "Remember to log in via a private message and not via the channel.");
        }

       // private void SendTweetInfo(IrcClient client, IList<IIrcMessageTarget> targets, TwitterStatus tweet)
       // {
            //client.LocalUser.SendMessage(targets, "@{0}: {1}", tweet.User.ScreenName,
            //    SanitizeTextForIrc(tweet.Text));
       // }

        #endregion
    }
}