using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Pathfinding.General
{
    public class Triple<T, U, V>
    {
        public Triple()
        {
        }

        public Triple(T iFirst, U iSecond, V iThird)
        {
            this.First = iFirst;
            this.Second = iSecond;
            this.Third = iThird;
        }

        public T First { get; set; }
        public U Second { get; set; }
        public V Third { get; set; }
    }
}
