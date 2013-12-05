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

namespace CellAO.Core.Requirements
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Enums;
    using CellAO.Stats;

    using Utility;

    #endregion

    /// <summary>
    /// AORequirements
    /// </summary>
    [Serializable]
    public class Requirements : IRequirements
    {
        #region Fields

        /// <summary>
        /// </summary>
        private Func<IInstancedEntity, bool> theCheckFunc = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Child operator
        /// </summary>
        public int ChildOperator { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        public int Operator { get; set; }

        /// <summary>
        /// Stat to check against
        /// </summary>
        public int Statnumber { get; set; }

        /// <summary>
        /// Target, from constants
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Value to check against
        /// </summary>
        public int Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="entity">
        /// </param>
        /// <returns>
        /// </returns>
        public bool CheckRequirement(IInstancedEntity entity)
        {
            if (this.theCheckFunc == null)
            {
                try
                {
                    this.theCheckFunc = RequirementLambdaCreator.Create(this);
                    return this.theCheckFunc(entity);
                }
                catch (Exception)
                {
                    LogUtil.Debug("Could not create lambda for a requirement.");
                    LogUtil.Debug("Values:");
                    LogUtil.Debug("Target:       " + ((ItemTarget)this.Target));
                    LogUtil.Debug(
                        "StatId:       " + (this.Statnumber + " (" + StatNamesDefaults.GetStatName(this.Statnumber))
                        + ")");
                    LogUtil.Debug("Operator:     " + ((Operator)this.Operator));
                    LogUtil.Debug("Value:        " + this.Value);
                    LogUtil.Debug("ChildOperator:" + ((Operator)this.ChildOperator));
                    return false;
                }
            }

            return this.theCheckFunc(entity);
        }

        #endregion
    }
}