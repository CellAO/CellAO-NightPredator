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

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Items;
    using CellAO.Core.Network;
    using CellAO.Database.Dao;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    using ZoneEngine.Core.PacketHandlers;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.All)]
    public class CharacterActionMessageHandler :
        BaseMessageHandler<CharacterActionMessage, CharacterActionMessageHandler>
    {
        /// <summary>
        /// </summary>
        public CharacterActionMessageHandler()
        {
            this.UpdateCharacterStatsOnReceive = true;
        }

        #region Inbound

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        protected override void Read(CharacterActionMessage message, IZoneClient client)
        {
            LogUtil.Debug(DebugInfoDetail.NetworkMessages, "Reading CharacterActionMessage");

            // var actionNum = (int)characterAction.Action;
            // int unknown1 = message.Unknown1;
            // int args1 = message.Parameter1;
            // int nanoId = message.Parameter2;
            // short unknown2 = message.Unknown2;

            IdentityType targetIdentityType = message.Target.Type;

            switch (message.Action)
            {
                case CharacterActionType.CastNano:

                    // Cast nano
                    // CastNanoSpell

                    // TODO: This has to be delayed (Casting attack speed) and needs to move to some other part
                    // TODO: Check nanoskill requirements
                    // TODO: Lower current nano points/check if enough nano points

                    client.Controller.CastNano(message.Parameter2, message.Target);

                    break;

                    /* this is here to prevent server crash that is caused by search action if server doesn't reply if something is found or not */
                case CharacterActionType.Search:

                    // If action == search
                    /* Msg 110:136744723 = "No hidden objects found." */
                    // TODO: SEARCH!!
                    FeedbackMessageHandler.Default.Send(client.Controller.Character, 110, 136744723);
                    break;

                case CharacterActionType.InfoRequest:

                    // If action == Info Request
                    IInstancedEntity tPlayer = client.Controller.Character.Playfield.FindByIdentity(message.Target);

                    // TODO: Think of a new method to distinguish players from mobs (NPCFamily for example)
                    var tChar = tPlayer as Character;
                    if (tChar != null)
                    {
                        // Is it a Player?
                        CharacterInfoPacketMessageHandler.Default.Send(client.Controller.Character, tChar);
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

                                 Start packet header
                                infoPacket.PushByte(0xDF);
                                infoPacket.PushByte(0xDF);
                                infoPacket.PushShort(10);
                                infoPacket.PushShort(1);
                                infoPacket.PushShort(0);
                                infoPacket.PushInt(3086);  sender (server ID)
                                infoPacket.PushInt(client.Character.Id.Instance);  receiver 
                                infoPacket.PushInt(0x4D38242E);  packet ID
                                infoPacket.PushIdentity(npc.Id);  affected identity
                                infoPacket.PushByte(0);  ?

                                 End packet header
                                infoPacket.PushByte(0x50);  npc's just have 0x50
                                infoPacket.PushByte(1);  esi_001?
                                infoPacket.PushByte((byte)npc.Stats.Profession.Value);  Profession
                                infoPacket.PushByte((byte)npc.Stats.Level.Value);  Level
                                infoPacket.PushByte((byte)npc.Stats.TitleLevel.Value);  Titlelevel
                                infoPacket.PushByte((byte)npc.Stats.VisualProfession.Value);  Visual Profession

                                infoPacket.PushShort(0);  no idea for npc's
                                infoPacket.PushUInt(npc.Stats.Health.Value);  Current Health (Health)
                                infoPacket.PushUInt(npc.Stats.Life.Value);  Max Health (Life)
                                infoPacket.PushInt(0);  BreedHostility?
                                infoPacket.PushUInt(0);  org ID
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

                    break;

                case CharacterActionType.Logout:

                    // If action == Logout
                    // Start 30 second logout timer if client is not a GM (statid 215)
                    if (client.Controller.Character.Stats[StatIds.gmlevel].Value == 0)
                    {
                        client.Controller.Character.StartLogoutTimer();
                    }
                    else
                    {
                        // If client is a GM, disconnect without timer
                        client.Controller.Character.StartLogoutTimer(1000);
                    }

                    break;

                case CharacterActionType.StopLogout:

                    // If action == Stop Logout
                    // Stop current logout timer and send stop logout packet
                    client.Controller.Character.StopLogoutTimer();
                    client.Controller.Character.UpdateMoveType((byte)client.Controller.Character.PreviousMoveMode);
                    client.Controller.Character.Playfield.Announce(message);
                    break;

                case CharacterActionType.StandUp:
                {
                    // If action == Stand
                    client.Controller.Character.UpdateMoveType(37);
                    client.Controller.Character.Playfield.Announce(message);

                    if (client.Controller.Character.InLogoutTimerPeriod())
                    {
                        this.Send(client.Controller.Character, this.StopLogout(client.Controller.Character), true);
                        client.Controller.Character.StopLogoutTimer();
                    }

                    // Send stand up packet, and cancel timer/send stop logout packet if timer is enabled
                    // ((ZoneClient)client).StandCancelLogout();
                }

                    break;

                case CharacterActionType.TeamKickMember:
                {
                    // Kick Team Member
                }

                    break;

                case CharacterActionType.LeaveTeam:
                {
                    // Leave Team
                    /*
                                                var team = new TeamClass();
                                                team.LeaveTeam(client);
                                                 */
                }

                    break;
                case CharacterActionType.TransferLeader:
                {
                    // Transfer Team Leadership
                }

                    break;

                case CharacterActionType.TeamRequestInvite:
                {
                    // Team Join Request
                    // Send Team Invite Request To Target Player
                    /*
                                                var team = new TeamClass();
                                                team.SendTeamRequest(client, packet.Target);
                                                 */
                }

                    break;
                case CharacterActionType.TeamRequestReply:
                {
                    /*
                                                 Request Reply
                                                 Check if positive or negative response

                                                 if positive
                                                var team = new TeamClass();
                                                var teamID = TeamClass.GenerateNewTeamId(client, packet.Target);

                                                 Destination Client 0 = Sender, 1 = Reciever

                                                 Reciever Packets
                                                ///////////////////

                                                 CharAction 15
                                                team.TeamRequestReply(client, packet.Target);

                                                 CharAction 23
                                                team.TeamRequestReplyCharacterAction23(client, packet.Target);

                                                 TeamMember Packet
                                                team.TeamReplyPacketTeamMember(1, client, packet.Target, "Member1");

                                                 TeamMemberInfo Packet
                                                team.TeamReplyPacketTeamMemberInfo(1, client, packet.Target);

                                                 TeamMember Packet
                                                team.TeamReplyPacketTeamMember(1, client, packet.Target, "Member2");

                                                 Sender Packets
                                                /////////////////

                                                 TeamMember Packet
                                                team.TeamReplyPacketTeamMember(0, client, packet.Target, "Member1");

                                                 TeamMemberInfo Packet
                                                team.TeamReplyPacketTeamMemberInfo(0, client, packet.Target);

                                                 TeamMember Packet
                                                team.TeamReplyPacketTeamMember(0, client, packet.Target, "Member2");
                                                 */
                }

                    break;

                case CharacterActionType.DeleteItem: // Remove/Delete item
                    ItemDao.Instance.Delete(
                        new
                        {
                            containertype = (int)targetIdentityType,
                            containerinstance = client.Controller.Character.Identity.Instance,
                            Id = message.Target.Instance
                        });
                    client.Controller.Character.BaseInventory.RemoveItem(
                        (int)targetIdentityType,
                        message.Target.Instance);

                    this.Acknowledge(client.Controller.Character, message);
                    break;

                case CharacterActionType.Split: // Split?
                    IItem it =
                        client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType][message.Target.Instance
                            ];
                    it.MultipleCount -= message.Parameter2;
                    Item newItem = new Item(it.Quality, it.LowID, it.HighID);
                    newItem.MultipleCount = message.Parameter2;

                    client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType].Add(
                        client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType].FindFreeSlot(),
                        newItem);
                    client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType].Write();

                    // Does it need to Acknowledge? Need to check that - Algorithman
                    break;

                case CharacterActionType.AcceptTeamRequest:
                    client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType][message.Target.Instance]
                        .MultipleCount +=
                        client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType][message.Parameter2]
                            .MultipleCount;
                    client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType].Remove(message.Parameter2);
                    client.Controller.Character.BaseInventory.Pages[(int)targetIdentityType].Write();
                    this.Acknowledge(client.Controller.Character, message);
                    break;

                    // ###################################################################################
                    // Spandexpants: This is all i have done so far as to make sneak turn on and off, 
                    // currently i cannot find a missing packet or link which tells the server the player
                    // has stopped sneaking, hidden packet or something, will come back to later.
                    // ###################################################################################

                    // Sneak Packet Received
                case CharacterActionType.StartSneak:

                    // TODO: IF SNEAKING IS ALLOWED RUN THIS CODE.
                    // TODO: Insert perception checks on receiving characters/mobs and then dont send to playfield
                    // Send Action 162 : Enable Sneak

                    this.Send(client.Controller.Character, this.Sneak(client.Controller.Character), true);

                    // End of Enable sneak
                    // TODO: IF SNEAKING IS NOT ALLOWED SEND REJECTION PACKET
                    break;

                case CharacterActionType.UseItemOnItem:
                {
                    Identity item1 = message.Target;
                    var item2 = new Identity { Type = (IdentityType)message.Parameter1, Instance = message.Parameter2 };

                    client.Controller.Character.TradeSkillSource = new TradeSkillInfo(
                        0,
                        (int)item1.Type,
                        item1.Instance);
                    client.Controller.Character.TradeSkillTarget = new TradeSkillInfo(
                        1,
                        (int)item2.Type,
                        item2.Instance);
                    TradeSkillReceiver.TradeSkillBuildPressed(client, 300);

                    break;
                }

                case CharacterActionType.ChangeVisualFlag:
                {
                    client.Controller.Character.Stats[StatIds.visualflags].Value = message.Parameter2;

                    ChatTextMessageHandler.Default.Send(
                        client.Controller.Character,
                        "Setting Visual Flag to " + message.Parameter2);
                    AppearanceUpdateMessageHandler.Default.Send(client.Controller.Character);
                }

                    break;
                case CharacterActionType.TradeskillSourceChanged:
                    TradeSkillReceiver.TradeSkillSourceChanged(client, message.Parameter1, message.Parameter2);
                    break;

                case CharacterActionType.TradeskillTargetChanged:
                    TradeSkillReceiver.TradeSkillTargetChanged(client, message.Parameter1, message.Parameter2);
                    break;

                case CharacterActionType.TradeskillBuildPressed:
                    TradeSkillReceiver.TradeSkillBuildPressed(client, message.Target.Instance);
                    break;

                default:
                {
                    // unkown
                    client.Controller.Character.Playfield.Announce(message);
                }

                    break;
            }
        }

        #endregion

        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="actionType">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="unknown1">
        /// </param>
        /// <param name="unknown2">
        /// </param>
        public void FinishNanoCasting(
            ICharacter character,
            CharacterActionType actionType,
            Identity target,
            int unknown1,
            int unknown2)
        {
            this.Send(character, this.ConstructFinishNanoCasting(character, target, unknown1, unknown2), true);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="unknown1">
        /// </param>
        /// <param name="unknown2">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller ConstructFinishNanoCasting(
            ICharacter character,
            Identity target,
            int unknown1,
            int unknown2)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Unknown = 0x00;
                x.Action = CharacterActionType.FinishNanoCasting;
                x.Unknown1 = 0x00000000;
                x.Target = Identity.None;
                x.Parameter1 = unknown1;
                x.Parameter2 = unknown2;
                x.Unknown2 = 0x0000;
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="unknown1">
        /// </param>
        /// <param name="duration">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller ConstructSetNanoDuration(
            ICharacter character,
            Identity target,
            int unknown1,
            int duration = 0x249F0)
        {
            return x =>
            {
                x.Identity = target;
                x.Unknown = 0x00;
                x.Action = CharacterActionType.SetNanoDuration;
                x.Unknown1 = 0x00000000;
                x.Target = new Identity { Type = IdentityType.NanoProgram, Instance = unknown1 };
                x.Parameter1 = character.Identity.Instance;
                x.Parameter2 = duration; // duration
                x.Unknown2 = 0x0000;
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="unknown1">
        /// </param>
        /// <param name="duration">
        /// </param>
        public void SetNanoDuration(ICharacter character, Identity target, int unknown1, int duration = 0x249F0)
        {
            this.Send(character, this.ConstructSetNanoDuration(character, target, unknown1, duration));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="container">
        /// </param>
        /// <param name="placement">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller DeleteItemAction(ICharacter character, int container, int placement)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Action = CharacterActionType.DeleteItem;
                x.Target = new Identity() { Type = (IdentityType)container, Instance = placement };
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="container">
        /// </param>
        /// <param name="placement">
        /// </param>
        public void SendDeleteItem(ICharacter character, int container, int placement)
        {
            this.Send(character, this.DeleteItemAction(character, container, placement));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller Sneak(ICharacter character)
        {
            return x =>
            {
                x.Identity = character.Identity;
                x.Unknown = 0x00;
                x.Action = CharacterActionType.StartedSneaking;
                x.Unknown1 = 0x00000000;
                x.Target = Identity.None;
                x.Parameter1 = 0;
                x.Parameter2 = 0;
                x.Unknown2 = 0;
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="message">
        /// </param>
        private void Acknowledge(ICharacter character, CharacterActionMessage message)
        {
            this.Send(character, this.Reply(message));
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller Reply(CharacterActionMessage message)
        {
            return x => { x = message; };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        private MessageDataFiller StopLogout(ICharacter character)
        {
            return x =>
            {
                x.Action = CharacterActionType.StopLogout;
                x.Identity = character.Identity;
            };
        }

        #endregion
    }
}