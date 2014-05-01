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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Database.Dao;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.PacketHandlers;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class CharacterInfoPacketMessageHandler :
        BaseMessageHandler<InfoPacketMessage, CharacterInfoPacketMessageHandler>
    {
        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="infoTarget">
        /// </param>
        public void Send(ICharacter character, ICharacter infoTarget)
        {
            this.Send(character, CharacterInfoPacket(character, infoTarget), false);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="tPlayer">
        /// </param>
        /// <returns>
        /// </returns>
        private static MessageDataFiller CharacterInfoPacket(ICharacter character, ICharacter tPlayer)
        {
            return x =>
            {
                uint LegacyScore = tPlayer.Stats[StatIds.pvp_rating].BaseValue;
                string LegacyTitle = null;
                if (LegacyScore < 1400)
                {
                    LegacyTitle = string.Empty;
                }
                else if (LegacyScore < 1500)
                {
                    LegacyTitle = "Freshman";
                }
                else if (LegacyScore < 1600)
                {
                    LegacyTitle = "Rookie";
                }
                else if (LegacyScore < 1700)
                {
                    LegacyTitle = "Apprentice";
                }
                else if (LegacyScore < 1800)
                {
                    LegacyTitle = "Novice";
                }
                else if (LegacyScore < 1900)
                {
                    LegacyTitle = "Neophyte";
                }
                else if (LegacyScore < 2000)
                {
                    LegacyTitle = "Experienced";
                }
                else if (LegacyScore < 2100)
                {
                    LegacyTitle = "Expert";
                }
                else if (LegacyScore < 2300)
                {
                    LegacyTitle = "Master";
                }
                else if (LegacyScore < 2500)
                {
                    LegacyTitle = "Champion";
                }
                else
                {
                    LegacyTitle = "Grand Master";
                }

                int orgGoverningForm = 0;
                try
                {
                    orgGoverningForm = OrganizationDao.Instance.GetGovernmentForm(character.Stats[StatIds.clan].Value);
                }
                catch (Exception)
                {
                }

                // Uses methods in ZoneEngine\PacketHandlers\OrgClient.cs
                /* Known packetFlags--
                    * 0x40 - No org | 0x41 - Org | 0x43 - Org and towers | 0x47 - Org, towers, player has personal towers | 0x50 - No pvp data shown
                    * Bitflags--
                    * Bit0 = hasOrg, Bit1 = orgTowers, Bit2 = personalTowers, Bit3 = (Int32) time until supression changes (Byte) type of supression level?, Bit4 = noPvpDataShown, Bit5 = hasFaction, Bit6 = ?, Bit 7 = null.
                */

                int? orgId;
                string orgRank;
                InfoPacketType type;
                if (tPlayer.Stats[StatIds.clan].BaseValue == 0)
                {
                    type = InfoPacketType.Character;
                    orgId = null;
                    orgRank = null;
                }
                else
                {
                    type = InfoPacketType.CharacterOrg;
                    orgId = (int?)tPlayer.Stats[StatIds.clan].BaseValue;
                    if (character.Stats[StatIds.clan].BaseValue == tPlayer.Stats[StatIds.clan].BaseValue)
                    {
                        orgRank = OrgClient.GetRank(orgGoverningForm, tPlayer.Stats[StatIds.clanlevel].BaseValue);
                    }
                    else
                    {
                        orgRank = string.Empty;
                    }
                }

                if (character.Stats[StatIds.npcfamily].Value != 0)
                {
                    type = InfoPacketType.Monster;
                    x.Info = new MonsterInfoPacket()
                             {
                                 CurrentHealth = tPlayer.Stats[StatIds.health].Value,
                                 Level = (byte)tPlayer.Stats[StatIds.level].Value,
                                 MaxHealth = tPlayer.Stats[StatIds.life].Value,
                                 OrganizationId = 0,
                                 Profession = (byte)tPlayer.Stats[StatIds.profession].Value,
                                 TitleLevel = (byte)tPlayer.Stats[StatIds.titlelevel].Value,
                                 VisualProfession =
                                     (byte)tPlayer.Stats[StatIds.visualprofession].Value,
                                 Unknown8 = 1234567890,
                                 Unknown9 = 1234567890,
                                 Unknown10 = 1234567890,
                             };
                }
                else
                {
                    x.Info = new CharacterInfoPacket
                             {
                                 Unknown1 = 0x01,
                                 Profession = (Profession)tPlayer.Stats[StatIds.profession].Value,
                                 Level = (byte)tPlayer.Stats[StatIds.level].Value,
                                 TitleLevel = (byte)tPlayer.Stats[StatIds.titlelevel].Value,
                                 VisualProfession =
                                     (Profession)tPlayer.Stats[StatIds.visualflags].Value,
                                 SideXp = 0,
                                 Health = tPlayer.Stats[StatIds.health].Value,
                                 MaxHealth = tPlayer.Stats[StatIds.life].Value,
                                 BreedHostility = 0x00000000,
                                 OrganizationId = orgId,
                                 FirstName = tPlayer.FirstName,
                                 LastName = tPlayer.LastName,
                                 LegacyTitle = LegacyTitle,
                                 Unknown2 = 0x0000,
                                 OrganizationRank = orgRank,
                                 TowerFields = null,
                                 CityPlayfieldId = 0x00000000,
                                 Towers = null,
                                 InvadersKilled = tPlayer.Stats[StatIds.invaderskilled].Value,
                                 KilledByInvaders = tPlayer.Stats[StatIds.killedbyinvaders].Value,
                                 AiLevel = tPlayer.Stats[StatIds.alienlevel].Value,
                                 PvpDuelWins = tPlayer.Stats[StatIds.pvpduelkills].Value,
                                 PvpDuelLoses = tPlayer.Stats[StatIds.pvpdueldeaths].Value,
                                 PvpProfessionDuelLoses =
                                     tPlayer.Stats[StatIds.pvpprofessiondueldeaths].Value,
                                 PvpSoloKills = tPlayer.Stats[StatIds.pvprankedsolokills].Value,
                                 PvpTeamKills = tPlayer.Stats[StatIds.pvprankedteamkills].Value,
                                 PvpSoloScore = tPlayer.Stats[StatIds.pvpsoloscore].Value,
                                 PvpTeamScore = tPlayer.Stats[StatIds.pvpteamscore].Value,
                                 PvpDuelScore = tPlayer.Stats[StatIds.pvpduelscore].Value
                             };
                }

                x.Type = type;
                x.Unknown = 0;
                x.Identity = tPlayer.Identity;
            };
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="identity">
        /// </param>
        internal void Send(ICharacter character, Identity identity)
        {
            // Only for Characters now
            // Need more info whether to send for non Characters too
            var obj = Pool.Instance.GetObject(identity) as ICharacter;

            if (obj != null)
            {
                if (obj.Stats[StatIds.npcfamily].Value != 0)
                {
                    // !=0 -> Monster
                    this.Send(character, obj);
                }
            }
        }
    }
}