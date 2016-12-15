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

    #endregion

    public class StaticGrid : BaseGrid, IBaseGrid
    {
        private Node[][] m_nodes;

        public StaticGrid(int iWidth, int iHeight, bool[][] iMatrix = null)
            : base()
        {
            this.width = iWidth;
            this.height = iHeight;
            this.m_gridRect.minX = 0;
            this.m_gridRect.minY = 0;
            this.m_gridRect.maxX = iWidth - 1;
            this.m_gridRect.maxY = iHeight - 1;
            this.m_nodes = this.buildNodes(iWidth, iHeight, iMatrix);
        }

        public int width { get; protected set; }

        public int height { get; protected set; }

        public override Node GetNodeAt(int iX, int iY)
        {
            return this.m_nodes[iX][iY];
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            return this.isInside(iX, iY) && this.m_nodes[iX][iY].walkable;
        }

        public bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            this.m_nodes[iX][iY].walkable = iWalkable;
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            return this.GetNodeAt(iPos.x, iPos.y);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return this.IsWalkableAt(iPos.x, iPos.y);
        }

        public bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return this.SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            this.Reset(null);
        }

        private Node[][] buildNodes(int iWidth, int iHeight, bool[][] iMatrix)
        {
            var tNodes = new Node[iWidth][];
            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                tNodes[widthTrav] = new Node[iHeight];
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    tNodes[widthTrav][heightTrav] = new Node(widthTrav, heightTrav, null);
                }
            }

            if (iMatrix == null)
            {
                return tNodes;
            }

            if (iMatrix.Length != iWidth || iMatrix[0].Length != iHeight)
            {
                throw new ApplicationException("Matrix size does not fit");
            }

            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    if (iMatrix[widthTrav][heightTrav])
                    {
                        tNodes[widthTrav][heightTrav].walkable = true;
                    }
                    else
                    {
                        tNodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
            return tNodes;
        }

        protected bool isInside(int iX, int iY)
        {
            return (iX >= 0 && iX < this.width) && (iY >= 0 && iY < this.height);
        }

        protected bool isInside(GridPos iPos)
        {
            return this.isInside(iPos.x, iPos.y);
        }

        public void Reset(bool[][] iMatrix)
        {
            for (int widthTrav = 0; widthTrav < this.width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < this.height; heightTrav++)
                {
                    this.m_nodes[widthTrav][heightTrav].Reset();
                }
            }

            if (iMatrix == null)
            {
                return;
            }
            if (iMatrix.Length != this.width || iMatrix[0].Length != this.height)
            {
                throw new ApplicationException("Matrix size does not fit");
            }

            for (int widthTrav = 0; widthTrav < this.width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < this.height; heightTrav++)
                {
                    if (iMatrix[widthTrav][heightTrav])
                    {
                        this.m_nodes[widthTrav][heightTrav].walkable = true;
                    }
                    else
                    {
                        this.m_nodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
        }

        public override BaseGrid Clone()
        {
            int tWidth = this.width;
            int tHeight = this.height;
            Node[][] tNodes = this.m_nodes;

            var tNewGrid = new StaticGrid(tWidth, tHeight, null);

            var tNewNodes = new Node[tWidth][];
            for (int widthTrav = 0; widthTrav < tWidth; widthTrav++)
            {
                tNewNodes[widthTrav] = new Node[tHeight];
                for (int heightTrav = 0; heightTrav < tHeight; heightTrav++)
                {
                    tNewNodes[widthTrav][heightTrav] = new Node(
                        widthTrav,
                        heightTrav,
                        tNodes[widthTrav][heightTrav].walkable);
                }
            }
            tNewGrid.m_nodes = tNewNodes;

            return tNewGrid;
        }
    }
}