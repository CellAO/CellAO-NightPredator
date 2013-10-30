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
// Last modified: 2013-10-29 22:26
// Created:       2013-10-29 19:57

#endregion

namespace CellAO.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    
    using CellAO.Core.Exceptions;
    using CellAO.Interfaces;

    #endregion

    #region Character_Stats holder for Character's stats

    /// <summary>
    /// </summary>
    public class DynelStats : IStatList
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbChemicalAC = new DynelStat(241, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbColdAC = new DynelStat(243, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbEnergyAC = new DynelStat(240, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbFireAC = new DynelStat(244, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbMeleeAC = new DynelStat(239, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbNanoAC = new DynelStat(246, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbPoisonAC = new DynelStat(245, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbProjectileAC = new DynelStat(238, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat absorbRadiationAC = new DynelStat(242, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat accessCount = new DynelStat(35, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat accessGrant = new DynelStat(258, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat accessKey = new DynelStat(195, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat accountFlags = new DynelStat(660, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat accumulatedDamage = new DynelStat(222, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat acgEntranceStyles = new DynelStat(384, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat acgItemCategoryId = new DynelStat(704, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat acgItemLevel = new DynelStat(701, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat acgItemSeed = new DynelStat(700, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat acgItemTemplateId = new DynelStat(702, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat acgItemTemplateId2 = new DynelStat(703, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat actionCategory = new DynelStat(588, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat advantageHash1 = new DynelStat(651, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat advantageHash2 = new DynelStat(652, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat advantageHash3 = new DynelStat(653, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat advantageHash4 = new DynelStat(654, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat advantageHash5 = new DynelStat(655, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat adventuring = new DynelStat(137, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat age = new DynelStat(58, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat aggDef = new DynelStat(51, 100, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat aggressiveness = new DynelStat(201, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat agility = new DynelStat(17, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat aimedShot = new DynelStat(151, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat alienLevel = new DynelStat(169, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat alienNextXP = new DynelStat(178, 1500, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat alienXP = new DynelStat(40, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat alignment = new DynelStat(62, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly List<DynelStat> all = new List<DynelStat>();

        /// <summary>
        /// </summary>
        private readonly DynelStat ammoName = new DynelStat(399, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat ammoType = new DynelStat(420, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat ams = new DynelStat(22, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat amsCap = new DynelStat(538, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat amsModifier = new DynelStat(276, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat anim = new DynelStat(13, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat animPlay = new DynelStat(501, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat animPos = new DynelStat(500, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat animSet = new DynelStat(353, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat animSpeed = new DynelStat(502, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat apartmentAccessCard = new DynelStat(584, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat apartmentsAllowed = new DynelStat(582, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat apartmentsOwned = new DynelStat(583, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat areaInstance = new DynelStat(87, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat areaType = new DynelStat(86, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat armourType = new DynelStat(424, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat assaultRifle = new DynelStat(116, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat attackCount = new DynelStat(36, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat attackRange = new DynelStat(287, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat attackShield = new DynelStat(516, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat attackSpeed = new DynelStat(3, 5, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat attackType = new DynelStat(354, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat attitude = new DynelStat(63, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat autoAttackFlags = new DynelStat(349, 5, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat autoLockTimeDefault = new DynelStat(175, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat autoUnlockTimeDefault = new DynelStat(176, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat backMesh = new DynelStat(38, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat backstab = new DynelStat(489, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat bandolierSlots = new DynelStat(46, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat battlestationRep = new DynelStat(670, 10, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat battlestationSide = new DynelStat(668, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat beltSlots = new DynelStat(45, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat berserkMode = new DynelStat(235, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat biologicalMetamorphose = new DynelStat(128, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat birthDate = new DynelStat(248, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat bodyDevelopment = new DynelStat(152, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat bow = new DynelStat(111, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat bowSpecialAttack = new DynelStat(121, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat brainType = new DynelStat(340, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat brawl = new DynelStat(142, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat breakingEntry = new DynelStat(165, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat breed = new DynelStat(4, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat breedHostility = new DynelStat(204, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat breedLimit = new DynelStat(320, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat buildingComplexInst = new DynelStat(188, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat buildingInstance = new DynelStat(185, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat buildingType = new DynelStat(184, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat burst = new DynelStat(148, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat burstRecharge = new DynelStat(374, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat buyModifier = new DynelStat(426, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat can = new DynelStat(30, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat canChangeClothes = new DynelStat(223, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat cardOwnerInstance = new DynelStat(187, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat cardOwnerType = new DynelStat(186, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat cash = new DynelStat(61, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat castEffectType = new DynelStat(428, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat castSelfAbstractAnim = new DynelStat(378, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat castSound = new DynelStat(270, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat castTargetAbstractAnim = new DynelStat(377, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat catAnim = new DynelStat(401, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat catAnimFlags = new DynelStat(402, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat catMesh = new DynelStat(42, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chanceOfBreakOnDebuff = new DynelStat(386, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chanceOfBreakOnSpellAttack = new DynelStat(385, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chanceOfUse = new DynelStat(422, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat changeSideCount = new DynelStat(237, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat charRadius = new DynelStat(421, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat charState = new DynelStat(434, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat charTmp1 = new DynelStat(441, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat charTmp2 = new DynelStat(442, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat charTmp3 = new DynelStat(443, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat charTmp4 = new DynelStat(444, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chemicalAC = new DynelStat(93, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chemicalDamageModifier = new DynelStat(281, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chemistry = new DynelStat(163, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat chestFlags = new DynelStat(394, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat cityInstance = new DynelStat(640, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat cityTerminalRechargePercent = new DynelStat(642, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clan = new DynelStat(5, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanConserver = new DynelStat(571, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanDevoted = new DynelStat(570, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanFinalized = new DynelStat(314, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanGaia = new DynelStat(563, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanHierarchy = new DynelStat(260, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanInstance = new DynelStat(305, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanItemInstance = new DynelStat(331, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanItemType = new DynelStat(330, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanLevel = new DynelStat(48, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanPrice = new DynelStat(302, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanRedeemed = new DynelStat(572, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanSentinels = new DynelStat(561, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanType = new DynelStat(304, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanUpkeepInterval = new DynelStat(312, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clanVanguards = new DynelStat(565, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat clientActivated = new DynelStat(262, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat closeCombatInitiative = new DynelStat(118, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat coldAC = new DynelStat(95, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat coldDamageModifier = new DynelStat(311, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat collideCheckInterval = new DynelStat(437, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat collisionRadius = new DynelStat(357, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat commandRange = new DynelStat(456, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat compulsion = new DynelStat(328, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat computerLiteracy = new DynelStat(161, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat concealment = new DynelStat(164, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat conditionState = new DynelStat(530, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat conformity = new DynelStat(200, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat corpseAnimKey = new DynelStat(417, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat corpseHash = new DynelStat(398, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat corpseInstance = new DynelStat(416, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat corpseType = new DynelStat(415, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat criticalDecrease = new DynelStat(391, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat criticalIncrease = new DynelStat(379, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currBodyLocation = new DynelStat(220, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentMass = new DynelStat(78, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentMovementMode = new DynelStat(173, 3, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentNCU = new DynelStat(180, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentNano = new DynelStat(214, 1, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentPlayfield = new DynelStat(589, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentState = new DynelStat(423, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat currentTime = new DynelStat(578, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat damageBonus = new DynelStat(284, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat damageOverrideType = new DynelStat(339, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat damageToNano = new DynelStat(659, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat damageToNanoMultiplier = new DynelStat(661, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat damageType = new DynelStat(436, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat deadTimer = new DynelStat(34, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat deathReason = new DynelStat(338, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat debuffFormula = new DynelStat(332, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat defaultAttackType = new DynelStat(292, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat defaultPos = new DynelStat(88, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat desiredTargetDistance = new DynelStat(447, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat dieAnim = new DynelStat(387, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat dimach = new DynelStat(144, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat disarmTrap = new DynelStat(135, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat displayCATAnim = new DynelStat(403, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat displayCATMesh = new DynelStat(404, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat distanceToSpawnpoint = new DynelStat(641, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat distanceWeaponInitiative = new DynelStat(119, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat districtNano = new DynelStat(590, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat districtNanoInterval = new DynelStat(591, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat dms = new DynelStat(29, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat dmsModifier = new DynelStat(277, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat dodge = new DynelStat(154, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat doorBlockTime = new DynelStat(335, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat doorFlags = new DynelStat(259, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat driveAir = new DynelStat(139, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat driveGround = new DynelStat(166, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat driveWater = new DynelStat(117, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat duck = new DynelStat(153, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat dudChance = new DynelStat(534, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat durationModifier = new DynelStat(464, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat effectBlue = new DynelStat(462, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat effectGreen = new DynelStat(461, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat effectIcon = new DynelStat(183, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat effectRed = new DynelStat(460, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat effectType = new DynelStat(413, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat electricalEngineering = new DynelStat(126, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat energy = new DynelStat(26, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat energyAC = new DynelStat(92, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat energyDamageModifier = new DynelStat(280, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat equipDelay = new DynelStat(211, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat equippedWeapons = new DynelStat(274, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat evade = new DynelStat(155, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat exitInstance = new DynelStat(189, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat expansion = new DynelStat(389, 0, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat expansionPlayfield = new DynelStat(531, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat extenalDoorInstance = new DynelStat(193, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat extenalPlayfieldInstance = new DynelStat(192, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat extendedFlags = new DynelStat(598, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat extendedTime = new DynelStat(373, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat extroverty = new DynelStat(203, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fabricType = new DynelStat(41, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat face = new DynelStat(31, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat faceTexture = new DynelStat(347, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat factionModifier = new DynelStat(543, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fallDamage = new DynelStat(474, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fastAttack = new DynelStat(147, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fatness = new DynelStat(47, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat features = new DynelStat(224, 6, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fieldQuantumPhysics = new DynelStat(157, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fireAC = new DynelStat(97, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fireDamageModifier = new DynelStat(316, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat firstAid = new DynelStat(123, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fixtureFlags = new DynelStat(473, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat flags = new DynelStat(0, 8917569, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat flingShot = new DynelStat(150, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fullAuto = new DynelStat(167, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat fullAutoRecharge = new DynelStat(375, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat gatherAbstractAnim = new DynelStat(376, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat gatherEffectType = new DynelStat(366, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat gatherSound = new DynelStat(269, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat genderLimit = new DynelStat(321, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat globalClanInstance = new DynelStat(310, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat globalClanType = new DynelStat(309, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat globalResearchGoal = new DynelStat(266, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat globalResearchLevel = new DynelStat(264, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat gmLevel = new DynelStat(215, 0, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat gos = new DynelStat(566, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat grenade = new DynelStat(109, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hairMesh = new DynelStat(32, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hasAlwaysLootable = new DynelStat(345, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hasKnuBotData = new DynelStat(768, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hateValueModifyer = new DynelStat(288, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat headMesh = new DynelStat(64, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat healDelta = new DynelStat(343, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat healInterval = new DynelStat(342, 29, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat healMultiplier = new DynelStat(535, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat health = new DynelStat(27, 1, true, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat healthChange = new DynelStat(172, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat healthChangeBest = new DynelStat(170, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat healthChangeWorst = new DynelStat(171, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat height = new DynelStat(28, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hitEffectType = new DynelStat(361, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hitSound = new DynelStat(272, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat houseTemplate = new DynelStat(620, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hpLevelUp = new DynelStat(601, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat hpPerSkill = new DynelStat(602, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat icon = new DynelStat(79, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat impactEffectType = new DynelStat(414, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat inPlay = new DynelStat(194, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat info = new DynelStat(15, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat initiativeType = new DynelStat(440, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat instance = new DynelStat(1002, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat insurancePercentage = new DynelStat(236, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat insuranceTime = new DynelStat(49, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat intelligence = new DynelStat(19, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat interactionRadius = new DynelStat(297, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat interruptModifier = new DynelStat(383, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat invadersKilled = new DynelStat(615, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat inventoryId = new DynelStat(55, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat inventoryTimeout = new DynelStat(50, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat ip = new DynelStat(53, 1500, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat isFightingMe = new DynelStat(410, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat isVehicle = new DynelStat(658, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemAnim = new DynelStat(99, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemClass = new DynelStat(76, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemDelay = new DynelStat(294, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemDelayCap = new DynelStat(523, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemHateValue = new DynelStat(283, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemOpposedSkill = new DynelStat(295, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemSIS = new DynelStat(296, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemSkill = new DynelStat(293, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat itemType = new DynelStat(72, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat killedByInvaders = new DynelStat(616, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastConcretePlayfieldInstance = new DynelStat(191, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastMailCheckTime = new DynelStat(650, 1283065897, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastPerkResetTime = new DynelStat(577, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastRnd = new DynelStat(522, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastSK = new DynelStat(574, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastSaveXP = new DynelStat(372, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastSaved = new DynelStat(249, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lastXP = new DynelStat(57, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat leaderLockDownTime = new DynelStat(614, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat level = new DynelStat(54, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat levelLimit = new DynelStat(322, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat life = new DynelStat(1, 1, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat liquidType = new DynelStat(268, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lockDifficulty = new DynelStat(299, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lockDownTime = new DynelStat(613, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat losHeight = new DynelStat(466, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lowresMesh = new DynelStat(390, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lrEnergyWeapon = new DynelStat(133, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat lrMultipleWeapon = new DynelStat(134, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapAreaPart1 = new DynelStat(471, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapAreaPart2 = new DynelStat(472, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapAreaPart3 = new DynelStat(585, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapAreaPart4 = new DynelStat(586, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapFlags = new DynelStat(9, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapNavigation = new DynelStat(140, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mapOptions = new DynelStat(470, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat martialArts = new DynelStat(100, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat materialCreation = new DynelStat(130, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat materialLocation = new DynelStat(131, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat materialMetamorphose = new DynelStat(127, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat maxDamage = new DynelStat(285, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat maxEnergy = new DynelStat(212, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat maxMass = new DynelStat(24, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat maxNCU = new DynelStat(181, 8, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat maxNanoEnergy = new DynelStat(221, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat maxShopItems = new DynelStat(606, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mechData = new DynelStat(662, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mechanicalEngineering = new DynelStat(125, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat meleeAC = new DynelStat(91, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat meleeDamageModifier = new DynelStat(279, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat meleeEnergyWeapon = new DynelStat(104, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat meleeMultiple = new DynelStat(101, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat memberInstance = new DynelStat(308, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat memberType = new DynelStat(307, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat members = new DynelStat(300, 999, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat mesh = new DynelStat(12, 17530, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat metaType = new DynelStat(75, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat metersWalked = new DynelStat(252, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat minDamage = new DynelStat(286, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat minMembers = new DynelStat(301, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits1 = new DynelStat(256, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits10 = new DynelStat(617, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits11 = new DynelStat(618, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits12 = new DynelStat(619, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits2 = new DynelStat(257, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits3 = new DynelStat(303, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits4 = new DynelStat(432, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits5 = new DynelStat(65, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits6 = new DynelStat(66, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits7 = new DynelStat(67, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits8 = new DynelStat(544, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat missionBits9 = new DynelStat(545, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat monsterData = new DynelStat(359, 0, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat monsterLevelsKilled = new DynelStat(254, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat monsterScale = new DynelStat(360, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat monsterTexture = new DynelStat(344, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat monthsPaid = new DynelStat(69, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat moreFlags = new DynelStat(177, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat multipleCount = new DynelStat(412, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat name = new DynelStat(14, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nameTemplate = new DynelStat(446, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoAC = new DynelStat(168, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoDamageModifier = new DynelStat(315, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoDamageMultiplier = new DynelStat(536, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoDelta = new DynelStat(364, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoEnergyPool = new DynelStat(132, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoFocusLevel = new DynelStat(355, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoInterval = new DynelStat(363, 28, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoPoints = new DynelStat(407, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoProgramming = new DynelStat(160, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoProwessInitiative = new DynelStat(149, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoSpeed = new DynelStat(406, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nanoVulnerability = new DynelStat(537, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat newbieHP = new DynelStat(600, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat newbieNP = new DynelStat(603, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nextDoorInBuilding = new DynelStat(190, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nextFormula = new DynelStat(411, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nextSK = new DynelStat(575, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat nextXP = new DynelStat(350, 1450, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npCostModifier = new DynelStat(318, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npLevelUp = new DynelStat(604, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npPerSkill = new DynelStat(605, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcBrainState = new DynelStat(429, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcCommand = new DynelStat(439, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcCommandArg = new DynelStat(445, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcCryForHelpRange = new DynelStat(465, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcFamily = new DynelStat(455, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcFlags = new DynelStat(179, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcFovStatus = new DynelStat(533, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcHasPatrolList = new DynelStat(452, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcHash = new DynelStat(356, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcHatelistSize = new DynelStat(457, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcIsSurrendering = new DynelStat(449, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcNumPets = new DynelStat(458, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcScriptAmsScale = new DynelStat(581, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcSpellArg1 = new DynelStat(638, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcSpellRet1 = new DynelStat(639, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcSurrenderInstance = new DynelStat(451, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcUseFightModeRegenRate = new DynelStat(519, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcVicinityChars = new DynelStat(453, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcVicinityFamily = new DynelStat(580, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat npcVicinityPlayers = new DynelStat(518, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat numAttackEffects = new DynelStat(291, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat numberOfItems = new DynelStat(396, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat numberOfTeamMembers = new DynelStat(587, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat numberOnHateList = new DynelStat(529, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat objectType = new DynelStat(1001, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat odMaxSizeAdd = new DynelStat(463, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat odMinSizeAdd = new DynelStat(459, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat oldTimeExist = new DynelStat(392, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat onTowerCreation = new DynelStat(513, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat onehBluntWeapons = new DynelStat(102, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat onehEdgedWeapon = new DynelStat(103, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat orientationMode = new DynelStat(197, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat originatorType = new DynelStat(490, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat otArmedForces = new DynelStat(560, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat otFollowers = new DynelStat(567, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat otMed = new DynelStat(562, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat otOperator = new DynelStat(568, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat otTrans = new DynelStat(564, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat otUnredeemed = new DynelStat(569, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat outerRadius = new DynelStat(358, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideMaterial = new DynelStat(337, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTexture = new DynelStat(336, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureAttractor = new DynelStat(1014, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureBack = new DynelStat(1013, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureHead = new DynelStat(1008, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureShoulderpadLeft = new DynelStat(1012, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureShoulderpadRight = new DynelStat(1011, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureWeaponLeft = new DynelStat(1010, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat overrideTextureWeaponRight = new DynelStat(1009, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat ownedTowers = new DynelStat(514, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat ownerInstance = new DynelStat(433, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat paidPoints = new DynelStat(672, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat parentInstance = new DynelStat(44, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat parentType = new DynelStat(43, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat parry = new DynelStat(145, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentChemicalDamage = new DynelStat(628, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentColdDamage = new DynelStat(622, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentEnergyDamage = new DynelStat(627, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentFireDamage = new DynelStat(621, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentMeleeDamage = new DynelStat(623, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentPoisonDamage = new DynelStat(625, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentProjectileDamage = new DynelStat(624, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentRadiationDamage = new DynelStat(626, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentRemainingHealth = new DynelStat(525, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat percentRemainingNano = new DynelStat(526, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat perception = new DynelStat(136, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat personalResearchGoal = new DynelStat(265, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat personalResearchLevel = new DynelStat(263, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petCounter = new DynelStat(251, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petMaster = new DynelStat(196, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petReq1 = new DynelStat(467, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petReq2 = new DynelStat(468, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petReq3 = new DynelStat(469, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petReqVal1 = new DynelStat(485, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petReqVal2 = new DynelStat(486, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petReqVal3 = new DynelStat(487, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petState = new DynelStat(671, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat petType = new DynelStat(512, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pharmaceuticals = new DynelStat(159, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat physicalProwessInitiative = new DynelStat(120, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat piercing = new DynelStat(106, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pistol = new DynelStat(112, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat placement = new DynelStat(298, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat playerId = new DynelStat(607, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat playerKilling = new DynelStat(323, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat playerOptions = new DynelStat(576, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat playfieldType = new DynelStat(438, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat poisonAC = new DynelStat(96, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat poisonDamageModifier = new DynelStat(317, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat prevMovementMode = new DynelStat(174, 3, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat previousHealth = new DynelStat(11, 50, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat price = new DynelStat(74, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat primaryItemInstance = new DynelStat(81, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat primaryItemType = new DynelStat(80, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat primaryTemplateId = new DynelStat(395, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procChance1 = new DynelStat(556, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procChance2 = new DynelStat(557, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procChance3 = new DynelStat(558, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procChance4 = new DynelStat(559, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procInitiative1 = new DynelStat(539, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procInitiative2 = new DynelStat(540, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procInitiative3 = new DynelStat(541, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procInitiative4 = new DynelStat(542, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procNano1 = new DynelStat(552, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procNano2 = new DynelStat(553, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procNano3 = new DynelStat(554, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat procNano4 = new DynelStat(555, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat profession = new DynelStat(60, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat professionLevel = new DynelStat(10, 1234567890, false, true, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat projectileAC = new DynelStat(90, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat projectileDamageModifier = new DynelStat(278, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat proximityRangeIndoors = new DynelStat(484, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat proximityRangeOutdoors = new DynelStat(454, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat psychic = new DynelStat(21, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat psychologicalModification = new DynelStat(129, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat psychology = new DynelStat(162, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvPLevelsKilled = new DynelStat(255, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpDuelDeaths = new DynelStat(675, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpDuelKills = new DynelStat(674, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpDuelScore = new DynelStat(684, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpProfessionDuelDeaths = new DynelStat(677, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpProfessionDuelKills = new DynelStat(676, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpRankedSoloDeaths = new DynelStat(679, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpRankedSoloKills = new DynelStat(678, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpRankedTeamDeaths = new DynelStat(681, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpRankedTeamKills = new DynelStat(680, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpRating = new DynelStat(333, 1300, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpSoloScore = new DynelStat(682, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat pvpTeamScore = new DynelStat(683, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtDungeonInstance = new DynelStat(497, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKillNumMonsterCount1 = new DynelStat(504, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKillNumMonsterCount2 = new DynelStat(506, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKillNumMonsterCount3 = new DynelStat(508, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKillNumMonsterID3 = new DynelStat(507, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKillNumMonsterId1 = new DynelStat(503, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKillNumMonsterId2 = new DynelStat(505, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtKilledMonsters = new DynelStat(499, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat qtNumMonsters = new DynelStat(498, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questAsMaximumRange = new DynelStat(802, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questAsMinimumRange = new DynelStat(801, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questBoothDifficulty = new DynelStat(800, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questIndex0 = new DynelStat(509, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questIndex1 = new DynelStat(492, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questIndex2 = new DynelStat(493, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questIndex3 = new DynelStat(494, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questIndex4 = new DynelStat(495, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questIndex5 = new DynelStat(496, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questInstance = new DynelStat(491, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questLevelsSolved = new DynelStat(253, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questStat = new DynelStat(261, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat questTimeout = new DynelStat(510, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat race = new DynelStat(89, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat radiationAC = new DynelStat(94, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat radiationDamageModifier = new DynelStat(282, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rangeIncreaserNF = new DynelStat(381, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rangeIncreaserWeapon = new DynelStat(380, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat readOnly = new DynelStat(435, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rechargeDelay = new DynelStat(210, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rechargeDelayCap = new DynelStat(524, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reclaimItem = new DynelStat(365, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectChemicalAC = new DynelStat(208, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectColdAC = new DynelStat(217, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectEnergyAC = new DynelStat(207, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectFireAC = new DynelStat(219, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectMeleeAC = new DynelStat(206, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectNanoAC = new DynelStat(218, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectPoisonAC = new DynelStat(225, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectProjectileAC = new DynelStat(205, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectRadiationAC = new DynelStat(216, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedChemicalAC = new DynelStat(478, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedColdAC = new DynelStat(480, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedEnergyAC = new DynelStat(477, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedFireAC = new DynelStat(482, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedMeleeAC = new DynelStat(476, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedNanoAC = new DynelStat(481, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedPoisonAC = new DynelStat(483, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedProjectileAC = new DynelStat(475, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat reflectReturnedRadiationAC = new DynelStat(479, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat regainXPPercentage = new DynelStat(593, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat repairDifficulty = new DynelStat(73, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat repairSkill = new DynelStat(77, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat resistModifier = new DynelStat(393, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat restModifier = new DynelStat(425, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat resurrectDest = new DynelStat(362, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rifle = new DynelStat(113, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat riposte = new DynelStat(143, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat ritualTargetInst = new DynelStat(370, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rnd = new DynelStat(520, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rotation = new DynelStat(400, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat rp = new DynelStat(199, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat runSpeed = new DynelStat(156, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat savedXP = new DynelStat(334, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat school = new DynelStat(405, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat secondaryItemInstance = new DynelStat(83, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat secondaryItemTemplate = new DynelStat(273, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat secondaryItemType = new DynelStat(82, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat selectedTarget = new DynelStat(431, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat selectedTargetType = new DynelStat(397, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sellModifier = new DynelStat(427, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sense = new DynelStat(20, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat senseImprovement = new DynelStat(122, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sessionTime = new DynelStat(198, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sex = new DynelStat(59, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shadowBreed = new DynelStat(532, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shadowBreedTemplate = new DynelStat(579, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldChemicalAC = new DynelStat(229, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldColdAC = new DynelStat(231, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldEnergyAC = new DynelStat(228, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldFireAC = new DynelStat(233, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldMeleeAC = new DynelStat(227, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldNanoAC = new DynelStat(232, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldPoisonAC = new DynelStat(234, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldProjectileAC = new DynelStat(226, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shieldRadiationAC = new DynelStat(230, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopFlags = new DynelStat(610, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopId = new DynelStat(657, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopIndex = new DynelStat(656, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopLastUsed = new DynelStat(611, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopPrice = new DynelStat(599, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopRent = new DynelStat(608, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shopType = new DynelStat(612, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shotgun = new DynelStat(115, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shoulderMeshHolder = new DynelStat(39, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shoulderMeshLeft = new DynelStat(1005, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat shoulderMeshRight = new DynelStat(1004, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat side = new DynelStat(33, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sisCap = new DynelStat(352, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sk = new DynelStat(573, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat skillDisabled = new DynelStat(329, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat skillLockModifier = new DynelStat(382, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat skillTimeOnSelectedTarget = new DynelStat(371, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat sneakAttack = new DynelStat(146, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat socialStatus = new DynelStat(521, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat soundVolume = new DynelStat(250, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat specialAttackShield = new DynelStat(517, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat specialCondition = new DynelStat(348, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat specialization = new DynelStat(182, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat speedPenalty = new DynelStat(70, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stability = new DynelStat(202, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stackingLine2 = new DynelStat(546, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stackingLine3 = new DynelStat(547, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stackingLine4 = new DynelStat(548, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stackingLine5 = new DynelStat(549, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stackingLine6 = new DynelStat(550, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stackingOrder = new DynelStat(551, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stamina = new DynelStat(18, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat statOne = new DynelStat(290, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat state = new DynelStat(7, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stateAction = new DynelStat(98, 1234567890, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat stateMachine = new DynelStat(450, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat staticInstance = new DynelStat(23, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat staticType = new DynelStat(25, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat streamCheckMagic = new DynelStat(999, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat strength = new DynelStat(16, 0, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat subMachineGun = new DynelStat(114, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat swim = new DynelStat(138, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat synergyHash = new DynelStat(609, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat taboo = new DynelStat(327, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat targetDistance = new DynelStat(527, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat targetDistanceChange = new DynelStat(889, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat targetFacing = new DynelStat(488, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat team = new DynelStat(6, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat teamAllowed = new DynelStat(324, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat teamCloseness = new DynelStat(528, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat teamSide = new DynelStat(213, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat teleportPauseMilliSeconds = new DynelStat(351, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tempSavePlayfield = new DynelStat(595, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tempSaveTeamId = new DynelStat(594, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tempSaveX = new DynelStat(596, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tempSaveY = new DynelStat(597, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat temporarySkillReduction = new DynelStat(247, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat throwingKnife = new DynelStat(108, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat thrownGrapplingWeapons = new DynelStat(110, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tideRequiredDynelId = new DynelStat(900, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat timeExist = new DynelStat(8, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat timeSinceCreation = new DynelStat(56, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat timeSinceUpkeep = new DynelStat(313, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat titleLevel = new DynelStat(37, 1, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat totalDamage = new DynelStat(629, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat totalMass = new DynelStat(71, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat towerInstance = new DynelStat(515, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat towerNpcHash = new DynelStat(511, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat towerType = new DynelStat(388, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tracerEffectType = new DynelStat(419, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackChemicalDamage = new DynelStat(633, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackColdDamage = new DynelStat(635, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackEnergyDamage = new DynelStat(632, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackFireDamage = new DynelStat(637, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackMeleeDamage = new DynelStat(631, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackPoisonDamage = new DynelStat(636, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackProjectileDamage = new DynelStat(630, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trackRadiationDamage = new DynelStat(634, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tradeLimit = new DynelStat(346, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trainSkill = new DynelStat(408, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trainSkillCost = new DynelStat(409, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat trapDifficulty = new DynelStat(289, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat travelSound = new DynelStat(271, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat treatment = new DynelStat(124, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat turnSpeed = new DynelStat(267, 40000, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat tutoring = new DynelStat(141, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat twohBluntWeapons = new DynelStat(107, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat twohEdgedWeapons = new DynelStat(105, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat unarmedTemplateInstance = new DynelStat(418, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat unreadMailCount = new DynelStat(649, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat unsavedXP = new DynelStat(592, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat userInstance = new DynelStat(85, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat userType = new DynelStat(84, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat vehicleAC = new DynelStat(664, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat vehicleDamage = new DynelStat(665, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat vehicleHealth = new DynelStat(666, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat vehicleSpeed = new DynelStat(667, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat veteranPoints = new DynelStat(68, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat vicinityRange = new DynelStat(448, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat victoryPoints = new DynelStat(669, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat visualBreed = new DynelStat(367, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat visualFlags = new DynelStat(673, 31, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat visualLodLevel = new DynelStat(888, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat visualProfession = new DynelStat(368, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat visualSex = new DynelStat(369, 1234567890, false, false, true);

        /// <summary>
        /// </summary>
        private readonly DynelStat volumeMass = new DynelStat(2, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat voteCount = new DynelStat(306, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat waitState = new DynelStat(430, 2, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponDisallowedInstance = new DynelStat(326, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponDisallowedType = new DynelStat(325, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponMeshHolder = new DynelStat(209, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponMeshLeft = new DynelStat(1007, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponMeshRight = new DynelStat(1006, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponSmithing = new DynelStat(158, 5, true, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponStyleLeft = new DynelStat(1015, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponStyleRight = new DynelStat(1016, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat weaponsStyle = new DynelStat(1003, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat xp = new DynelStat(52, 0, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat xpBonus = new DynelStat(341, 1234567890, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat xpKillRange = new DynelStat(275, 5, false, false, false);

        /// <summary>
        /// </summary>
        private readonly DynelStat xpModifier = new DynelStat(319, 0, false, false, false);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Character_Stats
        /// Class for character's stats
        /// </summary>
        /// <param name="parent">
        /// Stat's owner (Character or derived class)
        /// </param>
        public DynelStats()
        {
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
        public DynelStat AbsorbChemicalAC
        {
            get
            {
                return this.absorbChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbColdAC
        {
            get
            {
                return this.absorbColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbEnergyAC
        {
            get
            {
                return this.absorbEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbFireAC
        {
            get
            {
                return this.absorbFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbMeleeAC
        {
            get
            {
                return this.absorbMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbNanoAC
        {
            get
            {
                return this.absorbNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbPoisonAC
        {
            get
            {
                return this.absorbPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbProjectileAC
        {
            get
            {
                return this.absorbProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AbsorbRadiationAC
        {
            get
            {
                return this.absorbRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AccessCount
        {
            get
            {
                return this.accessCount;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AccessGrant
        {
            get
            {
                return this.accessGrant;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AccessKey
        {
            get
            {
                return this.accessKey;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AccountFlags
        {
            get
            {
                return this.accountFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AccumulatedDamage
        {
            get
            {
                return this.accumulatedDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AcgEntranceStyles
        {
            get
            {
                return this.acgEntranceStyles;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AcgItemCategoryId
        {
            get
            {
                return this.acgItemCategoryId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AcgItemLevel
        {
            get
            {
                return this.acgItemLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AcgItemSeed
        {
            get
            {
                return this.acgItemSeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AcgItemTemplateId
        {
            get
            {
                return this.acgItemTemplateId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AcgItemTemplateId2
        {
            get
            {
                return this.acgItemTemplateId2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ActionCategory
        {
            get
            {
                return this.actionCategory;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AdvantageHash1
        {
            get
            {
                return this.advantageHash1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AdvantageHash2
        {
            get
            {
                return this.advantageHash2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AdvantageHash3
        {
            get
            {
                return this.advantageHash3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AdvantageHash4
        {
            get
            {
                return this.advantageHash4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AdvantageHash5
        {
            get
            {
                return this.advantageHash5;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Adventuring
        {
            get
            {
                return this.adventuring;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Age
        {
            get
            {
                return this.age;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AggDef
        {
            get
            {
                return this.aggDef;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Aggressiveness
        {
            get
            {
                return this.aggressiveness;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Agility
        {
            get
            {
                return this.agility;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AimedShot
        {
            get
            {
                return this.aimedShot;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AlienLevel
        {
            get
            {
                return this.alienLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AlienNextXP
        {
            get
            {
                return this.alienNextXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AlienXP
        {
            get
            {
                return this.alienXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Alignment
        {
            get
            {
                return this.alignment;
            }
        }

        /// <summary>
        /// </summary>
        public List<DynelStat> All
        {
            get
            {
                return this.all;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AmmoName
        {
            get
            {
                return this.ammoName;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AmmoType
        {
            get
            {
                return this.ammoType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Ams
        {
            get
            {
                return this.ams;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AmsCap
        {
            get
            {
                return this.amsCap;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AmsModifier
        {
            get
            {
                return this.amsModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Anim
        {
            get
            {
                return this.anim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AnimPlay
        {
            get
            {
                return this.animPlay;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AnimPos
        {
            get
            {
                return this.animPos;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AnimSet
        {
            get
            {
                return this.animSet;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AnimSpeed
        {
            get
            {
                return this.animSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ApartmentAccessCard
        {
            get
            {
                return this.apartmentAccessCard;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ApartmentsAllowed
        {
            get
            {
                return this.apartmentsAllowed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ApartmentsOwned
        {
            get
            {
                return this.apartmentsOwned;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AreaInstance
        {
            get
            {
                return this.areaInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AreaType
        {
            get
            {
                return this.areaType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ArmourType
        {
            get
            {
                return this.armourType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AssaultRifle
        {
            get
            {
                return this.assaultRifle;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AttackCount
        {
            get
            {
                return this.attackCount;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AttackRange
        {
            get
            {
                return this.attackRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AttackShield
        {
            get
            {
                return this.attackShield;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AttackSpeed
        {
            get
            {
                return this.attackSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AttackType
        {
            get
            {
                return this.attackType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Attitude
        {
            get
            {
                return this.attitude;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AutoAttackFlags
        {
            get
            {
                return this.autoAttackFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AutoLockTimeDefault
        {
            get
            {
                return this.autoLockTimeDefault;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat AutoUnlockTimeDefault
        {
            get
            {
                return this.autoUnlockTimeDefault;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BackMesh
        {
            get
            {
                return this.backMesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Backstab
        {
            get
            {
                return this.backstab;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BandolierSlots
        {
            get
            {
                return this.bandolierSlots;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BattlestationRep
        {
            get
            {
                return this.battlestationRep;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BattlestationSide
        {
            get
            {
                return this.battlestationSide;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BeltSlots
        {
            get
            {
                return this.beltSlots;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BerserkMode
        {
            get
            {
                return this.berserkMode;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BiologicalMetamorphose
        {
            get
            {
                return this.biologicalMetamorphose;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BirthDate
        {
            get
            {
                return this.birthDate;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BodyDevelopment
        {
            get
            {
                return this.bodyDevelopment;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Bow
        {
            get
            {
                return this.bow;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BowSpecialAttack
        {
            get
            {
                return this.bowSpecialAttack;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BrainType
        {
            get
            {
                return this.brainType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Brawl
        {
            get
            {
                return this.brawl;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BreakingEntry
        {
            get
            {
                return this.breakingEntry;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Breed
        {
            get
            {
                return this.breed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BreedHostility
        {
            get
            {
                return this.breedHostility;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BreedLimit
        {
            get
            {
                return this.breedLimit;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BuildingComplexInst
        {
            get
            {
                return this.buildingComplexInst;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BuildingInstance
        {
            get
            {
                return this.buildingInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BuildingType
        {
            get
            {
                return this.buildingType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Burst
        {
            get
            {
                return this.burst;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BurstRecharge
        {
            get
            {
                return this.burstRecharge;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat BuyModifier
        {
            get
            {
                return this.buyModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Can
        {
            get
            {
                return this.can;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CanChangeClothes
        {
            get
            {
                return this.canChangeClothes;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CardOwnerInstance
        {
            get
            {
                return this.cardOwnerInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CardOwnerType
        {
            get
            {
                return this.cardOwnerType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Cash
        {
            get
            {
                return this.cash;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CastEffectType
        {
            get
            {
                return this.castEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CastSelfAbstractAnim
        {
            get
            {
                return this.castSelfAbstractAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CastSound
        {
            get
            {
                return this.castSound;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CastTargetAbstractAnim
        {
            get
            {
                return this.castTargetAbstractAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CatAnim
        {
            get
            {
                return this.catAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CatAnimFlags
        {
            get
            {
                return this.catAnimFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CatMesh
        {
            get
            {
                return this.catMesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChanceOfBreakOnDebuff
        {
            get
            {
                return this.chanceOfBreakOnDebuff;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChanceOfBreakOnSpellAttack
        {
            get
            {
                return this.chanceOfBreakOnSpellAttack;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChanceOfUse
        {
            get
            {
                return this.chanceOfUse;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChangeSideCount
        {
            get
            {
                return this.changeSideCount;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CharRadius
        {
            get
            {
                return this.charRadius;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CharState
        {
            get
            {
                return this.charState;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CharTmp1
        {
            get
            {
                return this.charTmp1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CharTmp2
        {
            get
            {
                return this.charTmp2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CharTmp3
        {
            get
            {
                return this.charTmp3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CharTmp4
        {
            get
            {
                return this.charTmp4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChemicalAC
        {
            get
            {
                return this.chemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChemicalDamageModifier
        {
            get
            {
                return this.chemicalDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Chemistry
        {
            get
            {
                return this.chemistry;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ChestFlags
        {
            get
            {
                return this.chestFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CityInstance
        {
            get
            {
                return this.cityInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CityTerminalRechargePercent
        {
            get
            {
                return this.cityTerminalRechargePercent;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Clan
        {
            get
            {
                return this.clan;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanConserver
        {
            get
            {
                return this.clanConserver;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanDevoted
        {
            get
            {
                return this.clanDevoted;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanFinalized
        {
            get
            {
                return this.clanFinalized;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanGaia
        {
            get
            {
                return this.clanGaia;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanHierarchy
        {
            get
            {
                return this.clanHierarchy;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanInstance
        {
            get
            {
                return this.clanInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanItemInstance
        {
            get
            {
                return this.clanItemInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanItemType
        {
            get
            {
                return this.clanItemType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanLevel
        {
            get
            {
                return this.clanLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanPrice
        {
            get
            {
                return this.clanPrice;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanRedeemed
        {
            get
            {
                return this.clanRedeemed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanSentinels
        {
            get
            {
                return this.clanSentinels;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanType
        {
            get
            {
                return this.clanType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanUpkeepInterval
        {
            get
            {
                return this.clanUpkeepInterval;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClanVanguards
        {
            get
            {
                return this.clanVanguards;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ClientActivated
        {
            get
            {
                return this.clientActivated;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CloseCombatInitiative
        {
            get
            {
                return this.closeCombatInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ColdAC
        {
            get
            {
                return this.coldAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ColdDamageModifier
        {
            get
            {
                return this.coldDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CollideCheckInterval
        {
            get
            {
                return this.collideCheckInterval;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CollisionRadius
        {
            get
            {
                return this.collisionRadius;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CommandRange
        {
            get
            {
                return this.commandRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Compulsion
        {
            get
            {
                return this.compulsion;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ComputerLiteracy
        {
            get
            {
                return this.computerLiteracy;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Concealment
        {
            get
            {
                return this.concealment;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ConditionState
        {
            get
            {
                return this.conditionState;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Conformity
        {
            get
            {
                return this.conformity;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CorpseAnimKey
        {
            get
            {
                return this.corpseAnimKey;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CorpseHash
        {
            get
            {
                return this.corpseHash;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CorpseInstance
        {
            get
            {
                return this.corpseInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CorpseType
        {
            get
            {
                return this.corpseType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CriticalDecrease
        {
            get
            {
                return this.criticalDecrease;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CriticalIncrease
        {
            get
            {
                return this.criticalIncrease;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrBodyLocation
        {
            get
            {
                return this.currBodyLocation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentMass
        {
            get
            {
                return this.currentMass;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentMovementMode
        {
            get
            {
                return this.currentMovementMode;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentNano
        {
            get
            {
                return this.currentNano;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentNcu
        {
            get
            {
                return this.currentNCU;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentPlayfield
        {
            get
            {
                return this.currentPlayfield;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentState
        {
            get
            {
                return this.currentState;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat CurrentTime
        {
            get
            {
                return this.currentTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DamageBonus
        {
            get
            {
                return this.damageBonus;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DamageOverrideType
        {
            get
            {
                return this.damageOverrideType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DamageToNano
        {
            get
            {
                return this.damageToNano;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DamageToNanoMultiplier
        {
            get
            {
                return this.damageToNanoMultiplier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DamageType
        {
            get
            {
                return this.damageType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DeadTimer
        {
            get
            {
                return this.deadTimer;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DeathReason
        {
            get
            {
                return this.deathReason;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DebuffFormula
        {
            get
            {
                return this.debuffFormula;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DefaultAttackType
        {
            get
            {
                return this.defaultAttackType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DefaultPos
        {
            get
            {
                return this.defaultPos;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DesiredTargetDistance
        {
            get
            {
                return this.desiredTargetDistance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DieAnim
        {
            get
            {
                return this.dieAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Dimach
        {
            get
            {
                return this.dimach;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DisarmTrap
        {
            get
            {
                return this.disarmTrap;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DisplayCatAnim
        {
            get
            {
                return this.displayCATAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DisplayCatMesh
        {
            get
            {
                return this.displayCATMesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DistanceToSpawnpoint
        {
            get
            {
                return this.distanceToSpawnpoint;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DistanceWeaponInitiative
        {
            get
            {
                return this.distanceWeaponInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DistrictNano
        {
            get
            {
                return this.districtNano;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DistrictNanoInterval
        {
            get
            {
                return this.districtNanoInterval;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Dms
        {
            get
            {
                return this.dms;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DmsModifier
        {
            get
            {
                return this.dmsModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Dodge
        {
            get
            {
                return this.dodge;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DoorBlockTime
        {
            get
            {
                return this.doorBlockTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DoorFlags
        {
            get
            {
                return this.doorFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DriveAir
        {
            get
            {
                return this.driveAir;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DriveGround
        {
            get
            {
                return this.driveGround;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DriveWater
        {
            get
            {
                return this.driveWater;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Duck
        {
            get
            {
                return this.duck;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DudChance
        {
            get
            {
                return this.dudChance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat DurationModifier
        {
            get
            {
                return this.durationModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EffectBlue
        {
            get
            {
                return this.effectBlue;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EffectGreen
        {
            get
            {
                return this.effectGreen;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EffectIcon
        {
            get
            {
                return this.effectIcon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EffectRed
        {
            get
            {
                return this.effectRed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EffectType
        {
            get
            {
                return this.effectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ElectricalEngineering
        {
            get
            {
                return this.electricalEngineering;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Energy
        {
            get
            {
                return this.energy;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EnergyAC
        {
            get
            {
                return this.energyAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EnergyDamageModifier
        {
            get
            {
                return this.energyDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EquipDelay
        {
            get
            {
                return this.equipDelay;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat EquippedWeapons
        {
            get
            {
                return this.equippedWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Evade
        {
            get
            {
                return this.evade;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ExitInstance
        {
            get
            {
                return this.exitInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Expansion
        {
            get
            {
                return this.expansion;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ExpansionPlayfield
        {
            get
            {
                return this.expansionPlayfield;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ExtenalDoorInstance
        {
            get
            {
                return this.extenalDoorInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ExtenalPlayfieldInstance
        {
            get
            {
                return this.extenalPlayfieldInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ExtendedFlags
        {
            get
            {
                return this.extendedFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ExtendedTime
        {
            get
            {
                return this.extendedTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Extroverty
        {
            get
            {
                return this.extroverty;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FabricType
        {
            get
            {
                return this.fabricType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Face
        {
            get
            {
                return this.face;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FaceTexture
        {
            get
            {
                return this.faceTexture;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FactionModifier
        {
            get
            {
                return this.factionModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FallDamage
        {
            get
            {
                return this.fallDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FastAttack
        {
            get
            {
                return this.fastAttack;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Fatness
        {
            get
            {
                return this.fatness;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Features
        {
            get
            {
                return this.features;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FieldQuantumPhysics
        {
            get
            {
                return this.fieldQuantumPhysics;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FireAC
        {
            get
            {
                return this.fireAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FireDamageModifier
        {
            get
            {
                return this.fireDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FirstAid
        {
            get
            {
                return this.firstAid;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FixtureFlags
        {
            get
            {
                return this.fixtureFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Flags
        {
            get
            {
                return this.flags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FlingShot
        {
            get
            {
                return this.flingShot;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FullAuto
        {
            get
            {
                return this.fullAuto;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat FullAutoRecharge
        {
            get
            {
                return this.fullAutoRecharge;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GMLevel
        {
            get
            {
                return this.gmLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GatherAbstractAnim
        {
            get
            {
                return this.gatherAbstractAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GatherEffectType
        {
            get
            {
                return this.gatherEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GatherSound
        {
            get
            {
                return this.gatherSound;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GenderLimit
        {
            get
            {
                return this.genderLimit;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GlobalClanInstance
        {
            get
            {
                return this.globalClanInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GlobalClanType
        {
            get
            {
                return this.globalClanType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GlobalResearchGoal
        {
            get
            {
                return this.globalResearchGoal;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat GlobalResearchLevel
        {
            get
            {
                return this.globalResearchLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Gos
        {
            get
            {
                return this.gos;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Grenade
        {
            get
            {
                return this.grenade;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HPLevelUp
        {
            get
            {
                return this.hpLevelUp;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HPPerSkill
        {
            get
            {
                return this.hpPerSkill;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HairMesh
        {
            get
            {
                return this.hairMesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HasAlwaysLootable
        {
            get
            {
                return this.hasAlwaysLootable;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HasKnuBotData
        {
            get
            {
                return this.hasKnuBotData;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HateValueModifyer
        {
            get
            {
                return this.hateValueModifyer;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HeadMesh
        {
            get
            {
                return this.headMesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HealDelta
        {
            get
            {
                return this.healDelta;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HealInterval
        {
            get
            {
                return this.healInterval;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HealMultiplier
        {
            get
            {
                return this.healMultiplier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Health
        {
            get
            {
                return this.health;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HealthChange
        {
            get
            {
                return this.healthChange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HealthChangeBest
        {
            get
            {
                return this.healthChangeBest;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HealthChangeWorst
        {
            get
            {
                return this.healthChangeWorst;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HitEffectType
        {
            get
            {
                return this.hitEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HitSound
        {
            get
            {
                return this.hitSound;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat HouseTemplate
        {
            get
            {
                return this.houseTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat IP
        {
            get
            {
                return this.ip;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Icon
        {
            get
            {
                return this.icon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ImpactEffectType
        {
            get
            {
                return this.impactEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InPlay
        {
            get
            {
                return this.inPlay;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Info
        {
            get
            {
                return this.info;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InitiativeType
        {
            get
            {
                return this.initiativeType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Instance
        {
            get
            {
                return this.instance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InsurancePercentage
        {
            get
            {
                return this.insurancePercentage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InsuranceTime
        {
            get
            {
                return this.insuranceTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Intelligence
        {
            get
            {
                return this.intelligence;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InteractionRadius
        {
            get
            {
                return this.interactionRadius;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InterruptModifier
        {
            get
            {
                return this.interruptModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InvadersKilled
        {
            get
            {
                return this.invadersKilled;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InventoryId
        {
            get
            {
                return this.inventoryId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat InventoryTimeout
        {
            get
            {
                return this.inventoryTimeout;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat IsFightingMe
        {
            get
            {
                return this.isFightingMe;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat IsVehicle
        {
            get
            {
                return this.isVehicle;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemAnim
        {
            get
            {
                return this.itemAnim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemClass
        {
            get
            {
                return this.itemClass;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemDelay
        {
            get
            {
                return this.itemDelay;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemDelayCap
        {
            get
            {
                return this.itemDelayCap;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemHateValue
        {
            get
            {
                return this.itemHateValue;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemOpposedSkill
        {
            get
            {
                return this.itemOpposedSkill;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemSis
        {
            get
            {
                return this.itemSIS;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemSkill
        {
            get
            {
                return this.itemSkill;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat KilledByInvaders
        {
            get
            {
                return this.killedByInvaders;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LREnergyWeapon
        {
            get
            {
                return this.lrEnergyWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LRMultipleWeapon
        {
            get
            {
                return this.lrMultipleWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastConcretePlayfieldInstance
        {
            get
            {
                return this.lastConcretePlayfieldInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastMailCheckTime
        {
            get
            {
                return this.lastMailCheckTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastPerkResetTime
        {
            get
            {
                return this.lastPerkResetTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastRnd
        {
            get
            {
                return this.lastRnd;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastSK
        {
            get
            {
                return this.lastSK;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastSaveXP
        {
            get
            {
                return this.lastSaveXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastSaved
        {
            get
            {
                return this.lastSaved;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LastXP
        {
            get
            {
                return this.lastXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LeaderLockDownTime
        {
            get
            {
                return this.leaderLockDownTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Level
        {
            get
            {
                return this.level;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LevelLimit
        {
            get
            {
                return this.levelLimit;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Life
        {
            get
            {
                return this.life;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LiquidType
        {
            get
            {
                return this.liquidType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LockDifficulty
        {
            get
            {
                return this.lockDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LockDownTime
        {
            get
            {
                return this.lockDownTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LosHeight
        {
            get
            {
                return this.losHeight;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat LowresMesh
        {
            get
            {
                return this.lowresMesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapAreaPart1
        {
            get
            {
                return this.mapAreaPart1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapAreaPart2
        {
            get
            {
                return this.mapAreaPart2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapAreaPart3
        {
            get
            {
                return this.mapAreaPart3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapAreaPart4
        {
            get
            {
                return this.mapAreaPart4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapFlags
        {
            get
            {
                return this.mapFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapNavigation
        {
            get
            {
                return this.mapNavigation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MapOptions
        {
            get
            {
                return this.mapOptions;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MartialArts
        {
            get
            {
                return this.martialArts;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaterialCreation
        {
            get
            {
                return this.materialCreation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaterialLocation
        {
            get
            {
                return this.materialLocation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaterialMetamorphose
        {
            get
            {
                return this.materialMetamorphose;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaxDamage
        {
            get
            {
                return this.maxDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaxEnergy
        {
            get
            {
                return this.maxEnergy;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaxMass
        {
            get
            {
                return this.maxMass;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaxNanoEnergy
        {
            get
            {
                return this.maxNanoEnergy;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaxNcu
        {
            get
            {
                return this.maxNCU;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MaxShopItems
        {
            get
            {
                return this.maxShopItems;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MechData
        {
            get
            {
                return this.mechData;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MechanicalEngineering
        {
            get
            {
                return this.mechanicalEngineering;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MeleeAC
        {
            get
            {
                return this.meleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MeleeDamageModifier
        {
            get
            {
                return this.meleeDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MeleeEnergyWeapon
        {
            get
            {
                return this.meleeEnergyWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MeleeMultiple
        {
            get
            {
                return this.meleeMultiple;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MemberInstance
        {
            get
            {
                return this.memberInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MemberType
        {
            get
            {
                return this.memberType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Members
        {
            get
            {
                return this.members;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Mesh
        {
            get
            {
                return this.mesh;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MetaType
        {
            get
            {
                return this.metaType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MetersWalked
        {
            get
            {
                return this.metersWalked;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MinDamage
        {
            get
            {
                return this.minDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MinMembers
        {
            get
            {
                return this.minMembers;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits1
        {
            get
            {
                return this.missionBits1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits10
        {
            get
            {
                return this.missionBits10;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits11
        {
            get
            {
                return this.missionBits11;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits12
        {
            get
            {
                return this.missionBits12;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits2
        {
            get
            {
                return this.missionBits2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits3
        {
            get
            {
                return this.missionBits3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits4
        {
            get
            {
                return this.missionBits4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits5
        {
            get
            {
                return this.missionBits5;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits6
        {
            get
            {
                return this.missionBits6;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits7
        {
            get
            {
                return this.missionBits7;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits8
        {
            get
            {
                return this.missionBits8;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MissionBits9
        {
            get
            {
                return this.missionBits9;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MonsterData
        {
            get
            {
                return this.monsterData;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MonsterLevelsKilled
        {
            get
            {
                return this.monsterLevelsKilled;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MonsterScale
        {
            get
            {
                return this.monsterScale;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MonsterTexture
        {
            get
            {
                return this.monsterTexture;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MonthsPaid
        {
            get
            {
                return this.monthsPaid;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MoreFlags
        {
            get
            {
                return this.moreFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat MultipleCount
        {
            get
            {
                return this.multipleCount;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NPCostModifier
        {
            get
            {
                return this.npCostModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NPLevelUp
        {
            get
            {
                return this.npLevelUp;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NPPerSkill
        {
            get
            {
                return this.npPerSkill;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NameTemplate
        {
            get
            {
                return this.nameTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoAC
        {
            get
            {
                return this.nanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoDamageModifier
        {
            get
            {
                return this.nanoDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoDamageMultiplier
        {
            get
            {
                return this.nanoDamageMultiplier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoDelta
        {
            get
            {
                return this.nanoDelta;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoEnergyPool
        {
            get
            {
                return this.nanoEnergyPool;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoFocusLevel
        {
            get
            {
                return this.nanoFocusLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoInterval
        {
            get
            {
                return this.nanoInterval;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoPoints
        {
            get
            {
                return this.nanoPoints;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoProgramming
        {
            get
            {
                return this.nanoProgramming;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoProwessInitiative
        {
            get
            {
                return this.nanoProwessInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoSpeed
        {
            get
            {
                return this.nanoSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NanoVulnerability
        {
            get
            {
                return this.nanoVulnerability;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NewbieHP
        {
            get
            {
                return this.newbieHP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NewbieNP
        {
            get
            {
                return this.newbieNP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NextDoorInBuilding
        {
            get
            {
                return this.nextDoorInBuilding;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NextFormula
        {
            get
            {
                return this.nextFormula;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NextSK
        {
            get
            {
                return this.nextSK;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NextXP
        {
            get
            {
                return this.nextXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcBrainState
        {
            get
            {
                return this.npcBrainState;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcCommand
        {
            get
            {
                return this.npcCommand;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcCommandArg
        {
            get
            {
                return this.npcCommandArg;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcCryForHelpRange
        {
            get
            {
                return this.npcCryForHelpRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcFamily
        {
            get
            {
                return this.npcFamily;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcFlags
        {
            get
            {
                return this.npcFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcFovStatus
        {
            get
            {
                return this.npcFovStatus;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcHasPatrolList
        {
            get
            {
                return this.npcHasPatrolList;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcHash
        {
            get
            {
                return this.npcHash;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcHatelistSize
        {
            get
            {
                return this.npcHatelistSize;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcIsSurrendering
        {
            get
            {
                return this.npcIsSurrendering;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcNumPets
        {
            get
            {
                return this.npcNumPets;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcScriptAmsScale
        {
            get
            {
                return this.npcScriptAmsScale;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcSpellArg1
        {
            get
            {
                return this.npcSpellArg1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcSpellRet1
        {
            get
            {
                return this.npcSpellRet1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcSurrenderInstance
        {
            get
            {
                return this.npcSurrenderInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcUseFightModeRegenRate
        {
            get
            {
                return this.npcUseFightModeRegenRate;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcVicinityChars
        {
            get
            {
                return this.npcVicinityChars;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcVicinityFamily
        {
            get
            {
                return this.npcVicinityFamily;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NpcVicinityPlayers
        {
            get
            {
                return this.npcVicinityPlayers;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NumAttackEffects
        {
            get
            {
                return this.numAttackEffects;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NumberOfItems
        {
            get
            {
                return this.numberOfItems;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NumberOfTeamMembers
        {
            get
            {
                return this.numberOfTeamMembers;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat NumberOnHateList
        {
            get
            {
                return this.numberOnHateList;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ODMaxSizeAdd
        {
            get
            {
                return this.odMaxSizeAdd;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ODMinSizeAdd
        {
            get
            {
                return this.odMinSizeAdd;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OTArmedForces
        {
            get
            {
                return this.otArmedForces;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OTFollowers
        {
            get
            {
                return this.otFollowers;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OTMed
        {
            get
            {
                return this.otMed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OTOperator
        {
            get
            {
                return this.otOperator;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OTTrans
        {
            get
            {
                return this.otTrans;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OTUnredeemed
        {
            get
            {
                return this.otUnredeemed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ObjectType
        {
            get
            {
                return this.objectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OldTimeExist
        {
            get
            {
                return this.oldTimeExist;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OnTowerCreation
        {
            get
            {
                return this.onTowerCreation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OnehBluntWeapons
        {
            get
            {
                return this.onehBluntWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OnehEdgedWeapon
        {
            get
            {
                return this.onehEdgedWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OrientationMode
        {
            get
            {
                return this.orientationMode;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OriginatorType
        {
            get
            {
                return this.originatorType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OuterRadius
        {
            get
            {
                return this.outerRadius;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideMaterial
        {
            get
            {
                return this.overrideMaterial;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTexture
        {
            get
            {
                return this.overrideTexture;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureAttractor
        {
            get
            {
                return this.overrideTextureAttractor;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureBack
        {
            get
            {
                return this.overrideTextureBack;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureHead
        {
            get
            {
                return this.overrideTextureHead;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureShoulderpadLeft
        {
            get
            {
                return this.overrideTextureShoulderpadLeft;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureShoulderpadRight
        {
            get
            {
                return this.overrideTextureShoulderpadRight;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureWeaponLeft
        {
            get
            {
                return this.overrideTextureWeaponLeft;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OverrideTextureWeaponRight
        {
            get
            {
                return this.overrideTextureWeaponRight;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OwnedTowers
        {
            get
            {
                return this.ownedTowers;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat OwnerInstance
        {
            get
            {
                return this.ownerInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PaidPoints
        {
            get
            {
                return this.paidPoints;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ParentInstance
        {
            get
            {
                return this.parentInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ParentType
        {
            get
            {
                return this.parentType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Parry
        {
            get
            {
                return this.parry;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentChemicalDamage
        {
            get
            {
                return this.percentChemicalDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentColdDamage
        {
            get
            {
                return this.percentColdDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentEnergyDamage
        {
            get
            {
                return this.percentEnergyDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentFireDamage
        {
            get
            {
                return this.percentFireDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentMeleeDamage
        {
            get
            {
                return this.percentMeleeDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentPoisonDamage
        {
            get
            {
                return this.percentPoisonDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentProjectileDamage
        {
            get
            {
                return this.percentProjectileDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentRadiationDamage
        {
            get
            {
                return this.percentRadiationDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentRemainingHealth
        {
            get
            {
                return this.percentRemainingHealth;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PercentRemainingNano
        {
            get
            {
                return this.percentRemainingNano;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Perception
        {
            get
            {
                return this.perception;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PersonalResearchGoal
        {
            get
            {
                return this.personalResearchGoal;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PersonalResearchLevel
        {
            get
            {
                return this.personalResearchLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetCounter
        {
            get
            {
                return this.petCounter;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetMaster
        {
            get
            {
                return this.petMaster;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetReq1
        {
            get
            {
                return this.petReq1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetReq2
        {
            get
            {
                return this.petReq2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetReq3
        {
            get
            {
                return this.petReq3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetReqVal1
        {
            get
            {
                return this.petReqVal1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetReqVal2
        {
            get
            {
                return this.petReqVal2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetReqVal3
        {
            get
            {
                return this.petReqVal3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetState
        {
            get
            {
                return this.petState;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PetType
        {
            get
            {
                return this.petType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Pharmaceuticals
        {
            get
            {
                return this.pharmaceuticals;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PhysicalProwessInitiative
        {
            get
            {
                return this.physicalProwessInitiative;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Piercing
        {
            get
            {
                return this.piercing;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Pistol
        {
            get
            {
                return this.pistol;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Placement
        {
            get
            {
                return this.placement;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PlayerId
        {
            get
            {
                return this.playerId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PlayerKilling
        {
            get
            {
                return this.playerKilling;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PlayerOptions
        {
            get
            {
                return this.playerOptions;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PlayfieldType
        {
            get
            {
                return this.playfieldType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PoisonAC
        {
            get
            {
                return this.poisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PoisonDamageModifier
        {
            get
            {
                return this.poisonDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PrevMovementMode
        {
            get
            {
                return this.prevMovementMode;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PreviousHealth
        {
            get
            {
                return this.previousHealth;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Price
        {
            get
            {
                return this.price;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PrimaryItemInstance
        {
            get
            {
                return this.primaryItemInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PrimaryItemType
        {
            get
            {
                return this.primaryItemType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PrimaryTemplateId
        {
            get
            {
                return this.primaryTemplateId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcChance1
        {
            get
            {
                return this.procChance1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcChance2
        {
            get
            {
                return this.procChance2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcChance3
        {
            get
            {
                return this.procChance3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcChance4
        {
            get
            {
                return this.procChance4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcInitiative1
        {
            get
            {
                return this.procInitiative1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcInitiative2
        {
            get
            {
                return this.procInitiative2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcInitiative3
        {
            get
            {
                return this.procInitiative3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcInitiative4
        {
            get
            {
                return this.procInitiative4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcNano1
        {
            get
            {
                return this.procNano1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcNano2
        {
            get
            {
                return this.procNano2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcNano3
        {
            get
            {
                return this.procNano3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProcNano4
        {
            get
            {
                return this.procNano4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Profession
        {
            get
            {
                return this.profession;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProfessionLevel
        {
            get
            {
                return this.professionLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProjectileAC
        {
            get
            {
                return this.projectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProjectileDamageModifier
        {
            get
            {
                return this.projectileDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProximityRangeIndoors
        {
            get
            {
                return this.proximityRangeIndoors;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ProximityRangeOutdoors
        {
            get
            {
                return this.proximityRangeOutdoors;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Psychic
        {
            get
            {
                return this.psychic;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PsychologicalModification
        {
            get
            {
                return this.psychologicalModification;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Psychology
        {
            get
            {
                return this.psychology;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvPLevelsKilled
        {
            get
            {
                return this.pvPLevelsKilled;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpDuelDeaths
        {
            get
            {
                return this.pvpDuelDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpDuelKills
        {
            get
            {
                return this.pvpDuelKills;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpDuelScore
        {
            get
            {
                return this.pvpDuelScore;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpProfessionDuelDeaths
        {
            get
            {
                return this.pvpProfessionDuelDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpProfessionDuelKills
        {
            get
            {
                return this.pvpProfessionDuelKills;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpRankedSoloDeaths
        {
            get
            {
                return this.pvpRankedSoloDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpRankedSoloKills
        {
            get
            {
                return this.pvpRankedSoloKills;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpRankedTeamDeaths
        {
            get
            {
                return this.pvpRankedTeamDeaths;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpRankedTeamKills
        {
            get
            {
                return this.pvpRankedTeamKills;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpRating
        {
            get
            {
                return this.pvpRating;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpSoloScore
        {
            get
            {
                return this.pvpSoloScore;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat PvpTeamScore
        {
            get
            {
                return this.pvpTeamScore;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTDungeonInstance
        {
            get
            {
                return this.qtDungeonInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKillNumMonsterCount1
        {
            get
            {
                return this.qtKillNumMonsterCount1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKillNumMonsterCount2
        {
            get
            {
                return this.qtKillNumMonsterCount2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKillNumMonsterCount3
        {
            get
            {
                return this.qtKillNumMonsterCount3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKillNumMonsterId1
        {
            get
            {
                return this.qtKillNumMonsterId1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKillNumMonsterId2
        {
            get
            {
                return this.qtKillNumMonsterId2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKillNumMonsterId3
        {
            get
            {
                return this.qtKillNumMonsterID3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTKilledMonsters
        {
            get
            {
                return this.qtKilledMonsters;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QTNumMonsters
        {
            get
            {
                return this.qtNumMonsters;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestAsMaximumRange
        {
            get
            {
                return this.questAsMaximumRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestAsMinimumRange
        {
            get
            {
                return this.questAsMinimumRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestBoothDifficulty
        {
            get
            {
                return this.questBoothDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestIndex0
        {
            get
            {
                return this.questIndex0;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestIndex1
        {
            get
            {
                return this.questIndex1;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestIndex2
        {
            get
            {
                return this.questIndex2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestIndex3
        {
            get
            {
                return this.questIndex3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestIndex4
        {
            get
            {
                return this.questIndex4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestIndex5
        {
            get
            {
                return this.questIndex5;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestInstance
        {
            get
            {
                return this.questInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestLevelsSolved
        {
            get
            {
                return this.questLevelsSolved;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestStat
        {
            get
            {
                return this.questStat;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat QuestTimeout
        {
            get
            {
                return this.questTimeout;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RP
        {
            get
            {
                return this.rp;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Race
        {
            get
            {
                return this.race;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RadiationAC
        {
            get
            {
                return this.radiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RadiationDamageModifier
        {
            get
            {
                return this.radiationDamageModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RangeIncreaserNF
        {
            get
            {
                return this.rangeIncreaserNF;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RangeIncreaserWeapon
        {
            get
            {
                return this.rangeIncreaserWeapon;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReadOnly
        {
            get
            {
                return this.readOnly;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RechargeDelay
        {
            get
            {
                return this.rechargeDelay;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RechargeDelayCap
        {
            get
            {
                return this.rechargeDelayCap;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReclaimItem
        {
            get
            {
                return this.reclaimItem;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectChemicalAC
        {
            get
            {
                return this.reflectChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectColdAC
        {
            get
            {
                return this.reflectColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectEnergyAC
        {
            get
            {
                return this.reflectEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectFireAC
        {
            get
            {
                return this.reflectFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectMeleeAC
        {
            get
            {
                return this.reflectMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectNanoAC
        {
            get
            {
                return this.reflectNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectPoisonAC
        {
            get
            {
                return this.reflectPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectProjectileAC
        {
            get
            {
                return this.reflectProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectRadiationAC
        {
            get
            {
                return this.reflectRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedChemicalAC
        {
            get
            {
                return this.reflectReturnedChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedColdAC
        {
            get
            {
                return this.reflectReturnedColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedEnergyAC
        {
            get
            {
                return this.reflectReturnedEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedFireAC
        {
            get
            {
                return this.reflectReturnedFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedMeleeAC
        {
            get
            {
                return this.reflectReturnedMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedNanoAC
        {
            get
            {
                return this.reflectReturnedNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedPoisonAC
        {
            get
            {
                return this.reflectReturnedPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedProjectileAC
        {
            get
            {
                return this.reflectReturnedProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ReflectReturnedRadiationAC
        {
            get
            {
                return this.reflectReturnedRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RegainXPPercentage
        {
            get
            {
                return this.regainXPPercentage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RepairDifficulty
        {
            get
            {
                return this.repairDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RepairSkill
        {
            get
            {
                return this.repairSkill;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ResistModifier
        {
            get
            {
                return this.resistModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RestModifier
        {
            get
            {
                return this.restModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ResurrectDest
        {
            get
            {
                return this.resurrectDest;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Rifle
        {
            get
            {
                return this.rifle;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Riposte
        {
            get
            {
                return this.riposte;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RitualTargetInst
        {
            get
            {
                return this.ritualTargetInst;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Rnd
        {
            get
            {
                return this.rnd;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Rotation
        {
            get
            {
                return this.rotation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat RunSpeed
        {
            get
            {
                return this.runSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SK
        {
            get
            {
                return this.sk;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SavedXP
        {
            get
            {
                return this.savedXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat School
        {
            get
            {
                return this.school;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SecondaryItemInstance
        {
            get
            {
                return this.secondaryItemInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SecondaryItemTemplate
        {
            get
            {
                return this.secondaryItemTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SecondaryItemType
        {
            get
            {
                return this.secondaryItemType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SelectedTarget
        {
            get
            {
                return this.selectedTarget;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SelectedTargetType
        {
            get
            {
                return this.selectedTargetType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SellModifier
        {
            get
            {
                return this.sellModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Sense
        {
            get
            {
                return this.sense;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SenseImprovement
        {
            get
            {
                return this.senseImprovement;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SessionTime
        {
            get
            {
                return this.sessionTime;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Sex
        {
            get
            {
                return this.sex;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShadowBreed
        {
            get
            {
                return this.shadowBreed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShadowBreedTemplate
        {
            get
            {
                return this.shadowBreedTemplate;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldChemicalAC
        {
            get
            {
                return this.shieldChemicalAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldColdAC
        {
            get
            {
                return this.shieldColdAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldEnergyAC
        {
            get
            {
                return this.shieldEnergyAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldFireAC
        {
            get
            {
                return this.shieldFireAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldMeleeAC
        {
            get
            {
                return this.shieldMeleeAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldNanoAC
        {
            get
            {
                return this.shieldNanoAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldPoisonAC
        {
            get
            {
                return this.shieldPoisonAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldProjectileAC
        {
            get
            {
                return this.shieldProjectileAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShieldRadiationAC
        {
            get
            {
                return this.shieldRadiationAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopFlags
        {
            get
            {
                return this.shopFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopId
        {
            get
            {
                return this.shopId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopIndex
        {
            get
            {
                return this.shopIndex;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopLastUsed
        {
            get
            {
                return this.shopLastUsed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopPrice
        {
            get
            {
                return this.shopPrice;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopRent
        {
            get
            {
                return this.shopRent;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShopType
        {
            get
            {
                return this.shopType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Shotgun
        {
            get
            {
                return this.shotgun;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShoulderMeshHolder
        {
            get
            {
                return this.shoulderMeshHolder;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShoulderMeshLeft
        {
            get
            {
                return this.shoulderMeshLeft;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ShoulderMeshRight
        {
            get
            {
                return this.shoulderMeshRight;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Side
        {
            get
            {
                return this.side;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SisCap
        {
            get
            {
                return this.sisCap;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SkillDisabled
        {
            get
            {
                return this.skillDisabled;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SkillLockModifier
        {
            get
            {
                return this.skillLockModifier;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SkillTimeOnSelectedTarget
        {
            get
            {
                return this.skillTimeOnSelectedTarget;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SneakAttack
        {
            get
            {
                return this.sneakAttack;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SocialStatus
        {
            get
            {
                return this.socialStatus;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SoundVolume
        {
            get
            {
                return this.soundVolume;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SpecialAttackShield
        {
            get
            {
                return this.specialAttackShield;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SpecialCondition
        {
            get
            {
                return this.specialCondition;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Specialization
        {
            get
            {
                return this.specialization;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SpeedPenalty
        {
            get
            {
                return this.speedPenalty;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Stability
        {
            get
            {
                return this.stability;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StackingLine2
        {
            get
            {
                return this.stackingLine2;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StackingLine3
        {
            get
            {
                return this.stackingLine3;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StackingLine4
        {
            get
            {
                return this.stackingLine4;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StackingLine5
        {
            get
            {
                return this.stackingLine5;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StackingLine6
        {
            get
            {
                return this.stackingLine6;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StackingOrder
        {
            get
            {
                return this.stackingOrder;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Stamina
        {
            get
            {
                return this.stamina;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StatOne
        {
            get
            {
                return this.statOne;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StateAction
        {
            get
            {
                return this.stateAction;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StateMachine
        {
            get
            {
                return this.stateMachine;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StaticInstance
        {
            get
            {
                return this.staticInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StaticType
        {
            get
            {
                return this.staticType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat StreamCheckMagic
        {
            get
            {
                return this.streamCheckMagic;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Strength
        {
            get
            {
                return this.strength;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SubMachineGun
        {
            get
            {
                return this.subMachineGun;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Swim
        {
            get
            {
                return this.swim;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat SynergyHash
        {
            get
            {
                return this.synergyHash;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Taboo
        {
            get
            {
                return this.taboo;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TargetDistance
        {
            get
            {
                return this.targetDistance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TargetDistanceChange
        {
            get
            {
                return this.targetDistanceChange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TargetFacing
        {
            get
            {
                return this.targetFacing;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Team
        {
            get
            {
                return this.team;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TeamAllowed
        {
            get
            {
                return this.teamAllowed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TeamCloseness
        {
            get
            {
                return this.teamCloseness;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TeamSide
        {
            get
            {
                return this.teamSide;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TeleportPauseMilliSeconds
        {
            get
            {
                return this.teleportPauseMilliSeconds;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TempSavePlayfield
        {
            get
            {
                return this.tempSavePlayfield;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TempSaveTeamId
        {
            get
            {
                return this.tempSaveTeamId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TempSaveX
        {
            get
            {
                return this.tempSaveX;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TempSaveY
        {
            get
            {
                return this.tempSaveY;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TemporarySkillReduction
        {
            get
            {
                return this.temporarySkillReduction;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ThrowingKnife
        {
            get
            {
                return this.throwingKnife;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat ThrownGrapplingWeapons
        {
            get
            {
                return this.thrownGrapplingWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TideRequiredDynelId
        {
            get
            {
                return this.tideRequiredDynelId;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TimeExist
        {
            get
            {
                return this.timeExist;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TimeSinceCreation
        {
            get
            {
                return this.timeSinceCreation;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TimeSinceUpkeep
        {
            get
            {
                return this.timeSinceUpkeep;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TitleLevel
        {
            get
            {
                return this.titleLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TotalDamage
        {
            get
            {
                return this.totalDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TotalMass
        {
            get
            {
                return this.totalMass;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TowerInstance
        {
            get
            {
                return this.towerInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TowerNpcHash
        {
            get
            {
                return this.towerNpcHash;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TowerType
        {
            get
            {
                return this.towerType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TracerEffectType
        {
            get
            {
                return this.tracerEffectType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackChemicalDamage
        {
            get
            {
                return this.trackChemicalDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackColdDamage
        {
            get
            {
                return this.trackColdDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackEnergyDamage
        {
            get
            {
                return this.trackEnergyDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackFireDamage
        {
            get
            {
                return this.trackFireDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackMeleeDamage
        {
            get
            {
                return this.trackMeleeDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackPoisonDamage
        {
            get
            {
                return this.trackPoisonDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackProjectileDamage
        {
            get
            {
                return this.trackProjectileDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrackRadiationDamage
        {
            get
            {
                return this.trackRadiationDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TradeLimit
        {
            get
            {
                return this.tradeLimit;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrainSkill
        {
            get
            {
                return this.trainSkill;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrainSkillCost
        {
            get
            {
                return this.trainSkillCost;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TrapDifficulty
        {
            get
            {
                return this.trapDifficulty;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TravelSound
        {
            get
            {
                return this.travelSound;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Treatment
        {
            get
            {
                return this.treatment;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TurnSpeed
        {
            get
            {
                return this.turnSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat Tutoring
        {
            get
            {
                return this.tutoring;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TwohBluntWeapons
        {
            get
            {
                return this.twohBluntWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat TwohEdgedWeapons
        {
            get
            {
                return this.twohEdgedWeapons;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat UnarmedTemplateInstance
        {
            get
            {
                return this.unarmedTemplateInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat UnreadMailCount
        {
            get
            {
                return this.unreadMailCount;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat UnsavedXP
        {
            get
            {
                return this.unsavedXP;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat UserInstance
        {
            get
            {
                return this.userInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat UserType
        {
            get
            {
                return this.userType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VehicleAC
        {
            get
            {
                return this.vehicleAC;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VehicleDamage
        {
            get
            {
                return this.vehicleDamage;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VehicleHealth
        {
            get
            {
                return this.vehicleHealth;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VehicleSpeed
        {
            get
            {
                return this.vehicleSpeed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VeteranPoints
        {
            get
            {
                return this.veteranPoints;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VicinityRange
        {
            get
            {
                return this.vicinityRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VictoryPoints
        {
            get
            {
                return this.victoryPoints;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VisualBreed
        {
            get
            {
                return this.visualBreed;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VisualFlags
        {
            get
            {
                return this.visualFlags;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VisualLodLevel
        {
            get
            {
                return this.visualLodLevel;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VisualProfession
        {
            get
            {
                return this.visualProfession;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VisualSex
        {
            get
            {
                return this.visualSex;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VolumeMass
        {
            get
            {
                return this.volumeMass;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat VoteCount
        {
            get
            {
                return this.voteCount;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WaitState
        {
            get
            {
                return this.waitState;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponDisallowedInstance
        {
            get
            {
                return this.weaponDisallowedInstance;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponDisallowedType
        {
            get
            {
                return this.weaponDisallowedType;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponMeshHolder
        {
            get
            {
                return this.weaponMeshHolder;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponMeshLeft
        {
            get
            {
                return this.weaponMeshLeft;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponMeshRight
        {
            get
            {
                return this.weaponMeshRight;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponSmithing
        {
            get
            {
                return this.weaponSmithing;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponStyleLeft
        {
            get
            {
                return this.weaponStyleLeft;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponStyleRight
        {
            get
            {
                return this.weaponStyleRight;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat WeaponsStyle
        {
            get
            {
                return this.weaponsStyle;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat XP
        {
            get
            {
                return this.xp;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat XPBonus
        {
            get
            {
                return this.xpBonus;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat XPKillRange
        {
            get
            {
                return this.xpKillRange;
            }
        }

        /// <summary>
        /// </summary>
        public DynelStat XPModifier
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
            foreach (DynelStat cs in this.all)
            {
                cs.Changed = false;
            }
        }

        /// <summary>
        /// </summary>
        public void ClearModifiers()
        {
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
        public DynelStat GetStatbyNumber(int number)
        {
            foreach (DynelStat c in this.all)
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
        public void ReadStatsfromSql()
        {
            foreach (DBStats dbStats in
                StatDao.GetById((int)this.flags.Parent.Identity.Type, this.flags.Parent.Identity.Instance))
            {
                this.SetBaseValue(dbStats.statid, (uint)dbStats.statvalue);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public void Send(object sender, StatChangedEventArgs e)
        {
            if (!((DynelStat)sender).Parent.DoNotDoTimers)
            {
                // TODO: Sending the value back to the client/whole playfield
                // Stat.Send(e.Stat.Parent, e.Stat.StatId, e.NewValue, e.Stat.AnnounceToPlayfield);

                e.Stat.Changed = false;
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            Contract.Requires(statName != null);
            int statid = StatNamesDefaults.GetStatNumber(statName.ToLower());
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
            foreach (DynelStat c in this.all)
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
        public bool Read()
        {
            try
            {
                this.ReadStatsfromSql();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Get LogUtil back in
                // LogUtil.Debug(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Write()
        {
            int inst = this.Flags.Parent.Identity.Instance;
            int typ = (int)this.Flags.Parent.Identity.Type;
            List<DBStats> temp = new List<DBStats>();
            foreach (IStat stat in this.all)
            {
                // Flags are special cases, save always
                if ((stat.StatId == 0)
                    || ((stat.BaseValue != StatNamesDefaults.GetDefault(stat.StatId))
                        && (((DynelStat)stat).DoNotDontWriteToSql == false)))
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
    }

    #endregion
}