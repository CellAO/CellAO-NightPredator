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

namespace NiceHexOutput
{
    #region Usings ...

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public static class NiceHexOutput
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="packet">
        /// </param>
        /// <returns>
        /// </returns>
        public static string Output(byte[] packet)
        {
            if (packet == null)
            {
                return string.Empty;
            }

            string outp = string.Empty;
            int counter = 0;

            outp = "Packet length: " + packet.Length + Environment.NewLine;

            while (counter < packet.Length)
            {
                outp = outp + " ";
                if (packet.Length - counter > 16)
                {
                    byte[] temp = new byte[16];
                    Array.Copy(packet, counter, temp, 0, 16);
                    outp = outp + BitConverter.ToString(temp).Replace("-", " ").PadRight(52);
                    foreach (byte b in temp)
                    {
                        outp = outp + ToSafeAscii(b);
                    }

                    outp = outp + Environment.NewLine;
                }
                else
                {
                    byte[] temp = new byte[packet.Length - counter];
                    Array.Copy(packet, counter, temp, 0, packet.Length - counter);
                    outp = outp + BitConverter.ToString(temp).Replace("-", " ").PadRight(52);
                    foreach (byte b in temp)
                    {
                        outp = outp + ToSafeAscii(b);
                    }

                    outp = outp + Environment.NewLine;
                }

                counter += 16;
            }

            return outp;
        }

        /// <summary>
        /// </summary>
        /// <param name="b">
        /// </param>
        /// <returns>
        /// </returns>
        public static char ToSafeAscii(int b)
        {
            if (b >= 32 && b <= 126)
            {
                return (char)b;
            }

            return '.';
        }

        #endregion
    }
}