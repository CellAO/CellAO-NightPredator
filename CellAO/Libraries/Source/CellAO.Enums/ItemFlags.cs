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
    public enum ItemFlags : int
    {
        /// <summary>
        /// </summary>
        CanBeAttacked = 0x1 << 28,

        /// <summary>
        /// </summary>
        CanBeTemplateItem = 0x1 << 3,

        /// <summary>
        /// </summary>
        DefaultTarget = 0x1 << 14,

        /// <summary>
        /// </summary>
        DisableFalling = 0x1 << 29,

        /// <summary>
        /// </summary>
        DisableStatelCollision = 0x1 << 31,

        /// <summary>
        /// </summary>
        HasAnimation = 0x1 << 17,

        /// <summary>
        /// </summary>
        HasDamage = 0x1 << 30,

        /// <summary>
        /// </summary>
        HasEnergy = 0x1 << 22,

        /// <summary>
        /// </summary>
        HasMultiplecount = 0x1 << 5,

        /// <summary>
        /// </summary>
        HasRotation = 0x1 << 18,

        /// <summary>
        /// </summary>
        HasSentFirstIir = 0x1 << 21,

        /// <summary>
        /// </summary>
        IllegalClan = 0x1 << 24,

        /// <summary>
        /// </summary>
        IllegalOmni = 0x1 << 25,

        /// <summary>
        /// </summary>
        ItemSocialArmour = 0x1 << 8,

        /// <summary>
        /// </summary>
        ItemTextureOverride = 0x1 << 15,

        /// <summary>
        /// </summary>
        Locked = 0x1 << 6,

        /// <summary>
        /// </summary>
        MirrorInLeftHand = 0x1 << 23,

        /// <summary>
        /// </summary>
        ModifiedDescription = 0x1 << 1,

        /// <summary>
        /// </summary>
        ModifiedName = 0x1 << 2,

        /// <summary>
        /// </summary>
        NoDrop = 0x1 << 26,

        /// <summary>
        /// </summary>
        NoSelectionIndicator = 0x1 << 10,

        /// <summary>
        /// </summary>
        Null = 0x1 << 16,

        /// <summary>
        /// </summary>
        Open = 0x1 << 7,

        /// <summary>
        /// </summary>
        Repulsive = 0x1 << 13,

        /// <summary>
        /// </summary>
        Stationary = 0x1 << 12,

        /// <summary>
        /// </summary>
        TellCollision = 0x1 << 9,

        /// <summary>
        /// </summary>
        TurnOnUse = 0x1 << 4,

        /// <summary>
        /// </summary>
        Unique = 0x1 << 27,

        /// <summary>
        /// </summary>
        UseEmptyDestruct = 0x1 << 11,

        /// <summary>
        /// </summary>
        Visible = 0x1 << 0,

        /// <summary>
        /// </summary>
        WantCollision = 0x1 << 19,

        /// <summary>
        /// </summary>
        WantSignals = 0x1 << 20,
    }
}