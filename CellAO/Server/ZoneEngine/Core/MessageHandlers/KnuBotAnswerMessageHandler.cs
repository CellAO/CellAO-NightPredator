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

    public class KnuBotAnswerMessageHandler : BaseMessageHandler<KnuBotAnswerMessage,KnuBotAnswerMessageHandler>
    {
        public KnuBotAnswerMessageHandler()
        {
            this.Direction=MessageHandlerDirection.InboundOnly;
        }

        public override void Receive(MessageWrapper<KnuBotAnswerMessage> messageWrapper)
        {
            // TODO: Fill in code!
            // Find character object by identity
            // call character AI controller 
            ICharacter npc = Pool.Instance.GetObject<ICharacter>(messageWrapper.MessageBody.Target);
            if (npc != null)
            {
                // npc.KnuBotAnswerIncoming(messageWrapper.MessageBody.Answer);
            }
        }
    }
}
