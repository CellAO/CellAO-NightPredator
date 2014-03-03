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
    using CellAO.ObjectManager;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class SocialArmorInventoryPage : BaseInventoryPage, IItemSlotHandler
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="ownerInstance">
        /// </param>
        public SocialArmorInventoryPage(int ownerInstance, Pool pooledIn)
            : base((int)IdentityType.SocialPage, 15, 0x31, ownerInstance, pooledIn)
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
                    foreach (Events events in item.ItemEvents.Where(x => x.EventType == EventType.OnWear))
                    {
                        foreach (
                            Functions functions in events.Functions.Where(x => this.IsSocialTabFunction(x.FunctionType))
                            )
                        {
                            bool result = true;
                            foreach (Requirements requirements in functions.Requirements)
                            {
                                result &= requirements.CheckRequirement(character);
                                if (!result)
                                {
                                    break;
                                }
                            }

                            if (result)
                            {
                                Functions copy = functions.Copy();
                                MessagePackObject mpo = new MessagePackObject();
                                mpo = itemSlot;
                                copy.Arguments.Values.Add(mpo);
                                character.Client.CallFunction(copy);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="p">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsSocialTabFunction(int p)
        {
            // Functions applyable on social page: (List not complete yet)
            int[] goodFunctions = { 53035, 53039, 53054 };
            return goodFunctions.Any(x => x == p);
        }

        #endregion
    }
}