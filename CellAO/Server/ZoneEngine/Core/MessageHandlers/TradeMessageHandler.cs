using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.MessageHandlers
{
    using CellAO.Core.Components;

    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    public class TradeMessageHandler : BaseMessageHandler<TradeMessage,TradeMessageHandler>
    {
        public TradeMessageHandler()
        {
            this.Direction = MessageHandlerDirection.InboundOnly;
        }

        protected override void Read(TradeMessage message, CellAO.Core.Network.IZoneClient client)
        {
            //FollowTargetMessageHandler.Default.Send()
        }

    }
}
