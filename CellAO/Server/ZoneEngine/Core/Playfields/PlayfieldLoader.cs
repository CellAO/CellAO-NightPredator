using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.Playfields
{
    using CellAO.Core.Playfields;
    using CellAO.Core.Statels;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    public static class PlayfieldLoader
    {
        public static Dictionary<int,PlayfieldData> PFData = new Dictionary<int, PlayfieldData>();
        public static int CacheAllPlayfieldData()
        {
            return CacheAllPlayfieldData("playfields.dat");
        }
        public static int CacheAllPlayfieldData(string fname)
        {
            PFData = new Dictionary<int, PlayfieldData>();

            MessagePackZip.UncompressData<PlayfieldData>(fname).ForEach(x => PFData.Add(x.PlayfieldId, x));

            GC.Collect();
            return PFData.Count;

        }
    }
    public class EqualityComparer : IEqualityComparer<Identity>
    {
        public bool Equals(Identity x, Identity y)
        {
            return (x.Type == y.Type) && (x.Instance == y.Instance);
        }

        public int GetHashCode(Identity obj)
        {
            return obj.Type.GetHashCode() ^ obj.Instance.GetHashCode();
        }
    }

}
