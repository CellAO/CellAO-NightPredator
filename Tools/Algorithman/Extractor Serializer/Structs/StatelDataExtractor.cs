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

    using System.Diagnostics;
    using System.IO;
    using System.Net;

    using CellAO.Core.Statels;

    #endregion

    /// <summary>
    /// </summary>
    public static class StatelDataExtractor
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="stream">
        /// </param>
        /// <returns>
        /// </returns>
        public static StatelData ReadFromStream(Stream stream)
        {
            StatelData statel = new StatelData();
            FlatIdentity fi = new FlatIdentity();
            fi.ReadFromStream(stream);
            statel.StatelIdentity = fi.Id;

            // Skip 4, is always 1?
            stream.ReadByte();
            stream.ReadByte();
            stream.ReadByte();
            stream.ReadByte();
            PFCoordHeading pfc = new PFCoordHeading();
            pfc.ReadFromStream(stream);

            statel.X = pfc.Coordinate.X;
            statel.Y = pfc.Coordinate.Y;
            statel.Z = pfc.Coordinate.Z;

            statel.HeadingX = pfc.Heading.X;
            statel.HeadingY = pfc.Heading.Y;
            statel.HeadingZ = pfc.Heading.Z;
            statel.HeadingW = pfc.Heading.W;

            statel.PlayfieldId = pfc.PlayfieldIdentity;

            BinaryReader br = new BinaryReader(stream);
            br.ReadInt32();
            statel.TemplateId = br.ReadInt32();
            int len2 = br.ReadInt32();
            byte[] HighLow = br.ReadBytes(len2);
            MemoryStream ms = new MemoryStream(HighLow);

            BinaryReader br2 = new BinaryReader(ms);
            br2.ReadBytes(8);
            int C1 = IPAddress.NetworkToHostOrder(br2.ReadInt32());
            Debug.Assert(C1 % 0x3f1 == 0, "Wrong 3f1 encountered... stop please");

            int evcount = IPAddress.NetworkToHostOrder(br2.ReadInt32());
            while (evcount > 0)
            {
                int dataType = IPAddress.NetworkToHostOrder(br2.ReadInt32());
                switch (dataType)
                {
                    case 2:
                        HLFlatEvent flatEvent = new HLFlatEvent();
                        flatEvent.ReadFromStream(ms);
                        statel.Events.Add(flatEvent.ToEvents());
                        break;
                    default:

                        // Console.WriteLine("DataType " + dataType + " found... stop please");
                        break;
                }

                evcount--;
            }

            return statel;
        }

        #endregion
    }
}