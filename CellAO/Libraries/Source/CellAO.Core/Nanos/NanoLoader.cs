﻿#region License

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

namespace CellAO.Core.Nanos
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;

    using locales;

    using MsgPack.Serialization;

    using zlib;

    #endregion

    /// <summary>
    /// Item handler class
    /// </summary>
    public class NanoLoader
    {
        #region Static Fields

        /// <summary>
        /// Cache of all item templates
        /// </summary>
        public static Dictionary<int, NanoFormula> NanoList = new Dictionary<int, NanoFormula>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Cache all item templates
        /// </summary>
        /// <returns>number of cached items</returns>
        public static int CacheAllNanos()
        {
            return CacheAllNanos("nanos.dat");
        }

        /// <summary>
        /// Cache all item templates
        /// </summary>
        /// <param name="fname">
        /// </param>
        /// <returns>
        /// number of cached items
        /// </returns>
        public static int CacheAllNanos(string fname)
        {
            Contract.Requires(!string.IsNullOrEmpty(fname));
            DateTime _now = DateTime.Now;
            NanoList = new Dictionary<int, NanoFormula>();
            Stream sf = new FileStream(fname, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream();

            ZOutputStream sm = new ZOutputStream(memoryStream);
            CopyStream(sf, sm);

            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(memoryStream);
            byte versionlength = (byte)memoryStream.ReadByte();
            char[] version = new char[versionlength];
            version = br.ReadChars(versionlength);

            // TODO: Check version and print a warning if not same as config.xml's
            Console.WriteLine("Reading Nanos (" + new string(version) + "):");

            MessagePackSerializer<NanoFormula> messagePackSerializer = MessagePackSerializer.Create<NanoFormula>();

            byte[] buffer = new byte[4];
            memoryStream.Read(buffer, 0, 4);
            int packaged = BitConverter.ToInt32(buffer, 0);

            while (true)
            {
                try
                {
                    List<NanoFormula> templist = new List<NanoFormula>();

                    for (int i = packaged; i > 0; i++)
                    {
                        NanoFormula nanoFormula = messagePackSerializer.Unpack(memoryStream);
                        templist.Add(nanoFormula);
                        if (nanoFormula == null)
                        {
                            break;
                        }

                        NanoList.Add(nanoFormula.ID, nanoFormula);
                    }

                    if (templist.Count != packaged)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                Console.Write(
                    locales.NanoLoaderLoadedNanos + " - {1}\r", 
                    new object[] { NanoList.Count, new DateTime((DateTime.Now - _now).Ticks).ToString("mm:ss.ff") });
            }

            Console.Write(
                locales.NanoLoaderLoadedNanos + " - {1}\r", 
                new object[] { NanoList.Count, new DateTime((DateTime.Now - _now).Ticks).ToString("mm:ss.ff") });

            GC.Collect();
            return NanoList.Count;
        }

        /// <summary>
        /// Returns a nano object
        /// </summary>
        /// <param name="id">
        /// ID of the nano
        /// </param>
        /// <returns>
        /// Nano
        /// </returns>
        public static NanoFormula GetNano(int id)
        {
            if (!NanoList.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException("No NanoFormula found with ID " + id);
            }

            return NanoList[id];
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="input">
        /// </param>
        /// <param name="output">
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

        #endregion
    }
}