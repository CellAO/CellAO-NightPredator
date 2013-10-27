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
// Last modified: 2013-10-26 22:26
// Created:       2013-10-26 22:15

#endregion

namespace CellAO.Core.Items
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;

    using MsgPack.Serialization;

    using zlib;

    #endregion

    /// <summary>
    /// Item template Loader/Cache class
    /// </summary>
    public static class ItemLoader
    {
        /// <summary>
        /// Cache of all item templates
        /// </summary>
        public static Dictionary<int, ItemTemplate> ItemList = new Dictionary<int, ItemTemplate>();

        /// <summary>
        /// Cache all item templates
        /// </summary>
        /// <returns>number of cached items</returns>
        public static int CacheAllItems()
        {
            return CacheAllItems("items.dat");
        }

        /// <summary>
        /// Cache all item templates
        /// </summary>
        /// <param name="fname">
        /// File to load from
        /// </param>
        /// <returns>
        /// Number of cached items
        /// </returns>
        public static int CacheAllItems(string fname)
        {
            Contract.Requires(!string.IsNullOrEmpty(fname));
            DateTime _now = DateTime.UtcNow;
            ItemList = new Dictionary<int, ItemTemplate>();
            Stream fileStream = new FileStream(fname, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream();

            ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
            CopyStream(fileStream, zOutputStream);

            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            byte versionlength = (byte)memoryStream.ReadByte();
            char[] version = new char[versionlength];
            version = binaryReader.ReadChars(versionlength);

            // TODO: Check version and print a warning if not same as config.xml's
            MessagePackSerializer<List<ItemTemplate>> messagePackSerializer =
                MessagePackSerializer.Create<List<ItemTemplate>>();

            var buffer = new byte[4];
            memoryStream.Read(buffer, 0, 4);
            int packaged = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Reading Items (" + new string(version) + "):");
            while (true)
            {
                try
                {
                    List<ItemTemplate> templates = messagePackSerializer.Unpack(memoryStream);
                    foreach (ItemTemplate template in templates)
                    {
                        ItemList.Add(template.ID, template);
                    }

                    if (templates.Count != packaged)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                Console.Write(
                    "Loaded {0} items in {1}\r",
                    new object[] { ItemList.Count, new DateTime((DateTime.UtcNow - _now).Ticks).ToString("mm:ss.ff") });
            }

            GC.Collect();
            return ItemList.Count;
        }

        /// <summary>
        /// Copy a stream to another stream with percentage output to console
        /// </summary>
        /// <param name="input">
        /// Input stream
        /// </param>
        /// <param name="output">
        /// Output stream
        /// </param>
        private static void CopyStream(Stream input, Stream output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "Input Stream must not be null");
            }

            if (output == null)
            {
                throw new ArgumentNullException("output", "Output Stream must not be null");
            }

            byte[] buffer = new byte[2097152];
            int len;
            while ((len = input.Read(buffer, 0, 2097152)) > 0)
            {
                output.Write(buffer, 0, len);
                Console.Write(
                    "\rDeflating " + Convert.ToInt32(Math.Floor((double)input.Position / input.Length * 100.0)) + "%");
            }

            output.Flush();
            Console.Write("\r                                             \r");
        }
    }
}