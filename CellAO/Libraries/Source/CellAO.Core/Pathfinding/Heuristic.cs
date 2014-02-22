using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Pathfinding
{
    public enum HeuristicMode
    {
        MANHATTAN,
        EUCLIDEAN,
        CHEBYSHEV,

    };

    public class Heuristic
    {
        public static float Manhattan(int iDx, int iDy)
        {
            return (float)iDx + iDy;
        }

        public static float Euclidean(int iDx, int iDy)
        {
            var tFdx = (float)iDx;
            var tFdy = (float)iDy;
            return (float)Math.Sqrt((double)(tFdx * tFdx + tFdy * tFdy));
        }

        public static float Chebyshev(int iDx, int iDy)
        {
            return (float)Math.Max(iDx, iDy);
        }

    }
}
