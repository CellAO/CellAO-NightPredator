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

namespace ZoneEngine.Core.Functions
{
    #region Usings ...

    using CellAO.Core.Entities;
    using CellAO.Enums;

    using MsgPack;

    #endregion

    /// <summary>
    /// </summary>
    public abstract class FunctionPrototype
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public abstract FunctionType FunctionId { get; }

        /// <summary>
        /// </summary>
        public string FunctionName
        {
            get
            {
                return this.FunctionId.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public int FunctionNumber
        {
            get
            {
                return (int)this.FunctionId;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Locks function targets and executes the function
        /// </summary>
        /// <param name="self">
        /// Dynel (Character or NPC)
        /// </param>
        /// <param name="caller">
        /// Caller of the function
        /// </param>
        /// <param name="target">
        /// Target of the Function (Dynel or Statel)
        /// </param>
        /// <param name="arguments">
        /// Function Arguments
        /// </param>
        /// <returns>
        /// </returns>
        public abstract bool Execute(
            INamedEntity self,
            INamedEntity caller,
            IInstancedEntity target,
            MessagePackObject[] arguments);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual string ReturnName()
        {
            return this.FunctionName;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual int ReturnNumber()
        {
            return this.FunctionNumber;
        }

        #endregion
    }
}