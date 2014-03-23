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

namespace CellAO.Enums
{
    /// <summary>
    /// </summary>
    public enum Operator : int
    {
        Unknown = 255,
        Unknown2 = 272,
        Unknown3 = 110,

        /// <summary>
        /// </summary>
        And = 4, 

        /// <summary>
        /// </summary>
        AreaTargetInVicinity = 60, 

        /// <summary>
        /// </summary>
        AreaZMinMax = 15, 

        /// <summary>
        /// </summary>
        BitAnd = 22, 

        /// <summary>
        /// </summary>
        BitOr = 23, 

        /// <summary>
        /// </summary>
        CanAttackChar = 79, 

        /// <summary>
        /// </summary>
        CanDisableDefenseShield = 83, 

        /// <summary>
        /// </summary>
        CanExecuteFormulaIOnTarget = 59, 

        /// <summary>
        /// </summary>
        DistanceTo = 51, 

        /// <summary>
        /// </summary>
        EqualTo = 0, 

        /// <summary>
        /// </summary>
        False = 99, 

        /// <summary>
        /// </summary>
        GreaterThan = 2, 

        /// <summary>
        /// </summary>
        HasChangedRoomWhileFighting = 64, 

        /// <summary>
        /// </summary>
        HasEnteredNonPvpZone = 87, 

        /// <summary>
        /// </summary>
        HasFormula = 35, 

        /// <summary>
        /// </summary>
        HasMaster = 58, 

        /// <summary>
        /// </summary>
        HasMeOnPetList = 72, 

        /// <summary>
        /// </summary>
        HasMoveToTarget = 96, 

        /// <summary>
        /// </summary>
        HasNotFormula = 36, 

        /// <summary>
        /// </summary>
        HasNotPerk = 103, 

        /// <summary>
        /// </summary>
        HasNotRunningNano = 101, 

        /// <summary>
        /// </summary>
        HasNotRunningNanoLine = 102, 

        /// <summary>
        /// </summary>
        HasNotWieldedItem = 34, 

        /// <summary>
        /// </summary>
        HasNotWornItem = 32, 

        /// <summary>
        /// </summary>
        HasPerk = 93, 

        /// <summary>
        /// </summary>
        HasPetPendingNanoFormula = 76, 

        /// <summary>
        /// </summary>
        HasRunningNano = 91, 

        /// <summary>
        /// </summary>
        HasRunningNanoLine = 92, 

        /// <summary>
        /// </summary>
        HasWieldedItem = 33, 

        /// <summary>
        /// </summary>
        HasWornItem = 31, 

        /// <summary>
        /// </summary>
        Id = 9, 

        /// <summary>
        /// </summary>
        Illegal = 25, 

        /// <summary>
        /// </summary>
        InventorySlotIsEmpty = 82, 

        /// <summary>
        /// </summary>
        InventorySlotIsFull = 81, 

        /// <summary>
        /// </summary>
        IsAlive = 40, 

        /// <summary>
        /// </summary>
        IsAnyoneLooking = 47, 

        /// <summary>
        /// </summary>
        IsAttacked = 46, 

        /// <summary>
        /// </summary>
        IsFactionReactionSet = 95, 

        /// <summary>
        /// </summary>
        IsFalling = 89, 

        /// <summary>
        /// </summary>
        IsFighting = 45, 

        /// <summary>
        /// </summary>
        IsFlying = 70, 

        /// <summary>
        /// </summary>
        IsFoe = 48, 

        /// <summary>
        /// </summary>
        IsInDungeon = 49, 

        /// <summary>
        /// </summary>
        IsInNoFightingArea = 52, 

        /// <summary>
        /// </summary>
        IsInvalid = 39, 

        /// <summary>
        /// </summary>
        IsLocationOk = 62, 

        /// <summary>
        /// </summary>
        IsNotTooHighLevel = 63, 

        /// <summary>
        /// </summary>
        IsNpc = 44, 

        /// <summary>
        /// </summary>
        IsNpcOrNpcControlledPet = 84, 

        /// <summary>
        /// </summary>
        IsOnDifferentPlayfield = 90, 

        /// <summary>
        /// </summary>
        IsPerkLocked = 94, 

        /// <summary>
        /// </summary>
        IsPerkUnlocked = 97, 

        /// <summary>
        /// </summary>
        IsPet = 77, 

        /// <summary>
        /// </summary>
        IsPetOverEquipped = 75, 

        /// <summary>
        /// </summary>
        IsPlayerOrPlayerControlledPet = 86, 

        /// <summary>
        /// </summary>
        IsSameAs = 50, 

        /// <summary>
        /// </summary>
        IsTeleporting = 69, 

        /// <summary>
        /// </summary>
        IsTowerCreateAllowed = 80, 

        /// <summary>
        /// </summary>
        IsUnderHeavyAttack = 61, 

        /// <summary>
        /// </summary>
        IsValid = 38, 

        /// <summary>
        /// </summary>
        IsWithinVicinity = 41, 

        /// <summary>
        /// </summary>
        IsWithinWeaponrange = 43, 

        /// <summary>
        /// </summary>
        ItemAnim = 17, 

        /// <summary>
        /// </summary>
        ItemHas = 7, 

        /// <summary>
        /// </summary>
        ItemHasnot = 8, 

        /// <summary>
        /// </summary>
        KullNumberOf = 65, 

        /// <summary>
        /// </summary>
        LessThan = 1, 

        /// <summary>
        /// </summary>
        MinMaxLevelCompare = 54, 

        /// <summary>
        /// </summary>
        MonsterTemplate = 57, 

        /// <summary>
        /// </summary>
        Not = 42, 

        /// <summary>
        /// </summary>
        NotBitAnd = 107, 

        /// <summary>
        /// </summary>
        NumberOfItems = 67, 

        /// <summary>
        /// </summary>
        ObtainedItem = 108, 

        /// <summary>
        /// </summary>
        OnCaster = 100, 

        /// <summary>
        /// </summary>
        OnGeneralBeholder = 37, 

        /// <summary>
        /// </summary>
        OnInvalidTarget = 28, 

        /// <summary>
        /// </summary>
        OnInvalidUser = 30, 

        /// <summary>
        /// </summary>
        OnSecondaryItem = 21, 

        /// <summary>
        /// </summary>
        OnSelf = 19, 

        /// <summary>
        /// </summary>
        OnTarget = 18, 

        /// <summary>
        /// </summary>
        OnUser = 26, 

        /// <summary>
        /// </summary>
        OnValidTarget = 27, 

        /// <summary>
        /// </summary>
        OnValidUser = 29, 

        /// <summary>
        /// </summary>
        Or = 3, 

        /// <summary>
        /// </summary>
        PrimaryItem = 13, 

        /// <summary>
        /// </summary>
        PrimaryTemplate = 68, 

        /// <summary>
        /// </summary>
        SameAsSelectedTarget = 85, 

        /// <summary>
        /// </summary>
        ScanForStat = 71, 

        /// <summary>
        /// </summary>
        SecondaryItem = 14, 

        /// <summary>
        /// </summary>
        Signal = 20, 

        /// <summary>
        /// </summary>
        TargetId = 10, 

        /// <summary>
        /// </summary>
        TargetSignal = 11, 

        /// <summary>
        /// </summary>
        TargetStat = 12, 

        /// <summary>
        /// </summary>
        TemplateCompare = 53, 

        /// <summary>
        /// </summary>
        TestNumPets = 66, 

        /// <summary>
        /// </summary>
        TimeLarger = 6, 

        /// <summary>
        /// </summary>
        TimeLess = 5, 

        /// <summary>
        /// </summary>
        TrickleDownLarger = 73, 

        /// <summary>
        /// </summary>
        TrickleDownLess = 74, 

        /// <summary>
        /// </summary>
        True = 98, 

        /// <summary>
        /// </summary>
        Unequal = 24, 

        /// <summary>
        /// </summary>
        UseLocation = 88, 

        /// <summary>
        /// </summary>
        User = 16,

        /// <summary>
        /// </summary>
        FlyingAllowed = 112,

    }
}