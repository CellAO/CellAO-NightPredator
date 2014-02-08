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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Network;
    using CellAO.Core.Playfields;

    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    [Export(typeof(IHandleMessage))]
    public class VicinityChatHandler : IHandleMessage<TextMessage>
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
            ICharacter character = ((IZoneClient)sender).Character;
            ChatTextMessage ctm = new ChatTextMessage
            {
                Identity = character.Identity,
                Text = ((TextMessage)message.Body).Message.Text
            };
            IPlayfield playfield = character.Playfield;
            List<Character> charsInRange = playfield.FindInRange(character, 20.0f);
            foreach (Character characterInRange in charsInRange)
            {
                
            }
        }

        #endregion
    }
}