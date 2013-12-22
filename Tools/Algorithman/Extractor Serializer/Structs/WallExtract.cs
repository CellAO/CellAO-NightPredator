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

    using System;
    using System.Diagnostics;
    using System.IO;

    using CellAO.Core.Statels;

    using NiceHexOutput;

    using zlib;

    #endregion

    /// <summary>
    /// </summary>
    public static class WallExtract
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="stream">
        /// </param>
        /// <returns>
        /// </returns>
        public static Walls ReadFromStream(Stream stream)
        {
            Walls wall = new Walls();
            BinaryReader br = new BinaryReader(stream);
            wall.Flags = br.ReadInt32();
            wall.Id = br.ReadInt32();
            byte c;
            string temp = string.Empty;
            while ((c = br.ReadByte()) != 0)
            {
                temp += (char)c;
            }

            // read rest of the 32 bytes reserved for name (-1 is for the already read 00)
            br.ReadBytes(32 - temp.Length - 1);

            br.ReadInt32(); // often playfield id, but not always (see Subway Junction, The Grid 2 and others)

            br.ReadInt32(); // 0x0A

            int counter1 = br.ReadInt32(); // unknown counter (districts?)
            br.ReadBytes(0x44);
            int lenCompressed = br.ReadInt32();
            int lenUnCompressed = br.ReadInt32();
            byte[] buffer = br.ReadBytes(lenCompressed - 4); // Just read for now, heightmap data probably

            MemoryStream ms = new MemoryStream();
            ZOutputStream zOutputStream = new ZOutputStream(ms);
            var xx = new MemoryStream(buffer);

            byte[] buffer2 = new byte[2097152];
            int len;
            while ((len = xx.Read(buffer2, 0, 2097152)) > 0)
            {
                zOutputStream.Write(buffer2, 0, len);
                Console.Write(
                    "\rDeflating " + Convert.ToInt32(Math.Floor((double)xx.Position / xx.Length * 100.0)) + "%");
            }

            zOutputStream.Flush();
            Console.Write("\r                                             \r");

            ms.Position = 0;
            Console.WriteLine(NiceHexOutput.Output(ms.GetBuffer()));
            Console.ReadLine();

            int x3f1 = br.ReadInt32();
            Debug.Assert(x3f1 % 1009 != 0);
            br.ReadBytes(0x2e);
            int counter2 = br.ReadInt32();
            while (counter2 > 0)
            {
                int lineId = br.ReadInt32();
                float X1 = br.ReadSingle();
                float Y1 = br.ReadSingle();
                float Z1 = br.ReadSingle();
                float X2 = br.ReadSingle();
                float Y2 = br.ReadSingle();
                float Z2 = br.ReadSingle();
            }

            return wall;
        }

        #endregion
    }
}