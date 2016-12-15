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
    using System.Collections.Generic;

    using CellAO.Core.Pathfinding.Grid;

    #endregion

    public delegate float HeuristicDelegate(int iDx, int iDy);

    public class JumpPointParam
    {
        protected HeuristicDelegate m_heuristic;

        protected bool m_crossAdjacentPoint;

        protected bool m_crossCorner;

        protected bool m_allowEndNodeUnWalkable;

        protected bool m_useRecursive;

        protected IBaseGrid m_searchGrid;

        protected Node m_startNode;

        protected Node m_endNode;

        public List<Node> openList;

        public JumpPointParam(
            IBaseGrid iGrid,
            GridPos iStartPos,
            GridPos iEndPos,
            bool iAllowEndNodeUnWalkable = true,
            bool iCrossCorner = true,
            bool iCrossAdjacentPoint = true,
            HeuristicMode iMode = HeuristicMode.EUCLIDEAN)
        {
            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
            }
            this.m_allowEndNodeUnWalkable = iAllowEndNodeUnWalkable;
            this.m_crossAdjacentPoint = iCrossAdjacentPoint;
            this.m_crossCorner = iCrossCorner;
            this.openList = new List<Node>();

            this.m_searchGrid = iGrid;
            this.m_startNode = this.m_searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
            this.m_endNode = this.m_searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);
            if (this.m_startNode == null)
            {
                this.m_startNode = new Node(iStartPos.x, iStartPos.y, true);
            }
            if (this.m_endNode == null)
            {
                this.m_endNode = new Node(iEndPos.x, iEndPos.y, true);
            }
            this.m_useRecursive = false;
        }

        public JumpPointParam(
            IBaseGrid iGrid,
            bool iAllowEndNodeUnWalkable = true,
            bool iCrossCorner = true,
            bool iCrossAdjacentPoint = true,
            HeuristicMode iMode = HeuristicMode.EUCLIDEAN)
        {
            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
            }
            this.m_allowEndNodeUnWalkable = iAllowEndNodeUnWalkable;
            this.m_crossAdjacentPoint = iCrossAdjacentPoint;
            this.m_crossCorner = iCrossCorner;

            this.openList = new List<Node>();

            this.m_searchGrid = iGrid;
            this.m_startNode = null;
            this.m_endNode = null;
            this.m_useRecursive = false;
        }

        public bool CrossAdjacentPoint
        {
            get
            {
                return this.m_crossCorner && this.m_crossAdjacentPoint;
            }
            set
            {
                this.m_crossAdjacentPoint = value;
            }
        }

        public bool CrossCorner
        {
            get
            {
                return this.m_crossCorner;
            }
            set
            {
                this.m_crossCorner = value;
            }
        }

        public bool AllowEndNodeUnWalkable
        {
            get
            {
                return this.m_allowEndNodeUnWalkable;
            }
            set
            {
                this.m_allowEndNodeUnWalkable = value;
            }
        }

        public HeuristicDelegate HeuristicFunc
        {
            get
            {
                return this.m_heuristic;
            }
        }

        public IBaseGrid SearchGrid
        {
            get
            {
                return this.m_searchGrid;
            }
        }

        public Node StartNode
        {
            get
            {
                return this.m_startNode;
            }
        }

        public Node EndNode
        {
            get
            {
                return this.m_endNode;
            }
        }

        public bool UseRecursive
        {
            get
            {
                return this.m_useRecursive;
            }
            set
            {
                this.m_useRecursive = value;
            }
        }

        public void SetHeuristic(HeuristicMode iMode)
        {
            this.m_heuristic = null;
            switch (iMode)
            {
                case HeuristicMode.MANHATTAN:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Manhattan);
                    break;
                case HeuristicMode.EUCLIDEAN:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
                case HeuristicMode.CHEBYSHEV:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Chebyshev);
                    break;
                default:
                    this.m_heuristic = new HeuristicDelegate(Heuristic.Euclidean);
                    break;
            }
        }

        public void Reset(GridPos iStartPos, GridPos iEndPos, IBaseGrid iSearchGrid = null)
        {
            this.openList.Clear();
            this.m_startNode = null;
            this.m_endNode = null;

            if (iSearchGrid != null)
            {
                this.m_searchGrid = iSearchGrid;
            }
            this.m_searchGrid.Reset();
            this.m_startNode = this.m_searchGrid.GetNodeAt(iStartPos.x, iStartPos.y);
            this.m_endNode = this.m_searchGrid.GetNodeAt(iEndPos.x, iEndPos.y);
            if (this.m_startNode == null)
            {
                this.m_startNode = new Node(iStartPos.x, iStartPos.y, true);
            }
            if (this.m_endNode == null)
            {
                this.m_endNode = new Node(iEndPos.x, iEndPos.y, true);
            }
        }
    }

    internal class JumpPointFinder
    {
        public static List<GridPos> FindPath(JumpPointParam iParam)
        {
            List<Node> tOpenList = iParam.openList;
            Node tStartNode = iParam.StartNode;
            Node tEndNode = iParam.EndNode;
            Node tNode;
            bool revertEndNodeWalkable = false;

            // set the `g` and `f` value of the start node to be 0
            tStartNode.startToCurNodeLen = 0;
            tStartNode.heuristicStartToEndLen = 0;

            // push the start node into the open list
            tOpenList.Add(tStartNode);
            tStartNode.isOpened = true;

            if (iParam.AllowEndNodeUnWalkable && !iParam.SearchGrid.IsWalkableAt(tEndNode.x, tEndNode.y))
            {
                iParam.SearchGrid.SetWalkableAt(tEndNode.x, tEndNode.y, true);
                revertEndNodeWalkable = true;
            }

            // while the open list is not empty
            while (tOpenList.Count > 0)
            {
                // pop the position of node which has the minimum `f` value.
                tOpenList.Sort();
                tNode = (Node)tOpenList[0];
                tOpenList.RemoveAt(0);
                tNode.isClosed = true;

                if (tNode.Equals(tEndNode))
                {
                    if (revertEndNodeWalkable)
                    {
                        iParam.SearchGrid.SetWalkableAt(tEndNode.x, tEndNode.y, false);
                    }
                    return Node.Backtrace(tNode); // rebuilding path
                }

                identifySuccessors(iParam, tNode);
            }

            if (revertEndNodeWalkable)
            {
                iParam.SearchGrid.SetWalkableAt(tEndNode.x, tEndNode.y, false);
            }

            // fail to find the path
            return new List<GridPos>();
        }

        private static void identifySuccessors(JumpPointParam iParam, Node iNode)
        {
            HeuristicDelegate tHeuristic = iParam.HeuristicFunc;
            List<Node> tOpenList = iParam.openList;
            int tEndX = iParam.EndNode.x;
            int tEndY = iParam.EndNode.y;
            GridPos tNeighbor;
            GridPos? tJumpPoint;
            Node tJumpNode;

            List<GridPos> tNeighbors = findNeighbors(iParam, iNode);
            for (int i = 0; i < tNeighbors.Count; i++)
            {
                tNeighbor = tNeighbors[i];
                if (iParam.UseRecursive)
                {
                    tJumpPoint = jump(iParam, tNeighbor.x, tNeighbor.y, iNode.x, iNode.y);
                }
                else
                {
                    tJumpPoint = jumpLoop(iParam, tNeighbor.x, tNeighbor.y, iNode.x, iNode.y);
                }
                if (tJumpPoint != null)
                {
                    tJumpNode = iParam.SearchGrid.GetNodeAt(tJumpPoint.Value.x, tJumpPoint.Value.y);
                    if (tJumpNode == null)
                    {
                        if (iParam.EndNode.x == tJumpPoint.Value.x && iParam.EndNode.y == tJumpPoint.Value.y)
                        {
                            tJumpNode = iParam.SearchGrid.GetNodeAt(tJumpPoint.Value);
                        }
                    }
                    if (tJumpNode.isClosed)
                    {
                        continue;
                    }
                    // include distance, as parent may not be immediately adjacent:
                    float tCurNodeToJumpNodeLen = tHeuristic(
                        Math.Abs(tJumpPoint.Value.x - iNode.x),
                        Math.Abs(tJumpPoint.Value.y - iNode.y));
                    float tStartToJumpNodeLen = iNode.startToCurNodeLen + tCurNodeToJumpNodeLen;
                        // next `startToCurNodeLen` value

                    if (!tJumpNode.isOpened || tStartToJumpNodeLen < tJumpNode.startToCurNodeLen)
                    {
                        tJumpNode.startToCurNodeLen = tStartToJumpNodeLen;
                        tJumpNode.heuristicCurNodeToEndLen = (tJumpNode.heuristicCurNodeToEndLen == null
                            ? tHeuristic(Math.Abs(tJumpPoint.Value.x - tEndX), Math.Abs(tJumpPoint.Value.y - tEndY))
                            : tJumpNode.heuristicCurNodeToEndLen);
                        tJumpNode.heuristicStartToEndLen = tJumpNode.startToCurNodeLen
                                                           + tJumpNode.heuristicCurNodeToEndLen.Value;
                        tJumpNode.parent = iNode;

                        if (!tJumpNode.isOpened)
                        {
                            tOpenList.Add(tJumpNode);
                            tJumpNode.isOpened = true;
                        }
                    }
                }
            }
        }

        private static GridPos? jumpLoop(JumpPointParam iParam, int iX, int iY, int iPx, int iPy)
        {
            GridPos? retVal = null;
            Stack<JumpSnapshot> stack = new Stack<JumpSnapshot>();

            JumpSnapshot currentSnapshot = new JumpSnapshot();
            JumpSnapshot newSnapshot = null;
            currentSnapshot.iX = iX;
            currentSnapshot.iY = iY;
            currentSnapshot.iPx = iPx;
            currentSnapshot.iPy = iPy;
            currentSnapshot.stage = 0;

            stack.Push(currentSnapshot);
            while (stack.Count != 0)
            {
                currentSnapshot = stack.Pop();
                switch (currentSnapshot.stage)
                {
                    case 0:
                        if (!iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX, currentSnapshot.iY))
                        {
                            retVal = null;
                            continue;
                        }
                        else if (
                            iParam.SearchGrid.GetNodeAt(currentSnapshot.iX, currentSnapshot.iY)
                                .Equals(iParam.EndNode))
                        {
                            retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                            continue;
                        }

                        currentSnapshot.tDx = currentSnapshot.iX - currentSnapshot.iPx;
                        currentSnapshot.tDy = currentSnapshot.iY - currentSnapshot.iPy;
                        currentSnapshot.jx = null;
                        currentSnapshot.jy = null;
                        if (iParam.CrossCorner)
                        {
                            // check for forced neighbors
                            // along the diagonal
                            if (currentSnapshot.tDx != 0 && currentSnapshot.tDy != 0)
                            {
                                if ((iParam.SearchGrid.IsWalkableAt(
                                    currentSnapshot.iX - currentSnapshot.tDx,
                                    currentSnapshot.iY + currentSnapshot.tDy)
                                     && !iParam.SearchGrid.IsWalkableAt(
                                         currentSnapshot.iX - currentSnapshot.tDx,
                                         currentSnapshot.iY))
                                    || (iParam.SearchGrid.IsWalkableAt(
                                        currentSnapshot.iX + currentSnapshot.tDx,
                                        currentSnapshot.iY - currentSnapshot.tDy)
                                        && !iParam.SearchGrid.IsWalkableAt(
                                            currentSnapshot.iX,
                                            currentSnapshot.iY - currentSnapshot.tDy)))
                                {
                                    retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                                    continue;
                                }
                            }
                                // horizontally/vertically
                            else
                            {
                                if (currentSnapshot.tDx != 0)
                                {
                                    // moving along x
                                    if ((iParam.SearchGrid.IsWalkableAt(
                                        currentSnapshot.iX + currentSnapshot.tDx,
                                        currentSnapshot.iY + 1)
                                         && !iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX, currentSnapshot.iY + 1))
                                        || (iParam.SearchGrid.IsWalkableAt(
                                            currentSnapshot.iX + currentSnapshot.tDx,
                                            currentSnapshot.iY - 1)
                                            && !iParam.SearchGrid.IsWalkableAt(
                                                currentSnapshot.iX,
                                                currentSnapshot.iY - 1)))
                                    {
                                        retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if ((iParam.SearchGrid.IsWalkableAt(
                                        currentSnapshot.iX + 1,
                                        currentSnapshot.iY + currentSnapshot.tDy)
                                         && !iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX + 1, currentSnapshot.iY))
                                        || (iParam.SearchGrid.IsWalkableAt(
                                            currentSnapshot.iX - 1,
                                            currentSnapshot.iY + currentSnapshot.tDy)
                                            && !iParam.SearchGrid.IsWalkableAt(
                                                currentSnapshot.iX - 1,
                                                currentSnapshot.iY)))
                                    {
                                        retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                                        continue;
                                    }
                                }
                            }
                            // when moving diagonally, must check for vertical/horizontal jump points
                            if (currentSnapshot.tDx != 0 && currentSnapshot.tDy != 0)
                            {
                                currentSnapshot.stage = 1;
                                stack.Push(currentSnapshot);

                                newSnapshot = new JumpSnapshot();
                                newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                                newSnapshot.iY = currentSnapshot.iY;
                                newSnapshot.iPx = currentSnapshot.iX;
                                newSnapshot.iPy = currentSnapshot.iY;
                                newSnapshot.stage = 0;
                                stack.Push(newSnapshot);
                                continue;
                            }

                            // moving diagonally, must make sure one of the vertical/horizontal
                            // neighbors is open to allow the path

                            // moving diagonally, must make sure one of the vertical/horizontal
                            // neighbors is open to allow the path
                            if (iParam.SearchGrid.IsWalkableAt(
                                currentSnapshot.iX + currentSnapshot.tDx,
                                currentSnapshot.iY)
                                || iParam.SearchGrid.IsWalkableAt(
                                    currentSnapshot.iX,
                                    currentSnapshot.iY + currentSnapshot.tDy))
                            {
                                newSnapshot = new JumpSnapshot();
                                newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                                newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                                newSnapshot.iPx = currentSnapshot.iX;
                                newSnapshot.iPy = currentSnapshot.iY;
                                newSnapshot.stage = 0;
                                stack.Push(newSnapshot);
                                continue;
                            }
                            else if (iParam.CrossAdjacentPoint)
                            {
                                newSnapshot = new JumpSnapshot();
                                newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                                newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                                newSnapshot.iPx = currentSnapshot.iX;
                                newSnapshot.iPy = currentSnapshot.iY;
                                newSnapshot.stage = 0;
                                stack.Push(newSnapshot);
                                continue;
                            }
                        }
                        else //if (!iParam.CrossCorner)
                        {
                            // check for forced neighbors
                            // along the diagonal
                            if (currentSnapshot.tDx != 0 && currentSnapshot.tDy != 0)
                            {
                                if ((iParam.SearchGrid.IsWalkableAt(
                                    currentSnapshot.iX + currentSnapshot.tDx,
                                    currentSnapshot.iY + currentSnapshot.tDy)
                                     && iParam.SearchGrid.IsWalkableAt(
                                         currentSnapshot.iX,
                                         currentSnapshot.iY + currentSnapshot.tDy)
                                     && !iParam.SearchGrid.IsWalkableAt(
                                         currentSnapshot.iX + currentSnapshot.tDx,
                                         currentSnapshot.iY))
                                    || (iParam.SearchGrid.IsWalkableAt(
                                        currentSnapshot.iX + currentSnapshot.tDx,
                                        currentSnapshot.iY + currentSnapshot.tDy)
                                        && iParam.SearchGrid.IsWalkableAt(
                                            currentSnapshot.iX + currentSnapshot.tDx,
                                            currentSnapshot.iY)
                                        && !iParam.SearchGrid.IsWalkableAt(
                                            currentSnapshot.iX,
                                            currentSnapshot.iY + currentSnapshot.tDy)))
                                {
                                    retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                                    continue;
                                }
                            }
                                // horizontally/vertically
                            else
                            {
                                if (currentSnapshot.tDx != 0)
                                {
                                    // moving along x
                                    if ((iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX, currentSnapshot.iY + 1)
                                         && !iParam.SearchGrid.IsWalkableAt(
                                             currentSnapshot.iX - currentSnapshot.tDx,
                                             currentSnapshot.iY + 1))
                                        || (iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX, currentSnapshot.iY - 1)
                                            && !iParam.SearchGrid.IsWalkableAt(
                                                currentSnapshot.iX - currentSnapshot.tDx,
                                                currentSnapshot.iY - 1)))
                                    {
                                        retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if ((iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX + 1, currentSnapshot.iY)
                                         && !iParam.SearchGrid.IsWalkableAt(
                                             currentSnapshot.iX + 1,
                                             currentSnapshot.iY - currentSnapshot.tDy))
                                        || (iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX - 1, currentSnapshot.iY)
                                            && !iParam.SearchGrid.IsWalkableAt(
                                                currentSnapshot.iX - 1,
                                                currentSnapshot.iY - currentSnapshot.tDy)))
                                    {
                                        retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                                        continue;
                                    }
                                }
                            }

                            // when moving diagonally, must check for vertical/horizontal jump points
                            if (currentSnapshot.tDx != 0 && currentSnapshot.tDy != 0)
                            {
                                currentSnapshot.stage = 3;
                                stack.Push(currentSnapshot);

                                newSnapshot = new JumpSnapshot();
                                newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                                newSnapshot.iY = currentSnapshot.iY;
                                newSnapshot.iPx = currentSnapshot.iX;
                                newSnapshot.iPy = currentSnapshot.iY;
                                newSnapshot.stage = 0;
                                stack.Push(newSnapshot);
                                continue;
                            }

                            // moving diagonally, must make sure both of the vertical/horizontal
                            // neighbors is open to allow the path
                            if (iParam.SearchGrid.IsWalkableAt(
                                currentSnapshot.iX + currentSnapshot.tDx,
                                currentSnapshot.iY)
                                && iParam.SearchGrid.IsWalkableAt(
                                    currentSnapshot.iX,
                                    currentSnapshot.iY + currentSnapshot.tDy))
                            {
                                newSnapshot = new JumpSnapshot();
                                newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                                newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                                newSnapshot.iPx = currentSnapshot.iX;
                                newSnapshot.iPy = currentSnapshot.iY;
                                newSnapshot.stage = 0;
                                stack.Push(newSnapshot);
                                continue;
                            }
                        }
                        retVal = null;
                        break;
                    case 1:
                        currentSnapshot.jx = retVal;

                        currentSnapshot.stage = 2;
                        stack.Push(currentSnapshot);

                        newSnapshot = new JumpSnapshot();
                        newSnapshot.iX = currentSnapshot.iX;
                        newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                        newSnapshot.iPx = currentSnapshot.iX;
                        newSnapshot.iPy = currentSnapshot.iY;
                        newSnapshot.stage = 0;
                        stack.Push(newSnapshot);
                        break;
                    case 2:
                        currentSnapshot.jy = retVal;
                        if (currentSnapshot.jx != null || currentSnapshot.jy != null)
                        {
                            retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                            continue;
                        }

                        // moving diagonally, must make sure one of the vertical/horizontal
                        // neighbors is open to allow the path
                        if (iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX + currentSnapshot.tDx, currentSnapshot.iY)
                            || iParam.SearchGrid.IsWalkableAt(
                                currentSnapshot.iX,
                                currentSnapshot.iY + currentSnapshot.tDy))
                        {
                            newSnapshot = new JumpSnapshot();
                            newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                            newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                            newSnapshot.iPx = currentSnapshot.iX;
                            newSnapshot.iPy = currentSnapshot.iY;
                            newSnapshot.stage = 0;
                            stack.Push(newSnapshot);
                            continue;
                        }
                        else if (iParam.CrossAdjacentPoint)
                        {
                            newSnapshot = new JumpSnapshot();
                            newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                            newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                            newSnapshot.iPx = currentSnapshot.iX;
                            newSnapshot.iPy = currentSnapshot.iY;
                            newSnapshot.stage = 0;
                            stack.Push(newSnapshot);
                            continue;
                        }
                        retVal = null;
                        break;
                    case 3:
                        currentSnapshot.jx = retVal;

                        currentSnapshot.stage = 4;
                        stack.Push(currentSnapshot);

                        newSnapshot = new JumpSnapshot();
                        newSnapshot.iX = currentSnapshot.iX;
                        newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                        newSnapshot.iPx = currentSnapshot.iX;
                        newSnapshot.iPy = currentSnapshot.iY;
                        newSnapshot.stage = 0;
                        stack.Push(newSnapshot);
                        break;
                    case 4:
                        currentSnapshot.jy = retVal;
                        if (currentSnapshot.jx != null || currentSnapshot.jy != null)
                        {
                            retVal = new GridPos(currentSnapshot.iX, currentSnapshot.iY);
                            continue;
                        }

                        // moving diagonally, must make sure both of the vertical/horizontal
                        // neighbors is open to allow the path
                        if (iParam.SearchGrid.IsWalkableAt(currentSnapshot.iX + currentSnapshot.tDx, currentSnapshot.iY)
                            && iParam.SearchGrid.IsWalkableAt(
                                currentSnapshot.iX,
                                currentSnapshot.iY + currentSnapshot.tDy))
                        {
                            newSnapshot = new JumpSnapshot();
                            newSnapshot.iX = currentSnapshot.iX + currentSnapshot.tDx;
                            newSnapshot.iY = currentSnapshot.iY + currentSnapshot.tDy;
                            newSnapshot.iPx = currentSnapshot.iX;
                            newSnapshot.iPy = currentSnapshot.iY;
                            newSnapshot.stage = 0;
                            stack.Push(newSnapshot);
                            continue;
                        }
                        retVal = null;
                        break;
                }
            }

            return retVal;
        }

        private static GridPos? jump(JumpPointParam iParam, int iX, int iY, int iPx, int iPy)
        {
            if (!iParam.SearchGrid.IsWalkableAt(iX, iY))
            {
                return null;
            }
            else if (iParam.SearchGrid.GetNodeAt(iX, iY).Equals(iParam.EndNode))
            {
                return new GridPos(iX, iY);
            }

            int tDx = iX - iPx;
            int tDy = iY - iPy;
            GridPos? jx = null;
            GridPos? jy = null;
            if (iParam.CrossCorner)
            {
                // check for forced neighbors
                // along the diagonal
                if (tDx != 0 && tDy != 0)
                {
                    if ((iParam.SearchGrid.IsWalkableAt(iX - tDx, iY + tDy)
                         && !iParam.SearchGrid.IsWalkableAt(iX - tDx, iY))
                        || (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY - tDy)
                            && !iParam.SearchGrid.IsWalkableAt(iX, iY - tDy)))
                    {
                        return new GridPos(iX, iY);
                    }
                }
                    // horizontally/vertically
                else
                {
                    if (tDx != 0)
                    {
                        // moving along x
                        if ((iParam.SearchGrid.IsWalkableAt(iX + tDx, iY + 1)
                             && !iParam.SearchGrid.IsWalkableAt(iX, iY + 1))
                            || (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY - 1)
                                && !iParam.SearchGrid.IsWalkableAt(iX, iY - 1)))
                        {
                            return new GridPos(iX, iY);
                        }
                    }
                    else
                    {
                        if ((iParam.SearchGrid.IsWalkableAt(iX + 1, iY + tDy)
                             && !iParam.SearchGrid.IsWalkableAt(iX + 1, iY))
                            || (iParam.SearchGrid.IsWalkableAt(iX - 1, iY + tDy)
                                && !iParam.SearchGrid.IsWalkableAt(iX - 1, iY)))
                        {
                            return new GridPos(iX, iY);
                        }
                    }
                }
                // when moving diagonally, must check for vertical/horizontal jump points
                if (tDx != 0 && tDy != 0)
                {
                    jx = jump(iParam, iX + tDx, iY, iX, iY);
                    jy = jump(iParam, iX, iY + tDy, iX, iY);
                    if (jx != null || jy != null)
                    {
                        return new GridPos(iX, iY);
                    }
                }

                // moving diagonally, must make sure one of the vertical/horizontal
                // neighbors is open to allow the path
                if (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY) || iParam.SearchGrid.IsWalkableAt(iX, iY + tDy))
                {
                    return jump(iParam, iX + tDx, iY + tDy, iX, iY);
                }
                else if (iParam.CrossAdjacentPoint)
                {
                    return jump(iParam, iX + tDx, iY + tDy, iX, iY);
                }
                else
                {
                    return null;
                }
            }
            else //if (!iParam.CrossCorner)
            {
                // check for forced neighbors
                // along the diagonal
                if (tDx != 0 && tDy != 0)
                {
                    if ((iParam.SearchGrid.IsWalkableAt(iX + tDx, iY + tDy)
                         && iParam.SearchGrid.IsWalkableAt(iX, iY + tDy)
                         && !iParam.SearchGrid.IsWalkableAt(iX + tDx, iY))
                        || (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY + tDy)
                            && iParam.SearchGrid.IsWalkableAt(iX + tDx, iY)
                            && !iParam.SearchGrid.IsWalkableAt(iX, iY + tDy)))
                    {
                        return new GridPos(iX, iY);
                    }
                }
                    // horizontally/vertically
                else
                {
                    if (tDx != 0)
                    {
                        // moving along x
                        if ((iParam.SearchGrid.IsWalkableAt(iX, iY + 1)
                             && !iParam.SearchGrid.IsWalkableAt(iX - tDx, iY + 1))
                            || (iParam.SearchGrid.IsWalkableAt(iX, iY - 1)
                                && !iParam.SearchGrid.IsWalkableAt(iX - tDx, iY - 1)))
                        {
                            return new GridPos(iX, iY);
                        }
                    }
                    else
                    {
                        if ((iParam.SearchGrid.IsWalkableAt(iX + 1, iY)
                             && !iParam.SearchGrid.IsWalkableAt(iX + 1, iY - tDy))
                            || (iParam.SearchGrid.IsWalkableAt(iX - 1, iY)
                                && !iParam.SearchGrid.IsWalkableAt(iX - 1, iY - tDy)))
                        {
                            return new GridPos(iX, iY);
                        }
                    }
                }

                // when moving diagonally, must check for vertical/horizontal jump points
                if (tDx != 0 && tDy != 0)
                {
                    jx = jump(iParam, iX + tDx, iY, iX, iY);
                    jy = jump(iParam, iX, iY + tDy, iX, iY);
                    if (jx != null || jy != null)
                    {
                        return new GridPos(iX, iY);
                    }
                }

                // moving diagonally, must make sure both of the vertical/horizontal
                // neighbors is open to allow the path
                if (iParam.SearchGrid.IsWalkableAt(iX + tDx, iY) && iParam.SearchGrid.IsWalkableAt(iX, iY + tDy))
                {
                    return jump(iParam, iX + tDx, iY + tDy, iX, iY);
                }
                else
                {
                    return null;
                }
            }
        }

        private static List<GridPos> findNeighbors(JumpPointParam iParam, Node iNode)
        {
            Node tParent = (Node)iNode.parent;
            int tX = iNode.x;
            int tY = iNode.y;
            int tPx, tPy, tDx, tDy;
            List<GridPos> tNeighbors = new List<GridPos>();
            List<Node> tNeighborNodes;
            Node tNeighborNode;

            // directed pruning: can ignore most neighbors, unless forced.
            if (tParent != null)
            {
                tPx = tParent.x;
                tPy = tParent.y;
                // get the normalized direction of travel
                tDx = (tX - tPx) / Math.Max(Math.Abs(tX - tPx), 1);
                tDy = (tY - tPy) / Math.Max(Math.Abs(tY - tPy), 1);

                if (iParam.CrossCorner)
                {
                    // search diagonally
                    if (tDx != 0 && tDy != 0)
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                        {
                            tNeighbors.Add(new GridPos(tX, tY + tDy));
                        }
                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY));
                        }

                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + tDy))
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy)
                                || iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                            }
                            else if (iParam.CrossAdjacentPoint)
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                            }
                        }

                        if (iParam.SearchGrid.IsWalkableAt(tX - tDx, tY + tDy))
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy)
                                && !iParam.SearchGrid.IsWalkableAt(tX - tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX - tDx, tY + tDy));
                            }
                        }

                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - tDy))
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY)
                                && !iParam.SearchGrid.IsWalkableAt(tX, tY - tDy))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY - tDy));
                            }
                        }
                    }
                        // search horizontally/vertically
                    else
                    {
                        if (tDx == 0)
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX, tY + tDy));

                                if (iParam.SearchGrid.IsWalkableAt(tX + 1, tY + tDy)
                                    && !iParam.SearchGrid.IsWalkableAt(tX + 1, tY))
                                {
                                    tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                                }
                                if (iParam.SearchGrid.IsWalkableAt(tX - 1, tY + tDy)
                                    && !iParam.SearchGrid.IsWalkableAt(tX - 1, tY))
                                {
                                    tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                                }
                            }
                            else if (iParam.CrossAdjacentPoint)
                            {
                                if (iParam.SearchGrid.IsWalkableAt(tX + 1, tY + tDy)
                                    && !iParam.SearchGrid.IsWalkableAt(tX + 1, tY))
                                {
                                    tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                                }
                                if (iParam.SearchGrid.IsWalkableAt(tX - 1, tY + tDy)
                                    && !iParam.SearchGrid.IsWalkableAt(tX - 1, tY))
                                {
                                    tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                                }
                            }
                        }
                        else
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY));

                                if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + 1)
                                    && !iParam.SearchGrid.IsWalkableAt(tX, tY + 1))
                                {
                                    tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                                }
                                if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - 1)
                                    && !iParam.SearchGrid.IsWalkableAt(tX, tY - 1))
                                {
                                    tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                                }
                            }
                            else if (iParam.CrossAdjacentPoint)
                            {
                                if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + 1)
                                    && !iParam.SearchGrid.IsWalkableAt(tX, tY + 1))
                                {
                                    tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                                }
                                if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - 1)
                                    && !iParam.SearchGrid.IsWalkableAt(tX, tY - 1))
                                {
                                    tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                                }
                            }
                        }
                    }
                }
                else // if(!iParam.CrossCorner)
                {
                    // search diagonally
                    if (tDx != 0 && tDy != 0)
                    {
                        if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                        {
                            tNeighbors.Add(new GridPos(tX, tY + tDy));
                        }
                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                        {
                            tNeighbors.Add(new GridPos(tX + tDx, tY));
                        }

                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + tDy))
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy)
                                && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY + tDy));
                            }
                        }

                        if (iParam.SearchGrid.IsWalkableAt(tX - tDx, tY + tDy))
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy)
                                && iParam.SearchGrid.IsWalkableAt(tX - tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX - tDx, tY + tDy));
                            }
                        }

                        if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - tDy))
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY - tDy)
                                && iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY - tDy));
                            }
                        }
                    }
                        // search horizontally/vertically
                    else
                    {
                        if (tDx == 0)
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + tDy))
                            {
                                tNeighbors.Add(new GridPos(tX, tY + tDy));

                                if (iParam.SearchGrid.IsWalkableAt(tX + 1, tY + tDy)
                                    && iParam.SearchGrid.IsWalkableAt(tX + 1, tY))
                                {
                                    tNeighbors.Add(new GridPos(tX + 1, tY + tDy));
                                }
                                if (iParam.SearchGrid.IsWalkableAt(tX - 1, tY + tDy)
                                    && iParam.SearchGrid.IsWalkableAt(tX - 1, tY))
                                {
                                    tNeighbors.Add(new GridPos(tX - 1, tY + tDy));
                                }
                            }
                            if (iParam.SearchGrid.IsWalkableAt(tX + 1, tY))
                            {
                                tNeighbors.Add(new GridPos(tX + 1, tY));
                            }
                            if (iParam.SearchGrid.IsWalkableAt(tX - 1, tY))
                            {
                                tNeighbors.Add(new GridPos(tX - 1, tY));
                            }
                        }
                        else
                        {
                            if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY))
                            {
                                tNeighbors.Add(new GridPos(tX + tDx, tY));

                                if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY + 1)
                                    && iParam.SearchGrid.IsWalkableAt(tX, tY + 1))
                                {
                                    tNeighbors.Add(new GridPos(tX + tDx, tY + 1));
                                }
                                if (iParam.SearchGrid.IsWalkableAt(tX + tDx, tY - 1)
                                    && iParam.SearchGrid.IsWalkableAt(tX, tY - 1))
                                {
                                    tNeighbors.Add(new GridPos(tX + tDx, tY - 1));
                                }
                            }
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY + 1))
                            {
                                tNeighbors.Add(new GridPos(tX, tY + 1));
                            }
                            if (iParam.SearchGrid.IsWalkableAt(tX, tY - 1))
                            {
                                tNeighbors.Add(new GridPos(tX, tY - 1));
                            }
                        }
                    }
                }
            }
                // return all neighbors
            else
            {
                tNeighborNodes = iParam.SearchGrid.GetNeighbors(iNode, iParam.CrossCorner, iParam.CrossAdjacentPoint);
                for (int i = 0; i < tNeighborNodes.Count; i++)
                {
                    tNeighborNode = tNeighborNodes[i];
                    tNeighbors.Add(new GridPos(tNeighborNode.x, tNeighborNode.y));
                }
            }

            return tNeighbors;
        }

        private class JumpSnapshot
        {
            public int iX;

            public int iY;

            public int iPx;

            public int iPy;

            public int tDx;

            public int tDy;

            public GridPos? jx;

            public GridPos? jy;

            public int stage;

            public JumpSnapshot()
            {
                this.iX = 0;
                this.iY = 0;
                this.iPx = 0;
                this.iPy = 0;
                this.tDx = 0;
                this.tDy = 0;
                this.jx = null;
                this.jy = null;
                this.stage = 0;
            }
        }
    }
}