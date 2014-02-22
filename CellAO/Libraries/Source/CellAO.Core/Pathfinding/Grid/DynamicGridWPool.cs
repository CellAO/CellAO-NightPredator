using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CellAO.Core.Pathfinding.Grid
{
    public class DynamicGridWPool : BaseGrid
    {
        private bool m_notSet;
        private NodePool m_nodePool;

        public override int width
        {
            get
            {
                if (m_notSet)
                    this.SetBoundingBox();
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
                if (m_notSet)
                    this.SetBoundingBox();
                return m_gridRect.maxY - m_gridRect.minY;
            }
            protected set
            {

            }
        }

        public DynamicGridWPool(NodePool iNodePool)
            : base()
        {
            m_gridRect = new GridRect();
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = 0;
            m_gridRect.maxY = 0;
            m_notSet = true;
            m_nodePool = iNodePool;
        }


        public override Node GetNodeAt(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            GridPos pos = new GridPos(iX, iY);
            return IsWalkableAt(pos);
        }

        private void SetBoundingBox()
        {
            foreach (KeyValuePair<GridPos, Node> pair in m_nodePool.Nodes)
            {
                if (pair.Key.x < m_gridRect.minX || m_notSet)
                    m_gridRect.minX = pair.Key.x;
                if (pair.Key.x > m_gridRect.maxX || m_notSet)
                    m_gridRect.maxX = pair.Key.x;
                if (pair.Key.y < m_gridRect.minY || m_notSet)
                    m_gridRect.minY = pair.Key.y;
                if (pair.Key.y > m_gridRect.maxY || m_notSet)
                    m_gridRect.maxY = pair.Key.y;
                m_notSet = false;
            }
            m_notSet = false;
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            GridPos pos = new GridPos(iX, iY);
            m_nodePool.SetNode(pos, iWalkable);
            if (iWalkable)
            {
                if (iX < m_gridRect.minX || m_notSet)
                    m_gridRect.minX = iX;
                if (iX > m_gridRect.maxX || m_notSet)
                    m_gridRect.maxX = iX;
                if (iY < m_gridRect.minY || m_notSet)
                    m_gridRect.minY = iY;
                if (iY > m_gridRect.maxY || m_notSet)
                    m_gridRect.maxY = iY;
                m_notSet = false;
            }
            else
            {
                if (iX == m_gridRect.minX || iX == m_gridRect.maxX || iY == m_gridRect.minY || iY == m_gridRect.maxY)
                    m_notSet = true;

            }
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            return m_nodePool.GetNode(iPos);
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return m_nodePool.Nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }


        public override void Reset()
        {
            foreach (KeyValuePair<GridPos, Node> keyValue in m_nodePool.Nodes)
            {
                keyValue.Value.Reset();
            }
        }

        public override BaseGrid Clone()
        {
            var tNewGrid = new DynamicGridWPool(m_nodePool);
            return tNewGrid;
        }
    }
}
