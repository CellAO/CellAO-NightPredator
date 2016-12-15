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

namespace CellAO.Core.Items
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Text;

    using CellAO.Core.Actions;
    using CellAO.Core.Events;
    using CellAO.Database.Dao;
    using CellAO.Stats;

    #endregion

    /// <summary>
    /// </summary>
    [Serializable]
    public class ItemTemplate : IItemNanoEvents, IItemNanoActions
    {
        #region Fields

        /// <summary>
        /// List of Attack attributes
        /// </summary>
        public Dictionary<int, int> Attack;

        /// <summary>
        /// List of defensive attributes
        /// </summary>
        public Dictionary<int, int> Defend;

        /// <summary>
        /// Item Flags
        /// </summary>
        public int Flags;

        /// <summary>
        /// Item low ID
        /// </summary>
        public int ID;

        /// <summary>
        /// Item type
        /// </summary>
        public int ItemType;

        /// <summary>
        /// Stacked item count
        /// </summary>
        public int MultipleCount;

        /// <summary>
        /// dunno yet
        /// </summary>
        public int Nothing;

        /// <summary>
        /// Quality level
        /// </summary>
        public int Quality;

        /// <summary>
        /// </summary>
        public List<int> Relations;

        /// <summary>
        /// Item attributes
        /// </summary>
        public Dictionary<int, int> Stats;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public ItemTemplate()
        {
            this.Actions = new List<AOAction>(4);
            this.Events = new List<Event>(4);
            this.Attack = new Dictionary<int, int>(6);
            this.Defend = new Dictionary<int, int>(3);
            this.Stats = new Dictionary<int, int>(20);
            this.Relations = new List<int>(6);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public List<AOAction> Actions { get; set; }

        /// <summary>
        /// </summary>
        public List<Event> Events { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanAimedShot()
        {
            return (this.CanFlags() & (1 << 14)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeAppliedOnFightingTarget()
        {
            return (this.CanFlags() & (1 << 31)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeAppliedOnFriendly()
        {
            return (this.CanFlags() & (1 << 21)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeAppliedOnHostile()
        {
            return (this.CanFlags() & (1 << 22)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeAppliedOnSelf()
        {
            return (this.CanFlags() & (1 << 23)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeAttacked()
        {
            return (this.Flags & (1 << 28)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeParriedRiposted()
        {
            return (this.CanFlags() & (1 << 30)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeTemplateItem()
        {
            return (this.Flags & (1 << 3)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBeWornWithSocialArmor()
        {
            return (this.CanFlags() & (1 << 28)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBow()
        {
            return (this.CanFlags() & (1 << 15)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBrawl()
        {
            return (this.CanFlags() & (1 << 25)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanBurst()
        {
            return (this.CanFlags() & (1 << 11)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanCarry()
        {
            return (this.CanFlags() & (1 << 0)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanDimach()
        {
            return (this.CanFlags() & (1 << 26)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanDisarmTraps()
        {
            return (this.CanFlags() & (1 << 19)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanFastAttack()
        {
            return (this.CanFlags() & (1 << 18)) > 0;
        }

        /// <summary>
        /// Item Flags
        /// </summary>
        /// <returns>
        /// </returns>
        public int CanFlags()
        {
            return this.getItemAttribute(30);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanFlingShot()
        {
            return (this.CanFlags() & (1 << 12)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanFullAuto()
        {
            return (this.CanFlags() & (1 << 13)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanParryRiposte()
        {
            return (this.CanFlags() & (1 << 29)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanSit()
        {
            return (this.CanFlags() & (1 << 1)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanSneakAttack()
        {
            return (this.CanFlags() & (1 << 17)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CanThrowAttack()
        {
            return (this.CanFlags() & (1 << 16)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool CantBeSplit()
        {
            return (this.CanFlags() & (1 << 24)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool ConfirmOnUse()
        {
            return (this.CanFlags() & (1 << 4)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public ItemTemplate Copy()
        {
            ItemTemplate copy = new ItemTemplate()
            {
                ID = this.ID,
                Flags = this.Flags,
                ItemType = this.ItemType,
                MultipleCount = this.MultipleCount,
                Nothing = this.Nothing,
                Quality = this.Quality
            };
            foreach (KeyValuePair<int, int> kv in this.Attack)
            {
                copy.Attack.Add(kv.Key, kv.Value);
            }

            foreach (AOAction actions in this.Actions)
            {
                copy.Actions.Add(actions.Copy());
            }

            foreach (KeyValuePair<int, int> kv in this.Defend)
            {
                copy.Defend.Add(kv.Key, kv.Value);
            }

            foreach (Event events in this.Events)
            {
                copy.Events.Add(events.Copy());
            }

            foreach (KeyValuePair<int, int> kv in this.Stats)
            {
                copy.Stats.Add(kv.Key, kv.Value);
            }

            return copy;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool DoesAutoSelect()
        {
            return (this.CanFlags() & (1 << 20)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool EnablesHandAttractors()
        {
            return (this.CanFlags() & (1 << 27)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool FallingDisabled()
        {
            return (this.Flags & (1 << 29)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="qualityLevel">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetHighId(int qualityLevel)
        {
            int result = 1234567890;
            foreach (int ids in this.Relations)
            {
                if (result == 1234567890)
                {
                    if (ItemLoader.ItemList[ids].Quality >= qualityLevel)
                    {
                        result = ids;
                    }
                }
                else if ((ItemLoader.ItemList[ids].Quality < ItemLoader.ItemList[result].Quality)
                         && (ItemLoader.ItemList[ids].Quality >= qualityLevel))
                {
                    result = ids;
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="qualityLevel">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetLowId(int qualityLevel)
        {
            int result = -1;
            foreach (int ids in this.Relations)
            {
                if (result == -1)
                {
                    if (ItemLoader.ItemList[ids].Quality <= qualityLevel)
                    {
                        result = ids;
                    }
                }
                else if ((ItemLoader.ItemList[ids].Quality > ItemLoader.ItemList[result].Quality)
                         && (ItemLoader.ItemList[ids].Quality <= qualityLevel))
                {
                    result = ids;
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasAnimation()
        {
            return (this.Stats[0] & (1 << 17)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasDamage()
        {
            return (this.Stats[0] & (1 << 30)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasEnergy()
        {
            return (this.Stats[0] & (1 << 22)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasModifiedDescription()
        {
            return (this.Stats[0] & (1 << 1)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasModifiedName()
        {
            return (this.Stats[0] & (1 << 2)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasMultipleCount()
        {
            return (this.Stats[0] & (1 << 5)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasRotation()
        {
            return (this.Stats[0] & (1 << 18)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool HasSentFirstIIR()
        {
            return (this.Stats[0] & (1 << 21)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IllegalForClan()
        {
            return (this.Stats[0] & (1 << 24)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IllegalForOmni()
        {
            return (this.Stats[0] & (1 << 25)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsConsumable()
        {
            return (this.CanFlags() & (1 << 5)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsDefaultTarget()
        {
            return (this.Stats[0] & (1 << 14)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsItemSocialArmor()
        {
            return (this.Stats[0] & (1 << 8)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsLockPicker()
        {
            return (this.CanFlags() & (1 << 8)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsLocked()
        {
            return (this.Stats[0] & (1 << 6)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsNoDrop()
        {
            return (this.Stats[0] & (1 << 26)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsNull()
        {
            return (this.Stats[0] & (1 << 16)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsOpen()
        {
            return (this.Stats[0] & (1 << 7)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsRepulsive()
        {
            return (this.Stats[0] & (1 << 13)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsStackable()
        {
            return (this.CanFlags() & (1 << 9)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsStaionary()
        {
            return (this.Stats[0] & (1 << 12)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsTutorChip()
        {
            return (this.CanFlags() & (1 << 6)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsTutorDevie()
        {
            return (this.CanFlags() & (1 << 7)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsUnique()
        {
            return (this.Stats[0] & (1 << 27)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsVisible()
        {
            return (this.Stats[0] & (1 << 0)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool MirrorsInLeftHand()
        {
            return (this.Stats[0] & (1 << 23)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool NeedsNoAmmo()
        {
            return (this.CanFlags() & (1 << 10)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool NoSelectionIndicator()
        {
            return (this.Stats[0] & (1 << 10)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool OverrideItemTexture()
        {
            return (this.Stats[0] & (1 << 15)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool StatelCollisionDisabled()
        {
            return (this.Stats[0] & (1 << 31)) != 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool TellCollision()
        {
            return (this.Stats[0] & (1 << 9)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool TurnsOnUse()
        {
            return (this.Stats[0] & (1 << 4)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool UseEmptyDestruct()
        {
            return (this.Stats[0] & (1 << 11)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Useable()
        {
            return (this.CanFlags() & (1 << 3)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool WantsCollision()
        {
            return (this.Stats[0] & (1 << 19)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool WantsSignals()
        {
            return (this.Stats[0] & (1 << 20)) > 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Wearable()
        {
            return (this.CanFlags() & (1 << 2)) > 0;
        }

        /// <summary>
        /// Get item attribute
        /// </summary>
        /// <param name="number">
        /// number of attribute
        /// </param>
        /// <returns>
        /// Value of item attribute
        /// </returns>
        public int getItemAttribute(int number)
        {
            Contract.Assume(this.Stats != null);
            if (this.Stats.ContainsKey(number))
            {
                return this.Stats[number];
            }

            // TODO: Might need adjustments for Items
            return StatNamesDefaults.GetDefault(number);
        }

        public string DebugString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Item template " + this.ID + " '" + ItemNamesDao.Instance.Get(this.ID).Name + "'");
            sb.Append("Actions: ");
            foreach (AOAction ac in this.Actions)
            {
                sb.Append(ac.ActionType.ToString()+", ");
            }
            sb.Remove(sb.Length - 2, 2);

            sb.AppendLine();

            sb.Append("Events: ");
            foreach (Event ev in this.Events)
            {
                sb.AppendLine(ev.EventType.ToString()+":");
                sb.AppendLine(ev.ToString());
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion
    }
}