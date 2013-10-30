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
// Last modified: 2013-10-27 11:38
// Created:       2013-10-27 09:25

#endregion

namespace Extractor_Serializer
{
    #region Usings ...

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    #endregion

    public sealed class Parser
    {
        public enum ParserReturns
        {
            Parsing = -1,

            Success,

            Aborted,

            Failed,

            Crashed
        }

        private struct ReqsStruc
        {
            public int AttrNum;

            public int AttrVal;

            public int AttrOp;
        }

        protected internal sealed class BufferedReader
        {
            private struct xTraceStruc
            {
                public string Type;

                public int Length;

                public string Label;

                public string Caller;

                public string Value;
            }

            public int Ptr;

            public byte[] Buffer;

            public int RecordType;

            public int RecordNum;

            public bool Tracing;

            public BufferedReader(int rectype, int recnum, byte[] data, bool EnableTracing)
            {
                this.RecordType = rectype;
                this.RecordNum = recnum;
                this.Buffer = data;
                this.Tracing = EnableTracing;
            }

            private bool AddTrace(string Type, int Length, string Label)
            {
                bool flag = !this.Tracing;
                checked
                {
                    bool result;
                    if (flag)
                    {
                        result = (this.Ptr + Length >= this.Buffer.Length);
                    }
                    else
                    {
                        xTraceStruc xTraceStruc = default(xTraceStruc);
                        StackTrace stackTrace = new StackTrace();
                        xTraceStruc.Type = Type;
                        xTraceStruc.Length = Length;
                        xTraceStruc.Label = Label;
                        xTraceStruc.Caller = stackTrace.GetFrame(2).GetMethod().Name;
                        xTraceStruc.Value = "[NULL]";
                        this.xTraceInfo.Add(xTraceStruc);
                        flag = (this.Ptr + Length > this.Buffer.Length);
                        if (flag)
                        {
                            this.DebugDump("Read past end", ParserReturns.Failed);
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    return result;
                }
            }

            private void AddValue(string Value)
            {
            }

            public int ReadInt32(string Label)
            {
                bool flag = this.AddTrace("integer", 4, Label);
                checked
                {
                    int result;
                    if (flag)
                    {
                        result = 0;
                    }
                    else
                    {
                        int num = BitConverter.ToInt32(this.Buffer, this.Ptr);
                        this.AddValue(Conversions.ToString(num));
                        this.Ptr += 4;
                        result = num;
                    }
                    return result;
                }
            }

            public int ReadCNum(string Label)
            {
                bool flag = this.AddTrace("CNum", 4, Label);
                checked
                {
                    int result;
                    if (flag)
                    {
                        result = 0;
                    }
                    else
                    {
                        int num = BitConverter.ToInt32(this.Buffer, this.Ptr);
                        num = (int)((long)Math.Round(Math.Round(unchecked(num / 1009.0 - 1.0))));
                        this.AddValue(Conversions.ToString(num));
                        this.Ptr += 4;
                        result = num;
                    }
                    return result;
                }
            }

            public short ReadInt16(string Label)
            {
                bool flag = this.AddTrace("Int16", 2, Label);
                checked
                {
                    short result;
                    if (flag)
                    {
                        result = 0;
                    }
                    else
                    {
                        short num = BitConverter.ToInt16(this.Buffer, this.Ptr);
                        this.AddValue(Conversions.ToString((int)num));
                        this.Ptr += 2;
                        result = num;
                    }
                    return result;
                }
            }

            public byte ReadByte(string Label)
            {
                bool flag = this.AddTrace("Byte", 1, Label);
                checked
                {
                    byte result;
                    if (flag)
                    {
                        result = 0;
                    }
                    else
                    {
                        byte b = this.Buffer[this.Ptr];
                        this.AddValue(Conversions.ToString(b));
                        this.Ptr++;
                        result = b;
                    }
                    return result;
                }
            }

            public string ReadHash(string Label)
            {
                bool flag = this.AddTrace("Hash", 4, Label);
                checked
                {
                    string result;
                    if (flag)
                    {
                        result = Conversions.ToString(0);
                    }
                    else
                    {
                        byte[] array = new byte[4];
                        Array.Copy(this.Buffer, this.Ptr, array, 0, 4);
                        Array.Reverse(array);
                        this.Ptr += 4;
                        result = Encoding.ASCII.GetString(array);
                    }
                    return result;
                }
            }

            public string ReadString(string Label)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int arg_19_0 = this.Ptr;
                checked
                {
                    int num = this.Buffer.Length - 1;
                    int num2 = arg_19_0;
                    bool flag;
                    while (true)
                    {
                        int arg_4E_0 = num2;
                        int num3 = num;
                        if (arg_4E_0 > num3)
                        {
                            break;
                        }
                        byte b = this.Buffer[num2];
                        flag = (b == 0);
                        if (flag)
                        {
                            break;
                        }
                        stringBuilder.Append(Strings.Chr((int)b));
                        num2++;
                    }
                    string text = stringBuilder.ToString();
                    int length = text.Length;
                    flag = this.AddTrace(string.Format("String({0})", length), length, Label);
                    string result;
                    if (flag)
                    {
                        result = "";
                    }
                    else
                    {
                        this.Ptr += length;
                        result = text;
                    }
                    return result;
                }
            }

            public string ReadString(int Length, string Label)
            {
                bool flag = this.AddTrace(string.Format("String({0})", Length), Length, Label);
                checked
                {
                    string result;
                    if (flag)
                    {
                        result = Conversions.ToString(0);
                    }
                    else
                    {
                        string @string = Encoding.UTF8.GetString(this.Buffer, this.Ptr, Length);
                        this.AddValue(@string);
                        this.Ptr += Length;
                        result = @string;
                    }
                    return result;
                }
            }

            public void SkipBytes(int Count, string Label)
            {
                bool flag = this.AddTrace(string.Format("Skipped({0})", Count), Count, Label);
                checked
                {
                    if (!flag)
                    {
                        this.Ptr += Count;
                    }
                }
            }
        }

        private static readonly List<WeakReference> __ENCList = new List<WeakReference>();

        private const byte BlankOperator = 255;

        private const byte DefaultTarget = 255;

        [AccessedThroughProperty("Output")]
        private static Plugin _Output;

        private readonly string OldDF;

        private readonly string NewDF;

        private readonly Hashtable Checksums;

        private static ParserReturns ReturnCode;

        private static Thread pThread;

        private static Thread cThread;

        private BufferedReader BR;

        private readonly Hashtable FunctionSets;

        private readonly Hashtable Blacklist;

        private int AOID;

        private readonly bool Tracing;

        private static string myAbortMsg;

        private static bool SkipCompare;

        private static ThreadPriority Pri;

        private static Plugin Output
        {
            [DebuggerNonUserCode]
            get
            {
                return _Output;
            }
            [DebuggerNonUserCode]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                Plugin.ChangePriorityEventHandler obj = new Plugin.ChangePriorityEventHandler(Parser.cPriority);
                Plugin.AbortEventHandler obj2 = new Plugin.AbortEventHandler(Parser.Abort);
                bool flag = _Output != null;
                if (flag)
                {
                    _Output.ChangePriority -= obj;
                    _Output.Abort -= obj2;
                }
                _Output = value;
                flag = (_Output != null);
                if (flag)
                {
                    _Output.ChangePriority += obj;
                    _Output.Abort += obj2;
                }
            }
        }

        public string AbortMsg
        {
            get
            {
                return myAbortMsg;
            }
        }

        [DebuggerNonUserCode]
        private static void __ENCAddToList(object value)
        {
            List<WeakReference> _ENCList = __ENCList;
            Monitor.Enter(_ENCList);
            checked
            {
                try
                {
                    bool flag = __ENCList.Count == __ENCList.Capacity;
                    if (flag)
                    {
                        int num = 0;
                        int arg_3F_0 = 0;
                        int num2 = __ENCList.Count - 1;
                        int num3 = arg_3F_0;
                        while (true)
                        {
                            int arg_90_0 = num3;
                            int num4 = num2;
                            if (arg_90_0 > num4)
                            {
                                break;
                            }
                            WeakReference weakReference = __ENCList[num3];
                            flag = weakReference.IsAlive;
                            if (flag)
                            {
                                bool flag2 = num3 != num;
                                if (flag2)
                                {
                                    __ENCList[num] = __ENCList[num3];
                                }
                                num++;
                            }
                            num3++;
                        }
                        __ENCList.RemoveRange(num, __ENCList.Count - num);
                        __ENCList.Capacity = __ENCList.Count;
                    }
                    __ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
                }
                finally
                {
                    Monitor.Exit(_ENCList);
                }
            }
        }

        public void Close(string Msg)
        {
            bool flag = Parser.GZB != null;
            if (flag)
            {
                Parser.GZB.Close();
            }
            Abort(Msg);
        }

        private static void cPriority(ThreadPriority value)
        {
            value = (ThreadPriority)Math.Min((int)value, 4);
            value = (ThreadPriority)Math.Max((int)value, 0);
            Pri = value;
            try
            {
                pThread.Priority = value;
            }
            catch (Exception expr_28)
            {
                ProjectData.SetProjectError(expr_28);
                ProjectData.ClearProjectError();
            }
            try
            {
                cThread.Priority = value;
            }
            catch (Exception expr_47)
            {
                ProjectData.SetProjectError(expr_47);
                ProjectData.ClearProjectError();
            }
        }

        public void ForcePriority(ThreadPriority P)
        {
            cPriority(P);
        }

        public Parser()
        {
            __ENCAddToList(this);
            this.Checksums = new Hashtable();
            this.FunctionSets = new Hashtable();
            this.Blacklist = new Hashtable();
            Output = Main.Plugin;
            this.OldDF = Main.OldData;
            this.NewDF = Main.NewData;
            this.lblStat = lblStatus;
            this.pbProg = pbProgress;
            Parser.cmbPrior = cmpPri;
            this.Tracing = EnableTracing;
        }

        public void ForceAbort(string Msg)
        {
            myAbortMsg = Msg;
            ReturnCode = ParserReturns.Aborted;
            bool flag = Parser.GZB != null;
            if (flag)
            {
                Parser.GZB.Close();
            }
        }

        private static void Abort(string Msg)
        {
            try
            {
                myAbortMsg = Msg;
                Output.Parse_End(true);
            }
            catch (SQLiteException expr_16)
            {
                ProjectData.SetProjectError(expr_16);
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            catch (Exception expr_2B)
            {
                ProjectData.SetProjectError(expr_2B);
                Exception ex = expr_2B;
                CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            ReturnCode = ParserReturns.Aborted;
            bool flag = pThread != null;
            if (flag)
            {
                pThread.Abort();
            }
        }

        private void PreParse()
        {
            try
            {
                Output.Parse_Begin(Main.ParsePath, Main.pvStr(Parser.XH.FileVer, "."), SkipCompare, Main.PCMD);
            }
            catch (SQLiteException expr_32)
            {
                ProjectData.SetProjectError(expr_32);
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            catch (Exception expr_47)
            {
                ProjectData.SetProjectError(expr_47);
                Exception ex = expr_47;
                CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
        }

        private void PostParse()
        {
            try
            {
                Output.Parse_End(false);
            }
            catch (SQLiteException expr_10)
            {
                ProjectData.SetProjectError(expr_10);
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            catch (Exception expr_25)
            {
                ProjectData.SetProjectError(expr_25);
                Exception ex = expr_25;
                CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
        }

        private int CleanReturn()
        {
            try
            {
                bool flag = Parser.GZB != null;
                if (flag)
                {
                    Parser.GZB.Close();
                }
                Parser.GZB = null;
            }
            catch (SQLiteException expr_24)
            {
                ProjectData.SetProjectError(expr_24);
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            catch (Exception expr_39)
            {
                ProjectData.SetProjectError(expr_39);
                ProjectData.ClearProjectError();
            }
            return (int)ReturnCode;
        }

        public ParserReturns Parse()
        {
            SkipCompare = (Operators.CompareString(this.OldDF, "", false) == 0);
            ReturnCode = ParserReturns.Parsing;
            Main.TotalParsed = 0;
            bool flag = SkipCompare & !Main.JobParse;
            if (flag)
            {
                this.lblStat.Text = "Full Parse?";
                flag =
                    (Interaction.MsgBox(
                        string.Format(
                            "You're about to perform a full parse.{0}This operation could take a long time.{0}Are you sure you want to do this?",
                            "\r\n"),
                        MsgBoxStyle.YesNo | MsgBoxStyle.Critical | MsgBoxStyle.Question,
                        null) == MsgBoxResult.No);
                if (flag)
                {
                    ParserReturns result = ParserReturns.Aborted;
                    return result;
                }
            }
            else
            {
                flag = !SkipCompare;
                if (flag)
                {
                    this.GetCRCs();
                }
                flag = (ReturnCode != ParserReturns.Parsing);
                if (flag)
                {
                    ParserReturns result = (ParserReturns)this.CleanReturn();
                    return result;
                }
            }
            Parser.GZB = new gzbFile(this.NewDF, FileMode.Open, CompressionMode.Decompress);
            Parser.XH = Parser.GZB.ReadHeader();
            this.lblStat.Text = "Initializing Plugin...";
            Main.DoEvents();
            cThread = new Thread(this.PreParse);
            cThread.IsBackground = false;
            cThread.Priority = Pri;
            cThread.Start();
            int pri = (int)Pri;
            checked
            {
                while (cThread.IsAlive)
                {
                    flag = (Pri != (ThreadPriority)pri);
                    if (flag)
                    {
                        pri = (int)Pri;
                        Parser.cmbPrior.SelectedIndex = (ThreadPriority.Highest - Pri);
                    }
                    Main.DoEvents();
                }
                flag = (ReturnCode != ParserReturns.Parsing);
                ParserReturns result;
                if (flag)
                {
                    result = (ParserReturns)this.CleanReturn();
                }
                else
                {
                    this.LoadFunctionSets(Parser.XH.FileVer);
                    this.lblStat.Text = "Parsing...";
                    Main.DoEvents();
                    pThread = new Thread(this.DoParse);
                    pThread.IsBackground = false;
                    pThread.Priority = Pri;
                    pThread.Start();
                    while (ReturnCode == ParserReturns.Parsing)
                    {
                        this.pbProg.Value =
                            (int)
                            Math.Round(
                                unchecked(
                                (double)Parser.GZB.FileStream.Position / (double)Parser.GZB.FileStream.Length * 100.0));
                        this.pbProg.Refresh();
                        flag = (Pri != (ThreadPriority)pri);
                        if (flag)
                        {
                            pri = (int)Pri;
                            Parser.cmbPrior.SelectedIndex = (ThreadPriority.Highest - Pri);
                        }
                        Main.DoEvents();
                    }
                    int num = this.CleanReturn();
                    flag = (ReturnCode != ParserReturns.Success);
                    if (flag)
                    {
                        result = (ParserReturns)num;
                    }
                    else
                    {
                        this.lblStat.Text = "Finalizing...";
                        cThread = new Thread(this.PostParse);
                        cThread.IsBackground = false;
                        cThread.Priority = Pri;
                        cThread.Start();
                        while (cThread.IsAlive)
                        {
                            flag = (Pri != (ThreadPriority)pri);
                            if (flag)
                            {
                                pri = (int)Pri;
                                Parser.cmbPrior.SelectedIndex = (ThreadPriority.Highest - Pri);
                            }
                            Main.DoEvents();
                        }
                        result = (ParserReturns)this.CleanReturn();
                    }
                }
                return result;
            }
        }

        private void LoadFunctionSets(int FileVer)
        {
            string left = Main.pvStr(FileVer, ".");
            cfgFile cfgFile = new cfgFile(Path.Combine(Main.AppPath(), "Config", "FunctionSets.cfg"));
            SortedList sortedList = new SortedList();
            Regex regex = new Regex("^\\d{2}\\.\\d{2}\\.\\d{2}\\.\\d{2}$");
            cfgFile.cfgSection[] sections = cfgFile.Sections;
            checked
            {
                for (int i = 0; i < sections.Length; i++)
                {
                    cfgFile.cfgSection cfgSection = sections[i];
                    bool flag = regex.Match(cfgSection.Name).Success;
                    if (flag)
                    {
                        sortedList.Add(cfgSection.Name, cfgSection);
                    }
                }
                IDictionaryEnumerator enumerator = sortedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object expr_98 = enumerator.Current;
                    DictionaryEntry dictionaryEntry2;
                    DictionaryEntry dictionaryEntry = (expr_98 != null) ? ((DictionaryEntry)expr_98) : dictionaryEntry2;
                    bool flag = Operators.ConditionalCompareObjectLess(left, dictionaryEntry.Key, false);
                    if (!flag)
                    {
                        cfgFile.cfgSection cfgSection2 = (cfgFile.cfgSection)dictionaryEntry.Value;
                        cfgFile.cfgKey[] keys = cfgSection2.Keys;
                        for (int j = 0; j < keys.Length; j++)
                        {
                            cfgFile.cfgKey cfgKey = keys[j];
                            flag = this.FunctionSets.ContainsKey(cfgKey.Name);
                            if (flag)
                            {
                                this.FunctionSets[cfgKey.Name] = cfgKey.Value;
                            }
                            else
                            {
                                this.FunctionSets.Add(cfgKey.Name, cfgKey.Value);
                            }
                        }
                    }
                }
            }
        }

        private void GetCRCs()
        {
            this.lblStat.Text = "Reading Checksums...";
            Parser.GZB = new gzbFile(this.OldDF, FileMode.Open, CompressionMode.Decompress);
            Parser.GZB.ReadHeader();
            while (ReturnCode == ParserReturns.Parsing)
            {
                gzbFile.FileXRDB4Record fileXRDB4Record = Parser.GZB.ReadRecord();
                bool flag = fileXRDB4Record.MarkerTag != -559038242;
                if (flag)
                {
                    CustomException ex =
                        new CustomException(
                            string.Format("{0}{1}{0}{2}is corrupt!", '"', Parser.GZB.FileStream.Name, "\r\n"),
                            "Corrupted data file detected");
                    ReturnCode = ParserReturns.Failed;
                    return;
                }
                flag = (fileXRDB4Record.RecordType == 0);
                if (flag)
                {
                    break;
                }
                string key = Conversions.ToString(fileXRDB4Record.RecordType) + ":"
                             + Conversions.ToString(fileXRDB4Record.RecordNum);
                this.Checksums.Add(key, fileXRDB4Record.RecordCRC);
                this.pbProg.Value =
                    checked(
                        (int)
                        Math.Round(
                            unchecked(
                            (double)Parser.GZB.FileStream.Position / (double)Parser.GZB.FileStream.Length * 100.0)));
                Main.DoEvents();
            }
            Parser.GZB.Close();
            Parser.GZB = null;
        }

        private void DoParse()
        {
            checked
            {
                while (ReturnCode == ParserReturns.Parsing)
                {
                    gzbFile.FileXRDB4Record xR = Parser.GZB.ReadRecord();
                    bool flag = xR.MarkerTag != -559038242;
                    if (flag)
                    {
                        CustomException ex =
                            new CustomException(
                                string.Format("{0}{1}{0}{2}is corrupt!", '"', Parser.GZB.FileStream.Name, "\r\n"),
                                "Corrupted data file detected");
                        Abort("Corrupt Datafile");
                        ReturnCode = ParserReturns.Failed;
                        break;
                    }
                    string key = Conversions.ToString(xR.RecordType) + ":" + Conversions.ToString(xR.RecordNum);
                    flag = this.Blacklist.ContainsKey(key);
                    if (!flag)
                    {
                        Plugin.ChangeStates changeStates = Plugin.ChangeStates.NewRecord;
                        flag = this.Checksums.ContainsKey(key);
                        bool flag2;
                        if (flag)
                        {
                            flag2 = Operators.ConditionalCompareObjectNotEqual(this.Checksums[key], xR.RecordCRC, false);
                            if (flag2)
                            {
                                changeStates = Plugin.ChangeStates.ModifiedRecord;
                            }
                            else
                            {
                                changeStates = Plugin.ChangeStates.NoChange;
                            }
                        }
                        Application.DoEvents();
                        this.AOID = xR.RecordNum;
                        int recordType = xR.RecordType;
                        flag2 = (recordType == 0);
                        if (flag2)
                        {
                            ReturnCode = ParserReturns.Success;
                            break;
                        }
                        flag2 = (recordType == 1000020 || recordType == 1040005);
                        if (flag2)
                        {
                            this.ParseItemNano(xR, changeStates);
                        }
                        else
                        {
                            try
                            {
                                flag2 = !Output.OtherData_Begin(xR.RecordNum, xR.RecordType, changeStates);
                                if (flag2)
                                {
                                    continue;
                                }
                                Output.OtherData(xR.RecordData);
                                Output.OtherData_End();
                            }
                            catch (SQLiteException expr_197)
                            {
                                ProjectData.SetProjectError(expr_197);
                                ReturnCode = ParserReturns.Crashed;
                                ProjectData.ClearProjectError();
                            }
                            catch (Exception expr_1AD)
                            {
                                ProjectData.SetProjectError(expr_1AD);
                                Exception ex2 = expr_1AD;
                                CustomException ex3 = new CustomException(ex2.ToString(), "Plugin Error");
                                ReturnCode = ParserReturns.Crashed;
                                ProjectData.ClearProjectError();
                            }
                        }
                        Main.TotalParsed++;
                    }
                }
            }
        }

        private bool CritError(string Msg)
        {
            return false;
        }

        private void ParseItemNano(int rectype, int recnum, byte[] data)
        {
            this.BR = new BufferedReader(rectype, recnum, data, this.Tracing);
            bool flag;
            flag = !Output.ItemNano_Begin(XR.RecordNum, XR.RecordType == 1040005, CS);
            if (flag)
            {
                return;
            }

            br.SkipBytes(16, "Pre-Attr");
            int num = br.ReadCNum("AttrCount");
            Plugin.ItemNanoInfo info = default(Plugin.ItemNanoInfo);
            List<Plugin.ItemNanoKeyVal> list = new List<Plugin.ItemNanoKeyVal>();
            bool flag2 = false;
            int arg_C0_0 = 0;
            short num5;
            short num6;
            checked
            {
                int num2 = num - 1;
                int num3 = arg_C0_0;
                while (true)
                {
                    int arg_1C2_0 = num3;
                    int num4 = num2;
                    if (arg_1C2_0 > num4)
                    {
                        break;
                    }
                    Plugin.ItemNanoKeyVal item = default(Plugin.ItemNanoKeyVal);
                    item.AttrKey = br.ReadInt32("AttrKey" + Conversions.ToString(num3));
                    item.AttrVal = br.ReadInt32("AttrVal" + Conversions.ToString(num3));
                    list.Add(item);
                    int attrKey = item.AttrKey;
                    flag = (attrKey == 54);
                    if (flag)
                    {
                        info.QL = item.AttrVal;
                    }
                    else
                    {
                        flag = (attrKey == 76);
                        if (flag)
                        {
                            info.EquipPage = item.AttrVal;
                        }
                        else
                        {
                            flag = (attrKey == 88);
                            if (flag)
                            {
                                info.DefaultSlot = item.AttrVal;
                            }
                            else
                            {
                                flag = (attrKey == 298);
                                if (flag)
                                {
                                    info.EquipSlots = item.AttrVal;
                                }
                                else
                                {
                                    flag = (attrKey == 388);
                                    if (flag)
                                    {
                                        flag2 = true;
                                    }
                                }
                            }
                        }
                    }
                    num3++;
                }
                br.SkipBytes(8, "Post-Attr");
                num5 = br.ReadInt16("NameLen");
                num6 = br.ReadInt16("DescLen");
            }
            bool arg_222_0;
            if (num5 >= 0 && num6 >= 0)
            {
                if (num5 <= 4095L)
                {
                    if (num6 <= 4095L)
                    {
                        arg_222_0 = false;
                        goto IL_222;
                    }
                }
            }
            arg_222_0 = true;
        IL_222:
            flag = arg_222_0;
            if (flag)
            {
                br.DebugDump("NameLen or DescLen is invalid", ParserReturns.Failed);
            }
            flag = (num5 > 0);
            if (flag)
            {
                info.Name = br.ReadString((int)num5, "Name");
            }
            else
            {
                info.Name = "";
            }
            flag = (num6 > 0);
            if (flag)
            {
                info.Description = br.ReadString((int)num6, "Description");
            }
            else
            {
                info.Description = "";
            }
            BitManipulation bitManipulation = new BitManipulation();
            info.Type = info.EquipPage;
            flag = (Strings.InStr(info.Name, "_", CompareMethod.Binary) != 0
                    || Strings.InStr(Strings.UCase(info.Name), "BOSS", CompareMethod.Binary) != 0);
            if (flag)
            {
                info.Type = 4;
            }
            else
            {
                flag = flag2;
                if (flag)
                {
                    info.Type = 7;
                }
                else
                {
                    flag = (info.EquipPage == 1);
                    if (flag)
                    {
                        bool flag3 = bitManipulation.CheckBit((long)info.EquipSlots, 6)
                                     || bitManipulation.CheckBit((long)info.EquipSlots, 8);
                        if (flag3)
                        {
                            info.Type = 1;
                        }
                        else
                        {
                            info.Type = 6;
                        }
                    }
                }
            }
            try
            {
                Output.ItemNano(info, list.ToArray());
            }
            catch (SQLiteException expr_371)
            {
                ProjectData.SetProjectError(expr_371);
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            catch (Exception expr_387)
            {
                ProjectData.SetProjectError(expr_387);
                Exception ex3 = expr_387;
                CustomException ex4 = new CustomException(ex3.ToString(), "Plugin Error");
                ReturnCode = ParserReturns.Crashed;
                ProjectData.ClearProjectError();
            }
            bool flag4 = true;
            checked
            {
                while (br.Ptr < br.Buffer.Length - 8 && flag4)
                {
                    switch (br.ReadInt32("ParseSetsKeyNum"))
                    {
                        case 2:
                            this.ParseFunctionSet(ref flag4);
                            break;
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 21:
                            goto IL_4BF;
                        case 4:
                            this.ParseAtkDefSet(ref flag4);
                            break;
                        case 6:
                            {
                                br.SkipBytes(4, "Pre-SkipSet");
                                int count = br.ReadCNum("SkipSet") * 8;
                                br.SkipBytes(count, "Post-SkipSet");
                                break;
                            }
                        case 14:
                            this.ParseAnimSoundSet(1, ref flag4);
                            break;
                        case 20:
                            this.ParseAnimSoundSet(2, ref flag4);
                            break;
                        case 22:
                            this.ParseActionSet(ref flag4);
                            break;
                        case 23:
                            this.ParseShopHash(ref flag4);
                            break;
                        default:
                            goto IL_4BF;
                    }
                    continue;
                IL_4BF:
                    flag4 = this.CritError("Invalid KeyNum");
                }
                try
                {
                    Output.ItemNano_End();
                }
                catch (SQLiteException expr_50A)
                {
                    ProjectData.SetProjectError(expr_50A);
                    ReturnCode = ParserReturns.Crashed;
                    ProjectData.ClearProjectError();
                }
                catch (Exception expr_520)
                {
                    ProjectData.SetProjectError(expr_520);
                    Exception ex5 = expr_520;
                    CustomException ex6 = new CustomException(ex5.ToString(), "Plugin Error");
                    ReturnCode = ParserReturns.Crashed;
                    ProjectData.ClearProjectError();
                }
            }
        }

        private void ParseShopHash(ref bool R)
        {
            List<Plugin.ItemNanoFunction> list = new List<Plugin.ItemNanoFunction>();
            int eventNum = br.ReadInt32("EventNum");
            int num = br.ReadCNum("NumFuncs");
            int arg_2D_0 = 1;
            int num2 = num;
            int num3 = arg_2D_0;
            checked
            {
                while (true)
                {
                    int arg_151_0 = num3;
                    int num4 = num2;
                    if (arg_151_0 > num4)
                    {
                        break;
                    }
                    string text = br.ReadString(4, "StrArg");
                    int num5 = (int)br.ReadByte("numA");
                    int num6 = (int)br.ReadByte("numB");
                    bool flag = num5 == 0 && num6 == 0;
                    if (flag)
                    {
                        num5 = (int)br.ReadInt16("numA2");
                        num6 = (int)br.ReadInt16("numB2");
                    }
                    int count = Math.Min(11, br.Buffer.Length - br.Ptr);
                    br.SkipBytes(count, "ShopHashSkip");
                    list.Add(
                        new Plugin.ItemNanoFunction
                            {
                                FunctionArgs =
                                    new[]
                                        {
                                            text, Conversions.ToString(num5),
                                            Conversions.ToString(num6)
                                        },
                                FunctionReqs = new Plugin.ItemNanoRequirement[0],
                                Target = 255,
                                TickCount = 1,
                                TickInterval = 0
                            });
                    num3++;
                }
                try
                {
                    Output.ItemNanoEventAndFunctions(eventNum, list.ToArray());
                }
                catch (SQLiteException expr_16B)
                {
                    ProjectData.SetProjectError(expr_16B);
                    ReturnCode = ParserReturns.Crashed;
                    ProjectData.ClearProjectError();
                }
                catch (Exception expr_181)
                {
                    ProjectData.SetProjectError(expr_181);
                    Exception ex = expr_181;
                    CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                    ReturnCode = ParserReturns.Crashed;
                    R = false;
                    ProjectData.ClearProjectError();
                }
            }
        }

        private void ParseActionSet(ref bool R)
        {
            bool flag = br.ReadInt32("&H24 Check") != 36;
            if (flag)
            {
                throw new Exception("Why am I here?");
            }
            int arg_3D_0 = 1;
            int num = br.ReadCNum("MaxSets");
            int num2 = arg_3D_0;
            checked
            {
                while (true)
                {
                    int arg_160_0 = num2;
                    int num3 = num;
                    if (arg_160_0 > num3)
                    {
                        break;
                    }
                    int actionNum = br.ReadInt32("actionNum");
                    int num4 = br.ReadCNum("NumReqs");
                    Plugin.ItemNanoRequirement[] requirements = new Plugin.ItemNanoRequirement[0];
                    flag = (num4 > 0);
                    if (flag)
                    {
                        List<ReqsStruc> list = new List<ReqsStruc>();
                        int arg_86_0 = 0;
                        int num5 = num4 - 1;
                        int num6 = arg_86_0;
                        while (true)
                        {
                            int arg_F0_0 = num6;
                            num3 = num5;
                            if (arg_F0_0 > num3)
                            {
                                break;
                            }
                            list.Add(
                                new ReqsStruc
                                    {
                                        AttrNum = br.ReadInt32("RawReqsNum"),
                                        AttrVal = br.ReadInt32("RawReqsVal"),
                                        AttrOp = br.ReadInt32("RawReqsOp")
                                    });
                            num6++;
                        }
                        requirements = this.ParseReqs(list.ToArray());
                    }
                    try
                    {
                        Output.ItemNanoAction(actionNum, requirements);
                    }
                    catch (SQLiteException expr_111)
                    {
                        ProjectData.SetProjectError(expr_111);
                        ReturnCode = ParserReturns.Crashed;
                        ProjectData.ClearProjectError();
                    }
                    catch (Exception expr_127)
                    {
                        ProjectData.SetProjectError(expr_127);
                        Exception ex = expr_127;
                        CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                        ReturnCode = ParserReturns.Crashed;
                        R = false;
                        ProjectData.ClearProjectError();
                    }
                    num2++;
                }
            }
        }

        private void ParseAnimSoundSet(int TypeN, ref bool R)
        {
            int num = br.ReadInt32("SetType");
            int num2 = br.ReadCNum("NumFunc");
            int arg_27_0 = 1;
            int num3 = num2;
            int num4 = arg_27_0;
            checked
            {
                while (true)
                {
                    int arg_13B_0 = num4;
                    int num5 = num3;
                    if (arg_13B_0 > num5)
                    {
                        break;
                    }
                    List<int> list = new List<int>();
                    int actionNum = br.ReadInt32("actionNum");
                    int num6 = br.ReadCNum("maxSets");
                    int arg_5C_0 = 1;
                    int num7 = num6;
                    int num8 = arg_5C_0;
                    while (true)
                    {
                        int arg_96_0 = num8;
                        num5 = num7;
                        if (arg_96_0 > num5)
                        {
                            break;
                        }
                        int item = br.ReadInt32("animNum" + Conversions.ToString(num8));
                        list.Add(item);
                        num8++;
                    }
                    try
                    {
                        switch (TypeN)
                        {
                            case 1:
                                Output.ItemNanoAnimSets(actionNum, list.ToArray());
                                break;
                            case 2:
                                Output.ItemNanoSoundSets(actionNum, list.ToArray());
                                break;
                            default:
                                throw new Exception("Xyphos, you're an idiot!");
                        }
                    }
                    catch (SQLiteException expr_E9)
                    {
                        ProjectData.SetProjectError(expr_E9);
                        ReturnCode = ParserReturns.Crashed;
                        ProjectData.ClearProjectError();
                    }
                    catch (Exception expr_FF)
                    {
                        ProjectData.SetProjectError(expr_FF);
                        Exception ex = expr_FF;
                        CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                        ReturnCode = ParserReturns.Crashed;
                        R = false;
                        ProjectData.ClearProjectError();
                    }
                    num4++;
                }
            }
        }

        private void ParseFunctionSet(ref bool R)
        {
            int eventNum = br.ReadInt32("EventNum");
            int num = br.ReadCNum("NumFuncs");
            List<Plugin.ItemNanoFunction> list = new List<Plugin.ItemNanoFunction>();
            int arg_2F_0 = 0;
            checked
            {
                int num2 = num - 1;
                int num3 = arg_2F_0;
                while (true)
                {
                    int arg_1C3_0 = num3;
                    int num4 = num2;
                    if (arg_1C3_0 > num4)
                    {
                        goto Block_4;
                    }
                    Plugin.ItemNanoFunction item = default(Plugin.ItemNanoFunction);
                    item.FunctionReqs = new Plugin.ItemNanoRequirement[0];
                    item.FunctionArgs = new string[0];
                    item.FunctionNum = br.ReadInt32("FuncNum");
                    br.SkipBytes(8, "FuncHeaderPreSkip");
                    int num5 = br.ReadInt32("NumReqs");
                    bool flag = num5 > 0;
                    if (flag)
                    {
                        List<ReqsStruc> list2 = new List<ReqsStruc>();
                        int arg_AE_0 = 0;
                        int num6 = num5 - 1;
                        int num7 = arg_AE_0;
                        while (true)
                        {
                            int arg_118_0 = num7;
                            num4 = num6;
                            if (arg_118_0 > num4)
                            {
                                break;
                            }
                            list2.Add(
                                new ReqsStruc
                                    {
                                        AttrNum = br.ReadInt32("RawReqsNum"),
                                        AttrVal = br.ReadInt32("RawReqsVal"),
                                        AttrOp = br.ReadInt32("RawReqsOp")
                                    });
                            num7++;
                        }
                        item.FunctionReqs = this.ParseReqs(list2.ToArray());
                    }
                    item.TickCount = br.ReadInt32("TickCount");
                    item.TickInterval = br.ReadInt32("TickInterval");
                    item.Target = br.ReadInt32("Target");
                    br.SkipBytes(4, "FuncHeaderPostSkip");
                    item.FunctionArgs = this.ParseArgs(Conversions.ToString(item.FunctionNum), ref R);
                    flag = (R == 0);
                    if (flag)
                    {
                        break;
                    }
                    list.Add(item);
                    num3++;
                }
                return;
            Block_4:
                try
                {
                    Output.ItemNanoEventAndFunctions(eventNum, list.ToArray());
                }
                catch (SQLiteException expr_1DD)
                {
                    ProjectData.SetProjectError(expr_1DD);
                    ReturnCode = ParserReturns.Crashed;
                    ProjectData.ClearProjectError();
                }
                catch (Exception expr_1F3)
                {
                    ProjectData.SetProjectError(expr_1F3);
                    Exception ex = expr_1F3;
                    CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                    ReturnCode = ParserReturns.Crashed;
                    R = false;
                    ProjectData.ClearProjectError();
                }
            }
        }

        private Plugin.ItemNanoRequirement[] ParseReqs(ReqsStruc[] RawReqs)
        {
            int num = Information.UBound(RawReqs, 1);
            List<Plugin.ItemNanoRequirement> list = new List<Plugin.ItemNanoRequirement>();
            int arg_16_0 = 0;
            int num2 = num;
            int num3 = arg_16_0;
            checked
            {
                while (true)
                {
                    int arg_1DA_0 = num3;
                    int num4 = num2;
                    if (arg_1DA_0 > num4)
                    {
                        break;
                    }
                    ReqsStruc reqsStruc = RawReqs[num3];
                    Plugin.ItemNanoRequirement item = default(Plugin.ItemNanoRequirement);
                    item.Target = 19;
                    item.AttrNum = reqsStruc.AttrNum;
                    item.MainOp = reqsStruc.AttrOp;
                    item.AttrValue = reqsStruc.AttrVal;
                    item.ChildOp = 255;
                    bool arg_F3_0;
                    if (num3 < num)
                    {
                        if (item.MainOp != 18 && item.MainOp != 19)
                        {
                            if (item.MainOp != 26 && item.MainOp != 27)
                            {
                                if (item.MainOp != 28 && item.MainOp != 29)
                                {
                                    if (item.MainOp != 30 && item.MainOp != 37)
                                    {
                                        if (item.MainOp != 100 && item.MainOp != 110)
                                        {
                                            goto IL_EF;
                                        }
                                    }
                                }
                            }
                        }
                    IL_F2:
                        arg_F3_0 = true;
                        goto IL_F3;
                        goto IL_F2;
                    }
                    goto IL_EF;
                IL_F3:
                    bool flag = arg_F3_0;
                    if (flag)
                    {
                        item.Target = item.MainOp;
                        num3++;
                        reqsStruc = RawReqs[num3];
                        item.AttrNum = reqsStruc.AttrNum;
                        item.MainOp = reqsStruc.AttrOp;
                        item.AttrValue = reqsStruc.AttrVal;
                    }
                    flag = (num3 < num && num != 1);
                    if (flag)
                    {
                        int attrNum = RawReqs[num3 + 1].AttrNum;
                        int attrVal = RawReqs[num3 + 1].AttrVal;
                        int attrOp = RawReqs[num3 + 1].AttrOp;
                        if (attrOp == 3 || attrOp == 4)
                        {
                            goto IL_1A1;
                        }
                        if (attrOp == 42)
                        {
                            goto IL_1A1;
                        }
                        goto IL_1A6;
                    IL_1AA:
                        bool arg_1AA_0;
                        flag = arg_1AA_0;
                        if (flag)
                        {
                            item.ChildOp = attrOp;
                            num3++;
                        }
                        goto IL_1C0;
                    IL_1A1:
                        if (attrNum == 0)
                        {
                            arg_1AA_0 = true;
                            goto IL_1AA;
                        }
                    IL_1A6:
                        arg_1AA_0 = false;
                        goto IL_1AA;
                    }
                IL_1C0:
                    list.Add(item);
                    num3++;
                    continue;
                IL_EF:
                    arg_F3_0 = false;
                    goto IL_F3;
                }
                int index = list.Count - 1;
                int num5 = list.Count - 2;
                int arg_1F7_0 = 0;
                int num6 = num5;
                int num7 = arg_1F7_0;
                Plugin.ItemNanoRequirement value;
                while (true)
                {
                    int arg_239_0 = num7;
                    int num4 = num6;
                    if (arg_239_0 > num4)
                    {
                        break;
                    }
                    value = list[num7];
                    value.ChildOp = list[num7 + 1].ChildOp;
                    list[num7] = value;
                    num7++;
                }
                value = list[index];
                value.ChildOp = 255;
                list[index] = value;
                return list.ToArray();
            }
        }

        private string[] ParseArgs(string FuncNum, ref bool R)
        {
            bool flag = !this.FunctionSets.ContainsKey(FuncNum);
            if (flag)
            {
                br.DebugDump("Unhandled Function", ParserReturns.Failed);
            }
            string[] array = Strings.Split(
                Conversions.ToString(this.FunctionSets[FuncNum]), ",", -1, CompareMethod.Binary);
            List<string> list = new List<string>();
            string[] array2 = array;
            checked
            {
                for (int i = 0; i < array2.Length; i++)
                {
                    string str = array2[i];
                    int num = (int)Math.Round(Conversion.Val(Strings.Trim(str)));
                    string text = Strings.LCase(Strings.Right(Strings.Trim(str), 1));
                    string left = text;
                    flag = (Operators.CompareString(left, "n", false) == 0);
                    if (flag)
                    {
                        int arg_A6_0 = 1;
                        int num2 = num;
                        int num3 = arg_A6_0;
                        while (true)
                        {
                            int arg_D9_0 = num3;
                            int num4 = num2;
                            if (arg_D9_0 > num4)
                            {
                                break;
                            }
                            int value = br.ReadInt32("n:V");
                            list.Add(Conversions.ToString(value));
                            num3++;
                        }
                    }
                    else
                    {
                        flag = (Operators.CompareString(left, "h", false) == 0);
                        if (flag)
                        {
                            int arg_FD_0 = 1;
                            int num5 = num;
                            int num6 = arg_FD_0;
                            while (true)
                            {
                                int arg_12B_0 = num6;
                                int num4 = num5;
                                if (arg_12B_0 > num4)
                                {
                                    break;
                                }
                                string item = br.ReadHash("h:V");
                                list.Add(item);
                                num6++;
                            }
                        }
                        else
                        {
                            flag = (Operators.CompareString(left, "s", false) == 0);
                            if (flag)
                            {
                                int arg_14F_0 = 1;
                                int num7 = num;
                                int num8 = arg_14F_0;
                                while (true)
                                {
                                    int arg_1B5_0 = num8;
                                    int num4 = num7;
                                    if (arg_1B5_0 > num4)
                                    {
                                        break;
                                    }
                                    string item2 = "";
                                    int num9 = br.ReadInt32("StrLen") - 1;
                                    flag = (num9 > 0);
                                    if (flag)
                                    {
                                        item2 = br.ReadString("s:V");
                                    }
                                    br.SkipBytes(1, "StringTerminator");
                                    list.Add(item2);
                                    num8++;
                                }
                            }
                            else
                            {
                                flag = (Operators.CompareString(left, "x", false) == 0);
                                if (flag)
                                {
                                    br.SkipBytes(num, "x:Skip");
                                }
                                else
                                {
                                    R = this.CritError("Function Misconfiguration: " + text);
                                }
                            }
                        }
                    }
                }
                return list.ToArray();
            }
        }

        private void ParseAtkDefSet(ref bool R)
        {
            List<Plugin.ItemNanoKeyVal> list = new List<Plugin.ItemNanoKeyVal>();
            List<Plugin.ItemNanoKeyVal> list2 = new List<Plugin.ItemNanoKeyVal>();
            br.SkipBytes(4, "AtkDefSkip");
            int num = br.ReadCNum("MaxSet");
            int arg_34_0 = 1;
            int num2 = num;
            int num3 = arg_34_0;
            checked
            {
                while (true)
                {
                    int arg_163_0 = num3;
                    int num4 = num2;
                    if (arg_163_0 > num4)
                    {
                        break;
                    }
                    int value = br.ReadInt32("Key");
                    int num5 = br.ReadCNum("Sets");
                    int arg_63_0 = 1;
                    int num6 = num5;
                    int num7 = arg_63_0;
                    while (true)
                    {
                        int arg_152_0 = num7;
                        num4 = num6;
                        if (arg_152_0 > num4)
                        {
                            break;
                        }
                        Plugin.ItemNanoKeyVal item = default(Plugin.ItemNanoKeyVal);
                        item.AttrKey = br.ReadInt32("AttrKey");
                        item.AttrVal = br.ReadInt32("AttrVal");
                        while (true)
                        {
                            switch (value)
                            {
                                case 3:
                                    {
                                        bool flag = Parser.XH.FileVer == 15000100 && this.AOID == 213413;
                                        if (flag)
                                        {
                                            value = 13;
                                            continue;
                                        }
                                        goto IL_10D;
                                    }
                                case 12:
                                    goto IL_110;
                                case 13:
                                    goto IL_11C;
                            }
                            goto Block_1;
                        }
                    IL_142:
                        num7++;
                        continue;
                    IL_10D:
                        goto IL_142;
                    IL_110:
                        list.Add(item);
                        goto IL_142;
                    IL_11C:
                        list2.Add(item);
                        goto IL_142;
                    IL_128:
                        R = this.CritError("Unhandled AtkDef Set: " + Conversions.ToString(value));
                        goto IL_142;
                    Block_1:
                        goto IL_128;
                    }
                    num3++;
                }
                try
                {
                    Output.ItemNanoAttackAndDefense(list.ToArray(), list2.ToArray());
                }
                catch (SQLiteException expr_182)
                {
                    ProjectData.SetProjectError(expr_182);
                    ReturnCode = ParserReturns.Crashed;
                    ProjectData.ClearProjectError();
                }
                catch (Exception expr_198)
                {
                    ProjectData.SetProjectError(expr_198);
                    Exception ex = expr_198;
                    CustomException ex2 = new CustomException(ex.ToString(), "Plugin Error");
                    ReturnCode = ParserReturns.Crashed;
                    R = false;
                    ProjectData.ClearProjectError();
                }
            }
        }
    }
}