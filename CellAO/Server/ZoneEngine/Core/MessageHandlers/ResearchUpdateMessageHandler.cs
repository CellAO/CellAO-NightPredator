using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.MessageHandlers
{
    using CellAO.Core.Components;
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class ResearchUpdateMessageHandler : BaseMessageHandler<ResearchUpdateMessage,ResearchUpdateMessageHandler>
    {

        public void Send(ICharacter character, ResearchUpdateEntry[] entries)
        {
            this.Send(character,Filler(character.Identity,entries));
        }

        private MessageDataFiller Filler(Identity identity, ResearchUpdateEntry[] entries)
        {
            return x =>
            {
                x.Entries = entries;
                x.Identity = identity;
                x.Unknown1 = 1;
                x.Unknown = 1;
            };
        }
    }
}
