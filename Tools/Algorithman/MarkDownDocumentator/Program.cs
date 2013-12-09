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

namespace MarkDownDocumentator
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;

    using CellAO.Enums;
    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    internal class Program
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="tw">
        /// </param>
        /// <param name="text">
        /// </param>
        public static void WriteCode(TextWriter tw, string text)
        {
            tw.Write("`" + text + "`  ");
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="enumName">
        /// </param>
        /// <param name="enumType">
        /// </param>
        /// <param name="writeValues">
        /// </param>
        public static void WriteEnumList(string fileName, string enumName, Type enumType, bool writeValues = true)
        {
            bool isFlagsEnum = false;
            object[] memInfo = enumType.GetCustomAttributes(typeof(FlagsAttribute), false);
            isFlagsEnum = memInfo.Length > 0;

            TextWriter tw = new StreamWriter(fileName);

            WriteHeader1(tw, enumName);
            WriteCode(tw, enumType.FullName);
            if (writeValues)
            {
                tw.Write(" : ");
                WriteCode(tw, enumType.GetEnumUnderlyingType().Name);
            }

            if (isFlagsEnum)
            {
                tw.WriteLine();
                tw.WriteLine();
                WriteCode(tw, "[Flags]");
            }

            tw.WriteLine();
            WriteHorizonalLine(tw);
            tw.WriteLine();
            Array temp2 = Enum.GetValues(enumType);

            int max = temp2.Length;
            foreach (object v in temp2)
            {
                string eName = Enum.GetName(enumType, v);
                tw.Write("**" + eName + "**");
                if (writeValues)
                {
                    object underlyingValue = Convert.ChangeType(v, Enum.GetUnderlyingType(v.GetType()));
                    tw.Write(" = " + underlyingValue);
                }

                max--;
                if (max > 0)
                {
                    tw.WriteLine(",");
                }

                tw.WriteLine();
            }

            WriteFooter(tw);
            tw.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="tw">
        /// </param>
        /// <param name="text">
        /// </param>
        public static void WriteHeader1(TextWriter tw, string text)
        {
            tw.WriteLine("# " + text + " #");
        }

        /// <summary>
        /// </summary>
        /// <param name="tw">
        /// </param>
        /// <param name="text">
        /// </param>
        public static void WriteHeader2(TextWriter tw, string text)
        {
            tw.WriteLine("## " + text + " ##");
        }

        /// <summary>
        /// </summary>
        /// <param name="tw">
        /// </param>
        public static void WriteHorizonalLine(TextWriter tw)
        {
            tw.WriteLine();
            tw.WriteLine("----------");
            tw.WriteLine();
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        public static void WriteStatsList(string fileName)
        {
            TextWriter tw = new StreamWriter(fileName);

            WriteHeader1(tw, "CellAO Stats List");
            WriteHorizonalLine(tw);

            Stats stats = new Stats(new Identity() { Instance = 1, Type = IdentityType.CanbeAffected });

            foreach (Stat stat in stats.All)
            {
                string statName = StatNamesDefaults.GetStatName(stat.StatId);
                uint statDefaultValue = stat.BaseValue;
                bool dontWriteToSql = stat.DoNotDontWriteToSql;
                bool announceToPlayfield = stat.AnnounceToPlayfield;
                string className = stat.GetType().FullName;
                tw.WriteLine("**" + statName + " [" + stat.StatId + "]**");
                tw.WriteLine();
                tw.WriteLine("**Class type:** " + className);
                tw.WriteLine();
                tw.WriteLine("**Default value:** " + statDefaultValue);
                tw.WriteLine();
                tw.WriteLine("**Tags:** ");
                if (!dontWriteToSql)
                {
                    WriteCode(tw, "Save in Database");
                }

                if (!dontWriteToSql && announceToPlayfield)
                {
                    tw.Write(", ");
                }

                if (announceToPlayfield)
                {
                    WriteCode(tw, "Announce to Playfield");
                }

                tw.WriteLine();
                WriteHorizonalLine(tw);
                tw.WriteLine();
            }

            WriteFooter(tw);
            tw.Close();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            TextWriter tw =
                new StreamWriter(Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "Index.md"))));

            WriteHeader1(tw, "CellAO Stats and Enums");
            WriteHorizonalLine(tw);
            WriteLink(tw, "Stats list", "Stats.md");

            WriteStatsList(Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "Stats.md"))));

            WriteLink(tw, "Move modes", "MoveModes.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "MoveModes.md"))),
                "MoveModes",
                typeof(MoveModes),
                false);

            WriteHeader2(tw, "Inventory slots");
            WriteLink(tw, "Weapon page slots", "WeaponSlots.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "WeaponSlots.md"))),
                "Weapon slots",
                typeof(WeaponSlots));

            WriteLink(tw, "Armor page slots", "ArmorSlots.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "ArmorSlots.md"))),
                "Armor slots",
                typeof(ArmorSlots));

            WriteLink(tw, "Implant page slots", "ImplantSlots.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "ImplantSlots.md"))),
                "Implant slots",
                typeof(ImplantSlots));

            WriteHeader2(tw, "Item/Inventory related");
            tw.WriteLine();
            WriteLink(tw, "Inventory Errors", "InventoryError.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "InventoryError.md"))),
                "Inventory Errors",
                typeof(InventoryError),
                false);

            WriteLink(tw, "Can flags", "CanFlags.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "CanFlags.md"))),
                "Can Flags",
                typeof(CanFlags));

            WriteLink(tw, "Action Types", "ActionTypes.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "ActionTypes.md"))),
                "Action Types",
                typeof(ActionType));
            WriteLink(tw, "Event Types", "EventTypes.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "EventTypes.md"))),
                "Event Types",
                typeof(EventType));
            WriteLink(tw, "Function Types", "FunctionTypes.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "FunctionTypes.md"))),
                "Function Types",
                typeof(ActionType));
            WriteLink(tw, "Operators", "Operator.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "Operator.md"))),
                "Operators",
                typeof(Operator));

            WriteLink(tw, "Identity Types", "IdentityTypes.md");
            WriteEnumList(
                Path.Combine("..", Path.Combine("..", Path.Combine("Documentation", "IdentityTypes.md"))),
                "Identity Types",
                typeof(IdentityType));

            WriteHeader2(tw, "Network related");
            tw.WriteLine();

            WriteLink(tw, "N3 Message IDs", "N3MessageIDs.md");
            WriteEnumListHex(
                Path.Combine("..", "..", "Documentation", "N3MessageIDs.md"),
                "N3 Message IDs",
                typeof(N3MessageType));

            WriteFooter(tw);
            tw.Close();
        }

        private static void WriteEnumListHex(string fileName, string enumName, Type enumType, bool writeValues = true)
        {
            bool isFlagsEnum = false;
            object[] memInfo = enumType.GetCustomAttributes(typeof(FlagsAttribute), false);
            isFlagsEnum = memInfo.Length > 0;

            TextWriter tw = new StreamWriter(fileName);

            WriteHeader1(tw, enumName);
            WriteCode(tw, enumType.FullName);
            if (writeValues)
            {
                tw.Write(" : ");
                WriteCode(tw, enumType.GetEnumUnderlyingType().Name);
            }

            if (isFlagsEnum)
            {
                tw.WriteLine();
                tw.WriteLine();
                WriteCode(tw, "[Flags]");
            }

            tw.WriteLine();
            WriteHorizonalLine(tw);
            tw.WriteLine();
            Array temp2 = Enum.GetValues(enumType);

            List<String> tempList = new List<string>();

            int max = temp2.Length;
            foreach (object v in temp2)
            {
                string eName = Enum.GetName(enumType, v);
                string tempString = "**" + eName + "**";
                
                if (writeValues)
                {
                    object underlyingValue = Convert.ChangeType(v, Enum.GetUnderlyingType(v.GetType()));
                    tempString+=" = 0x" + ((int)underlyingValue).ToString("X8");
                }

                max--;
                if (max > 0)
                {
                    tempString+=",";
                }

                tempList.Add(tempString);
            }


            tempList.Sort();
            foreach (string s in tempList)
            {
                tw.WriteLine(s);
                tw.WriteLine();
            }

            WriteFooter(tw);
            tw.Close();

        }

        /// <summary>
        /// </summary>
        /// <param name="tw">
        /// </param>
        private static void WriteFooter(TextWriter tw)
        {
            tw.WriteLine();
            WriteHorizonalLine(tw);
            tw.WriteLine("*" + AssemblyInfoclass.Copyright + "*");
            tw.WriteLine();
            tw.WriteLine(
                "*Created by " + AssemblyInfoclass.Title + " Version " + AssemblyInfoclass.AssemblyVersion + " - "
                + AssemblyInfoclass.RevisionName + "*");
            tw.WriteLine();
            tw.WriteLine();
        }

        /// <summary>
        /// </summary>
        /// <param name="tw">
        /// </param>
        /// <param name="p1">
        /// </param>
        /// <param name="p2">
        /// </param>
        private static void WriteLink(TextWriter tw, string p1, string p2)
        {
            tw.Write("[" + p1 + "](./" + p2 + ")");
            tw.WriteLine();
            tw.WriteLine();
        }

        #endregion
    }
}