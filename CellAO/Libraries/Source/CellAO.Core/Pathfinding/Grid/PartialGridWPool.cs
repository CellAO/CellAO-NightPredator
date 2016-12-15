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

    using System.Collections.Generic;

    #endregion

    public class PartialGridWPool : BaseGrid, IBaseGrid
    {
        private readonly NodePool m_nodePool;

        public PartialGridWPool(NodePool iNodePool, GridRect? iGridRect = null)
            : base()
        {
            if (iGridRect == null)
            {
                this.m_gridRect = new GridRect();
            }
            else
            {
                this.m_gridRect = iGridRect.Value;
            }
            this.m_nodePool = iNodePool;
        }

        public int width
        {
            get
            {
                return this.m_gridRect.maxX - this.m_gridRect.minX;
            }
            protected set
            {
            }
        }

        public int height
        {
            get
            {
                return this.m_gridRect.maxY - this.m_gridRect.minY;
            }
            protected set
            {
            }
        }

        public override Node GetNodeAt(int iX, int iY)
        {
            var pos = new GridPos(iX, iY);
            return this.GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            var pos = new GridPos(iX, iY);
            return this.IsWalkableAt(pos);
        }

        public bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            if (!this.IsInside(iX, iY))
            {
                return false;
            }
            var pos = new GridPos(iX, iY);
            this.m_nodePool.SetNode(pos, iWalkable);
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            if (!this.IsInside(iPos))
            {
                return null;
            }
            return this.m_nodePool.GetNode(iPos);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            if (!this.IsInside(iPos))
            {
                return false;
            }
            return this.m_nodePool.Nodes.ContainsKey(iPos);
        }

        public bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return this.SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            int rectCount = (this.m_gridRect.maxX - this.m_gridRect.minX)
                            * (this.m_gridRect.maxY - this.m_gridRect.minY);
            if (this.m_nodePool.Nodes.Count > rectCount)
            {
                var travPos = new GridPos(0, 0);
                for (int xTrav = this.m_gridRect.minX; xTrav <= this.m_gridRect.maxX; xTrav++)
                {
                    travPos.x = xTrav;
                    for (int yTrav = this.m_gridRect.minY; yTrav <= this.m_gridRect.maxY; yTrav++)
                    {
                        travPos.y = yTrav;
                        Node curNode = this.m_nodePool.GetNode(travPos);
                        if (curNode != null)
                        {
                            curNode.Reset();
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<GridPos, Node> keyValue in this.m_nodePool.Nodes)
                {
                    keyValue.Value.Reset();
                }
            }
        }

        public void SetGridRect(GridRect iGridRect)
        {
            this.m_gridRect = iGridRect;
        }

        public bool IsInside(int iX, int iY)
        {
            if (iX < this.m_gridRect.minX || iX > this.m_gridRect.maxX || iY < this.m_gridRect.minY
                || iY > this.m_gridRect.maxY)
            {
                return false;
            }
            return true;
        }

        public bool IsInside(GridPos iPos)
        {
            return this.IsInside(iPos.x, iPos.y);
        }

        public override BaseGrid Clone()
        {
            var tNewGrid = new PartialGridWPool(this.m_nodePool, this.m_gridRect);
            return tNewGrid;
        }
    }
}