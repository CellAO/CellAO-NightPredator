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

    using CellAO.Core.Components;
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    internal class KnuBotAnswerListMessageHandler :
        BaseMessageHandler<KnuBotAnswerListMessage, KnuBotAnswerListMessageHandler>
    {
        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="knubotTarget">
        /// </param>
        /// <param name="choices">
        /// </param>
        public void Send(ICharacter character, Identity knubotTarget, string[] choices)
        {
            this.Send(character, this.KnuBotAnswerList(character, knubotTarget, choices), false);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="knubotTarget">
        /// </param>
        /// <param name="choices">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller KnuBotAnswerList(ICharacter character, Identity knubotTarget, string[] choices)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Target = knubotTarget;
                List<KnuBotDialogOption> temp = new List<KnuBotDialogOption>();
                foreach (string choice in choices)
                {
                    temp.Add(new KnuBotDialogOption() { Text = choice });
                }

                x.DialogOptions = temp.ToArray();
                x.Unknown1 = 2;
            };
        }
    }
}