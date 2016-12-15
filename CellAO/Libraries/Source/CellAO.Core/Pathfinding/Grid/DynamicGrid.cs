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

    public class DynamicGrid : BaseGrid, IBaseGrid
    {
        protected Dictionary<GridPos, Node> m_nodes;

        private bool m_notSet;

        public DynamicGrid(List<GridPos> iWalkableGridList = null)
            : base()
        {
            this.m_gridRect = new GridRect();
            this.m_gridRect.minX = 0;
            this.m_gridRect.minY = 0;
            this.m_gridRect.maxX = 0;
            this.m_gridRect.maxY = 0;
            this.m_notSet = true;
            this.buildNodes(iWalkableGridList);
        }

        public int width
        {
            get
            {
                if (this.m_notSet)
                {
                    this.setBoundingBox();
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
                    this.setBoundingBox();
                }
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
            var pos = new GridPos(iX, iY);

            if (iWalkable)
            {
                if (this.m_nodes.ContainsKey(pos))
                {
                    // this.m_nodes[pos].walkable = iWalkable;
                    return true;
                }
                else
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
                    this.m_nodes.Add(new GridPos(pos.x, pos.y), new Node(pos.x, pos.y, iWalkable));
                    this.m_notSet = false;
                }
            }
            else
            {
                if (this.m_nodes.ContainsKey(pos))
                {
                    this.m_nodes.Remove(pos);
                    if (iX == this.m_gridRect.minX || iX == this.m_gridRect.maxX || iY == this.m_gridRect.minY
                        || iY == this.m_gridRect.maxY)
                    {
                        this.m_notSet = true;
                    }
                }
            }
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            if (this.m_nodes.ContainsKey(iPos))
            {
                return this.m_nodes[iPos];
            }
            return null;
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return this.m_nodes.ContainsKey(iPos);
        }

        public bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return this.SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            this.Reset(null);
        }

        protected void buildNodes(List<GridPos> iWalkableGridList)
        {
            this.m_nodes = new Dictionary<GridPos, Node>();
            if (iWalkableGridList != null)
            {
                foreach (GridPos gridPos in iWalkableGridList)
                {
                    this.SetWalkableAt(gridPos.x, gridPos.y, true);
                }
            }
        }

        private void setBoundingBox()
        {
            this.m_notSet = true;
            foreach (KeyValuePair<GridPos, Node> pair in this.m_nodes)
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

        public void Reset(List<GridPos> iWalkableGridList)
        {
            foreach (KeyValuePair<GridPos, Node> keyValue in this.m_nodes)
            {
                keyValue.Value.Reset();
            }

            if (iWalkableGridList == null)
            {
                return;
            }
            foreach (KeyValuePair<GridPos, Node> keyValue in this.m_nodes)
            {
                if (iWalkableGridList.Contains(keyValue.Key))
                {
                    this.SetWalkableAt(keyValue.Key, true);
                }
                else
                {
                    this.SetWalkableAt(keyValue.Key, false);
                }
            }
        }

        public override BaseGrid Clone()
        {
            var tNewGrid = new DynamicGrid(null);

            foreach (KeyValuePair<GridPos, Node> keyValue in this.m_nodes)
            {
                tNewGrid.SetWalkableAt(keyValue.Key.x, keyValue.Key.y, true);
            }

            return tNewGrid;
        }
    }
}