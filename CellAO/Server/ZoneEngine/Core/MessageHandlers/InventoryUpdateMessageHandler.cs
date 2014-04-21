using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.MessageHandlers
{
    using System.Net.Mail;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class InventoryUpdateMessageHandler : BaseMessageHandler<InventoryUpdateMessage, InventoryUpdateMessageHandler>
    {
        public void Send(ICharacter character, IInventoryPage page)
        {
            this.Send(character, this.FillData(character, page));
        }

        public MessageDataFiller FillData(ICharacter character, IInventoryPage page)
        {
            return x =>
            {
                x.BagIdentity = page.Identity;
                x.NumberOfSlots = page.MaxSlots;
                x.SlotnumberInMainInventory = 0;
                List<InventoryEntry> temp = new List<InventoryEntry>();

                foreach (KeyValuePair<int, IItem> kv in page.List())
                {
                    temp.Add(
                        new InventoryEntry()
                        {
                            Slotnumber = kv.Key,
                            Identity = Identity.None,
                            Quality = kv.Value.Quality,
                            HighId = kv.Value.HighID,
                            LowId = kv.Value.LowID,
                            UnknownFlags = 0x21,
                            Unknown1 = (short)kv.Value.MultipleCount,
                            Unknown2 = 0
                        });
                }
                x.Entries = temp.ToArray();
                x.Unknown2 = 1;
                x.Unknown1 = 3;
                x.Identity = character.Identity;
                x.Unknown = 1;
            };
        }
    }
}
