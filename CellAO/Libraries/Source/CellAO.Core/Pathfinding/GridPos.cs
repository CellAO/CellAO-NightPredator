using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellAO.Core.Pathfinding
{

    public struct GridPos
    {
        public int x;
        public int y;
        public GridPos(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override bool Equals(System.Object obj)
        {
            if (!(obj is GridPos))
                return false;
            GridPos p = (GridPos)obj;
            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public bool Equals(GridPos p)
        {
            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public static bool operator ==(GridPos a, GridPos b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // Return true if the fields match:
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(GridPos a, GridPos b)
        {
            return !(a == b);
        }

        public GridPos Set(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
            return this;
        }
    }
}
