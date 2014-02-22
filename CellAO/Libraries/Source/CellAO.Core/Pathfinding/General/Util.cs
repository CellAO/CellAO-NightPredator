using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Pathfinding.General
{
    public class Util
    {
        private static Random m_random = new Random();
        public static float Random()
        {
            var result = m_random.NextDouble();
            return (float)result;
        }
    }
}
