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

namespace Extractor_Serializer
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;

    using CellAO.Core.Items;
    using CellAO.Core.Statels;

    using Extractor_Serializer.Structs;

    #endregion

    /// <summary>
    /// </summary>
    public static class PlayfieldParser
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<Door> ParseDoors(byte[] buffer)
        {
            List<Door> result = new List<Door>();
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);

            // Drop first short
            br.ReadInt16();

            int count = br.ReadInt16();
            for (int cc = count; cc > 0; cc--)
            {
                Door newDoor = DoorExtract.ReadFromStream(br.BaseStream);
                result.Add(newDoor);
            }

            try
            {
                ms.Close();
                br.Close();
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<StatelData> ParseStatels(byte[] buffer)
        {
            List<StatelData> statels = new List<StatelData>();
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);
            int count = br.ReadInt32();
            while (count > 0)
            {
                int bufLen = br.ReadInt32();
                byte[] buf2 = br.ReadBytes(bufLen);
                MemoryStream ms2 = new MemoryStream(buf2);
                StatelData sd = StatelDataExtractor.ReadFromStream(ms2);
                ms2.Close();
                if (sd.Events.Count == 0)
                {
                    // Getting the events from the template item if no events are provided in the RDB record
                    if (ItemLoader.ItemList.ContainsKey(sd.TemplateId))
                    {
                        ItemTemplate template = ItemLoader.ItemList[sd.TemplateId];
                        if (template.Events.Count > 0)
                        {
                            sd.Events = template.Events;
                        }
                    }
                }

                statels.Add(sd);
                count--;
            }

            return statels;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="p">
        /// </param>
        /// <returns>
        /// </returns>
        internal static string ParseName(byte[] p)
        {
            MemoryStream ms = new MemoryStream(p);
            BinaryReader br = new BinaryReader(ms);
            br.ReadInt32();
            br.ReadInt32();
            string name = string.Empty;
            byte c = 0;
            while ((c = br.ReadByte()) != 0)
            {
                name += (char)c;
            }

            br.Close();
            ms.Close();
            return name;
        }

        #endregion
    }
}