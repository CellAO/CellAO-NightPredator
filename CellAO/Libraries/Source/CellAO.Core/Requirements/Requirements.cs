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
    /// Requirements
    /// </summary>
    [Serializable]
    public class Requirement : IRequirement
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
        public Operator ChildOperator { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        public Operator Operator { get; set; }

        /// <summary>
        /// Stat to check against
        /// </summary>
        public int Statnumber { get; set; }

        /// <summary>
        /// Target, from constants
        /// </summary>
        public ItemTarget Target { get; set; }

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
                    //return this.theCheckFunc(entity);
                }
                catch (Exception)
                {
                    LogUtil.Debug(DebugInfoDetail.GameFunctions, "Could not create lambda for a requirement.");
                    LogUtil.Debug(DebugInfoDetail.GameFunctions, "Values:");
                    LogUtil.Debug(DebugInfoDetail.GameFunctions, "Target:       " + (this.Target));
                    LogUtil.Debug(
                        DebugInfoDetail.GameFunctions,
                        "StatId:       " + (this.Statnumber + " (" + StatNamesDefaults.GetStatName(this.Statnumber))
                        + ")");
                    LogUtil.Debug(DebugInfoDetail.GameFunctions, "Operator:     " + (this.Operator));
                    LogUtil.Debug(DebugInfoDetail.GameFunctions, "Value:        " + this.Value);
                    LogUtil.Debug(DebugInfoDetail.GameFunctions, "ChildOperator:" + (this.ChildOperator));
                    return false;
                }
            }
            LogUtil.Debug(DebugInfoDetail.GameFunctions, "Values:");
            LogUtil.Debug(DebugInfoDetail.GameFunctions, "Target:       " + (this.Target));
            LogUtil.Debug(
                DebugInfoDetail.GameFunctions,
                "StatId:       " + (this.Statnumber + " (" + StatNamesDefaults.GetStatName(this.Statnumber))
                + ")");
            LogUtil.Debug(DebugInfoDetail.GameFunctions, "Operator:     " + (this.Operator));
            LogUtil.Debug(DebugInfoDetail.GameFunctions, "Value:        " + this.Value + " <-> " + entity.Stats[this.Statnumber].Value.ToString());
            LogUtil.Debug(DebugInfoDetail.GameFunctions, "ChildOperator:" + (this.ChildOperator));
            LogUtil.Debug(DebugInfoDetail.GameFunctions, "Result:       " + this.theCheckFunc(entity));
            return this.theCheckFunc(entity);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public Requirement Copy()
        {
            Requirement copy = new Requirement();
            copy.Operator = this.Operator;
            copy.ChildOperator = this.ChildOperator;
            copy.Target = this.Target;
            copy.Statnumber = this.Statnumber;
            copy.Value = this.Value;
            return copy;
        }

        public override string ToString()
        {
            return this.Target + " " + this.Statnumber + " " + this.Operator + " " + this.Value;
        }

        #endregion
    }
}