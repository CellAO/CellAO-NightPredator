using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.MessageHandlers
{
    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    public class KnuBotCloseChatWindowMessageHandler: BaseMessageHandler<KnuBotCloseChatWindowMessage,KnuBotCloseChatWindowMessageHandler>
    {
        public KnuBotCloseChatWindowMessageHandler()
        {
            this.Direction=MessageHandlerDirection.InboundOnly;
        }

        public override void Receive(MessageWrapper<KnuBotCloseChatWindowMessage> messageWrapper)
        {
            ICharacter npc = Pool.Instance.GetObject<ICharacter>(messageWrapper.MessageBody.Target);
            if (npc != null)
            {
                // npc.KnuBotClose(messageWrapper.Client.Character.Identity);
            }

        }
    }
}
