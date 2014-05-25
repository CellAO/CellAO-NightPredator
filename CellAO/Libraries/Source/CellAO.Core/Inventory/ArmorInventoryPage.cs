#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace CellAO.Core.Inventory
{
    #region Usings ...

    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Items;
    using CellAO.Core.Requirements;
    using CellAO.Enums;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class ArmorInventoryPage : BaseInventoryPage, IItemSlotHandler, IItemHotSwapHandler, IEquipmentPage
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="ownerInstance">
        /// </param>
        public ArmorInventoryPage(Identity ownerInstance)
            : base((int)IdentityType.ArmorPage, 15, 0x11, ownerInstance)
        {
            this.NeedsItemCheck = true;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public override void CalculateModifiers(Character character)
        {
            for (int itemSlot = this.FirstSlotNumber; itemSlot < this.FirstSlotNumber + this.MaxSlots; itemSlot++)
            {
                IItem item = this[itemSlot];
                if (item != null)
                {
                    foreach (Event events in item.ItemEvents.Where(x => x.EventType == EventType.OnWear))
                    {
                        foreach (Function functions in events.Functions)
                        {
                            bool result = true;
                            foreach (Requirement requirements in functions.Requirements)
                            {
                                result &= requirements.CheckRequirement(character);
                                if (!result)
                                {
                                    break;
                                }
                            }

                            if (result)
                            {
                                Function copy = functions.Copy();
                                MessagePackObject mpo = new MessagePackObject();
                                mpo = itemSlot;
                                copy.Arguments.Values.Add(mpo);
                                character.Controller.CallFunction(copy);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="statId">
        /// </param>
        /// <returns>
        /// </returns>
        public int Stat(int statId)
        {
            int value = 0;
            foreach (IItem item in this.List().Values)
            {
                value += item.GetAttribute(statId);
            }

            return value;
        }

        #endregion
    }
}