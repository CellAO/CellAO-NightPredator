using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Pathfinding.Grid
{
    public class StaticGrid : BaseGrid
    {
        public override int width { get; protected set; }

        public override int height { get; protected set; }

        private Node[][] m_nodes;

        public StaticGrid(int iWidth, int iHeight, bool[][] iMatrix = null)
            : base()
        {
            width = iWidth;
            height = iHeight;
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = iWidth - 1;
            m_gridRect.maxY = iHeight - 1;
            this.m_nodes = buildNodes(iWidth, iHeight, iMatrix);
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
                throw new System.ApplicationException("Matrix size does not fit");
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

        public override Node GetNodeAt(int iX, int iY)
        {
            return this.m_nodes[iX][iY];
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            return isInside(iX, iY) && this.m_nodes[iX][iY].walkable;
        }

        protected bool isInside(int iX, int iY)
        {
            return (iX >= 0 && iX < width) && (iY >= 0 && iY < height);
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            this.m_nodes[iX][iY].walkable = iWalkable;
            return true;
        }

        protected bool isInside(GridPos iPos)
        {
            return isInside(iPos.x, iPos.y);
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            return GetNodeAt(iPos.x, iPos.y);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return IsWalkableAt(iPos.x, iPos.y);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            Reset(null);
        }

        public void Reset(bool[][] iMatrix)
        {
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    m_nodes[widthTrav][heightTrav].Reset();
                }
            }

            if (iMatrix == null)
            {
                return;
            }
            if (iMatrix.Length != width || iMatrix[0].Length != height)
            {
                throw new ApplicationException("Matrix size does not fit");
            }

            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    if (iMatrix[widthTrav][heightTrav])
                    {
                        m_nodes[widthTrav][heightTrav].walkable = true;
                    }
                    else
                    {
                        m_nodes[widthTrav][heightTrav].walkable = false;
                    }
                }
            }
        }

        public override BaseGrid Clone()
        {
            var tWidth = width;
            var tHeight = height;
            var tNodes = this.m_nodes;

            var tNewGrid = new StaticGrid(tWidth, tHeight, null);

            var tNewNodes = new Node[tWidth][];
            for (var widthTrav = 0; widthTrav < tWidth; widthTrav++)
            {
                tNewNodes[widthTrav] = new Node[tHeight];
                for (var heightTrav = 0; heightTrav < tHeight; heightTrav++)
                {
                    tNewNodes[widthTrav][heightTrav] = new Node(widthTrav, heightTrav, tNodes[widthTrav][heightTrav].walkable);
                }
            }
            tNewGrid.m_nodes = tNewNodes;

            return tNewGrid;
        }
    }
}
