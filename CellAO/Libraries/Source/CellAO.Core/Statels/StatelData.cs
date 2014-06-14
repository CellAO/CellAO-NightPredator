#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace CellAO.Core.Statels
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Events;
    using CellAO.Core.Vector;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class StatelData : IEntity, IEventHolder
    {
        #region Fields

        public StatelData()
        {
            this.Events = new List<Event>();
        }

        /// <summary>
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// </summary>
        public float HeadingW;

        /// <summary>
        /// </summary>
        public float HeadingX;

        /// <summary>
        /// </summary>
        public float HeadingY;

        /// <summary>
        /// </summary>
        public float HeadingZ;

        /// <summary>
        /// </summary>
        public int PlayfieldId = 0;

        /// <summary>
        /// </summary>
        public int TemplateId = 0;

        /// <summary>
        /// </summary>
        public float X;

        /// <summary>
        /// </summary>
        public float Y;

        /// <summary>
        /// </summary>
        public float Z;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public Coordinate Coord()
        {
            return new Coordinate(this.X, this.Y, this.Z);
        }

        #endregion

        public Identity Identity { get; set; }

        public Identity Parent { get; set; }
    }
}