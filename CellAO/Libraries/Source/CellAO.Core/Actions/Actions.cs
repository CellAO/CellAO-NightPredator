﻿#region License

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

namespace CellAO.Core.Actions
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Requirements;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// AOActions covers all action types, with their reqs
    /// </summary>
    [Serializable]
    public class Actions : IActions
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public Actions()
        {
            this.Requirements = new List<Requirements>(15);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Type of Action (constants in ItemLoader)
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// List of Requirements for this action
        /// </summary>
        public List<Requirements> Requirements { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="entity">
        /// </param>
        /// <returns>
        /// </returns>
        public bool CheckRequirements(IInstancedEntity entity)
        {
            bool result = true;
            foreach (Requirements requirements in this.Requirements)
            {
                if (requirements.ChildOperator == (int)Operator.And)
                {
                    result &= requirements.CheckRequirement(entity);
                }

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal Actions Copy()
        {
            Actions copy = new Actions();
            copy.ActionType = this.ActionType;
            foreach (Requirements requirements in this.Requirements)
            {
                copy.Requirements.Add(requirements.Copy());
            }

            return copy;
        }

        #endregion
    }
}