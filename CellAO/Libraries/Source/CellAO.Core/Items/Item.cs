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

namespace CellAO.Core.Items
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Actions;
    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class Item : IItem
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Dictionary<int, int> Attributes = new Dictionary<int, int>();

        /// <summary>
        /// </summary>
        private readonly ItemTemplate templateHigh;

        /// <summary>
        /// </summary>
        private readonly ItemTemplate templateLow;

        /// <summary>
        /// </summary>
        private List<Actions> actions = null;

        /// <summary>
        /// </summary>
        private List<Events> events = null;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="QL">
        /// </param>
        /// <param name="lowID">
        /// </param>
        /// <param name="highID">
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public Item(int QL, int lowID, int highID)
        {
            // Checks:
            if ((!ItemLoader.ItemList.ContainsKey(lowID)) || (!ItemLoader.ItemList.ContainsKey(highID)))
            {
                throw new ArgumentOutOfRangeException("No Item found with ID " + lowID);
            }

            this.templateLow = ItemLoader.ItemList[lowID];
            this.templateHigh = ItemLoader.ItemList[highID];
            this.Quality = QL < this.templateLow.Quality
                ? this.templateLow.Quality
                : (QL > this.templateHigh.Quality ? this.templateHigh.Quality : QL);
            this.Identity = new Identity();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// </summary>
        public int HighID
        {
            get
            {
                return this.templateHigh.ID;
            }
        }

        /// <summary>
        /// Gets or sets Item Instance Identity
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        /// </summary>
        public List<Actions> ItemActions
        {
            get
            {
                if (this.actions == null)
                {
                    this.CreateInterpolatedRequirements();
                }

                return this.actions;
            }
        }

        /// <summary>
        /// </summary>
        public List<Events> ItemEvents
        {
            get
            {
                if (this.events == null)
                {
                    this.CreateInterpolatedRequirements();
                }

                return this.events;
            }
        }

        /// <summary>
        /// </summary>
        public int LowID
        {
            get
            {
                return this.templateLow.ID;
            }
        }

        /// <summary>
        /// </summary>
        public int MultipleCount
        {
            get
            {
                // Return MaxEnergy = MultipleCount
                return this.GetAttribute(412);
            }

            set
            {
                this.SetAttribute(412, value);
            }
        }

        /// <summary>
        /// </summary>
        public int Nothing
        {
            get
            {
                return this.templateLow.Nothing;
            }
        }

        /// <summary>
        /// </summary>
        public int Quality { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="actionType">
        /// </param>
        /// <param name="entity">
        /// </param>
        /// <returns>
        /// </returns>
        public bool CheckRequirements(ActionType actionType, ITargetingEntity entity)
        {
            return true;
        }

        /// <summary>
        /// </summary>
        public void CreateInterpolatedRequirements()
        {
            float factor = (this.Quality - this.templateLow.Quality)
                           / (this.templateHigh.Quality - this.templateLow.Quality);
            if (this.actions == null)
            {
                if (this.Quality == this.templateLow.Quality)
                {
                    this.actions = this.templateLow.Actions;
                }
                else if (this.Quality == this.templateHigh.Quality)
                {
                    this.actions = this.templateHigh.Actions;
                }
                else
                {
                    // We need to create the interpolated actions first
                    this.actions = new List<Actions>();
                    foreach (Actions action in this.templateLow.Actions)
                    {
                        Actions temp = this.templateLow.Actions.Single(x => x.ActionType == action.ActionType).Copy();

                        Actions highActions = this.templateHigh.Actions.Single(x => x.ActionType == action.ActionType);
                        Actions lowActions = action;
                        for (int reqnum = 0; reqnum < highActions.Requirements.Count; reqnum++)
                        {
                            temp.Requirements[reqnum].Value =
                                Convert.ToInt32(
                                    factor
                                    * (highActions.Requirements[reqnum].Value - lowActions.Requirements[reqnum].Value));
                        }

                        this.actions.Add(temp);
                    }
                }
            }

            if (this.events == null)
            {
                // We need to create interpolated events first
                if (this.Quality == this.templateLow.Quality)
                {
                    this.events = this.templateLow.Events;
                }
                else if (this.Quality == this.templateHigh.Quality)
                {
                    this.events = this.templateHigh.Events;
                }
                else
                {
                    this.events = new List<Events>();

                    for (int evnum = 0; evnum < this.templateLow.Events.Count; evnum++)
                    {
                        Events temp = this.templateLow.Events[evnum].Copy();
                        for (int funcnum = 0; funcnum < this.templateLow.Events[evnum].Functions.Count; funcnum++)
                        {
                            for (int reqnum = 0;
                                reqnum < this.templateLow.Events[evnum].Functions[funcnum].Requirements.Count;
                                reqnum++)
                            {
                                temp.Functions[funcnum].Requirements[reqnum].Value =
                                    Convert.ToInt32(
                                        factor
                                        * (this.templateHigh.Events[evnum].Functions[funcnum].Requirements[reqnum].Value
                                           - this.templateLow.Events[evnum].Functions[funcnum].Requirements[reqnum]
                                               .Value)
                                        + this.templateLow.Events[evnum].Functions[funcnum].Requirements[reqnum].Value);
                            }

                            for (int argnum = 0;
                                argnum < this.templateLow.Events[evnum].Functions[funcnum].Arguments.Values.Count;
                                argnum++)
                            {
                                if (temp.Functions[funcnum].Arguments.Values[argnum].IsTypeOf<int>() == true)
                                {
                                    temp.Functions[funcnum].Arguments.Values[argnum] =
                                        Convert.ToInt32(
                                            factor
                                            * (this.templateHigh.Events[evnum].Functions[funcnum].Arguments.Values[
                                                argnum].AsInt32()
                                               - this.templateLow.Events[evnum].Functions[funcnum].Arguments.Values[
                                                   argnum].AsInt32())
                                            + this.templateLow.Events[evnum].Functions[funcnum].Arguments.Values[argnum]
                                                .AsInt32());
                                }
                                else if (temp.Functions[funcnum].Arguments.Values[argnum].IsTypeOf<float>() == true)
                                {
                                    temp.Functions[funcnum].Arguments.Values[argnum] = factor
                                                                                       * (this.templateHigh.Events[evnum
                                                                                           ].Functions[funcnum]
                                                                                           .Arguments.Values[argnum]
                                                                                           .AsSingle()
                                                                                          - this.templateLow.Events[
                                                                                              evnum].Functions[funcnum]
                                                                                              .Arguments.Values[argnum]
                                                                                              .AsSingle())
                                                                                       + this.templateLow.Events[evnum]
                                                                                           .Functions[funcnum].Arguments
                                                                                           .Values[argnum].AsSingle();
                                }
                            }
                        }

                        this.events.Add(temp);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="attributeId">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetAttribute(int attributeId)
        {
            if (this.Attributes.Keys.Contains(attributeId))
            {
                return this.Attributes[attributeId];
            }

            int lowAttribute = this.templateLow.getItemAttribute(attributeId);
            int highAttribute = this.templateHigh.getItemAttribute(attributeId);

            if (this.templateHigh.Quality - this.templateLow.Quality == 0)
            {
                return lowAttribute;
            }

            return
                Convert.ToInt32(
                    (double)lowAttribute
                    + (highAttribute - lowAttribute) * (this.Quality - this.templateLow.Quality)
                    / (this.templateHigh.Quality - this.templateLow.Quality));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public byte[] GetItemAttributes()
        {
            byte[] temp = new byte[this.Attributes.Count * 2 * 4];

            int counter = 0;
            foreach (KeyValuePair<int, int> kv in this.Attributes)
            {
                byte[] temp2 = BitConverter.GetBytes(kv.Key);
                temp[counter++] = temp2[0];
                temp[counter++] = temp2[1];
                temp[counter++] = temp2[2];
                temp[counter++] = temp2[3];

                temp2 = BitConverter.GetBytes(kv.Value);
                temp[counter++] = temp2[0];
                temp[counter++] = temp2[1];
                temp[counter++] = temp2[2];
                temp[counter++] = temp2[3];
            }

            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="attributeId">
        /// </param>
        /// <param name="newValue">
        /// </param>
        public void SetAttribute(int attributeId, int newValue)
        {
            if (this.GetAttribute(attributeId) != newValue)
            {
                if (this.Identity.Type == IdentityType.None)
                {
                    // TODO: Instantiate Item
                }
            }

            // Do always set it for caching purposes
            if (!this.Attributes.ContainsKey(attributeId))
            {
                this.Attributes.Add(attributeId, newValue);
            }
            else
            {
                this.Attributes[attributeId] = newValue;
            }
        }

        /// <summary>
        /// </summary>
        public void WriteToDatabase()
        {
        }

        #endregion
    }
}