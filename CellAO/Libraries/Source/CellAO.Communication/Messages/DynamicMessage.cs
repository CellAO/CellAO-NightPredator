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
// Last modified: 2013-11-16 19:01

#endregion

namespace CellAO.Communication.Messages
{
    #region Usings ...

    using System;

    using MsgPack;
    using MsgPack.Serialization;

    #endregion

    /// <summary>
    /// Message object with dynamic content
    /// </summary>
    public class DynamicMessage : IPackable, IUnpackable
    {
        #region Fields

        /// <summary>
        /// The message data
        /// </summary>
        private MessageBase dataObject;

        /// <summary>
        /// Type name of the dataObject
        /// </summary>
        private string typeName;

        #endregion

        #region Public Properties

        /// <summary>
        /// Holds the actual data object (MessageBase or any derived objects)
        /// </summary>
        public MessageBase DataObject
        {
            get
            {
                return this.dataObject;
            }

            set
            {
                this.dataObject = value;
                this.typeName = value.GetType().ToString();
            }
        }

        /// <summary>
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.typeName;
            }

            set
            {
                this.typeName = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="packer">
        /// </param>
        /// <param name="options">
        /// </param>
        public void PackToMessage(Packer packer, PackingOptions options)
        {
            // Serialize type name of the data object
            packer.Pack(this.typeName);

            // Serialize the data object itself as a byte array
            packer.Pack(this.dataObject.GetData());
        }

        /// <summary>
        /// </summary>
        /// <param name="unpacker">
        /// </param>
        public void UnpackFromMessage(Unpacker unpacker)
        {
            // Read the type name
            this.typeName = unpacker.LastReadData.AsString();

            // Read the data object as byte array
            byte[] temp;
            unpacker.ReadBinary(out temp);

            // Create a message serializer object
            IMessagePackSingleObjectSerializer ser = MessagePackSerializer.Create(Type.GetType(this.typeName));

            // Unpack the message's data object
            this.dataObject = (MessageBase)ser.UnpackSingleObject(temp);
        }

        #endregion
    }
}