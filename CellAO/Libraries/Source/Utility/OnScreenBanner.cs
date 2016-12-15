#region License

// Copyright (c) 2005-2016, CellAO Team
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

#endregion

namespace Utility
{
    #region Usings ...

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public static class OnScreenBanner
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="titleColor">
        /// </param>
        public static void PrintCellAOBanner(ConsoleColor titleColor)
        {
            int consoleWidth = Console.WindowWidth;
            bool sameWidth = consoleWidth == Console.BufferWidth;
            Console.Clear();

            Console.Write("**".PadRight(consoleWidth, '*'));
            if (!sameWidth)
            {
                Console.WriteLine();
            }

            CenteredString(string.Empty, "**");
            if (!sameWidth)
            {
                Console.WriteLine();
            }

            CenteredString("CellAO " + AssemblyInfoclass.Title, "**", titleColor);
            if (!sameWidth)
            {
                Console.WriteLine();
            }

            CenteredString(AssemblyInfoclass.AssemblyVersion, "**", ConsoleColor.White);
            if (!sameWidth)
            {
                Console.WriteLine();
            }

            CenteredString(AssemblyInfoclass.RevisionName, "**", ConsoleColor.DarkGray);
            if (!sameWidth)
            {
                Console.WriteLine();
            }

            CenteredString(string.Empty, "**");
            if (!sameWidth)
            {
                Console.WriteLine();
            }

            Console.Write("**".PadRight(consoleWidth, '*'));
            if (!sameWidth)
            {
                Console.WriteLine();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="boundary">
        /// </param>
        /// <param name="c">
        /// </param>
        private static void CenteredString(string text, string boundary, ConsoleColor c = ConsoleColor.Black)
        {
            int consoleWidth = Console.WindowWidth;

            // Mono "fix"
            if (consoleWidth == 0)
            {
                // Since Console.WindowWidth doesnt work on mono, lets assume 80 chars
                consoleWidth = 80;
            }

            int centered = (consoleWidth - text.Length) / 2;
            Console.Write(boundary.PadRight(centered, ' '));

            if (c != ConsoleColor.Black)
            {
                Colouring.Push(c);
            }

            Console.Write(text);
            Colouring.Pop();
            Console.Write(boundary.PadLeft(consoleWidth - (text.Length + centered), ' '));
        }

        #endregion
    }
}