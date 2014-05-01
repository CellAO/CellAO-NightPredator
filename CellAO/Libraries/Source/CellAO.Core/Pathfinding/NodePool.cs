#region License

// Copyright (c) 2005-2014, CellAO Team
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

    using System.Collections.Generic;

    using CellAO.Core.Pathfinding.Grid;

    #endregion

    public class NodePool
    {
        protected Dictionary<GridPos, Node> m_nodes;

        public NodePool()
        {
            this.m_nodes = new Dictionary<GridPos, Node>();
        }

        public Dictionary<GridPos, Node> Nodes
        {
            get
            {
                return this.m_nodes;
            }
        }

        public Node GetNode(int iX, int iY)
        {
            var pos = new GridPos(iX, iY);
            return this.GetNode(pos);
        }

        public Node GetNode(GridPos iPos)
        {
            if (this.m_nodes.ContainsKey(iPos))
            {
                return this.m_nodes[iPos];
            }
            return null;
        }

        public Node SetNode(int iX, int iY, bool? iWalkable = null)
        {
            var pos = new GridPos(iX, iY);
            return SetNode(pos, iWalkable);
        }

        public Node SetNode(GridPos iPos, bool? iWalkable = null)
        {
            if (iWalkable.HasValue)
            {
                if (iWalkable.Value == true)
                {
                    if (this.m_nodes.ContainsKey(iPos))
                    {
                        return this.m_nodes[iPos];
                    }
                    var newNode = new Node(iPos.x, iPos.y, iWalkable);
                    this.m_nodes.Add(iPos, newNode);
                    return newNode;
                }
                else
                {
                    this.removeNode(iPos);
                }
            }
            else
            {
                var newNode = new Node(iPos.x, iPos.y, true);
                this.m_nodes.Add(iPos, newNode);
                return newNode;
            }
            return null;
        }

        protected void removeNode(int iX, int iY)
        {
            var pos = new GridPos(iX, iY);
            this.removeNode(pos);
        }

        protected void removeNode(GridPos iPos)
        {
            if (this.m_nodes.ContainsKey(iPos))
            {
                this.m_nodes.Remove(iPos);
            }
        }
    }
}