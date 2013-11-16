#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-11-16 19:07

#endregion

namespace LoginEngine.MessageHandlers
{
    #region Usings ...

    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using CellAO.Core.Components;
    using CellAO.Database.Dao;

    using LoginEngine.CoreClient;
    using LoginEngine.Packets;

    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.SystemMessages;

    using Utility.Config;

    #endregion

    /// <summary>
    /// </summary>
    [Export(typeof(IHandleMessage))]
    public class SelectCharacterHandler : IHandleMessage<SelectCharacterMessage>
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="message">
        /// </param>
        public void Handle(object sender, Message message)
        {
            var client = (Client)sender;
            var selectCharacterMessage = (SelectCharacterMessage)message.Body;

            var checkLogin = new CheckLogin();
            if (checkLogin.IsCharacterOnAccount(client, selectCharacterMessage.CharacterId) == false)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    "Client '" + client.AccountName + "' tried to log in as CharID "
                    + selectCharacterMessage.CharacterId + " but it is not on their account!");
                Console.ResetColor();

                // NV: Is this really what we want to send? Should find out sometime...
                client.Send(0x00001F83, new LoginErrorMessage { Error = LoginError.InvalidUserNamePassword });
                client.Server.DisconnectClient(client);
                return;
            }

            if (OnlineDao.IsOnline(selectCharacterMessage.CharacterId).Online == 1)
            {
                Console.WriteLine(
                    "Client '" + client.AccountName
                    + "' is trying to login, but the requested character is already logged in.");
                client.Send(0x00001F83, new LoginErrorMessage { Error = LoginError.AlreadyLoggedIn });
                client.Server.DisconnectClient(client);
                return;
            }

            OnlineDao.SetOnline(selectCharacterMessage.CharacterId);

            IPAddress zoneIpAdress;
            if (IPAddress.TryParse(ConfigReadWrite.Instance.CurrentConfig.ZoneIP, out zoneIpAdress) == false)
            {
                IPHostEntry zoneHost = Dns.GetHostEntry(ConfigReadWrite.Instance.CurrentConfig.ZoneIP);
                zoneIpAdress = zoneHost.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            }

            var zoneRedirectionMessage = new ZoneInfoMessage
                                         {
                                             CharacterId = selectCharacterMessage.CharacterId, 
                                             ServerIpAddress = zoneIpAdress, 
                                             ServerPort =
                                                 (ushort)
                                                 ConfigReadWrite.Instance.CurrentConfig.ZonePort
                                         };
            client.Send(0x0000615B, zoneRedirectionMessage);
        }

        #endregion
    }
}