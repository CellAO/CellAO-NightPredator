#region License

// Copyright (c) 2005-2016, CellAO Team
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

namespace CellAO.Enums
{
    #region Usings ...

    using System;

    #endregion

    /// <summary>
    /// </summary>
    [Flags]
    public enum CanFlags : uint
    {
        /// <summary>
        /// </summary>
        AimedShot = 0x1 << 14,

        /// <summary>
        /// </summary>
        ApplyOnFightingTarget = (uint)0x1 << 31,

        /// <summary>
        /// </summary>
        ApplyOnFriendly = 0x1 << 21,

        /// <summary>
        /// </summary>
        ApplyOnHostile = 0x1 << 22,

        /// <summary>
        /// </summary>
        ApplyOnSelf = 0x1 << 23,

        /// <summary>
        /// </summary>
        AutoSelect = 0x1 << 20,

        /// <summary>
        /// </summary>
        Bow = 0x1 << 15,

        /// <summary>
        /// </summary>
        Brawl = 0x1 << 25,

        /// <summary>
        /// </summary>
        BreakAndEnter = 0x1 << 8,

        /// <summary>
        /// </summary>
        Burst = 0x1 << 11,

        /// <summary>
        /// </summary>
        CanBeParriedRiposted = 0x1 << 30,

        /// <summary>
        /// </summary>
        CanBeWornWithSocialArmor = 0x1 << 28,

        /// <summary>
        /// </summary>
        CanParryRiposte = 0x1 << 29,

        /// <summary>
        /// </summary>
        CantSplit = 0x1 << 24,

        /// <summary>
        /// </summary>
        Carry = 0x1 << 0,

        /// <summary>
        /// </summary>
        ConfirmUse = 0x1 << 4,

        /// <summary>
        /// </summary>
        Consume = 0x1 << 5,

        /// <summary>
        /// </summary>
        Dimach = 0x1 << 26,

        /// <summary>
        /// </summary>
        DisarmTraps = 0x1 << 19,

        /// <summary>
        /// </summary>
        EnableHandAttractors = 0x1 << 27,

        /// <summary>
        /// </summary>
        FastAttack = 0x1 << 18,

        /// <summary>
        /// </summary>
        FlingShot = 0x1 << 12,

        /// <summary>
        /// </summary>
        FullAuto = 0x1 << 13,

        /// <summary>
        /// </summary>
        NoAmmo = 0x1 << 10,

        /// <summary>
        /// </summary>
        Sit = 0x1 << 1,

        /// <summary>
        /// </summary>
        SneakAttack = 0x1 << 17,

        /// <summary>
        /// </summary>
        Stackable = 0x1 << 9,

        /// <summary>
        /// </summary>
        ThrowAttack = 0x1 << 16,

        /// <summary>
        /// </summary>
        TutorChip = 0x1 << 6,

        /// <summary>
        /// </summary>
        TutorDevice = 0x1 << 7,

        /// <summary>
        /// </summary>
        Use = 0x1 << 3,

        /// <summary>
        /// </summary>
        Wear = 0x1 << 2
    }
}