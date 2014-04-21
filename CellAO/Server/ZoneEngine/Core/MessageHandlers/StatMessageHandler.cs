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
    public class StatMessageHandler : BaseMessageHandler<StatMessage, StatMessageHandler>
    {
        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="statsToClient">
        /// </param>
        /// <param name="statsToPlayfield">
        /// </param>
        public void SendBulk(
            ICharacter character, 
            Dictionary<int, uint> statsToClient, 
            Dictionary<int, uint> statsToPlayfield)
        {
            if (statsToClient.Count > 0)
            {
                this.Send(character, this.FillerBulk(character, statsToClient));
            }

            if (statsToPlayfield.Count > 0)
            {
                this.Send(character, this.FillerBulk(character, statsToPlayfield), true);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="statsToClient">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller FillerBulk(ICharacter character, Dictionary<int, uint> statsToClient)
        {
            return x =>
            {
                GameTuple<CharacterStat, uint>[] temp = new GameTuple<CharacterStat, uint>[statsToClient.Count];
                int cnt = 0;
                foreach (KeyValuePair<int, uint> kv in statsToClient)
                {
                    temp[cnt] = new GameTuple<CharacterStat, uint>()
                                {
                                    Value1 = (CharacterStat)kv.Key, 
                                    Value2 = kv.Value
                                };
                    cnt++;
                }

                x.Identity = character.Identity;
                x.Stats = temp;
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="statId">
        /// </param>
        /// <param name="statValue">
        /// </param>
        public void SendSingle(ICharacter character, int statId, uint statValue)
        {
            this.Send(character, this.Filler(character, statId, statValue));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="statId">
        /// </param>
        /// <param name="statValue">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller Filler(ICharacter character, int statId, uint statValue)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Stats = new[]
                          {
                              new GameTuple<CharacterStat, uint>() { Value1 = (CharacterStat)statId, Value2 = statValue }
                          };
            };
        }
    }
}