#region License

// Copyright (c) 2005-2013, CellAO Team
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

namespace ZoneEngine.Core.Packets
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Inventory;
    using CellAO.Core.Items;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    public static class FullCharacter
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        public static void Send(ZoneClient client)
        {
            var fc = new FullCharacterMessage { Identity = client.Character.Identity, Unknown1 = 25 };

            /* part 1 of data */
            List<InventorySlot> inventory = new List<InventorySlot>();
            foreach (IInventoryPage ivp in client.Character.BaseInventory.Pages.Values)
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

            fc.InventorySlots = inventory.ToArray();

            /* part 2 of data */
            /* number of entries */
            fc.UploadedNanoIds = client.Character.UploadedNanos.Select(n => n.NanoId).ToArray();

            /* part 3 of data */
            /* number of entries */
            fc.Unknown2 = new object[0];

            /* No idea what these are */
            /* used to be skill locks + some unknown data */

            // TODO: Find out what following 6 ints are
            fc.Unknown3 = 1;
            fc.Unknown4 = 0;
            fc.Unknown5 = 1;
            fc.Unknown6 = 0;
            fc.Unknown7 = 1;
            fc.Unknown8 = 0;

            /* part 6 of data (1-st stats block) */

            /* Int32 stat number
               Int32 stat value */
            var stats1 = new List<GameTuple<int, uint>>();

            /* State */
            AddStat3232(client, stats1, 7);

            /* UnarmedTemplateInstance */
            AddStat3232(client, stats1, 418);

            /* Invaders Killed */
            AddStat3232(client, stats1, 615);

            /* KilledByInvaders */
            AddStat3232(client, stats1, 616);

            /* AccountFlags */
            AddStat3232(client, stats1, 660);

            /* VP */
            AddStat3232(client, stats1, 669);

            /* UnsavedXP */
            AddStat3232(client, stats1, 592);

            /* NanoFocusLevel */
            AddStat3232(client, stats1, 355);

            /* Specialization */
            AddStat3232(client, stats1, 182);

            /* ShadowBreedTemplate */
            AddStat3232(client, stats1, 579);

            /* ShadowBreed */
            AddStat3232(client, stats1, 532);

            /* LastPerkResetTime */
            AddStat3232(client, stats1, 577);

            /* SocialStatus */
            AddStat3232(client, stats1, 521);

            /* PlayerOptions */
            AddStat3232(client, stats1, 576);

            /* TempSaveTeamID */
            AddStat3232(client, stats1, 594);

            /* TempSavePlayfield */
            AddStat3232(client, stats1, 595);

            /* TempSaveX */
            AddStat3232(client, stats1, 596);

            /* TempSaveY */
            AddStat3232(client, stats1, 597);

            /* VisualFlags */
            AddStat3232(client, stats1, 673);

            /* PvPDuelKills */
            AddStat3232(client, stats1, 674);

            /* PvPDuelDeaths */
            AddStat3232(client, stats1, 675);

            /* PvPProfessionDuelKills */
            AddStat3232(client, stats1, 676);

            /* PvPProfessionDuelDeaths */
            AddStat3232(client, stats1, 677);

            /* PvPRankedSoloKills */
            AddStat3232(client, stats1, 678);

            /* PvPRankedSoloDeaths */
            AddStat3232(client, stats1, 679);

            /* PvPRankedTeamKills */
            AddStat3232(client, stats1, 680);

            /* PvPRankedTeamDeaths */
            AddStat3232(client, stats1, 681);

            /* PvPSoloScore */
            AddStat3232(client, stats1, 682);

            /* PvPTeamScore */
            AddStat3232(client, stats1, 683);

            /* PvPDuelScore */
            AddStat3232(client, stats1, 684);

            AddStat3232(client, stats1, 0x289);
            AddStat3232(client, stats1, 0x28a);

            /* SavedXP */
            AddStat3232(client, stats1, 334);

            /* Flags */
            AddStat3232(client, stats1, 0);

            /* Features */
            AddStat3232(client, stats1, 224);

            /* ApartmentsAllowed */
            AddStat3232(client, stats1, 582);

            /* ApartmentsOwned */
            AddStat3232(client, stats1, 583);

            /* MonsterScale */
            AddStat3232(client, stats1, 360);

            /* VisualProfession */
            AddStat3232(client, stats1, 368);

            /* NanoAC */
            AddStat3232(client, stats1, 168);

            AddStat3232(client, stats1, 214);
            AddStat3232(client, stats1, 221);

            /* LastConcretePlayfieldInstance */
            AddStat3232(client, stats1, 191);

            /* MapOptions */
            AddStat3232(client, stats1, 470);

            /* MapAreaPart1 */
            AddStat3232(client, stats1, 471);

            /* MapAreaPart2 */
            AddStat3232(client, stats1, 472);

            /* MapAreaPart3 */
            AddStat3232(client, stats1, 585);

            /* MapAreaPart4 */
            AddStat3232(client, stats1, 586);

            /* MissionBits1 */
            AddStat3232(client, stats1, 256);

            /* MissionBits2 */
            AddStat3232(client, stats1, 257);

            /* MissionBits3 */
            AddStat3232(client, stats1, 303);

            /* MissionBits4 */
            AddStat3232(client, stats1, 432);

            /* MissionBits5 */
            AddStat3232(client, stats1, 65);

            /* MissionBits6 */
            AddStat3232(client, stats1, 66);

            /* MissionBits7 */
            AddStat3232(client, stats1, 67);

            /* MissionBits8 */
            AddStat3232(client, stats1, 544);

            /* MissionBits9 */
            AddStat3232(client, stats1, 545);

            /* MissionBits10 */
            AddStat3232(client, stats1, 617);
            AddStat3232(client, stats1, 618);
            AddStat3232(client, stats1, 619);
            AddStat3232(client, stats1, 198);

            /* AutoAttackFlags */
            AddStat3232(client, stats1, 349);

            /* PersonalResearchLevel */
            AddStat3232(client, stats1, 263);

            /* GlobalResearchLevel */
            AddStat3232(client, stats1, 264);

            /* PersonalResearchGoal */
            AddStat3232(client, stats1, 265);

            /* GlobalResearchGoal */
            AddStat3232(client, stats1, 266);

            /* BattlestationSide */
            AddStat3232(client, stats1, 668);

            /* BattlestationRep */
            AddStat3232(client, stats1, 670);

            /* Members */
            AddStat3232(client, stats1, 300);

            fc.Stats1 = stats1.ToArray();

            /* Int32 stat number
               Int32 stat value */
            var stats2 = new List<GameTuple<int, uint>>();

            /* VeteranPoints */
            AddStat3232(client, stats2, 68);

            /* MonthsPaid */
            AddStat3232(client, stats2, 69);

            /* PaidPoints */
            AddStat3232(client, stats2, 672);

            /* AutoAttackFlags */
            AddStat3232(client, stats2, 349);

            /* XPKillRange */
            AddStat3232(client, stats2, 275);

            /* InPlay */
            AddStat3232(client, stats2, 194);

            /* Health (current health)*/
            AddStat3232(client, stats2, 27);

            /* Life (max health)*/
            AddStat3232(client, stats2, 1);

            /* Psychic */
            AddStat3232(client, stats2, 21);

            /* Sense */
            AddStat3232(client, stats2, 20);

            /* Intelligence */
            AddStat3232(client, stats2, 19);

            /* Stamina */
            AddStat3232(client, stats2, 18);

            /* Agility */
            AddStat3232(client, stats2, 17);

            /* Strength */
            AddStat3232(client, stats2, 16);

            /* Attitude */
            AddStat3232(client, stats2, 63);

            /* Alignment (Clan Tokens) */
            AddStat3232(client, stats2, 62);

            /* Cash */
            AddStat3232(client, stats2, 61);

            /* Profession */
            AddStat3232(client, stats2, 60);

            /* AggDef */
            AddStat3232(client, stats2, 51);

            /* Icon */
            AddStat3232(client, stats2, 79);

            /* Mesh */
            AddStat3232(client, stats2, 12);

            /* RunSpeed */
            AddStat3232(client, stats2, 156);

            /* DeadTimer */
            AddStat3232(client, stats2, 34);

            /* Team */
            AddStat3232(client, stats2, 6);

            /* Breed */
            AddStat3232(client, stats2, 4);

            /* Sex */
            AddStat3232(client, stats2, 59);

            /* LastSaveXP */
            AddStat3232(client, stats2, 372);

            /* NextXP */
            AddStat3232(client, stats2, 350);

            /* LastXP */
            AddStat3232(client, stats2, 57);

            /* Level */
            AddStat3232(client, stats2, 54);

            /* XP */
            AddStat3232(client, stats2, 52);

            /* IP */
            AddStat3232(client, stats2, 53);

            /* CurrentMass */
            AddStat3232(client, stats2, 78);

            /* ItemType */
            AddStat3232(client, stats2, 72);

            /* PreviousHealth */
            AddStat3232(client, stats2, 11);

            /* CurrentState */
            AddStat3232(client, stats2, 423);

            /* Age */
            AddStat3232(client, stats2, 58);

            /* Side */
            AddStat3232(client, stats2, 33);

            /* WaitState */
            AddStat3232(client, stats2, 430);

            /* DriveWater */
            AddStat3232(client, stats2, 117);

            /* MeleeMultiple */
            AddStat3232(client, stats2, 101);

            /* LR_MultipleWeapon */
            AddStat3232(client, stats2, 134);

            /* LR_EnergyWeapon */
            AddStat3232(client, stats2, 133);

            /* RadiationAC */
            AddStat3232(client, stats2, 94);

            /* SenseImprovement */
            AddStat3232(client, stats2, 122);

            /* BowSpecialAttack */
            AddStat3232(client, stats2, 121);

            /* Burst */
            AddStat3232(client, stats2, 148);

            /* FullAuto */
            AddStat3232(client, stats2, 167);

            /* MapNavigation */
            AddStat3232(client, stats2, 140);

            /* DriveAir */
            AddStat3232(client, stats2, 139);

            /* DriveGround */
            AddStat3232(client, stats2, 166);

            /* BreakingEntry */
            AddStat3232(client, stats2, 165);

            /* Concealment */
            AddStat3232(client, stats2, 164);

            /* Chemistry */
            AddStat3232(client, stats2, 163);

            /* Psychology */
            AddStat3232(client, stats2, 162);

            /* ComputerLiteracy */
            AddStat3232(client, stats2, 161);

            /* NanoProgramming */
            AddStat3232(client, stats2, 160);

            /* Pharmaceuticals */
            AddStat3232(client, stats2, 159);

            /* WeaponSmithing */
            AddStat3232(client, stats2, 158);

            /* FieldQuantumPhysics */
            AddStat3232(client, stats2, 157);

            /* AttackSpeed */
            AddStat3232(client, stats2, 3);

            /* Evade */
            AddStat3232(client, stats2, 155);

            /* Dodge */
            AddStat3232(client, stats2, 154);

            /* Duck */
            AddStat3232(client, stats2, 153);

            /* BodyDevelopment */
            AddStat3232(client, stats2, 152);

            /* AimedShot */
            AddStat3232(client, stats2, 151);

            /* FlingShot */
            AddStat3232(client, stats2, 150);

            /* NanoProwessInitiative */
            AddStat3232(client, stats2, 149);

            /* FastAttack */
            AddStat3232(client, stats2, 147);

            /* SneakAttack */
            AddStat3232(client, stats2, 146);

            /* Parry */
            AddStat3232(client, stats2, 145);

            /* Dimach */
            AddStat3232(client, stats2, 144);

            /* Riposte */
            AddStat3232(client, stats2, 143);

            /* Brawl */
            AddStat3232(client, stats2, 142);

            /* Tutoring */
            AddStat3232(client, stats2, 141);

            /* Swim */
            AddStat3232(client, stats2, 138);

            /* Adventuring */
            AddStat3232(client, stats2, 137);

            /* Perception */
            AddStat3232(client, stats2, 136);

            /* DisarmTraps */
            AddStat3232(client, stats2, 135);

            /* NanoEnergyPool */
            AddStat3232(client, stats2, 132);

            /* MaterialLocation */
            AddStat3232(client, stats2, 131);

            /* MaterialCreation */
            AddStat3232(client, stats2, 130);

            /* PsychologicalModification */
            AddStat3232(client, stats2, 129);

            /* BiologicalMetamorphose */
            AddStat3232(client, stats2, 128);

            /* MaterialMetamorphose */
            AddStat3232(client, stats2, 127);

            /* ElectricalEngineering */
            AddStat3232(client, stats2, 126);

            /* MechanicalEngineering */
            AddStat3232(client, stats2, 125);

            /* Treatment */
            AddStat3232(client, stats2, 124);

            /* FirstAid */
            AddStat3232(client, stats2, 123);

            /* PhysicalProwessInitiative */
            AddStat3232(client, stats2, 120);

            /* DistanceWeaponInitiative */
            AddStat3232(client, stats2, 119);

            /* CloseCombatInitiative */
            AddStat3232(client, stats2, 118);

            /* AssaultRifle */
            AddStat3232(client, stats2, 116);

            /* Shotgun */
            AddStat3232(client, stats2, 115);

            /* SubMachineGun */
            AddStat3232(client, stats2, 114);

            /* Rifle */
            AddStat3232(client, stats2, 113);

            /* Pistol */
            AddStat3232(client, stats2, 112);

            /* Bow */
            AddStat3232(client, stats2, 111);

            /* ThrownGrapplingWeapons */
            AddStat3232(client, stats2, 110);

            /* Grenade */
            AddStat3232(client, stats2, 109);

            /* ThrowingKnife */
            AddStat3232(client, stats2, 108);

            /* 2HBluntWeapons */
            AddStat3232(client, stats2, 107);

            /* Piercing */
            AddStat3232(client, stats2, 106);

            /* 2HEdgedWeapons */
            AddStat3232(client, stats2, 105);

            /* MeleeEnergyWeapon */
            AddStat3232(client, stats2, 104);

            /* 1HEdgedWeapons */
            AddStat3232(client, stats2, 103);

            /* 1HBluntWeapons */
            AddStat3232(client, stats2, 102);

            /* MartialArts */
            AddStat3232(client, stats2, 100);

            /* Alignment (Clan Tokens) */
            AddStat3232(client, stats2, 62);

            /* MetaType (Omni Tokens) */
            AddStat3232(client, stats2, 75);

            /* TitleLevel */
            AddStat3232(client, stats2, 37);

            /* GmLevel */
            AddStat3232(client, stats2, 215);

            /* FireAC */
            AddStat3232(client, stats2, 97);

            /* PoisonAC */
            AddStat3232(client, stats2, 96);

            /* ColdAC */
            AddStat3232(client, stats2, 95);

            /* RadiationAC */
            AddStat3232(client, stats2, 94);

            /* ChemicalAC */
            AddStat3232(client, stats2, 93);

            /* EnergyAC */
            AddStat3232(client, stats2, 92);

            /* MeleeAC */
            AddStat3232(client, stats2, 91);

            /* ProjectileAC */
            AddStat3232(client, stats2, 90);

            /* RP */
            AddStat3232(client, stats2, 199);

            /* SpecialCondition */
            AddStat3232(client, stats2, 348);

            /* SK */
            AddStat3232(client, stats2, 573);

            /* Expansions */
            AddStat3232(client, stats2, 389);

            /* ClanRedeemed */
            AddStat3232(client, stats2, 572);

            /* ClanConserver */
            AddStat3232(client, stats2, 571);

            /* ClanDevoted */
            AddStat3232(client, stats2, 570);

            /* OTUnredeemed */
            AddStat3232(client, stats2, 569);

            /* OTOperator */
            AddStat3232(client, stats2, 568);

            /* OTFollowers */
            AddStat3232(client, stats2, 567);

            /* GOS */
            AddStat3232(client, stats2, 566);

            /* ClanVanguards */
            AddStat3232(client, stats2, 565);

            /* OTTrans */
            AddStat3232(client, stats2, 564);

            /* ClanGaia */
            AddStat3232(client, stats2, 563);

            /* OTMed*/
            AddStat3232(client, stats2, 562);

            /* ClanSentinels */
            AddStat3232(client, stats2, 561);

            /* OTArmedForces */
            AddStat3232(client, stats2, 560);

            /* SocialStatus */
            AddStat3232(client, stats2, 521);

            /* PlayerID */
            AddStat3232(client, stats2, 607);

            /* KilledByInvaders */
            AddStat3232(client, stats2, 616);

            /* InvadersKilled */
            AddStat3232(client, stats2, 615);

            /* AlienLevel */
            AddStat3232(client, stats2, 169);

            /* AlienNextXP */
            AddStat3232(client, stats2, 178);

            /* AlienXP */
            AddStat3232(client, stats2, 40);

            fc.Stats2 = stats2.ToArray();

            /* Byte stat number
               Byte stat value */
            var stats3 = new List<GameTuple<byte, byte>>();

            /* InsurancePercentage */
            AddStat88(client, stats3, 236);

            /* ProfessionLevel */
            AddStat88(client, stats3, 10);

            /* PrevMovementMode */
            AddStat88(client, stats3, 174);

            /* CurrentMovementMode */
            AddStat88(client, stats3, 173);

            /* Fatness */
            AddStat88(client, stats3, 47);

            /* Race */
            AddStat88(client, stats3, 89);

            /* TeamSide */
            AddStat88(client, stats3, 213);

            /* BeltSlots */
            AddStat88(client, stats3, 45);

            fc.Stats3 = stats3.ToArray();

            /* Byte stat number
               Int16 (short) stat value */
            var stats4 = new List<GameTuple<byte, short>>();

            /* AbsorbProjectileAC */
            AddStat816(client, stats4, 238);

            /* AbsorbMeleeAC */
            AddStat816(client, stats4, 239);

            /* AbsorbEnergyAC */
            AddStat816(client, stats4, 240);

            /* AbsorbChemicalAC */
            AddStat816(client, stats4, 241);

            /* AbsorbRadiationAC */
            AddStat816(client, stats4, 242);

            /* AbsorbColdAC */
            AddStat816(client, stats4, 243);

            /* AbsorbNanoAC */
            AddStat816(client, stats4, 246);

            /* AbsorbFireAC */
            AddStat816(client, stats4, 244);

            /* AbsorbPoisonAC */
            AddStat816(client, stats4, 245);

            /* TemporarySkillReduction */
            AddStat816(client, stats4, 247);

            /* InsuranceTime */
            AddStat816(client, stats4, 49);

            /* CurrentNano */
            AddStat816(client, stats4, 214);

            /* maxMaxNanoEnergyEnergy */
            AddStat816(client, stats4, 221);

            /* MaxNCU */
            AddStat816(client, stats4, 181);

            /* MapFlags */
            AddStat816(client, stats4, 9);

            /* ChangeSideCount */
            AddStat816(client, stats4, 237);

            fc.Stats4 = stats4.ToArray();

            /* ? */
            fc.Unknown9 = 0;

            /* ? */
            fc.Unknown10 = 0;

            fc.Unknown11 = new object[0];

            fc.Unknown12 = new object[0];

            fc.Unknown13 = new object[0];

            client.SendCompressed(fc);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="list">
        /// </param>
        /// <param name="statId">
        /// </param>
        private static void AddStat3232(ZoneClient client, IList<GameTuple<int, uint>> list, int statId)
        {
            var tuple = new GameTuple<int, uint> { Value1 = statId, Value2 = client.Character.Stats[statId].BaseValue };

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
        private static void AddStat816(ZoneClient client, IList<GameTuple<byte, short>> list, int statId)
        {
            if (statId > 255)
            {
                Console.WriteLine("AddStat816 statId(" + statId + ") > 255");
            }

            var tuple = new GameTuple<byte, short>
                        {
                            Value1 = (byte)statId, 
                            Value2 = (short)client.Character.Stats[statId].BaseValue
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
        private static void AddStat88(ZoneClient client, IList<GameTuple<byte, byte>> list, int statId)
        {
            if (statId > 255)
            {
                Console.WriteLine("AddStat88 statId(" + statId + ") > 255");
            }

            var tuple = new GameTuple<byte, byte>
                        {
                            Value1 = (byte)statId, 
                            Value2 = (byte)client.Character.Stats[statId].BaseValue
                        };

            list.Add(tuple);
        }

        #endregion
    }
}