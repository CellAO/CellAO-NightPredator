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
// Last modified: 2013-11-16 19:07

#endregion

namespace ChatEngine.Channels
{
    #region Usings ...

    using System;

    #endregion

    /// <summary>
    /// </summary>
    [Flags]
    public enum ChannelFlags : uint
    {
        /// <summary>
        /// </summary>
        None = 0, 

        /// <summary>
        /// Can not be ignored
        /// </summary>
        CantIgnore = 0x1, 

        /// <summary>
        /// Can not send on channel
        /// </summary>
        CantSend = 0x2, 

        /// <summary>
        /// </summary>
        NoInternational = 0x10, 

        /// <summary>
        /// No Voice blobs
        /// </summary>
        NoVoice = 0x20, 

        /// <summary>
        /// </summary>
        SendCriteria = 0x40, 

        /// <summary>
        /// </summary>
        GroupOnName = 0x80, 

        /// <summary>
        /// </summary>
        Muted = 0x1000000, 
    }
}