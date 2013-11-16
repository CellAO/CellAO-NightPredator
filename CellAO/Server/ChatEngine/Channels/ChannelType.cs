#region License

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
// Last modified: 2013-11-16 09:35

#endregion

namespace ChatEngine.Channels
{
    /// <summary>
    /// ChannelType is the first byte of the Channel Id
    /// </summary>
    public enum ChannelType : byte
    {
        /// <summary>
        /// </summary>
        Unknown = 0, 

        /// <summary>
        /// </summary>
        Admin = 1, 

        /// <summary>
        /// </summary>
        Team = 2 | 0x80, 

        /// <summary>
        /// </summary>
        Organization = 3, 

        /// <summary>
        /// </summary>
        Leaders = 4, 

        /// <summary>
        /// </summary>
        GM = 5, 

        /// <summary>
        /// </summary>
        Shopping = 6 | 0x80, 

        /// <summary>
        /// </summary>
        General = 7 | 0x80, 

        /// <summary>
        /// </summary>
        Towers = 10, 

        /// <summary>
        /// </summary>
        Announcements = 12, 

        /// <summary>
        /// </summary>
        Raid = 15 | 0x80, 

        /// <summary>
        /// </summary>
        Battlestation = 16 | 0x80
    }
}