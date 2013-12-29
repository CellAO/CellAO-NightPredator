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
    using System.IO;

    using CellAO.Core.Playfields;

    #endregion

    /// <summary>
    /// </summary>
    public static class DestinationExtractor
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="stream">
        /// </param>
        /// <returns>
        /// </returns>
        public static DestinationStruct ReadFromStream(Stream stream)
        {
            DestinationStruct destinationStruct = new DestinationStruct();
            BinaryReader br = new BinaryReader(stream);
            destinationStruct.Version = br.ReadInt32();
            destinationStruct.Id = br.ReadInt32();
            byte c;
            string temp = string.Empty;
            int maxcount = 32;
            while (maxcount > 0)
            {
                c = br.ReadByte();
                if (c != 0)
                {
                    temp += (char)c;
                }

                if (temp.Length == 32)
                {
                    break;
                }

                maxcount--;
            }

            // Do the backward search
            br.BaseStream.Position = br.BaseStream.Length - 4;
            int destinationCounter;
            int realCounter = 0;
            while (true)
            {
                destinationCounter = br.ReadInt32();
                if (destinationCounter == realCounter)
                {
                    break;
                }

                realCounter++;
                br.BaseStream.Position = br.BaseStream.Position - (7 * 4) - 4;

                // -4 because we already read the 4 bytes to check if it is a counter
            }

            while (destinationCounter > 0)
            {
                // Read the stuff

                PlayfieldDestination pfd = new PlayfieldDestination();
                pfd.DestinationId = br.ReadInt32();
                pfd.StartX = br.ReadSingle();
                pfd.StartY = br.ReadSingle();
                pfd.StartZ = br.ReadSingle();

                pfd.EndX = br.ReadSingle();
                pfd.EndY = br.ReadSingle();
                pfd.EndZ = br.ReadSingle();

                destinationStruct.Destinations.Add(pfd);
                destinationCounter--;
            }

            // Should be on end position now
            Console.WriteLine(temp + ":" + br.BaseStream.Position);

            return destinationStruct;
        }

        #endregion
    }
}