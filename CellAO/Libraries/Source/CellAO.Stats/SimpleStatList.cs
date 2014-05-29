using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Stats
{
    using CellAO.Enums;
    using CellAO.Interfaces;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    public class SimpleStatList : IStatList
    {

        public bool Read()
        {
            return true;
        }

        public bool Write()
        {
            return true;
        }

        public SimpleStatList()
        {
            this.All = new List<IStat>();
        }

        public event EventHandler<StatChangedEventArgs> AfterStatChangedEvent;

        public List<IStat> All { get; private set; }

        public GameTuple<CharacterStat, uint>[] ChangedAnnouncingStats { get; private set; }

        public GameTuple<CharacterStat, uint>[] ChangedStats { get; private set; }

        public Identity Owner
        {
            get; private set; }

        IStat IStatList.this[int index]
        {
            get
            {
                var stat = this.All.FirstOrDefault(x => x.StatId == index);
                if (stat == null)
                {
                    stat = new SimpleStat(index);
                    this.All.Add(stat);
                }
                return stat;
            }
        }

        IStat IStatList.this[StatIds i]
        {
            get
            {
                var stat = this.All.FirstOrDefault(x => x.StatId == (int)i);
                if (stat == null)
                {
                    stat = new SimpleStat((int)i);
                    this.All.Add(stat);
                }
                return stat;
            }
        }

        IStat IStatList.this[string name]
        {
            get
            {
                int id = StatNamesDefaults.GetStatNumber(name);
                var stat = this.All.FirstOrDefault(x => x.StatId == id);
                if (stat == null)
                {
                    stat = new SimpleStat(id);
                    this.All.Add(stat);
                }
                return stat;
            }
        }

        public void AfterStatChangedEventHandler(StatChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ClearChangedFlags()
        {
            throw new NotImplementedException();
        }

        public void ClearModifiers()
        {
            throw new NotImplementedException();
        }

        public Stat GetStatByNumber(int number)
        {
            throw new NotImplementedException();
        }

        public void GetChangedStats(Dictionary<int, uint> toPlayer, Dictionary<int, uint> toPlayfield)
        {
            throw new NotImplementedException();
        }

        public void SetBaseValueWithoutTriggering(int stat, uint value)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, uint> GetStatValues()
        {
            Dictionary<int,uint> temp = new Dictionary<int, uint>(this.All.Count);
            foreach (var t in this.All)
            {
                temp.Add(t.StatId,(uint)t.Value);
            }
            return temp;
        }
    }
}
