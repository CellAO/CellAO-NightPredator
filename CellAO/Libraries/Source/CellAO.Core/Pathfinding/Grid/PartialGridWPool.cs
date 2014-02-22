using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace CellAO.Core.Pathfinding.Grid
{
    public class PartialGridWPool : BaseGrid
    {
        private NodePool m_nodePool;

        public override int width
        {
            get
            {
                return m_gridRect.maxX - m_gridRect.minX;
            }
            protected set
            {

            }
        }

        public override int height
        {
            get
            {
                return m_gridRect.maxY - m_gridRect.minY;
            }
            protected set
            {

            }
        }


        public PartialGridWPool(NodePool iNodePool, GridRect? iGridRect = null)
            : base()
        {
            if (iGridRect == null)
                m_gridRect = new GridRect();
            else
                m_gridRect = iGridRect.Value;
            m_nodePool = iNodePool;
        }


        public void SetGridRect(GridRect iGridRect)
        {
            m_gridRect = iGridRect;
        }


        public bool IsInside(int iX, int iY)
        {
            if (iX < m_gridRect.minX || iX > m_gridRect.maxX || iY < m_gridRect.minY || iY > m_gridRect.maxY)
                return false;
            return true;
        }

        public override Node GetNodeAt(int iX, int iY)
        {
            var pos = new GridPos(iX, iY);
            return GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            var pos = new GridPos(iX, iY);
            return IsWalkableAt(pos);
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            if (!IsInside(iX, iY))
                return false;
            var pos = new GridPos(iX, iY);
            m_nodePool.SetNode(pos, iWalkable);
            return true;
        }

        public bool IsInside(GridPos iPos)
        {
            return IsInside(iPos.x, iPos.y);
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            if (!IsInside(iPos))
                return null;
            return m_nodePool.GetNode(iPos);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            if (!IsInside(iPos))
                return false;
            return m_nodePool.Nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            var rectCount = (m_gridRect.maxX - m_gridRect.minX) * (m_gridRect.maxY - m_gridRect.minY);
            if (m_nodePool.Nodes.Count > rectCount)
            {
                var travPos = new GridPos(0, 0);
                for (var xTrav = m_gridRect.minX; xTrav <= m_gridRect.maxX; xTrav++)
                {
                    travPos.x = xTrav;
                    for (var yTrav = m_gridRect.minY; yTrav <= m_gridRect.maxY; yTrav++)
                    {
                        travPos.y = yTrav;
                        var curNode = m_nodePool.GetNode(travPos);
                        if (curNode != null)
                            curNode.Reset();
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<GridPos, Node> keyValue in m_nodePool.Nodes)
                {
                    keyValue.Value.Reset();
                }
            }
        }


        public override BaseGrid Clone()
        {
            var tNewGrid = new PartialGridWPool(m_nodePool, m_gridRect);
            return tNewGrid;
        }
    }
}
