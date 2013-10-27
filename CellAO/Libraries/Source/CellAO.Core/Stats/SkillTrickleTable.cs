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
// Last modified: 2013-10-27 11:37
// Created:       2013-10-27 07:58

#endregion

namespace CellAO.Core.Stats
{
    /// <summary>
    /// </summary>
    public static class SkillTrickleTable
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static double[,] table =
            {
                { 1, 0, 0.00000001, 0, 0, 0, 0 }, { 221, 0, 0, 0, 0, 0, 0.00000001 },
                { 343, 0, 0.00000001, 0, 0, 0, 0 }, { 364, 0, 0, 0, 0, 0, 0.00000001 },
                { 100, 0.2, 0.5, 0, 0, 0, 0.3 }, { 101, 0.3, 0.6, 0.1, 0, 0, 0 },
                { 102, 0.5, 0.1, 0.4, 0, 0, 0 }, { 103, 0.3, 0.4, 0.3, 0, 0, 0 },
                { 104, 0, 0, 0.3, 0.3, 0.3, 0.1 }, { 105, 0.6, 0, 0.4, 0, 0, 0 },
                { 106, 0.2, 0.5, 0.3, 0, 0, 0 }, { 107, 0.5, 0, 0.5, 0, 0, 0 },
                { 108, 0.2, 0.6, 0, 0, 0.2, 0 }, { 109, 0, 0.4, 0, 0, 0.2, 0.4 },
                { 110, 0.4, 0.6, 0, 0, 0, 0 }, { 111, 0.2, 0.4, 0, 0, 0.4, 0 },
                { 112, 0, 0.6, 0, 0, 0.4, 0 }, { 113, 0, 0.6, 0, 0, 0.4, 0 },
                { 114, 0.3, 0.3, 0.3, 0, 0.1, 0 }, { 115, 0.4, 0.6, 0, 0, 0, 0 },
                { 116, 0.1, 0.3, 0.4, 0, 0.2, 0 }, { 117, 0, 0.2, 0, 0.2, 0.6, 0 },
                { 118, 0, 0.1, 0, 0.1, 0.6, 0.2 }, { 119, 0, 0.1, 0, 0.1, 0.6, 0.2 },
                { 120, 0, 0.1, 0, 0.1, 0.6, 0.2 }, { 121, 0.1, 0.5, 0, 0, 0.4, 0 },
                { 122, 0.2, 0, 0, 0.8, 0, 0 }, { 123, 0, 0.3, 0, 0.3, 0.4, 0 },
                { 124, 0, 0.3, 0, 0.5, 0.2, 0 }, { 125, 0, 0.5, 0, 0.5, 0, 0 },
                { 126, 0, 0.3, 0.2, 0.5, 0, 0 }, { 127, 0, 0.8, 0, 0, 0, 0.2 },
                { 128, 0, 0, 0, 0.8, 0, 0.2 }, { 129, 0, 0.8, 0, 0, 0.2, 0 },
                { 130, 0, 0.8, 0.2, 0, 0, 0 }, { 131, 0, 0.2, 0, 0.8, 0, 0 },
                { 132, 0, 0.1, 0, 0.1, 0.1, 0.7 }, { 133, 0, 0, 0, 0.2, 0.4, 0.4 },
                { 134, 0, 0.6, 0, 0.4, 0, 0 }, { 135, 0, 0.3, 0, 0.2, 0.6, 0 },
                { 136, 0, 0, 0, 0.3, 0.7, 0 }, { 137, 0.2, 0.5, 0.3, 0, 0, 0 },
                { 138, 0.2, 0.6, 0.2, 0, 0, 0 }, { 139, 0, 0.2, 0, 0.2, 0.6, 0 },
                { 140, 0, 0, 0, 0.4, 0.5, 0.1 }, { 141, 0, 0, 0, 0.7, 0.2, 0.1 },
                { 142, 0.6, 0, 0.4, 0, 0, 0 }, { 143, 0, 0.5, 0, 0, 0.5, 0 },
                { 144, 0, 0, 0, 0, 0.8, 0.2 }, { 145, 0.5, 0.2, 0, 0, 0.3, 0 },
                { 146, 0, 0, 0, 0, 0.8, 0.2 }, { 147, 0, 0.6, 0, 0, 0.4, 0 },
                { 148, 0.3, 0.5, 0.2, 0, 0, 0 }, { 149, 0, 0.4, 0, 0, 0.6, 0 },
                { 150, 0, 0.3, 0, 0.3, 0.4, 0 }, { 151, 0, 0, 0, 0, 1, 0 },
                { 152, 0, 0, 1, 0, 0, 0 }, { 153, 0, 0.5, 0, 0.2, 0.3, 0 },
                { 154, 0, 0.5, 0, 0.2, 0.3, 0 }, { 155, 0, 0.5, 0, 0.2, 0.3, 0 },
                { 156, 0, 0.2, 0.4, 0.4, 0, 0 }, { 157, 0, 0.5, 0, 0, 0, 0.5 },
                { 158, 0, 0.2, 0, 0.8, 0, 0 }, { 159, 0, 0, 0, 1, 0, 0 },
                { 160, 0, 0, 0, 1, 0, 0 }, { 161, 0, 0, 0, 1, 0, 0 },
                { 162, 0, 0.5, 0, 0, 0.5, 0 }, { 163, 0, 0, 0, 0.5, 0.5, 0 },
                { 164, 0, 0.3, 0, 0, 0.7, 0 }, { 165, 0, 0.4, 0, 0, 0.3, 0.3 },
                { 166, 0, 0.2, 0, 0.2, 0.6, 0 }, { 167, 0.6, 0, 0.4, 0, 0, 0 }
            };

        #endregion
    }
}