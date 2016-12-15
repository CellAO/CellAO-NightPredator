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
    public enum Expansions : int
    {
        /// <summary>
        /// </summary>
        AlienInvasion = 0x1 << 3,

        /// <summary>
        /// </summary>
        AlienInvasionPreOrder = 1 << 4,

        /// <summary>
        /// </summary>
        LegacyOfTheXan = 0x1 << 7,

        /// <summary>
        /// </summary>
        LegacyOfTheXanPreOrder = 1 << 8,

        /// <summary>
        /// </summary>
        LostEden = 0x1 << 5,

        /// <summary>
        /// </summary>
        LostEdenPreOrder = 0x1 << 6,

        /// <summary>
        /// </summary>
        NotumWars = 0x1 << 0,

        /// <summary>
        /// </summary>
        ShadowLands = 0x1 << 1,

        /// <summary>
        /// </summary>
        ShadowLandsPreOrder = 0x1 << 2,

        /// <summary>
        /// </summary>
        Mail = 0x1 << 9,

        /// <summary>
        /// </summary>
        PMVObsidianEdition = 0x1 << 10
    }
}