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
// Last modified: 2013-11-16 19:03

#endregion

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System.Data.Linq;

    #endregion

    /// <summary>
    /// Data object for instanced items
    /// </summary>
    public class DBInstancedItem
    {
        #region Public Properties

        /// <summary>
        /// Instance of the container
        /// </summary>
        public int containerinstance { get; set; }

        /// <summary>
        /// Slot inside the container
        /// </summary>
        public int containerplacement { get; set; }

        /// <summary>
        /// Type id of the container
        /// </summary>
        public int containertype { get; set; }

        /// <summary>
        /// Heading (W)
        /// </summary>
        public float headingw { get; set; }

        /// <summary>
        /// Heading (X)
        /// </summary>
        public float headingx { get; set; }

        /// <summary>
        /// heading (Y)
        /// </summary>
        public float headingy { get; set; }

        /// <summary>
        /// Heading (Z)
        /// </summary>
        public float headingz { get; set; }

        /// <summary>
        /// High item Id
        /// </summary>
        public int highid { get; set; }

        /// <summary>
        /// Instance id of the item
        /// </summary>
        public int iteminstance { get; set; }

        /// <summary>
        /// Type id of the instance
        /// </summary>
        public int itemtype { get; set; }

        /// <summary>
        /// Low Item id
        /// </summary>
        public int lowid { get; set; }

        /// <summary>
        /// Multiple count stat
        /// </summary>
        public int multiplecount { get; set; }

        /// <summary>
        /// QL of the Item
        /// </summary>
        public int quality { get; set; }

        /// <summary>
        /// Item's stats
        /// </summary>
        public Binary stats { get; set; }

        /// <summary>
        /// Coordinates (X)
        /// </summary>
        public float x { get; set; }

        /// <summary>
        /// Coordinates (Y)
        /// </summary>
        public float y { get; set; }

        /// <summary>
        /// Coordinates (Z)
        /// </summary>
        public float z { get; set; }

        #endregion
    }
}