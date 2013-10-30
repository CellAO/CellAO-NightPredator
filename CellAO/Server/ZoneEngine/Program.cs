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
// Last modified: 2013-10-30 22:52
// Created:       2013-10-30 17:25

#endregion

namespace ZoneEngine
{
    #region Usings ...

    using System;

    using CellAO.Core.Items;
    using CellAO.Core.Nanos;
    using CellAO.Database;

    using Utility;

    #endregion

    internal class Program
    {
        private static bool LoadItemsAndNanos()
        {
            try
            {
                Console.WriteLine("Loaded {0} items", ItemLoader.CacheAllItems());
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured while loading the items.dat.");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            try
            {
                Console.WriteLine("Loaded {0} nanos", NanoLoader.CacheAllNanos());
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured while loading the nanos.dat.");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            return true;
        }

        private static bool CheckDatabase()
        {
            return Misc.CheckDatabase();
        }

        private static bool Initialize()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;

            if (!CheckDatabase())
            {
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            if (!LoadItemsAndNanos())
            {
                return false;
            }

            Console.ForegroundColor = ConsoleColor.White;

            return true;
        }

        private static void Main(string[] args)
        {
            ConsoleText ct = new ConsoleText();

            OnScreenBanner.PrintCellAOBanner(ConsoleColor.Green);

            Console.WriteLine();
            ct.TextRead("main.txt");

            if (!Initialize())
            {
                return;
            }
            Console.ReadLine();
        }
    }
}