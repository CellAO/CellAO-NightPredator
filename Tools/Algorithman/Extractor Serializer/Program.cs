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
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.Text;
    using System.Text.RegularExpressions;

    using CellAO.Core.Items;
    using CellAO.Core.Nanos;
    using CellAO.Core.Playfields;

    using Utility;

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
        /// </summary>
        public static List<List<int>> Relations = new List<List<int>>(100000);

        /// <summary>
        /// </summary>
        public static Regex reg = new Regex(@".*\/item\/([0-9]*)\/.*");

        /// <summary>
        /// </summary>
        public static WebClient webClient = new WebClient();

        /// <summary>
        /// The ext.
        /// </summary>
        private static Extractor extractor;

        /// <summary>
        /// </summary>
        private static Dictionary<int, ItemTemplate> rawItemDictionary = new Dictionary<int, ItemTemplate>(130000);

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
        public static void GetData(string path, Extractor.RecordType recordtype)
        {
            int[] items = extractor.GetRecordInstances(recordtype);
            int cou = 0;
            foreach (int item in items)
            {
                try
                {

                    using (
                        var fileStream = new FileStream(Path.Combine(
                            path, item.ToString(CultureInfo.InvariantCulture)),
                            FileMode.Create,
                            FileAccess.Write))
                    {
                        byte[] data = extractor.GetRecordData(recordtype, item);

                        fileStream.Write(data, 0, data.Length);
                    }
                    if (cou % 10 == 0)
                    {
                        Console.WriteLine(item);
                    }

                    cou++;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
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
                return string.Empty;
            }

            TextReader tr = new StreamReader(Path.Combine(aopath, "version.id"));

            string line = tr.ReadToEnd().Trim().Trim('\r').Trim('\n');
            tr.Close();
            return line;
        }

        /// <summary>
        /// </summary>
        public static void ReadItemRelations()
        {
            TextReader tr = new StreamReader("itemrelations.txt");
            string line;
            string lastline = null;
            while ((line = tr.ReadLine()) != null)
            {
                if (line != lastline)
                {
                    string[] rels = line.Split(' ');
                    List<int> temp = new List<int>();
                    foreach (string r in rels)
                    {
                        temp.Add(int.Parse(r));
                    }

                    Relations.Add(temp);
                }

                lastline = line;
            }

            tr.Close();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="ItemNamesSql">
        /// </param>
        private static void CompactingItemNamesSql(List<string> ItemNamesSql)
        {
            Console.WriteLine();
            Console.WriteLine("Compacting itemnames.sql");
            TextWriter itnsql = new StreamWriter("itemnames.sql", true, Encoding.GetEncoding("windows-1252"));
            StringBuilder bb = new StringBuilder(51);
            bool hasData = false;
            while (ItemNamesSql.Count > 0)
            {
                int count = 0;
                while ((count < 100) && (ItemNamesSql.Count > 0))
                {
                    if (hasData)
                    {
                        bb.Append(", ");
                    }

                    hasData = true;
                    bb.Append(ItemNamesSql[ItemNamesSql.Count - 1]);
                    ItemNamesSql.RemoveAt(ItemNamesSql.Count - 1);
                    count++;
                }

                if (hasData)
                {
                    itnsql.WriteLine("INSERT INTO itemnames VALUES " + bb.ToString() + ";");
                    bb.Clear();
                    hasData = false;
                }
            }

            itnsql.Close();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool CopyDatafiles()
        {
            bool result = true;

            // Presume we are in CellAO/Built/Debug or Release
            string pathToDatafiles = Path.Combine("..", "..", "Datafiles");
            if (File.Exists(Path.Combine(pathToDatafiles, "items.dat")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "items.dat"));
            }

            File.Copy("items.dat", Path.Combine(pathToDatafiles, "items.dat"));

            if (File.Exists(Path.Combine(pathToDatafiles, "nanos.dat")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "nanos.dat"));
            }

            File.Copy("nanos.dat", Path.Combine(pathToDatafiles, "nanos.dat"));

            if (File.Exists(Path.Combine(pathToDatafiles, "playfields.dat")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "playfields.dat"));
            }

            File.Copy("playfields.dat", Path.Combine(pathToDatafiles, "playfields.dat"));

            if (File.Exists(Path.Combine(pathToDatafiles, "itemrelations.txt")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "itemrelations.txt"));
            }

            File.Copy("itemrelations.txt", Path.Combine(pathToDatafiles, "itemrelations.txt"));

            pathToDatafiles = Path.Combine("..", "..", "Libraries", "Source", "CellAO.Database", "SqlTables");
            if (File.Exists(Path.Combine(pathToDatafiles, "itemnames.sql")))
            {
                File.Delete(Path.Combine(pathToDatafiles, "itemnames.sql"));
            }

            File.Copy("itemnames.sql", Path.Combine(pathToDatafiles, "itemnames.sql"));

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="itemNamesSqls">
        /// </param>
        /// <returns>
        /// </returns>
        private static List<ItemTemplate> ExtractItemTemplates(List<string> itemNamesSqls)
        {
            var np = new NewParser();
            int[] instances = extractor.GetRecordInstances(Extractor.RecordType.Item);
            List<ItemTemplate> rawItemList = new List<ItemTemplate>(instances.Length);
            rawItemDictionary = new Dictionary<int, ItemTemplate>(instances.Length);
            int counter = 0;
            foreach (int recnum in instances)
            {
                byte[] data = extractor.GetRecordData(Extractor.RecordType.Item, recnum);
                ItemTemplate xt = np.ParseItem(Extractor.RecordType.Item, recnum, data, itemNamesSqls);

                rawItemList.Add(xt);
                rawItemDictionary.Add(recnum, xt);

                if ((counter % 7500) == 0)
                {
                    Console.Write("\rItem ID: " + recnum.ToString().PadLeft(9));
                }

                counter++;
            }

            Console.Write("\rItem ID: " + rawItemList[rawItemList.Count - 1].ID.ToString().PadLeft(9));

            Console.WriteLine();
            return rawItemList;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static List<PlayfieldData> ExtractPlayfieldData()
        {
            List<PlayfieldData> playfields = new List<PlayfieldData>(700);
            int[] instances = extractor.GetRecordInstances(Extractor.RecordType.Playfield);
            foreach (int recnum in instances)
            {
                PlayfieldData pf = new PlayfieldData();
                pf.PlayfieldId = recnum;

                // Console.WriteLine("Parsing PF " + recnum);
                int[] doors = extractor.GetRecordInstances(Extractor.RecordType.Door);
                if (doors.Contains(recnum))
                {
                    byte[] doorData = extractor.GetRecordData(Extractor.RecordType.Door, recnum);
                    pf.Doors1 = PlayfieldParser.ParseDoors(doorData);
                }

                pf.Name = PlayfieldParser.ParseName(extractor.GetRecordData(Extractor.RecordType.Playfield, recnum));
                pf.Destinations = PlayfieldParser.ParseDestinations(extractor.GetRecordData(Extractor.RecordType.Playfield, recnum)).Destinations;

                if (extractor.GetRecordInstances(Extractor.RecordType.Wall).Contains(recnum))
                {
                    pf.Walls = PlayfieldParser.ParseWalls(extractor.GetRecordData(Extractor.RecordType.Wall, recnum));
                }

                playfields.Add(pf);
            }

            return playfields;
        }

        /// <summary>
        /// </summary>
        /// <param name="playfields">
        /// </param>
        private static void ExtractPlayfieldStatels(List<PlayfieldData> playfields)
        {
            foreach (int recnum in extractor.GetRecordInstances(Extractor.RecordType.Statel)) // statels
            {
                Console.Write("Parsing Statels for playfield " + recnum + "\r");

                if (playfields.Any(x => x.PlayfieldId == recnum))
                {
                    playfields.First(x => x.PlayfieldId == recnum)
                        .Statels.AddRange(
                            PlayfieldParser.ParseStatels(
                                extractor.GetRecordData(Extractor.RecordType.Statel, recnum)
                                )

                        /* .Where(x => x.Events.Count > 0)*/

                        );
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static string GetAOPath()
        {
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
                    return string.Empty;
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    foundAO = false;
                }

                // Try to add cd_image\data\db
                if (!foundAO)
                {
                    try
                    {
                        AOPath = Path.Combine(AOPath, "cd_image", "data", "db");
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

            return AOPath;
        }

        /// <summary>
        /// </summary>
        /// <param name="template">
        /// </param>
        private static void GetItemRelations(ItemTemplate template)
        {
            try
            {
                string html = webClient.DownloadString("http://www.aoitems.com/item/" + template.ID + "/");
                int pos;
                if ((pos = html.IndexOf("<select class=\"TemplateSelector\">")) != -1)
                {
                    // found template selector
                    // now narrow down to the links
                    html = html.Substring(pos + 33);
                    html = html.Substring(0, html.IndexOf("</select"));
                    foreach (Match r in reg.Matches(html))
                    {
                        int id = int.Parse(r.Groups[1].Value);
                        template.Relations.Add(id);
                    }
                }
                else
                {
                    template.Relations.Add(template.ID);
                }
            }
            catch (Exception)
            {
                template.Relations.Add(template.ID);
            }
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            OnScreenBanner.PrintCellAOBanner(ConsoleColor.White);

            Console.WriteLine();

            string AOPath = GetAOPath();
            if (AOPath == string.Empty)
            {
                // Exit
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Loading item relations...");
            ReadItemRelations();

            PrepareItemNamesSQL();

            Console.WriteLine("Number of Items to extract: " + extractor.GetRecordInstances(Extractor.RecordType.Item).Length);

            // ITEM RECORD TYPE
            Console.WriteLine("Number of Nanos to extract: " + extractor.GetRecordInstances(Extractor.RecordType.Nano).Length);
            Console.WriteLine();

            // NANO RECORD TYPE

            // Console.WriteLine(extractor.GetRecordInstances(0xF4241).Length); // Playfields
            // Console.WriteLine(extractor.GetRecordInstances(0xF4266).Length); // Nano Strains
            // Console.WriteLine(extractor.GetRecordInstances(0xF4264).Length); // Perks

            // GetData(@"D:\c#\extractor serializer\data\items\",0xf4254);
            // GetData(@"D:\c#\extractor serializer\data\nanos\",0xfde85);
            // GetData(@"D:\c#\extractor serializer\data\playfields\",0xf4241);
            // GetData(@"D:\c#\extractor serializer\data\nanostrains\",0xf4266);
            // GetData(@"D:\c#\extractor serializer\data\perks\",0xf4264);

            Console.WriteLine();
            List<NanoFormula> rawNanoList = ReadNanoFormulas();
            Console.WriteLine();
            Console.WriteLine("Nanos extracted: " + rawNanoList.Count);
            Console.WriteLine();

            List<string> ItemNamesSql = new List<string>(extractor.GetRecordInstanceCount(0xF4254));
            List<ItemTemplate> rawItemList = ExtractItemTemplates(ItemNamesSql);

            Console.WriteLine("Items extracted: " + rawItemList.Count);

            SetItemRelations(rawItemList);

            CompactingItemNamesSql(ItemNamesSql);

            // SerializationContext.Default.Serializers.Register(new AOFunctionArgumentsSerializer());
            Console.WriteLine();
            Console.WriteLine("Items extracted: " + rawItemList.Count);

            Console.WriteLine();
            Console.WriteLine("Creating serialized nano data file - please wait");

            string version = GetVersion(AOPath);
            MessagePackZip.CompressData<NanoFormula>("nanos.dat", version, rawNanoList, 1000);

            Console.WriteLine();
            Console.WriteLine("Checking Nanos...");
            Console.WriteLine();
            NanoLoader.CacheAllNanos("nanos.dat");
            Console.WriteLine();
            Console.WriteLine("Nanos: " + NanoLoader.NanoList.Count + " successfully converted");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Creating serialized item data file - please wait");

            MessagePackZip.CompressData<ItemTemplate>("items.dat", GetVersion(AOPath), rawItemList, 10240);

            Console.WriteLine();
            Console.WriteLine("Checking Items...");
            Console.WriteLine();

            ItemLoader.CacheAllItems("items.dat");

            Console.WriteLine();
            Console.WriteLine("Items: " + ItemLoader.ItemList.Count + " successfully converted");

            Console.WriteLine("Extracting playfield walls/destinations/statels");
            List<PlayfieldData> playfields = ExtractPlayfieldData();
            ExtractPlayfieldStatels(playfields);
            Console.WriteLine();
            Console.WriteLine("Compressing playfield data...");
            MessagePackZip.CompressData<PlayfieldData>("playfields.dat", GetVersion(AOPath), playfields);
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Further Instructions:");
            Console.WriteLine(
                "- Copy items.dat, nanos.dat and playfields.dat into your CellAO/Datafiles folder and overwrite.");
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

            while (true)
            {
                Console.WriteLine("Do you want to extract the icons for WebCore? [Y/N]");
                string line = Console.ReadLine();
                if (line.Trim().ToLower() == "y")
                {
                    ExtractIcons();
                    break;
                }
                if (line.Trim().ToLower() == "n")
                {
                    break;
                }
            }

            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }

        private static void ExtractIcons()
        {
            if (!Directory.Exists("icons"))
            {
                Directory.CreateDirectory("icons");
            }

            // Delete all pngs in that folder (makes conversion much faster)
            string[] filesToDelete = Directory.GetFiles("icons", "*.png", SearchOption.TopDirectoryOnly);
            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }

            int GCcount = 0;
            foreach (ItemTemplate template in ItemLoader.ItemList.Values)
            {
                string pngName = Path.Combine("icons", template.getItemAttribute(79) + ".png");
                if (!File.Exists(pngName))
                {
                    byte[] icon;
                    try
                    {
                        icon = extractor.GetRecordData(Extractor.RecordType.Icon, template.getItemAttribute(79));
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    FileStream fs = new FileStream(
                        pngName,
                        FileMode.Create,
                        FileAccess.ReadWrite);
                    fs.Write(icon, 0, icon.Length);
                    fs.Close();

                    MakeTransparent(pngName);
                    GCcount++;
                    if (GCcount % 100 == 0)
                    {
                        GC.Collect();
                    }
                }
            }
        }

        private static void MakeTransparent(string p)
        {
            FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read);
            using (Image original = new Bitmap(fs))
            {
                using (Bitmap image = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb))
                {
                    Graphics g = Graphics.FromImage(image);
                    ImageAttributes ia = new ImageAttributes();
                    ia.SetColorKey(Color.FromArgb(0, 0xde, 0), Color.FromArgb(33, 255, 5));
                    g.DrawImage(
                        original,
                        new Rectangle(0, 0, original.Width, original.Height),
                        0,
                        0,
                        original.Width,
                        original.Height,
                        GraphicsUnit.Pixel,
                        ia);
                    g.Dispose();
                    ia.Dispose();
                    image.Save(p, ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// </summary>
        private static void PrepareItemNamesSQL()
        {
            TextWriter tw = new StreamWriter("itemnames.sql", false, Encoding.GetEncoding("windows-1252"));
            tw.WriteLine("DROP TABLE IF EXISTS `itemnames`;");
            tw.WriteLine("CREATE TABLE `itemnames` (");
            tw.WriteLine("  `Id` int(10) NOT NULL,");
            tw.WriteLine("  `Name` varchar(250) NOT NULL,");
            tw.WriteLine("  `ItemType` varchar(50) NOT NULL,");
            tw.WriteLine("  `Icon` varchar(20) NOT NULL,");
            tw.WriteLine("  PRIMARY KEY (`Id`)");
            tw.WriteLine(") ENGINE=MyIsam DEFAULT CHARSET=latin1;");
            tw.WriteLine();
            tw.Close();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static List<NanoFormula> ReadNanoFormulas()
        {
            var np = new NewParser();
            List<NanoFormula> rawNanoList = new List<NanoFormula>();
            int counter = 0;
            foreach (int recnum in extractor.GetRecordInstances(Extractor.RecordType.Nano))
            {
                if (counter == 0)
                {
                    counter = recnum;
                }

                byte[] data = extractor.GetRecordData(Extractor.RecordType.Nano, recnum);
                NanoFormula nano = np.ParseNano(recnum, data, "temp.sql");
                rawNanoList.Add(nano);
                if ((counter % 2000) == 0)
                {
                    Console.Write("\rNano ID: " + recnum.ToString().PadLeft(9));
                }

                counter++;
            }

            Console.Write("\rNano ID: " + rawNanoList[rawNanoList.Count - 1].ID.ToString().PadLeft(9));

            File.Delete("temp.sql");
            return rawNanoList;
        }

        /// <summary>
        /// </summary>
        /// <param name="rawItemList">
        /// </param>
        private static void SetItemRelations(List<ItemTemplate> rawItemList)
        {
            Dictionary<int, ItemTemplate> tp = new Dictionary<int, ItemTemplate>(150000);

            HashSet<ItemTemplate> hsitp = new HashSet<ItemTemplate>(rawItemList);

            Console.WriteLine("Setting item relations");

            foreach (ItemTemplate tep in rawItemList)
            {
                tp.Add(tep.ID, tep);
            }

            int perc = Relations.Count / 100;
            int counter = 0;
            int counter2 = 0;
            for (int pos = Relations.Count - 1; pos >= 0; pos--)
            {
                List<int> rels = Relations[pos];
                foreach (int id in rels)
                {
                    try
                    {
                        ItemTemplate temp = tp[id];

                        if (temp != null)
                        {
                            temp.Relations = rels;
                            hsitp.Remove(temp);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                if (counter % perc == 0)
                {
                    Console.Write("\r" + counter2 + "% done");
                    counter2++;
                }

                counter++;
            }

            Console.WriteLine("\r100% done");
            if (hsitp.Count != 0)
            {
                foreach (ItemTemplate template in hsitp)
                {
                    GetItemRelations(template);
                    Console.Write("\rFound missing item relations for " + template.ID);
                }

                Console.WriteLine();
                Console.Write("Saving new itemrelations...");
                List<string> newItemrelations = new List<string>();
                foreach (ItemTemplate it in hsitp)
                {
                    string ir = string.Empty;
                    foreach (int i in it.Relations)
                    {
                        ir += ir == string.Empty ? i.ToString() : " " + i;
                    }

                    if (!newItemrelations.Contains(ir))
                    {
                        newItemrelations.Add(ir);
                    }
                }

                newItemrelations.Sort();

                TextWriter tw = new StreamWriter("itemrelations.txt");
                foreach (string s in newItemrelations)
                {
                    tw.WriteLine(s);
                }

                tw.Close();
                Console.WriteLine(" done");
            }

            Console.WriteLine();
        }

        #endregion
    }
}