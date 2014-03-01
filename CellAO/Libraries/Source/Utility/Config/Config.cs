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

namespace Utility.Config
{
    using System.Net;

    #region enum

    /// <summary>
    /// This is the Enums for the Config File..
    /// </summary>
    public enum Information
    {
        /// <summary>
        /// IP to listen
        /// </summary>
        ListenIP, 

        /// <summary>
        /// Chat Server IP
        /// </summary>
        ChatIP, 

        /// <summary>
        /// Zone Server IP
        /// </summary>
        ZoneIP, 

        /// <summary>
        /// Communication Port between Zone and Chat Engines..
        /// </summary>
        CommPort, 

        /// <summary>
        /// Login Port Number
        /// </summary>
        LoginPort, 

        /// <summary>
        /// Zone Port Number
        /// </summary>
        ZonePort, 

        /// <summary>
        /// Chat Port Number
        /// </summary>
        ChatPort, 

        /// <summary>
        /// Chat Server's Motd
        /// </summary>
        Motd, 

        /// <summary>
        /// This is for the LoginEncryption to turn it on and off
        /// </summary>
        UsePassword, 

        /// <summary>
        /// This enables or Disables SQL Logging
        /// </summary>
        SqlLog, 

        /// <summary>
        /// This is for turning the chat logging on and off
        /// </summary>
        LogChat, 

        /// <summary>
        /// This is the Sql Connection String
        /// </summary>
        connectionString, 

        /// <summary>
        /// ISComm local IP address?
        /// </summary>
        ISCommLocalIP, 

        /// <summary>
        /// This is for the SQL Choices
        /// </summary>
        SQLType, 

        /// <summary>
        /// Locale language selection for you, en = english , gr = german, more languages as we go
        /// </summary>
        Locale, 

        /// <summary>
        /// 
        /// </summary>
        IRCServer, 

        /// <summary>
        /// 
        /// </summary>
        RelayBotNick, 

        /// <summary>
        /// 
        /// </summary>
        RelayBotIdent, 

        /// <summary>
        /// 
        /// </summary>
        IRCChannel, 

        /// <summary>
        /// 
        /// </summary>
        RelayIngameChannel, 

        /// <summary>
        /// 
        /// </summary>
        UseIRCRelay,

        /// <summary>
        /// 
        /// </summary>
        WebHostName,

        /// <summary>
        /// 
        /// </summary>
        WebHostPort,

        /// <summary>
        /// 
        /// </summary>
        WebHostRoot,

        /// <summary>
        /// 
        /// </summary>
        WebCoreRepo,

        /// <summary>
        /// 
        /// </summary>
        WebHostDefaultPage,
        
        /// <summary>
        /// 
        /// </summary>
        WebHostPhpPath

    }

    #endregion

    #region Config

    /// <summary>
    /// This here Sets up the Gets and Sets for the Config System.
    /// </summary>
    public class Config
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Chat server IP
        /// </summary>
        public string ChatIP { get; set; }

        /// <summary>
        /// Gets or sets the Chat Port
        /// </summary>
        public int ChatPort { get; set; }

        /// <summary>
        /// Gets or sets Communication Port for Communicating between Zone and Chat Engines.
        /// </summary>
        public int CommPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IRCChannel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IRCServer { get; set; }

        /// <summary>
        /// Gets or sets Local ISComm IP address
        /// </summary>
        public string ISCommLocalIP { get; set; }

        /// <summary>
        /// Gets or sets IP to listen
        /// </summary>
        public string ListenIP { get; set; }

        /// <summary>
        /// Gets or sets your Locale language
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable chat logging
        /// </summary>
        public bool LogChat { get; set; }

        /// <summary>
        /// Gets or sets the Login Port
        /// </summary>
        public int LoginPort { get; set; }

        /// <summary>
        /// Gets or sets the Motd for Chat Server
        /// </summary>
        public string Motd { get; set; }

        /// <summary>
        /// Gets or sets the MsSQL Connection String...
        /// </summary>
        public string MsSqlConnection { get; set; }

        /// <summary>
        /// Gets or sets the Mysql Connection String...
        /// </summary>
        public string MysqlConnection { get; set; }

        /// <summary>
        /// Gets or sets the  PostgreSQL Connection String..
        /// </summary>
        public string PostgreConnection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RelayBotIdent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RelayBotNick { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RelayIngameChannel { get; set; }

        /// <summary>
        /// Gets or sets Your SQL Type
        /// </summary>
        public string SQLType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable Sql Logging
        /// </summary>
        public bool SqlLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseIRCRelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the Password
        /// </summary>
        public bool UsePassword { get; set; }

        /// <summary>
        /// Gets or sets Zone server IP
        /// </summary>
        public string ZoneIP { get; set; }

        /// <summary>
        /// Gets or sets the Zone Port
        /// </summary>
        public int ZonePort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WebHostName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WebHostPort { get; set; }

        public string WebHostRoot { get; set; }

        public string WebCoreRepo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WebHostDefaultPage { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string WebHostPhpPath { get; set; }
        #endregion
    }

    #endregion
}