#region License

// Copyright (c) 2005-2016, CellAO Team
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

namespace LoginEngine.MessageHandlers
{
    #region Usings ...

    using System;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.Text;

    using CellAO.Core.Components;

    using LoginEngine.CoreClient;

    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.SystemMessages;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    [Export(typeof(IHandleMessage))]
    public class UserLoginHandler : IHandleMessage<UserLoginMessage>
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
            var userLoginMessage = (UserLoginMessage)message.Body;
            client.AccountName = userLoginMessage.UserName;
            client.ClientVersion = userLoginMessage.ClientVersion;
            Colouring.Push(ConsoleColor.Green);
            Console.WriteLine(
                "Client '" + client.AccountName + "' connected using version '" + client.ClientVersion + "'");
            Colouring.Pop();

            var salt = new byte[0x20];
            var rand = new Random();

            rand.NextBytes(salt);

            var sb = new StringBuilder();
            for (int i = 0; i < 32; i++)
            {
                // 0x00 Breaks Things
                if (salt[i] == 0)
                {
                    salt[i] = 42; // So we change it to something nicer
                }

                sb.Append(salt[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            client.ServerSalt = sb.ToString();
            var serverSaltMessage = new ServerSaltMessage { ServerSalt = salt };
            client.Send(0x00002B3F, serverSaltMessage);
        }

        #endregion
    }
}