#region License

// Copyright (c) 2005-2016, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

#endregion

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class InventoryUpdateMessageHandler :
        BaseMessageHandler<InventoryUpdateMessage, InventoryUpdateMessageHandler>
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