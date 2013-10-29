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
// Last modified: 2013-10-29 21:43
// Created:       2013-10-29 19:57

#endregion

namespace CellAO.Enums
{
    /// <summary>
    /// Enumeration of Move modes
    /// </summary>
    public enum MoveModes
    {
        /// <summary>
        /// </summary>
        None,

        /// <summary>
        /// </summary>
        Rooted,

        /// <summary>
        /// </summary>
        Walk,

        /// <summary>
        /// </summary>
        Run,

        /// <summary>
        /// </summary>
        Swim,

        /// <summary>
        /// </summary>
        Crawl,

        /// <summary>
        /// </summary>
        Sneak,

        /// <summary>
        /// </summary>
        Fly,

        /// <summary>
        /// </summary>
        Sit,

        /// <summary>
        /// </summary>
        SocialTemp, // NV: What is this again exactly?
        /// <summary>
        /// </summary>
        Nothing,

        /// <summary>
        /// </summary>
        Sleep,

        /// <summary>
        /// </summary>
        Lounge
    }

    /// <summary>
    /// Enumeration of Spin or Strafe directions
    /// </summary>
    public enum SpinOrStrafeDirections
    {
        /// <summary>
        /// </summary>
        None,

        /// <summary>
        /// </summary>
        Left,

        /// <summary>
        /// </summary>
        Right
    }

    /// <summary>
    /// Enumeration of Move directions
    /// </summary>
    public enum MoveDirections
    {
        /// <summary>
        /// </summary>
        None,

        /// <summary>
        /// </summary>
        Forwards,

        /// <summary>
        /// </summary>
        Backwards
    }
}