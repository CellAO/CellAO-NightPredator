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

namespace Extractor_Serializer.Structs
{
    #region Usings ...

    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using CellAO.Core.Events;
    using CellAO.Enums;
    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class HLFlatEvent : IFCSerializable
    {
        #region Fields

        /// <summary>
        /// </summary>
        public int EventType = 0;

        /// <summary>
        /// </summary>
        public List<HLFlatFunction> Functions = new List<HLFlatFunction>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="stream">
        /// </param>
        public void ReadFromStream(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);

            this.EventType = IPAddress.NetworkToHostOrder(br.ReadInt32());
            var counter = new HLFlat3F1Counter();
            counter.ReadFromStream(stream);
            int count = counter.Counter;
            while (count > 0)
            {
                HLFlatFunction func = new HLFlatFunction();
                func.ReadFromStream(stream);
                this.Functions.Add(func);
                count--;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal Event ToEvents()
        {
            Event ev = new Event();
            ev.EventType = (EventType) Enum.ToObject(typeof(EventType), this.EventType);
            foreach (HLFlatFunction flf in this.Functions)
            {
                ev.Functions.Add(flf.ToFunctions());
            }

            return ev;
        }

        #endregion
    }
}