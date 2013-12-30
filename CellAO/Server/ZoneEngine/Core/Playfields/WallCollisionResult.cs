using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.Playfields
{
    using CellAO.Core.Playfields;

    public class WallCollisionResult
    {
        public PlayfieldWall FirstWall = new PlayfieldWall();
        public PlayfieldWall SecondWall = new PlayfieldWall();

        public float Factor = 0.0f;

        internal int GetDestinationIndex()
        {
            return (((int)FirstWall.DestinationIndex) << 16) | FirstWall.DestinationPlayfield;

        }

        public override string ToString()
        {
            return "First: " + Environment.NewLine + FirstWall.ToString() + Environment.NewLine + "Second: "
                   + Environment.NewLine + SecondWall.ToString();
        }
    }
}
