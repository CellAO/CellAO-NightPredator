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

namespace CellAO.Core.Events
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Functions;
    using CellAO.Core.Requirements;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    [Serializable]
    public class Event : IEvent
    {
        #region Fields

        /// <summary>
        /// Type of the Event (constants in ItemLoader)
        /// </summary>
        // private int eventType;
        /// <summary>
        /// List of Functions of the Event
        /// </summary>
        //private List<Functions> functions = new List<Functions>(10);

        #endregion

        #region Public Properties

        /// <summary>
        /// Type of the Event (constants in ItemLoader)
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// List of Functions of the Event
        /// </summary>
        public List<Function> Functions { get; set; }

        #endregion

        public Event()
        {
            this.Functions = new List<Function>(10);
        }

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public Event Copy()
        {
            Event copy = new Event();

            copy.EventType = this.EventType;
            foreach (Function functions in this.Functions)
            {
                copy.Functions.Add(functions.Copy());
            }

            return copy;
        }

        /// <summary>
        /// </summary>
        /// <param name="self">
        /// </param>
        /// <param name="caller">
        /// </param>
        public void Perform(ICharacter self, ICharacter caller)
        {
            foreach (Function functions in this.Functions)
            {
                bool result = true;
                foreach (Requirement requirements in functions.Requirements)
                {
                    result &= requirements.CheckRequirement(self);
                    if (!result)
                    {
                        break;
                    }
                }

                if (result)
                {
                    self.Controller.CallFunction(functions);
                }
            }
        }

        #endregion
    }
}