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

namespace Extractor_Serializer
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using CellAO.Core.Actions;
    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Items;
    using CellAO.Core.Nanos;
    using CellAO.Core.Requirements;
    using CellAO.Enums;

    using MsgPack;

    using NiceHexOutput;

    #endregion

    /// <summary>
    /// Parser for serialized items of the RDB
    /// </summary>
    public class NewParser
    {
        #region Fields

        /// <summary>
        /// The br.
        /// </summary>
        public BufferedReader br;

        /// <summary>
        /// The function sets.
        /// </summary>
        private Dictionary<string, string> FunctionSets;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewParser"/> class.
        /// </summary>
        public NewParser()
        {
            this.LoadFunctionSets();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The parse anim sound set.
        /// </summary>
        /// <param name="typeN">
        /// The type n.
        /// </param>
        /// <param name="ITEM">
        /// The item.
        /// </param>
        public void ParseAnimSoundSet(int typeN, ItemTemplate ITEM)
        {
            int num = this.br.ReadInt32();
            int num2 = this.br.Read3F1();
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
                    int actionNum = this.br.ReadInt32();
                    int num6 = this.br.Read3F1();
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

                        int item = this.br.ReadInt32();
                        list.Add(item);
                        num8++;
                    }

                    // TODO: Add to item class
                    num4++;
                }
            }
        }

        /// <summary>
        /// The parse item.
        /// </summary>
        /// <param name="rectype">
        /// The rectype.
        /// </param>
        /// <param name="recnum">
        /// The recnum.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="itemnamessql">
        /// </param>
        /// <returns>
        /// The <see cref="AOItem"/>.
        /// </returns>
        public ItemTemplate ParseItem(int rectype, int recnum, byte[] data, List<string> itemnamessql)
        {
            this.br = new BufferedReader(rectype, recnum, data);
            ItemTemplate aoi = new ItemTemplate();
            aoi.ID = recnum;
            this.br.Skip(16);

            int num = this.br.Read3F1();
            int argc0 = 0;

            int num2 = num - 1;
            int num3 = argc0;

            while (true)
            {
                int arg1c2 = num3;
                int num4 = num2;
                if (arg1c2 > num4)
                {
                    break;
                }

                int attrkey = this.br.ReadInt32();
                int attrval = this.br.ReadInt32();
                if (attrkey == 54)
                {
                    aoi.Quality = attrval;
                }
                else
                {
                    aoi.Stats.Add(attrkey, attrval);
                }

                num3++;
            }

            this.br.Skip(8);

            short num5 = this.br.ReadInt16();
            short num6 = this.br.ReadInt16();
            string itemname = string.Empty;
            if (num5 > 0)
            {
                itemname = this.br.ReadString(num5);
            }

            if (itemnamessql != null)
            {
                itemnamessql.Add("(" + recnum + ",'" + itemname.Replace("'", "''") + "')");
            }

            if (num6 > 0)
            {
                this.br.ReadString(num6); // Read and discard Description
            }

            bool flag4 = true;
            checked
            {
                while (this.br.Ptr < this.br.Buffer.Length - 8 && flag4)
                {
                    switch (this.br.ReadInt32())
                    {
                        case 2:
                            this.ParseFunctionSet(aoi.Events);
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
                            this.ParseAtkDefSet(aoi.Attack, aoi.Defend);
                            break;
                        case 6:
                        {
                            this.br.Skip(4);
                            int count = this.br.Read3F1() * 8;
                            this.br.Skip(count);
                            break;
                        }

                        case 14:
                            this.ParseAnimSoundSet(1, aoi);
                            break;
                        case 20:
                            this.ParseAnimSoundSet(2, aoi);
                            break;
                        case 22:
                            this.ParseActionSet(aoi.Actions);
                            break;
                        case 23:
                            this.ParseShopHash(aoi.Events);
                            break;
                        default:
                            goto IL_4BF;
                    }

                    continue;
                    IL_4BF:
                    flag4 = false;
                }
            }

            return aoi;
        }

        /// <summary>
        /// The parse nano.
        /// </summary>
        /// <param name="rectype">
        /// The rectype.
        /// </param>
        /// <param name="recnum">
        /// The recnum.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="sqlFile">
        /// The sql file.
        /// </param>
        /// <returns>
        /// The <see cref="AONanos"/>.
        /// </returns>
        public NanoFormula ParseNano(int rectype, int recnum, byte[] data, string sqlFile)
        {
            this.br = new BufferedReader(rectype, recnum, data);
            NanoFormula aon = new NanoFormula();
            aon.ID = recnum;
            this.br.Skip(16);

            int numberOfAttributes = this.br.Read3F1() - 1;
            int counter = 0;

            while (true)
            {
                if (counter > numberOfAttributes)
                {
                    break;
                }

                int attrkey = this.br.ReadInt32();
                int attrval = this.br.ReadInt32();
                if (attrkey == 54)
                {
                    aon.Stats.Add(attrkey, attrval);
                }
                else
                {
                    aon.Stats.Add(attrkey, attrval);
                }

                counter++;
            }

            this.br.Skip(8);

            short nameLength = this.br.ReadInt16();
            short descriptionLength = this.br.ReadInt16();
            if (nameLength > 0)
            {
                this.br.ReadString(nameLength);
            }

            if (descriptionLength > 0)
            {
                this.br.ReadString(descriptionLength); // Read and discard Description
            }

            bool flag4 = true;
            checked
            {
                while (this.br.Ptr < this.br.Buffer.Length - 8 && flag4)
                {
                    switch (this.br.ReadInt32())
                    {
                        case 2:
                            this.ParseFunctionSet(aon.Events);
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
                            this.ParseAtkDefSet(aon.Attack, aon.Defend);
                            break;
                        case 6:
                        {
                            this.br.Skip(4);
                            int count = this.br.Read3F1() * 8;
                            this.br.Skip(count);
                            break;
                        }

                        case 14:
                            this.ParseAnimSoundSet(1, null);
                            break;
                        case 20:
                            this.ParseAnimSoundSet(2, null);
                            break;
                        case 22:
                            this.ParseActionSet(aon.Actions);
                            break;
                        case 23:
                            this.ParseShopHash(aon.Events);
                            break;
                        default:
                            goto IL_4BF;
                    }

                    continue;
                    IL_4BF:
                    flag4 = false;
                }
            }

            return aon;
        }

        /// <summary>
        /// The parse reqs.
        /// </summary>
        /// <param name="rreqs">
        /// The rreqs.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Requirements> ParseReqs(List<rawreqs> rreqs)
        {
            int numreqs = rreqs.Count;

            List<Requirements> output = new List<Requirements>();

            for (int i = 0; i < numreqs; i++)
            {
                rawreqs rr = rreqs[i];
                Requirements aor = new Requirements();

                aor.Target = ItemTarget.Self; // 0x13
                aor.Statnumber = rr.stat;
                aor.Operator = (Operator) Enum.ToObject(typeof(Operator), rr.ops);
                aor.Value = rr.val;
                aor.ChildOperator = Operator.Unknown;

                if ((i < numreqs - 1)
                    && (
                    (aor.Operator == Operator.OnTarget) 
                    || (aor.Operator == Operator.OnSelf) 
                    || (aor.Operator == Operator.OnUser)
                        || (aor.Operator == Operator.OnValidTarget) 
                        || (aor.Operator == Operator.OnInvalidTarget) 
                        || (aor.Operator == Operator.OnValidUser)
                        || (aor.Operator == Operator.OnInvalidUser) 
                        || (aor.Operator == Operator.OnGeneralBeholder) 
                        || (aor.Operator == Operator.OnCaster)
                        || (aor.Operator == Operator.Unknown2)))
                {
                    aor.Target = (ItemTarget) (int)aor.Operator;
                    i++;
                    rr = rreqs[i];
                    aor.Statnumber = rr.stat;
                    aor.Value = rr.val;
                    aor.Operator = (Operator)Enum.ToObject(typeof(Operator), rr.ops);
                }

                if (!((i >= numreqs - 1) || (numreqs == 2)))
                {
                    int anum = rreqs[i + 1].stat;
                    int aval = rreqs[i + 1].val;
                    int aop = rreqs[i + 1].ops;

                    if ((((aop == (int)Operator.Or) || (aop == (int)Operator.And)) || (aop == (int)Operator.Not)) && (anum == (int)Operator.EqualTo))
                    {
                        aor.ChildOperator = (Operator)Enum.ToObject(typeof(Operator), aop);
                        i++;
                    }
                }

                output.Add(aor);
            }

            for (int i = 0; i < output.Count - 2; i++)
            {
                output[i].ChildOperator = output[i + 1].ChildOperator;
            }

            return output;
        }

        /// <summary>
        /// The read reqs.
        /// </summary>
        /// <param name="numreqs">
        /// The numreqs.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Requirements> ReadReqs(int numreqs)
        {
            int num4 = numreqs;
            bool flag = num4 > 0;
            if (flag)
            {
                List<rawreqs> list = new List<rawreqs>();
                int arg_86_0 = 0;
                int num5 = num4 - 1;
                int num6 = arg_86_0;
                while (true)
                {
                    int arg_F0_0 = num6;
                    if (arg_F0_0 > num5)
                    {
                        break;
                    }

                    int stat = this.br.ReadInt32();
                    int val = this.br.ReadInt32();
                    int ops = this.br.ReadInt32();
                    rawreqs r = new rawreqs();
                    r.ops = ops;
                    r.stat = stat;
                    r.val = val;
                    list.Add(r);

                    num6++;
                }

                return this.ParseReqs(list);
            }

            return new List<Requirements>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load function sets.
        /// </summary>
        private void LoadFunctionSets()
        {
            this.FunctionSets = new Dictionary<string, string>();
            TextReader tr = new StreamReader("FunctionSets.cfg", Encoding.GetEncoding("windows-1252"));
            string line;
            while ((line = tr.ReadLine()) != null)
            {
                if (line != string.Empty)
                {
                    string[] parts = line.Split('=');
                    this.FunctionSets.Add(parts[0], parts[1]);
                }
            }

            tr.Close();
        }

        /// <summary>
        /// The parse action set.
        /// </summary>
        /// <param name="actions">
        /// The actions.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private void ParseActionSet(List<Actions> actions)
        {
            bool flag = this.br.ReadInt32() != 36;
            if (flag)
            {
                throw new Exception("Why am I here?");
            }

            int arg_3D_0 = 1;
            int num = this.br.Read3F1();
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

                    int actionNum = this.br.ReadInt32();

                    Actions aoa = new Actions();
                    aoa.ActionType = (ActionType) Enum.ToObject(typeof(ActionType), actionNum);

                    int numreqs = this.br.Read3F1();
                    List<Requirements> cookedreqs = this.ReadReqs(numreqs);
                    foreach (Requirements REQ in cookedreqs)
                    {
                        aoa.Requirements.Add(REQ);
                    }

                    if (actions == null)
                    {
                        actions = new List<Actions>();
                    }

                    actions.Add(aoa);
                    cookedreqs.Clear();
                    num2++;
                }
            }
        }

        /// <summary>
        /// The parse args.
        /// </summary>
        /// <param name="funcNum">
        /// The func num.
        /// </param>
        /// <param name="R">
        /// The r.
        /// </param>
        /// <returns>
        /// The <see cref="object[]"/>.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        private object[] ParseArgs(int funcNum, ref bool R)
        {
            bool flag = !this.FunctionSets.ContainsKey(funcNum.ToString());
            if (flag)
            {
                TextWriter lastitem = new StreamWriter("lastitem.txt");
                lastitem.WriteLine(NiceHexOutput.Output(this.br.Buffer));
                lastitem.Close();
                throw new IndexOutOfRangeException("Not handled function " + funcNum.ToString());
            }

            string[] array = this.FunctionSets[funcNum.ToString()].Split(',');
            List<object> list = new List<object>();
            string[] array2 = array;
            checked
            {
                for (int i = 0; i < array2.Length; i++)
                {
                    string str = array2[i];
                    int num = int.Parse(str.Trim().Substring(0, str.Length - 1));
                    string text = str.Trim().ToLower().Substring(str.Length - 1, 1);

                    // Strings.LCase(Strings.Right(Strings.Trim(str), 1));
                    string left = text;
                    flag = left == "n";
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

                            int value = this.br.ReadInt32();
                            list.Add(value);
                            num3++;
                        }
                    }
                    else
                    {
                        flag = left == "h";
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

                                string item = this.br.ReadHash();
                                list.Add(item);
                                num6++;
                            }
                        }
                        else
                        {
                            flag = left == "s";
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

                                    string item2 = string.Empty;
                                    int num9 = this.br.ReadInt32() - 1;
                                    flag = num9 > 0;
                                    if (flag)
                                    {
                                        item2 = this.br.ReadString();
                                    }

                                    this.br.Skip(1);
                                    list.Add(item2);
                                    num8++;
                                }
                            }
                            else
                            {
                                flag = left == "x";
                                if (flag)
                                {
                                    this.br.Skip(num);
                                }
                                else
                                {
                                    R = true;
                                }
                            }
                        }
                    }
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// The parse atk def set.
        /// </summary>
        /// <param name="attackstat">
        /// The attackstat.
        /// </param>
        /// <param name="defstat">
        /// The defstat.
        /// </param>
        private void ParseAtkDefSet(Dictionary<int, int> attackstat, Dictionary<int, int> defstat)
        {
            Dictionary<int, int> list = new Dictionary<int, int>();
            Dictionary<int, int> list2 = new Dictionary<int, int>();

            this.br.Skip(4);
            int num2 = this.br.Read3F1(); // Number of Attack/Defense Stat members
            int num3 = 1;
            checked
            {
                while (true)
                {
                    if (num3 > num2)
                    {
                        break;
                    }

                    int value = this.br.ReadInt32();
                    int numberOfMembers = this.br.Read3F1();
                    int num7 = 1;
                    while (true)
                    {
                        if (num7 > numberOfMembers)
                        {
                            break;
                        }

                        try
                        {
                            int attrkey = this.br.ReadInt32();
                            int attrval = this.br.ReadInt32();

                            if (value == 12)
                            {
                                list.Add(attrkey, attrval);
                                num7++;
                            }

                            if (value == 13)
                            {
                                list2.Add(attrkey, attrval);
                                num7++;
                            }
                        }
                        catch (Exception)
                        {
                            num7++;
                        }
                    }

                    num3++;
                }

                foreach (KeyValuePair<int, int> ua in list)
                {
                    attackstat.Add(ua.Key, ua.Value);
                }

                foreach (KeyValuePair<int, int> ua in list2)
                {
                    defstat.Add(ua.Key, ua.Value);
                }
            }
        }

        /// <summary>
        /// The parse function set.
        /// </summary>
        /// <param name="retlist">
        /// The retlist.
        /// </param>
        private void ParseFunctionSet(List<Events> retlist)
        {
            int eventTypeValue = this.br.ReadInt32();
            int num = this.br.Read3F1();
            List<Functions> list = new List<Functions>();
            int arg_2F_0 = 0;
            bool R;
            int num2 = num - 1;
            int num3 = arg_2F_0;
            while (true)
            {
                int arg_1C3_0 = num3;
                int num4 = num2;
                if (arg_1C3_0 > num4)
                {
                    break;
                }

                Functions func = new Functions();

                func.FunctionType = this.br.ReadInt32();
                this.br.Skip(8);
                int num5 = this.br.ReadInt32(); // Reqs
                bool flag = num5 > 0;
                if (flag)
                {
                    foreach (Requirements ur in this.ReadReqs(num5))
                    {
                        func.Requirements.Add(ur);
                    }
                }

                func.TickCount = this.br.ReadInt32();
                func.TickInterval = (uint)this.br.ReadInt32();
                func.Target = this.br.ReadInt32();

                this.br.Skip(4);
                R = false;
                foreach (object oo in this.ParseArgs(func.FunctionType, ref R))
                {
                    MessagePackObject x = MessagePackObject.FromObject(oo);
                    func.Arguments.Values.Add(x);
                }

                list.Add(func);
                num3++;
            }

            Events aoe = new Events();
            aoe.EventType = (EventType) Enum.ToObject(typeof(EventType), eventTypeValue);

            foreach (Functions ff in list)
            {
                aoe.Functions.Add(ff);
            }

            if (retlist == null)
            {
                retlist = new List<Events>();
            }

            retlist.Add(aoe);
        }

        /// <summary>
        /// The parse shop hash.
        /// </summary>
        /// <param name="events">
        /// The events.
        /// </param>
        private void ParseShopHash(List<Events> events)
        {
            int eventNum = this.br.ReadInt32();
            int num = this.br.Read3F1();
            int arg_2D_0 = 1;
            int num2 = num;
            int num3 = arg_2D_0;
            Events aoe = new Events();
            aoe.EventType = (EventType)Enum.ToObject(typeof(EventType), eventNum);
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

                    string text = this.br.ReadString(4);
                    int num5 = this.br.ReadByte();
                    int num6 = this.br.ReadByte();
                    bool flag = num5 == 0 && num6 == 0;
                    if (flag)
                    {
                        num5 = this.br.ReadInt16();
                        num6 = this.br.ReadInt16();
                    }

                    int count = Math.Min(11, this.br.Buffer.Length - this.br.Ptr);
                    this.br.Skip(count);

                    Functions aof = new Functions();
                    aof.Arguments.Values.Add(text);
                    aof.Arguments.Values.Add(num5);
                    aof.Arguments.Values.Add(num6);
                    aof.Target = 255;
                    aof.TickCount = 1;
                    aof.TickInterval = 0;
                    aof.FunctionType = (int)FunctionType.Shophash;
                    aoe.Functions.Add(aof);

                    num3++;
                }
            }

            if (events == null)
            {
                events = new List<Events>();
            }

            events.Add(aoe);
        }

        #endregion

        /// <summary>
        /// The buffered reader.
        /// </summary>
        public class BufferedReader
        {
            #region Fields

            /// <summary>
            /// The buffer.
            /// </summary>
            public byte[] Buffer;

            /// <summary>
            /// The ptr.
            /// </summary>
            public int Ptr;

            /// <summary>
            /// The record num.
            /// </summary>
            public int RecordNum;

            /// <summary>
            /// The record type.
            /// </summary>
            public int RecordType;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="BufferedReader"/> class.
            /// </summary>
            /// <param name="rectype">
            /// The rectype.
            /// </param>
            /// <param name="recnum">
            /// The recnum.
            /// </param>
            /// <param name="data">
            /// The data.
            /// </param>
            public BufferedReader(int rectype, int recnum, byte[] data)
            {
                this.RecordType = rectype;
                this.RecordNum = recnum;
                this.Buffer = data;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The read 3 f 1.
            /// </summary>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int Read3F1()
            {
                int num = BitConverter.ToInt32(this.Buffer, this.Ptr);
                num = (int)((long)Math.Round(Math.Round(unchecked(num / 1009.0 - 1.0))));
                this.Ptr += 4;
                return num;
            }

            /// <summary>
            /// The read byte.
            /// </summary>
            /// <returns>
            /// The <see cref="byte"/>.
            /// </returns>
            public byte ReadByte()
            {
                byte b = this.Buffer[this.Ptr];
                this.Ptr++;
                return b;
            }

            /// <summary>
            /// The read hash.
            /// </summary>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            public string ReadHash()
            {
                byte[] array = new byte[4];
                Array.Copy(this.Buffer, this.Ptr, array, 0, 4);
                Array.Reverse(array);
                this.Ptr += 4;
                return Encoding.ASCII.GetString(array);
            }

            /// <summary>
            /// The read int 16.
            /// </summary>
            /// <returns>
            /// The <see cref="short"/>.
            /// </returns>
            public short ReadInt16()
            {
                short num = BitConverter.ToInt16(this.Buffer, this.Ptr);
                this.Ptr += 2;
                return num;
            }

            /// <summary>
            /// The read int 32.
            /// </summary>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int ReadInt32()
            {
                int num = BitConverter.ToInt32(this.Buffer, this.Ptr);
                this.Ptr += 4;
                return num;
            }

            /// <summary>
            /// The read string.
            /// </summary>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            public string ReadString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                int arg_19_0 = this.Ptr;
                int num = this.Buffer.Length - 1;
                int num2 = arg_19_0;
                while (true)
                {
                    byte b = this.Buffer[num2];
                    if (this.Buffer[num2] == 0)
                    {
                        break;
                    }

                    stringBuilder.Append((char)this.Buffer[num2]);
                    num2++;
                    if (num2 > this.Buffer.Length - 1)
                    {
                        break;
                    }
                }

                string text = stringBuilder.ToString();
                this.Ptr += text.Length;
                return text;
            }

            /// <summary>
            /// The read string.
            /// </summary>
            /// <param name="Length">
            /// The length.
            /// </param>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            public string ReadString(int Length)
            {
                string result = Encoding.UTF8.GetString(this.Buffer, this.Ptr, Length);
                this.Ptr += Length;
                return result;
            }

            /// <summary>
            /// The skip.
            /// </summary>
            /// <param name="Count">
            /// The count.
            /// </param>
            public void Skip(int Count)
            {
                this.Ptr += Count;
            }

            #endregion
        }

        /// <summary>
        /// The rawreqs.
        /// </summary>
        public class rawreqs
        {
            #region Fields

            /// <summary>
            /// The ops.
            /// </summary>
            public int ops = 0;

            /// <summary>
            /// The stat.
            /// </summary>
            public int stat = 0;

            /// <summary>
            /// The val.
            /// </summary>
            public int val = 0;

            #endregion
        }
    }
}