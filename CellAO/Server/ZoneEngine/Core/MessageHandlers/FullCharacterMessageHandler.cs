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
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Core.Network;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class FullCharacterMessageHandler : BaseMessageHandler<FullCharacterMessage, FullCharacterMessageHandler>
    {
        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public void Send(ICharacter character)
        {
            this.Send(character, character);
        }

        /// <summary>
        /// </summary>
        /// <param name="dataProvider">
        /// </param>
        /// <param name="receiver">
        /// </param>
        public void Send(ICharacter dataProvider, ICharacter receiver)
        {
            this.Send(receiver, Filler(dataProvider));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        private static MessageDataFiller Filler(ICharacter character)
        {
            return fullCharacterMessage =>
            {
                /* part 1 of data */
                List<InventorySlot> inventory = new List<InventorySlot>();
                foreach (IInventoryPage ivp in character.BaseInventory.Pages.Values)
                {
                    foreach (KeyValuePair<int, IItem> kv in ivp.List())
                    {
                        var temp = new InventorySlot
                                   {
                                       Placement = kv.Key,
                                       Flags = (short)kv.Value.Flags,
                                       Count = (short)kv.Value.MultipleCount,
                                       Identity = kv.Value.Identity,
                                       ItemLowId = kv.Value.LowID,
                                       ItemHighId = kv.Value.HighID,
                                       Quality = kv.Value.Quality,
                                       Unknown = kv.Value.Nothing
                                   };
                        inventory.Add(temp);
                    }
                }

                fullCharacterMessage.Identity = character.Identity;
                fullCharacterMessage.MsgVersion = 25;
                fullCharacterMessage.InventorySlots = inventory.ToArray();

                /* part 2 of data */
                /* number of entries */
                fullCharacterMessage.UploadedNanoIds = character.UploadedNanos.Select(n => n.NanoId).ToArray();

                /* part 3 of data */
                /* number of entries */
                fullCharacterMessage.Unknown2 = new FullCharacterSub[0];

                /* No idea what these are */
                /* used to be skill locks + some unknown data */

                // TODO: Find out what following 6 ints are
                fullCharacterMessage.Unknown3 = 1;
                fullCharacterMessage.UnknownI1 = 0;
                fullCharacterMessage.Unknown4 = new FullCharacterSub2[0];
                fullCharacterMessage.UnknownI2 = 0;
                fullCharacterMessage.Unknown5 = new FullCharacterSub2[0];
                fullCharacterMessage.UnknownI3 = 0;
                fullCharacterMessage.Unknown6 = new FullCharacterSub2[0];

                IZoneClient client = character.Controller.Client;

                /* part 6 of data (1-st stats block) */

                /* Int32 stat number
                   Int32 stat value */
                var statGroup1 = new List<GameTuple<int, uint>>();

                /* State */
                AddStat3232(client, statGroup1, 7);

                /* UnarmedTemplateInstance */
                AddStat3232(client, statGroup1, 418);

                /* Invaders Killed */
                AddStat3232(client, statGroup1, 615);

                /* KilledByInvaders */
                AddStat3232(client, statGroup1, 616);

                /* AccountFlags */
                AddStat3232(client, statGroup1, 660);

                /* VP */
                AddStat3232(client, statGroup1, 669);

                /* UnsavedXP */
                AddStat3232(client, statGroup1, 592);

                /* NanoFocusLevel */
                AddStat3232(client, statGroup1, 355);

                /* Specialization */
                AddStat3232(client, statGroup1, 182);

                /* ShadowBreedTemplate */
                AddStat3232(client, statGroup1, 579);

                /* ShadowBreed */
                AddStat3232(client, statGroup1, 532);

                /* LastPerkResetTime */
                AddStat3232(client, statGroup1, 577);

                /* SocialStatus */
                AddStat3232(client, statGroup1, 521);

                /* PlayerOptions */
                AddStat3232(client, statGroup1, 576);

                /* TempSaveTeamID */
                AddStat3232(client, statGroup1, 594);

                /* TempSavePlayfield */
                AddStat3232(client, statGroup1, 595);

                /* TempSaveX */
                AddStat3232(client, statGroup1, 596);

                /* TempSaveY */
                AddStat3232(client, statGroup1, 597);

                /* VisualFlags */
                AddStat3232(client, statGroup1, 673);

                /* PvPDuelKills */
                AddStat3232(client, statGroup1, 674);

                /* PvPDuelDeaths */
                AddStat3232(client, statGroup1, 675);

                /* PvPProfessionDuelKills */
                AddStat3232(client, statGroup1, 676);

                /* PvPProfessionDuelDeaths */
                AddStat3232(client, statGroup1, 677);

                /* PvPRankedSoloKills */
                AddStat3232(client, statGroup1, 678);

                /* PvPRankedSoloDeaths */
                AddStat3232(client, statGroup1, 679);

                /* PvPRankedTeamKills */
                AddStat3232(client, statGroup1, 680);

                /* PvPRankedTeamDeaths */
                AddStat3232(client, statGroup1, 681);

                /* PvPSoloScore */
                AddStat3232(client, statGroup1, 682);

                /* PvPTeamScore */
                AddStat3232(client, statGroup1, 683);

                /* PvPDuelScore */
                AddStat3232(client, statGroup1, 684);

                AddStat3232(client, statGroup1, 0x289);
                AddStat3232(client, statGroup1, 0x28a);

                /* SavedXP */
                AddStat3232(client, statGroup1, 334);

                /* Flags */
                AddStat3232(client, statGroup1, 0);

                /* Features */
                AddStat3232(client, statGroup1, 224);

                /* ApartmentsAllowed */
                AddStat3232(client, statGroup1, 582);

                /* ApartmentsOwned */
                AddStat3232(client, statGroup1, 583);

                /* MonsterScale */
                AddStat3232(client, statGroup1, 360);

                /* VisualProfession */
                AddStat3232(client, statGroup1, 368);

                /* NanoAC */
                AddStat3232(client, statGroup1, 168);

                AddStat3232(client, statGroup1, 214);
                AddStat3232(client, statGroup1, 221);

                /* LastConcretePlayfieldInstance */
                AddStat3232(client, statGroup1, 191);

                /* MapOptions */
                AddStat3232(client, statGroup1, 470);

                /* MapAreaPart1 */
                AddStat3232(client, statGroup1, 471);

                /* MapAreaPart2 */
                AddStat3232(client, statGroup1, 472);

                /* MapAreaPart3 */
                AddStat3232(client, statGroup1, 585);

                /* MapAreaPart4 */
                AddStat3232(client, statGroup1, 586);

                /* MissionBits1 */
                AddStat3232(client, statGroup1, 256);

                /* MissionBits2 */
                AddStat3232(client, statGroup1, 257);

                /* MissionBits3 */
                AddStat3232(client, statGroup1, 303);

                /* MissionBits4 */
                AddStat3232(client, statGroup1, 432);

                /* MissionBits5 */
                AddStat3232(client, statGroup1, 65);

                /* MissionBits6 */
                AddStat3232(client, statGroup1, 66);

                /* MissionBits7 */
                AddStat3232(client, statGroup1, 67);

                /* MissionBits8 */
                AddStat3232(client, statGroup1, 544);

                /* MissionBits9 */
                AddStat3232(client, statGroup1, 545);

                /* MissionBits10 */
                AddStat3232(client, statGroup1, 617);
                AddStat3232(client, statGroup1, 618);
                AddStat3232(client, statGroup1, 619);
                AddStat3232(client, statGroup1, 198);

                /* AutoAttackFlags */
                AddStat3232(client, statGroup1, 349);

                /* PersonalResearchLevel */
                AddStat3232(client, statGroup1, 263);

                /* GlobalResearchLevel */
                AddStat3232(client, statGroup1, 264);

                /* PersonalResearchGoal */
                AddStat3232(client, statGroup1, 265);

                /* GlobalResearchGoal */
                AddStat3232(client, statGroup1, 266);

                /* BattlestationSide */
                AddStat3232(client, statGroup1, 668);

                /* BattlestationRep */
                AddStat3232(client, statGroup1, 670);

                /* Members */
                AddStat3232(client, statGroup1, 300);

                /* Int32 stat number
                   Int32 stat value */
                var statGroup2 = new List<GameTuple<int, uint>>();

                /* VeteranPoints */
                AddStat3232(client, statGroup2, 68);

                /* MonthsPaid */
                AddStat3232(client, statGroup2, 69);

                /* PaidPoints */
                AddStat3232(client, statGroup2, 672);

                /* AutoAttackFlags */
                AddStat3232(client, statGroup2, 349);

                /* XPKillRange */
                AddStat3232(client, statGroup2, 275);

                /* InPlay */
                AddStat3232(client, statGroup2, 194);

                /* Health (current health)*/
                AddStat3232(client, statGroup2, 27);

                /* Life (max health)*/
                AddStat3232(client, statGroup2, 1);

                /* Psychic */
                AddStat3232(client, statGroup2, 21);

                /* Sense */
                AddStat3232(client, statGroup2, 20);

                /* Intelligence */
                AddStat3232(client, statGroup2, 19);

                /* Stamina */
                AddStat3232(client, statGroup2, 18);

                /* Agility */
                AddStat3232(client, statGroup2, 17);

                /* Strength */
                AddStat3232(client, statGroup2, 16);

                /* Attitude */
                AddStat3232(client, statGroup2, 63);

                /* Alignment (Clan Tokens) */
                AddStat3232(client, statGroup2, 62);

                /* Cash */
                AddStat3232(client, statGroup2, 61);

                /* Profession */
                AddStat3232(client, statGroup2, 60);

                /* AggDef */
                AddStat3232(client, statGroup2, 51);

                /* Icon */
                AddStat3232(client, statGroup2, 79);

                /* Mesh */
                AddStat3232(client, statGroup2, 12);

                /* RunSpeed */
                AddStat3232(client, statGroup2, 156);

                /* DeadTimer */
                AddStat3232(client, statGroup2, 34);

                /* Team */
                AddStat3232(client, statGroup2, 6);

                /* Breed */
                AddStat3232(client, statGroup2, 4);

                /* Sex */
                AddStat3232(client, statGroup2, 59);

                /* LastSaveXP */
                AddStat3232(client, statGroup2, 372);

                /* NextXP */
                AddStat3232(client, statGroup2, 350);

                /* LastXP */
                AddStat3232(client, statGroup2, 57);

                /* Level */
                AddStat3232(client, statGroup2, 54);

                /* XP */
                AddStat3232(client, statGroup2, 52);

                /* IP */
                AddStat3232(client, statGroup2, 53);

                /* CurrentMass */
                AddStat3232(client, statGroup2, 78);

                /* ItemType */
                AddStat3232(client, statGroup2, 72);

                /* PreviousHealth */
                AddStat3232(client, statGroup2, 11);

                /* CurrentState */
                AddStat3232(client, statGroup2, 423);

                /* Age */
                AddStat3232(client, statGroup2, 58);

                /* Side */
                AddStat3232(client, statGroup2, 33);

                /* WaitState */
                AddStat3232(client, statGroup2, 430);

                /* DriveWater */
                AddStat3232(client, statGroup2, 117);

                /* MeleeMultiple */
                AddStat3232(client, statGroup2, 101);

                /* LR_MultipleWeapon */
                AddStat3232(client, statGroup2, 134);

                /* LR_EnergyWeapon */
                AddStat3232(client, statGroup2, 133);

                /* RadiationAC */
                AddStat3232(client, statGroup2, 94);

                /* SenseImprovement */
                AddStat3232(client, statGroup2, 122);

                /* BowSpecialAttack */
                AddStat3232(client, statGroup2, 121);

                /* Burst */
                AddStat3232(client, statGroup2, 148);

                /* FullAuto */
                AddStat3232(client, statGroup2, 167);

                /* MapNavigation */
                AddStat3232(client, statGroup2, 140);

                /* DriveAir */
                AddStat3232(client, statGroup2, 139);

                /* DriveGround */
                AddStat3232(client, statGroup2, 166);

                /* BreakingEntry */
                AddStat3232(client, statGroup2, 165);

                /* Concealment */
                AddStat3232(client, statGroup2, 164);

                /* Chemistry */
                AddStat3232(client, statGroup2, 163);

                /* Psychology */
                AddStat3232(client, statGroup2, 162);

                /* ComputerLiteracy */
                AddStat3232(client, statGroup2, 161);

                /* NanoProgramming */
                AddStat3232(client, statGroup2, 160);

                /* Pharmaceuticals */
                AddStat3232(client, statGroup2, 159);

                /* WeaponSmithing */
                AddStat3232(client, statGroup2, 158);

                /* FieldQuantumPhysics */
                AddStat3232(client, statGroup2, 157);

                /* AttackSpeed */
                AddStat3232(client, statGroup2, 3);

                /* Evade */
                AddStat3232(client, statGroup2, 155);

                /* Dodge */
                AddStat3232(client, statGroup2, 154);

                /* Duck */
                AddStat3232(client, statGroup2, 153);

                /* BodyDevelopment */
                AddStat3232(client, statGroup2, 152);

                /* AimedShot */
                AddStat3232(client, statGroup2, 151);

                /* FlingShot */
                AddStat3232(client, statGroup2, 150);

                /* NanoProwessInitiative */
                AddStat3232(client, statGroup2, 149);

                /* FastAttack */
                AddStat3232(client, statGroup2, 147);

                /* SneakAttack */
                AddStat3232(client, statGroup2, 146);

                /* Parry */
                AddStat3232(client, statGroup2, 145);

                /* Dimach */
                AddStat3232(client, statGroup2, 144);

                /* Riposte */
                AddStat3232(client, statGroup2, 143);

                /* Brawl */
                AddStat3232(client, statGroup2, 142);

                /* Tutoring */
                AddStat3232(client, statGroup2, 141);

                /* Swim */
                AddStat3232(client, statGroup2, 138);

                /* Adventuring */
                AddStat3232(client, statGroup2, 137);

                /* Perception */
                AddStat3232(client, statGroup2, 136);

                /* DisarmTraps */
                AddStat3232(client, statGroup2, 135);

                /* NanoEnergyPool */
                AddStat3232(client, statGroup2, 132);

                /* MaterialLocation */
                AddStat3232(client, statGroup2, 131);

                /* MaterialCreation */
                AddStat3232(client, statGroup2, 130);

                /* PsychologicalModification */
                AddStat3232(client, statGroup2, 129);

                /* BiologicalMetamorphose */
                AddStat3232(client, statGroup2, 128);

                /* MaterialMetamorphose */
                AddStat3232(client, statGroup2, 127);

                /* ElectricalEngineering */
                AddStat3232(client, statGroup2, 126);

                /* MechanicalEngineering */
                AddStat3232(client, statGroup2, 125);

                /* Treatment */
                AddStat3232(client, statGroup2, 124);

                /* FirstAid */
                AddStat3232(client, statGroup2, 123);

                /* PhysicalProwessInitiative */
                AddStat3232(client, statGroup2, 120);

                /* DistanceWeaponInitiative */
                AddStat3232(client, statGroup2, 119);

                /* CloseCombatInitiative */
                AddStat3232(client, statGroup2, 118);

                /* AssaultRifle */
                AddStat3232(client, statGroup2, 116);

                /* Shotgun */
                AddStat3232(client, statGroup2, 115);

                /* SubMachineGun */
                AddStat3232(client, statGroup2, 114);

                /* Rifle */
                AddStat3232(client, statGroup2, 113);

                /* Pistol */
                AddStat3232(client, statGroup2, 112);

                /* Bow */
                AddStat3232(client, statGroup2, 111);

                /* ThrownGrapplingWeapons */
                AddStat3232(client, statGroup2, 110);

                /* Grenade */
                AddStat3232(client, statGroup2, 109);

                /* ThrowingKnife */
                AddStat3232(client, statGroup2, 108);

                /* 2HBluntWeapons */
                AddStat3232(client, statGroup2, 107);

                /* Piercing */
                AddStat3232(client, statGroup2, 106);

                /* 2HEdgedWeapons */
                AddStat3232(client, statGroup2, 105);

                /* MeleeEnergyWeapon */
                AddStat3232(client, statGroup2, 104);

                /* 1HEdgedWeapons */
                AddStat3232(client, statGroup2, 103);

                /* 1HBluntWeapons */
                AddStat3232(client, statGroup2, 102);

                /* MartialArts */
                AddStat3232(client, statGroup2, 100);

                /* Alignment (Clan Tokens) */
                AddStat3232(client, statGroup2, 62);

                /* MetaType (Omni Tokens) */
                AddStat3232(client, statGroup2, 75);

                /* TitleLevel */
                AddStat3232(client, statGroup2, 37);

                /* GmLevel */
                AddStat3232(client, statGroup2, 215);

                /* FireAC */
                AddStat3232(client, statGroup2, 97);

                /* PoisonAC */
                AddStat3232(client, statGroup2, 96);

                /* ColdAC */
                AddStat3232(client, statGroup2, 95);

                /* RadiationAC */
                AddStat3232(client, statGroup2, 94);

                /* ChemicalAC */
                AddStat3232(client, statGroup2, 93);

                /* EnergyAC */
                AddStat3232(client, statGroup2, 92);

                /* MeleeAC */
                AddStat3232(client, statGroup2, 91);

                /* ProjectileAC */
                AddStat3232(client, statGroup2, 90);

                /* RP */
                AddStat3232(client, statGroup2, 199);

                /* SpecialCondition */
                AddStat3232(client, statGroup2, 348);

                /* SK */
                AddStat3232(client, statGroup2, 573);

                /* Expansions */
                AddStat3232(client, statGroup2, 389);

                /* ClanRedeemed */
                AddStat3232(client, statGroup2, 572);

                /* ClanConserver */
                AddStat3232(client, statGroup2, 571);

                /* ClanDevoted */
                AddStat3232(client, statGroup2, 570);

                /* OTUnredeemed */
                AddStat3232(client, statGroup2, 569);

                /* OTOperator */
                AddStat3232(client, statGroup2, 568);

                /* OTFollowers */
                AddStat3232(client, statGroup2, 567);

                /* GOS */
                AddStat3232(client, statGroup2, 566);

                /* ClanVanguards */
                AddStat3232(client, statGroup2, 565);

                /* OTTrans */
                AddStat3232(client, statGroup2, 564);

                /* ClanGaia */
                AddStat3232(client, statGroup2, 563);

                /* OTMed*/
                AddStat3232(client, statGroup2, 562);

                /* ClanSentinels */
                AddStat3232(client, statGroup2, 561);

                /* OTArmedForces */
                AddStat3232(client, statGroup2, 560);

                /* SocialStatus */
                AddStat3232(client, statGroup2, 521);

                /* PlayerID */
                AddStat3232(client, statGroup2, 607);

                /* KilledByInvaders */
                AddStat3232(client, statGroup2, 616);

                /* InvadersKilled */
                AddStat3232(client, statGroup2, 615);

                /* AlienLevel */
                AddStat3232(client, statGroup2, 169);

                /* AlienNextXP */
                AddStat3232(client, statGroup2, 178);

                /* AlienXP */
                AddStat3232(client, statGroup2, 40);

                /* Byte stat number
               Byte stat value */
                var statGroup3 = new List<GameTuple<byte, byte>>();

                /* InsurancePercentage */
                AddStat88(client, statGroup3, 236);

                /* ProfessionLevel */
                AddStat88(client, statGroup3, 10);

                /* PrevMovementMode */
                AddStat88(client, statGroup3, 174);

                /* CurrentMovementMode */
                AddStat88(client, statGroup3, 173);

                /* Fatness */
                AddStat88(client, statGroup3, 47);

                /* Race */
                AddStat88(client, statGroup3, 89);

                /* TeamSide */
                AddStat88(client, statGroup3, 213);

                /* BeltSlots */
                AddStat88(client, statGroup3, 45);

                /* Byte stat number
               Int16 (short) stat value */
                var statGroup4 = new List<GameTuple<byte, short>>();

                /* AbsorbProjectileAC */
                AddStat816(client, statGroup4, 238);

                /* AbsorbMeleeAC */
                AddStat816(client, statGroup4, 239);

                /* AbsorbEnergyAC */
                AddStat816(client, statGroup4, 240);

                /* AbsorbChemicalAC */
                AddStat816(client, statGroup4, 241);

                /* AbsorbRadiationAC */
                AddStat816(client, statGroup4, 242);

                /* AbsorbColdAC */
                AddStat816(client, statGroup4, 243);

                /* AbsorbNanoAC */
                AddStat816(client, statGroup4, 246);

                /* AbsorbFireAC */
                AddStat816(client, statGroup4, 244);

                /* AbsorbPoisonAC */
                AddStat816(client, statGroup4, 245);

                /* TemporarySkillReduction */
                AddStat816(client, statGroup4, 247);

                /* InsuranceTime */
                AddStat816(client, statGroup4, 49);

                /* CurrentNano */
                AddStat816(client, statGroup4, 214);

                /* maxMaxNanoEnergyEnergy */
                AddStat816(client, statGroup4, 221);

                /* MaxNCU */
                AddStat816(client, statGroup4, 181);

                /* MapFlags */
                AddStat816(client, statGroup4, 9);

                /* ChangeSideCount */
                AddStat816(client, statGroup4, 237);

                /* Attach stat groups to FullCharacterMessage */

                fullCharacterMessage.Stats1 = statGroup1.ToArray();
                fullCharacterMessage.Stats2 = statGroup2.ToArray();
                fullCharacterMessage.Stats3 = statGroup3.ToArray();
                fullCharacterMessage.Stats4 = statGroup4.ToArray();

                /* Unknown fields to be populated later if someone finds out what this is */

                fullCharacterMessage.Unknown9 = 0;

                fullCharacterMessage.Unknown10 = 0;

                fullCharacterMessage.Unknown11 = new object[0];

                fullCharacterMessage.Unknown12 = new object[0];

                fullCharacterMessage.Unknown13 = new object[0];
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="list">
        /// </param>
        /// <param name="statId">
        /// </param>
        private static void AddStat3232(IZoneClient client, IList<GameTuple<int, uint>> list, int statId)
        {
            var tuple = new GameTuple<int, uint>
                        {
                            Value1 = statId,
                            Value2 = client.Controller.Character.Stats[statId].BaseValue
                        };

            list.Add(tuple);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="list">
        /// </param>
        /// <param name="statId">
        /// </param>
        private static void AddStat816(IZoneClient client, IList<GameTuple<byte, short>> list, int statId)
        {
            if (statId > 255)
            {
                Console.WriteLine("AddStat816 statId(" + statId + ") > 255");
            }

            var tuple = new GameTuple<byte, short>
                        {
                            Value1 = (byte)statId,
                            Value2 = (short)client.Controller.Character.Stats[statId].BaseValue
                        };

            list.Add(tuple);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="list">
        /// </param>
        /// <param name="statId">
        /// </param>
        private static void AddStat88(IZoneClient client, IList<GameTuple<byte, byte>> list, int statId)
        {
            if (statId > 255)
            {
                Console.WriteLine("AddStat88 statId(" + statId + ") > 255");
            }

            var tuple = new GameTuple<byte, byte>
                        {
                            Value1 = (byte)statId,
                            Value2 = (byte)client.Controller.Character.Stats[statId].BaseValue
                        };

            list.Add(tuple);
        }

        #endregion
    }
}