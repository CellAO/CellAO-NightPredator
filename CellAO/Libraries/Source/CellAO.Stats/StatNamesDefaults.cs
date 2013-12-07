﻿#region License

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

namespace CellAO.Stats
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Exceptions;

    #endregion

    /// <summary>
    /// </summary>
    public static class StatNamesDefaults
    {
        #region Static Fields

        /// <summary>
        /// List Stat ID's -> Default values
        /// </summary>
        private static readonly Dictionary<int, int> Defaults = new Dictionary<int, int>();

        /// <summary>
        /// List Stat ID's -> Names
        /// </summary>
        private static readonly Dictionary<int, string> NameList = new Dictionary<int, string>();

        /// <summary>
        /// List Stat Names -> ID's
        /// </summary>
        private static readonly Dictionary<string, int> NumberList = new Dictionary<string, int>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create static list of Stat Names (replaces the XML)
        /// </summary>
        static StatNamesDefaults()
        {
            // Filling Names List
            NameList.Add(0, "Flags");
            NameList.Add(1, "Life");
            NameList.Add(2, "VolumeMass");
            NameList.Add(3, "AttackSpeed");
            NameList.Add(4, "Breed");
            NameList.Add(5, "Clan");
            NameList.Add(6, "Team");
            NameList.Add(7, "State");
            NameList.Add(8, "TimeExist");
            NameList.Add(9, "MapFlags");
            NameList.Add(10, "ProfessionLevel");
            NameList.Add(11, "PreviousHealth");
            NameList.Add(12, "Mesh");
            NameList.Add(13, "Anim");
            NameList.Add(14, "Name");
            NameList.Add(15, "Info");
            NameList.Add(16, "Strength");
            NameList.Add(17, "Agility");
            NameList.Add(18, "Stamina");
            NameList.Add(19, "Intelligence");
            NameList.Add(20, "Sense");
            NameList.Add(21, "Psychic");
            NameList.Add(22, "AMS");
            NameList.Add(23, "StaticInstance");
            NameList.Add(24, "MaxMass");
            NameList.Add(25, "StaticType");
            NameList.Add(26, "Energy");
            NameList.Add(27, "Health");
            NameList.Add(28, "Height");
            NameList.Add(29, "DMS");
            NameList.Add(30, "Can");
            NameList.Add(31, "Face");
            NameList.Add(32, "HairMesh");
            NameList.Add(33, "Side");
            NameList.Add(34, "DeadTimer");
            NameList.Add(35, "AccessCount");
            NameList.Add(36, "AttackCount");
            NameList.Add(37, "TitleLevel");
            NameList.Add(38, "BackMesh");
            NameList.Add(40, "AlienXP");
            NameList.Add(41, "FabricType");
            NameList.Add(42, "CATMesh");
            NameList.Add(43, "ParentType");
            NameList.Add(44, "ParentInstance");
            NameList.Add(45, "BeltSlots");
            NameList.Add(46, "BandolierSlots");
            NameList.Add(47, "Fatness");
            NameList.Add(48, "ClanLevel");
            NameList.Add(49, "InsuranceTime");
            NameList.Add(50, "InventoryTimeout");
            NameList.Add(51, "AggDef");
            NameList.Add(52, "XP");
            NameList.Add(53, "IP");
            NameList.Add(54, "Level");
            NameList.Add(55, "InventoryId");
            NameList.Add(56, "TimeSinceCreation");
            NameList.Add(57, "LastXP");
            NameList.Add(58, "Age");
            NameList.Add(59, "Sex");
            NameList.Add(60, "Profession");
            NameList.Add(61, "Cash");
            NameList.Add(62, "Alignment");
            NameList.Add(63, "Attitude");
            NameList.Add(64, "HeadMesh");
            NameList.Add(65, "MissionBits5");
            NameList.Add(66, "MissionBits6");
            NameList.Add(67, "MissionBits7");
            NameList.Add(68, "VeteranPoints");
            NameList.Add(69, "MonthsPaid");
            NameList.Add(70, "SpeedPenalty");
            NameList.Add(71, "TotalMass");
            NameList.Add(72, "ItemType");
            NameList.Add(73, "RepairDifficulty");
            NameList.Add(74, "Price");
            NameList.Add(75, "MetaType");
            NameList.Add(76, "ItemClass");
            NameList.Add(77, "RepairSkill");
            NameList.Add(78, "CurrentMass");
            NameList.Add(79, "Icon");
            NameList.Add(80, "PrimaryItemType");
            NameList.Add(81, "PrimaryItemInstance");
            NameList.Add(82, "SecondaryItemType");
            NameList.Add(83, "SecondaryItemInstance");
            NameList.Add(84, "UserType");
            NameList.Add(85, "UserInstance");
            NameList.Add(86, "AreaType");
            NameList.Add(87, "AreaInstance");
            NameList.Add(88, "DefaultPos");
            NameList.Add(89, "Race");
            NameList.Add(90, "ProjectileAC");
            NameList.Add(91, "MeleeAC");
            NameList.Add(92, "EnergyAC");
            NameList.Add(93, "ChemicalAC");
            NameList.Add(94, "RadiationAC");
            NameList.Add(95, "ColdAC");
            NameList.Add(96, "PoisonAC");
            NameList.Add(97, "FireAC");
            NameList.Add(98, "StateAction");
            NameList.Add(99, "ItemAnim");
            NameList.Add(100, "MartialArts");
            NameList.Add(101, "MeleeMultiple");
            NameList.Add(102, "1hBluntWeapons");
            NameList.Add(103, "1hEdgedWeapon");
            NameList.Add(104, "MeleeEnergyWeapon");
            NameList.Add(105, "2hEdgedWeapons");
            NameList.Add(106, "Piercing");
            NameList.Add(107, "2hBluntWeapons");
            NameList.Add(108, "ThrowingKnife");
            NameList.Add(109, "Grenade");
            NameList.Add(110, "ThrownGrapplingWeapons");
            NameList.Add(111, "Bow");
            NameList.Add(112, "Pistol");
            NameList.Add(113, "Rifle");
            NameList.Add(114, "SubMachineGun");
            NameList.Add(115, "Shotgun");
            NameList.Add(116, "AssaultRifle");
            NameList.Add(117, "DriveWater");
            NameList.Add(118, "CloseCombatInitiative");
            NameList.Add(119, "DistanceWeaponInitiative");
            NameList.Add(120, "PhysicalProwessInitiative");
            NameList.Add(121, "BowSpecialAttack");
            NameList.Add(122, "SenseImprovement");
            NameList.Add(123, "FirstAid");
            NameList.Add(124, "Treatment");
            NameList.Add(125, "MechanicalEngineering");
            NameList.Add(126, "ElectricalEngineering");
            NameList.Add(127, "MaterialMetamorphose");
            NameList.Add(128, "BiologicalMetamorphose");
            NameList.Add(129, "PsychologicalModification");
            NameList.Add(130, "MaterialCreation");
            NameList.Add(131, "MaterialLocation");
            NameList.Add(132, "NanoEnergyPool");
            NameList.Add(133, "LR_EnergyWeapon");
            NameList.Add(134, "LR_MultipleWeapon");
            NameList.Add(135, "DisarmTrap");
            NameList.Add(136, "Perception");
            NameList.Add(137, "Adventuring");
            NameList.Add(138, "Swim");
            NameList.Add(139, "DriveAir");
            NameList.Add(140, "MapNavigation");
            NameList.Add(141, "Tutoring");
            NameList.Add(142, "Brawl");
            NameList.Add(143, "Riposte");
            NameList.Add(144, "Dimach");
            NameList.Add(145, "Parry");
            NameList.Add(146, "SneakAttack");
            NameList.Add(147, "FastAttack");
            NameList.Add(148, "Burst");
            NameList.Add(149, "NanoProwessInitiative");
            NameList.Add(150, "FlingShot");
            NameList.Add(151, "AimedShot");
            NameList.Add(152, "BodyDevelopment");
            NameList.Add(153, "Duck");
            NameList.Add(154, "Dodge");
            NameList.Add(155, "Evade");
            NameList.Add(156, "RunSpeed");
            NameList.Add(157, "FieldQuantumPhysics");
            NameList.Add(158, "WeaponSmithing");
            NameList.Add(159, "Pharmaceuticals");
            NameList.Add(160, "NanoProgramming");
            NameList.Add(161, "ComputerLiteracy");
            NameList.Add(162, "Psychology");
            NameList.Add(163, "Chemistry");
            NameList.Add(164, "Concealment");
            NameList.Add(165, "BreakingEntry");
            NameList.Add(166, "DriveGround");
            NameList.Add(167, "FullAuto");
            NameList.Add(168, "NanoAC");
            NameList.Add(169, "AlienLevel");
            NameList.Add(170, "HealthChangeBest");
            NameList.Add(171, "HealthChangeWorst");
            NameList.Add(172, "HealthChange");
            NameList.Add(173, "CurrentMovementMode");
            NameList.Add(174, "PrevMovementMode");
            NameList.Add(175, "AutoLockTimeDefault");
            NameList.Add(176, "AutoUnlockTimeDefault");
            NameList.Add(177, "MoreFlags");
            NameList.Add(178, "AlienNextXP");
            NameList.Add(179, "NPCFlags");
            NameList.Add(180, "CurrentNCU");
            NameList.Add(181, "MaxNCU");
            NameList.Add(182, "Specialization");
            NameList.Add(183, "EffectIcon");
            NameList.Add(184, "BuildingType");
            NameList.Add(185, "BuildingInstance");
            NameList.Add(186, "CardOwnerType");
            NameList.Add(187, "CardOwnerInstance");
            NameList.Add(188, "BuildingComplexInst");
            NameList.Add(189, "ExitInstance");
            NameList.Add(190, "NextDoorInBuilding");
            NameList.Add(191, "LastConcretePlayfieldInstance");
            NameList.Add(192, "ExtenalPlayfieldInstance");
            NameList.Add(193, "ExtenalDoorInstance");
            NameList.Add(194, "InPlay");
            NameList.Add(195, "AccessKey");
            NameList.Add(196, "PetMaster");
            NameList.Add(197, "OrientationMode");
            NameList.Add(198, "SessionTime");
            NameList.Add(199, "RP");
            NameList.Add(200, "Conformity");
            NameList.Add(201, "Aggressiveness");
            NameList.Add(202, "Stability");
            NameList.Add(203, "Extroverty");
            NameList.Add(204, "BreedHostility");
            NameList.Add(205, "ReflectProjectileAC");
            NameList.Add(206, "ReflectMeleeAC");
            NameList.Add(207, "ReflectEnergyAC");
            NameList.Add(208, "ReflectChemicalAC");
            NameList.Add(210, "RechargeDelay");
            NameList.Add(211, "EquipDelay");
            NameList.Add(212, "MaxEnergy");
            NameList.Add(213, "TeamSide");
            NameList.Add(214, "CurrentNano");
            NameList.Add(215, "GmLevel");
            NameList.Add(216, "ReflectRadiationAC");
            NameList.Add(217, "ReflectColdAC");
            NameList.Add(218, "ReflectNanoAC");
            NameList.Add(219, "ReflectFireAC");
            NameList.Add(220, "CurrBodyLocation");
            NameList.Add(221, "MaxNanoEnergy");
            NameList.Add(222, "AccumulatedDamage");
            NameList.Add(223, "CanChangeClothes");
            NameList.Add(224, "Features");
            NameList.Add(225, "ReflectPoisonAC");
            NameList.Add(226, "ShieldProjectileAC");
            NameList.Add(227, "ShieldMeleeAC");
            NameList.Add(228, "ShieldEnergyAC");
            NameList.Add(229, "ShieldChemicalAC");
            NameList.Add(230, "ShieldRadiationAC");
            NameList.Add(231, "ShieldColdAC");
            NameList.Add(232, "ShieldNanoAC");
            NameList.Add(233, "ShieldFireAC");
            NameList.Add(234, "ShieldPoisonAC");
            NameList.Add(235, "BerserkMode");
            NameList.Add(236, "InsurancePercentage");
            NameList.Add(237, "ChangeSideCount");
            NameList.Add(238, "AbsorbProjectileAC");
            NameList.Add(239, "AbsorbMeleeAC");
            NameList.Add(240, "AbsorbEnergyAC");
            NameList.Add(241, "AbsorbChemicalAC");
            NameList.Add(242, "AbsorbRadiationAC");
            NameList.Add(243, "AbsorbColdAC");
            NameList.Add(244, "AbsorbFireAC");
            NameList.Add(245, "AbsorbPoisonAC");
            NameList.Add(246, "AbsorbNanoAC");
            NameList.Add(247, "TemporarySkillReduction");
            NameList.Add(248, "BirthDate");
            NameList.Add(249, "LastSaved");
            NameList.Add(250, "SoundVolume");
            NameList.Add(251, "PetCounter");
            NameList.Add(252, "MeetersWalked");
            NameList.Add(253, "QuestLevelsSolved");
            NameList.Add(254, "MonsterLevelsKilled");
            NameList.Add(255, "PvPLevelsKilled");
            NameList.Add(256, "MissionBits1");
            NameList.Add(257, "MissionBits2");
            NameList.Add(258, "AccessGrant");
            NameList.Add(259, "DoorFlags");
            NameList.Add(260, "ClanHierarchy");
            NameList.Add(261, "QuestStat");
            NameList.Add(262, "ClientActivated");
            NameList.Add(263, "PersonalResearchLevel");
            NameList.Add(264, "GlobalResearchLevel");
            NameList.Add(265, "PersonalResearchGoal");
            NameList.Add(266, "GlobalResearchGoal");
            NameList.Add(267, "TurnSpeed");
            NameList.Add(268, "LiquidType");
            NameList.Add(269, "GatherSound");
            NameList.Add(270, "CastSound");
            NameList.Add(271, "TravelSound");
            NameList.Add(272, "HitSound");
            NameList.Add(273, "SecondaryItemTemplate");
            NameList.Add(274, "EquippedWeapons");
            NameList.Add(275, "XPKillRange");
            NameList.Add(276, "AMSModifier");
            NameList.Add(277, "DMSModifier");
            NameList.Add(278, "ProjectileDamageModifier");
            NameList.Add(279, "MeleeDamageModifier");
            NameList.Add(280, "EnergyDamageModifier");
            NameList.Add(281, "ChemicalDamageModifier");
            NameList.Add(282, "RadiationDamageModifier");
            NameList.Add(283, "ItemHateValue");
            NameList.Add(284, "DamageBonus");
            NameList.Add(285, "MaxDamage");
            NameList.Add(286, "MinDamage");
            NameList.Add(287, "AttackRange");
            NameList.Add(288, "HateValueModifyer");
            NameList.Add(289, "TrapDifficulty");
            NameList.Add(290, "StatOne");
            NameList.Add(291, "NumAttackEffects");
            NameList.Add(292, "DefaultAttackType");
            NameList.Add(293, "ItemSkill");
            NameList.Add(294, "ItemDelay");
            NameList.Add(295, "ItemOpposedSkill");
            NameList.Add(296, "ItemSIS");
            NameList.Add(297, "InteractionRadius");
            NameList.Add(298, "Placement");
            NameList.Add(299, "LockDifficulty");
            NameList.Add(300, "Members");
            NameList.Add(301, "MinMembers");
            NameList.Add(302, "ClanPrice");
            NameList.Add(303, "MissionBits3");
            NameList.Add(304, "ClanType");
            NameList.Add(305, "ClanInstance");
            NameList.Add(306, "VoteCount");
            NameList.Add(307, "MemberType");
            NameList.Add(308, "MemberInstance");
            NameList.Add(309, "GlobalClanType");
            NameList.Add(310, "GlobalClanInstance");
            NameList.Add(311, "ColdDamageModifier");
            NameList.Add(312, "ClanUpkeepInterval");
            NameList.Add(313, "TimeSinceUpkeep");
            NameList.Add(314, "ClanFinalized");
            NameList.Add(315, "NanoDamageModifier");
            NameList.Add(316, "FireDamageModifier");
            NameList.Add(317, "PoisonDamageModifier");
            NameList.Add(318, "NPCostModifier");
            NameList.Add(319, "XPModifier");
            NameList.Add(320, "BreedLimit");
            NameList.Add(321, "GenderLimit");
            NameList.Add(322, "LevelLimit");
            NameList.Add(323, "PlayerKilling");
            NameList.Add(324, "TeamAllowed");
            NameList.Add(325, "WeaponDisallowedType");
            NameList.Add(326, "WeaponDisallowedInstance");
            NameList.Add(327, "Taboo");
            NameList.Add(328, "Compulsion");
            NameList.Add(329, "SkillDisabled");
            NameList.Add(330, "ClanItemType");
            NameList.Add(331, "ClanItemInstance");
            NameList.Add(332, "DebuffFormula");
            NameList.Add(333, "PvP_Rating");
            NameList.Add(334, "SavedXP");
            NameList.Add(335, "DoorBlockTime");
            NameList.Add(336, "OverrideTexture");
            NameList.Add(337, "OverrideMaterial");
            NameList.Add(338, "DeathReason");
            NameList.Add(339, "DamageOverrideType");
            NameList.Add(340, "BrainType");
            NameList.Add(341, "XPBonus");
            NameList.Add(342, "HealInterval");
            NameList.Add(343, "HealDelta");
            NameList.Add(344, "MonsterTexture");
            NameList.Add(345, "HasAlwaysLootable");
            NameList.Add(346, "TradeLimit");
            NameList.Add(347, "FaceTexture");
            NameList.Add(348, "SpecialCondition");
            NameList.Add(349, "AutoAttackFlags");
            NameList.Add(350, "NextXP");
            NameList.Add(351, "TeleportPauseMilliSeconds");
            NameList.Add(352, "SISCap");
            NameList.Add(353, "AnimSet");
            NameList.Add(354, "AttackType");
            NameList.Add(355, "NanoFocusLevel");
            NameList.Add(356, "NPCHash");
            NameList.Add(357, "CollisionRadius");
            NameList.Add(358, "OuterRadius");
            NameList.Add(359, "MonsterData");
            NameList.Add(360, "MonsterScale");
            NameList.Add(361, "HitEffectType");
            NameList.Add(362, "ResurrectDest");
            NameList.Add(363, "NanoInterval");
            NameList.Add(364, "NanoDelta");
            NameList.Add(365, "ReclaimItem");
            NameList.Add(366, "GatherEffectType");
            NameList.Add(367, "VisualBreed");
            NameList.Add(368, "VisualProfession");
            NameList.Add(369, "VisualSex");
            NameList.Add(370, "RitualTargetInst");
            NameList.Add(371, "SkillTimeOnSelectedTarget");
            NameList.Add(372, "LastSaveXP");
            NameList.Add(373, "ExtendedTime");
            NameList.Add(374, "BurstRecharge");
            NameList.Add(375, "FullAutoRecharge");
            NameList.Add(376, "GatherAbstractAnim");
            NameList.Add(377, "CastTargetAbstractAnim");
            NameList.Add(378, "CastSelfAbstractAnim");
            NameList.Add(379, "CriticalIncrease");
            NameList.Add(380, "RangeIncreaserWeapon");
            NameList.Add(381, "RangeIncreaserNF");
            NameList.Add(382, "SkillLockModifier");
            NameList.Add(383, "InterruptModifier");
            NameList.Add(384, "ACGEntranceStyles");
            NameList.Add(385, "ChanceOfBreakOnSpellAttack");
            NameList.Add(386, "ChanceOfBreakOnDebuff");
            NameList.Add(387, "DieAnim");
            NameList.Add(388, "TowerType");
            NameList.Add(389, "Expansion");
            NameList.Add(390, "LowresMesh");
            NameList.Add(391, "CriticalDecrease");
            NameList.Add(392, "OldTimeExist");
            NameList.Add(393, "ResistModifier");
            NameList.Add(394, "ChestFlags");
            NameList.Add(395, "PrimaryTemplateID");
            NameList.Add(396, "NumberOfItems");
            NameList.Add(397, "SelectedTargetType");
            NameList.Add(398, "Corpse_Hash");
            NameList.Add(399, "AmmoName");
            NameList.Add(400, "Rotation");
            NameList.Add(401, "CATAnim");
            NameList.Add(402, "CATAnimFlags");
            NameList.Add(403, "DisplayCATAnim");
            NameList.Add(404, "DisplayCATMesh");
            NameList.Add(405, "School");
            NameList.Add(406, "NanoSpeed");
            NameList.Add(407, "NanoPoints");
            NameList.Add(408, "TrainSkill");
            NameList.Add(409, "TrainSkillCost");
            NameList.Add(410, "IsFightingMe");
            NameList.Add(411, "NextFormula");
            NameList.Add(412, "MultipleCount");
            NameList.Add(413, "EffectType");
            NameList.Add(414, "ImpactEffectType");
            NameList.Add(415, "CorpseType");
            NameList.Add(416, "CorpseInstance");
            NameList.Add(417, "CorpseAnimKey");
            NameList.Add(418, "UnarmedTemplateInstance");
            NameList.Add(419, "TracerEffectType");
            NameList.Add(420, "AmmoType");
            NameList.Add(421, "CharRadius");
            NameList.Add(422, "ChanceOfUse");
            NameList.Add(423, "CurrentState");
            NameList.Add(424, "ArmourType");
            NameList.Add(425, "RestModifier");
            NameList.Add(426, "BuyModifier");
            NameList.Add(427, "SellModifier");
            NameList.Add(428, "CastEffectType");
            NameList.Add(429, "NPCBrainState");
            NameList.Add(430, "WaitState");
            NameList.Add(431, "SelectedTarget");
            NameList.Add(432, "MissionBits4");
            NameList.Add(433, "OwnerInstance");
            NameList.Add(434, "CharState");
            NameList.Add(435, "ReadOnly");
            NameList.Add(436, "DamageType");
            NameList.Add(437, "CollideCheckInterval");
            NameList.Add(438, "PlayfieldType");
            NameList.Add(439, "NPCCommand");
            NameList.Add(440, "InitiativeType");
            NameList.Add(441, "CharTmp1");
            NameList.Add(442, "CharTmp2");
            NameList.Add(443, "CharTmp3");
            NameList.Add(444, "CharTmp4");
            NameList.Add(445, "NPCCommandArg");
            NameList.Add(446, "NameTemplate");
            NameList.Add(447, "DesiredTargetDistance");
            NameList.Add(448, "VicinityRange");
            NameList.Add(449, "NPCIsSurrendering");
            NameList.Add(450, "StateMachine");
            NameList.Add(451, "NPCSurrenderInstance");
            NameList.Add(452, "NPCHasPatrolList");
            NameList.Add(453, "NPCVicinityChars");
            NameList.Add(454, "ProximityRangeOutdoors");
            NameList.Add(455, "NPCFamily");
            NameList.Add(456, "CommandRange");
            NameList.Add(457, "NPCHatelistSize");
            NameList.Add(458, "NPCNumPets");
            NameList.Add(459, "ODMinSizeAdd");
            NameList.Add(460, "EffectRed");
            NameList.Add(461, "EffectGreen");
            NameList.Add(462, "EffectBlue");
            NameList.Add(463, "ODMaxSizeAdd");
            NameList.Add(464, "DurationModifier");
            NameList.Add(465, "NPCCryForHelpRange");
            NameList.Add(466, "LOSHeight");
            NameList.Add(467, "PetReq1");
            NameList.Add(468, "PetReq2");
            NameList.Add(469, "PetReq3");
            NameList.Add(470, "MapOptions");
            NameList.Add(471, "MapAreaPart1");
            NameList.Add(472, "MapAreaPart2");
            NameList.Add(473, "FixtureFlags");
            NameList.Add(474, "FallDamage");
            NameList.Add(475, "ReflectReturnedProjectileAC");
            NameList.Add(476, "ReflectReturnedMeleeAC");
            NameList.Add(477, "ReflectReturnedEnergyAC");
            NameList.Add(478, "ReflectReturnedChemicalAC");
            NameList.Add(479, "ReflectReturnedRadiationAC");
            NameList.Add(480, "ReflectReturnedColdAC");
            NameList.Add(481, "ReflectReturnedNanoAC");
            NameList.Add(482, "ReflectReturnedFireAC");
            NameList.Add(483, "ReflectReturnedPoisonAC");
            NameList.Add(484, "ProximityRangeIndoors");
            NameList.Add(485, "PetReqVal1");
            NameList.Add(486, "PetReqVal2");
            NameList.Add(487, "PetReqVal3");
            NameList.Add(488, "TargetFacing");
            NameList.Add(489, "Backstab");
            NameList.Add(490, "OriginatorType");
            NameList.Add(491, "QuestInstance");
            NameList.Add(492, "QuestIndex1");
            NameList.Add(493, "QuestIndex2");
            NameList.Add(494, "QuestIndex3");
            NameList.Add(495, "QuestIndex4");
            NameList.Add(496, "QuestIndex5");
            NameList.Add(497, "QTDungeonInstance");
            NameList.Add(498, "QTNumMonsters");
            NameList.Add(499, "QTKilledMonsters");
            NameList.Add(500, "AnimPos");
            NameList.Add(501, "AnimPlay");
            NameList.Add(502, "AnimSpeed");
            NameList.Add(503, "QTKillNumMonsterID1");
            NameList.Add(504, "QTKillNumMonsterCount1");
            NameList.Add(505, "QTKillNumMonsterID2");
            NameList.Add(506, "QTKillNumMonsterCount2");
            NameList.Add(507, "QTKillNumMonsterID3");
            NameList.Add(508, "QTKillNumMonsterCount3");
            NameList.Add(509, "QuestIndex0");
            NameList.Add(510, "QuestTimeout");
            NameList.Add(511, "Tower_NPCHash");
            NameList.Add(512, "PetType");
            NameList.Add(513, "OnTowerCreation");
            NameList.Add(514, "OwnedTowers");
            NameList.Add(515, "TowerInstance");
            NameList.Add(516, "AttackShield");
            NameList.Add(517, "SpecialAttackShield");
            NameList.Add(518, "NPCVicinityPlayers");
            NameList.Add(519, "NPCUseFightModeRegenRate");
            NameList.Add(520, "Rnd");
            NameList.Add(521, "SocialStatus");
            NameList.Add(522, "LastRnd");
            NameList.Add(523, "ItemDelayCap");
            NameList.Add(524, "RechargeDelayCap");
            NameList.Add(525, "PercentRemainingHealth");
            NameList.Add(526, "PercentRemainingNano");
            NameList.Add(527, "TargetDistance");
            NameList.Add(528, "TeamCloseness");
            NameList.Add(529, "NumberOnHateList");
            NameList.Add(530, "ConditionState");
            NameList.Add(531, "ExpansionPlayfield");
            NameList.Add(532, "ShadowBreed");
            NameList.Add(533, "NPCFovStatus");
            NameList.Add(534, "DudChance");
            NameList.Add(535, "HealMultiplier");
            NameList.Add(536, "NanoDamageMultiplier");
            NameList.Add(537, "NanoVulnerability");
            NameList.Add(538, "AmsCap");
            NameList.Add(539, "ProcInitiative1");
            NameList.Add(540, "ProcInitiative2");
            NameList.Add(541, "ProcInitiative3");
            NameList.Add(542, "ProcInitiative4");
            NameList.Add(543, "FactionModifier");
            NameList.Add(544, "MissionBits8");
            NameList.Add(545, "MissionBits9");
            NameList.Add(546, "StackingLine2");
            NameList.Add(547, "StackingLine3");
            NameList.Add(548, "StackingLine4");
            NameList.Add(549, "StackingLine5");
            NameList.Add(550, "StackingLine6");
            NameList.Add(551, "StackingOrder");
            NameList.Add(552, "ProcNano1");
            NameList.Add(553, "ProcNano2");
            NameList.Add(554, "ProcNano3");
            NameList.Add(555, "ProcNano4");
            NameList.Add(556, "ProcChance1");
            NameList.Add(557, "ProcChance2");
            NameList.Add(558, "ProcChance3");
            NameList.Add(559, "ProcChance4");
            NameList.Add(560, "OTArmedForces");
            NameList.Add(561, "ClanSentinels");
            NameList.Add(562, "OTMed");
            NameList.Add(563, "ClanGaia");
            NameList.Add(564, "OTTrans");
            NameList.Add(565, "ClanVanguards");
            NameList.Add(566, "GOS");
            NameList.Add(567, "OTFollowers");
            NameList.Add(568, "OTOperator");
            NameList.Add(569, "OTUnredeemed");
            NameList.Add(570, "ClanDevoted");
            NameList.Add(571, "ClanConserver");
            NameList.Add(572, "ClanRedeemed");
            NameList.Add(573, "SK");
            NameList.Add(574, "LastSK");
            NameList.Add(575, "NextSK");
            NameList.Add(576, "PlayerOptions");
            NameList.Add(577, "LastPerkResetTime");
            NameList.Add(578, "CurrentTime");
            NameList.Add(579, "ShadowBreedTemplate");
            NameList.Add(580, "NPCVicinityFamily");
            NameList.Add(581, "NPCScriptAMSScale");
            NameList.Add(582, "ApartmentsAllowed");
            NameList.Add(583, "ApartmentsOwned");
            NameList.Add(584, "ApartmentAccessCard");
            NameList.Add(585, "MapAreaPart3");
            NameList.Add(586, "MapAreaPart4");
            NameList.Add(587, "NumberOfTeamMembers");
            NameList.Add(588, "ActionCategory");
            NameList.Add(589, "CurrentPlayfield");
            NameList.Add(590, "DistrictNano");
            NameList.Add(591, "DistrictNanoInterval");
            NameList.Add(592, "UnsavedXP");
            NameList.Add(593, "RegainXPPercentage");
            NameList.Add(594, "TempSaveTeamID");
            NameList.Add(595, "TempSavePlayfield");
            NameList.Add(596, "TempSaveX");
            NameList.Add(597, "TempSaveY");
            NameList.Add(598, "ExtendedFlags");
            NameList.Add(599, "ShopPrice");
            NameList.Add(600, "NewbieHP");
            NameList.Add(601, "HPLevelUp");
            NameList.Add(602, "HPPerSkill");
            NameList.Add(603, "NewbieNP");
            NameList.Add(604, "NPLevelUp");
            NameList.Add(605, "NPPerSkill");
            NameList.Add(606, "MaxShopItems");
            NameList.Add(607, "PlayerID");
            NameList.Add(608, "ShopRent");
            NameList.Add(609, "SynergyHash");
            NameList.Add(610, "ShopFlags");
            NameList.Add(611, "ShopLastUsed");
            NameList.Add(612, "ShopType");
            NameList.Add(613, "LockDownTime");
            NameList.Add(614, "LeaderLockDownTime");
            NameList.Add(615, "InvadersKilled");
            NameList.Add(616, "KilledByInvaders");
            NameList.Add(617, "MissionBits10");
            NameList.Add(618, "MissionBits11");
            NameList.Add(619, "MissionBits12");
            NameList.Add(620, "HouseTemplate");
            NameList.Add(621, "PercentFireDamage");
            NameList.Add(622, "PercentColdDamage");
            NameList.Add(623, "PercentMeleeDamage");
            NameList.Add(624, "PercentProjectileDamage");
            NameList.Add(625, "PercentPoisonDamage");
            NameList.Add(626, "PercentRadiationDamage");
            NameList.Add(627, "PercentEnergyDamage");
            NameList.Add(628, "PercentChemicalDamage");
            NameList.Add(629, "TotalDamage");
            NameList.Add(630, "TrackProjectileDamage");
            NameList.Add(631, "TrackMeleeDamage");
            NameList.Add(632, "TrackEnergyDamage");
            NameList.Add(633, "TrackChemicalDamage");
            NameList.Add(634, "TrackRadiationDamage");
            NameList.Add(635, "TrackColdDamage");
            NameList.Add(636, "TrackPoisonDamage");
            NameList.Add(637, "TrackFireDamage");
            NameList.Add(638, "NPCSpellArg1");
            NameList.Add(639, "NPCSpellRet1");
            NameList.Add(640, "CityInstance");
            NameList.Add(641, "DistanceToSpawnpoint");
            NameList.Add(642, "CityTerminalRechargePercent");
            NameList.Add(649, "UnreadMailCount");
            NameList.Add(650, "LastMailCheckTime");
            NameList.Add(651, "AdvantageHash1");
            NameList.Add(652, "AdvantageHash2");
            NameList.Add(653, "AdvantageHash3");
            NameList.Add(654, "AdvantageHash4");
            NameList.Add(655, "AdvantageHash5");
            NameList.Add(656, "ShopIndex");
            NameList.Add(657, "ShopID");
            NameList.Add(658, "IsVehicle");
            NameList.Add(659, "DamageToNano");
            NameList.Add(660, "AccountFlags");
            NameList.Add(661, "DamageToNanoMultiplier");
            NameList.Add(662, "MechData");
            NameList.Add(664, "VehicleAC");
            NameList.Add(665, "VehicleDamage");
            NameList.Add(666, "VehicleHealth");
            NameList.Add(667, "VehicleSpeed");
            NameList.Add(668, "BattlestationSide");
            NameList.Add(669, "VP");
            NameList.Add(670, "BattlestationRep");
            NameList.Add(671, "PetState");
            NameList.Add(672, "PaidPoints");
            NameList.Add(673, "VisualFlags");
            NameList.Add(674, "PVPDuelKills");
            NameList.Add(675, "PVPDuelDeaths");
            NameList.Add(676, "PVPProfessionDuelKills");
            NameList.Add(677, "PVPProfessionDuelDeaths");
            NameList.Add(678, "PVPRankedSoloKills");
            NameList.Add(679, "PVPRankedSoloDeaths");
            NameList.Add(680, "PVPRankedTeamKills");
            NameList.Add(681, "PVPRankedTeamDeaths");
            NameList.Add(682, "PVPSoloScore");
            NameList.Add(683, "PVPTeamScore");
            NameList.Add(684, "PVPDuelScore");
            NameList.Add(700, "ACGItemSeed");
            NameList.Add(701, "ACGItemLevel");
            NameList.Add(702, "ACGItemTemplateID");
            NameList.Add(703, "ACGItemTemplateID2");
            NameList.Add(704, "ACGItemCategoryID");
            NameList.Add(768, "HasKnuBotData");
            NameList.Add(800, "QuestBoothDifficulty");
            NameList.Add(801, "QuestASMinimumRange");
            NameList.Add(802, "QuestASMaximumRange");
            NameList.Add(888, "VisualLODLevel");
            NameList.Add(889, "TargetDistanceChange");
            NameList.Add(900, "TideRequiredDynelID");
            NameList.Add(999, "StreamCheckMagic");
            NameList.Add(1001, "Type");
            NameList.Add(1002, "Instance");
            NameList.Add(1003, "WeaponType");
            NameList.Add(1004, "ShoulderMeshRight");
            NameList.Add(1005, "ShoulderMeshLeft");
            NameList.Add(1006, "WeaponMeshRight");
            NameList.Add(1007, "WeaponMeshLeft");
            NameList.Add(1014, "OverrideTextureAttractor");
            NameList.Add(1013, "OverrideTextureBack");
            NameList.Add(1008, "OverrideTextureHead");
            NameList.Add(1012, "OverrideTextureShoulderpadLeft");
            NameList.Add(1011, "OverrideTextureShoulderpadRight");
            NameList.Add(1010, "OverrideTextureWeaponLeft");
            NameList.Add(1009, "OverrideTextureWeaponRight");

            Dictionary<int, string> temp = new Dictionary<int, string>();

            // We dont want to be too specific here, so lets turn them all lower case (for user input)
            foreach (int number in NameList.Keys)
            {
                temp.Add(number, NameList[number].ToLower());
            }

            NameList = temp;

            // and create a crossreferencing List
            foreach (KeyValuePair<int, string> keyValuePair in NameList)
            {
                NumberList.Add(keyValuePair.Value, keyValuePair.Key);
            }

            // Filling Default List (only not 1234567890 values)
            Defaults.Add(0, 8917569);
            Defaults.Add(1, 1);
            Defaults.Add(3, 5);
            Defaults.Add(5, 0);
            Defaults.Add(6, 0);
            Defaults.Add(7, 0);
            Defaults.Add(9, 0);
            Defaults.Add(11, 50);
            Defaults.Add(12, 17530);
            Defaults.Add(16, 0);
            Defaults.Add(17, 0);
            Defaults.Add(18, 0);
            Defaults.Add(19, 0);
            Defaults.Add(20, 0);
            Defaults.Add(21, 0);
            Defaults.Add(27, 1);
            Defaults.Add(32, 0);
            Defaults.Add(33, 0);
            Defaults.Add(34, 0);
            Defaults.Add(37, 1);
            Defaults.Add(38, 0);
            Defaults.Add(40, 0);
            Defaults.Add(45, 0);
            Defaults.Add(49, 0);
            Defaults.Add(51, 100);
            Defaults.Add(52, 0);
            Defaults.Add(53, 1500);
            Defaults.Add(57, 0);
            Defaults.Add(58, 0);
            Defaults.Add(61, 0);
            Defaults.Add(62, 0);
            Defaults.Add(63, 0);
            Defaults.Add(64, 0);
            Defaults.Add(65, 0);
            Defaults.Add(66, 0);
            Defaults.Add(67, 0);
            Defaults.Add(68, 0);
            Defaults.Add(69, 0);
            Defaults.Add(72, 0);
            Defaults.Add(75, 0);
            Defaults.Add(78, 0);
            Defaults.Add(79, 0);
            Defaults.Add(89, 1);
            Defaults.Add(90, 0);
            Defaults.Add(91, 0);
            Defaults.Add(92, 0);
            Defaults.Add(93, 0);
            Defaults.Add(94, 0);
            Defaults.Add(95, 0);
            Defaults.Add(96, 0);
            Defaults.Add(97, 0);
            Defaults.Add(100, 5);
            Defaults.Add(101, 5);
            Defaults.Add(102, 5);
            Defaults.Add(103, 5);
            Defaults.Add(104, 5);
            Defaults.Add(105, 5);
            Defaults.Add(106, 5);
            Defaults.Add(107, 5);
            Defaults.Add(108, 5);
            Defaults.Add(109, 5);
            Defaults.Add(110, 5);
            Defaults.Add(111, 5);
            Defaults.Add(112, 5);
            Defaults.Add(113, 5);
            Defaults.Add(114, 5);
            Defaults.Add(115, 5);
            Defaults.Add(116, 5);
            Defaults.Add(117, 5);
            Defaults.Add(118, 5);
            Defaults.Add(119, 5);
            Defaults.Add(120, 5);
            Defaults.Add(121, 5);
            Defaults.Add(122, 5);
            Defaults.Add(123, 5);
            Defaults.Add(124, 5);
            Defaults.Add(125, 5);
            Defaults.Add(126, 5);
            Defaults.Add(127, 5);
            Defaults.Add(128, 5);
            Defaults.Add(129, 5);
            Defaults.Add(130, 5);
            Defaults.Add(131, 5);
            Defaults.Add(132, 5);
            Defaults.Add(133, 5);
            Defaults.Add(134, 5);
            Defaults.Add(135, 5);
            Defaults.Add(136, 5);
            Defaults.Add(137, 5);
            Defaults.Add(138, 5);
            Defaults.Add(139, 5);
            Defaults.Add(140, 5);
            Defaults.Add(141, 5);
            Defaults.Add(142, 5);
            Defaults.Add(143, 5);
            Defaults.Add(144, 5);
            Defaults.Add(145, 5);
            Defaults.Add(146, 5);
            Defaults.Add(147, 5);
            Defaults.Add(148, 5);
            Defaults.Add(149, 5);
            Defaults.Add(150, 5);
            Defaults.Add(151, 5);
            Defaults.Add(152, 5);
            Defaults.Add(153, 5);
            Defaults.Add(154, 5);
            Defaults.Add(155, 5);
            Defaults.Add(156, 5);
            Defaults.Add(157, 5);
            Defaults.Add(158, 5);
            Defaults.Add(159, 5);
            Defaults.Add(160, 5);
            Defaults.Add(161, 5);
            Defaults.Add(162, 5);
            Defaults.Add(163, 5);
            Defaults.Add(164, 5);
            Defaults.Add(165, 5);
            Defaults.Add(166, 5);
            Defaults.Add(167, 5);
            Defaults.Add(168, 5);
            Defaults.Add(169, 0);
            Defaults.Add(173, 3);
            Defaults.Add(174, 3);
            Defaults.Add(178, 1500);
            Defaults.Add(180, 0);
            Defaults.Add(181, 8);
            Defaults.Add(182, 0);
            Defaults.Add(191, 0);
            Defaults.Add(194, 0);
            Defaults.Add(199, 0);
            Defaults.Add(205, 0);
            Defaults.Add(206, 0);
            Defaults.Add(207, 0);
            Defaults.Add(208, 0);
            Defaults.Add(212, 1);
            Defaults.Add(213, 0);
            Defaults.Add(214, 1);
            Defaults.Add(215, 0);
            Defaults.Add(216, 0);
            Defaults.Add(217, 0);
            Defaults.Add(218, 0);
            Defaults.Add(219, 0);
            Defaults.Add(220, 0);
            Defaults.Add(221, 1);
            Defaults.Add(224, 6);
            Defaults.Add(225, 0);
            Defaults.Add(226, 0);
            Defaults.Add(227, 0);
            Defaults.Add(228, 0);
            Defaults.Add(229, 0);
            Defaults.Add(230, 0);
            Defaults.Add(231, 0);
            Defaults.Add(232, 0);
            Defaults.Add(233, 0);
            Defaults.Add(234, 0);
            Defaults.Add(236, 0);
            Defaults.Add(237, 0);
            Defaults.Add(238, 0);
            Defaults.Add(239, 0);
            Defaults.Add(240, 0);
            Defaults.Add(241, 0);
            Defaults.Add(242, 0);
            Defaults.Add(243, 0);
            Defaults.Add(244, 0);
            Defaults.Add(245, 0);
            Defaults.Add(246, 0);
            Defaults.Add(247, 0);
            Defaults.Add(256, 0);
            Defaults.Add(257, 0);
            Defaults.Add(263, 0);
            Defaults.Add(264, 0);
            Defaults.Add(265, 0);
            Defaults.Add(266, 0);
            Defaults.Add(267, 40000);
            Defaults.Add(275, 5);
            Defaults.Add(276, 0);
            Defaults.Add(277, 0);
            Defaults.Add(278, 0);
            Defaults.Add(279, 0);
            Defaults.Add(280, 0);
            Defaults.Add(281, 0);
            Defaults.Add(282, 0);
            Defaults.Add(300, 999);
            Defaults.Add(303, 0);
            Defaults.Add(315, 0);
            Defaults.Add(316, 0);
            Defaults.Add(317, 0);
            Defaults.Add(318, 0);
            Defaults.Add(319, 0);
            Defaults.Add(333, 1300);
            Defaults.Add(334, 0);
            Defaults.Add(342, 29);
            Defaults.Add(348, 1);
            Defaults.Add(349, 5);
            Defaults.Add(350, 1450);
            Defaults.Add(355, 0);
            Defaults.Add(359, 0);
            Defaults.Add(363, 28);
            Defaults.Add(372, 0);
            Defaults.Add(380, 0);
            Defaults.Add(381, 0);
            Defaults.Add(382, 0);
            Defaults.Add(389, 0);
            Defaults.Add(410, 0);
            Defaults.Add(412, 1);
            Defaults.Add(418, 0);
            Defaults.Add(423, 0);
            Defaults.Add(430, 2);
            Defaults.Add(432, 0);
            Defaults.Add(470, 0);
            Defaults.Add(471, 0);
            Defaults.Add(472, 0);
            Defaults.Add(475, 0);
            Defaults.Add(476, 0);
            Defaults.Add(477, 0);
            Defaults.Add(478, 0);
            Defaults.Add(479, 0);
            Defaults.Add(480, 0);
            Defaults.Add(481, 0);
            Defaults.Add(482, 0);
            Defaults.Add(483, 0);
            Defaults.Add(521, 0);
            Defaults.Add(532, 0);
            Defaults.Add(536, 0);
            Defaults.Add(544, 0);
            Defaults.Add(545, 0);
            Defaults.Add(560, 0);
            Defaults.Add(561, 0);
            Defaults.Add(563, 0);
            Defaults.Add(564, 0);
            Defaults.Add(565, 0);
            Defaults.Add(566, 0);
            Defaults.Add(567, 0);
            Defaults.Add(568, 0);
            Defaults.Add(569, 0);
            Defaults.Add(570, 0);
            Defaults.Add(571, 0);
            Defaults.Add(572, 0);
            Defaults.Add(573, 0);
            Defaults.Add(574, 0);
            Defaults.Add(575, 0);
            Defaults.Add(576, 0);
            Defaults.Add(577, 0);
            Defaults.Add(579, 0);
            Defaults.Add(582, 1);
            Defaults.Add(583, 0);
            Defaults.Add(585, 0);
            Defaults.Add(586, 0);
            Defaults.Add(592, 0);
            Defaults.Add(593, 0);
            Defaults.Add(594, 0);
            Defaults.Add(595, 0);
            Defaults.Add(596, 0);
            Defaults.Add(597, 0);
            Defaults.Add(615, 0);
            Defaults.Add(616, 0);
            Defaults.Add(617, 0);
            Defaults.Add(618, 0);
            Defaults.Add(619, 0);
            Defaults.Add(649, 0);
            Defaults.Add(650, 1283065897);
            Defaults.Add(662, 0);
            Defaults.Add(668, 0);
            Defaults.Add(669, 0);
            Defaults.Add(670, 10);
            Defaults.Add(672, 0);
            Defaults.Add(673, 31);
            Defaults.Add(674, 0);
            Defaults.Add(675, 0);
            Defaults.Add(676, 0);
            Defaults.Add(677, 0);
            Defaults.Add(678, 0);
            Defaults.Add(679, 0);
            Defaults.Add(680, 0);
            Defaults.Add(681, 0);
            Defaults.Add(682, 0);
            Defaults.Add(683, 0);
            Defaults.Add(684, 0);
            Defaults.Add(1004, 0);
            Defaults.Add(1005, 0);
            Defaults.Add(1006, 0);
            Defaults.Add(1007, 0);
            Defaults.Add(1014, 0);
            Defaults.Add(1013, 0);
            Defaults.Add(1008, 0);
            Defaults.Add(1012, 0);
            Defaults.Add(1011, 0);
            Defaults.Add(1010, 0);
            Defaults.Add(1009, 0);
        }

        #endregion

        // TODO: generate the default value list from database, make it depending on breed, so NPCs can have different defaults

        #region Public Methods and Operators

        /// <summary>
        /// Return Stat's default value
        /// </summary>
        /// <param name="statId">
        /// Stat Id to look for
        /// </param>
        /// <returns>
        /// Stat Default value
        /// </returns>
        public static int GetDefault(int statId)
        {
            // Return 1234567890 if nothing else is specified
            return !Defaults.ContainsKey(statId) ? 1234567890 : Defaults[statId];
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public static string GetStatName(int index)
        {
            if (NameList.ContainsKey(index))
            {
                return NameList[index];
            }

            throw new StatDoesNotExistException("Stat with id '" + index + "' does not exist");
        }

        /// <summary>
        /// Returns number of named Stat
        /// </summary>
        /// <param name="name">
        /// The name of the stat to look for
        /// </param>
        /// <returns>
        /// Stat Id
        /// </returns>
        /// <exception cref="StatDoesNotExistException">
        /// If specified Stat does not exist
        /// </exception>
        public static int GetStatNumber(string name)
        {
            // Check lowercase
            if (!NumberList.ContainsKey(name.ToLower()))
            {
                throw new StatDoesNotExistException("Stat with name '" + name + "' does not exist");
            }

            return NumberList[name.ToLower()];
        }

        #endregion
    }
}