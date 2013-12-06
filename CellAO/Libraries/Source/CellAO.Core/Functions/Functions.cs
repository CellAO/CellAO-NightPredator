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

namespace CellAO.Core.Functions
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;

    using CellAO.Core.Requirements;

    using MsgPack.Serialization;

    #endregion

    /// <summary>
    /// AOFunctions
    /// </summary>
    [Serializable]
    public class Functions : IFunctions
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public Functions()
        {
            this.Arguments = new FunctionArguments();
            this.Requirements = new List<Requirements>();
            this.dolocalstats = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of Arguments
        /// </summary>
        public FunctionArguments Arguments { get; set; }

        /// <summary>
        /// Type of function (constants in ItemLoader)
        /// </summary>
        public int FunctionType { get; set; }

        /// <summary>
        /// Requirements to execute this function
        /// </summary>
        public List<Requirements> Requirements { get; set; }

        /// <summary>
        /// TargetType (constants in ItemLoader)
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// TickCount (for timers)
        /// </summary>
        public int TickCount { get; set; }

        /// <summary>
        /// TickInterval (for timers)
        /// </summary>
        public uint TickInterval { get; set; }

        /// <summary>
        /// process local stats (not serialized)
        /// </summary>
        public bool dolocalstats { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="ms">
        /// </param>
        /// <returns>
        /// </returns>
        public static Functions Deserialize(MemoryStream ms)
        {
            MessagePackSerializer<Functions> fromByte = MessagePackSerializer.Create<Functions>();
            return fromByte.Unpack(ms);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string Serialize()
        {
            MessagePackSerializer<Functions> toByte = MessagePackSerializer.Create<Functions>();
            var ms = new MemoryStream();
            toByte.Pack(ms, this);

            ms.Position = 0;
            return BitConverter.ToString(ms.ToArray()).Replace("-", string.Empty);
        }

        /// <summary>
        /// Copy Function
        /// </summary>
        /// <returns>new copy</returns>
        public IFunctions ShallowCopy()
        {
            IFunctions newAOF = new Functions();
            foreach (Requirements aor in this.Requirements)
            {
                Requirements newAOR = new Requirements();
                newAOR.ChildOperator = aor.ChildOperator;
                newAOR.Operator = aor.Operator;
                newAOR.Statnumber = aor.Statnumber;
                newAOR.Target = aor.Target;
                newAOR.Value = aor.Value;
                newAOF.Requirements.Add(newAOR);
            }

            foreach (object ob in this.Arguments.Values)
            {
                if (ob.GetType() == typeof(string))
                {
                    string z = (string)ob;
                    newAOF.Arguments.Values.Add(z);
                }

                if (ob.GetType() == typeof(int))
                {
                    int i = (int)ob;
                    newAOF.Arguments.Values.Add(i);
                }

                if (ob.GetType() == typeof(Single))
                {
                    float s = (Single)ob;
                    newAOF.Arguments.Values.Add(s);
                }
            }

            newAOF.dolocalstats = this.dolocalstats;
            newAOF.FunctionType = this.FunctionType;
            newAOF.Target = this.Target;
            newAOF.TickCount = this.TickCount;
            newAOF.TickInterval = this.TickInterval;

            return newAOF;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal Functions Copy()
        {
            Functions copy = new Functions();

            foreach (Requirements requirements in this.Requirements)
            {
                copy.Requirements.Add(requirements.Copy());
            }

            foreach (object ob in this.Arguments.Values)
            {
                if (ob.GetType() == typeof(string))
                {
                    string z = (string)ob;
                    copy.Arguments.Values.Add(z);
                }

                if (ob.GetType() == typeof(int))
                {
                    int i = (int)ob;
                    copy.Arguments.Values.Add(i);
                }

                if (ob.GetType() == typeof(Single))
                {
                    float s = (Single)ob;
                    copy.Arguments.Values.Add(s);
                }
            }

            copy.dolocalstats = this.dolocalstats;
            copy.FunctionType = this.FunctionType;
            copy.Target = this.Target;
            copy.TickCount = this.TickCount;
            copy.TickInterval = this.TickInterval;

            return copy;
        }

        #endregion
    }
}