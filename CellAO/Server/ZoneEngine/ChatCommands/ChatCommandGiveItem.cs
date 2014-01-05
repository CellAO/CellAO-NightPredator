#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace ZoneEngine.ChatCommands
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Inventory;
    using CellAO.Core.Items;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public class ChatCommandGiveItem : AOChatCommand
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool CheckCommandArguments(string[] args)
        {
            List<Type> check = new List<Type>();
            check.Add(typeof(int));
            check.Add(typeof(int));

            return CheckArgumentHelper(check, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatText.CreateIM(
                    character, 
                    "Usage: Select target and /command giveitem id ql\r\nIt doesn't matter if high or low id is given"));
            return;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="args">
        /// </param>
        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            IInstancedEntity targetEntity = null;

            // Fall back to self it no target is selected
            if ((targetEntity = character.Playfield.FindByIdentity(target)) == null)
            {
                targetEntity = character;
            }

            IItemContainer container = targetEntity as IItemContainer;

            // Does this entity have a BaseInventory?
            if (container != null)
            {
                int lowId;
                int highId;
                int ql;
                if (!int.TryParse(args[1], out lowId))
                {
                    character.Playfield.Publish(ChatText.CreateIM(character, "LowId is no number"));
                    return;
                }

                if (!int.TryParse(args[2], out ql))
                {
                    character.Playfield.Publish(ChatText.CreateIM(character, "QualityLevel is no number"));
                    return;
                }

                // Determine low and high id depending on ql
                lowId = ItemLoader.ItemList[lowId].GetLowId(ql);
                highId = ItemLoader.ItemList[lowId].GetHighId(ql);

                Item item = new Item(ql, lowId, highId);
                if (ItemLoader.ItemList[lowId].IsStackable())
                {
                    item.MultipleCount = ItemLoader.ItemList[lowId].getItemAttribute(212);
                }

                InventoryError err = container.BaseInventory.TryAdd(item);
                if (err != InventoryError.OK)
                {
                    character.Playfield.Publish(
                        ChatText.CreateIM(character, "Could not add to inventory. (" + err + ")"));
                }

                if (targetEntity as Character != null)
                {
                    AddTemplate.Send((targetEntity as Character).Client, item);
                }
            }
            else
            {
                character.Playfield.Publish(ChatText.CreateIM(character, "Target has no Inventory."));
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GMLevelNeeded()
        {
            return 1;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override List<string> ListCommands()
        {
            List<string> temp = new List<string>();
            temp.Add("giveitem");
            return temp;
        }

        #endregion
    }
}