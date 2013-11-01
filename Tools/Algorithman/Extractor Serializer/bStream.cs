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
// Last modified: 2013-11-01 21:02

#endregion

namespace Extractor_Serializer
{
    #region Usings ...

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// The b stream.
    /// </summary>
    internal class bStream : BinaryReader
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="bStream"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public bStream(string fileName)
            : base(
                new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.RandomAccess)
                )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="bStream"/> class.
        /// </summary>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        public bStream(byte[] buffer)
            : base(new MemoryStream(buffer))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length
        {
            get
            {
                return (int)this.BaseStream.Length;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public uint Position
        {
            get
            {
                return (uint)this.BaseStream.Position;
            }

            set
            {
                this.BaseStream.Position = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The read int 32_ at.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int ReadInt32_At(uint position)
        {
            this.Position = position;
            return this.ReadInt32();
        }

        /// <summary>
        /// The read int 32_ msb.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int ReadInt32_MSB()
        {
            byte[] bytes = BitConverter.GetBytes(this.ReadInt32());
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// The read u int 32_ at.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// The <see cref="uint"/>.
        /// </returns>
        public uint ReadUInt32_At(uint position)
        {
            this.Position = position;
            return this.ReadUInt32();
        }

        #endregion
    }
}