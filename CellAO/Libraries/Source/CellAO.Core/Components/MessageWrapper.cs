using CellAO.Core.Network;
using SmokeLounge.AOtomation.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Components
{
    public class MessageWrapper<T> where T : MessageBody
    {
        public IZoneClient Client;
        public Message Message;
        public T MessageBody;
    }
}
