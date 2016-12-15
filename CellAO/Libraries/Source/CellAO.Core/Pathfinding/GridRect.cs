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

namespace CellAO.Core.Pathfinding
{
    #region Usings ...

    using System;

    #endregion

    public struct GridRect
    {
        //TODO: Add Z cord to pathfinding.
        public int minX;

        public int minY;

        public int maxX;

        public int maxY;

        public GridRect(int iMinX, int iMinY, int iMaxX, int iMaxY)
        {
            this.minX = iMinX;
            this.minY = iMinY;
            this.maxX = iMaxX;
            this.maxY = iMaxY;
        }

        public override int GetHashCode()
        {
            return this.minX ^ this.minY ^ this.maxX ^ this.maxY;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is GridRect))
            {
                return false;
            }
            var p = (GridRect)obj;
            // Return true if the fields match:
            return (this.minX == p.minX) && (this.minY == p.minY) && (this.maxX == p.maxX) && (this.maxY == p.maxY);
        }

        public bool Equals(GridRect p)
        {
            // Return true if the fields match:
            return (this.minX == p.minX) && (this.minY == p.minY) && (this.maxX == p.maxX) && (this.maxY == p.maxY);
        }

        public static bool operator ==(GridRect a, GridRect b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // Return true if the fields match:
            return (a.minX == b.minX) && (a.minY == b.minY) && (a.maxX == b.maxX) && (a.maxY == b.maxY);
        }

        public static bool operator !=(GridRect a, GridRect b)
        {
            return !(a == b);
        }

        public GridRect Set(int iMinX, int iMinY, int iMaxX, int iMaxY)
        {
            this.minX = iMinX;
            this.minY = iMinY;
            this.maxX = iMaxX;
            this.maxY = iMaxY;
            return this;
        }
    }
}