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

namespace ZoneEngine.Core.PacketHandlers
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Core.Items;
    using CellAO.Database.Dao;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public static class CharacterAction
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="packet">
        /// </param>
        /// <param name="client">
        /// </param>
        public static void Read(CharacterActionMessage packet, ZoneClient client)
        {
            var actionNum = (int)packet.Action;
            int unknown1 = packet.Unknown1;
            int args1 = packet.Parameter1;
            int args2 = packet.Parameter2;
            short unknown2 = packet.Unknown2;

            switch (actionNum)
            {
                case 19:
                {
                    // Cast nano
                    // CastNanoSpell
                    var msg = new CastNanoSpellMessage
                              {
                                  Identity = client.Character.Identity, 
                                  Unknown = 0x00, 
                                  NanoId = args2, 
                                  Target = packet.Target, 
                                  Unknown1 = 0x00000000, 
                                  Caster = client.Character.Identity
                              };

                    client.Character.Playfield.Announce(msg);

                    // TODO: This has to be delayed (Casting attack speed) and needs to move to some other part
                    // TODO: Check nanoskill requirements
                    // TODO: Lower current nano points/check if enough nano points

                    // CharacterAction 107
                    var characterAction107 = new CharacterActionMessage
                                             {
                                                 Identity = client.Character.Identity, 
                                                 Unknown = 0x00, 
                                                 Action = CharacterActionType.FinishNanoCasting, 
                                                 Unknown1 = 0x00000000, 
                                                 Target = Identity.None, 
                                                 Parameter1 = 1, 
                                                 Parameter2 = args2, 
                                                 Unknown2 = 0x0000
                                             };

                    client.Character.Playfield.Announce(characterAction107);

                    // CharacterAction 98
                    var characterAction98 = new CharacterActionMessage
                                            {
                                                Identity = packet.Target, 
                                                Unknown = 0x00, 
                                                Action = CharacterActionType.SetNanoDuration, 
                                                Unknown1 = 0x00000000, 
                                                Target =
                                                    new Identity
                                                    {
                                                        Type =
                                                            IdentityType
                                                            .NanoProgram, 
                                                        Instance = args2
                                                    }, 
                                                Parameter1 = client.Character.Identity.Instance, 
                                                Parameter2 = 0x249F0, // duration? 
                                                // Algorithman: Yes, it's duration
                                                Unknown2 = 0x0000
                                            };

                    client.Character.Playfield.Announce(characterAction98);
                }

                    break;

                    /* this is here to prevent server crash that is caused by
 * search action if server doesn't reply if something is
 * found or not */
                case 66:
                {
                    // If action == search
                    /* Msg 110:136744723 = "No hidden objects found." */
                    // TODO: SEARCH!!
                    SendFeedback.Send(client, 110, 136744723);
                }

                    break;

                case 105:
                {
                    // If action == Info Request
                    IInstancedEntity tPlayer = client.Playfield.FindByIdentity(packet.Target);
                    var tChar = tPlayer as Character;
                    if (tChar != null)
                    {
                        uint LegacyScore = tChar.Stats[StatIds.pvp_rating].BaseValue;
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
                            orgGoverningForm = OrganizationDao.GetGovernmentForm(tChar.Stats[StatIds.clan].Value);
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
                            if (client.Character.Stats[StatIds.clan].BaseValue == tPlayer.Stats[StatIds.clan].BaseValue)
                            {
                                orgRank = OrgClient.GetRank(
                                    orgGoverningForm, 
                                    tPlayer.Stats[StatIds.clanlevel].BaseValue);
                            }
                            else
                            {
                                orgRank = string.Empty;
                            }
                        }

                        var info = new CharacterInfoPacket
                                   {
                                       Unknown1 = 0x01, 
                                       Profession =
                                           (Profession)tPlayer.Stats[StatIds.profession].Value, 
                                       Level = (byte)tPlayer.Stats[StatIds.level].Value, 
                                       TitleLevel = (byte)tPlayer.Stats[StatIds.titlelevel].Value, 
                                       VisualProfession =
                                           (Profession)tPlayer.Stats[StatIds.visualflags].Value, 
                                       SideXp = 0, 
                                       Health = tPlayer.Stats[StatIds.health].Value, 
                                       MaxHealth = tPlayer.Stats[StatIds.life].Value, 
                                       BreedHostility = 0x00000000, 
                                       OrganizationId = orgId, 
                                       FirstName = tChar.FirstName, 
                                       LastName = tChar.LastName, 
                                       LegacyTitle = LegacyTitle, 
                                       Unknown2 = 0x0000, 
                                       OrganizationRank = orgRank, 
                                       TowerFields = null, 
                                       CityPlayfieldId = 0x00000000, 
                                       Towers = null, 
                                       InvadersKilled =
                                           tPlayer.Stats[StatIds.invaderskilled].Value, 
                                       KilledByInvaders =
                                           tPlayer.Stats[StatIds.killedbyinvaders].Value, 
                                       AiLevel = tPlayer.Stats[StatIds.alienlevel].Value, 
                                       PvpDuelWins = tPlayer.Stats[StatIds.pvpduelkills].Value, 
                                       PvpDuelLoses = tPlayer.Stats[StatIds.pvpdueldeaths].Value, 
                                       PvpProfessionDuelLoses =
                                           tPlayer.Stats[StatIds.pvpprofessiondueldeaths].Value, 
                                       PvpSoloKills =
                                           tPlayer.Stats[StatIds.pvprankedsolokills].Value, 
                                       PvpTeamKills =
                                           tPlayer.Stats[StatIds.pvprankedteamkills].Value, 
                                       PvpSoloScore = tPlayer.Stats[StatIds.pvpsoloscore].Value, 
                                       PvpTeamScore = tPlayer.Stats[StatIds.pvpteamscore].Value, 
                                       PvpDuelScore = tPlayer.Stats[StatIds.pvpduelscore].Value
                                   };

                        var infoPacketMessage = new InfoPacketMessage
                                                {
                                                    Identity = tPlayer.Identity, 
                                                    Unknown = 0x00, 
                                                    Type = type, 
                                                    Info = info
                                                };

                        client.SendCompressed(infoPacketMessage);
                    }
                    else
                    {
                        // TODO: NPC's
                        /*
                                        var npc =
                                            (NonPlayerCharacterClass)
                                            FindDynel.FindDynelById(packet.Target);
                                        if (npc != null)
                                        {
                                            var infoPacket = new PacketWriter();

                                            // Start packet header
                                            infoPacket.PushByte(0xDF);
                                            infoPacket.PushByte(0xDF);
                                            infoPacket.PushShort(10);
                                            infoPacket.PushShort(1);
                                            infoPacket.PushShort(0);
                                            infoPacket.PushInt(3086); // sender (server ID)
                                            infoPacket.PushInt(client.Character.Id.Instance); // receiver 
                                            infoPacket.PushInt(0x4D38242E); // packet ID
                                            infoPacket.PushIdentity(npc.Id); // affected identity
                                            infoPacket.PushByte(0); // ?

                                            // End packet header
                                            infoPacket.PushByte(0x50); // npc's just have 0x50
                                            infoPacket.PushByte(1); // esi_001?
                                            infoPacket.PushByte((byte)npc.Stats.Profession.Value); // Profession
                                            infoPacket.PushByte((byte)npc.Stats.Level.Value); // Level
                                            infoPacket.PushByte((byte)npc.Stats.TitleLevel.Value); // Titlelevel
                                            infoPacket.PushByte((byte)npc.Stats.VisualProfession.Value); // Visual Profession

                                            infoPacket.PushShort(0); // no idea for npc's
                                            infoPacket.PushUInt(npc.Stats.Health.Value); // Current Health (Health)
                                            infoPacket.PushUInt(npc.Stats.Life.Value); // Max Health (Life)
                                            infoPacket.PushInt(0); // BreedHostility?
                                            infoPacket.PushUInt(0); // org ID
                                            infoPacket.PushShort(0);
                                            infoPacket.PushShort(0);
                                            infoPacket.PushShort(0);
                                            infoPacket.PushShort(0);
                                            infoPacket.PushInt(0x499602d2);
                                            infoPacket.PushInt(0x499602d2);
                                            infoPacket.PushInt(0x499602d2);
                                            var infoPacketA = infoPacket.Finish();
                                            client.SendCompressed(infoPacketA);
                                        }*/
                    }
                }

                    break;

                case 120:
                {
                    // If action == Logout
                    // Start 30 second logout timer if client is not a GM (statid 215)
                    if (client.Character.Stats[StatIds.gmlevel].Value == 0)
                    {
                        ((Character)client.Character).StartLogoutTimer();
                    }
                    else
                    {
                        // If client is a GM, disconnect without timer
                        client.Character.Dispose();
                    }
                }

                    break;
                case 121:
                {
                    // If action == Stop Logout
                    // Stop current logout timer and send stop logout packet
                    ((Character)client.Character).StopLogoutTimer();
                    client.Character.UpdateMoveType((byte)client.Character.PreviousMoveMode);
                    client.Playfield.Announce(packet);

                    // client.CancelLogOut();
                }

                    break;

                case 87:
                {
                    // If action == Stand
                    client.Character.UpdateMoveType(37);
                    client.Playfield.Announce(packet);

                    if (client.Character.InLogoutTimerPeriod())
                    {
                        client.Playfield.Send(
                            client, 
                            new CharacterActionMessage()
                            {
                                Action = CharacterActionType.StopLogout, 
                                Identity = client.Character.Identity
                            });
                        ((Character)client.Character).StopLogoutTimer();
                    }

                    // Send stand up packet, and cancel timer/send stop logout packet if timer is enabled
                    // client.StandCancelLogout();
                }

                    break;

                case 22:
                {
                    // Kick Team Member
                }

                    break;
                case 24:
                {
                    // Leave Team
                    /*
                                    var team = new TeamClass();
                                    team.LeaveTeam(client);
                                     */
                }

                    break;
                case 25:
                {
                    // Transfer Team Leadership
                }

                    break;
                case 26:
                {
                    // Team Join Request
                    // Send Team Invite Request To Target Player
                    /*
                                    var team = new TeamClass();
                                    team.SendTeamRequest(client, packet.Target);
                                     */
                }

                    break;
                case 28:
                {
                    /*
                                    // Request Reply
                                    // Check if positive or negative response

                                    // if positive
                                    var team = new TeamClass();
                                    var teamID = TeamClass.GenerateNewTeamId(client, packet.Target);

                                    // Destination Client 0 = Sender, 1 = Reciever

                                    // Reciever Packets
                                    ///////////////////

                                    // CharAction 15
                                    team.TeamRequestReply(client, packet.Target);

                                    // CharAction 23
                                    team.TeamRequestReplyCharacterAction23(client, packet.Target);

                                    // TeamMember Packet
                                    team.TeamReplyPacketTeamMember(1, client, packet.Target, "Member1");

                                    // TeamMemberInfo Packet
                                    team.TeamReplyPacketTeamMemberInfo(1, client, packet.Target);

                                    // TeamMember Packet
                                    team.TeamReplyPacketTeamMember(1, client, packet.Target, "Member2");

                                    // Sender Packets
                                    /////////////////

                                    // TeamMember Packet
                                    team.TeamReplyPacketTeamMember(0, client, packet.Target, "Member1");

                                    // TeamMemberInfo Packet
                                    team.TeamReplyPacketTeamMemberInfo(0, client, packet.Target);

                                    // TeamMember Packet
                                    team.TeamReplyPacketTeamMember(0, client, packet.Target, "Member2");
                                     */
                }

                    break;

                case 0x70: // Remove/Delete item
                    ItemDao.RemoveItem(
                        (int)packet.Target.Type, 
                        client.Character.Identity.Instance, 
                        packet.Target.Instance);
                    client.Character.BaseInventory.RemoveItem((int)packet.Target.Type, packet.Target.Instance);
                    client.SendCompressed(packet);
                    break;

                case 0x34: // Split?
                    IItem it = client.Character.BaseInventory.Pages[(int)packet.Target.Type][packet.Target.Instance];
                    it.MultipleCount -= args2;
                    Item newItem = new Item(it.Quality, it.LowID, it.HighID);
                    newItem.MultipleCount = args2;

                    client.Character.BaseInventory.Pages[(int)packet.Target.Type].Add(
                        client.Character.BaseInventory.Pages[(int)packet.Target.Type].FindFreeSlot(), 
                        newItem);
                    client.Character.BaseInventory.Pages[(int)packet.Target.Type].Write();
                    break;

                case 0x35:
                    client.Character.BaseInventory.Pages[(int)packet.Target.Type][packet.Target.Instance].MultipleCount
                        += client.Character.BaseInventory.Pages[(int)packet.Target.Type][args2].MultipleCount;
                    client.Character.BaseInventory.Pages[(int)packet.Target.Type].Remove(args2);
                    client.Character.BaseInventory.Pages[(int)packet.Target.Type].Write();
                    client.SendCompressed(packet);
                    break;

                    // ###################################################################################
                    // Spandexpants: This is all i have done so far as to make sneak turn on and off, 
                    // currently i cannot find a missing packet or link which tells the server the player
                    // has stopped sneaking, hidden packet or something, will come back to later.
                    // ###################################################################################

                    // Sneak Packet Received
                case 163:
                {
                    // TODO: IF SNEAKING IS ALLOWED RUN THIS CODE.
                    // Send Action 162 : Enable Sneak
                    var sneak = new CharacterActionMessage
                                {
                                    Identity = client.Character.Identity, 
                                    Unknown = 0x00, 
                                    Action = CharacterActionType.StartedSneaking, 
                                    Unknown1 = 0x00000000, 
                                    Target = Identity.None, 
                                    Parameter1 = 0, 
                                    Parameter2 = 0, 
                                    Unknown2 = 0x0000
                                };

                    client.SendCompressed(sneak);

                    // End of Enable sneak
                    // TODO: IF SNEAKING IS NOT ALLOWED SEND REJECTION PACKET
                }

                    break;

                case 81:
                {
                    Identity item1 = packet.Target;
                    var item2 = new Identity { Type = (IdentityType)args1, Instance = args2 };

                    client.Character.TradeSkillSource = new TradeSkillInfo(0, (int)item1.Type, item1.Instance);
                    client.Character.TradeSkillTarget = new TradeSkillInfo(1, (int)item2.Type, item2.Instance);
                    TradeSkillReceiver.TradeSkillBuildPressed(client, 300);

                    break;
                }

                case 166:
                {
                    client.Character.Stats[StatIds.visualflags].Value = args2;

                    // client.SendChatText("Setting Visual Flag to "+unknown3.ToString());
                    AppearanceUpdate.AnnounceAppearanceUpdate((Character)client.Character);
                    break;
                }

                case 0xdc:
                    TradeSkillReceiver.TradeSkillSourceChanged(client, args1, args2);
                    break;

                case 0xdd:
                    TradeSkillReceiver.TradeSkillTargetChanged(client, args1, args2);
                    break;

                case 0xde:
                    TradeSkillReceiver.TradeSkillBuildPressed(client, packet.Target.Instance);
                    break;

                default:
                {
                    client.Playfield.Announce(packet);
                }

                    break;
            }
        }

        #endregion
    }
}