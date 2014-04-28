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

namespace ZoneEngine.Core.Packets
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Network;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.InternalMessages;

    #endregion

    /// <summary>
    ///     Set/Get clients stat
    /// </summary>
    public static class Stat
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void Send(IZoneClient client, int stat, int value, bool announce)
        {
            Send(client, stat, (UInt32)value, announce);
        }

        /// <summary>
        /// </summary>
        /// <param name="dynel">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void Send(Dynel dynel, int stat, uint value, bool announce)
        {
            Send(dynel, stat, (int)value, announce);
        }

        /// <summary>
        /// </summary>
        /// <param name="dynel">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void Send(Dynel dynel, int stat, int value, bool announce)
        {
            ICharacter character = dynel as ICharacter;
            if (character != null)
            {
                Send(character, stat, value, announce);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void Send(IZoneClient client, int stat, uint value, bool announce)
        {
            var statMessage = new StatMessage
                              {
                                  Identity = client.Controller.Character.Identity,
                                  Stats =
                                      new[]
                                      {
                                          new GameTuple<CharacterStat, uint>
                                          {
                                              Value1 =
                                                  (CharacterStat)stat,
                                              Value2 = value
                                          }
                                      }
                              };
            var statM = new Message { Body = statMessage };
            if (!client.Controller.Character.DoNotDoTimers)
            {
                client.Controller.Character.Playfield.Publish(
                    new IMSendAOtomationMessageToClient { client = client, message = statM });
            }

            /* announce to playfield? */
            if (announce)
            {
                client.Controller.Character.Playfield.AnnounceOthers(statMessage, client.Controller.Character.Identity);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void Send(ICharacter character, int stat, uint value, bool announce)
        {
            if (character.Controller.Client != null)
            {
                Send((IZoneClient)character.Controller.Client, stat, value, announce);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void Send(ICharacter character, int stat, int value, bool announce)
        {
            if (character.Controller.Client != null)
            {
                Send(character.Controller.Client, stat, value, announce);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ch">
        /// </param>
        /// <param name="statsToUpdate">
        /// </param>
        public static void SendBulk(ICharacter ch, Dictionary<int, uint> statsToUpdate)
        {
            if (statsToUpdate.Count == 0)
            {
                return;
            }

            var toPlayfield = new List<int>();
            foreach (KeyValuePair<int, uint> keyValuePair in statsToUpdate)
            {
                if (ch.Stats[keyValuePair.Key].AnnounceToPlayfield)
                {
                    toPlayfield.Add(keyValuePair.Key);
                }
            }

            var stats = new List<GameTuple<CharacterStat, uint>>();
            foreach (KeyValuePair<int, uint> keyValuePair in statsToUpdate)
            {
                if (toPlayfield.Contains(keyValuePair.Key))
                {
                    stats.Add(
                        new GameTuple<CharacterStat, uint>
                        {
                            Value1 = (CharacterStat)keyValuePair.Key,
                            Value2 = keyValuePair.Value
                        });
                }
            }

            /* announce to playfield? */
            if (toPlayfield.Any() == false)
            {
                return;
            }

            var message = new StatMessage { Identity = ch.Identity, Stats = stats.ToArray() };

            ch.Playfield.AnnounceOthers(message, ch.Identity);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="statsToUpdate">
        /// </param>
        public static void SendBulk(IZoneClient client, Dictionary<int, uint> statsToUpdate)
        {
            if (statsToUpdate.Count == 0)
            {
                return;
            }

            var toPlayfieldIds = new List<int>();
            foreach (KeyValuePair<int, uint> keyValuePair in statsToUpdate)
            {
                if (client.Controller.Character.Stats[keyValuePair.Key].AnnounceToPlayfield)
                {
                    toPlayfieldIds.Add(keyValuePair.Key);
                }
            }

            var toPlayfield = new List<GameTuple<CharacterStat, uint>>();
            var toClient = new List<GameTuple<CharacterStat, uint>>();

            foreach (KeyValuePair<int, uint> keyValuePair in statsToUpdate)
            {
                var statValue = new GameTuple<CharacterStat, uint>
                                {
                                    Value1 = (CharacterStat)keyValuePair.Key,
                                    Value2 = keyValuePair.Value
                                };
                toClient.Add(statValue);

                if (toPlayfieldIds.Contains(keyValuePair.Key))
                {
                    toPlayfield.Add(statValue);
                }
            }

            var message = new StatMessage
                          {
                              Identity = client.Controller.Character.Identity,
                              Stats = toClient.ToArray()
                          };

            client.SendCompressed(message);

            /* announce to playfield? */
            if (toPlayfieldIds.Count > 0)
            {
                message.Stats = toPlayfield.ToArray();
                client.Controller.Character.Playfield.AnnounceOthers(message, client.Controller.Character.Identity);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <param name="announce">
        /// </param>
        public static void SendDirect(IZoneClient client, int stat, uint value, bool announce)
        {
            var statMessage = new StatMessage
                              {
                                  Identity = client.Controller.Character.Identity,
                                  Stats =
                                      new[]
                                      {
                                          new GameTuple<CharacterStat, uint>
                                          {
                                              Value1 =
                                                  (CharacterStat)stat,
                                              Value2 = value
                                          }
                                      }
                              };
            client.SendCompressed(statMessage);
        }

        /// <summary>
        /// Set own stat (no announce)
        /// </summary>
        /// <param name="client">
        /// Affected client
        /// </param>
        /// <param name="stat">
        /// Stat
        /// </param>
        /// <param name="value">
        /// Value
        /// </param>
        /// <param name="announce">
        /// Let others on same playfield know?
        /// </param>
        /// <returns>
        /// The <see cref="uint"/>.
        /// </returns>
        public static uint Set(IZoneClient client, int stat, uint value, bool announce)
        {
            var oldValue = (uint)client.Controller.Character.Stats[stat].Value;
            Send(client, stat, value, announce);
            return oldValue;
        }

        #endregion
    }
}