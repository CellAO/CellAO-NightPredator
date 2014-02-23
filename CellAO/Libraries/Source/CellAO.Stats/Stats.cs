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

namespace CellAO.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using CellAO.Core.Exceptions;
    using CellAO.Database.Dao;
    using CellAO.Enums;
    using CellAO.Stats.SpecialStats;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class Stats : IStatList
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Stat absorbChemicalAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbColdAC;

        /// <summary>m
        /// </summary>
        private readonly Stat absorbEnergyAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbFireAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbMeleeAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbNanoAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbPoisonAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbProjectileAC;

        /// <summary>
        /// </summary>
        private readonly Stat absorbRadiationAC;

        /// <summary>
        /// </summary>
        private readonly Stat accessCount;

        /// <summary>
        /// </summary>
        private readonly Stat accessGrant;

        /// <summary>
        /// </summary>
        private readonly Stat accessKey;

        /// <summary>
        /// </summary>
        private readonly Stat accountFlags;

        /// <summary>
        /// </summary>
        private readonly Stat accumulatedDamage;

        /// <summary>
        /// </summary>
        private readonly Stat acgEntranceStyles;

        /// <summary>
        /// </summary>
        private readonly Stat acgItemCategoryId;

        /// <summary>
        /// </summary>
        private readonly Stat acgItemLevel;

        /// <summary>
        /// </summary>
        private readonly Stat acgItemSeed;

        /// <summary>
        /// </summary>
        private readonly Stat acgItemTemplateId;

        /// <summary>
        /// </summary>
        private readonly Stat acgItemTemplateId2;

        /// <summary>
        /// </summary>
        private readonly Stat actionCategory;

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash1;

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash2;

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash3;

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash4;

        /// <summary>
        /// </summary>
        private readonly Stat advantageHash5;

        /// <summary>
        /// </summary>
        private readonly StatSkill adventuring;

        /// <summary>
        /// </summary>
        private readonly Stat age;

        /// <summary>
        /// </summary>
        private readonly Stat aggDef;

        /// <summary>
        /// </summary>
        private readonly Stat aggressiveness;

        /// <summary>
        /// </summary>
        private readonly Stat agility;

        /// <summary>
        /// </summary>
        private readonly StatSkill aimedShot;

        /// <summary>
        /// </summary>
        private readonly Stat alienLevel;

        /// <summary>
        /// </summary>
        private readonly StatAlienNextXP alienNextXP;

        /// <summary>
        /// </summary>
        private readonly Stat alienXP;

        /// <summary>
        /// </summary>
        private readonly Stat alignment;

        /// <summary>
        /// </summary>
        private readonly List<IStat> all = new List<IStat>();

        /// <summary>
        /// </summary>
        private readonly Stat ammoName;

        /// <summary>
        /// </summary>
        private readonly Stat ammoType;

        /// <summary>
        /// </summary>
        private readonly Stat ams;

        /// <summary>
        /// </summary>
        private readonly Stat amsCap;

        /// <summary>
        /// </summary>
        private readonly Stat amsModifier;

        /// <summary>
        /// </summary>
        private readonly Stat anim;

        /// <summary>
        /// </summary>
        private readonly Stat animPlay;

        /// <summary>
        /// </summary>
        private readonly Stat animPos;

        /// <summary>
        /// </summary>
        private readonly Stat animSet;

        /// <summary>
        /// </summary>
        private readonly Stat animSpeed;

        /// <summary>
        /// </summary>
        private readonly Stat apartmentAccessCard;

        /// <summary>
        /// </summary>
        private readonly Stat apartmentsAllowed;

        /// <summary>
        /// </summary>
        private readonly Stat apartmentsOwned;

        /// <summary>
        /// </summary>
        private readonly Stat areaInstance;

        /// <summary>
        /// </summary>
        private readonly Stat areaType;

        /// <summary>
        /// </summary>
        private readonly Stat armourType;

        /// <summary>
        /// </summary>
        private readonly StatSkill assaultRifle;

        /// <summary>
        /// </summary>
        private readonly Stat attackCount;

        /// <summary>
        /// </summary>
        private readonly Stat attackRange;

        /// <summary>
        /// </summary>
        private readonly Stat attackShield;

        /// <summary>
        /// </summary>
        private readonly Stat attackSpeed;

        /// <summary>
        /// </summary>
        private readonly Stat attackType;

        /// <summary>
        /// </summary>
        private readonly Stat attitude;

        /// <summary>
        /// </summary>
        private readonly Stat autoAttackFlags;

        /// <summary>
        /// </summary>
        private readonly Stat autoLockTimeDefault;

        /// <summary>
        /// </summary>
        private readonly Stat autoUnlockTimeDefault;

        /// <summary>
        /// </summary>
        private readonly Stat backMesh;

        /// <summary>
        /// </summary>
        private readonly Stat backstab;

        /// <summary>
        /// </summary>
        private readonly Stat bandolierSlots;

        /// <summary>
        /// </summary>
        private readonly Stat battlestationRep;

        /// <summary>
        /// </summary>
        private readonly Stat battlestationSide;

        /// <summary>
        /// </summary>
        private readonly Stat beltSlots;

        /// <summary>
        /// </summary>
        private readonly Stat berserkMode;

        /// <summary>
        /// </summary>
        private readonly StatSkill biologicalMetamorphose;

        /// <summary>
        /// </summary>
        private readonly Stat birthDate;

        /// <summary>
        /// </summary>
        private readonly StatSkill bodyDevelopment;

        /// <summary>
        /// </summary>
        private readonly StatSkill bow;

        /// <summary>
        /// </summary>
        private readonly StatSkill bowSpecialAttack;

        /// <summary>
        /// </summary>
        private readonly Stat brainType;

        /// <summary>
        /// </summary>
        private readonly StatSkill brawl;

        /// <summary>
        /// </summary>
        private readonly StatSkill breakingEntry;

        /// <summary>
        /// </summary>
        private readonly Stat breed;

        /// <summary>
        /// </summary>
        private readonly Stat breedHostility;

        /// <summary>
        /// </summary>
        private readonly Stat breedLimit;

        /// <summary>
        /// </summary>
        private readonly Stat buildingComplexInst;

        /// <summary>
        /// </summary>
        private readonly Stat buildingInstance;

        /// <summary>
        /// </summary>
        private readonly Stat buildingType;

        /// <summary>
        /// </summary>
        private readonly StatSkill burst;

        /// <summary>
        /// </summary>
        private readonly Stat burstRecharge;

        /// <summary>
        /// </summary>
        private readonly Stat buyModifier;

        /// <summary>
        /// </summary>
        private readonly Stat can;

        /// <summary>
        /// </summary>
        private readonly Stat canChangeClothes;

        /// <summary>
        /// </summary>
        private readonly Stat cardOwnerInstance;

        /// <summary>
        /// </summary>
        private readonly Stat cardOwnerType;

        /// <summary>
        /// </summary>
        private readonly Stat cash;

        /// <summary>
        /// </summary>
        private readonly Stat castEffectType;

        /// <summary>
        /// </summary>
        private readonly Stat castSelfAbstractAnim;

        /// <summary>
        /// </summary>
        private readonly Stat castSound;

        /// <summary>
        /// </summary>
        private readonly Stat castTargetAbstractAnim;

        /// <summary>
        /// </summary>
        private readonly Stat catAnim;

        /// <summary>
        /// </summary>
        private readonly Stat catAnimFlags;

        /// <summary>
        /// </summary>
        private readonly Stat catMesh;

        /// <summary>
        /// </summary>
        private readonly Stat chanceOfBreakOnDebuff;

        /// <summary>
        /// </summary>
        private readonly Stat chanceOfBreakOnSpellAttack;

        /// <summary>
        /// </summary>
        private readonly Stat chanceOfUse;

        /// <summary>
        /// </summary>
        private readonly Stat changeSideCount;

        /// <summary>
        /// </summary>
        private readonly Stat charRadius;

        /// <summary>
        /// </summary>
        private readonly Stat charState;

        /// <summary>
        /// </summary>
        private readonly Stat charTmp1;

        /// <summary>
        /// </summary>
        private readonly Stat charTmp2;

        /// <summary>
        /// </summary>
        private readonly Stat charTmp3;

        /// <summary>
        /// </summary>
        private readonly Stat charTmp4;

        /// <summary>
        /// </summary>
        private readonly Stat chemicalAC;

        /// <summary>
        /// </summary>
        private readonly Stat chemicalDamageModifier;

        /// <summary>
        /// </summary>
        private readonly StatSkill chemistry;

        /// <summary>
        /// </summary>
        private readonly Stat chestFlags;

        /// <summary>
        /// </summary>
        private readonly Stat cityInstance;

        /// <summary>
        /// </summary>
        private readonly Stat cityTerminalRechargePercent;

        /// <summary>
        /// </summary>
        private readonly Stat clan;

        /// <summary>
        /// </summary>
        private readonly Stat clanConserver;

        /// <summary>
        /// </summary>
        private readonly Stat clanDevoted;

        /// <summary>
        /// </summary>
        private readonly Stat clanFinalized;

        /// <summary>
        /// </summary>
        private readonly Stat clanGaia;

        /// <summary>
        /// </summary>
        private readonly Stat clanHierarchy;

        /// <summary>
        /// </summary>
        private readonly Stat clanInstance;

        /// <summary>
        /// </summary>
        private readonly Stat clanItemInstance;

        /// <summary>
        /// </summary>
        private readonly Stat clanItemType;

        /// <summary>
        /// </summary>
        private readonly Stat clanLevel;

        /// <summary>
        /// </summary>
        private readonly Stat clanPrice;

        /// <summary>
        /// </summary>
        private readonly Stat clanRedeemed;

        /// <summary>
        /// </summary>
        private readonly Stat clanSentinels;

        /// <summary>
        /// </summary>
        private readonly Stat clanType;

        /// <summary>
        /// </summary>
        private readonly Stat clanUpkeepInterval;

        /// <summary>
        /// </summary>
        private readonly Stat clanVanguards;

        /// <summary>
        /// </summary>
        private readonly Stat clientActivated;

        /// <summary>
        /// </summary>
        private readonly StatSkill closeCombatInitiative;

        /// <summary>
        /// </summary>
        private readonly Stat coldAC;

        /// <summary>
        /// </summary>
        private readonly Stat coldDamageModifier;

        /// <summary>
        /// </summary>
        private readonly Stat collideCheckInterval;

        /// <summary>
        /// </summary>
        private readonly Stat collisionRadius;

        /// <summary>
        /// </summary>
        private readonly Stat commandRange;

        /// <summary>
        /// </summary>
        private readonly Stat compulsion;

        /// <summary>
        /// </summary>
        private readonly StatSkill computerLiteracy;

        /// <summary>
        /// </summary>
        private readonly StatSkill concealment;

        /// <summary>
        /// </summary>
        private readonly Stat conditionState;

        /// <summary>
        /// </summary>
        private readonly Stat conformity;

        /// <summary>
        /// </summary>
        private readonly Stat corpseAnimKey;

        /// <summary>
        /// </summary>
        private readonly Stat corpseHash;

        /// <summary>
        /// </summary>
        private readonly Stat corpseInstance;

        /// <summary>
        /// </summary>
        private readonly Stat corpseType;

        /// <summary>
        /// </summary>
        private readonly Stat criticalDecrease;

        /// <summary>
        /// </summary>
        private readonly Stat criticalIncrease;

        /// <summary>
        /// </summary>
        private readonly Stat currBodyLocation;

        /// <summary>
        /// </summary>
        private readonly Stat currentMass;

        /// <summary>
        /// </summary>
        private readonly Stat currentMovementMode;

        /// <summary>
        /// </summary>
        private readonly Stat currentNCU;

        /// <summary>
        /// </summary>
        private readonly StatCurrentNano currentNano;

        /// <summary>
        /// </summary>
        private readonly Stat currentPlayfield;

        /// <summary>
        /// </summary>
        private readonly Stat currentState;

        /// <summary>
        /// </summary>
        private readonly Stat currentTime;

        /// <summary>
        /// </summary>
        private readonly Stat damageBonus;

        /// <summary>
        /// </summary>
        private readonly Stat damageOverrideType;

        /// <summary>
        /// </summary>
        private readonly Stat damageToNano;

        /// <summary>
        /// </summary>
        private readonly Stat damageToNanoMultiplier;

        /// <summary>
        /// </summary>
        private readonly Stat damageType;

        /// <summary>
        /// </summary>
        private readonly Stat deadTimer;

        /// <summary>
        /// </summary>
        private readonly Stat deathReason;

        /// <summary>
        /// </summary>
        private readonly Stat debuffFormula;

        /// <summary>
        /// </summary>
        private readonly Stat defaultAttackType;

        /// <summary>
        /// </summary>
        private readonly Stat defaultPos;

        /// <summary>
        /// </summary>
        private readonly Stat desiredTargetDistance;

        /// <summary>
        /// </summary>
        private readonly Stat dieAnim;

        /// <summary>
        /// </summary>
        private readonly StatSkill dimach;

        /// <summary>
        /// </summary>
        private readonly StatSkill disarmTrap;

        /// <summary>
        /// </summary>
        private readonly Stat displayCATAnim;

        /// <summary>
        /// </summary>
        private readonly Stat displayCATMesh;

        /// <summary>
        /// </summary>
        private readonly Stat distanceToSpawnpoint;

        /// <summary>
        /// </summary>
        private readonly StatSkill distanceWeaponInitiative;

        /// <summary>
        /// </summary>
        private readonly Stat districtNano;

        /// <summary>
        /// </summary>
        private readonly Stat districtNanoInterval;

        /// <summary>
        /// </summary>
        private readonly Stat dms;

        /// <summary>
        /// </summary>
        private readonly Stat dmsModifier;

        /// <summary>
        /// </summary>
        private readonly StatSkill dodge;

        /// <summary>
        /// </summary>
        private readonly Stat doorBlockTime;

        /// <summary>
        /// </summary>
        private readonly Stat doorFlags;

        /// <summary>
        /// </summary>
        private readonly StatSkill driveAir;

        /// <summary>
        /// </summary>
        private readonly StatSkill driveGround;

        /// <summary>
        /// </summary>
        private readonly StatSkill driveWater;

        /// <summary>
        /// </summary>
        private readonly StatSkill duck;

        /// <summary>
        /// </summary>
        private readonly Stat dudChance;

        /// <summary>
        /// </summary>
        private readonly Stat durationModifier;

        /// <summary>
        /// </summary>
        private readonly Stat effectBlue;

        /// <summary>
        /// </summary>
        private readonly Stat effectGreen;

        /// <summary>
        /// </summary>
        private readonly Stat effectIcon;

        /// <summary>
        /// </summary>
        private readonly Stat effectRed;

        /// <summary>
        /// </summary>
        private readonly Stat effectType;

        /// <summary>
        /// </summary>
        private readonly StatSkill electricalEngineering;

        /// <summary>
        /// </summary>
        private readonly Stat energy;

        /// <summary>
        /// </summary>
        private readonly Stat energyAC;

        /// <summary>
        /// </summary>
        private readonly Stat energyDamageModifier;

        /// <summary>
        /// </summary>
        private readonly Stat equipDelay;

        /// <summary>
        /// </summary>
        private readonly Stat equippedWeapons;

        /// <summary>
        /// </summary>
        private readonly StatSkill evade;

        /// <summary>
        /// </summary>
        private readonly Stat exitInstance;

        /// <summary>
        /// </summary>
        private readonly Stat expansion;

        /// <summary>
        /// </summary>
        private readonly Stat expansionPlayfield;

        /// <summary>
        /// </summary>
        private readonly Stat extenalDoorInstance;

        /// <summary>
        /// </summary>
        private readonly Stat extenalPlayfieldInstance;

        /// <summary>
        /// </summary>
        private readonly Stat extendedFlags;

        /// <summary>
        /// </summary>
        private readonly Stat extendedTime;

        /// <summary>
        /// </summary>
        private readonly Stat extroverty;

        /// <summary>
        /// </summary>
        private readonly Stat fabricType;

        /// <summary>
        /// </summary>
        private readonly Stat face;

        /// <summary>
        /// </summary>
        private readonly Stat faceTexture;

        /// <summary>
        /// </summary>
        private readonly Stat factionModifier;

        /// <summary>
        /// </summary>
        private readonly Stat fallDamage;

        /// <summary>
        /// </summary>
        private readonly StatSkill fastAttack;

        /// <summary>
        /// </summary>
        private readonly Stat fatness;

        /// <summary>
        /// </summary>
        private readonly Stat features;

        /// <summary>
        /// </summary>
        private readonly StatSkill fieldQuantumPhysics;

        /// <summary>
        /// </summary>
        private readonly Stat fireAC;

        /// <summary>
        /// </summary>
        private readonly Stat fireDamageModifier;

        /// <summary>
        /// </summary>
        private readonly StatSkill firstAid;

        /// <summary>
        /// </summary>
        private readonly Stat fixtureFlags;

        /// <summary>
        /// </summary>
        private readonly Stat flags;

        /// <summary>
        /// </summary>
        private readonly StatSkill flingShot;

        /// <summary>
        /// </summary>
        private readonly StatSkill fullAuto;

        /// <summary>
        /// </summary>
        private readonly Stat fullAutoRecharge;

        /// <summary>
        /// </summary>
        private readonly Stat gatherAbstractAnim;

        /// <summary>
        /// </summary>
        private readonly Stat gatherEffectType;

        /// <summary>
        /// </summary>
        private readonly Stat gatherSound;

        /// <summary>
        /// </summary>
        private readonly Stat genderLimit;

        /// <summary>
        /// </summary>
        private readonly Stat globalClanInstance;

        /// <summary>
        /// </summary>
        private readonly Stat globalClanType;

        /// <summary>
        /// </summary>
        private readonly Stat globalResearchGoal;

        /// <summary>
        /// </summary>
        private readonly Stat globalResearchLevel;

        /// <summary>
        /// </summary>
        private readonly StatGmLevel gmLevel;

        /// <summary>
        /// </summary>
        private readonly Stat gos;

        /// <summary>
        /// </summary>
        private readonly StatSkill grenade;

        /// <summary>
        /// </summary>
        private readonly Stat hairMesh;

        /// <summary>
        /// </summary>
        private readonly Stat hasAlwaysLootable;

        /// <summary>
        /// </summary>
        private readonly Stat hasKnuBotData;

        /// <summary>
        /// </summary>
        private readonly Stat hateValueModifyer;

        /// <summary>
        /// </summary>
        private readonly OverridingModifierStat headMesh;

        /// <summary>
        /// </summary>
        private readonly StatHealDelta healDelta;

        /// <summary>
        /// </summary>
        private readonly StatHealInterval healInterval;

        /// <summary>
        /// </summary>
        private readonly Stat healMultiplier;

        /// <summary>
        /// </summary>
        private readonly StatHitPoints health;

        /// <summary>
        /// </summary>
        private readonly Stat healthChange;

        /// <summary>
        /// </summary>
        private readonly Stat healthChangeBest;

        /// <summary>
        /// </summary>
        private readonly Stat healthChangeWorst;

        /// <summary>
        /// </summary>
        private readonly Stat height;

        /// <summary>
        /// </summary>
        private readonly Stat hitEffectType;

        /// <summary>
        /// </summary>
        private readonly Stat hitSound;

        /// <summary>
        /// </summary>
        private readonly Stat houseTemplate;

        /// <summary>
        /// </summary>
        private readonly Stat hpLevelUp;

        /// <summary>
        /// </summary>
        private readonly Stat hpPerSkill;

        /// <summary>
        /// </summary>
        private readonly Stat icon;

        /// <summary>
        /// </summary>
        private readonly Stat impactEffectType;

        /// <summary>
        /// </summary>
        private readonly Stat inPlay;

        /// <summary>
        /// </summary>
        private readonly Stat info;

        /// <summary>
        /// </summary>
        private readonly Stat initiativeType;

        /// <summary>
        /// </summary>
        private readonly Stat instance;

        /// <summary>
        /// </summary>
        private readonly Stat insurancePercentage;

        /// <summary>
        /// </summary>
        private readonly Stat insuranceTime;

        /// <summary>
        /// </summary>
        private readonly Stat intelligence;

        /// <summary>
        /// </summary>
        private readonly Stat interactionRadius;

        /// <summary>
        /// </summary>
        private readonly Stat interruptModifier;

        /// <summary>
        /// </summary>
        private readonly Stat invadersKilled;

        /// <summary>
        /// </summary>
        private readonly Stat inventoryId;

        /// <summary>
        /// </summary>
        private readonly Stat inventoryTimeout;

        /// <summary>
        /// </summary>
        private readonly StatIp ip;

        /// <summary>
        /// </summary>
        private readonly Stat isFightingMe;

        /// <summary>
        /// </summary>
        private readonly Stat isVehicle;

        /// <summary>
        /// </summary>
        private readonly Stat itemAnim;

        /// <summary>
        /// </summary>
        private readonly Stat itemClass;

        /// <summary>
        /// </summary>
        private readonly Stat itemDelay;

        /// <summary>
        /// </summary>
        private readonly Stat itemDelayCap;

        /// <summary>
        /// </summary>
        private readonly Stat itemHateValue;

        /// <summary>
        /// </summary>
        private readonly Stat itemOpposedSkill;

        /// <summary>
        /// </summary>
        private readonly Stat itemSIS;

        /// <summary>
        /// </summary>
        private readonly Stat itemSkill;

        /// <summary>
        /// </summary>
        private readonly Stat itemType;

        /// <summary>
        /// </summary>
        private readonly Stat killedByInvaders;

        /// <summary>
        /// </summary>
        private readonly Stat lastConcretePlayfieldInstance;

        /// <summary>
        /// </summary>
        private readonly Stat lastMailCheckTime;

        /// <summary>
        /// </summary>
        private readonly Stat lastPerkResetTime;

        /// <summary>
        /// </summary>
        private readonly Stat lastRnd;

        /// <summary>
        /// </summary>
        private readonly Stat lastSK;

        /// <summary>
        /// </summary>
        private readonly Stat lastSaveXP;

        /// <summary>
        /// </summary>
        private readonly Stat lastSaved;

        /// <summary>
        /// </summary>
        private readonly Stat lastXP;

        /// <summary>
        /// </summary>
        private readonly Stat leaderLockDownTime;

        /// <summary>
        /// </summary>
        private readonly Stat level;

        /// <summary>
        /// </summary>
        private readonly Stat levelLimit;

        /// <summary>
        /// </summary>
        private readonly StatLife life;

        /// <summary>
        /// </summary>
        private readonly Stat liquidType;

        /// <summary>
        /// </summary>
        private readonly Stat lockDifficulty;

        /// <summary>
        /// </summary>
        private readonly Stat lockDownTime;

        /// <summary>
        /// </summary>
        private readonly Stat losHeight;

        /// <summary>
        /// </summary>
        private readonly Stat lowresMesh;

        /// <summary>
        /// </summary>
        private readonly StatSkill lrEnergyWeapon;

        /// <summary>
        /// </summary>
        private readonly StatSkill lrMultipleWeapon;

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart1;

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart2;

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart3;

        /// <summary>
        /// </summary>
        private readonly Stat mapAreaPart4;

        /// <summary>
        /// </summary>
        private readonly Stat mapFlags;

        /// <summary>
        /// </summary>
        private readonly StatSkill mapNavigation;

        /// <summary>
        /// </summary>
        private readonly Stat mapOptions;

        /// <summary>
        /// </summary>
        private readonly StatSkill martialArts;

        /// <summary>
        /// </summary>
        private readonly StatSkill materialCreation;

        /// <summary>
        /// </summary>
        private readonly StatSkill materialLocation;

        /// <summary>
        /// </summary>
        private readonly StatSkill materialMetamorphose;

        /// <summary>
        /// </summary>
        private readonly Stat maxDamage;

        /// <summary>
        /// </summary>
        private readonly Stat maxEnergy;

        /// <summary>
        /// </summary>
        private readonly Stat maxMass;

        /// <summary>
        /// </summary>
        private readonly Stat maxNCU;

        /// <summary>
        /// </summary>
        private readonly StatMaxNanoEnergy maxNanoEnergy;

        /// <summary>
        /// </summary>
        private readonly Stat maxShopItems;

        /// <summary>
        /// </summary>
        private readonly Stat mechData;

        /// <summary>
        /// </summary>
        private readonly StatSkill mechanicalEngineering;

        /// <summary>
        /// </summary>
        private readonly Stat meleeAC;

        /// <summary>
        /// </summary>
        private readonly Stat meleeDamageModifier;

        /// <summary>
        /// </summary>
        private readonly StatSkill meleeEnergyWeapon;

        /// <summary>
        /// </summary>
        private readonly StatSkill meleeMultiple;

        /// <summary>
        /// </summary>
        private readonly Stat memberInstance;

        /// <summary>
        /// </summary>
        private readonly Stat memberType;

        /// <summary>
        /// </summary>
        private readonly Stat members;

        /// <summary>
        /// </summary>
        private readonly OverridingModifierStat mesh;

        /// <summary>
        /// </summary>
        private readonly Stat metaType;

        /// <summary>
        /// </summary>
        private readonly Stat metersWalked;

        /// <summary>
        /// </summary>
        private readonly Stat minDamage;

        /// <summary>
        /// </summary>
        private readonly Stat minMembers;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits1;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits10;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits11;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits12;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits2;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits3;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits4;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits5;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits6;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits7;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits8;

        /// <summary>
        /// </summary>
        private readonly Stat missionBits9;

        /// <summary>
        /// </summary>
        private readonly Stat monsterData;

        /// <summary>
        /// </summary>
        private readonly Stat monsterLevelsKilled;

        /// <summary>
        /// </summary>
        private readonly Stat monsterScale;

        /// <summary>
        /// </summary>
        private readonly Stat monsterTexture;

        /// <summary>
        /// </summary>
        private readonly Stat monthsPaid;

        /// <summary>
        /// </summary>
        private readonly Stat moreFlags;

        /// <summary>
        /// </summary>
        private readonly Stat multipleCount;

        /// <summary>
        /// </summary>
        private readonly Stat name;

        /// <summary>
        /// </summary>
        private readonly Stat nameTemplate;

        /// <summary>
        /// </summary>
        private readonly StatSkill nanoAC;

        /// <summary>
        /// </summary>
        private readonly Stat nanoDamageModifier;

        /// <summary>
        /// </summary>
        private readonly Stat nanoDamageMultiplier;

        /// <summary>
        /// </summary>
        private readonly StatNanoDelta nanoDelta;

        /// <summary>
        /// </summary>
        private readonly StatSkill nanoEnergyPool;

        /// <summary>
        /// </summary>
        private readonly Stat nanoFocusLevel;

        /// <summary>
        /// </summary>
        private readonly StatNanoInterval nanoInterval;

        /// <summary>
        /// </summary>
        private readonly Stat nanoPoints;

        /// <summary>
        /// </summary>
        private readonly StatSkill nanoProgramming;

        /// <summary>
        /// </summary>
        private readonly StatSkill nanoProwessInitiative;

        /// <summary>
        /// </summary>
        private readonly Stat nanoSpeed;

        /// <summary>
        /// </summary>
        private readonly Stat nanoVulnerability;

        /// <summary>
        /// </summary>
        private readonly Stat newbieHP;

        /// <summary>
        /// </summary>
        private readonly Stat newbieNP;

        /// <summary>
        /// </summary>
        private readonly Stat nextDoorInBuilding;

        /// <summary>
        /// </summary>
        private readonly Stat nextFormula;

        /// <summary>
        /// </summary>
        private readonly StatNextSK nextSK;

        /// <summary>
        /// </summary>
        private readonly StatNextXP nextXP;

        /// <summary>
        /// </summary>
        private readonly Stat npCostModifier;

        /// <summary>
        /// </summary>
        private readonly Stat npLevelUp;

        /// <summary>
        /// </summary>
        private readonly Stat npPerSkill;

        /// <summary>
        /// </summary>
        private readonly Stat npcBrainState;

        /// <summary>
        /// </summary>
        private readonly Stat npcCommand;

        /// <summary>
        /// </summary>
        private readonly Stat npcCommandArg;

        /// <summary>
        /// </summary>
        private readonly Stat npcCryForHelpRange;

        /// <summary>
        /// </summary>
        private readonly Stat npcFamily;

        /// <summary>
        /// </summary>
        private readonly Stat npcFlags;

        /// <summary>
        /// </summary>
        private readonly Stat npcFovStatus;

        /// <summary>
        /// </summary>
        private readonly Stat npcHasPatrolList;

        /// <summary>
        /// </summary>
        private readonly Stat npcHash;

        /// <summary>
        /// </summary>
        private readonly Stat npcHatelistSize;

        /// <summary>
        /// </summary>
        private readonly Stat npcIsSurrendering;

        /// <summary>
        /// </summary>
        private readonly Stat npcNumPets;

        /// <summary>
        /// </summary>
        private readonly Stat npcScriptAmsScale;

        /// <summary>
        /// </summary>
        private readonly Stat npcSpellArg1;

        /// <summary>
        /// </summary>
        private readonly Stat npcSpellRet1;

        /// <summary>
        /// </summary>
        private readonly Stat npcSurrenderInstance;

        /// <summary>
        /// </summary>
        private readonly Stat npcUseFightModeRegenRate;

        /// <summary>
        /// </summary>
        private readonly Stat npcVicinityChars;

        /// <summary>
        /// </summary>
        private readonly Stat npcVicinityFamily;

        /// <summary>
        /// </summary>
        private readonly Stat npcVicinityPlayers;

        /// <summary>
        /// </summary>
        private readonly Stat numAttackEffects;

        /// <summary>
        /// </summary>
        private readonly Stat numberOfItems;

        /// <summary>
        /// </summary>
        private readonly Stat numberOfTeamMembers;

        /// <summary>
        /// </summary>
        private readonly Stat numberOnHateList;

        /// <summary>
        /// </summary>
        private readonly Stat objectType;

        /// <summary>
        /// </summary>
        private readonly Stat odMaxSizeAdd;

        /// <summary>
        /// </summary>
        private readonly Stat odMinSizeAdd;

        /// <summary>
        /// </summary>
        private readonly Stat oldTimeExist;

        /// <summary>
        /// </summary>
        private readonly Stat onTowerCreation;

        /// <summary>
        /// </summary>
        private readonly StatSkill onehBluntWeapons;

        /// <summary>
        /// </summary>
        private readonly StatSkill onehEdgedWeapon;

        /// <summary>
        /// </summary>
        private readonly Stat orientationMode;

        /// <summary>
        /// </summary>
        private readonly Stat originatorType;

        /// <summary>
        /// </summary>
        private readonly Stat otArmedForces;

        /// <summary>
        /// </summary>
        private readonly Stat otFollowers;

        /// <summary>
        /// </summary>
        private readonly Stat otMed;

        /// <summary>
        /// </summary>
        private readonly Stat otOperator;

        /// <summary>
        /// </summary>
        private readonly Stat otTrans;

        /// <summary>
        /// </summary>
        private readonly Stat otUnredeemed;

        /// <summary>
        /// </summary>
        private readonly Stat outerRadius;

        /// <summary>
        /// </summary>
        private readonly Stat overrideMaterial;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTexture;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureAttractor;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureBack;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureHead;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureShoulderpadLeft;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureShoulderpadRight;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureWeaponLeft;

        /// <summary>
        /// </summary>
        private readonly Stat overrideTextureWeaponRight;

        /// <summary>
        /// </summary>
        private readonly Stat ownedTowers;

        /// <summary>
        /// </summary>
        private readonly Identity owner = new Identity();

        /// <summary>
        /// </summary>
        private readonly Stat ownerInstance;

        /// <summary>
        /// </summary>
        private readonly Stat paidPoints;

        /// <summary>
        /// </summary>
        private readonly Stat parentInstance;

        /// <summary>
        /// </summary>
        private readonly Stat parentType;

        /// <summary>
        /// </summary>
        private readonly StatSkill parry;

        /// <summary>
        /// </summary>
        private readonly Stat percentChemicalDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentColdDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentEnergyDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentFireDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentMeleeDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentPoisonDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentProjectileDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentRadiationDamage;

        /// <summary>
        /// </summary>
        private readonly Stat percentRemainingHealth;

        /// <summary>
        /// </summary>
        private readonly Stat percentRemainingNano;

        /// <summary>
        /// </summary>
        private readonly StatSkill perception;

        /// <summary>
        /// </summary>
        private readonly Stat personalResearchGoal;

        /// <summary>
        /// </summary>
        private readonly Stat personalResearchLevel;

        /// <summary>
        /// </summary>
        private readonly Stat petCounter;

        /// <summary>
        /// </summary>
        private readonly Stat petMaster;

        /// <summary>
        /// </summary>
        private readonly Stat petReq1;

        /// <summary>
        /// </summary>
        private readonly Stat petReq2;

        /// <summary>
        /// </summary>
        private readonly Stat petReq3;

        /// <summary>
        /// </summary>
        private readonly Stat petReqVal1;

        /// <summary>
        /// </summary>
        private readonly Stat petReqVal2;

        /// <summary>
        /// </summary>
        private readonly Stat petReqVal3;

        /// <summary>
        /// </summary>
        private readonly Stat petState;

        /// <summary>
        /// </summary>
        private readonly Stat petType;

        /// <summary>
        /// </summary>
        private readonly StatSkill pharmaceuticals;

        /// <summary>
        /// </summary>
        private readonly StatSkill physicalProwessInitiative;

        /// <summary>
        /// </summary>
        private readonly StatSkill piercing;

        /// <summary>
        /// </summary>
        private readonly StatSkill pistol;

        /// <summary>
        /// </summary>
        private readonly Stat placement;

        /// <summary>
        /// </summary>
        private readonly Stat playerId;

        /// <summary>
        /// </summary>
        private readonly Stat playerKilling;

        /// <summary>
        /// </summary>
        private readonly Stat playerOptions;

        /// <summary>
        /// </summary>
        private readonly Stat playfieldType;

        /// <summary>
        /// </summary>
        private readonly Stat poisonAC;

        /// <summary>
        /// </summary>
        private readonly Stat poisonDamageModifier;

        /// <summary>
        /// </summary>
        private readonly Stat prevMovementMode;

        /// <summary>
        /// </summary>
        private readonly Stat previousHealth;

        /// <summary>
        /// </summary>
        private readonly Stat price;

        /// <summary>
        /// </summary>
        private readonly Stat primaryItemInstance;

        /// <summary>
        /// </summary>
        private readonly Stat primaryItemType;

        /// <summary>
        /// </summary>
        private readonly Stat primaryTemplateId;

        /// <summary>
        /// </summary>
        private readonly Stat procChance1;

        /// <summary>
        /// </summary>
        private readonly Stat procChance2;

        /// <summary>
        /// </summary>
        private readonly Stat procChance3;

        /// <summary>
        /// </summary>
        private readonly Stat procChance4;

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative1;

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative2;

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative3;

        /// <summary>
        /// </summary>
        private readonly Stat procInitiative4;

        /// <summary>
        /// </summary>
        private readonly Stat procNano1;

        /// <summary>
        /// </summary>
        private readonly Stat procNano2;

        /// <summary>
        /// </summary>
        private readonly Stat procNano3;

        /// <summary>
        /// </summary>
        private readonly Stat procNano4;

        /// <summary>
        /// </summary>
        private readonly Stat profession;

        /// <summary>
        /// </summary>
        private readonly Stat professionLevel;

        /// <summary>
        /// </summary>
        private readonly Stat projectileAC;

        /// <summary>
        /// </summary>
        private readonly Stat projectileDamageModifier;

        /// <summary>
        /// </summary>
        private readonly Stat proximityRangeIndoors;

        /// <summary>
        /// </summary>
        private readonly Stat proximityRangeOutdoors;

        /// <summary>
        /// </summary>
        private readonly Stat psychic;

        /// <summary>
        /// </summary>
        private readonly StatSkill psychologicalModification;

        /// <summary>
        /// </summary>
        private readonly StatSkill psychology;

        /// <summary>
        /// </summary>
        private readonly Stat pvPLevelsKilled;

        /// <summary>
        /// </summary>
        private readonly Stat pvpDuelDeaths;

        /// <summary>
        /// </summary>
        private readonly Stat pvpDuelKills;

        /// <summary>
        /// </summary>
        private readonly Stat pvpDuelScore;

        /// <summary>
        /// </summary>
        private readonly Stat pvpProfessionDuelDeaths;

        /// <summary>
        /// </summary>
        private readonly Stat pvpProfessionDuelKills;

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedSoloDeaths;

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedSoloKills;

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedTeamDeaths;

        /// <summary>
        /// </summary>
        private readonly Stat pvpRankedTeamKills;

        /// <summary>
        /// </summary>
        private readonly Stat pvpRating;

        /// <summary>
        /// </summary>
        private readonly Stat pvpSoloScore;

        /// <summary>
        /// </summary>
        private readonly Stat pvpTeamScore;

        /// <summary>
        /// </summary>
        private readonly Stat qtDungeonInstance;

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterCount1;

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterCount2;

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterCount3;

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterID3;

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterId1;

        /// <summary>
        /// </summary>
        private readonly Stat qtKillNumMonsterId2;

        /// <summary>
        /// </summary>
        private readonly Stat qtKilledMonsters;

        /// <summary>
        /// </summary>
        private readonly Stat qtNumMonsters;

        /// <summary>
        /// </summary>
        private readonly Stat questAsMaximumRange;

        /// <summary>
        /// </summary>
        private readonly Stat questAsMinimumRange;

        /// <summary>
        /// </summary>
        private readonly Stat questBoothDifficulty;

        /// <summary>
        /// </summary>
        private readonly Stat questIndex0;

        /// <summary>
        /// </summary>
        private readonly Stat questIndex1;

        /// <summary>
        /// </summary>
        private readonly Stat questIndex2;

        /// <summary>
        /// </summary>
        private readonly Stat questIndex3;

        /// <summary>
        /// </summary>
        private readonly Stat questIndex4;

        /// <summary>
        /// </summary>
        private readonly Stat questIndex5;

        /// <summary>
        /// </summary>
        private readonly Stat questInstance;

        /// <summary>
        /// </summary>
        private readonly Stat questLevelsSolved;

        /// <summary>
        /// </summary>
        private readonly Stat questStat;

        /// <summary>
        /// </summary>
        private readonly Stat questTimeout;

        /// <summary>
        /// </summary>
        private readonly Stat race;

        /// <summary>
        /// </summary>
        private readonly Stat radiationAC;

        /// <summary>
        /// </summary>
        private readonly Stat radiationDamageModifier;

        /// <summary>
        /// </summary>
        private readonly Stat rangeIncreaserNF;

        /// <summary>
        /// </summary>
        private readonly Stat rangeIncreaserWeapon;

        /// <summary>
        /// </summary>
        private readonly Stat readOnly;

        /// <summary>
        /// </summary>
        private readonly Stat rechargeDelay;

        /// <summary>
        /// </summary>
        private readonly Stat rechargeDelayCap;

        /// <summary>
        /// </summary>
        private readonly Stat reclaimItem;

        /// <summary>
        /// </summary>
        private readonly Stat reflectChemicalAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectColdAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectEnergyAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectFireAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectMeleeAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectNanoAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectPoisonAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectProjectileAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectRadiationAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedChemicalAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedColdAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedEnergyAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedFireAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedMeleeAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedNanoAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedPoisonAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedProjectileAC;

        /// <summary>
        /// </summary>
        private readonly Stat reflectReturnedRadiationAC;

        /// <summary>
        /// </summary>
        private readonly Stat regainXPPercentage;

        /// <summary>
        /// </summary>
        private readonly Stat repairDifficulty;

        /// <summary>
        /// </summary>
        private readonly Stat repairSkill;

        /// <summary>
        /// </summary>
        private readonly Stat resistModifier;

        /// <summary>
        /// </summary>
        private readonly Stat restModifier;

        /// <summary>
        /// </summary>
        private readonly Stat resurrectDest;

        /// <summary>
        /// </summary>
        private readonly StatSkill rifle;

        /// <summary>
        /// </summary>
        private readonly StatSkill riposte;

        /// <summary>
        /// </summary>
        private readonly Stat ritualTargetInst;

        /// <summary>
        /// </summary>
        private readonly Stat rnd;

        /// <summary>
        /// </summary>
        private readonly Stat rotation;

        /// <summary>
        /// </summary>
        private readonly Stat rp;

        /// <summary>
        /// </summary>
        private readonly StatSkill runSpeed;

        /// <summary>
        /// </summary>
        private readonly Stat savedXP;

        /// <summary>
        /// </summary>
        private readonly Stat school;

        /// <summary>
        /// </summary>
        private readonly Stat secondaryItemInstance;

        /// <summary>
        /// </summary>
        private readonly Stat secondaryItemTemplate;

        /// <summary>
        /// </summary>
        private readonly Stat secondaryItemType;

        /// <summary>
        /// </summary>
        private readonly Stat selectedTarget;

        /// <summary>
        /// </summary>
        private readonly Stat selectedTargetType;

        /// <summary>
        /// </summary>
        private readonly Stat sellModifier;

        /// <summary>
        /// </summary>
        private readonly Stat sense;

        /// <summary>
        /// </summary>
        private readonly StatSkill senseImprovement;

        /// <summary>
        /// </summary>
        private readonly Stat sessionTime;

        /// <summary>
        /// </summary>
        private readonly Stat sex;

        /// <summary>
        /// </summary>
        private readonly Stat shadowBreed;

        /// <summary>
        /// </summary>
        private readonly Stat shadowBreedTemplate;

        /// <summary>
        /// </summary>
        private readonly Stat shieldChemicalAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldColdAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldEnergyAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldFireAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldMeleeAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldNanoAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldPoisonAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldProjectileAC;

        /// <summary>
        /// </summary>
        private readonly Stat shieldRadiationAC;

        /// <summary>
        /// </summary>
        private readonly Stat shopFlags;

        /// <summary>
        /// </summary>
        private readonly Stat shopId;

        /// <summary>
        /// </summary>
        private readonly Stat shopIndex;

        /// <summary>
        /// </summary>
        private readonly Stat shopLastUsed;

        /// <summary>
        /// </summary>
        private readonly Stat shopPrice;

        /// <summary>
        /// </summary>
        private readonly Stat shopRent;

        /// <summary>
        /// </summary>
        private readonly Stat shopType;

        /// <summary>
        /// </summary>
        private readonly StatSkill shotgun;

        /// <summary>
        /// </summary>
        private readonly Stat shoulderMeshHolder;

        /// <summary>
        /// </summary>
        private readonly Stat shoulderMeshLeft;

        /// <summary>
        /// </summary>
        private readonly Stat shoulderMeshRight;

        /// <summary>
        /// </summary>
        private readonly Stat side;

        /// <summary>
        /// </summary>
        private readonly Stat sisCap;

        /// <summary>
        /// </summary>
        private readonly Stat sk;

        /// <summary>
        /// </summary>
        private readonly Stat skillDisabled;

        /// <summary>
        /// </summary>
        private readonly Stat skillLockModifier;

        /// <summary>
        /// </summary>
        private readonly Stat skillTimeOnSelectedTarget;

        /// <summary>
        /// </summary>
        private readonly StatSkill sneakAttack;

        /// <summary>
        /// </summary>
        private readonly Stat socialStatus;

        /// <summary>
        /// </summary>
        private readonly Stat soundVolume;

        /// <summary>
        /// </summary>
        private readonly Stat specialAttackShield;

        /// <summary>
        /// </summary>
        private readonly Stat specialCondition;

        /// <summary>
        /// </summary>
        private readonly Stat specialization;

        /// <summary>
        /// </summary>
        private readonly Stat speedPenalty;

        /// <summary>
        /// </summary>
        private readonly Stat stability;

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine2;

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine3;

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine4;

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine5;

        /// <summary>
        /// </summary>
        private readonly Stat stackingLine6;

        /// <summary>
        /// </summary>
        private readonly Stat stackingOrder;

        /// <summary>
        /// </summary>
        private readonly Stat stamina;

        /// <summary>
        /// </summary>
        private readonly Stat statOne;

        /// <summary>
        /// </summary>
        private readonly Stat state;

        /// <summary>
        /// </summary>
        private readonly Stat stateAction;

        /// <summary>
        /// </summary>
        private readonly Stat stateMachine;

        /// <summary>
        /// </summary>
        private readonly Stat staticInstance;

        /// <summary>
        /// </summary>
        private readonly Stat staticType;

        /// <summary>
        /// </summary>
        private readonly Stat streamCheckMagic;

        /// <summary>
        /// </summary>
        private readonly Stat strength;

        /// <summary>
        /// </summary>
        private readonly StatSkill subMachineGun;

        /// <summary>
        /// </summary>
        private readonly StatSkill swim;

        /// <summary>
        /// </summary>
        private readonly Stat synergyHash;

        /// <summary>
        /// </summary>
        private readonly Stat taboo;

        /// <summary>
        /// </summary>
        private readonly Stat targetDistance;

        /// <summary>
        /// </summary>
        private readonly Stat targetDistanceChange;

        /// <summary>
        /// </summary>
        private readonly Stat targetFacing;

        /// <summary>
        /// </summary>
        private readonly Stat team;

        /// <summary>
        /// </summary>
        private readonly Stat teamAllowed;

        /// <summary>
        /// </summary>
        private readonly Stat teamCloseness;

        /// <summary>
        /// </summary>
        private readonly Stat teamSide;

        /// <summary>
        /// </summary>
        private readonly Stat teleportPauseMilliSeconds;

        /// <summary>
        /// </summary>
        private readonly Stat tempSavePlayfield;

        /// <summary>
        /// </summary>
        private readonly Stat tempSaveTeamId;

        /// <summary>
        /// </summary>
        private readonly Stat tempSaveX;

        /// <summary>
        /// </summary>
        private readonly Stat tempSaveY;

        /// <summary>
        /// </summary>
        private readonly Stat temporarySkillReduction;

        /// <summary>
        /// </summary>
        private readonly StatSkill throwingKnife;

        /// <summary>
        /// </summary>
        private readonly StatSkill thrownGrapplingWeapons;

        /// <summary>
        /// </summary>
        private readonly Stat tideRequiredDynelId;

        /// <summary>
        /// </summary>
        private readonly Stat timeExist;

        /// <summary>
        /// </summary>
        private readonly Stat timeSinceCreation;

        /// <summary>
        /// </summary>
        private readonly Stat timeSinceUpkeep;

        /// <summary>
        /// </summary>
        private readonly StatTitleLevel titleLevel;

        /// <summary>
        /// </summary>
        private readonly Stat totalDamage;

        /// <summary>
        /// </summary>
        private readonly Stat totalMass;

        /// <summary>
        /// </summary>
        private readonly Stat towerInstance;

        /// <summary>
        /// </summary>
        private readonly Stat towerNpcHash;

        /// <summary>
        /// </summary>
        private readonly Stat towerType;

        /// <summary>
        /// </summary>
        private readonly Stat tracerEffectType;

        /// <summary>
        /// </summary>
        private readonly Stat trackChemicalDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackColdDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackEnergyDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackFireDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackMeleeDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackPoisonDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackProjectileDamage;

        /// <summary>
        /// </summary>
        private readonly Stat trackRadiationDamage;

        /// <summary>
        /// </summary>
        private readonly Stat tradeLimit;

        /// <summary>
        /// </summary>
        private readonly Stat trainSkill;

        /// <summary>
        /// </summary>
        private readonly Stat trainSkillCost;

        /// <summary>
        /// </summary>
        private readonly Stat trapDifficulty;

        /// <summary>
        /// </summary>
        private readonly Stat travelSound;

        /// <summary>
        /// </summary>
        private readonly StatSkill treatment;

        /// <summary>
        /// </summary>
        private readonly Stat turnSpeed;

        /// <summary>
        /// </summary>
        private readonly StatSkill tutoring;

        /// <summary>
        /// </summary>
        private readonly StatSkill twohBluntWeapons;

        /// <summary>
        /// </summary>
        private readonly StatSkill twohEdgedWeapons;

        /// <summary>
        /// </summary>
        private readonly Stat unarmedTemplateInstance;

        /// <summary>
        /// </summary>
        private readonly Stat unreadMailCount;

        /// <summary>
        /// </summary>
        private readonly Stat unsavedXP;

        /// <summary>
        /// </summary>
        private readonly Stat userInstance;

        /// <summary>
        /// </summary>
        private readonly Stat userType;

        /// <summary>
        /// </summary>
        private readonly Stat vehicleAC;

        /// <summary>
        /// </summary>
        private readonly Stat vehicleDamage;

        /// <summary>
        /// </summary>
        private readonly Stat vehicleHealth;

        /// <summary>
        /// </summary>
        private readonly Stat vehicleSpeed;

        /// <summary>
        /// </summary>
        private readonly Stat veteranPoints;

        /// <summary>
        /// </summary>
        private readonly Stat vicinityRange;

        /// <summary>
        /// </summary>
        private readonly Stat victoryPoints;

        /// <summary>
        /// </summary>
        private readonly Stat visualBreed;

        /// <summary>
        /// </summary>
        private readonly Stat visualFlags;

        /// <summary>
        /// </summary>
        private readonly Stat visualLodLevel;

        /// <summary>
        /// </summary>
        private readonly Stat visualProfession;

        /// <summary>
        /// </summary>
        private readonly Stat visualSex;

        /// <summary>
        /// </summary>
        private readonly Stat volumeMass;

        /// <summary>
        /// </summary>
        private readonly Stat voteCount;

        /// <summary>
        /// </summary>
        private readonly Stat waitState;

        /// <summary>
        /// </summary>
        private readonly Stat weaponDisallowedInstance;

        /// <summary>
        /// </summary>
        private readonly Stat weaponDisallowedType;

        /// <summary>
        /// </summary>
        private readonly Stat weaponMeshHolder;

        /// <summary>
        /// </summary>
        private readonly Stat weaponMeshLeft;

        /// <summary>
        /// </summary>
        private readonly Stat weaponMeshRight;

        /// <summary>
        /// </summary>
        private readonly StatSkill weaponSmithing;

        /// <summary>
        /// </summary>
        private readonly Stat weaponStyleLeft;

        /// <summary>
        /// </summary>
        private readonly Stat weaponStyleRight;

        /// <summary>
        /// </summary>
        private readonly Stat weaponsStyle;

        /// <summary>
        /// </summary>
        private readonly Stat xp;

        /// <summary>
        /// </summary>
        private readonly Stat xpBonus;

        /// <summary>
        /// </summary>
        private readonly Stat xpKillRange;

        /// <summary>
        /// </summary>
        private readonly Stat xpModifier;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Character_Stats
        /// Class for character's stats
        /// </summary>
        /// <param name="owner">
        /// </param>
        public Stats(Identity owner)
        {
            this.owner = owner;

            this.absorbChemicalAC = new Stat(this, 241, 0, true, false, false);
            this.absorbColdAC = new Stat(this, 243, 0, true, false, false);
            this.absorbEnergyAC = new Stat(this, 240, 0, true, false, false);
            this.absorbFireAC = new Stat(this, 244, 0, true, false, false);
            this.absorbMeleeAC = new Stat(this, 239, 0, true, false, false);
            this.absorbNanoAC = new Stat(this, 246, 0, true, false, false);
            this.absorbPoisonAC = new Stat(this, 245, 0, true, false, false);
            this.absorbProjectileAC = new Stat(this, 238, 0, true, false, false);
            this.absorbRadiationAC = new Stat(this, 242, 0, true, false, false);
            this.accessCount = new Stat(this, 35, 1234567890, false, false, false);
            this.accessGrant = new Stat(this, 258, 1234567890, false, false, false);
            this.accessKey = new Stat(this, 195, 1234567890, false, false, false);
            this.accountFlags = new Stat(this, 660, 1234567890, false, true, false);
            this.accumulatedDamage = new Stat(this, 222, 1234567890, false, false, false);
            this.acgEntranceStyles = new Stat(this, 384, 1234567890, false, false, false);
            this.acgItemCategoryId = new Stat(this, 704, 1234567890, false, false, false);
            this.acgItemLevel = new Stat(this, 701, 1234567890, false, false, false);
            this.acgItemSeed = new Stat(this, 700, 1234567890, false, false, false);
            this.acgItemTemplateId = new Stat(this, 702, 1234567890, false, false, false);
            this.acgItemTemplateId2 = new Stat(this, 703, 1234567890, false, false, false);
            this.actionCategory = new Stat(this, 588, 1234567890, false, false, false);
            this.advantageHash1 = new Stat(this, 651, 1234567890, false, false, false);
            this.advantageHash2 = new Stat(this, 652, 1234567890, false, false, false);
            this.advantageHash3 = new Stat(this, 653, 1234567890, false, false, false);
            this.advantageHash4 = new Stat(this, 654, 1234567890, false, false, false);
            this.advantageHash5 = new Stat(this, 655, 1234567890, false, false, false);
            this.adventuring = new StatSkill(this, 137, 5, true, false, false);
            this.age = new Stat(this, 58, 0, false, false, false);
            this.aggDef = new Stat(this, 51, 100, false, false, false);
            this.aggressiveness = new Stat(this, 201, 1234567890, false, false, false);
            this.agility = new Stat(this, 17, 0, true, false, false);
            this.aimedShot = new StatSkill(this, 151, 5, true, false, false);
            this.alienLevel = new Stat(this, 169, 0, false, false, false);
            this.alienNextXP = new StatAlienNextXP(this, 178, 1500, true, false, false);
            this.alienXP = new Stat(this, 40, 0, false, false, false);
            this.alignment = new Stat(this, 62, 0, false, false, false);
            this.ammoName = new Stat(this, 399, 1234567890, false, false, false);
            this.ammoType = new Stat(this, 420, 1234567890, false, false, false);
            this.ams = new Stat(this, 22, 1234567890, false, false, false);
            this.amsCap = new Stat(this, 538, 1234567890, false, false, false);
            this.amsModifier = new Stat(this, 276, 0, false, false, false);
            this.anim = new Stat(this, 13, 1234567890, false, false, false);
            this.animPlay = new Stat(this, 501, 1234567890, false, false, false);
            this.animPos = new Stat(this, 500, 1234567890, false, false, false);
            this.animSet = new Stat(this, 353, 1234567890, false, false, false);
            this.animSpeed = new Stat(this, 502, 1234567890, false, false, false);
            this.apartmentAccessCard = new Stat(this, 584, 1234567890, false, false, false);
            this.apartmentsAllowed = new Stat(this, 582, 1, false, false, false);
            this.apartmentsOwned = new Stat(this, 583, 0, false, false, false);
            this.areaInstance = new Stat(this, 87, 1234567890, false, false, false);
            this.areaType = new Stat(this, 86, 1234567890, false, false, false);
            this.armourType = new Stat(this, 424, 1234567890, false, false, false);
            this.assaultRifle = new StatSkill(this, 116, 5, true, false, false);
            this.attackCount = new Stat(this, 36, 1234567890, false, false, false);
            this.attackRange = new Stat(this, 287, 1234567890, false, false, false);
            this.attackShield = new Stat(this, 516, 1234567890, false, false, false);
            this.attackSpeed = new Stat(this, 3, 5, false, false, false);
            this.attackType = new Stat(this, 354, 1234567890, false, false, false);
            this.attitude = new Stat(this, 63, 0, false, false, false);
            this.autoAttackFlags = new Stat(this, 349, 5, false, false, false);
            this.autoLockTimeDefault = new Stat(this, 175, 1234567890, false, false, false);
            this.autoUnlockTimeDefault = new Stat(this, 176, 1234567890, false, false, false);
            this.backMesh = new Stat(this, 38, 0, false, false, false);
            this.backstab = new Stat(this, 489, 1234567890, true, false, false);
            this.bandolierSlots = new Stat(this, 46, 1234567890, false, false, false);
            this.battlestationRep = new Stat(this, 670, 10, false, false, false);
            this.battlestationSide = new Stat(this, 668, 0, false, false, false);
            this.beltSlots = new Stat(this, 45, 0, false, false, false);
            this.berserkMode = new Stat(this, 235, 1234567890, false, false, false);
            this.biologicalMetamorphose = new StatSkill(this, 128, 5, true, false, false);
            this.birthDate = new Stat(this, 248, 1234567890, false, false, false);
            this.bodyDevelopment = new StatSkill(this, 152, 5, true, false, false);
            this.bow = new StatSkill(this, 111, 5, true, false, false);
            this.bowSpecialAttack = new StatSkill(this, 121, 5, true, false, false);
            this.brainType = new Stat(this, 340, 1234567890, false, false, false);
            this.brawl = new StatSkill(this, 142, 5, true, false, false);
            this.breakingEntry = new StatSkill(this, 165, 5, true, false, false);
            this.breed = new Stat(this, 4, 1, false, false, false); // Needed to set default value to 1
            this.breedHostility = new Stat(this, 204, 1234567890, false, false, false);
            this.breedLimit = new Stat(this, 320, 1234567890, false, false, false);
            this.buildingComplexInst = new Stat(this, 188, 1234567890, false, false, false);
            this.buildingInstance = new Stat(this, 185, 1234567890, false, false, false);
            this.buildingType = new Stat(this, 184, 1234567890, false, false, false);
            this.burst = new StatSkill(this, 148, 5, true, false, false);
            this.burstRecharge = new Stat(this, 374, 1234567890, false, false, false);
            this.buyModifier = new Stat(this, 426, 1234567890, false, false, false);
            this.can = new Stat(this, 30, 1234567890, false, false, false);
            this.canChangeClothes = new Stat(this, 223, 1234567890, false, false, false);
            this.cardOwnerInstance = new Stat(this, 187, 1234567890, false, false, false);
            this.cardOwnerType = new Stat(this, 186, 1234567890, false, false, false);
            this.cash = new Stat(this, 61, 0, false, false, false);
            this.castEffectType = new Stat(this, 428, 1234567890, false, false, false);
            this.castSelfAbstractAnim = new Stat(this, 378, 1234567890, false, false, false);
            this.castSound = new Stat(this, 270, 1234567890, false, false, false);
            this.castTargetAbstractAnim = new Stat(this, 377, 1234567890, false, false, false);
            this.catAnim = new Stat(this, 401, 1234567890, false, false, false);
            this.catAnimFlags = new Stat(this, 402, 1234567890, false, false, false);
            this.catMesh = new Stat(this, 42, 1234567890, false, false, false);
            this.chanceOfBreakOnDebuff = new Stat(this, 386, 1234567890, false, false, false);
            this.chanceOfBreakOnSpellAttack = new Stat(this, 385, 1234567890, false, false, false);
            this.chanceOfUse = new Stat(this, 422, 1234567890, false, false, false);
            this.changeSideCount = new Stat(this, 237, 0, false, false, false);
            this.charRadius = new Stat(this, 421, 1234567890, false, false, false);
            this.charState = new Stat(this, 434, 1234567890, false, false, false);
            this.charTmp1 = new Stat(this, 441, 1234567890, false, false, false);
            this.charTmp2 = new Stat(this, 442, 1234567890, false, false, false);
            this.charTmp3 = new Stat(this, 443, 1234567890, false, false, false);
            this.charTmp4 = new Stat(this, 444, 1234567890, false, false, false);
            this.chemicalAC = new Stat(this, 93, 0, true, false, false);
            this.chemicalDamageModifier = new Stat(this, 281, 0, false, false, false);
            this.chemistry = new StatSkill(this, 163, 5, true, false, false);
            this.chestFlags = new Stat(this, 394, 1234567890, false, false, false);
            this.cityInstance = new Stat(this, 640, 1234567890, false, false, false);
            this.cityTerminalRechargePercent = new Stat(this, 642, 1234567890, false, false, false);
            this.clan = new Stat(this, 5, 0, false, false, false);
            this.clanConserver = new Stat(this, 571, 0, false, false, false);
            this.clanDevoted = new Stat(this, 570, 0, false, false, false);
            this.clanFinalized = new Stat(this, 314, 1234567890, false, false, false);
            this.clanGaia = new Stat(this, 563, 0, false, false, false);
            this.clanHierarchy = new Stat(this, 260, 1234567890, false, false, false);
            this.clanInstance = new Stat(this, 305, 1234567890, false, false, false);
            this.clanItemInstance = new Stat(this, 331, 1234567890, false, false, false);
            this.clanItemType = new Stat(this, 330, 1234567890, false, false, false);
            this.clanLevel = new Stat(this, 48, 1234567890, false, false, false);
            this.clanPrice = new Stat(this, 302, 1234567890, false, false, false);
            this.clanRedeemed = new Stat(this, 572, 0, false, false, false);
            this.clanSentinels = new Stat(this, 561, 0, false, false, false);
            this.clanType = new Stat(this, 304, 1234567890, false, false, false);
            this.clanUpkeepInterval = new Stat(this, 312, 1234567890, false, false, false);
            this.clanVanguards = new Stat(this, 565, 0, false, false, false);
            this.clientActivated = new Stat(this, 262, 1234567890, false, false, false);
            this.closeCombatInitiative = new StatSkill(this, 118, 5, true, false, false);
            this.coldAC = new Stat(this, 95, 0, true, false, false);
            this.coldDamageModifier = new Stat(this, 311, 1234567890, false, false, false);
            this.collideCheckInterval = new Stat(this, 437, 1234567890, false, false, false);
            this.collisionRadius = new Stat(this, 357, 1234567890, false, false, false);
            this.commandRange = new Stat(this, 456, 1234567890, false, false, false);
            this.compulsion = new Stat(this, 328, 1234567890, false, false, false);
            this.computerLiteracy = new StatSkill(this, 161, 5, true, false, false);
            this.concealment = new StatSkill(this, 164, 5, true, false, false);
            this.conditionState = new Stat(this, 530, 1234567890, false, false, false);
            this.conformity = new Stat(this, 200, 1234567890, false, false, false);
            this.corpseAnimKey = new Stat(this, 417, 1234567890, false, false, false);
            this.corpseHash = new Stat(this, 398, 1234567890, false, false, false);
            this.corpseInstance = new Stat(this, 416, 1234567890, false, false, false);
            this.corpseType = new Stat(this, 415, 1234567890, false, false, false);
            this.criticalDecrease = new Stat(this, 391, 1234567890, false, false, false);
            this.criticalIncrease = new Stat(this, 379, 1234567890, false, false, false);
            this.currBodyLocation = new Stat(this, 220, 0, false, false, false);
            this.currentMass = new Stat(this, 78, 0, false, false, false);
            this.currentMovementMode = new Stat(this, 173, 3, false, false, false);
            this.currentNCU = new Stat(this, 180, 0, false, false, false);
            this.currentNano = new StatCurrentNano(this, 214, 1, true, false, false);
            this.currentPlayfield = new Stat(this, 589, 1234567890, false, false, false);
            this.currentState = new Stat(this, 423, 0, false, false, false);
            this.currentTime = new Stat(this, 578, 1234567890, false, false, false);
            this.damageBonus = new Stat(this, 284, 1234567890, false, false, false);
            this.damageOverrideType = new Stat(this, 339, 1234567890, false, false, false);
            this.damageToNano = new Stat(this, 659, 1234567890, false, false, false);
            this.damageToNanoMultiplier = new Stat(this, 661, 1234567890, false, false, false);
            this.damageType = new Stat(this, 436, 1234567890, false, false, false);
            this.deadTimer = new Stat(this, 34, 0, false, false, false);
            this.deathReason = new Stat(this, 338, 1234567890, false, false, false);
            this.debuffFormula = new Stat(this, 332, 1234567890, false, false, false);
            this.defaultAttackType = new Stat(this, 292, 1234567890, false, false, false);
            this.defaultPos = new Stat(this, 88, 1234567890, false, false, false);
            this.desiredTargetDistance = new Stat(this, 447, 1234567890, false, false, false);
            this.dieAnim = new Stat(this, 387, 1234567890, false, false, false);
            this.dimach = new StatSkill(this, 144, 5, true, false, false);
            this.disarmTrap = new StatSkill(this, 135, 5, true, false, false);
            this.displayCATAnim = new Stat(this, 403, 1234567890, false, false, false);
            this.displayCATMesh = new Stat(this, 404, 1234567890, false, false, false);
            this.distanceToSpawnpoint = new Stat(this, 641, 1234567890, false, false, false);
            this.distanceWeaponInitiative = new StatSkill(this, 119, 5, true, false, false);
            this.districtNano = new Stat(this, 590, 1234567890, false, false, false);
            this.districtNanoInterval = new Stat(this, 591, 1234567890, false, false, false);
            this.dms = new Stat(this, 29, 1234567890, false, false, false);
            this.dmsModifier = new Stat(this, 277, 0, false, false, false);
            this.dodge = new StatSkill(this, 154, 5, true, false, false);
            this.doorBlockTime = new Stat(this, 335, 1234567890, false, false, false);
            this.doorFlags = new Stat(this, 259, 1234567890, false, false, false);
            this.driveAir = new StatSkill(this, 139, 5, true, false, false);
            this.driveGround = new StatSkill(this, 166, 5, true, false, false);
            this.driveWater = new StatSkill(this, 117, 5, true, false, false);
            this.duck = new StatSkill(this, 153, 5, true, false, false);
            this.dudChance = new Stat(this, 534, 1234567890, false, false, false);
            this.durationModifier = new Stat(this, 464, 1234567890, false, false, false);
            this.effectBlue = new Stat(this, 462, 1234567890, false, false, false);
            this.effectGreen = new Stat(this, 461, 1234567890, false, false, false);
            this.effectIcon = new Stat(this, 183, 1234567890, false, false, false);
            this.effectRed = new Stat(this, 460, 1234567890, false, false, false);
            this.effectType = new Stat(this, 413, 1234567890, false, false, false);
            this.electricalEngineering = new StatSkill(this, 126, 5, true, false, false);
            this.energy = new Stat(this, 26, 1234567890, false, false, false);
            this.energyAC = new Stat(this, 92, 0, true, false, false);
            this.energyDamageModifier = new Stat(this, 280, 0, false, false, false);
            this.equipDelay = new Stat(this, 211, 1234567890, false, false, false);
            this.equippedWeapons = new Stat(this, 274, 1234567890, false, false, false);
            this.evade = new StatSkill(this, 155, 5, true, false, false);
            this.exitInstance = new Stat(this, 189, 1234567890, false, false, false);
            this.expansion = new Stat(this, 389, 0, false, true, false);
            this.expansionPlayfield = new Stat(this, 531, 1234567890, false, false, false);
            this.extenalDoorInstance = new Stat(this, 193, 1234567890, false, false, false);
            this.extenalPlayfieldInstance = new Stat(this, 192, 1234567890, false, false, false);
            this.extendedFlags = new Stat(this, 598, 1234567890, false, false, false);
            this.extendedTime = new Stat(this, 373, 1234567890, false, false, false);
            this.extroverty = new Stat(this, 203, 1234567890, false, false, false);
            this.fabricType = new Stat(this, 41, 1234567890, false, false, false);
            this.face = new Stat(this, 31, 1234567890, false, false, false);
            this.faceTexture = new Stat(this, 347, 1234567890, false, false, false);
            this.factionModifier = new Stat(this, 543, 1234567890, false, false, false);
            this.fallDamage = new Stat(this, 474, 1234567890, false, false, false);
            this.fastAttack = new StatSkill(this, 147, 5, true, false, false);
            this.fatness = new Stat(this, 47, 1234567890, false, false, false);
            this.features = new Stat(this, 224, 6, false, false, false);
            this.fieldQuantumPhysics = new StatSkill(this, 157, 5, true, false, false);
            this.fireAC = new Stat(this, 97, 0, true, false, false);
            this.fireDamageModifier = new Stat(this, 316, 0, false, false, false);
            this.firstAid = new StatSkill(this, 123, 5, true, false, false);
            this.fixtureFlags = new Stat(this, 473, 1234567890, false, false, false);
            this.flags = new Stat(this, 0, 8917569, false, false, true);
            this.flingShot = new StatSkill(this, 150, 5, true, false, false);
            this.fullAuto = new StatSkill(this, 167, 5, true, false, false);
            this.fullAutoRecharge = new Stat(this, 375, 1234567890, false, false, false);
            this.gatherAbstractAnim = new Stat(this, 376, 1234567890, false, false, false);
            this.gatherEffectType = new Stat(this, 366, 1234567890, false, false, false);
            this.gatherSound = new Stat(this, 269, 1234567890, false, false, false);
            this.genderLimit = new Stat(this, 321, 1234567890, false, false, false);
            this.globalClanInstance = new Stat(this, 310, 1234567890, false, false, false);
            this.globalClanType = new Stat(this, 309, 1234567890, false, false, false);
            this.globalResearchGoal = new Stat(this, 266, 0, false, false, false);
            this.globalResearchLevel = new Stat(this, 264, 0, false, false, false);
            this.gmLevel = new StatGmLevel(this, 215, 0, false, true, false);
            this.gos = new Stat(this, 566, 0, false, false, false);
            this.grenade = new StatSkill(this, 109, 5, true, false, false);
            this.hairMesh = new Stat(this, 32, 0, false, false, false);
            this.hasAlwaysLootable = new Stat(this, 345, 1234567890, false, false, false);
            this.hasKnuBotData = new Stat(this, 768, 1234567890, false, false, false);
            this.hateValueModifyer = new Stat(this, 288, 1234567890, false, false, false);
            this.headMesh = new OverridingModifierStat(this, 64, 0, false, false, false);
            this.healDelta = new StatHealDelta(this, 343, 1234567890, true, false, false);
            this.healInterval = new StatHealInterval(this, 342, 29, true, false, false);
            this.healMultiplier = new Stat(this, 535, 1234567890, false, false, false);
            this.health = new StatHitPoints(this, 27, 1, true, false, true);
            this.healthChange = new Stat(this, 172, 1234567890, false, false, false);
            this.healthChangeBest = new Stat(this, 170, 1234567890, false, false, false);
            this.healthChangeWorst = new Stat(this, 171, 1234567890, false, false, false);
            this.height = new Stat(this, 28, 1234567890, false, false, false);
            this.hitEffectType = new Stat(this, 361, 1234567890, false, false, false);
            this.hitSound = new Stat(this, 272, 1234567890, false, false, false);
            this.houseTemplate = new Stat(this, 620, 1234567890, false, false, false);
            this.hpLevelUp = new Stat(this, 601, 1234567890, false, false, false);
            this.hpPerSkill = new Stat(this, 602, 1234567890, false, false, false);
            this.icon = new Stat(this, 79, 0, false, false, false);
            this.impactEffectType = new Stat(this, 414, 1234567890, false, false, false);
            this.inPlay = new Stat(this, 194, 0, false, false, false);
            this.info = new Stat(this, 15, 1234567890, false, false, false);
            this.initiativeType = new Stat(this, 440, 1234567890, false, false, false);
            this.instance = new Stat(this, 1002, 1234567890, false, true, false);
            this.insurancePercentage = new Stat(this, 236, 0, false, false, false);
            this.insuranceTime = new Stat(this, 49, 0, false, false, false);
            this.intelligence = new Stat(this, 19, 0, true, false, false);
            this.interactionRadius = new Stat(this, 297, 1234567890, false, false, false);
            this.interruptModifier = new Stat(this, 383, 1234567890, false, false, false);
            this.invadersKilled = new Stat(this, 615, 0, false, false, false);
            this.inventoryId = new Stat(this, 55, 1234567890, false, false, false);
            this.inventoryTimeout = new Stat(this, 50, 1234567890, false, false, false);
            this.ip = new StatIp(this, 53, 1500, false, true, false);
            this.isFightingMe = new Stat(this, 410, 0, false, false, false);
            this.isVehicle = new Stat(this, 658, 1234567890, false, false, false);
            this.itemAnim = new Stat(this, 99, 1234567890, true, false, false);
            this.itemClass = new Stat(this, 76, 1234567890, false, false, false);
            this.itemDelay = new Stat(this, 294, 1234567890, false, false, false);
            this.itemDelayCap = new Stat(this, 523, 1234567890, false, false, false);
            this.itemHateValue = new Stat(this, 283, 1234567890, false, false, false);
            this.itemOpposedSkill = new Stat(this, 295, 1234567890, false, false, false);
            this.itemSIS = new Stat(this, 296, 1234567890, false, false, false);
            this.itemSkill = new Stat(this, 293, 1234567890, false, false, false);
            this.itemType = new Stat(this, 72, 0, false, false, false);
            this.killedByInvaders = new Stat(this, 616, 0, false, false, false);
            this.lastConcretePlayfieldInstance = new Stat(this, 191, 0, false, false, false);
            this.lastMailCheckTime = new Stat(this, 650, 1283065897, false, false, false);
            this.lastPerkResetTime = new Stat(this, 577, 0, false, false, false);
            this.lastRnd = new Stat(this, 522, 1234567890, false, false, false);
            this.lastSK = new Stat(this, 574, 0, false, false, false);
            this.lastSaveXP = new Stat(this, 372, 0, false, false, false);
            this.lastSaved = new Stat(this, 249, 1234567890, false, false, false);
            this.lastXP = new Stat(this, 57, 0, false, false, false);
            this.leaderLockDownTime = new Stat(this, 614, 1234567890, false, false, false);
            this.level = new Stat(this, 54, 1234567890, false, false, false);
            this.levelLimit = new Stat(this, 322, 1234567890, false, false, false);
            this.life = new StatLife(this, 1, 1, true, false, true);
            this.liquidType = new Stat(this, 268, 1234567890, false, false, false);
            this.lockDifficulty = new Stat(this, 299, 1234567890, false, false, false);
            this.lockDownTime = new Stat(this, 613, 1234567890, false, false, false);
            this.losHeight = new Stat(this, 466, 1234567890, false, false, false);
            this.lowresMesh = new Stat(this, 390, 1234567890, false, false, false);
            this.lrEnergyWeapon = new StatSkill(this, 133, 5, true, false, false);
            this.lrMultipleWeapon = new StatSkill(this, 134, 5, true, false, false);
            this.mapAreaPart1 = new Stat(this, 471, 0, false, false, false);
            this.mapAreaPart2 = new Stat(this, 472, 0, false, false, false);
            this.mapAreaPart3 = new Stat(this, 585, 0, false, false, false);
            this.mapAreaPart4 = new Stat(this, 586, 0, false, false, false);
            this.mapFlags = new Stat(this, 9, 0, false, false, false);
            this.mapNavigation = new StatSkill(this, 140, 5, true, false, false);
            this.mapOptions = new Stat(this, 470, 0, false, false, false);
            this.martialArts = new StatSkill(this, 100, 5, true, false, false);
            this.materialCreation = new StatSkill(this, 130, 5, true, false, false);
            this.materialLocation = new StatSkill(this, 131, 5, true, false, false);
            this.materialMetamorphose = new StatSkill(this, 127, 5, true, false, false);
            this.maxDamage = new Stat(this, 285, 1234567890, false, false, false);
            this.maxEnergy = new Stat(this, 212, 1234567890, false, false, false);
            this.maxMass = new Stat(this, 24, 1234567890, false, false, false);
            this.maxNCU = new Stat(this, 181, 8, false, false, false);
            this.maxNanoEnergy = new StatMaxNanoEnergy(this, 221, 1, false, false, false);
            this.maxShopItems = new Stat(this, 606, 1234567890, false, false, false);
            this.mechData = new Stat(this, 662, 0, false, false, false);
            this.mechanicalEngineering = new StatSkill(this, 125, 5, true, false, false);
            this.meleeAC = new Stat(this, 91, 0, true, false, false);
            this.meleeDamageModifier = new Stat(this, 279, 0, false, false, false);
            this.meleeEnergyWeapon = new StatSkill(this, 104, 5, true, false, false);
            this.meleeMultiple = new StatSkill(this, 101, 5, true, false, false);
            this.memberInstance = new Stat(this, 308, 1234567890, false, false, false);
            this.memberType = new Stat(this, 307, 1234567890, false, false, false);
            this.members = new Stat(this, 300, 999, false, false, false);
            this.mesh = new OverridingModifierStat(this, 12, 0, false, true, false);
            this.metaType = new Stat(this, 75, 0, false, false, false);
            this.metersWalked = new Stat(this, 252, 1234567890, false, false, false);
            this.minDamage = new Stat(this, 286, 1234567890, false, false, false);
            this.minMembers = new Stat(this, 301, 1234567890, false, false, false);
            this.missionBits1 = new Stat(this, 256, 0, false, false, false);
            this.missionBits10 = new Stat(this, 617, 0, false, false, false);
            this.missionBits11 = new Stat(this, 618, 0, false, false, false);
            this.missionBits12 = new Stat(this, 619, 0, false, false, false);
            this.missionBits2 = new Stat(this, 257, 0, false, false, false);
            this.missionBits3 = new Stat(this, 303, 0, false, false, false);
            this.missionBits4 = new Stat(this, 432, 0, false, false, false);
            this.missionBits5 = new Stat(this, 65, 0, false, false, false);
            this.missionBits6 = new Stat(this, 66, 0, false, false, false);
            this.missionBits7 = new Stat(this, 67, 0, false, false, false);
            this.missionBits8 = new Stat(this, 544, 0, false, false, false);
            this.missionBits9 = new Stat(this, 545, 0, false, false, false);
            this.monsterData = new Stat(this, 359, 0, false, false, true);
            this.monsterLevelsKilled = new Stat(this, 254, 1234567890, false, false, false);
            this.monsterScale = new Stat(this, 360, 1234567890, false, false, true);
            this.monsterTexture = new Stat(this, 344, 1234567890, false, false, false);
            this.monthsPaid = new Stat(this, 69, 0, false, false, false);
            this.moreFlags = new Stat(this, 177, 1234567890, false, false, true);
            this.multipleCount = new Stat(this, 412, 1234567890, false, false, false);
            this.name = new Stat(this, 14, 1234567890, false, false, false);
            this.nameTemplate = new Stat(this, 446, 1234567890, false, false, false);
            this.nanoAC = new StatSkill(this, 168, 5, true, false, false);
            this.nanoDamageModifier = new Stat(this, 315, 0, false, false, false);
            this.nanoDamageMultiplier = new Stat(this, 536, 0, false, false, false);
            this.nanoDelta = new StatNanoDelta(this, 364, 1234567890, true, false, false);
            this.nanoEnergyPool = new StatSkill(this, 132, 5, true, false, false);
            this.nanoFocusLevel = new Stat(this, 355, 0, false, false, false);
            this.nanoInterval = new StatNanoInterval(this, 363, 28, true, false, false);
            this.nanoPoints = new Stat(this, 407, 1234567890, false, false, false);
            this.nanoProgramming = new StatSkill(this, 160, 5, true, false, false);
            this.nanoProwessInitiative = new StatSkill(this, 149, 5, true, false, false);
            this.nanoSpeed = new Stat(this, 406, 1234567890, false, false, false);
            this.nanoVulnerability = new Stat(this, 537, 1234567890, false, false, false);
            this.newbieHP = new Stat(this, 600, 1234567890, false, false, false);
            this.newbieNP = new Stat(this, 603, 1234567890, false, false, false);
            this.nextDoorInBuilding = new Stat(this, 190, 1234567890, false, false, false);
            this.nextFormula = new Stat(this, 411, 1234567890, false, false, false);
            this.nextSK = new StatNextSK(this, 575, 0, true, false, false);
            this.nextXP = new StatNextXP(this, 350, 1450, true, false, false);
            this.npCostModifier = new Stat(this, 318, 0, false, false, false);
            this.npLevelUp = new Stat(this, 604, 1234567890, false, false, false);
            this.npPerSkill = new Stat(this, 605, 1234567890, false, false, false);
            this.npcBrainState = new Stat(this, 429, 1234567890, false, false, false);
            this.npcCommand = new Stat(this, 439, 1234567890, false, false, false);
            this.npcCommandArg = new Stat(this, 445, 1234567890, false, false, false);
            this.npcCryForHelpRange = new Stat(this, 465, 1234567890, false, false, false);
            this.npcFamily = new Stat(this, 455, 1234567890, false, false, false);
            this.npcFlags = new Stat(this, 179, 1234567890, false, false, false);
            this.npcFovStatus = new Stat(this, 533, 1234567890, false, false, false);
            this.npcHasPatrolList = new Stat(this, 452, 1234567890, false, false, false);
            this.npcHash = new Stat(this, 356, 1234567890, false, false, false);
            this.npcHatelistSize = new Stat(this, 457, 1234567890, false, false, false);
            this.npcIsSurrendering = new Stat(this, 449, 1234567890, false, false, false);
            this.npcNumPets = new Stat(this, 458, 1234567890, false, false, false);
            this.npcScriptAmsScale = new Stat(this, 581, 1234567890, false, false, false);
            this.npcSpellArg1 = new Stat(this, 638, 1234567890, false, false, false);
            this.npcSpellRet1 = new Stat(this, 639, 1234567890, false, false, false);
            this.npcSurrenderInstance = new Stat(this, 451, 1234567890, false, false, false);
            this.npcUseFightModeRegenRate = new Stat(this, 519, 1234567890, false, false, false);
            this.npcVicinityChars = new Stat(this, 453, 1234567890, false, false, false);
            this.npcVicinityFamily = new Stat(this, 580, 1234567890, false, false, false);
            this.npcVicinityPlayers = new Stat(this, 518, 1234567890, false, false, false);
            this.numAttackEffects = new Stat(this, 291, 1234567890, false, false, false);
            this.numberOfItems = new Stat(this, 396, 1234567890, false, false, false);
            this.numberOfTeamMembers = new Stat(this, 587, 1234567890, false, false, false);
            this.numberOnHateList = new Stat(this, 529, 1234567890, false, false, false);
            this.objectType = new Stat(this, 1001, 1234567890, false, true, false);
            this.odMaxSizeAdd = new Stat(this, 463, 1234567890, false, false, false);
            this.odMinSizeAdd = new Stat(this, 459, 1234567890, false, false, false);
            this.oldTimeExist = new Stat(this, 392, 1234567890, false, false, false);
            this.onTowerCreation = new Stat(this, 513, 1234567890, false, false, false);
            this.onehBluntWeapons = new StatSkill(this, 102, 5, true, false, false);
            this.onehEdgedWeapon = new StatSkill(this, 103, 5, true, false, false);
            this.orientationMode = new Stat(this, 197, 1234567890, false, false, false);
            this.originatorType = new Stat(this, 490, 1234567890, false, false, false);
            this.otArmedForces = new Stat(this, 560, 0, false, false, false);
            this.otFollowers = new Stat(this, 567, 0, false, false, false);
            this.otMed = new Stat(this, 562, 1234567890, false, false, false);
            this.otOperator = new Stat(this, 568, 0, false, false, false);
            this.otTrans = new Stat(this, 564, 0, false, false, false);
            this.otUnredeemed = new Stat(this, 569, 0, false, false, false);
            this.outerRadius = new Stat(this, 358, 1234567890, false, false, false);
            this.overrideMaterial = new Stat(this, 337, 1234567890, false, false, false);
            this.overrideTexture = new Stat(this, 336, 1234567890, false, false, false);
            this.overrideTextureAttractor = new Stat(this, 1014, 0, false, false, false);
            this.overrideTextureBack = new Stat(this, 1013, 0, false, false, false);
            this.overrideTextureHead = new Stat(this, 1008, 0, false, false, false);
            this.overrideTextureShoulderpadLeft = new Stat(this, 1012, 0, false, false, false);
            this.overrideTextureShoulderpadRight = new Stat(this, 1011, 0, false, false, false);
            this.overrideTextureWeaponLeft = new Stat(this, 1010, 0, false, false, false);
            this.overrideTextureWeaponRight = new Stat(this, 1009, 0, false, false, false);
            this.ownedTowers = new Stat(this, 514, 1234567890, false, false, false);
            this.ownerInstance = new Stat(this, 433, 1234567890, false, false, false);
            this.paidPoints = new Stat(this, 672, 0, false, false, false);
            this.parentInstance = new Stat(this, 44, 1234567890, false, false, false);
            this.parentType = new Stat(this, 43, 1234567890, false, false, false);
            this.parry = new StatSkill(this, 145, 5, true, false, false);
            this.percentChemicalDamage = new Stat(this, 628, 1234567890, false, false, false);
            this.percentColdDamage = new Stat(this, 622, 1234567890, false, false, false);
            this.percentEnergyDamage = new Stat(this, 627, 1234567890, false, false, false);
            this.percentFireDamage = new Stat(this, 621, 1234567890, false, false, false);
            this.percentMeleeDamage = new Stat(this, 623, 1234567890, false, false, false);
            this.percentPoisonDamage = new Stat(this, 625, 1234567890, false, false, false);
            this.percentProjectileDamage = new Stat(this, 624, 1234567890, false, false, false);
            this.percentRadiationDamage = new Stat(this, 626, 1234567890, false, false, false);
            this.percentRemainingHealth = new Stat(this, 525, 1234567890, false, false, false);
            this.percentRemainingNano = new Stat(this, 526, 1234567890, false, false, false);
            this.perception = new StatSkill(this, 136, 5, true, false, false);
            this.personalResearchGoal = new Stat(this, 265, 0, false, false, false);
            this.personalResearchLevel = new Stat(this, 263, 0, false, false, false);
            this.petCounter = new Stat(this, 251, 1234567890, false, false, false);
            this.petMaster = new Stat(this, 196, 1234567890, false, false, false);
            this.petReq1 = new Stat(this, 467, 1234567890, false, false, false);
            this.petReq2 = new Stat(this, 468, 1234567890, false, false, false);
            this.petReq3 = new Stat(this, 469, 1234567890, false, false, false);
            this.petReqVal1 = new Stat(this, 485, 1234567890, false, false, false);
            this.petReqVal2 = new Stat(this, 486, 1234567890, false, false, false);
            this.petReqVal3 = new Stat(this, 487, 1234567890, false, false, false);
            this.petState = new Stat(this, 671, 1234567890, false, false, false);
            this.petType = new Stat(this, 512, 1234567890, false, false, false);
            this.pharmaceuticals = new StatSkill(this, 159, 5, true, false, false);
            this.physicalProwessInitiative = new StatSkill(this, 120, 5, true, false, false);
            this.piercing = new StatSkill(this, 106, 5, true, false, false);
            this.pistol = new StatSkill(this, 112, 5, true, false, false);
            this.placement = new Stat(this, 298, 1234567890, false, false, false);
            this.playerId = new Stat(this, 607, 1234567890, false, true, false);
            this.playerKilling = new Stat(this, 323, 1234567890, false, false, false);
            this.playerOptions = new Stat(this, 576, 0, false, false, false);
            this.playfieldType = new Stat(this, 438, 1234567890, false, false, false);
            this.poisonAC = new Stat(this, 96, 0, true, false, false);
            this.poisonDamageModifier = new Stat(this, 317, 0, false, false, false);
            this.prevMovementMode = new Stat(this, 174, 3, false, false, false);
            this.previousHealth = new Stat(this, 11, 50, false, false, false);
            this.price = new Stat(this, 74, 1234567890, false, false, false);
            this.primaryItemInstance = new Stat(this, 81, 1234567890, false, false, false);
            this.primaryItemType = new Stat(this, 80, 1234567890, false, false, false);
            this.primaryTemplateId = new Stat(this, 395, 1234567890, false, false, false);
            this.procChance1 = new Stat(this, 556, 1234567890, false, false, false);
            this.procChance2 = new Stat(this, 557, 1234567890, false, false, false);
            this.procChance3 = new Stat(this, 558, 1234567890, false, false, false);
            this.procChance4 = new Stat(this, 559, 1234567890, false, false, false);
            this.procInitiative1 = new Stat(this, 539, 1234567890, false, false, false);
            this.procInitiative2 = new Stat(this, 540, 1234567890, false, false, false);
            this.procInitiative3 = new Stat(this, 541, 1234567890, false, false, false);
            this.procInitiative4 = new Stat(this, 542, 1234567890, false, false, false);
            this.procNano1 = new Stat(this, 552, 1234567890, false, false, false);
            this.procNano2 = new Stat(this, 553, 1234567890, false, false, false);
            this.procNano3 = new Stat(this, 554, 1234567890, false, false, false);
            this.procNano4 = new Stat(this, 555, 1234567890, false, false, false);
            this.profession = new Stat(this, 60, 1, false, false, false);
            this.professionLevel = new Stat(this, 10, 1234567890, false, true, false);
            this.projectileAC = new Stat(this, 90, 0, true, false, false);
            this.projectileDamageModifier = new Stat(this, 278, 0, false, false, false);
            this.proximityRangeIndoors = new Stat(this, 484, 1234567890, false, false, false);
            this.proximityRangeOutdoors = new Stat(this, 454, 1234567890, false, false, false);
            this.psychic = new Stat(this, 21, 0, true, false, false);
            this.psychologicalModification = new StatSkill(this, 129, 5, true, false, false);
            this.psychology = new StatSkill(this, 162, 5, true, false, false);
            this.pvPLevelsKilled = new Stat(this, 255, 1234567890, false, false, false);
            this.pvpDuelDeaths = new Stat(this, 675, 0, false, false, false);
            this.pvpDuelKills = new Stat(this, 674, 0, false, false, false);
            this.pvpDuelScore = new Stat(this, 684, 0, false, false, false);
            this.pvpProfessionDuelDeaths = new Stat(this, 677, 0, false, false, false);
            this.pvpProfessionDuelKills = new Stat(this, 676, 0, false, false, false);
            this.pvpRankedSoloDeaths = new Stat(this, 679, 0, false, false, false);
            this.pvpRankedSoloKills = new Stat(this, 678, 0, false, false, false);
            this.pvpRankedTeamDeaths = new Stat(this, 681, 0, false, false, false);
            this.pvpRankedTeamKills = new Stat(this, 680, 0, false, false, false);
            this.pvpRating = new Stat(this, 333, 1300, false, false, false);
            this.pvpSoloScore = new Stat(this, 682, 0, false, false, false);
            this.pvpTeamScore = new Stat(this, 683, 0, false, false, false);
            this.qtDungeonInstance = new Stat(this, 497, 1234567890, false, false, false);
            this.qtKillNumMonsterCount1 = new Stat(this, 504, 1234567890, false, false, false);
            this.qtKillNumMonsterCount2 = new Stat(this, 506, 1234567890, false, false, false);
            this.qtKillNumMonsterCount3 = new Stat(this, 508, 1234567890, false, false, false);
            this.qtKillNumMonsterID3 = new Stat(this, 507, 1234567890, false, false, false);
            this.qtKillNumMonsterId1 = new Stat(this, 503, 1234567890, false, false, false);
            this.qtKillNumMonsterId2 = new Stat(this, 505, 1234567890, false, false, false);
            this.qtKilledMonsters = new Stat(this, 499, 1234567890, false, false, false);
            this.qtNumMonsters = new Stat(this, 498, 1234567890, false, false, false);
            this.questAsMaximumRange = new Stat(this, 802, 1234567890, false, false, false);
            this.questAsMinimumRange = new Stat(this, 801, 1234567890, false, false, false);
            this.questBoothDifficulty = new Stat(this, 800, 1234567890, false, false, false);
            this.questIndex0 = new Stat(this, 509, 1234567890, false, false, false);
            this.questIndex1 = new Stat(this, 492, 1234567890, false, false, false);
            this.questIndex2 = new Stat(this, 493, 1234567890, false, false, false);
            this.questIndex3 = new Stat(this, 494, 1234567890, false, false, false);
            this.questIndex4 = new Stat(this, 495, 1234567890, false, false, false);
            this.questIndex5 = new Stat(this, 496, 1234567890, false, false, false);
            this.questInstance = new Stat(this, 491, 1234567890, false, false, false);
            this.questLevelsSolved = new Stat(this, 253, 1234567890, false, false, false);
            this.questStat = new Stat(this, 261, 1234567890, false, false, false);
            this.questTimeout = new Stat(this, 510, 1234567890, false, false, false);
            this.race = new Stat(this, 89, 1, false, false, false);
            this.radiationAC = new Stat(this, 94, 0, true, false, false);
            this.radiationDamageModifier = new Stat(this, 282, 0, false, false, false);
            this.rangeIncreaserNF = new Stat(this, 381, 0, false, false, false);
            this.rangeIncreaserWeapon = new Stat(this, 380, 0, false, false, false);
            this.readOnly = new Stat(this, 435, 1234567890, false, false, false);
            this.rechargeDelay = new Stat(this, 210, 1234567890, false, false, false);
            this.rechargeDelayCap = new Stat(this, 524, 1234567890, false, false, false);
            this.reclaimItem = new Stat(this, 365, 1234567890, false, false, false);
            this.reflectChemicalAC = new Stat(this, 208, 0, true, false, false);
            this.reflectColdAC = new Stat(this, 217, 0, true, false, false);
            this.reflectEnergyAC = new Stat(this, 207, 0, true, false, false);
            this.reflectFireAC = new Stat(this, 219, 0, true, false, false);
            this.reflectMeleeAC = new Stat(this, 206, 0, true, false, false);
            this.reflectNanoAC = new Stat(this, 218, 0, true, false, false);
            this.reflectPoisonAC = new Stat(this, 225, 0, false, false, false);
            this.reflectProjectileAC = new Stat(this, 205, 0, true, false, false);
            this.reflectRadiationAC = new Stat(this, 216, 0, true, false, false);
            this.reflectReturnedChemicalAC = new Stat(this, 478, 0, false, false, false);
            this.reflectReturnedColdAC = new Stat(this, 480, 0, false, false, false);
            this.reflectReturnedEnergyAC = new Stat(this, 477, 0, false, false, false);
            this.reflectReturnedFireAC = new Stat(this, 482, 0, false, false, false);
            this.reflectReturnedMeleeAC = new Stat(this, 476, 0, false, false, false);
            this.reflectReturnedNanoAC = new Stat(this, 481, 0, false, false, false);
            this.reflectReturnedPoisonAC = new Stat(this, 483, 0, false, false, false);
            this.reflectReturnedProjectileAC = new Stat(this, 475, 0, false, false, false);
            this.reflectReturnedRadiationAC = new Stat(this, 479, 0, false, false, false);
            this.regainXPPercentage = new Stat(this, 593, 0, false, false, false);
            this.repairDifficulty = new Stat(this, 73, 1234567890, false, false, false);
            this.repairSkill = new Stat(this, 77, 1234567890, false, false, false);
            this.resistModifier = new Stat(this, 393, 1234567890, false, false, false);
            this.restModifier = new Stat(this, 425, 1234567890, false, false, false);
            this.resurrectDest = new Stat(this, 362, 1234567890, false, false, false);
            this.rifle = new StatSkill(this, 113, 5, true, false, false);
            this.riposte = new StatSkill(this, 143, 5, true, false, false);
            this.ritualTargetInst = new Stat(this, 370, 1234567890, false, false, false);
            this.rnd = new Stat(this, 520, 1234567890, false, false, false);
            this.rotation = new Stat(this, 400, 1234567890, false, false, false);
            this.rp = new Stat(this, 199, 0, false, false, false);
            this.runSpeed = new StatSkill(this, 156, 5, true, false, false);
            this.savedXP = new Stat(this, 334, 0, false, false, false);
            this.school = new Stat(this, 405, 1234567890, false, false, false);
            this.secondaryItemInstance = new Stat(this, 83, 1234567890, false, false, false);
            this.secondaryItemTemplate = new Stat(this, 273, 1234567890, false, false, false);
            this.secondaryItemType = new Stat(this, 82, 1234567890, false, false, false);
            this.selectedTarget = new Stat(this, 431, 1234567890, false, false, false);
            this.selectedTargetType = new Stat(this, 397, 1234567890, false, false, false);
            this.sellModifier = new Stat(this, 427, 1234567890, false, false, false);
            this.sense = new Stat(this, 20, 0, true, false, false);
            this.senseImprovement = new StatSkill(this, 122, 5, true, false, false);
            this.sessionTime = new Stat(this, 198, 1234567890, false, false, false);
            this.sex = new Stat(this, 59, 1234567890, false, false, false);
            this.shadowBreed = new Stat(this, 532, 0, false, false, false);
            this.shadowBreedTemplate = new Stat(this, 579, 0, false, false, false);
            this.shieldChemicalAC = new Stat(this, 229, 0, true, false, false);
            this.shieldColdAC = new Stat(this, 231, 0, true, false, false);
            this.shieldEnergyAC = new Stat(this, 228, 0, true, false, false);
            this.shieldFireAC = new Stat(this, 233, 0, true, false, false);
            this.shieldMeleeAC = new Stat(this, 227, 0, true, false, false);
            this.shieldNanoAC = new Stat(this, 232, 0, true, false, false);
            this.shieldPoisonAC = new Stat(this, 234, 0, true, false, false);
            this.shieldProjectileAC = new Stat(this, 226, 0, true, false, false);
            this.shieldRadiationAC = new Stat(this, 230, 0, true, false, false);
            this.shopFlags = new Stat(this, 610, 1234567890, false, false, false);
            this.shopId = new Stat(this, 657, 1234567890, false, false, false);
            this.shopIndex = new Stat(this, 656, 1234567890, false, false, false);
            this.shopLastUsed = new Stat(this, 611, 1234567890, false, false, false);
            this.shopPrice = new Stat(this, 599, 1234567890, false, false, false);
            this.shopRent = new Stat(this, 608, 1234567890, false, false, false);
            this.shopType = new Stat(this, 612, 1234567890, false, false, false);
            this.shotgun = new StatSkill(this, 115, 5, true, false, false);
            this.shoulderMeshHolder = new Stat(this, 39, 0, false, false, false);
            this.shoulderMeshLeft = new Stat(this, 1005, 0, false, false, false);
            this.shoulderMeshRight = new Stat(this, 1004, 0, false, false, false);
            this.side = new Stat(this, 33, 0, false, false, false);
            this.sisCap = new Stat(this, 352, 1234567890, false, false, false);
            this.sk = new Stat(this, 573, 0, false, false, false);
            this.skillDisabled = new Stat(this, 329, 1234567890, false, false, false);
            this.skillLockModifier = new Stat(this, 382, 0, false, false, false);
            this.skillTimeOnSelectedTarget = new Stat(this, 371, 1234567890, false, false, false);
            this.sneakAttack = new StatSkill(this, 146, 5, true, false, false);
            this.socialStatus = new Stat(this, 521, 0, false, false, false);
            this.soundVolume = new Stat(this, 250, 1234567890, false, false, false);
            this.specialAttackShield = new Stat(this, 517, 1234567890, false, false, false);
            this.specialCondition = new Stat(this, 348, 1, false, false, false);
            this.specialization = new Stat(this, 182, 0, false, false, false);
            this.speedPenalty = new Stat(this, 70, 1234567890, false, false, false);
            this.stability = new Stat(this, 202, 1234567890, false, false, false);
            this.stackingLine2 = new Stat(this, 546, 1234567890, false, false, false);
            this.stackingLine3 = new Stat(this, 547, 1234567890, false, false, false);
            this.stackingLine4 = new Stat(this, 548, 1234567890, false, false, false);
            this.stackingLine5 = new Stat(this, 549, 1234567890, false, false, false);
            this.stackingLine6 = new Stat(this, 550, 1234567890, false, false, false);
            this.stackingOrder = new Stat(this, 551, 1234567890, false, false, false);
            this.stamina = new Stat(this, 18, 0, true, false, false);
            this.statOne = new Stat(this, 290, 1234567890, false, false, false);
            this.state = new Stat(this, 7, 0, false, false, false);
            this.stateAction = new Stat(this, 98, 1234567890, true, false, false);
            this.stateMachine = new Stat(this, 450, 1234567890, false, false, false);
            this.staticInstance = new Stat(this, 23, 1234567890, false, false, false);
            this.staticType = new Stat(this, 25, 1234567890, false, false, false);
            this.streamCheckMagic = new Stat(this, 999, 1234567890, false, false, false);
            this.strength = new Stat(this, 16, 0, true, false, false);
            this.subMachineGun = new StatSkill(this, 114, 5, true, false, false);
            this.swim = new StatSkill(this, 138, 5, true, false, false);
            this.synergyHash = new Stat(this, 609, 1234567890, false, false, false);
            this.taboo = new Stat(this, 327, 1234567890, false, false, false);
            this.targetDistance = new Stat(this, 527, 1234567890, false, false, false);
            this.targetDistanceChange = new Stat(this, 889, 1234567890, false, false, false);
            this.targetFacing = new Stat(this, 488, 1234567890, false, false, false);
            this.team = new Stat(this, 6, 0, false, false, false);
            this.teamAllowed = new Stat(this, 324, 1234567890, false, false, false);
            this.teamCloseness = new Stat(this, 528, 1234567890, false, false, false);
            this.teamSide = new Stat(this, 213, 0, false, false, false);
            this.teleportPauseMilliSeconds = new Stat(this, 351, 1234567890, false, false, false);
            this.tempSavePlayfield = new Stat(this, 595, 0, false, false, false);
            this.tempSaveTeamId = new Stat(this, 594, 0, false, false, false);
            this.tempSaveX = new Stat(this, 596, 0, false, false, false);
            this.tempSaveY = new Stat(this, 597, 0, false, false, false);
            this.temporarySkillReduction = new Stat(this, 247, 0, false, false, false);
            this.throwingKnife = new StatSkill(this, 108, 5, true, false, false);
            this.thrownGrapplingWeapons = new StatSkill(this, 110, 5, true, false, false);
            this.tideRequiredDynelId = new Stat(this, 900, 1234567890, false, false, false);
            this.timeExist = new Stat(this, 8, 1234567890, false, false, false);
            this.timeSinceCreation = new Stat(this, 56, 1234567890, false, false, false);
            this.timeSinceUpkeep = new Stat(this, 313, 1234567890, false, false, false);
            this.titleLevel = new StatTitleLevel(this, 37, 1, false, false, false);
            this.totalDamage = new Stat(this, 629, 1234567890, false, false, false);
            this.totalMass = new Stat(this, 71, 1234567890, false, false, false);
            this.towerInstance = new Stat(this, 515, 1234567890, false, false, false);
            this.towerNpcHash = new Stat(this, 511, 1234567890, false, false, false);
            this.towerType = new Stat(this, 388, 1234567890, false, false, false);
            this.tracerEffectType = new Stat(this, 419, 1234567890, false, false, false);
            this.trackChemicalDamage = new Stat(this, 633, 1234567890, false, false, false);
            this.trackColdDamage = new Stat(this, 635, 1234567890, false, false, false);
            this.trackEnergyDamage = new Stat(this, 632, 1234567890, false, false, false);
            this.trackFireDamage = new Stat(this, 637, 1234567890, false, false, false);
            this.trackMeleeDamage = new Stat(this, 631, 1234567890, false, false, false);
            this.trackPoisonDamage = new Stat(this, 636, 1234567890, false, false, false);
            this.trackProjectileDamage = new Stat(this, 630, 1234567890, false, false, false);
            this.trackRadiationDamage = new Stat(this, 634, 1234567890, false, false, false);
            this.tradeLimit = new Stat(this, 346, 1234567890, false, false, false);
            this.trainSkill = new Stat(this, 408, 1234567890, false, false, false);
            this.trainSkillCost = new Stat(this, 409, 1234567890, false, false, false);
            this.trapDifficulty = new Stat(this, 289, 1234567890, false, false, false);
            this.travelSound = new Stat(this, 271, 1234567890, false, false, false);
            this.treatment = new StatSkill(this, 124, 5, true, false, false);
            this.turnSpeed = new Stat(this, 267, 40000, false, false, false);
            this.tutoring = new StatSkill(this, 141, 5, true, false, false);
            this.twohBluntWeapons = new StatSkill(this, 107, 5, true, false, false);
            this.twohEdgedWeapons = new StatSkill(this, 105, 5, true, false, false);
            this.unarmedTemplateInstance = new Stat(this, 418, 0, false, false, false);
            this.unreadMailCount = new Stat(this, 649, 0, false, false, false);
            this.unsavedXP = new Stat(this, 592, 0, false, false, false);
            this.userInstance = new Stat(this, 85, 1234567890, false, false, false);
            this.userType = new Stat(this, 84, 1234567890, false, false, false);
            this.vehicleAC = new Stat(this, 664, 1234567890, false, false, false);
            this.vehicleDamage = new Stat(this, 665, 1234567890, false, false, false);
            this.vehicleHealth = new Stat(this, 666, 1234567890, false, false, false);
            this.vehicleSpeed = new Stat(this, 667, 1234567890, false, false, false);
            this.veteranPoints = new Stat(this, 68, 0, false, false, false);
            this.vicinityRange = new Stat(this, 448, 1234567890, false, false, false);
            this.victoryPoints = new Stat(this, 669, 0, false, false, false);
            this.visualBreed = new Stat(this, 367, 1234567890, false, false, true);
            this.visualFlags = new Stat(this, 673, 31, false, false, false);
            this.visualLodLevel = new Stat(this, 888, 1234567890, false, false, false);
            this.visualProfession = new Stat(this, 368, 1234567890, false, false, true);
            this.visualSex = new Stat(this, 369, 1234567890, false, false, true);
            this.volumeMass = new Stat(this, 2, 1234567890, false, false, false);
            this.voteCount = new Stat(this, 306, 1234567890, false, false, false);
            this.waitState = new Stat(this, 430, 2, false, false, false);
            this.weaponDisallowedInstance = new Stat(this, 326, 1234567890, false, false, false);
            this.weaponDisallowedType = new Stat(this, 325, 1234567890, false, false, false);
            this.weaponMeshHolder = new Stat(this, 209, 0, false, false, false);
            this.weaponMeshLeft = new Stat(this, 1007, 0, false, false, false);
            this.weaponMeshRight = new Stat(this, 1006, 0, false, false, false);
            this.weaponSmithing = new StatSkill(this, 158, 5, true, false, false);
            this.weaponStyleLeft = new Stat(this, 1015, 0, false, false, false);
            this.weaponStyleRight = new Stat(this, 1016, 0, false, false, false);
            this.weaponsStyle = new Stat(this, 1003, 1234567890, false, false, false);
            this.xp = new Stat(this, 52, 0, false, false, false);
            this.xpBonus = new Stat(this, 341, 1234567890, false, false, false);
            this.xpKillRange = new Stat(this, 275, 5, false, false, false);
            this.xpModifier = new Stat(this, 319, 0, false, false, false);

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
            this.nanoEnergyPool.Affects.Add(this.nanoDelta.StatId);
            this.currentMovementMode.Affects.Add(this.nanoDelta.StatId);
            this.currentMovementMode.Affects.Add(this.healDelta.StatId);
            this.currentMovementMode.Affects.Add(this.nanoInterval.StatId);
            this.currentMovementMode.Affects.Add(this.healInterval.StatId);
            this.level.Affects.Add(this.life.StatId);
            this.level.Affects.Add(this.health.StatId);
            this.level.Affects.Add(this.maxNanoEnergy.StatId);
            this.level.Affects.Add(this.titleLevel.StatId);
            this.level.Affects.Add(this.nextSK.StatId);
            this.level.Affects.Add(this.nextXP.StatId);
            this.level.Affects.Add(this.ip.StatId);
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
            this.stamina.Affects.Add(this.bodyDevelopment.StatId);

            this.expansion.DoNotDontWriteToSql = true;
            this.accountFlags.DoNotDontWriteToSql = true;
            this.playerId.DoNotDontWriteToSql = true;
            this.professionLevel.DoNotDontWriteToSql = true;
            this.objectType.DoNotDontWriteToSql = true;
            this.instance.DoNotDontWriteToSql = true;
            this.gmLevel.DoNotDontWriteToSql = true;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// </summary>
        public event EventHandler<StatChangedEventArgs> AfterStatChangedEvent;

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
        public GameTuple<CharacterStat, uint>[] ChangedAnnouncingStats
        {
            get
            {
                List<GameTuple<CharacterStat, uint>> temp = new List<GameTuple<CharacterStat, uint>>();
                foreach (Stat stat in this.all)
                {
                    if (stat.Changed && stat.AnnounceToPlayfield)
                    {
                        temp.Add(
                            new GameTuple<CharacterStat, uint>()
                            {
                                Value1 = (CharacterStat)stat.StatId,
                                Value2 =
                                    stat.SendBaseValue
                                        ? stat.BaseValue
                                        : (uint)stat.Value
                            });
                    }
                }

                return temp.ToArray();
            }
        }

        /// <summary>
        /// </summary>
        public GameTuple<CharacterStat, uint>[] ChangedStats
        {
            get
            {
                List<GameTuple<CharacterStat, uint>> temp = new List<GameTuple<CharacterStat, uint>>();
                foreach (Stat stat in this.all)
                {
                    if (stat.Changed)
                    {
                        temp.Add(
                            new GameTuple<CharacterStat, uint>()
                            {
                                Value1 = (CharacterStat)stat.StatId,
                                Value2 =
                                    stat.SendBaseValue
                                        ? stat.BaseValue
                                        : (uint)stat.Value
                            });
                        stat.Changed = false;
                    }
                }

                return temp.ToArray();
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
        public Identity Owner
        {
            get
            {
                return this.owner;
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

        #region Public Indexers

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <exception cref="StatDoesNotExistException">
        /// </exception>
        /// <returns>
        /// </returns>
        public IStat this[int index]
        {
            get
            {
                try
                {
                    return this.All.Single(x => x.StatId == index);
                }
                catch (Exception)
                {
                    throw new StatDoesNotExistException("Stat with Id " + index + " does not exist");
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="i">
        /// </param>
        /// <returns>
        /// </returns>
        public IStat this[StatIds i]
        {
            get
            {
                return this.all.Single(x => x.StatId == (int)i);
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
        public IStat this[string name]
        {
            get
            {
                int index = StatNamesDefaults.GetStatNumber(name);
                return this.all.Single(x => x.StatId == index);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        public void AfterStatChangedEventHandler(StatChangedEventArgs e)
        {
            EventHandler<StatChangedEventArgs> handler = this.AfterStatChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

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
        public Stat GetStatByNumber(int number)
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
        /// </summary>
        /// <param name="identity">
        /// </param>
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
        public bool Read()
        {
            return this.Read(this.Owner);
        }

        /// <summary>
        /// Read all stats from Sql
        /// </summary>
        /// <param name="identity">
        /// </param>
        public void ReadStatsfromSql(Identity identity)
        {
            foreach (DBStats dbStats in
                StatDao.Instance.GetAll(new{Type=(int)identity.Type, Instance=identity.Instance}))
            {
                this.SetBaseValueWithoutTriggering(dbStats.StatId, (uint)dbStats.StatValue);
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
        public void SetBaseValueWithoutTriggering(int stat, uint value)
        {
            this.all.Single(x => x.StatId == stat).SetBaseValue(value);
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
        /// </summary>
        /// <param name="identity">
        /// </param>
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
                            StatId = stat.StatId,
                            StatValue = (int)stat.BaseValue,
                            Type = typ,
                            Instance = inst
                        });
                }
            }

            if (temp.Count == 0)
            {
                StatDao.Instance.Delete(new { type = typ, Id = inst });
            }
            else
            {
                StatDao.Instance.BulkReplace(temp);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Write()
        {
            return this.Write(this.Owner);
        }

        /// <summary>
        /// Write all Stats to Sql
        /// </summary>
        public void WriteStatstoSql()
        {
            this.Write();
        }

        #endregion
    }
}