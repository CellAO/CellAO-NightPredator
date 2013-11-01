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
// Last modified: 2013-11-01 21:05

#endregion

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System.Data.Linq;

    #endregion

    /// <summary>
    /// </summary>
    public class DBInstancedItem
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public int containerinstance { get; set; }

        /// <summary>
        /// </summary>
        public int containerplacement { get; set; }

        /// <summary>
        /// </summary>
        public int containertype { get; set; }

        /// <summary>
        /// </summary>
        public float headingw { get; set; }

        /// <summary>
        /// </summary>
        public float headingx { get; set; }

        /// <summary>
        /// </summary>
        public float headingy { get; set; }

        /// <summary>
        /// </summary>
        public float headingz { get; set; }

        /// <summary>
        /// </summary>
        public int highid { get; set; }

        /// <summary>
        /// </summary>
        public int iteminstance { get; set; }

        /// <summary>
        /// </summary>
        public int itemtype { get; set; }

        /// <summary>
        /// </summary>
        public int lowid { get; set; }

        /// <summary>
        /// </summary>
        public int multiplecount { get; set; }

        /// <summary>
        /// </summary>
        public int quality { get; set; }

        /// <summary>
        /// </summary>
        public Binary stats { get; set; }

        /// <summary>
        /// </summary>
        public float x { get; set; }

        /// <summary>
        /// </summary>
        public float y { get; set; }

        /// <summary>
        /// </summary>
        public float z { get; set; }

        #endregion
    }
}