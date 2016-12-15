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

namespace ZoneEngine.Core.Packets
{
    #region Usings ...

    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    public static class TradeSkillPacket
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public static void SendNotTradeskill(ICharacter character)
        {
            var messageBody = new CharacterActionMessage()
                              {
                                  Identity = character.Identity,
                                  Action = CharacterActionType.TradeskillNotValid,
                                  Unknown = 0,
                                  Target = new Identity(),
                                  Parameter1 = 0,
                                  Parameter2 = 0
                              };
            character.Send(messageBody);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="min">
        /// </param>
        public static void SendOutOfRange(ICharacter character, int min)
        {
            var messageBody = new CharacterActionMessage()
                              {
                                  Identity = character.Identity,
                                  Action = CharacterActionType.TradeskillOutOfRange,
                                  Unknown = 0,
                                  Target = new Identity(),
                                  Parameter1 = 0,
                                  Parameter2 = min
                              };
            character.Send(messageBody);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="tradeSkillStatId">
        /// </param>
        /// <param name="tradeSkillRequirement">
        /// </param>
        public static void SendRequirement(ICharacter character, int tradeSkillStatId, int tradeSkillRequirement)
        {
            var messageBody = new CharacterActionMessage()
                              {
                                  Action = CharacterActionType.TradeskillRequirement,
                                  Identity = character.Identity,
                                  Unknown1 = 0,
                                  Target = new Identity(),
                                  Parameter1 = tradeSkillStatId,
                                  Parameter2 = tradeSkillRequirement
                              };
            character.Send(messageBody);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="min">
        /// </param>
        /// <param name="max">
        /// </param>
        /// <param name="low">
        /// </param>
        /// <param name="high">
        /// </param>
        public static void SendResult(ICharacter character, int min, int max, int low, int high)
        {
            var messageBody = new CharacterActionMessage()
                              {
                                  Action = CharacterActionType.TradeskillResult,
                                  Identity = character.Identity,
                                  Unknown1 = 0,
                                  Target =
                                      new Identity()
                                      {
                                          Type = (IdentityType)max,
                                          Instance = high
                                      },
                                  Parameter1 = min,
                                  Parameter2 = low
                              };

            character.Send(messageBody);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="count">
        /// </param>
        public static void SendSource(ICharacter character, int count)
        {
            var messageBody = new CharacterActionMessage()
                              {
                                  Identity = character.Identity,
                                  Action = CharacterActionType.TradeskillSource,
                                  Unknown = 0,
                                  Target = new Identity(),
                                  Parameter1 = 0,
                                  Parameter2 = count
                              };
            character.Send(messageBody);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="count">
        /// </param>
        public static void SendTarget(ICharacter character, int count)
        {
            var messageBody = new CharacterActionMessage()
                              {
                                  Identity = character.Identity,
                                  Action = CharacterActionType.TradeskillTarget,
                                  Unknown = 0,
                                  Target = new Identity(),
                                  Parameter1 = 0,
                                  Parameter2 = count
                              };
            character.Send(messageBody);
        }

        #endregion
    }
}