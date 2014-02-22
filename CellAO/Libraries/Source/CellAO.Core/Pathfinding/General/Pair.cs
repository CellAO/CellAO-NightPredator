using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CellAO.Core.Pathfinding.General
{
   public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T iFirst, U iSecond)
        {
            this.First = iFirst;
            this.Second = iSecond;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }
}
