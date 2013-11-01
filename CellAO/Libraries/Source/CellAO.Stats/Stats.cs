#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-11-01 18:28

#endregion

namespace CellAO.Stats
{
    #region Usings ...

    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using CellAO.Core.Exceptions;
    using CellAO.Database.Dao;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    #endregion

    #region Character_Stats holder for Character's stats

    #endregion

    /// <summary>
    /// </summary>
    public class Stats : IStatList
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Stat absorbChemicalAC = new Stat(241, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbColdAC = new Stat(243, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbEnergyAC = new Stat(240, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbFireAC = new Stat(244, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbMeleeAC = new Stat(239, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbNanoAC = new Stat(246, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbPoisonAC = new Stat(245, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbProjectileAC = new Stat(238, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat absorbRadiationAC = new Stat(242, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat accessCount = new Stat(35, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat accessGrant = new Stat(258, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat accessKey = new Stat(195, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat accountFlags = new Stat(660, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat accumulatedDamage = new Stat(222, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat acgEntranceStyles = new Stat(384, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat acgItemCategoryId = new Stat(704, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat acgItemLevel = new Stat(701, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat acgItemSeed = new Stat(700, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat acgItemTemplateId = new Stat(702, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat acgItemTemplateId2 = new Stat(703, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat actionCategory = new Stat(588, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash1 = new Stat(651, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash2 = new Stat(652, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash3 = new Stat(653, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash4 = new Stat(654, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash5 = new Stat(655, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat adventuring = new Stat(137, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat age = new Stat(58, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat aggDef = new Stat(51, 100, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat aggressiveness = new Stat(201, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat agility = new Stat(17, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat aimedShot = new Stat(151, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat alienLevel = new Stat(169, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat alienNextXP = new Stat(178, 1500, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat alienXP = new Stat(40, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat alignment = new Stat(62, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly List<IStat> all = new List<IStat>();

        /// <summary>
        /// </summary>
        private readonly Stat ammoName = new Stat(399, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat ammoType = new Stat(420, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat ams = new Stat(22, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat amsCap = new Stat(538, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat amsModifier = new Stat(276, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat anim = new Stat(13, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat animPlay = new Stat(501, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat animPos = new Stat(500, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat animSet = new Stat(353, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat animSpeed = new Stat(502, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat apartmentAccessCard = new Stat(584, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat apartmentsAllowed = new Stat(582, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat apartmentsOwned = new Stat(583, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat areaInstance = new Stat(87, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat areaType = new Stat(86, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat armourType = new Stat(424, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat assaultRifle = new Stat(116, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat attackCount = new Stat(36, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat attackRange = new Stat(287, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat attackShield = new Stat(516, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat attackSpeed = new Stat(3, 5, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat attackType = new Stat(354, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat attitude = new Stat(63, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat autoAttackFlags = new Stat(349, 5, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat autoLockTimeDefault = new Stat(175, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat autoUnlockTimeDefault = new Stat(176, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat backMesh = new Stat(38, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat backstab = new Stat(489, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat bandolierSlots = new Stat(46, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat battlestationRep = new Stat(670, 10, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat battlestationSide = new Stat(668, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat beltSlots = new Stat(45, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat berserkMode = new Stat(235, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat biologicalMetamorphose = new Stat(128, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat birthDate = new Stat(248, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat bodyDevelopment = new Stat(152, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat bow = new Stat(111, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat bowSpecialAttack = new Stat(121, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat brainType = new Stat(340, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat brawl = new Stat(142, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat breakingEntry = new Stat(165, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat breed = new Stat(4, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat breedHostility = new Stat(204, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat breedLimit = new Stat(320, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat buildingComplexInst = new Stat(188, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat buildingInstance = new Stat(185, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat buildingType = new Stat(184, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat burst = new Stat(148, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat burstRecharge = new Stat(374, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat buyModifier = new Stat(426, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat can = new Stat(30, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat canChangeClothes = new Stat(223, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat cardOwnerInstance = new Stat(187, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat cardOwnerType = new Stat(186, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat cash = new Stat(61, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat castEffectType = new Stat(428, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat castSelfAbstractAnim = new Stat(378, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat castSound = new Stat(270, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat castTargetAbstractAnim = new Stat(377, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat catAnim = new Stat(401, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat catAnimFlags = new Stat(402, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat catMesh = new Stat(42, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chanceOfBreakOnDebuff = new Stat(386, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chanceOfBreakOnSpellAttack = new Stat(385, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chanceOfUse = new Stat(422, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat changeSideCount = new Stat(237, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat charRadius = new Stat(421, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat charState = new Stat(434, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat charTmp1 = new Stat(441, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat charTmp2 = new Stat(442, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat charTmp3 = new Stat(443, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat charTmp4 = new Stat(444, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chemicalAC = new Stat(93, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chemicalDamageModifier = new Stat(281, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chemistry = new Stat(163, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat chestFlags = new Stat(394, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat cityInstance = new Stat(640, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat cityTerminalRechargePercent = new Stat(642, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clan = new Stat(5, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanConserver = new Stat(571, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanDevoted = new Stat(570, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanFinalized = new Stat(314, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanGaia = new Stat(563, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanHierarchy = new Stat(260, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanInstance = new Stat(305, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanItemInstance = new Stat(331, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanItemType = new Stat(330, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanLevel = new Stat(48, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanPrice = new Stat(302, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanRedeemed = new Stat(572, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanSentinels = new Stat(561, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanType = new Stat(304, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanUpkeepInterval = new Stat(312, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clanVanguards = new Stat(565, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat clientActivated = new Stat(262, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat closeCombatInitiative = new Stat(118, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat coldAC = new Stat(95, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat coldDamageModifier = new Stat(311, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat collideCheckInterval = new Stat(437, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat collisionRadius = new Stat(357, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat commandRange = new Stat(456, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat compulsion = new Stat(328, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat computerLiteracy = new Stat(161, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat concealment = new Stat(164, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat conditionState = new Stat(530, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat conformity = new Stat(200, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat corpseAnimKey = new Stat(417, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat corpseHash = new Stat(398, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat corpseInstance = new Stat(416, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat corpseType = new Stat(415, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat criticalDecrease = new Stat(391, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat criticalIncrease = new Stat(379, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currBodyLocation = new Stat(220, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentMass = new Stat(78, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentMovementMode = new Stat(173, 3, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentNCU = new Stat(180, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentNano = new Stat(214, 1, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentPlayfield = new Stat(589, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentState = new Stat(423, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat currentTime = new Stat(578, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat damageBonus = new Stat(284, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat damageOverrideType = new Stat(339, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat damageToNano = new Stat(659, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat damageToNanoMultiplier = new Stat(661, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat damageType = new Stat(436, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat deadTimer = new Stat(34, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat deathReason = new Stat(338, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat debuffFormula = new Stat(332, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat defaultAttackType = new Stat(292, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat defaultPos = new Stat(88, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat desiredTargetDistance = new Stat(447, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat dieAnim = new Stat(387, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat dimach = new Stat(144, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat disarmTrap = new Stat(135, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat displayCATAnim = new Stat(403, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat displayCATMesh = new Stat(404, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat distanceToSpawnpoint = new Stat(641, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat distanceWeaponInitiative = new Stat(119, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat districtNano = new Stat(590, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat districtNanoInterval = new Stat(591, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat dms = new Stat(29, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat dmsModifier = new Stat(277, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat dodge = new Stat(154, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat doorBlockTime = new Stat(335, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat doorFlags = new Stat(259, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat driveAir = new Stat(139, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat driveGround = new Stat(166, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat driveWater = new Stat(117, 5, true, false, false);

        private readonly Identity owner;

        public Identity Owner
        {
            get
            {
                return this.owner;
            }
        }

        /// <summary>
        /// </summary>
        private readonly Stat duck = new Stat(153, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat dudChance = new Stat(534, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat durationModifier = new Stat(464, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat effectBlue = new Stat(462, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat effectGreen = new Stat(461, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat effectIcon = new Stat(183, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat effectRed = new Stat(460, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat effectType = new Stat(413, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat electricalEngineering = new Stat(126, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat energy = new Stat(26, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat energyAC = new Stat(92, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat energyDamageModifier = new Stat(280, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat equipDelay = new Stat(211, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat equippedWeapons = new Stat(274, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat evade = new Stat(155, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat exitInstance = new Stat(189, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat expansion = new Stat(389, 0, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat expansionPlayfield = new Stat(531, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat extenalDoorInstance = new Stat(193, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat extenalPlayfieldInstance = new Stat(192, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat extendedFlags = new Stat(598, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat extendedTime = new Stat(373, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat extroverty = new Stat(203, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fabricType = new Stat(41, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat face = new Stat(31, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat faceTexture = new Stat(347, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat factionModifier = new Stat(543, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fallDamage = new Stat(474, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fastAttack = new Stat(147, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fatness = new Stat(47, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat features = new Stat(224, 6, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fieldQuantumPhysics = new Stat(157, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fireAC = new Stat(97, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fireDamageModifier = new Stat(316, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat firstAid = new Stat(123, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fixtureFlags = new Stat(473, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat flags = new Stat(0, 8917569, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat flingShot = new Stat(150, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fullAuto = new Stat(167, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat fullAutoRecharge = new Stat(375, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat gatherAbstractAnim = new Stat(376, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat gatherEffectType = new Stat(366, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat gatherSound = new Stat(269, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat genderLimit = new Stat(321, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat globalClanInstance = new Stat(310, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat globalClanType = new Stat(309, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat globalResearchGoal = new Stat(266, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat globalResearchLevel = new Stat(264, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat gmLevel = new Stat(215, 0, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat gos = new Stat(566, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat grenade = new Stat(109, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hairMesh = new Stat(32, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hasAlwaysLootable = new Stat(345, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hasKnuBotData = new Stat(768, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hateValueModifyer = new Stat(288, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat headMesh = new Stat(64, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat healDelta = new Stat(343, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat healInterval = new Stat(342, 29, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat healMultiplier = new Stat(535, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat health = new Stat(27, 1, true, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat healthChange = new Stat(172, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat healthChangeBest = new Stat(170, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat healthChangeWorst = new Stat(171, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat height = new Stat(28, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hitEffectType = new Stat(361, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hitSound = new Stat(272, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat houseTemplate = new Stat(620, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hpLevelUp = new Stat(601, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat hpPerSkill = new Stat(602, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat icon = new Stat(79, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat impactEffectType = new Stat(414, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat inPlay = new Stat(194, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat info = new Stat(15, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat initiativeType = new Stat(440, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat instance = new Stat(1002, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat insurancePercentage = new Stat(236, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat insuranceTime = new Stat(49, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat intelligence = new Stat(19, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat interactionRadius = new Stat(297, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat interruptModifier = new Stat(383, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat invadersKilled = new Stat(615, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat inventoryId = new Stat(55, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat inventoryTimeout = new Stat(50, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat ip = new Stat(53, 1500, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat isFightingMe = new Stat(410, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat isVehicle = new Stat(658, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemAnim = new Stat(99, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemClass = new Stat(76, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemDelay = new Stat(294, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemDelayCap = new Stat(523, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemHateValue = new Stat(283, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemOpposedSkill = new Stat(295, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemSIS = new Stat(296, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemSkill = new Stat(293, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat itemType = new Stat(72, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat killedByInvaders = new Stat(616, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastConcretePlayfieldInstance = new Stat(191, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastMailCheckTime = new Stat(650, 1283065897, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastPerkResetTime = new Stat(577, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastRnd = new Stat(522, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastSK = new Stat(574, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastSaveXP = new Stat(372, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastSaved = new Stat(249, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lastXP = new Stat(57, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat leaderLockDownTime = new Stat(614, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat level = new Stat(54, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat levelLimit = new Stat(322, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat life = new Stat(1, 1, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat liquidType = new Stat(268, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lockDifficulty = new Stat(299, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lockDownTime = new Stat(613, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat losHeight = new Stat(466, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lowresMesh = new Stat(390, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lrEnergyWeapon = new Stat(133, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat lrMultipleWeapon = new Stat(134, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart1 = new Stat(471, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart2 = new Stat(472, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart3 = new Stat(585, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart4 = new Stat(586, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapFlags = new Stat(9, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapNavigation = new Stat(140, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mapOptions = new Stat(470, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat martialArts = new Stat(100, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat materialCreation = new Stat(130, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat materialLocation = new Stat(131, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat materialMetamorphose = new Stat(127, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat maxDamage = new Stat(285, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat maxEnergy = new Stat(212, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat maxMass = new Stat(24, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat maxNCU = new Stat(181, 8, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat maxNanoEnergy = new Stat(221, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat maxShopItems = new Stat(606, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mechData = new Stat(662, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mechanicalEngineering = new Stat(125, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat meleeAC = new Stat(91, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat meleeDamageModifier = new Stat(279, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat meleeEnergyWeapon = new Stat(104, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat meleeMultiple = new Stat(101, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat memberInstance = new Stat(308, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat memberType = new Stat(307, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat members = new Stat(300, 999, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat mesh = new Stat(12, 17530, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat metaType = new Stat(75, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat metersWalked = new Stat(252, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat minDamage = new Stat(286, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat minMembers = new Stat(301, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits1 = new Stat(256, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits10 = new Stat(617, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits11 = new Stat(618, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits12 = new Stat(619, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits2 = new Stat(257, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits3 = new Stat(303, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits4 = new Stat(432, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits5 = new Stat(65, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits6 = new Stat(66, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits7 = new Stat(67, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits8 = new Stat(544, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat missionBits9 = new Stat(545, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat monsterData = new Stat(359, 0, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat monsterLevelsKilled = new Stat(254, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat monsterScale = new Stat(360, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat monsterTexture = new Stat(344, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat monthsPaid = new Stat(69, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat moreFlags = new Stat(177, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat multipleCount = new Stat(412, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat name = new Stat(14, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nameTemplate = new Stat(446, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoAC = new Stat(168, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoDamageModifier = new Stat(315, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoDamageMultiplier = new Stat(536, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoDelta = new Stat(364, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoEnergyPool = new Stat(132, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoFocusLevel = new Stat(355, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoInterval = new Stat(363, 28, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoPoints = new Stat(407, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoProgramming = new Stat(160, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoProwessInitiative = new Stat(149, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoSpeed = new Stat(406, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nanoVulnerability = new Stat(537, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat newbieHP = new Stat(600, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat newbieNP = new Stat(603, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nextDoorInBuilding = new Stat(190, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nextFormula = new Stat(411, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nextSK = new Stat(575, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat nextXP = new Stat(350, 1450, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npCostModifier = new Stat(318, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npLevelUp = new Stat(604, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npPerSkill = new Stat(605, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcBrainState = new Stat(429, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcCommand = new Stat(439, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcCommandArg = new Stat(445, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcCryForHelpRange = new Stat(465, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcFamily = new Stat(455, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcFlags = new Stat(179, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcFovStatus = new Stat(533, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcHasPatrolList = new Stat(452, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcHash = new Stat(356, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcHatelistSize = new Stat(457, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcIsSurrendering = new Stat(449, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcNumPets = new Stat(458, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcScriptAmsScale = new Stat(581, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcSpellArg1 = new Stat(638, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcSpellRet1 = new Stat(639, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcSurrenderInstance = new Stat(451, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcUseFightModeRegenRate = new Stat(519, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcVicinityChars = new Stat(453, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcVicinityFamily = new Stat(580, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat npcVicinityPlayers = new Stat(518, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat numAttackEffects = new Stat(291, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat numberOfItems = new Stat(396, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat numberOfTeamMembers = new Stat(587, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat numberOnHateList = new Stat(529, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat objectType = new Stat(1001, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat odMaxSizeAdd = new Stat(463, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat odMinSizeAdd = new Stat(459, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat oldTimeExist = new Stat(392, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat onTowerCreation = new Stat(513, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat onehBluntWeapons = new Stat(102, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat onehEdgedWeapon = new Stat(103, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat orientationMode = new Stat(197, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat originatorType = new Stat(490, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat otArmedForces = new Stat(560, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat otFollowers = new Stat(567, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat otMed = new Stat(562, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat otOperator = new Stat(568, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat otTrans = new Stat(564, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat otUnredeemed = new Stat(569, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat outerRadius = new Stat(358, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideMaterial = new Stat(337, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTexture = new Stat(336, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureAttractor = new Stat(1014, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureBack = new Stat(1013, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureHead = new Stat(1008, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureShoulderpadLeft = new Stat(1012, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureShoulderpadRight = new Stat(1011, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureWeaponLeft = new Stat(1010, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureWeaponRight = new Stat(1009, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat ownedTowers = new Stat(514, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat ownerInstance = new Stat(433, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat paidPoints = new Stat(672, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat parentInstance = new Stat(44, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat parentType = new Stat(43, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat parry = new Stat(145, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentChemicalDamage = new Stat(628, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentColdDamage = new Stat(622, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentEnergyDamage = new Stat(627, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentFireDamage = new Stat(621, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentMeleeDamage = new Stat(623, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentPoisonDamage = new Stat(625, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentProjectileDamage = new Stat(624, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentRadiationDamage = new Stat(626, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentRemainingHealth = new Stat(525, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat percentRemainingNano = new Stat(526, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat perception = new Stat(136, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat personalResearchGoal = new Stat(265, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat personalResearchLevel = new Stat(263, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petCounter = new Stat(251, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petMaster = new Stat(196, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petReq1 = new Stat(467, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petReq2 = new Stat(468, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petReq3 = new Stat(469, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petReqVal1 = new Stat(485, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petReqVal2 = new Stat(486, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petReqVal3 = new Stat(487, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petState = new Stat(671, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat petType = new Stat(512, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pharmaceuticals = new Stat(159, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat physicalProwessInitiative = new Stat(120, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat piercing = new Stat(106, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pistol = new Stat(112, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat placement = new Stat(298, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat playerId = new Stat(607, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat playerKilling = new Stat(323, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat playerOptions = new Stat(576, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat playfieldType = new Stat(438, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat poisonAC = new Stat(96, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat poisonDamageModifier = new Stat(317, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat prevMovementMode = new Stat(174, 3, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat previousHealth = new Stat(11, 50, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat price = new Stat(74, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat primaryItemInstance = new Stat(81, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat primaryItemType = new Stat(80, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat primaryTemplateId = new Stat(395, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procChance1 = new Stat(556, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procChance2 = new Stat(557, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procChance3 = new Stat(558, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procChance4 = new Stat(559, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative1 = new Stat(539, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative2 = new Stat(540, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative3 = new Stat(541, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative4 = new Stat(542, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procNano1 = new Stat(552, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procNano2 = new Stat(553, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procNano3 = new Stat(554, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat procNano4 = new Stat(555, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat profession = new Stat(60, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat professionLevel = new Stat(10, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly Stat projectileAC = new Stat(90, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat projectileDamageModifier = new Stat(278, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat proximityRangeIndoors = new Stat(484, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat proximityRangeOutdoors = new Stat(454, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat psychic = new Stat(21, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat psychologicalModification = new Stat(129, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat psychology = new Stat(162, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvPLevelsKilled = new Stat(255, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpDuelDeaths = new Stat(675, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpDuelKills = new Stat(674, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpDuelScore = new Stat(684, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpProfessionDuelDeaths = new Stat(677, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpProfessionDuelKills = new Stat(676, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedSoloDeaths = new Stat(679, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedSoloKills = new Stat(678, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedTeamDeaths = new Stat(681, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedTeamKills = new Stat(680, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpRating = new Stat(333, 1300, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpSoloScore = new Stat(682, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat pvpTeamScore = new Stat(683, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtDungeonInstance = new Stat(497, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterCount1 = new Stat(504, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterCount2 = new Stat(506, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterCount3 = new Stat(508, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterID3 = new Stat(507, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterId1 = new Stat(503, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterId2 = new Stat(505, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtKilledMonsters = new Stat(499, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat qtNumMonsters = new Stat(498, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questAsMaximumRange = new Stat(802, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questAsMinimumRange = new Stat(801, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questBoothDifficulty = new Stat(800, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questIndex0 = new Stat(509, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questIndex1 = new Stat(492, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questIndex2 = new Stat(493, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questIndex3 = new Stat(494, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questIndex4 = new Stat(495, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questIndex5 = new Stat(496, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questInstance = new Stat(491, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questLevelsSolved = new Stat(253, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questStat = new Stat(261, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat questTimeout = new Stat(510, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat race = new Stat(89, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat radiationAC = new Stat(94, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat radiationDamageModifier = new Stat(282, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rangeIncreaserNF = new Stat(381, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rangeIncreaserWeapon = new Stat(380, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat readOnly = new Stat(435, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rechargeDelay = new Stat(210, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rechargeDelayCap = new Stat(524, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reclaimItem = new Stat(365, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectChemicalAC = new Stat(208, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectColdAC = new Stat(217, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectEnergyAC = new Stat(207, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectFireAC = new Stat(219, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectMeleeAC = new Stat(206, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectNanoAC = new Stat(218, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectPoisonAC = new Stat(225, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectProjectileAC = new Stat(205, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectRadiationAC = new Stat(216, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedChemicalAC = new Stat(478, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedColdAC = new Stat(480, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedEnergyAC = new Stat(477, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedFireAC = new Stat(482, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedMeleeAC = new Stat(476, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedNanoAC = new Stat(481, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedPoisonAC = new Stat(483, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedProjectileAC = new Stat(475, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedRadiationAC = new Stat(479, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat regainXPPercentage = new Stat(593, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat repairDifficulty = new Stat(73, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat repairSkill = new Stat(77, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat resistModifier = new Stat(393, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat restModifier = new Stat(425, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat resurrectDest = new Stat(362, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rifle = new Stat(113, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat riposte = new Stat(143, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat ritualTargetInst = new Stat(370, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rnd = new Stat(520, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rotation = new Stat(400, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat rp = new Stat(199, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat runSpeed = new Stat(156, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat savedXP = new Stat(334, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat school = new Stat(405, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat secondaryItemInstance = new Stat(83, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat secondaryItemTemplate = new Stat(273, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat secondaryItemType = new Stat(82, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat selectedTarget = new Stat(431, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat selectedTargetType = new Stat(397, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sellModifier = new Stat(427, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sense = new Stat(20, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat senseImprovement = new Stat(122, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sessionTime = new Stat(198, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sex = new Stat(59, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shadowBreed = new Stat(532, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shadowBreedTemplate = new Stat(579, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldChemicalAC = new Stat(229, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldColdAC = new Stat(231, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldEnergyAC = new Stat(228, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldFireAC = new Stat(233, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldMeleeAC = new Stat(227, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldNanoAC = new Stat(232, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldPoisonAC = new Stat(234, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldProjectileAC = new Stat(226, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shieldRadiationAC = new Stat(230, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopFlags = new Stat(610, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopId = new Stat(657, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopIndex = new Stat(656, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopLastUsed = new Stat(611, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopPrice = new Stat(599, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopRent = new Stat(608, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shopType = new Stat(612, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shotgun = new Stat(115, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shoulderMeshHolder = new Stat(39, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shoulderMeshLeft = new Stat(1005, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat shoulderMeshRight = new Stat(1004, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat side = new Stat(33, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sisCap = new Stat(352, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sk = new Stat(573, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat skillDisabled = new Stat(329, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat skillLockModifier = new Stat(382, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat skillTimeOnSelectedTarget = new Stat(371, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat sneakAttack = new Stat(146, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat socialStatus = new Stat(521, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat soundVolume = new Stat(250, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat specialAttackShield = new Stat(517, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat specialCondition = new Stat(348, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat specialization = new Stat(182, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat speedPenalty = new Stat(70, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stability = new Stat(202, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine2 = new Stat(546, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine3 = new Stat(547, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine4 = new Stat(548, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine5 = new Stat(549, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine6 = new Stat(550, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stackingOrder = new Stat(551, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stamina = new Stat(18, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat statOne = new Stat(290, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat state = new Stat(7, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stateAction = new Stat(98, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat stateMachine = new Stat(450, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat staticInstance = new Stat(23, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat staticType = new Stat(25, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat streamCheckMagic = new Stat(999, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat strength = new Stat(16, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat subMachineGun = new Stat(114, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat swim = new Stat(138, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat synergyHash = new Stat(609, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat taboo = new Stat(327, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat targetDistance = new Stat(527, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat targetDistanceChange = new Stat(889, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat targetFacing = new Stat(488, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat team = new Stat(6, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat teamAllowed = new Stat(324, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat teamCloseness = new Stat(528, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat teamSide = new Stat(213, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat teleportPauseMilliSeconds = new Stat(351, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tempSavePlayfield = new Stat(595, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tempSaveTeamId = new Stat(594, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tempSaveX = new Stat(596, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tempSaveY = new Stat(597, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat temporarySkillReduction = new Stat(247, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat throwingKnife = new Stat(108, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat thrownGrapplingWeapons = new Stat(110, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tideRequiredDynelId = new Stat(900, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat timeExist = new Stat(8, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat timeSinceCreation = new Stat(56, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat timeSinceUpkeep = new Stat(313, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat titleLevel = new Stat(37, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat totalDamage = new Stat(629, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat totalMass = new Stat(71, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat towerInstance = new Stat(515, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat towerNpcHash = new Stat(511, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat towerType = new Stat(388, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tracerEffectType = new Stat(419, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackChemicalDamage = new Stat(633, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackColdDamage = new Stat(635, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackEnergyDamage = new Stat(632, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackFireDamage = new Stat(637, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackMeleeDamage = new Stat(631, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackPoisonDamage = new Stat(636, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackProjectileDamage = new Stat(630, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trackRadiationDamage = new Stat(634, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tradeLimit = new Stat(346, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trainSkill = new Stat(408, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trainSkillCost = new Stat(409, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat trapDifficulty = new Stat(289, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat travelSound = new Stat(271, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat treatment = new Stat(124, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat turnSpeed = new Stat(267, 40000, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat tutoring = new Stat(141, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat twohBluntWeapons = new Stat(107, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat twohEdgedWeapons = new Stat(105, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat unarmedTemplateInstance = new Stat(418, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat unreadMailCount = new Stat(649, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat unsavedXP = new Stat(592, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat userInstance = new Stat(85, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat userType = new Stat(84, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat vehicleAC = new Stat(664, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat vehicleDamage = new Stat(665, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat vehicleHealth = new Stat(666, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat vehicleSpeed = new Stat(667, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat veteranPoints = new Stat(68, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat vicinityRange = new Stat(448, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat victoryPoints = new Stat(669, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat visualBreed = new Stat(367, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat visualFlags = new Stat(673, 31, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat visualLodLevel = new Stat(888, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat visualProfession = new Stat(368, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat visualSex = new Stat(369, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly Stat volumeMass = new Stat(2, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat voteCount = new Stat(306, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat waitState = new Stat(430, 2, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponDisallowedInstance = new Stat(326, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponDisallowedType = new Stat(325, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponMeshHolder = new Stat(209, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponMeshLeft = new Stat(1007, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponMeshRight = new Stat(1006, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponSmithing = new Stat(158, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponStyleLeft = new Stat(1015, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponStyleRight = new Stat(1016, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat weaponsStyle = new Stat(1003, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat xp = new Stat(52, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat xpBonus = new Stat(341, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat xpKillRange = new Stat(275, 5, false, false, false);

        /// <summary>
        /// </summary>
        private readonly Stat xpModifier = new Stat(319, 0, false, false, false);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Character_Stats
        /// Class for character's stats
        /// </summary>
        /// <param name="parent">
        /// Stat's owner (Character or derived class)
        /// </param>
        public Stats(Identity owner)
        {
            this.owner = owner;
            this.all.Add(this.flags);
            this.all.Add(this.life);
            this.all.Add(this.volumeMass);
            this.all.Add(this.attackSpeed);
            this.all.Add(this.breed);
            this.all.Add(this.clan);
            this.all.Add(this.team);
            this.all.Add(this.state);
            this.all.Add(this.timeExist);
            this.all.Add(this.mapFlags);
            this.all.Add(this.professionLevel);
            this.all.Add(this.previousHealth);
            this.all.Add(this.mesh);
            this.all.Add(this.anim);
            this.all.Add(this.name);
            this.all.Add(this.info);
            this.all.Add(this.strength);
            this.all.Add(this.agility);
            this.all.Add(this.stamina);
            this.all.Add(this.intelligence);
            this.all.Add(this.sense);
            this.all.Add(this.psychic);
            this.all.Add(this.ams);
            this.all.Add(this.staticInstance);
            this.all.Add(this.maxMass);
            this.all.Add(this.staticType);
            this.all.Add(this.energy);
            this.all.Add(this.health);
            this.all.Add(this.height);
            this.all.Add(this.dms);
            this.all.Add(this.can);
            this.all.Add(this.face);
            this.all.Add(this.hairMesh);
            this.all.Add(this.side);
            this.all.Add(this.deadTimer);
            this.all.Add(this.accessCount);
            this.all.Add(this.attackCount);
            this.all.Add(this.titleLevel);
            this.all.Add(this.backMesh);
            this.all.Add(this.alienXP);
            this.all.Add(this.fabricType);
            this.all.Add(this.catMesh);
            this.all.Add(this.parentType);
            this.all.Add(this.parentInstance);
            this.all.Add(this.beltSlots);
            this.all.Add(this.bandolierSlots);
            this.all.Add(this.fatness);
            this.all.Add(this.clanLevel);
            this.all.Add(this.insuranceTime);
            this.all.Add(this.inventoryTimeout);
            this.all.Add(this.aggDef);
            this.all.Add(this.xp);
            this.all.Add(this.ip);
            this.all.Add(this.level);
            this.all.Add(this.inventoryId);
            this.all.Add(this.timeSinceCreation);
            this.all.Add(this.lastXP);
            this.all.Add(this.age);
            this.all.Add(this.sex);
            this.all.Add(this.profession);
            this.all.Add(this.cash);
            this.all.Add(this.alignment);
            this.all.Add(this.attitude);
            this.all.Add(this.headMesh);
            this.all.Add(this.missionBits5);
            this.all.Add(this.missionBits6);
            this.all.Add(this.missionBits7);
            this.all.Add(this.veteranPoints);
            this.all.Add(this.monthsPaid);
            this.all.Add(this.speedPenalty);
            this.all.Add(this.totalMass);
            this.all.Add(this.itemType);
            this.all.Add(this.repairDifficulty);
            this.all.Add(this.price);
            this.all.Add(this.metaType);
            this.all.Add(this.itemClass);
            this.all.Add(this.repairSkill);
            this.all.Add(this.currentMass);
            this.all.Add(this.icon);
            this.all.Add(this.primaryItemType);
            this.all.Add(this.primaryItemInstance);
            this.all.Add(this.secondaryItemType);
            this.all.Add(this.secondaryItemInstance);
            this.all.Add(this.userType);
            this.all.Add(this.userInstance);
            this.all.Add(this.areaType);
            this.all.Add(this.areaInstance);
            this.all.Add(this.defaultPos);
            this.all.Add(this.race);
            this.all.Add(this.projectileAC);
            this.all.Add(this.meleeAC);
            this.all.Add(this.energyAC);
            this.all.Add(this.chemicalAC);
            this.all.Add(this.radiationAC);
            this.all.Add(this.coldAC);
            this.all.Add(this.poisonAC);
            this.all.Add(this.fireAC);
            this.all.Add(this.stateAction);
            this.all.Add(this.itemAnim);
            this.all.Add(this.martialArts);
            this.all.Add(this.meleeMultiple);
            this.all.Add(this.onehBluntWeapons);
            this.all.Add(this.onehEdgedWeapon);
            this.all.Add(this.meleeEnergyWeapon);
            this.all.Add(this.twohEdgedWeapons);
            this.all.Add(this.piercing);
            this.all.Add(this.twohBluntWeapons);
            this.all.Add(this.throwingKnife);
            this.all.Add(this.grenade);
            this.all.Add(this.thrownGrapplingWeapons);
            this.all.Add(this.bow);
            this.all.Add(this.pistol);
            this.all.Add(this.rifle);
            this.all.Add(this.subMachineGun);
            this.all.Add(this.shotgun);
            this.all.Add(this.assaultRifle);
            this.all.Add(this.driveWater);
            this.all.Add(this.closeCombatInitiative);
            this.all.Add(this.distanceWeaponInitiative);
            this.all.Add(this.physicalProwessInitiative);
            this.all.Add(this.bowSpecialAttack);
            this.all.Add(this.senseImprovement);
            this.all.Add(this.firstAid);
            this.all.Add(this.treatment);
            this.all.Add(this.mechanicalEngineering);
            this.all.Add(this.electricalEngineering);
            this.all.Add(this.materialMetamorphose);
            this.all.Add(this.biologicalMetamorphose);
            this.all.Add(this.psychologicalModification);
            this.all.Add(this.materialCreation);
            this.all.Add(this.materialLocation);
            this.all.Add(this.nanoEnergyPool);
            this.all.Add(this.lrEnergyWeapon);
            this.all.Add(this.lrMultipleWeapon);
            this.all.Add(this.disarmTrap);
            this.all.Add(this.perception);
            this.all.Add(this.adventuring);
            this.all.Add(this.swim);
            this.all.Add(this.driveAir);
            this.all.Add(this.mapNavigation);
            this.all.Add(this.tutoring);
            this.all.Add(this.brawl);
            this.all.Add(this.riposte);
            this.all.Add(this.dimach);
            this.all.Add(this.parry);
            this.all.Add(this.sneakAttack);
            this.all.Add(this.fastAttack);
            this.all.Add(this.burst);
            this.all.Add(this.nanoProwessInitiative);
            this.all.Add(this.flingShot);
            this.all.Add(this.aimedShot);
            this.all.Add(this.bodyDevelopment);
            this.all.Add(this.duck);
            this.all.Add(this.dodge);
            this.all.Add(this.evade);
            this.all.Add(this.runSpeed);
            this.all.Add(this.fieldQuantumPhysics);
            this.all.Add(this.weaponSmithing);
            this.all.Add(this.pharmaceuticals);
            this.all.Add(this.nanoProgramming);
            this.all.Add(this.computerLiteracy);
            this.all.Add(this.psychology);
            this.all.Add(this.chemistry);
            this.all.Add(this.concealment);
            this.all.Add(this.breakingEntry);
            this.all.Add(this.driveGround);
            this.all.Add(this.fullAuto);
            this.all.Add(this.nanoAC);
            this.all.Add(this.alienLevel);
            this.all.Add(this.healthChangeBest);
            this.all.Add(this.healthChangeWorst);
            this.all.Add(this.healthChange);
            this.all.Add(this.currentMovementMode);
            this.all.Add(this.prevMovementMode);
            this.all.Add(this.autoLockTimeDefault);
            this.all.Add(this.autoUnlockTimeDefault);
            this.all.Add(this.moreFlags);
            this.all.Add(this.alienNextXP);
            this.all.Add(this.npcFlags);
            this.all.Add(this.currentNCU);
            this.all.Add(this.maxNCU);
            this.all.Add(this.specialization);
            this.all.Add(this.effectIcon);
            this.all.Add(this.buildingType);
            this.all.Add(this.buildingInstance);
            this.all.Add(this.cardOwnerType);
            this.all.Add(this.cardOwnerInstance);
            this.all.Add(this.buildingComplexInst);
            this.all.Add(this.exitInstance);
            this.all.Add(this.nextDoorInBuilding);
            this.all.Add(this.lastConcretePlayfieldInstance);
            this.all.Add(this.extenalPlayfieldInstance);
            this.all.Add(this.extenalDoorInstance);
            this.all.Add(this.inPlay);
            this.all.Add(this.accessKey);
            this.all.Add(this.petMaster);
            this.all.Add(this.orientationMode);
            this.all.Add(this.sessionTime);
            this.all.Add(this.rp);
            this.all.Add(this.conformity);
            this.all.Add(this.aggressiveness);
            this.all.Add(this.stability);
            this.all.Add(this.extroverty);
            this.all.Add(this.breedHostility);
            this.all.Add(this.reflectProjectileAC);
            this.all.Add(this.reflectMeleeAC);
            this.all.Add(this.reflectEnergyAC);
            this.all.Add(this.reflectChemicalAC);
            this.all.Add(this.rechargeDelay);
            this.all.Add(this.equipDelay);
            this.all.Add(this.maxEnergy);
            this.all.Add(this.teamSide);
            this.all.Add(this.currentNano);
            this.all.Add(this.gmLevel);
            this.all.Add(this.reflectRadiationAC);
            this.all.Add(this.reflectColdAC);
            this.all.Add(this.reflectNanoAC);
            this.all.Add(this.reflectFireAC);
            this.all.Add(this.currBodyLocation);
            this.all.Add(this.maxNanoEnergy);
            this.all.Add(this.accumulatedDamage);
            this.all.Add(this.canChangeClothes);
            this.all.Add(this.features);
            this.all.Add(this.reflectPoisonAC);
            this.all.Add(this.shieldProjectileAC);
            this.all.Add(this.shieldMeleeAC);
            this.all.Add(this.shieldEnergyAC);
            this.all.Add(this.shieldChemicalAC);
            this.all.Add(this.shieldRadiationAC);
            this.all.Add(this.shieldColdAC);
            this.all.Add(this.shieldNanoAC);
            this.all.Add(this.shieldFireAC);
            this.all.Add(this.shieldPoisonAC);
            this.all.Add(this.berserkMode);
            this.all.Add(this.insurancePercentage);
            this.all.Add(this.changeSideCount);
            this.all.Add(this.absorbProjectileAC);
            this.all.Add(this.absorbMeleeAC);
            this.all.Add(this.absorbEnergyAC);
            this.all.Add(this.absorbChemicalAC);
            this.all.Add(this.absorbRadiationAC);
            this.all.Add(this.absorbColdAC);
            this.all.Add(this.absorbFireAC);
            this.all.Add(this.absorbPoisonAC);
            this.all.Add(this.absorbNanoAC);
            this.all.Add(this.temporarySkillReduction);
            this.all.Add(this.birthDate);
            this.all.Add(this.lastSaved);
            this.all.Add(this.soundVolume);
            this.all.Add(this.petCounter);
            this.all.Add(this.metersWalked);
            this.all.Add(this.questLevelsSolved);
            this.all.Add(this.monsterLevelsKilled);
            this.all.Add(this.pvPLevelsKilled);
            this.all.Add(this.missionBits1);
            this.all.Add(this.missionBits2);
            this.all.Add(this.accessGrant);
            this.all.Add(this.doorFlags);
            this.all.Add(this.clanHierarchy);
            this.all.Add(this.questStat);
            this.all.Add(this.clientActivated);
            this.all.Add(this.personalResearchLevel);
            this.all.Add(this.globalResearchLevel);
            this.all.Add(this.personalResearchGoal);
            this.all.Add(this.globalResearchGoal);
            this.all.Add(this.turnSpeed);
            this.all.Add(this.liquidType);
            this.all.Add(this.gatherSound);
            this.all.Add(this.castSound);
            this.all.Add(this.travelSound);
            this.all.Add(this.hitSound);
            this.all.Add(this.secondaryItemTemplate);
            this.all.Add(this.equippedWeapons);
            this.all.Add(this.xpKillRange);
            this.all.Add(this.amsModifier);
            this.all.Add(this.dmsModifier);
            this.all.Add(this.projectileDamageModifier);
            this.all.Add(this.meleeDamageModifier);
            this.all.Add(this.energyDamageModifier);
            this.all.Add(this.chemicalDamageModifier);
            this.all.Add(this.radiationDamageModifier);
            this.all.Add(this.itemHateValue);
            this.all.Add(this.damageBonus);
            this.all.Add(this.maxDamage);
            this.all.Add(this.minDamage);
            this.all.Add(this.attackRange);
            this.all.Add(this.hateValueModifyer);
            this.all.Add(this.trapDifficulty);
            this.all.Add(this.statOne);
            this.all.Add(this.numAttackEffects);
            this.all.Add(this.defaultAttackType);
            this.all.Add(this.itemSkill);
            this.all.Add(this.itemDelay);
            this.all.Add(this.itemOpposedSkill);
            this.all.Add(this.itemSIS);
            this.all.Add(this.interactionRadius);
            this.all.Add(this.placement);
            this.all.Add(this.lockDifficulty);
            this.all.Add(this.members);
            this.all.Add(this.minMembers);
            this.all.Add(this.clanPrice);
            this.all.Add(this.missionBits3);
            this.all.Add(this.clanType);
            this.all.Add(this.clanInstance);
            this.all.Add(this.voteCount);
            this.all.Add(this.memberType);
            this.all.Add(this.memberInstance);
            this.all.Add(this.globalClanType);
            this.all.Add(this.globalClanInstance);
            this.all.Add(this.coldDamageModifier);
            this.all.Add(this.clanUpkeepInterval);
            this.all.Add(this.timeSinceUpkeep);
            this.all.Add(this.clanFinalized);
            this.all.Add(this.nanoDamageModifier);
            this.all.Add(this.fireDamageModifier);
            this.all.Add(this.poisonDamageModifier);
            this.all.Add(this.npCostModifier);
            this.all.Add(this.xpModifier);
            this.all.Add(this.breedLimit);
            this.all.Add(this.genderLimit);
            this.all.Add(this.levelLimit);
            this.all.Add(this.playerKilling);
            this.all.Add(this.teamAllowed);
            this.all.Add(this.weaponDisallowedType);
            this.all.Add(this.weaponDisallowedInstance);
            this.all.Add(this.taboo);
            this.all.Add(this.compulsion);
            this.all.Add(this.skillDisabled);
            this.all.Add(this.clanItemType);
            this.all.Add(this.clanItemInstance);
            this.all.Add(this.debuffFormula);
            this.all.Add(this.pvpRating);
            this.all.Add(this.savedXP);
            this.all.Add(this.doorBlockTime);
            this.all.Add(this.overrideTexture);
            this.all.Add(this.overrideMaterial);
            this.all.Add(this.deathReason);
            this.all.Add(this.damageOverrideType);
            this.all.Add(this.brainType);
            this.all.Add(this.xpBonus);
            this.all.Add(this.healInterval);
            this.all.Add(this.healDelta);
            this.all.Add(this.monsterTexture);
            this.all.Add(this.hasAlwaysLootable);
            this.all.Add(this.tradeLimit);
            this.all.Add(this.faceTexture);
            this.all.Add(this.specialCondition);
            this.all.Add(this.autoAttackFlags);
            this.all.Add(this.nextXP);
            this.all.Add(this.teleportPauseMilliSeconds);
            this.all.Add(this.sisCap);
            this.all.Add(this.animSet);
            this.all.Add(this.attackType);
            this.all.Add(this.nanoFocusLevel);
            this.all.Add(this.npcHash);
            this.all.Add(this.collisionRadius);
            this.all.Add(this.outerRadius);
            this.all.Add(this.monsterData);
            this.all.Add(this.monsterScale);
            this.all.Add(this.hitEffectType);
            this.all.Add(this.resurrectDest);
            this.all.Add(this.nanoInterval);
            this.all.Add(this.nanoDelta);
            this.all.Add(this.reclaimItem);
            this.all.Add(this.gatherEffectType);
            this.all.Add(this.visualBreed);
            this.all.Add(this.visualProfession);
            this.all.Add(this.visualSex);
            this.all.Add(this.ritualTargetInst);
            this.all.Add(this.skillTimeOnSelectedTarget);
            this.all.Add(this.lastSaveXP);
            this.all.Add(this.extendedTime);
            this.all.Add(this.burstRecharge);
            this.all.Add(this.fullAutoRecharge);
            this.all.Add(this.gatherAbstractAnim);
            this.all.Add(this.castTargetAbstractAnim);
            this.all.Add(this.castSelfAbstractAnim);
            this.all.Add(this.criticalIncrease);
            this.all.Add(this.rangeIncreaserWeapon);
            this.all.Add(this.rangeIncreaserNF);
            this.all.Add(this.skillLockModifier);
            this.all.Add(this.interruptModifier);
            this.all.Add(this.acgEntranceStyles);
            this.all.Add(this.chanceOfBreakOnSpellAttack);
            this.all.Add(this.chanceOfBreakOnDebuff);
            this.all.Add(this.dieAnim);
            this.all.Add(this.towerType);
            this.all.Add(this.expansion);
            this.all.Add(this.lowresMesh);
            this.all.Add(this.criticalDecrease);
            this.all.Add(this.oldTimeExist);
            this.all.Add(this.resistModifier);
            this.all.Add(this.chestFlags);
            this.all.Add(this.primaryTemplateId);
            this.all.Add(this.numberOfItems);
            this.all.Add(this.selectedTargetType);
            this.all.Add(this.corpseHash);
            this.all.Add(this.ammoName);
            this.all.Add(this.rotation);
            this.all.Add(this.catAnim);
            this.all.Add(this.catAnimFlags);
            this.all.Add(this.displayCATAnim);
            this.all.Add(this.displayCATMesh);
            this.all.Add(this.school);
            this.all.Add(this.nanoSpeed);
            this.all.Add(this.nanoPoints);
            this.all.Add(this.trainSkill);
            this.all.Add(this.trainSkillCost);
            this.all.Add(this.isFightingMe);
            this.all.Add(this.nextFormula);
            this.all.Add(this.multipleCount);
            this.all.Add(this.effectType);
            this.all.Add(this.impactEffectType);
            this.all.Add(this.corpseType);
            this.all.Add(this.corpseInstance);
            this.all.Add(this.corpseAnimKey);
            this.all.Add(this.unarmedTemplateInstance);
            this.all.Add(this.tracerEffectType);
            this.all.Add(this.ammoType);
            this.all.Add(this.charRadius);
            this.all.Add(this.chanceOfUse);
            this.all.Add(this.currentState);
            this.all.Add(this.armourType);
            this.all.Add(this.restModifier);
            this.all.Add(this.buyModifier);
            this.all.Add(this.sellModifier);
            this.all.Add(this.castEffectType);
            this.all.Add(this.npcBrainState);
            this.all.Add(this.waitState);
            this.all.Add(this.selectedTarget);
            this.all.Add(this.missionBits4);
            this.all.Add(this.ownerInstance);
            this.all.Add(this.charState);
            this.all.Add(this.readOnly);
            this.all.Add(this.damageType);
            this.all.Add(this.collideCheckInterval);
            this.all.Add(this.playfieldType);
            this.all.Add(this.npcCommand);
            this.all.Add(this.initiativeType);
            this.all.Add(this.charTmp1);
            this.all.Add(this.charTmp2);
            this.all.Add(this.charTmp3);
            this.all.Add(this.charTmp4);
            this.all.Add(this.npcCommandArg);
            this.all.Add(this.nameTemplate);
            this.all.Add(this.desiredTargetDistance);
            this.all.Add(this.vicinityRange);
            this.all.Add(this.npcIsSurrendering);
            this.all.Add(this.stateMachine);
            this.all.Add(this.npcSurrenderInstance);
            this.all.Add(this.npcHasPatrolList);
            this.all.Add(this.npcVicinityChars);
            this.all.Add(this.proximityRangeOutdoors);
            this.all.Add(this.npcFamily);
            this.all.Add(this.commandRange);
            this.all.Add(this.npcHatelistSize);
            this.all.Add(this.npcNumPets);
            this.all.Add(this.odMinSizeAdd);
            this.all.Add(this.effectRed);
            this.all.Add(this.effectGreen);
            this.all.Add(this.effectBlue);
            this.all.Add(this.odMaxSizeAdd);
            this.all.Add(this.durationModifier);
            this.all.Add(this.npcCryForHelpRange);
            this.all.Add(this.losHeight);
            this.all.Add(this.petReq1);
            this.all.Add(this.petReq2);
            this.all.Add(this.petReq3);
            this.all.Add(this.mapOptions);
            this.all.Add(this.mapAreaPart1);
            this.all.Add(this.mapAreaPart2);
            this.all.Add(this.fixtureFlags);
            this.all.Add(this.fallDamage);
            this.all.Add(this.reflectReturnedProjectileAC);
            this.all.Add(this.reflectReturnedMeleeAC);
            this.all.Add(this.reflectReturnedEnergyAC);
            this.all.Add(this.reflectReturnedChemicalAC);
            this.all.Add(this.reflectReturnedRadiationAC);
            this.all.Add(this.reflectReturnedColdAC);
            this.all.Add(this.reflectReturnedNanoAC);
            this.all.Add(this.reflectReturnedFireAC);
            this.all.Add(this.reflectReturnedPoisonAC);
            this.all.Add(this.proximityRangeIndoors);
            this.all.Add(this.petReqVal1);
            this.all.Add(this.petReqVal2);
            this.all.Add(this.petReqVal3);
            this.all.Add(this.targetFacing);
            this.all.Add(this.backstab);
            this.all.Add(this.originatorType);
            this.all.Add(this.questInstance);
            this.all.Add(this.questIndex1);
            this.all.Add(this.questIndex2);
            this.all.Add(this.questIndex3);
            this.all.Add(this.questIndex4);
            this.all.Add(this.questIndex5);
            this.all.Add(this.qtDungeonInstance);
            this.all.Add(this.qtNumMonsters);
            this.all.Add(this.qtKilledMonsters);
            this.all.Add(this.animPos);
            this.all.Add(this.animPlay);
            this.all.Add(this.animSpeed);
            this.all.Add(this.qtKillNumMonsterId1);
            this.all.Add(this.qtKillNumMonsterCount1);
            this.all.Add(this.qtKillNumMonsterId2);
            this.all.Add(this.qtKillNumMonsterCount2);
            this.all.Add(this.qtKillNumMonsterID3);
            this.all.Add(this.qtKillNumMonsterCount3);
            this.all.Add(this.questIndex0);
            this.all.Add(this.questTimeout);
            this.all.Add(this.towerNpcHash);
            this.all.Add(this.petType);
            this.all.Add(this.onTowerCreation);
            this.all.Add(this.ownedTowers);
            this.all.Add(this.towerInstance);
            this.all.Add(this.attackShield);
            this.all.Add(this.specialAttackShield);
            this.all.Add(this.npcVicinityPlayers);
            this.all.Add(this.npcUseFightModeRegenRate);
            this.all.Add(this.rnd);
            this.all.Add(this.socialStatus);
            this.all.Add(this.lastRnd);
            this.all.Add(this.itemDelayCap);
            this.all.Add(this.rechargeDelayCap);
            this.all.Add(this.percentRemainingHealth);
            this.all.Add(this.percentRemainingNano);
            this.all.Add(this.targetDistance);
            this.all.Add(this.teamCloseness);
            this.all.Add(this.numberOnHateList);
            this.all.Add(this.conditionState);
            this.all.Add(this.expansionPlayfield);
            this.all.Add(this.shadowBreed);
            this.all.Add(this.npcFovStatus);
            this.all.Add(this.dudChance);
            this.all.Add(this.healMultiplier);
            this.all.Add(this.nanoDamageMultiplier);
            this.all.Add(this.nanoVulnerability);
            this.all.Add(this.amsCap);
            this.all.Add(this.procInitiative1);
            this.all.Add(this.procInitiative2);
            this.all.Add(this.procInitiative3);
            this.all.Add(this.procInitiative4);
            this.all.Add(this.factionModifier);
            this.all.Add(this.missionBits8);
            this.all.Add(this.missionBits9);
            this.all.Add(this.stackingLine2);
            this.all.Add(this.stackingLine3);
            this.all.Add(this.stackingLine4);
            this.all.Add(this.stackingLine5);
            this.all.Add(this.stackingLine6);
            this.all.Add(this.stackingOrder);
            this.all.Add(this.procNano1);
            this.all.Add(this.procNano2);
            this.all.Add(this.procNano3);
            this.all.Add(this.procNano4);
            this.all.Add(this.procChance1);
            this.all.Add(this.procChance2);
            this.all.Add(this.procChance3);
            this.all.Add(this.procChance4);
            this.all.Add(this.otArmedForces);
            this.all.Add(this.clanSentinels);
            this.all.Add(this.otMed);
            this.all.Add(this.clanGaia);
            this.all.Add(this.otTrans);
            this.all.Add(this.clanVanguards);
            this.all.Add(this.gos);
            this.all.Add(this.otFollowers);
            this.all.Add(this.otOperator);
            this.all.Add(this.otUnredeemed);
            this.all.Add(this.clanDevoted);
            this.all.Add(this.clanConserver);
            this.all.Add(this.clanRedeemed);
            this.all.Add(this.sk);
            this.all.Add(this.lastSK);
            this.all.Add(this.nextSK);
            this.all.Add(this.playerOptions);
            this.all.Add(this.lastPerkResetTime);
            this.all.Add(this.currentTime);
            this.all.Add(this.shadowBreedTemplate);
            this.all.Add(this.npcVicinityFamily);
            this.all.Add(this.npcScriptAmsScale);
            this.all.Add(this.apartmentsAllowed);
            this.all.Add(this.apartmentsOwned);
            this.all.Add(this.apartmentAccessCard);
            this.all.Add(this.mapAreaPart3);
            this.all.Add(this.mapAreaPart4);
            this.all.Add(this.numberOfTeamMembers);
            this.all.Add(this.actionCategory);
            this.all.Add(this.currentPlayfield);
            this.all.Add(this.districtNano);
            this.all.Add(this.districtNanoInterval);
            this.all.Add(this.unsavedXP);
            this.all.Add(this.regainXPPercentage);
            this.all.Add(this.tempSaveTeamId);
            this.all.Add(this.tempSavePlayfield);
            this.all.Add(this.tempSaveX);
            this.all.Add(this.tempSaveY);
            this.all.Add(this.extendedFlags);
            this.all.Add(this.shopPrice);
            this.all.Add(this.newbieHP);
            this.all.Add(this.hpLevelUp);
            this.all.Add(this.hpPerSkill);
            this.all.Add(this.newbieNP);
            this.all.Add(this.npLevelUp);
            this.all.Add(this.npPerSkill);
            this.all.Add(this.maxShopItems);
            this.all.Add(this.playerId);
            this.all.Add(this.shopRent);
            this.all.Add(this.synergyHash);
            this.all.Add(this.shopFlags);
            this.all.Add(this.shopLastUsed);
            this.all.Add(this.shopType);
            this.all.Add(this.lockDownTime);
            this.all.Add(this.leaderLockDownTime);
            this.all.Add(this.invadersKilled);
            this.all.Add(this.killedByInvaders);
            this.all.Add(this.missionBits10);
            this.all.Add(this.missionBits11);
            this.all.Add(this.missionBits12);
            this.all.Add(this.houseTemplate);
            this.all.Add(this.percentFireDamage);
            this.all.Add(this.percentColdDamage);
            this.all.Add(this.percentMeleeDamage);
            this.all.Add(this.percentProjectileDamage);
            this.all.Add(this.percentPoisonDamage);
            this.all.Add(this.percentRadiationDamage);
            this.all.Add(this.percentEnergyDamage);
            this.all.Add(this.percentChemicalDamage);
            this.all.Add(this.totalDamage);
            this.all.Add(this.trackProjectileDamage);
            this.all.Add(this.trackMeleeDamage);
            this.all.Add(this.trackEnergyDamage);
            this.all.Add(this.trackChemicalDamage);
            this.all.Add(this.trackRadiationDamage);
            this.all.Add(this.trackColdDamage);
            this.all.Add(this.trackPoisonDamage);
            this.all.Add(this.trackFireDamage);
            this.all.Add(this.npcSpellArg1);
            this.all.Add(this.npcSpellRet1);
            this.all.Add(this.cityInstance);
            this.all.Add(this.distanceToSpawnpoint);
            this.all.Add(this.cityTerminalRechargePercent);
            this.all.Add(this.unreadMailCount);
            this.all.Add(this.lastMailCheckTime);
            this.all.Add(this.advantageHash1);
            this.all.Add(this.advantageHash2);
            this.all.Add(this.advantageHash3);
            this.all.Add(this.advantageHash4);
            this.all.Add(this.advantageHash5);
            this.all.Add(this.shopIndex);
            this.all.Add(this.shopId);
            this.all.Add(this.isVehicle);
            this.all.Add(this.damageToNano);
            this.all.Add(this.accountFlags);
            this.all.Add(this.damageToNanoMultiplier);
            this.all.Add(this.mechData);
            this.all.Add(this.vehicleAC);
            this.all.Add(this.vehicleDamage);
            this.all.Add(this.vehicleHealth);
            this.all.Add(this.vehicleSpeed);
            this.all.Add(this.battlestationSide);
            this.all.Add(this.victoryPoints);
            this.all.Add(this.battlestationRep);
            this.all.Add(this.petState);
            this.all.Add(this.paidPoints);
            this.all.Add(this.visualFlags);
            this.all.Add(this.pvpDuelKills);
            this.all.Add(this.pvpDuelDeaths);
            this.all.Add(this.pvpProfessionDuelKills);
            this.all.Add(this.pvpProfessionDuelDeaths);
            this.all.Add(this.pvpRankedSoloKills);
            this.all.Add(this.pvpRankedSoloDeaths);
            this.all.Add(this.pvpRankedTeamKills);
            this.all.Add(this.pvpRankedTeamDeaths);
            this.all.Add(this.pvpSoloScore);
            this.all.Add(this.pvpTeamScore);
            this.all.Add(this.pvpDuelScore);
            this.all.Add(this.acgItemSeed);
            this.all.Add(this.acgItemLevel);
            this.all.Add(this.acgItemTemplateId);
            this.all.Add(this.acgItemTemplateId2);
            this.all.Add(this.acgItemCategoryId);
            this.all.Add(this.hasKnuBotData);
            this.all.Add(this.questBoothDifficulty);
            this.all.Add(this.questAsMinimumRange);
            this.all.Add(this.questAsMaximumRange);
            this.all.Add(this.visualLodLevel);
            this.all.Add(this.targetDistanceChange);
            this.all.Add(this.tideRequiredDynelId);
            this.all.Add(this.streamCheckMagic);
            this.all.Add(this.objectType);
            this.all.Add(this.instance);
            this.all.Add(this.weaponsStyle);
            this.all.Add(this.shoulderMeshRight);
            this.all.Add(this.shoulderMeshLeft);
            this.all.Add(this.weaponMeshRight);
            this.all.Add(this.weaponMeshLeft);
            this.all.Add(this.overrideTextureAttractor);
            this.all.Add(this.overrideTextureBack);
            this.all.Add(this.overrideTextureHead);
            this.all.Add(this.overrideTextureShoulderpadLeft);
            this.all.Add(this.overrideTextureShoulderpadRight);
            this.all.Add(this.overrideTextureWeaponLeft);
            this.all.Add(this.overrideTextureWeaponRight);

            // add Tricklers, try not to do circulars!!
            this.SetAbilityTricklers();
            this.bodyDevelopment.Affects.Add(this.life.StatId);
            this.nanoEnergyPool.Affects.Add(this.maxNanoEnergy.StatId);
            this.level.Affects.Add(this.life.StatId);
            this.level.Affects.Add(this.maxNanoEnergy.StatId);
            this.level.Affects.Add(this.titleLevel.StatId);
            this.level.Affects.Add(this.nextSK.StatId);
            this.level.Affects.Add(this.nextXP.StatId);
            this.alienLevel.Affects.Add(this.alienNextXP.StatId);
            this.xp.Affects.Add(this.level.StatId);
            this.sk.Affects.Add(this.level.StatId);
            this.alienXP.Affects.Add(this.alienLevel.StatId);
            this.profession.Affects.Add(this.health.StatId);
            this.profession.Affects.Add(this.maxNanoEnergy.StatId);
            this.profession.Affects.Add(this.ip.StatId);
            this.stamina.Affects.Add(this.healDelta.StatId);
            this.psychic.Affects.Add(this.nanoDelta.StatId);
            this.stamina.Affects.Add(this.healInterval.StatId);
            this.psychic.Affects.Add(this.nanoInterval.StatId);
            this.level.Affects.Add(this.ip.StatId);

            this.expansion.DoNotDontWriteToSql = true;
            this.accountFlags.DoNotDontWriteToSql = true;
            this.playerId.DoNotDontWriteToSql = true;
            this.professionLevel.DoNotDontWriteToSql = true;
            this.objectType.DoNotDontWriteToSql = true;
            this.instance.DoNotDontWriteToSql = true;
            this.gmLevel.DoNotDontWriteToSql = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public Stat AbsorbChemicalAC
        {
            get
            {
                return this.absorbChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbColdAC
        {
            get
            {
                return this.absorbColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbEnergyAC
        {
            get
            {
                return this.absorbEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbFireAC
        {
            get
            {
                return this.absorbFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbMeleeAC
        {
            get
            {
                return this.absorbMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbNanoAC
        {
            get
            {
                return this.absorbNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbPoisonAC
        {
            get
            {
                return this.absorbPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbProjectileAC
        {
            get
            {
                return this.absorbProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AbsorbRadiationAC
        {
            get
            {
                return this.absorbRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AccessCount
        {
            get
            {
                return this.accessCount;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AccessGrant
        {
            get
            {
                return this.accessGrant;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AccessKey
        {
            get
            {
                return this.accessKey;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AccountFlags
        {
            get
            {
                return this.accountFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AccumulatedDamage
        {
            get
            {
                return this.accumulatedDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AcgEntranceStyles
        {
            get
            {
                return this.acgEntranceStyles;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AcgItemCategoryId
        {
            get
            {
                return this.acgItemCategoryId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AcgItemLevel
        {
            get
            {
                return this.acgItemLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AcgItemSeed
        {
            get
            {
                return this.acgItemSeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AcgItemTemplateId
        {
            get
            {
                return this.acgItemTemplateId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AcgItemTemplateId2
        {
            get
            {
                return this.acgItemTemplateId2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ActionCategory
        {
            get
            {
                return this.actionCategory;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AdvantageHash1
        {
            get
            {
                return this.advantageHash1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AdvantageHash2
        {
            get
            {
                return this.advantageHash2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AdvantageHash3
        {
            get
            {
                return this.advantageHash3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AdvantageHash4
        {
            get
            {
                return this.advantageHash4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AdvantageHash5
        {
            get
            {
                return this.advantageHash5;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Adventuring
        {
            get
            {
                return this.adventuring;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Age
        {
            get
            {
                return this.age;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AggDef
        {
            get
            {
                return this.aggDef;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Aggressiveness
        {
            get
            {
                return this.aggressiveness;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Agility
        {
            get
            {
                return this.agility;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AimedShot
        {
            get
            {
                return this.aimedShot;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AlienLevel
        {
            get
            {
                return this.alienLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AlienNextXP
        {
            get
            {
                return this.alienNextXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AlienXP
        {
            get
            {
                return this.alienXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Alignment
        {
            get
            {
                return this.alignment;
            }
        }

        /// <summary>
        /// </summary>
        public List<IStat> All
        {
            get
            {
                return this.all;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AmmoName
        {
            get
            {
                return this.ammoName;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AmmoType
        {
            get
            {
                return this.ammoType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Ams
        {
            get
            {
                return this.ams;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AmsCap
        {
            get
            {
                return this.amsCap;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AmsModifier
        {
            get
            {
                return this.amsModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Anim
        {
            get
            {
                return this.anim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AnimPlay
        {
            get
            {
                return this.animPlay;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AnimPos
        {
            get
            {
                return this.animPos;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AnimSet
        {
            get
            {
                return this.animSet;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AnimSpeed
        {
            get
            {
                return this.animSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ApartmentAccessCard
        {
            get
            {
                return this.apartmentAccessCard;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ApartmentsAllowed
        {
            get
            {
                return this.apartmentsAllowed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ApartmentsOwned
        {
            get
            {
                return this.apartmentsOwned;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AreaInstance
        {
            get
            {
                return this.areaInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AreaType
        {
            get
            {
                return this.areaType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ArmourType
        {
            get
            {
                return this.armourType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AssaultRifle
        {
            get
            {
                return this.assaultRifle;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AttackCount
        {
            get
            {
                return this.attackCount;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AttackRange
        {
            get
            {
                return this.attackRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AttackShield
        {
            get
            {
                return this.attackShield;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AttackSpeed
        {
            get
            {
                return this.attackSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AttackType
        {
            get
            {
                return this.attackType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Attitude
        {
            get
            {
                return this.attitude;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AutoAttackFlags
        {
            get
            {
                return this.autoAttackFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AutoLockTimeDefault
        {
            get
            {
                return this.autoLockTimeDefault;
            }
        }

        /// <summary>
        /// </summary>
        public Stat AutoUnlockTimeDefault
        {
            get
            {
                return this.autoUnlockTimeDefault;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BackMesh
        {
            get
            {
                return this.backMesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Backstab
        {
            get
            {
                return this.backstab;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BandolierSlots
        {
            get
            {
                return this.bandolierSlots;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BattlestationRep
        {
            get
            {
                return this.battlestationRep;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BattlestationSide
        {
            get
            {
                return this.battlestationSide;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BeltSlots
        {
            get
            {
                return this.beltSlots;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BerserkMode
        {
            get
            {
                return this.berserkMode;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BiologicalMetamorphose
        {
            get
            {
                return this.biologicalMetamorphose;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BirthDate
        {
            get
            {
                return this.birthDate;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BodyDevelopment
        {
            get
            {
                return this.bodyDevelopment;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Bow
        {
            get
            {
                return this.bow;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BowSpecialAttack
        {
            get
            {
                return this.bowSpecialAttack;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BrainType
        {
            get
            {
                return this.brainType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Brawl
        {
            get
            {
                return this.brawl;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BreakingEntry
        {
            get
            {
                return this.breakingEntry;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Breed
        {
            get
            {
                return this.breed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BreedHostility
        {
            get
            {
                return this.breedHostility;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BreedLimit
        {
            get
            {
                return this.breedLimit;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BuildingComplexInst
        {
            get
            {
                return this.buildingComplexInst;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BuildingInstance
        {
            get
            {
                return this.buildingInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BuildingType
        {
            get
            {
                return this.buildingType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Burst
        {
            get
            {
                return this.burst;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BurstRecharge
        {
            get
            {
                return this.burstRecharge;
            }
        }

        /// <summary>
        /// </summary>
        public Stat BuyModifier
        {
            get
            {
                return this.buyModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Can
        {
            get
            {
                return this.can;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CanChangeClothes
        {
            get
            {
                return this.canChangeClothes;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CardOwnerInstance
        {
            get
            {
                return this.cardOwnerInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CardOwnerType
        {
            get
            {
                return this.cardOwnerType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Cash
        {
            get
            {
                return this.cash;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CastEffectType
        {
            get
            {
                return this.castEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CastSelfAbstractAnim
        {
            get
            {
                return this.castSelfAbstractAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CastSound
        {
            get
            {
                return this.castSound;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CastTargetAbstractAnim
        {
            get
            {
                return this.castTargetAbstractAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CatAnim
        {
            get
            {
                return this.catAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CatAnimFlags
        {
            get
            {
                return this.catAnimFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CatMesh
        {
            get
            {
                return this.catMesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChanceOfBreakOnDebuff
        {
            get
            {
                return this.chanceOfBreakOnDebuff;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChanceOfBreakOnSpellAttack
        {
            get
            {
                return this.chanceOfBreakOnSpellAttack;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChanceOfUse
        {
            get
            {
                return this.chanceOfUse;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChangeSideCount
        {
            get
            {
                return this.changeSideCount;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CharRadius
        {
            get
            {
                return this.charRadius;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CharState
        {
            get
            {
                return this.charState;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CharTmp1
        {
            get
            {
                return this.charTmp1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CharTmp2
        {
            get
            {
                return this.charTmp2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CharTmp3
        {
            get
            {
                return this.charTmp3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CharTmp4
        {
            get
            {
                return this.charTmp4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChemicalAC
        {
            get
            {
                return this.chemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChemicalDamageModifier
        {
            get
            {
                return this.chemicalDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Chemistry
        {
            get
            {
                return this.chemistry;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ChestFlags
        {
            get
            {
                return this.chestFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CityInstance
        {
            get
            {
                return this.cityInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CityTerminalRechargePercent
        {
            get
            {
                return this.cityTerminalRechargePercent;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Clan
        {
            get
            {
                return this.clan;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanConserver
        {
            get
            {
                return this.clanConserver;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanDevoted
        {
            get
            {
                return this.clanDevoted;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanFinalized
        {
            get
            {
                return this.clanFinalized;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanGaia
        {
            get
            {
                return this.clanGaia;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanHierarchy
        {
            get
            {
                return this.clanHierarchy;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanInstance
        {
            get
            {
                return this.clanInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanItemInstance
        {
            get
            {
                return this.clanItemInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanItemType
        {
            get
            {
                return this.clanItemType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanLevel
        {
            get
            {
                return this.clanLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanPrice
        {
            get
            {
                return this.clanPrice;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanRedeemed
        {
            get
            {
                return this.clanRedeemed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanSentinels
        {
            get
            {
                return this.clanSentinels;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanType
        {
            get
            {
                return this.clanType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanUpkeepInterval
        {
            get
            {
                return this.clanUpkeepInterval;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClanVanguards
        {
            get
            {
                return this.clanVanguards;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ClientActivated
        {
            get
            {
                return this.clientActivated;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CloseCombatInitiative
        {
            get
            {
                return this.closeCombatInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ColdAC
        {
            get
            {
                return this.coldAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ColdDamageModifier
        {
            get
            {
                return this.coldDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CollideCheckInterval
        {
            get
            {
                return this.collideCheckInterval;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CollisionRadius
        {
            get
            {
                return this.collisionRadius;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CommandRange
        {
            get
            {
                return this.commandRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Compulsion
        {
            get
            {
                return this.compulsion;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ComputerLiteracy
        {
            get
            {
                return this.computerLiteracy;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Concealment
        {
            get
            {
                return this.concealment;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ConditionState
        {
            get
            {
                return this.conditionState;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Conformity
        {
            get
            {
                return this.conformity;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CorpseAnimKey
        {
            get
            {
                return this.corpseAnimKey;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CorpseHash
        {
            get
            {
                return this.corpseHash;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CorpseInstance
        {
            get
            {
                return this.corpseInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CorpseType
        {
            get
            {
                return this.corpseType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CriticalDecrease
        {
            get
            {
                return this.criticalDecrease;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CriticalIncrease
        {
            get
            {
                return this.criticalIncrease;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrBodyLocation
        {
            get
            {
                return this.currBodyLocation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentMass
        {
            get
            {
                return this.currentMass;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentMovementMode
        {
            get
            {
                return this.currentMovementMode;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentNano
        {
            get
            {
                return this.currentNano;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentNcu
        {
            get
            {
                return this.currentNCU;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentPlayfield
        {
            get
            {
                return this.currentPlayfield;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentState
        {
            get
            {
                return this.currentState;
            }
        }

        /// <summary>
        /// </summary>
        public Stat CurrentTime
        {
            get
            {
                return this.currentTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DamageBonus
        {
            get
            {
                return this.damageBonus;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DamageOverrideType
        {
            get
            {
                return this.damageOverrideType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DamageToNano
        {
            get
            {
                return this.damageToNano;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DamageToNanoMultiplier
        {
            get
            {
                return this.damageToNanoMultiplier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DamageType
        {
            get
            {
                return this.damageType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DeadTimer
        {
            get
            {
                return this.deadTimer;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DeathReason
        {
            get
            {
                return this.deathReason;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DebuffFormula
        {
            get
            {
                return this.debuffFormula;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DefaultAttackType
        {
            get
            {
                return this.defaultAttackType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DefaultPos
        {
            get
            {
                return this.defaultPos;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DesiredTargetDistance
        {
            get
            {
                return this.desiredTargetDistance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DieAnim
        {
            get
            {
                return this.dieAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Dimach
        {
            get
            {
                return this.dimach;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DisarmTrap
        {
            get
            {
                return this.disarmTrap;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DisplayCatAnim
        {
            get
            {
                return this.displayCATAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DisplayCatMesh
        {
            get
            {
                return this.displayCATMesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DistanceToSpawnpoint
        {
            get
            {
                return this.distanceToSpawnpoint;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DistanceWeaponInitiative
        {
            get
            {
                return this.distanceWeaponInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DistrictNano
        {
            get
            {
                return this.districtNano;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DistrictNanoInterval
        {
            get
            {
                return this.districtNanoInterval;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Dms
        {
            get
            {
                return this.dms;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DmsModifier
        {
            get
            {
                return this.dmsModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Dodge
        {
            get
            {
                return this.dodge;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DoorBlockTime
        {
            get
            {
                return this.doorBlockTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DoorFlags
        {
            get
            {
                return this.doorFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DriveAir
        {
            get
            {
                return this.driveAir;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DriveGround
        {
            get
            {
                return this.driveGround;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DriveWater
        {
            get
            {
                return this.driveWater;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Duck
        {
            get
            {
                return this.duck;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DudChance
        {
            get
            {
                return this.dudChance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat DurationModifier
        {
            get
            {
                return this.durationModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EffectBlue
        {
            get
            {
                return this.effectBlue;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EffectGreen
        {
            get
            {
                return this.effectGreen;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EffectIcon
        {
            get
            {
                return this.effectIcon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EffectRed
        {
            get
            {
                return this.effectRed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EffectType
        {
            get
            {
                return this.effectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ElectricalEngineering
        {
            get
            {
                return this.electricalEngineering;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Energy
        {
            get
            {
                return this.energy;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EnergyAC
        {
            get
            {
                return this.energyAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EnergyDamageModifier
        {
            get
            {
                return this.energyDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EquipDelay
        {
            get
            {
                return this.equipDelay;
            }
        }

        /// <summary>
        /// </summary>
        public Stat EquippedWeapons
        {
            get
            {
                return this.equippedWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Evade
        {
            get
            {
                return this.evade;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ExitInstance
        {
            get
            {
                return this.exitInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Expansion
        {
            get
            {
                return this.expansion;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ExpansionPlayfield
        {
            get
            {
                return this.expansionPlayfield;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ExtenalDoorInstance
        {
            get
            {
                return this.extenalDoorInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ExtenalPlayfieldInstance
        {
            get
            {
                return this.extenalPlayfieldInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ExtendedFlags
        {
            get
            {
                return this.extendedFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ExtendedTime
        {
            get
            {
                return this.extendedTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Extroverty
        {
            get
            {
                return this.extroverty;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FabricType
        {
            get
            {
                return this.fabricType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Face
        {
            get
            {
                return this.face;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FaceTexture
        {
            get
            {
                return this.faceTexture;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FactionModifier
        {
            get
            {
                return this.factionModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FallDamage
        {
            get
            {
                return this.fallDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FastAttack
        {
            get
            {
                return this.fastAttack;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Fatness
        {
            get
            {
                return this.fatness;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Features
        {
            get
            {
                return this.features;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FieldQuantumPhysics
        {
            get
            {
                return this.fieldQuantumPhysics;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FireAC
        {
            get
            {
                return this.fireAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FireDamageModifier
        {
            get
            {
                return this.fireDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FirstAid
        {
            get
            {
                return this.firstAid;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FixtureFlags
        {
            get
            {
                return this.fixtureFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Flags
        {
            get
            {
                return this.flags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FlingShot
        {
            get
            {
                return this.flingShot;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FullAuto
        {
            get
            {
                return this.fullAuto;
            }
        }

        /// <summary>
        /// </summary>
        public Stat FullAutoRecharge
        {
            get
            {
                return this.fullAutoRecharge;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GMLevel
        {
            get
            {
                return this.gmLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GatherAbstractAnim
        {
            get
            {
                return this.gatherAbstractAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GatherEffectType
        {
            get
            {
                return this.gatherEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GatherSound
        {
            get
            {
                return this.gatherSound;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GenderLimit
        {
            get
            {
                return this.genderLimit;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GlobalClanInstance
        {
            get
            {
                return this.globalClanInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GlobalClanType
        {
            get
            {
                return this.globalClanType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GlobalResearchGoal
        {
            get
            {
                return this.globalResearchGoal;
            }
        }

        /// <summary>
        /// </summary>
        public Stat GlobalResearchLevel
        {
            get
            {
                return this.globalResearchLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Gos
        {
            get
            {
                return this.gos;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Grenade
        {
            get
            {
                return this.grenade;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HPLevelUp
        {
            get
            {
                return this.hpLevelUp;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HPPerSkill
        {
            get
            {
                return this.hpPerSkill;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HairMesh
        {
            get
            {
                return this.hairMesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HasAlwaysLootable
        {
            get
            {
                return this.hasAlwaysLootable;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HasKnuBotData
        {
            get
            {
                return this.hasKnuBotData;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HateValueModifyer
        {
            get
            {
                return this.hateValueModifyer;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HeadMesh
        {
            get
            {
                return this.headMesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HealDelta
        {
            get
            {
                return this.healDelta;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HealInterval
        {
            get
            {
                return this.healInterval;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HealMultiplier
        {
            get
            {
                return this.healMultiplier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Health
        {
            get
            {
                return this.health;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HealthChange
        {
            get
            {
                return this.healthChange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HealthChangeBest
        {
            get
            {
                return this.healthChangeBest;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HealthChangeWorst
        {
            get
            {
                return this.healthChangeWorst;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HitEffectType
        {
            get
            {
                return this.hitEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HitSound
        {
            get
            {
                return this.hitSound;
            }
        }

        /// <summary>
        /// </summary>
        public Stat HouseTemplate
        {
            get
            {
                return this.houseTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public Stat IP
        {
            get
            {
                return this.ip;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Icon
        {
            get
            {
                return this.icon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ImpactEffectType
        {
            get
            {
                return this.impactEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InPlay
        {
            get
            {
                return this.inPlay;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Info
        {
            get
            {
                return this.info;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InitiativeType
        {
            get
            {
                return this.initiativeType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Instance
        {
            get
            {
                return this.instance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InsurancePercentage
        {
            get
            {
                return this.insurancePercentage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InsuranceTime
        {
            get
            {
                return this.insuranceTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Intelligence
        {
            get
            {
                return this.intelligence;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InteractionRadius
        {
            get
            {
                return this.interactionRadius;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InterruptModifier
        {
            get
            {
                return this.interruptModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InvadersKilled
        {
            get
            {
                return this.invadersKilled;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InventoryId
        {
            get
            {
                return this.inventoryId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat InventoryTimeout
        {
            get
            {
                return this.inventoryTimeout;
            }
        }

        /// <summary>
        /// </summary>
        public Stat IsFightingMe
        {
            get
            {
                return this.isFightingMe;
            }
        }

        /// <summary>
        /// </summary>
        public Stat IsVehicle
        {
            get
            {
                return this.isVehicle;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemAnim
        {
            get
            {
                return this.itemAnim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemClass
        {
            get
            {
                return this.itemClass;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemDelay
        {
            get
            {
                return this.itemDelay;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemDelayCap
        {
            get
            {
                return this.itemDelayCap;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemHateValue
        {
            get
            {
                return this.itemHateValue;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemOpposedSkill
        {
            get
            {
                return this.itemOpposedSkill;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemSis
        {
            get
            {
                return this.itemSIS;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemSkill
        {
            get
            {
                return this.itemSkill;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat KilledByInvaders
        {
            get
            {
                return this.killedByInvaders;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LREnergyWeapon
        {
            get
            {
                return this.lrEnergyWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LRMultipleWeapon
        {
            get
            {
                return this.lrMultipleWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastConcretePlayfieldInstance
        {
            get
            {
                return this.lastConcretePlayfieldInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastMailCheckTime
        {
            get
            {
                return this.lastMailCheckTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastPerkResetTime
        {
            get
            {
                return this.lastPerkResetTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastRnd
        {
            get
            {
                return this.lastRnd;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastSK
        {
            get
            {
                return this.lastSK;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastSaveXP
        {
            get
            {
                return this.lastSaveXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastSaved
        {
            get
            {
                return this.lastSaved;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LastXP
        {
            get
            {
                return this.lastXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LeaderLockDownTime
        {
            get
            {
                return this.leaderLockDownTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Level
        {
            get
            {
                return this.level;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LevelLimit
        {
            get
            {
                return this.levelLimit;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Life
        {
            get
            {
                return this.life;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LiquidType
        {
            get
            {
                return this.liquidType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LockDifficulty
        {
            get
            {
                return this.lockDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LockDownTime
        {
            get
            {
                return this.lockDownTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LosHeight
        {
            get
            {
                return this.losHeight;
            }
        }

        /// <summary>
        /// </summary>
        public Stat LowresMesh
        {
            get
            {
                return this.lowresMesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapAreaPart1
        {
            get
            {
                return this.mapAreaPart1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapAreaPart2
        {
            get
            {
                return this.mapAreaPart2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapAreaPart3
        {
            get
            {
                return this.mapAreaPart3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapAreaPart4
        {
            get
            {
                return this.mapAreaPart4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapFlags
        {
            get
            {
                return this.mapFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapNavigation
        {
            get
            {
                return this.mapNavigation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MapOptions
        {
            get
            {
                return this.mapOptions;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MartialArts
        {
            get
            {
                return this.martialArts;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaterialCreation
        {
            get
            {
                return this.materialCreation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaterialLocation
        {
            get
            {
                return this.materialLocation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaterialMetamorphose
        {
            get
            {
                return this.materialMetamorphose;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaxDamage
        {
            get
            {
                return this.maxDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaxEnergy
        {
            get
            {
                return this.maxEnergy;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaxMass
        {
            get
            {
                return this.maxMass;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaxNanoEnergy
        {
            get
            {
                return this.maxNanoEnergy;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaxNcu
        {
            get
            {
                return this.maxNCU;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MaxShopItems
        {
            get
            {
                return this.maxShopItems;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MechData
        {
            get
            {
                return this.mechData;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MechanicalEngineering
        {
            get
            {
                return this.mechanicalEngineering;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MeleeAC
        {
            get
            {
                return this.meleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MeleeDamageModifier
        {
            get
            {
                return this.meleeDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MeleeEnergyWeapon
        {
            get
            {
                return this.meleeEnergyWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MeleeMultiple
        {
            get
            {
                return this.meleeMultiple;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MemberInstance
        {
            get
            {
                return this.memberInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MemberType
        {
            get
            {
                return this.memberType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Members
        {
            get
            {
                return this.members;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Mesh
        {
            get
            {
                return this.mesh;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MetaType
        {
            get
            {
                return this.metaType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MetersWalked
        {
            get
            {
                return this.metersWalked;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MinDamage
        {
            get
            {
                return this.minDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MinMembers
        {
            get
            {
                return this.minMembers;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits1
        {
            get
            {
                return this.missionBits1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits10
        {
            get
            {
                return this.missionBits10;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits11
        {
            get
            {
                return this.missionBits11;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits12
        {
            get
            {
                return this.missionBits12;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits2
        {
            get
            {
                return this.missionBits2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits3
        {
            get
            {
                return this.missionBits3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits4
        {
            get
            {
                return this.missionBits4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits5
        {
            get
            {
                return this.missionBits5;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits6
        {
            get
            {
                return this.missionBits6;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits7
        {
            get
            {
                return this.missionBits7;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits8
        {
            get
            {
                return this.missionBits8;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MissionBits9
        {
            get
            {
                return this.missionBits9;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MonsterData
        {
            get
            {
                return this.monsterData;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MonsterLevelsKilled
        {
            get
            {
                return this.monsterLevelsKilled;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MonsterScale
        {
            get
            {
                return this.monsterScale;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MonsterTexture
        {
            get
            {
                return this.monsterTexture;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MonthsPaid
        {
            get
            {
                return this.monthsPaid;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MoreFlags
        {
            get
            {
                return this.moreFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat MultipleCount
        {
            get
            {
                return this.multipleCount;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NPCostModifier
        {
            get
            {
                return this.npCostModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NPLevelUp
        {
            get
            {
                return this.npLevelUp;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NPPerSkill
        {
            get
            {
                return this.npPerSkill;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NameTemplate
        {
            get
            {
                return this.nameTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoAC
        {
            get
            {
                return this.nanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoDamageModifier
        {
            get
            {
                return this.nanoDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoDamageMultiplier
        {
            get
            {
                return this.nanoDamageMultiplier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoDelta
        {
            get
            {
                return this.nanoDelta;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoEnergyPool
        {
            get
            {
                return this.nanoEnergyPool;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoFocusLevel
        {
            get
            {
                return this.nanoFocusLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoInterval
        {
            get
            {
                return this.nanoInterval;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoPoints
        {
            get
            {
                return this.nanoPoints;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoProgramming
        {
            get
            {
                return this.nanoProgramming;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoProwessInitiative
        {
            get
            {
                return this.nanoProwessInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoSpeed
        {
            get
            {
                return this.nanoSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NanoVulnerability
        {
            get
            {
                return this.nanoVulnerability;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NewbieHP
        {
            get
            {
                return this.newbieHP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NewbieNP
        {
            get
            {
                return this.newbieNP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NextDoorInBuilding
        {
            get
            {
                return this.nextDoorInBuilding;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NextFormula
        {
            get
            {
                return this.nextFormula;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NextSK
        {
            get
            {
                return this.nextSK;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NextXP
        {
            get
            {
                return this.nextXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcBrainState
        {
            get
            {
                return this.npcBrainState;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcCommand
        {
            get
            {
                return this.npcCommand;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcCommandArg
        {
            get
            {
                return this.npcCommandArg;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcCryForHelpRange
        {
            get
            {
                return this.npcCryForHelpRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcFamily
        {
            get
            {
                return this.npcFamily;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcFlags
        {
            get
            {
                return this.npcFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcFovStatus
        {
            get
            {
                return this.npcFovStatus;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcHasPatrolList
        {
            get
            {
                return this.npcHasPatrolList;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcHash
        {
            get
            {
                return this.npcHash;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcHatelistSize
        {
            get
            {
                return this.npcHatelistSize;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcIsSurrendering
        {
            get
            {
                return this.npcIsSurrendering;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcNumPets
        {
            get
            {
                return this.npcNumPets;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcScriptAmsScale
        {
            get
            {
                return this.npcScriptAmsScale;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcSpellArg1
        {
            get
            {
                return this.npcSpellArg1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcSpellRet1
        {
            get
            {
                return this.npcSpellRet1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcSurrenderInstance
        {
            get
            {
                return this.npcSurrenderInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcUseFightModeRegenRate
        {
            get
            {
                return this.npcUseFightModeRegenRate;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcVicinityChars
        {
            get
            {
                return this.npcVicinityChars;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcVicinityFamily
        {
            get
            {
                return this.npcVicinityFamily;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NpcVicinityPlayers
        {
            get
            {
                return this.npcVicinityPlayers;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NumAttackEffects
        {
            get
            {
                return this.numAttackEffects;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NumberOfItems
        {
            get
            {
                return this.numberOfItems;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NumberOfTeamMembers
        {
            get
            {
                return this.numberOfTeamMembers;
            }
        }

        /// <summary>
        /// </summary>
        public Stat NumberOnHateList
        {
            get
            {
                return this.numberOnHateList;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ODMaxSizeAdd
        {
            get
            {
                return this.odMaxSizeAdd;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ODMinSizeAdd
        {
            get
            {
                return this.odMinSizeAdd;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OTArmedForces
        {
            get
            {
                return this.otArmedForces;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OTFollowers
        {
            get
            {
                return this.otFollowers;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OTMed
        {
            get
            {
                return this.otMed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OTOperator
        {
            get
            {
                return this.otOperator;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OTTrans
        {
            get
            {
                return this.otTrans;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OTUnredeemed
        {
            get
            {
                return this.otUnredeemed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ObjectType
        {
            get
            {
                return this.objectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OldTimeExist
        {
            get
            {
                return this.oldTimeExist;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OnTowerCreation
        {
            get
            {
                return this.onTowerCreation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OnehBluntWeapons
        {
            get
            {
                return this.onehBluntWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OnehEdgedWeapon
        {
            get
            {
                return this.onehEdgedWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OrientationMode
        {
            get
            {
                return this.orientationMode;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OriginatorType
        {
            get
            {
                return this.originatorType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OuterRadius
        {
            get
            {
                return this.outerRadius;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideMaterial
        {
            get
            {
                return this.overrideMaterial;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTexture
        {
            get
            {
                return this.overrideTexture;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureAttractor
        {
            get
            {
                return this.overrideTextureAttractor;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureBack
        {
            get
            {
                return this.overrideTextureBack;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureHead
        {
            get
            {
                return this.overrideTextureHead;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureShoulderpadLeft
        {
            get
            {
                return this.overrideTextureShoulderpadLeft;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureShoulderpadRight
        {
            get
            {
                return this.overrideTextureShoulderpadRight;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureWeaponLeft
        {
            get
            {
                return this.overrideTextureWeaponLeft;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OverrideTextureWeaponRight
        {
            get
            {
                return this.overrideTextureWeaponRight;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OwnedTowers
        {
            get
            {
                return this.ownedTowers;
            }
        }

        /// <summary>
        /// </summary>
        public Stat OwnerInstance
        {
            get
            {
                return this.ownerInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PaidPoints
        {
            get
            {
                return this.paidPoints;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ParentInstance
        {
            get
            {
                return this.parentInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ParentType
        {
            get
            {
                return this.parentType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Parry
        {
            get
            {
                return this.parry;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentChemicalDamage
        {
            get
            {
                return this.percentChemicalDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentColdDamage
        {
            get
            {
                return this.percentColdDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentEnergyDamage
        {
            get
            {
                return this.percentEnergyDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentFireDamage
        {
            get
            {
                return this.percentFireDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentMeleeDamage
        {
            get
            {
                return this.percentMeleeDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentPoisonDamage
        {
            get
            {
                return this.percentPoisonDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentProjectileDamage
        {
            get
            {
                return this.percentProjectileDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentRadiationDamage
        {
            get
            {
                return this.percentRadiationDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentRemainingHealth
        {
            get
            {
                return this.percentRemainingHealth;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PercentRemainingNano
        {
            get
            {
                return this.percentRemainingNano;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Perception
        {
            get
            {
                return this.perception;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PersonalResearchGoal
        {
            get
            {
                return this.personalResearchGoal;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PersonalResearchLevel
        {
            get
            {
                return this.personalResearchLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetCounter
        {
            get
            {
                return this.petCounter;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetMaster
        {
            get
            {
                return this.petMaster;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetReq1
        {
            get
            {
                return this.petReq1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetReq2
        {
            get
            {
                return this.petReq2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetReq3
        {
            get
            {
                return this.petReq3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetReqVal1
        {
            get
            {
                return this.petReqVal1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetReqVal2
        {
            get
            {
                return this.petReqVal2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetReqVal3
        {
            get
            {
                return this.petReqVal3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetState
        {
            get
            {
                return this.petState;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PetType
        {
            get
            {
                return this.petType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Pharmaceuticals
        {
            get
            {
                return this.pharmaceuticals;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PhysicalProwessInitiative
        {
            get
            {
                return this.physicalProwessInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Piercing
        {
            get
            {
                return this.piercing;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Pistol
        {
            get
            {
                return this.pistol;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Placement
        {
            get
            {
                return this.placement;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PlayerId
        {
            get
            {
                return this.playerId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PlayerKilling
        {
            get
            {
                return this.playerKilling;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PlayerOptions
        {
            get
            {
                return this.playerOptions;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PlayfieldType
        {
            get
            {
                return this.playfieldType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PoisonAC
        {
            get
            {
                return this.poisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PoisonDamageModifier
        {
            get
            {
                return this.poisonDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PrevMovementMode
        {
            get
            {
                return this.prevMovementMode;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PreviousHealth
        {
            get
            {
                return this.previousHealth;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Price
        {
            get
            {
                return this.price;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PrimaryItemInstance
        {
            get
            {
                return this.primaryItemInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PrimaryItemType
        {
            get
            {
                return this.primaryItemType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PrimaryTemplateId
        {
            get
            {
                return this.primaryTemplateId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcChance1
        {
            get
            {
                return this.procChance1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcChance2
        {
            get
            {
                return this.procChance2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcChance3
        {
            get
            {
                return this.procChance3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcChance4
        {
            get
            {
                return this.procChance4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcInitiative1
        {
            get
            {
                return this.procInitiative1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcInitiative2
        {
            get
            {
                return this.procInitiative2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcInitiative3
        {
            get
            {
                return this.procInitiative3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcInitiative4
        {
            get
            {
                return this.procInitiative4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcNano1
        {
            get
            {
                return this.procNano1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcNano2
        {
            get
            {
                return this.procNano2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcNano3
        {
            get
            {
                return this.procNano3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProcNano4
        {
            get
            {
                return this.procNano4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Profession
        {
            get
            {
                return this.profession;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProfessionLevel
        {
            get
            {
                return this.professionLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProjectileAC
        {
            get
            {
                return this.projectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProjectileDamageModifier
        {
            get
            {
                return this.projectileDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProximityRangeIndoors
        {
            get
            {
                return this.proximityRangeIndoors;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ProximityRangeOutdoors
        {
            get
            {
                return this.proximityRangeOutdoors;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Psychic
        {
            get
            {
                return this.psychic;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PsychologicalModification
        {
            get
            {
                return this.psychologicalModification;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Psychology
        {
            get
            {
                return this.psychology;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvPLevelsKilled
        {
            get
            {
                return this.pvPLevelsKilled;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpDuelDeaths
        {
            get
            {
                return this.pvpDuelDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpDuelKills
        {
            get
            {
                return this.pvpDuelKills;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpDuelScore
        {
            get
            {
                return this.pvpDuelScore;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpProfessionDuelDeaths
        {
            get
            {
                return this.pvpProfessionDuelDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpProfessionDuelKills
        {
            get
            {
                return this.pvpProfessionDuelKills;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpRankedSoloDeaths
        {
            get
            {
                return this.pvpRankedSoloDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpRankedSoloKills
        {
            get
            {
                return this.pvpRankedSoloKills;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpRankedTeamDeaths
        {
            get
            {
                return this.pvpRankedTeamDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpRankedTeamKills
        {
            get
            {
                return this.pvpRankedTeamKills;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpRating
        {
            get
            {
                return this.pvpRating;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpSoloScore
        {
            get
            {
                return this.pvpSoloScore;
            }
        }

        /// <summary>
        /// </summary>
        public Stat PvpTeamScore
        {
            get
            {
                return this.pvpTeamScore;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTDungeonInstance
        {
            get
            {
                return this.qtDungeonInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKillNumMonsterCount1
        {
            get
            {
                return this.qtKillNumMonsterCount1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKillNumMonsterCount2
        {
            get
            {
                return this.qtKillNumMonsterCount2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKillNumMonsterCount3
        {
            get
            {
                return this.qtKillNumMonsterCount3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKillNumMonsterId1
        {
            get
            {
                return this.qtKillNumMonsterId1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKillNumMonsterId2
        {
            get
            {
                return this.qtKillNumMonsterId2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKillNumMonsterId3
        {
            get
            {
                return this.qtKillNumMonsterID3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTKilledMonsters
        {
            get
            {
                return this.qtKilledMonsters;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QTNumMonsters
        {
            get
            {
                return this.qtNumMonsters;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestAsMaximumRange
        {
            get
            {
                return this.questAsMaximumRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestAsMinimumRange
        {
            get
            {
                return this.questAsMinimumRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestBoothDifficulty
        {
            get
            {
                return this.questBoothDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestIndex0
        {
            get
            {
                return this.questIndex0;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestIndex1
        {
            get
            {
                return this.questIndex1;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestIndex2
        {
            get
            {
                return this.questIndex2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestIndex3
        {
            get
            {
                return this.questIndex3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestIndex4
        {
            get
            {
                return this.questIndex4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestIndex5
        {
            get
            {
                return this.questIndex5;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestInstance
        {
            get
            {
                return this.questInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestLevelsSolved
        {
            get
            {
                return this.questLevelsSolved;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestStat
        {
            get
            {
                return this.questStat;
            }
        }

        /// <summary>
        /// </summary>
        public Stat QuestTimeout
        {
            get
            {
                return this.questTimeout;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RP
        {
            get
            {
                return this.rp;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Race
        {
            get
            {
                return this.race;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RadiationAC
        {
            get
            {
                return this.radiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RadiationDamageModifier
        {
            get
            {
                return this.radiationDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RangeIncreaserNF
        {
            get
            {
                return this.rangeIncreaserNF;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RangeIncreaserWeapon
        {
            get
            {
                return this.rangeIncreaserWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReadOnly
        {
            get
            {
                return this.readOnly;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RechargeDelay
        {
            get
            {
                return this.rechargeDelay;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RechargeDelayCap
        {
            get
            {
                return this.rechargeDelayCap;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReclaimItem
        {
            get
            {
                return this.reclaimItem;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectChemicalAC
        {
            get
            {
                return this.reflectChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectColdAC
        {
            get
            {
                return this.reflectColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectEnergyAC
        {
            get
            {
                return this.reflectEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectFireAC
        {
            get
            {
                return this.reflectFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectMeleeAC
        {
            get
            {
                return this.reflectMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectNanoAC
        {
            get
            {
                return this.reflectNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectPoisonAC
        {
            get
            {
                return this.reflectPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectProjectileAC
        {
            get
            {
                return this.reflectProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectRadiationAC
        {
            get
            {
                return this.reflectRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedChemicalAC
        {
            get
            {
                return this.reflectReturnedChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedColdAC
        {
            get
            {
                return this.reflectReturnedColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedEnergyAC
        {
            get
            {
                return this.reflectReturnedEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedFireAC
        {
            get
            {
                return this.reflectReturnedFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedMeleeAC
        {
            get
            {
                return this.reflectReturnedMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedNanoAC
        {
            get
            {
                return this.reflectReturnedNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedPoisonAC
        {
            get
            {
                return this.reflectReturnedPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedProjectileAC
        {
            get
            {
                return this.reflectReturnedProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ReflectReturnedRadiationAC
        {
            get
            {
                return this.reflectReturnedRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RegainXPPercentage
        {
            get
            {
                return this.regainXPPercentage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RepairDifficulty
        {
            get
            {
                return this.repairDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RepairSkill
        {
            get
            {
                return this.repairSkill;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ResistModifier
        {
            get
            {
                return this.resistModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RestModifier
        {
            get
            {
                return this.restModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ResurrectDest
        {
            get
            {
                return this.resurrectDest;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Rifle
        {
            get
            {
                return this.rifle;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Riposte
        {
            get
            {
                return this.riposte;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RitualTargetInst
        {
            get
            {
                return this.ritualTargetInst;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Rnd
        {
            get
            {
                return this.rnd;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Rotation
        {
            get
            {
                return this.rotation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat RunSpeed
        {
            get
            {
                return this.runSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SK
        {
            get
            {
                return this.sk;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SavedXP
        {
            get
            {
                return this.savedXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat School
        {
            get
            {
                return this.school;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SecondaryItemInstance
        {
            get
            {
                return this.secondaryItemInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SecondaryItemTemplate
        {
            get
            {
                return this.secondaryItemTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SecondaryItemType
        {
            get
            {
                return this.secondaryItemType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SelectedTarget
        {
            get
            {
                return this.selectedTarget;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SelectedTargetType
        {
            get
            {
                return this.selectedTargetType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SellModifier
        {
            get
            {
                return this.sellModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Sense
        {
            get
            {
                return this.sense;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SenseImprovement
        {
            get
            {
                return this.senseImprovement;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SessionTime
        {
            get
            {
                return this.sessionTime;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Sex
        {
            get
            {
                return this.sex;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShadowBreed
        {
            get
            {
                return this.shadowBreed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShadowBreedTemplate
        {
            get
            {
                return this.shadowBreedTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldChemicalAC
        {
            get
            {
                return this.shieldChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldColdAC
        {
            get
            {
                return this.shieldColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldEnergyAC
        {
            get
            {
                return this.shieldEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldFireAC
        {
            get
            {
                return this.shieldFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldMeleeAC
        {
            get
            {
                return this.shieldMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldNanoAC
        {
            get
            {
                return this.shieldNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldPoisonAC
        {
            get
            {
                return this.shieldPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldProjectileAC
        {
            get
            {
                return this.shieldProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShieldRadiationAC
        {
            get
            {
                return this.shieldRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopFlags
        {
            get
            {
                return this.shopFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopId
        {
            get
            {
                return this.shopId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopIndex
        {
            get
            {
                return this.shopIndex;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopLastUsed
        {
            get
            {
                return this.shopLastUsed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopPrice
        {
            get
            {
                return this.shopPrice;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopRent
        {
            get
            {
                return this.shopRent;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShopType
        {
            get
            {
                return this.shopType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Shotgun
        {
            get
            {
                return this.shotgun;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShoulderMeshHolder
        {
            get
            {
                return this.shoulderMeshHolder;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShoulderMeshLeft
        {
            get
            {
                return this.shoulderMeshLeft;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ShoulderMeshRight
        {
            get
            {
                return this.shoulderMeshRight;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Side
        {
            get
            {
                return this.side;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SisCap
        {
            get
            {
                return this.sisCap;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SkillDisabled
        {
            get
            {
                return this.skillDisabled;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SkillLockModifier
        {
            get
            {
                return this.skillLockModifier;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SkillTimeOnSelectedTarget
        {
            get
            {
                return this.skillTimeOnSelectedTarget;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SneakAttack
        {
            get
            {
                return this.sneakAttack;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SocialStatus
        {
            get
            {
                return this.socialStatus;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SoundVolume
        {
            get
            {
                return this.soundVolume;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SpecialAttackShield
        {
            get
            {
                return this.specialAttackShield;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SpecialCondition
        {
            get
            {
                return this.specialCondition;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Specialization
        {
            get
            {
                return this.specialization;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SpeedPenalty
        {
            get
            {
                return this.speedPenalty;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Stability
        {
            get
            {
                return this.stability;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StackingLine2
        {
            get
            {
                return this.stackingLine2;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StackingLine3
        {
            get
            {
                return this.stackingLine3;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StackingLine4
        {
            get
            {
                return this.stackingLine4;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StackingLine5
        {
            get
            {
                return this.stackingLine5;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StackingLine6
        {
            get
            {
                return this.stackingLine6;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StackingOrder
        {
            get
            {
                return this.stackingOrder;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Stamina
        {
            get
            {
                return this.stamina;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StatOne
        {
            get
            {
                return this.statOne;
            }
        }

        /// <summary>
        /// </summary>
        public Stat State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StateAction
        {
            get
            {
                return this.stateAction;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StateMachine
        {
            get
            {
                return this.stateMachine;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StaticInstance
        {
            get
            {
                return this.staticInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StaticType
        {
            get
            {
                return this.staticType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat StreamCheckMagic
        {
            get
            {
                return this.streamCheckMagic;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Strength
        {
            get
            {
                return this.strength;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SubMachineGun
        {
            get
            {
                return this.subMachineGun;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Swim
        {
            get
            {
                return this.swim;
            }
        }

        /// <summary>
        /// </summary>
        public Stat SynergyHash
        {
            get
            {
                return this.synergyHash;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Taboo
        {
            get
            {
                return this.taboo;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TargetDistance
        {
            get
            {
                return this.targetDistance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TargetDistanceChange
        {
            get
            {
                return this.targetDistanceChange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TargetFacing
        {
            get
            {
                return this.targetFacing;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Team
        {
            get
            {
                return this.team;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TeamAllowed
        {
            get
            {
                return this.teamAllowed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TeamCloseness
        {
            get
            {
                return this.teamCloseness;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TeamSide
        {
            get
            {
                return this.teamSide;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TeleportPauseMilliSeconds
        {
            get
            {
                return this.teleportPauseMilliSeconds;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TempSavePlayfield
        {
            get
            {
                return this.tempSavePlayfield;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TempSaveTeamId
        {
            get
            {
                return this.tempSaveTeamId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TempSaveX
        {
            get
            {
                return this.tempSaveX;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TempSaveY
        {
            get
            {
                return this.tempSaveY;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TemporarySkillReduction
        {
            get
            {
                return this.temporarySkillReduction;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ThrowingKnife
        {
            get
            {
                return this.throwingKnife;
            }
        }

        /// <summary>
        /// </summary>
        public Stat ThrownGrapplingWeapons
        {
            get
            {
                return this.thrownGrapplingWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TideRequiredDynelId
        {
            get
            {
                return this.tideRequiredDynelId;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TimeExist
        {
            get
            {
                return this.timeExist;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TimeSinceCreation
        {
            get
            {
                return this.timeSinceCreation;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TimeSinceUpkeep
        {
            get
            {
                return this.timeSinceUpkeep;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TitleLevel
        {
            get
            {
                return this.titleLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TotalDamage
        {
            get
            {
                return this.totalDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TotalMass
        {
            get
            {
                return this.totalMass;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TowerInstance
        {
            get
            {
                return this.towerInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TowerNpcHash
        {
            get
            {
                return this.towerNpcHash;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TowerType
        {
            get
            {
                return this.towerType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TracerEffectType
        {
            get
            {
                return this.tracerEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackChemicalDamage
        {
            get
            {
                return this.trackChemicalDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackColdDamage
        {
            get
            {
                return this.trackColdDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackEnergyDamage
        {
            get
            {
                return this.trackEnergyDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackFireDamage
        {
            get
            {
                return this.trackFireDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackMeleeDamage
        {
            get
            {
                return this.trackMeleeDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackPoisonDamage
        {
            get
            {
                return this.trackPoisonDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackProjectileDamage
        {
            get
            {
                return this.trackProjectileDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrackRadiationDamage
        {
            get
            {
                return this.trackRadiationDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TradeLimit
        {
            get
            {
                return this.tradeLimit;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrainSkill
        {
            get
            {
                return this.trainSkill;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrainSkillCost
        {
            get
            {
                return this.trainSkillCost;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TrapDifficulty
        {
            get
            {
                return this.trapDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TravelSound
        {
            get
            {
                return this.travelSound;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Treatment
        {
            get
            {
                return this.treatment;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TurnSpeed
        {
            get
            {
                return this.turnSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat Tutoring
        {
            get
            {
                return this.tutoring;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TwohBluntWeapons
        {
            get
            {
                return this.twohBluntWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public Stat TwohEdgedWeapons
        {
            get
            {
                return this.twohEdgedWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public Stat UnarmedTemplateInstance
        {
            get
            {
                return this.unarmedTemplateInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat UnreadMailCount
        {
            get
            {
                return this.unreadMailCount;
            }
        }

        /// <summary>
        /// </summary>
        public Stat UnsavedXP
        {
            get
            {
                return this.unsavedXP;
            }
        }

        /// <summary>
        /// </summary>
        public Stat UserInstance
        {
            get
            {
                return this.userInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat UserType
        {
            get
            {
                return this.userType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VehicleAC
        {
            get
            {
                return this.vehicleAC;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VehicleDamage
        {
            get
            {
                return this.vehicleDamage;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VehicleHealth
        {
            get
            {
                return this.vehicleHealth;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VehicleSpeed
        {
            get
            {
                return this.vehicleSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VeteranPoints
        {
            get
            {
                return this.veteranPoints;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VicinityRange
        {
            get
            {
                return this.vicinityRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VictoryPoints
        {
            get
            {
                return this.victoryPoints;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VisualBreed
        {
            get
            {
                return this.visualBreed;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VisualFlags
        {
            get
            {
                return this.visualFlags;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VisualLodLevel
        {
            get
            {
                return this.visualLodLevel;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VisualProfession
        {
            get
            {
                return this.visualProfession;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VisualSex
        {
            get
            {
                return this.visualSex;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VolumeMass
        {
            get
            {
                return this.volumeMass;
            }
        }

        /// <summary>
        /// </summary>
        public Stat VoteCount
        {
            get
            {
                return this.voteCount;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WaitState
        {
            get
            {
                return this.waitState;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponDisallowedInstance
        {
            get
            {
                return this.weaponDisallowedInstance;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponDisallowedType
        {
            get
            {
                return this.weaponDisallowedType;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponMeshHolder
        {
            get
            {
                return this.weaponMeshHolder;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponMeshLeft
        {
            get
            {
                return this.weaponMeshLeft;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponMeshRight
        {
            get
            {
                return this.weaponMeshRight;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponSmithing
        {
            get
            {
                return this.weaponSmithing;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponStyleLeft
        {
            get
            {
                return this.weaponStyleLeft;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponStyleRight
        {
            get
            {
                return this.weaponStyleRight;
            }
        }

        /// <summary>
        /// </summary>
        public Stat WeaponsStyle
        {
            get
            {
                return this.weaponsStyle;
            }
        }

        /// <summary>
        /// </summary>
        public Stat XP
        {
            get
            {
                return this.xp;
            }
        }

        /// <summary>
        /// </summary>
        public Stat XPBonus
        {
            get
            {
                return this.xpBonus;
            }
        }

        /// <summary>
        /// </summary>
        public Stat XPKillRange
        {
            get
            {
                return this.xpKillRange;
            }
        }

        /// <summary>
        /// </summary>
        public Stat XPModifier
        {
            get
            {
                return this.xpModifier;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void ClearChangedFlags()
        {
            foreach (Stat cs in this.all)
            {
                cs.Changed = false;
            }
        }

        /// <summary>
        /// </summary>
        public void ClearModifiers()
        {
            foreach (Stat c in this.all)
            {
                c.Modifier = 0;
                c.PercentageModifier = 100;
                c.Trickle = 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="stat">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public uint GetBaseValue(int stat)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != stat)
                {
                    continue;
                }

                return c.BaseValue;
            }

            throw new StatDoesNotExistException("Stat " + stat + " does not exist.\r\nMethod: GetBaseValue");
        }

        /// <summary>
        /// </summary>
        /// <param name="stat">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public int GetModifier(int stat)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != stat)
                {
                    continue;
                }

                return c.Modifier;
            }

            throw new StatDoesNotExistException("Stat " + stat + " does not exist.\r\nMethod: GetModifier");
        }

        /// <summary>
        /// </summary>
        /// <param name="stat">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public int GetPercentageModifier(int stat)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != stat)
                {
                    continue;
                }

                return c.PercentageModifier;
            }

            throw new StatDoesNotExistException("Stat " + stat + " does not exist.\r\nMethod: GetPercentageModifier");
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        public Stat GetStatbyNumber(int number)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != number)
                {
                    continue;
                }

                return c;
            }

            return null;
        }

        /// <summary>
        /// Read all stats from Sql
        /// </summary>
        public void ReadStatsfromSql(Identity identity)
        {
            foreach (DBStats dbStats in
                StatDao.GetById((int)identity.Type, identity.Instance))
            {
                this.SetBaseValue(dbStats.statid, (uint)dbStats.statvalue);
            }
        }

        /// <summary>
        /// </summary>
        public void SetAbilityTricklers()
        {
            for (int c = 0; c < SkillTrickleTable.table.Length / 7; c++)
            {
                int skillnum = Convert.ToInt32(SkillTrickleTable.table[c, 0]);
                if (SkillTrickleTable.table[c, 1] > 0)
                {
                    this.strength.Affects.Add(skillnum);
                }

                if (SkillTrickleTable.table[c, 2] > 0)
                {
                    this.stamina.Affects.Add(skillnum);
                }

                if (SkillTrickleTable.table[c, 3] > 0)
                {
                    this.sense.Affects.Add(skillnum);
                }

                if (SkillTrickleTable.table[c, 4] > 0)
                {
                    this.agility.Affects.Add(skillnum);
                }

                if (SkillTrickleTable.table[c, 5] > 0)
                {
                    this.intelligence.Affects.Add(skillnum);
                }

                if (SkillTrickleTable.table[c, 6] > 0)
                {
                    this.psychic.Affects.Add(skillnum);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public void SetBaseValue(int stat, uint value)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != stat)
                {
                    continue;
                }

                c.Changed = c.BaseValue != value;
                c.BaseValue = value;
                return;
            }

            throw new StatDoesNotExistException(
                "Stat " + stat + " does not exist.\r\nValue: " + value + "\r\nMethod: SetBaseValue");
        }

        /// <summary>
        /// </summary>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public void SetModifier(int stat, int value)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != stat)
                {
                    continue;
                }

                c.Modifier = value;
                return;
            }

            throw new StatDoesNotExistException(
                "Stat " + stat + " does not exist.\r\nValue: " + value + "\r\nMethod: SetModifier");
        }

        /// <summary>
        /// </summary>
        /// <param name="stat">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public void SetPercentageModifier(int stat, int value)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != stat)
                {
                    continue;
                }

                c.PercentageModifier = value;
                return;
            }

            throw new StatDoesNotExistException(
                "Stat " + stat + " does not exist.\r\nValue: " + value + "\r\nMethod: SetPercentageModifier");
        }

        /// <summary>
        /// Sets Stat's value
        /// </summary>
        /// <param name="number">
        /// Stat number
        /// </param>
        /// <param name="newValue">
        /// Stat's new value
        /// </param>
        public void SetStatValueByName(int number, uint newValue)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != number)
                {
                    continue;
                }

                c.Set(newValue);
                return;
            }

            throw new StatDoesNotExistException(
                "Stat " + number + " does not exist.\r\nValue: " + newValue + "\r\nMethod: Set");
        }

        /// <summary>
        /// Sets Stat's value
        /// </summary>
        /// <param name="statName">
        /// Stat's name
        /// </param>
        /// <param name="newValue">
        /// Stat's new value
        /// </param>
        public void SetStatValueByName(string statName, uint newValue)
        {
            Contract.Requires(statName != null);
            int statid = StatNamesDefaults.GetStatNumber(statName.ToLower());
            foreach (Stat c in this.all)
            {
                if (c.StatId != statid)
                {
                    continue;
                }

                c.Set(newValue);
                return;
            }

            throw new StatDoesNotExistException(
                "Stat " + statName + " does not exist.\r\nValue: " + newValue + "\r\nMethod: GetID");
        }

        /// <summary>
        /// </summary>
        /// <param name="statId">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public void SetTrickle(int statId, int value)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != statId)
                {
                    continue;
                }

                c.Trickle = value;
                return;
            }

            throw new StatDoesNotExistException("Stat " + statId + " does not exist.\r\nMethod: Trickle");
        }

        /// <summary>
        /// </summary>
        /// <param name="statName">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        public int StatIdByName(string statName)
        {
            int statid = StatNamesDefaults.GetStatNumber(statName.ToLower());
            foreach (Stat c in this.all)
            {
                if (c.StatId != statid)
                {
                    continue;
                }

                return c.StatId;
            }

            throw new StatDoesNotExistException("Stat " + statName + " does not exist.\r\nMethod: GetID");
        }

        /// <summary>
        /// Returns Stat's value
        /// </summary>
        /// <param name="number">
        /// Stat number
        /// </param>
        /// <returns>
        /// Stat's value
        /// </returns>
        public int StatValueByName(int number)
        {
            foreach (Stat c in this.all)
            {
                if (c.StatId != number)
                {
                    continue;
                }

                return c.Value;
            }

            throw new StatDoesNotExistException("Stat " + number + " does not exist.\r\nMethod: Get");
        }

        /// <summary>
        /// Returns Stat's value
        /// </summary>
        /// <param name="name">
        /// Name of the Stat
        /// </param>
        /// <returns>
        /// Stat's value
        /// </returns>
        public int StatValueByName(string name)
        {
            Contract.Requires(name != null);
            int statid = StatNamesDefaults.GetStatNumber(name.ToLower());
            foreach (Stat c in this.all)
            {
                if (c.StatId != statid)
                {
                    continue;
                }

                return c.Value;
            }

            throw new StatDoesNotExistException("Stat " + name + " does not exist.\r\nMethod: Get");
        }

        /// <summary>
        /// Write all Stats to Sql
        /// </summary>
        public void WriteStatstoSql()
        {
            this.Write();
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        /// <returns>
        /// </returns>
        IStat IStatList.this[int index]
        {
            get
            {
                foreach (IStat stat in this.all)
                {
                    if (stat.StatId == index)
                    {
                        return stat;
                    }
                }

                throw new StatDoesNotExistException("Stat with Id " + index + " does not exist");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        /// <returns>
        /// </returns>
        IStat IStatList.this[string name]
        {
            get
            {
                int index = StatNamesDefaults.GetStatNumber(name);
                foreach (IStat stat in this.all)
                {
                    if (stat.StatId == index)
                    {
                        return stat;
                    }
                }

                throw new StatDoesNotExistException(
                    "huh? Stat with Id " + index + " does not exist, but the name " + name + " exists? CODER ALERT");
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Read(Identity identity)
        {
            try
            {
                this.ReadStatsfromSql(identity);
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorException(ex);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Write(Identity identity)
        {
            int inst = identity.Instance;
            int typ = (int)identity.Type;
            List<DBStats> temp = new List<DBStats>();
            foreach (IStat stat in this.all)
            {
                // Flags are special cases, save always
                if ((stat.StatId == 0)
                    || ((stat.BaseValue != StatNamesDefaults.GetDefault(stat.StatId))
                        && (((Stat)stat).DoNotDontWriteToSql == false)))
                {
                    temp.Add(
                        new DBStats
                        {
                            statid = stat.StatId,
                            statvalue = (int)stat.BaseValue,
                            type = typ,
                            instance = inst
                        });
                }
            }

            if (temp.Count == 0)
            {
                StatDao.DeleteStats(typ, inst);
            }
            else
            {
                StatDao.BulkReplace(temp);
            }

            return true;
        }

        public bool Read()
        {
            return this.Read(this.Owner);
        }

        public bool Write()
        {
            return this.Write(this.Owner);
        }
    }

    #endregion
}