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

namespace CellAO.Core.Pathfinding.Grid
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    #endregion

    public class Node : IComparable
    {
        // TODO:  Mabye add z cords, and change these to look more like AO?

        public int x;

        public int y;

        public bool walkable;

        public float heuristicStartToEndLen; // which passes current node

        public float startToCurNodeLen;

        public float? heuristicCurNodeToEndLen;

        public bool isOpened;

        public bool isClosed;

        public Object parent;

        public Node(int iX, int iY, bool? iWalkable = null)
        {
            this.x = iX;
            this.y = iY;
            this.walkable = (iWalkable.HasValue ? iWalkable.Value : false);
            this.heuristicStartToEndLen = 0;
            this.startToCurNodeLen = 0;
            this.heuristicCurNodeToEndLen = null;
            this.isOpened = false;
            this.isClosed = false;
            this.parent = null;
        }

        public int CompareTo(object iObj)
        {
            var tOtherNode = (Node)iObj;
            float result = this.heuristicStartToEndLen - tOtherNode.heuristicStartToEndLen;
            if (result > 0.0f)
            {
                return 1;
            }
            else if (result == 0.0f)
            {
                return 0;
            }
            return -1;
        }

        public void Reset(bool? iWalkable = null)
        {
            if (iWalkable.HasValue)
            {
                this.walkable = iWalkable.Value;
            }
            this.heuristicStartToEndLen = 0;
            this.startToCurNodeLen = 0;
            this.heuristicCurNodeToEndLen = null;
            this.isOpened = false;
            this.isClosed = false;
            this.parent = null;
        }

        public static List<GridPos> Backtrace(Node iNode)
        {
            List<GridPos> path = new List<GridPos>();
            path.Add(new GridPos(iNode.x, iNode.y));
            while (iNode.parent != null)
            {
                iNode = (Node)iNode.parent;
                path.Add(new GridPos(iNode.x, iNode.y));
            }
            path.Reverse();
            return path;
        }

        public override int GetHashCode()
        {
            return this.x ^ this.y;
        }

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as Node;
            if ((Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.x == p.x) && (this.y == p.y);
        }

        public bool Equals(Node p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.x == p.x) && (this.y == p.y);
        }

        public static bool operator ==(Node a, Node b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }
    }

    public interface IBaseGrid
    {
        int width { get; }

        int height { get; }

        Node GetNodeAt(int iX, int iY);

        bool IsWalkableAt(int iX, int iY);

        bool SetWalkableAt(int iX, int iY, bool iWalkable);

        Node GetNodeAt(GridPos iPos);

        bool IsWalkableAt(GridPos iPos);

        bool SetWalkableAt(GridPos iPos, bool iWalkable);

        void Reset();

        List<Node> GetNeighbors(Node iNode, bool iCrossCorners, bool iCrossAdjacentPoint);
    }

    public abstract class BaseGrid
    {
        protected GridRect m_gridRect;

        public BaseGrid()
        {
        }

        public GridRect gridRect
        {
            get
            {
                return this.m_gridRect;
            }
        }

        public List<Node> GetNeighbors(Node iNode, bool iCrossCorners, bool iCrossAdjacentPoint)
        {
            int tX = iNode.x;
            int tY = iNode.y;
            List<Node> neighbors = new List<Node>();
            bool tS0 = false, tD0 = false, tS1 = false, tD1 = false, tS2 = false, tD2 = false, tS3 = false, tD3 = false;

            var pos = new GridPos();
            if (this.IsWalkableAt(pos.Set(tX, tY - 1)))
            {
                neighbors.Add(this.GetNodeAt(pos));
                tS0 = true;
            }
            if (this.IsWalkableAt(pos.Set(tX + 1, tY)))
            {
                neighbors.Add(this.GetNodeAt(pos));
                tS1 = true;
            }
            if (this.IsWalkableAt(pos.Set(tX, tY + 1)))
            {
                neighbors.Add(this.GetNodeAt(pos));
                tS2 = true;
            }
            if (this.IsWalkableAt(pos.Set(tX - 1, tY)))
            {
                neighbors.Add(this.GetNodeAt(pos));
                tS3 = true;
            }
            if (iCrossCorners && iCrossAdjacentPoint)
            {
                tD0 = true;
                tD1 = true;
                tD2 = true;
                tD3 = true;
            }
            else if (iCrossCorners)
            {
                tD0 = tS3 || tS0;
                tD1 = tS0 || tS1;
                tD2 = tS1 || tS2;
                tD3 = tS2 || tS3;
            }
            else
            {
                tD0 = tS3 && tS0;
                tD1 = tS0 && tS1;
                tD2 = tS1 && tS2;
                tD3 = tS2 && tS3;
            }

            if (tD0 && this.IsWalkableAt(pos.Set(tX - 1, tY - 1)))
            {
                neighbors.Add(this.GetNodeAt(pos));
            }
            if (tD1 && this.IsWalkableAt(pos.Set(tX + 1, tY - 1)))
            {
                neighbors.Add(this.GetNodeAt(pos));
            }
            if (tD2 && this.IsWalkableAt(pos.Set(tX + 1, tY + 1)))
            {
                neighbors.Add(this.GetNodeAt(pos));
            }
            if (tD3 && this.IsWalkableAt(pos.Set(tX - 1, tY + 1)))
            {
                neighbors.Add(this.GetNodeAt(pos));
            }
            return neighbors;
        }

        public abstract void Reset();

        public abstract BaseGrid Clone();

        public abstract Node GetNodeAt(int iX, int iY);

        public abstract bool IsWalkableAt(int iX, int iY);

        public abstract Node GetNodeAt(GridPos iPos);

        public abstract bool IsWalkableAt(GridPos iPos);
    }
}