#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-10-27 08:48
// Created:       2013-10-27 07:58

#endregion

namespace CellAO.Core.Functions
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    using MsgPack;

    #endregion

    /// <summary>
    /// Holder class for Function arguments
    /// </summary>
    [Serializable]
    public class FunctionArguments : IPackable, IUnpackable
    {
        #region Fields

        /// <summary>
        /// The function's arguments
        /// </summary>
        public List<object> Values = new List<object>();

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
            foreach (object obj in this.Values)
            {
                if (obj.GetType() == typeof(string))
                {
                    string temp = (string)obj;
                    packer.PackString(temp, Encoding.GetEncoding("UTF-8"));
                }
                else if (obj.GetType() == typeof(Single))
                {
                    float temp = (Single)obj;
                    packer.Pack<float>(temp);
                }
                else if (obj.GetType() == typeof(int))
                {
                    int temp = (Int32)obj;
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
            int numberOfItems = unpacker.Data.Value.AsInt32();

            while (numberOfItems > 0)
            {
                unpacker.ReadItem();

                if (unpacker.Data.Value.IsTypeOf(typeof(Int32)) == true)
                {
                    int temp = unpacker.Data.Value.AsInt32();
                    this.Values.Add(temp);
                }
                else if (unpacker.Data.Value.IsTypeOf(typeof(Single)) == true)
                {
                    float temp = unpacker.Data.Value.AsSingle();
                    this.Values.Add(temp);
                }
                else if (unpacker.Data.Value.IsTypeOf(typeof(String)) == true)
                {
                    string temp = unpacker.Data.Value.AsStringUtf8();
                    this.Values.Add(temp);
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