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

namespace ZoneEngine.Core.Functions
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using CellAO.Core.Entities;

    using MsgPack;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public class FunctionCollection
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static readonly FunctionCollection Instance;

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        private readonly Dictionary<int, FunctionPrototype> functions = new Dictionary<int, FunctionPrototype>();

        /// <summary>
        /// </summary>
        private Assembly assembly;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        static FunctionCollection()
        {
            Instance = new FunctionCollection();
            Instance.ReadFunctions();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Calls a function by its number
        /// </summary>
        /// <param name="functionNumber">
        /// The number of the function
        /// </param>
        /// <param name="self">
        /// The self.
        /// </param>
        /// <param name="caller">
        /// The caller.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <returns>
        /// </returns>
        public bool CallFunction(
            int functionNumber, 
            INamedEntity self, 
            INamedEntity caller, 
            IInstancedEntity target, 
            MessagePackObject[] arguments)
        {
            FunctionPrototype func = this.GetFunctionByNumber(functionNumber);
            if (func != null)
            {
                if (Program.DebugGameFunctions)
                {
                    LogUtil.Debug("Called " + func.GetType().Name + ": ");
                    LogUtil.Debug(FunctionArgumentList.List(arguments));
                }

                return func.Execute(self, caller, target, arguments);
            }

            if (Program.DebugGameFunctions)
            {
                LogUtil.Debug("Function " + functionNumber + " not found!");
                LogUtil.Debug(FunctionArgumentList.List(arguments));
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="functionnumber">
        /// </param>
        /// <returns>
        /// </returns>
        public FunctionPrototype GetFunctionByNumber(int functionnumber)
        {
            if (this.functions.Keys.Contains(functionnumber))
            {
                return this.functions[functionnumber];
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int NumberofRegisteredFunctions()
        {
            return this.functions.Keys.Count;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public bool ReadFunctions()
        {
            try
            {
                this.assembly = Assembly.GetExecutingAssembly();

                foreach (Type t in
                    this.assembly.GetTypes()
                        .Where(
                            x =>
                                x.IsClass && (x.Namespace == "ZoneEngine.Core.Functions.GameFunctions")
                                && ((x.Name != "FunctionPrototype") && (x.Name != "FunctionCollection"))))
                {
                    {
                        FunctionPrototype temp = (FunctionPrototype)this.assembly.CreateInstance(t.FullName);
                        if (temp == null)
                        {
                            throw new NullReferenceException("Could not create function " + t.FullName);
                        }

                        this.functions.Add(temp.ReturnNumber(), temp);
                    }
                }
            }
            catch (MissingMethodException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (FileLoadException)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}