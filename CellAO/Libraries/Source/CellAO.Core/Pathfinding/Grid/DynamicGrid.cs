using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace CellAO.Core.Pathfinding.Grid
{
    public class DynamicGrid : BaseGrid
    {
        protected Dictionary<GridPos, Node> m_nodes;
        private bool m_notSet;


        public override int width
        {
            get
            {
                if (m_notSet)
                    setBoundingBox();
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
                    setBoundingBox();
                return m_gridRect.maxY - m_gridRect.minY;
            }
            protected set
            {

            }
        }

        public DynamicGrid(List<GridPos> iWalkableGridList = null)
            : base()
        {
            m_gridRect = new GridRect();
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = 0;
            m_gridRect.maxY = 0;
            m_notSet = true;
            buildNodes(iWalkableGridList);
        }

        protected void buildNodes(List<GridPos> iWalkableGridList)
        {

            m_nodes = new Dictionary<GridPos, Node>();
            if (iWalkableGridList == null)
                return;
            foreach (GridPos gridPos in iWalkableGridList)
            {
                SetWalkableAt(gridPos.x, gridPos.y, true);
            }
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

        private void setBoundingBox()
        {
            m_notSet = true;
            foreach (KeyValuePair<GridPos, Node> pair in m_nodes)
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
            var pos = new GridPos(iX, iY);

            if (iWalkable)
            {
                if (m_nodes.ContainsKey(pos))
                {
                    // this.m_nodes[pos].walkable = iWalkable;
                    return true;
                }
                else
                {
                    if (iX < m_gridRect.minX || m_notSet)
                        m_gridRect.minX = iX;
                    if (iX > m_gridRect.maxX || m_notSet)
                        m_gridRect.maxX = iX;
                    if (iY < m_gridRect.minY || m_notSet)
                        m_gridRect.minY = iY;
                    if (iY > m_gridRect.maxY || m_notSet)
                        m_gridRect.maxY = iY;
                    m_nodes.Add(new GridPos(pos.x, pos.y), new Node(pos.x, pos.y, iWalkable));
                    m_notSet = false;
                }
            }
            else
            {
                if (m_nodes.ContainsKey(pos))
                {
                    m_nodes.Remove(pos);
                    if (iX == m_gridRect.minX || iX == m_gridRect.maxX || iY == m_gridRect.minY || iY == m_gridRect.maxY)
                        m_notSet = true;
                }
            }
            return true;
        }

        public override Node GetNodeAt(GridPos iPos)
        {
            if (m_nodes.ContainsKey(iPos))
            {
                return m_nodes[iPos];
            }
            return null;
        }

        public override bool IsWalkableAt(GridPos iPos)
        {
            return m_nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(GridPos iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.x, iPos.y, iWalkable);
        }

        public override void Reset()
        {
            Reset(null);
        }

        public void Reset(List<GridPos> iWalkableGridList)
        {

            foreach (KeyValuePair<GridPos, Node> keyValue in m_nodes)
            {
                keyValue.Value.Reset();
            }

            if (iWalkableGridList == null)
                return;
            foreach (KeyValuePair<GridPos, Node> keyValue in m_nodes)
            {
                if (iWalkableGridList.Contains(keyValue.Key))
                    SetWalkableAt(keyValue.Key, true);
                else
                    SetWalkableAt(keyValue.Key, false);
            }
        }

        public override BaseGrid Clone()
        {
            var tNewGrid = new DynamicGrid(null);

            foreach (KeyValuePair<GridPos, Node> keyValue in m_nodes)
            {
                tNewGrid.SetWalkableAt(keyValue.Key.x, keyValue.Key.y, true);

            }

            return tNewGrid;
        }
    }
}
