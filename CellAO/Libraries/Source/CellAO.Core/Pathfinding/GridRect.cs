using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Pathfinding
{
    public struct GridRect
    {
        //TODO: Add Z cord to pathfinding.
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;

        public GridRect(int iMinX, int iMinY, int iMaxX, int iMaxY)
        {
            minX = iMinX;
            minY = iMinY;
            maxX = iMaxX;
            maxY = iMaxY;
        }

        public override int GetHashCode()
        {
            return minX ^ minY ^ maxX ^ maxY;
        }

        public override bool Equals(System.Object obj)
        {
            if (!(obj is GridRect))
                return false;
            var p = (GridRect)obj;
            // Return true if the fields match:
            return (minX == p.minX) && (minY == p.minY) && (maxX == p.maxX) && (maxY == p.maxY);
        }

        public bool Equals(GridRect p)
        {
            // Return true if the fields match:
            return (minX == p.minX) && (minY == p.minY) && (maxX == p.maxX) && (maxY == p.maxY);
        }

        public static bool operator ==(GridRect a, GridRect b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // Return true if the fields match:
            return (a.minX == b.minX) && (a.minY == b.minY) && (a.maxX == b.maxX) && (a.maxY == b.maxY);
        }

        public static bool operator !=(GridRect a, GridRect b)
        {
            return !(a == b);
        }

        public GridRect Set(int iMinX, int iMinY, int iMaxX, int iMaxY)
        {
            this.minX = iMinX;
            this.minY = iMinY;
            this.maxX = iMaxX;
            this.maxY = iMaxY;
            return this;
        }
    }
}
