#region License

// Copyright (c) 2005-2014, CellAO Team
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
    using System.Runtime.Serialization;

    using CellAO.Interfaces;

    using MsgPack;

    #endregion

    /// <summary>
    /// Holder class for Function arguments
    /// </summary>
    [Serializable]
    public class FunctionArguments : IPackable, IUnpackable, IFunctionArguments
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public FunctionArguments()
        {
            this.Values = new List<MessagePackObject>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The function's arguments
        /// </summary>
        public List<MessagePackObject> Values { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Use msgpack to compress the data
        /// </summary>
        /// <param name="packer">
        /// The msgpack packer
        /// </param>
        /// <param name="options">
        /// msgpack packing options
        /// </param>
        public void PackToMessage(Packer packer, PackingOptions options)
        {
            packer.PackArrayHeader(this.Values.Count);
            foreach (MessagePackObject obj in this.Values)
            {
                if (obj.IsTypeOf(typeof(string)) == true)
                {
                    string temp = obj.AsStringUtf8();
                    packer.PackString(temp);
                }
                else if (obj.IsTypeOf(typeof(Single)) == true)
                {
                    float temp = obj.AsSingle();
                    packer.Pack<float>(temp);
                }
                else if (obj.IsTypeOf(typeof(int)) == true)
                {
                    int temp = obj.AsInt32();
                    packer.Pack(temp);
                }
            }
        }

        /// <summary>
        /// Unpack from msgpack'd stream
        /// </summary>
        /// <param name="unpacker">
        /// The msgpack unpacker
        /// </param>
        /// <exception cref="SerializationException">
        /// Unsuitable data encountered
        /// </exception>
        public void UnpackFromMessage(Unpacker unpacker)
        {
            int numberOfItems = unpacker.LastReadData.AsInt32();
            this.Values = new List<MessagePackObject>(numberOfItems);

            while (numberOfItems > 0)
            {
                unpacker.ReadItem();

                if (unpacker.LastReadData.IsTypeOf(typeof(Int32)) == true)
                {
                    this.Values.Add(unpacker.LastReadData.AsInt32());
                }
                else if (unpacker.LastReadData.IsTypeOf(typeof(Single)) == true)
                {
                    this.Values.Add(unpacker.LastReadData.AsSingle());
                }
                else if (unpacker.LastReadData.IsTypeOf(typeof(string)) == true)
                {
                    this.Values.Add(unpacker.LastReadData.AsStringUtf8());
                }
                else
                {
                    throw new SerializationException("Unpacker found no suitable data inside Function Arguments!");
                }

                numberOfItems--;
            }
        }

        #endregion
    }
}