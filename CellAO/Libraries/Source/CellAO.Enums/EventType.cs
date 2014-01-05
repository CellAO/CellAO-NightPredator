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
    public enum EventType : int
    {
        /// <summary>
        /// </summary>
        OnActivate = 10, 

        /// <summary>
        /// </summary>
        OnClose = 19, 

        /// <summary>
        /// </summary>
        OnCollide = 22, 

        /// <summary>
        /// </summary>
        OnCreate = 7, 

        /// <summary>
        /// </summary>
        OnEffects = 8, 

        /// <summary>
        /// </summary>
        OnEndCollide = 23, 

        /// <summary>
        /// </summary>
        OnEndEffect = 13, 

        /// <summary>
        /// </summary>
        OnEnemyInVicinity = 25, 

        /// <summary>
        /// </summary>
        OnEnter = 16, 

        /// <summary>
        /// </summary>
        OnFailure = 27, 

        /// <summary>
        /// </summary>
        OnFriendlyInVicinity = 24, 

        /// <summary>
        /// </summary>
        OnHit = 5, 

        /// <summary>
        /// </summary>
        OnOpen = 18, 

        /// <summary>
        /// </summary>
        OnRepair = 1, 

        /// <summary>
        /// </summary>
        OnRun = 9, 

        /// <summary>
        /// </summary>
        OnStartEffect = 12, 

        /// <summary>
        /// </summary>
        OnTargetInVicinity = 3, 

        /// <summary>
        /// </summary>
        OnTerminate = 20, 

        /// <summary>
        /// </summary>
        OnTrade = 37, 

        /// <summary>
        /// </summary>
        OnUseItemOn = 4, 

        /// <summary>
        /// </summary>
        OnUse = 0, 

        /// <summary>
        /// </summary>
        OnUseFailed = 15, 

        /// <summary>
        /// </summary>
        OnWear = 14, 

        /// <summary>
        /// </summary>
        OnWield = 2, 

        /// <summary>
        /// </summary>
        PersonalModifier = 26
    }
}