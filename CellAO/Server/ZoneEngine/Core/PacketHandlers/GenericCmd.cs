#region License

// Copyright (c) 2005-2013, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
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

#endregion

namespace ZoneEngine.Core.PacketHandlers
{
    #region Usings ...

    using System;
    using System.Linq;

    using CellAO.Core.Events;
    using CellAO.Core.Items;
    using CellAO.Core.Statels;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.Packets;
    using ZoneEngine.Core.Playfields;

    #endregion

    /// <summary>
    /// </summary>
    public static class GenericCmd
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public static void Read(GenericCmdMessage message, ZoneClient client)
        {
            switch (message.Action)
            {
                case GenericCmdAction.Get:
                    break;
                case GenericCmdAction.Drop:
                    break;
                case GenericCmdAction.Use:
                    if (message.Target.Type == IdentityType.Inventory)
                    {
                        Item item = null;
                        try
                        {
                            item = client.Character.BaseInventory.GetItemInContainer(
                                (int)message.Target.Type, 
                                message.Target.Instance);
                        }
                        catch (Exception)
                        {
                        }

                        if (item == null)
                        {
                            throw new NullReferenceException(
                                "No item found at " + message.Target.Type + "/" + message.Target.Instance);
                        }

                        TemplateAction.Send(client, item, (int)message.Target.Type, message.Target.Instance);

                        if (ItemLoader.ItemList[item.HighID].IsConsumable())
                        {
                            item.MultipleCount--;
                            if (item.MultipleCount == 0)
                            {
                                client.Character.BaseInventory.RemoveItem(
                                    (int)message.Target.Type, 
                                    message.Target.Instance);

                                DeleteItem.Send(client, (int)message.Target.Type, message.Target.Instance);
                            }
                        }

                        item.PerformAction(client.Character, EventType.OnUse, message.Target.Instance);
                        Reply(message, client);
                    }
                    else
                    {
                        string s = "Generic Command received:\r\nAction: " + message.Action.ToString() + "("
                                   + ((int)message.Action).ToString() + ")\r\nTarget: " + message.Target.Type + " "
                                   + ((int)message.Target.Type).ToString("X8") + ":"
                                   + message.Target.Instance.ToString("X8");
                        if (PlayfieldLoader.PFData.ContainsKey(client.Character.Playfield.Identity.Instance))
                        {
                            StatelData sd =
                                PlayfieldLoader.PFData[client.Playfield.Identity.Instance].Statels.FirstOrDefault(
                                    x =>
                                        (x.StatelIdentity.Type == message.Target.Type)
                                        && (x.StatelIdentity.Instance == message.Target.Instance));

                            if (sd != null)
                            {
                                s = s + "\r\nFound Statel with " + sd.Events.Count + " events";
                                Events onUse = sd.Events.FirstOrDefault(x => x.EventType == (int)EventType.OnUse);
                                onUse.Perform(client.Character, client.Character);
                            }
                        }

                        client.Character.Send(new ChatTextMessage() { Identity = client.Character.Identity, Text = s });
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        private static void Reply(GenericCmdMessage message, ZoneClient client)
        {
            // Acknowledge action
            message.Identity = client.Character.Identity;
            message.Temp1 = 1;
            message.Unknown = 0;
            client.SendCompressed(message);
        }

        #endregion
    }
}