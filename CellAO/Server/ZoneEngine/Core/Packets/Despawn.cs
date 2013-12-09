using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.Packets
{
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    public static class Despawn
    {
        public static DespawnMessage Create(Identity targetId)
        {
            var message = new DespawnMessage { Identity = targetId, Unknown = 0x01 };
            return message;
        }
    }
}
