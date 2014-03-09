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

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Components;
    using CellAO.Core.Network;
    using CellAO.Enums;
    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.InternalMessages;

    #endregion

    /// <summary>
    /// </summary>
    public class SkillMessageHandler : BaseMessageHandler<SkillMessage, SkillMessageHandler>
    {
        /// <summary>
        /// </summary>
        public SkillMessageHandler()
        {
            this.Direction = MessageHandlerDirection.InboundOnly;
        }

        #region Inbound

        /// <summary>
        /// </summary>
        /// <param name="skillMessage">
        /// </param>
        /// <param name="client">
        /// </param>
        protected override void Read(SkillMessage skillMessage, IZoneClient client)
        {
            uint baseIp = 0;

            uint characterLevel = client.Character.Stats[StatIds.level].BaseValue;

            // Calculate base IP value for character level
            if (characterLevel > 204)
            {
                baseIp += (characterLevel - 204) * 600000;
                characterLevel = 204;
            }

            if (characterLevel > 189)
            {
                baseIp += (characterLevel - 189) * 150000;
                characterLevel = 189;
            }

            if (characterLevel > 149)
            {
                baseIp += (characterLevel - 149) * 80000;
                characterLevel = 149;
            }

            if (characterLevel > 99)
            {
                baseIp += (characterLevel - 99) * 40000;
                characterLevel = 99;
            }

            if (characterLevel > 49)
            {
                baseIp += (characterLevel - 49) * 20000;
                characterLevel = 49;
            }

            if (characterLevel > 14)
            {
                baseIp += (characterLevel - 14) * 10000; // Change 99 => 14 by Wizard
                characterLevel = 14;
            }

            baseIp += 1500 + ((characterLevel - 1) * 4000);

            int count = skillMessage.Skills.Length;
            var statlist = new List<int>();
            while (count > 0)
            {
                count--;
                GameTuple<CharacterStat, uint> stat = skillMessage.Skills[count];
                client.Character.Stats[(int)stat.Value1].Value = (int)stat.Value2;
                statlist.Add((int)stat.Value1);
            }

            statlist.Add(53); // IP
            uint usedIp = baseIp - (uint)Math.Floor(SkillUpdate.CalculateIP(client.Character.Stats));
            client.Character.Stats[StatIds.ip].BaseValue = usedIp;

            // Send the changed stats back to the client
            count = 0;
            var newStats = new List<GameTuple<CharacterStat, uint>>();
            while (count < statlist.Count)
            {
                int stat = statlist[count];
                uint statval = client.Character.Stats[stat].BaseValue;
                newStats.Add(new GameTuple<CharacterStat, uint> { Value1 = (CharacterStat)stat, Value2 = statval });
                count++;
            }

            var reply = new SkillMessage
                        {
                            Identity = skillMessage.Identity, 
                            Unknown = 0x00, 
                            Skills = newStats.ToArray()
                        };

            client.Character.Playfield.Publish(
                new IMSendAOtomationMessageBodyToClient { client = client, Body = reply });

            // and save the changes to the statsdb
            client.Character.WriteStats();
        }

        #endregion
    }
}