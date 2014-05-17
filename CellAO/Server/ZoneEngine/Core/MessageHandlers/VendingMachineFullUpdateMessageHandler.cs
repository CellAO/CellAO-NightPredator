using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.MessageHandlers
{
    using CellAO.Core.Components;

    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class VendingMachineFullUpdateMessageHandler : BaseMessageHandler<VendingMachineFullUpdateMessage,VendingMachineFullUpdateMessageHandler>
    {

    }
}
