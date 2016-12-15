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

    public class DynamicGridWPool : BaseGrid, IBaseGrid
    {
        private bool m_notSet;

        private readonly NodePool m_nodePool;

        public DynamicGridWPool(NodePool iNodePool)
            : base()
        {
            this.m_gridRect = new GridRect()
            {
                minX = 0,
                minY = 0,
                maxX = 0,
                maxY = 0
            };
            this.m_notSet = true;
            this.m_nodePool = iNodePool;
        }

        public int width
        {
            get
            {
                if (this.m_notSet)
                {
                    this.SetBoundingBox();
                }
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
                if (this.m_notSet)
                {
                    this.SetBoundingBox();
                }
                return this.m_gridRect.maxY - this.m_gridRect.minY;
            }
            protected set
            {
            }
        }

        public override Node GetNodeAt(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return this.GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return this.IsWalkableAt(pos);
        }

        public bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            GridPos pos = new GridPos(iX, iY);
            this.m_nodePool.SetNode(pos, iWalkable);
            if (iWalkable)
            {
                if (iX < this.m_gridRect.minX || this.m_notSet)
                {
                    this.m_gridRect.minX = iX;
                }
                if (iX > this.m_gridRect.maxX || this.m_notSet)
                {
                    this.m_gridRect.maxX = iX;
                }
                if (iY < this.m_gridRect.minY || this.m_notSet)
                {
                    this.m_gridRect.minY = iY;
                }
                if (iY > this.m_gridRect.maxY || this.m_notSet)
                {
                    this.m_gridRect.maxY = iY;
                }
                this.m_notSet = false;
            }
            else
            {
                if (iX == this.m_gridRect.minX || iX == this.m_gridRect.maxX || iY == this.m_gridRect.minY
                    || iY == this.m_gridRect.maxY)
                {
                    this.m_notSet = true;
                }
            }
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            return this.m_nodePool.GetNode(iPos);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return this.m_nodePool.Nodes.ContainsKey(iPos);
        }

        public bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return this.SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            foreach (KeyValuePair<GridPos, Node> keyValue in this.m_nodePool.Nodes)
            {
                keyValue.Value.Reset();
            }
        }

        private void SetBoundingBox()
        {
            foreach (KeyValuePair<GridPos, Node> pair in this.m_nodePool.Nodes)
            {
                if (pair.Key.x < this.m_gridRect.minX || this.m_notSet)
                {
                    this.m_gridRect.minX = pair.Key.x;
                }
                if (pair.Key.x > this.m_gridRect.maxX || this.m_notSet)
                {
                    this.m_gridRect.maxX = pair.Key.x;
                }
                if (pair.Key.y < this.m_gridRect.minY || this.m_notSet)
                {
                    this.m_gridRect.minY = pair.Key.y;
                }
                if (pair.Key.y > this.m_gridRect.maxY || this.m_notSet)
                {
                    this.m_gridRect.maxY = pair.Key.y;
                }
                this.m_notSet = false;
            }
            this.m_notSet = false;
        }

        public override BaseGrid Clone()
        {
            var tNewGrid = new DynamicGridWPool(this.m_nodePool);
            return tNewGrid;
        }
    }
}