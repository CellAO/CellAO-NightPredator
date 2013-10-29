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
// Last modified: 2013-10-29 22:29
// Created:       2013-10-29 19:57

#endregion

#region License

// Copyright (c) 2005-2012, CellAO Team
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
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

#region License

// Copyright (c) 2005-2012, CellAO Team
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
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
    using System.Globalization;
    using System.IO;
    using System.Text;

    using CellAO.Core.Items;
    using CellAO.Core.Nanos;

    using MsgPack.Serialization;

    using zlib;

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        #region Constants

        /// <summary>
        /// </summary>
        private const int CopyStreamBufferLength = 1 * 1024 * 1024; // 8 MB

        #endregion

        #region Static Fields

        /// <summary>
        /// The ext.
        /// </summary>
        private static Extractor extractor;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The copy stream.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[CopyStreamBufferLength];
            int len;
            while ((len = input.Read(buffer, 0, CopyStreamBufferLength)) > 0)
            {
                output.Write(buffer, 0, len);
                Console.Write(
                    "\rCompressing " + Convert.ToInt32(Math.Floor((double)input.Position / input.Length * 100.0)) + "%");
            }

            output.Flush();
        }

        /// <summary>
        /// The GetData.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="recordtype">
        /// The recordtype.
        /// </param>
        public static void GetData(string path, int recordtype)
        {
            int[] items = extractor.GetRecordInstances(recordtype);
            int cou = 0;
            foreach (int item in items)
            {
                var fileStream = new FileStream(
                    path + item.ToString(CultureInfo.InvariantCulture), FileMode.Create, FileAccess.Write);

                byte[] data = extractor.GetRecordData(recordtype, item);
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
                if (cou % 10 == 0)
                {
                    Console.WriteLine(item);
                }

                cou++;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("**                                                                  **");
            Console.WriteLine("**  AO Item and Nano Extractor/Serializer v0.85beta                  **");
            Console.WriteLine("**                                                                  **");
            Console.WriteLine("**********************************************************************");

            Console.WriteLine();

            string AOPath = string.Empty;
            bool foundAO = false;
            Console.WriteLine("Enter exit to close program");
            while (!foundAO)
            {
                if (File.Exists("config.txt"))
                {
                    TextReader tr = new StreamReader("config.txt");
                    AOPath = tr.ReadLine();
                    tr.Close();
                }

                foundAO = false;
                Console.Write("Please enter your AO Install Path [" + AOPath + "]:");
                string temp = Console.ReadLine();
                if (temp != string.Empty)
                {
                    AOPath = temp;
                }

                if (temp.ToLower() == "exit")
                {
                    return;
                }

                if (!Directory.Exists(AOPath))
                {
                    continue;
                }

                try
                {
                    extractor = new Extractor(AOPath);
                    TextWriter tw2 = new StreamWriter("config.txt", false, Encoding.GetEncoding("windows-1252"));
                    tw2.WriteLine(AOPath);
                    tw2.Close();
                    foundAO = true;
                    Console.WriteLine("Found AO Database on " + AOPath);
                }
                catch (Exception)
                {
                    foundAO = false;
                }

                // Try to add cd_image\data\db
                if (!foundAO)
                {
                    try
                    {
                        AOPath = Path.Combine(AOPath, "cd_image\\data\\db");
                        extractor = new Extractor(AOPath);
                        TextWriter tw2 = new StreamWriter("config.txt", false, Encoding.GetEncoding("windows-1252"));
                        tw2.WriteLine(AOPath);
                        tw2.Close();
                        foundAO = true;
                        Console.WriteLine("Found AO Database on " + AOPath);
                    }
                    catch (Exception)
                    {
                        foundAO = false;
                    }
                }
            }

            TextWriter tw = new StreamWriter("itemnames.sql", false, Encoding.GetEncoding("windows-1252"));
            tw.WriteLine("DROP TABLE IF EXISTS `itemnames`;");
            tw.WriteLine("CREATE TABLE `itemnames` (");
            tw.WriteLine("  `AOID` int(10) NOT NULL,");
            tw.WriteLine("  `Name` varchar(250) NOT NULL,");
            tw.WriteLine("  PRIMARY KEY (`AOID`)");
            tw.WriteLine(") ENGINE=MyIsam DEFAULT CHARSET=latin1;");
            tw.WriteLine();
            tw.Close();

            Console.WriteLine("Number of Items to extract: " + extractor.GetRecordInstances(0xF4254).Length);

            // ITEM RECORD TYPE
            Console.WriteLine("Number of Nanos to extract: " + extractor.GetRecordInstances(0xFDE85).Length);

            // NANO RECORD TYPE

            // Console.WriteLine(extractor.GetRecordInstances(0xF4241).Length); // Playfields
            // Console.WriteLine(extractor.GetRecordInstances(0xF4266).Length); // Nano Strains
            // Console.WriteLine(extractor.GetRecordInstances(0xF4264).Length); // Perks

            // GetData(@"D:\c#\extractor serializer\data\items\",0xf4254);
            // GetData(@"D:\c#\extractor serializer\data\nanos\",0xfde85);
            // GetData(@"D:\c#\extractor serializer\data\playfields\",0xf4241);
            // GetData(@"D:\c#\extractor serializer\data\nanostrains\",0xf4266);
            // GetData(@"D:\c#\extractor serializer\data\perks\",0xf4264);
            var np = new NewParser();
            var rawItemList = new List<ItemTemplate>();
            var rawNanoList = new List<NanoFormula>();
            int counter = 0;
            foreach (int recnum in extractor.GetRecordInstances(0xFDE85))
            {
                if (counter == 0)
                {
                    counter = recnum;
                }

                rawNanoList.Add(np.ParseNano(0xFDE85, recnum, extractor.GetRecordData(0xFDE85, recnum), "temp.sql"));
                if ((counter % 1000) == 0)
                {
                    Console.Write("\rNano ID: " + recnum.ToString().PadLeft(9));
                }

                counter++;
            }

            Console.Write("\rNano ID: " + rawNanoList[rawNanoList.Count - 1].ID.ToString().PadLeft(9));

            File.Delete("temp.sql");
            Console.WriteLine();
            Console.WriteLine("Nanos extracted: " + rawNanoList.Count);

            List<string> ItemNamesSql = new List<string>();

            counter = 0;
            foreach (int recnum in extractor.GetRecordInstances(0xF4254))
            {
                Console.Write("\rItem ID: " + recnum.ToString().PadLeft(9));
                rawItemList.Add(np.ParseItem(0xF4254, recnum, extractor.GetRecordData(0xF4254, recnum), ItemNamesSql));
                if ((counter % 1000) == 0)
                {
                    Console.Write("\rItem ID: " + recnum.ToString().PadLeft(9));
                }

                counter++;
            }

            Console.Write("\rItem ID: " + rawItemList[rawItemList.Count - 1].ID.ToString().PadLeft(9));

            Console.WriteLine();
            Console.WriteLine("Items extracted: " + rawItemList.Count);

            Console.WriteLine();
            Console.WriteLine("Compacting itemnames.sql");
            TextWriter itnsql = new StreamWriter("itemnames.sql", true, Encoding.GetEncoding("windows-1252"));
            while (ItemNamesSql.Count > 0)
            {
                int count = 0;
                string toWrite = string.Empty;
                while ((count < 20) && (ItemNamesSql.Count > 0))
                {
                    if (toWrite.Length > 0)
                    {
                        toWrite += ",";
                    }

                    toWrite += ItemNamesSql[0];
                    ItemNamesSql.RemoveAt(0);
                    count++;
                }

                if (toWrite != string.Empty)
                {
                    itnsql.WriteLine("INSERT INTO itemnames VALUES " + toWrite + ";");
                }
            }

            itnsql.Close();

            // SerializationContext.Default.Serializers.Register(new AOFunctionArgumentsSerializer());
            Console.WriteLine();
            Console.WriteLine("Items extracted: " + rawItemList.Count);

            Console.WriteLine();
            Console.WriteLine("Creating serialized nano data file - please wait");
            Stream sf = new FileStream("nanos.dat", FileMode.Create);

            var ds = new ZOutputStream(sf, zlibConst.Z_BEST_COMPRESSION);
            var sm = new MemoryStream();
            MessagePackSerializer<List<NanoFormula>> bf = MessagePackSerializer.Create<List<NanoFormula>>();

            var nanoList2 = new List<NanoFormula>();

            int maxnum = 5000;
            string version = GetVersion(AOPath);
            if (version == "")
            {
                return;
            }
            byte[] versionbuffer = Encoding.ASCII.GetBytes(version);
            sm.WriteByte((byte)(versionbuffer.Length));
            sm.Write(versionbuffer, 0, versionbuffer.Length);

            byte[] buffer = BitConverter.GetBytes(maxnum);
            sm.Write(buffer, 0, buffer.Length);

            foreach (NanoFormula nanos in rawNanoList)
            {
                nanoList2.Add(nanos);
                if (nanoList2.Count == maxnum)
                {
                    bf.Pack(sm, nanoList2);
                    sm.Flush();
                    nanoList2.Clear();
                }
            }

            bf.Pack(sm, nanoList2);
            sm.Seek(0, SeekOrigin.Begin);
            CopyStream(sm, ds);
            sm.Close();
            ds.Close();

            Console.WriteLine();
            Console.WriteLine("Checking Nanos...");
            Console.WriteLine();
            NanoLoader.CacheAllNanos("nanos.dat");
            Console.WriteLine();
            Console.WriteLine("Nanos: " + NanoLoader.NanoList.Count + " successfully converted");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Creating serialized item data file - please wait");

            sf = new FileStream("items.dat", FileMode.Create);

            ds = new ZOutputStream(sf, zlibConst.Z_BEST_COMPRESSION);
            sm = new MemoryStream();
            MessagePackSerializer<List<ItemTemplate>> bf2 = MessagePackSerializer.Create<List<ItemTemplate>>();

            List<ItemTemplate> items = new List<ItemTemplate>();

            maxnum = 5000;
            sm.WriteByte((byte)(versionbuffer.Length));
            sm.Write(versionbuffer, 0, versionbuffer.Length);

            buffer = BitConverter.GetBytes(maxnum);
            sm.Write(buffer, 0, buffer.Length);

            foreach (ItemTemplate it in rawItemList)
            {
                items.Add(it);
                if (items.Count == maxnum)
                {
                    bf2.Pack(sm, items);
                    sm.Flush();
                    items.Clear();
                }
            }

            bf2.Pack(sm, items);
            sm.Seek(0, SeekOrigin.Begin);
            CopyStream(sm, ds);
            sm.Close();
            ds.Close();

            Console.WriteLine();
            Console.WriteLine("Checking Items...");
            Console.WriteLine();

            ItemLoader.CacheAllItems("items.dat");

            Console.WriteLine();
            Console.WriteLine("Items: " + ItemLoader.ItemList.Count + " successfully converted");

            Console.WriteLine();
            Console.WriteLine("Further Instructions:");
            Console.WriteLine("- Copy items.dat and nanos.dat into your CellAO folder and overwrite.");
            Console.WriteLine("- Apply itemnames.sql to your database");
            Console.WriteLine();
            Console.WriteLine("   OR   ");
            Console.WriteLine();
            Console.WriteLine("Let me copy it over to the Source Tree");
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Please choose:");
                Console.WriteLine("1: Copy the files to CellAO/Datafiles and CellAO/.../CellAO.Database/SqlTables.");
                Console.WriteLine("2: Exit and copy yourself");
                Console.WriteLine("[1,2]:");
                string line = Console.ReadLine();
                if (line.Trim() == "1")
                {
                    if (CopyDatafiles())
                    {
                        break;
                    }
                }
                if (line.Trim() == "2")
                {
                    break;
                }
            }
        }

        private static bool CopyDatafiles()
        {
            bool result = true;
            // Presume we are in CellAO/Built/Debug or Release
            string pathToDatafiles = Path.Combine("..", "..", "Datafiles");
            if (File.Exists(Path.Combine(pathToDatafiles, "items.dat")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "items.dat"));
                File.Copy("items.dat", Path.Combine(pathToDatafiles, "items.dat"));
            }
            else
            {
                Console.WriteLine("Could not find old items.dat in the Datafiles folder.");
                result = false;
            }

            if (File.Exists(Path.Combine(pathToDatafiles, "nanos.dat")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "nanos.dat"));
                File.Copy("nanos.dat", Path.Combine(pathToDatafiles, "nanos.dat"));
            }
            else
            {
                Console.WriteLine("Could not find old nanos.dat in the Datafiles folder.");
                result = false;
            }

            pathToDatafiles = Path.Combine("..", "..", "Libraries", "Source", "CellAO.Database", "SqlTables");
            if (File.Exists(Path.Combine(pathToDatafiles, "itemnames.sql")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "itemnames.sql"));
                File.Copy("itemnames.sql", Path.Combine(pathToDatafiles, "itemnames.sql"));
            }
            else
            {
                Console.WriteLine("Could not find old itemnames.sql in the SqlTables folder.");
                result = false;
            }
            return result;
        }

        public static string GetVersion(string path)
        {
            string aopath = path;
            try
            {
                while (!File.Exists(Path.Combine(aopath, "version.id")))
                {
                    aopath = Path.Combine(aopath, "..");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("File 'version.id' not found.");
                Console.WriteLine("Plese press <Enter> to exit.");
                Console.ReadLine();
                return "";
            }
            TextReader tr = new StreamReader(Path.Combine(aopath, "version.id"));

            string line = tr.ReadToEnd().Trim().Trim('\r').Trim('\n');
            tr.Close();
            return line;
        }

        #endregion
    }
}