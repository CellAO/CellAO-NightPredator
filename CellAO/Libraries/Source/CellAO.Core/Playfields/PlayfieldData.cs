using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Playfields
{
    using CellAO.Core.Statels;

    public class PlayfieldData
    {
        public int PlayfieldId;

        public string Name;

        public List<Door> Doors1 = new List<Door>();

        public List<StatelData> Statels = new List<StatelData>(); 
    }
}
