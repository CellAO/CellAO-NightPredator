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
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    using CellAO.Core.Functions;
    using CellAO.Core.Requirements;

    using MsgPack;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public class HLFlatFunction : IFCSerializable
    {
        #region Fields

        /// <summary>
        /// </summary>
        public FunctionArguments Arguments = new FunctionArguments();

        /// <summary>
        /// </summary>
        public int FunctionType = 0;

        /// <summary>
        /// </summary>
        public List<Requirements> Requirements = new List<Requirements>();

        /// <summary>
        /// </summary>
        public int Target;

        /// <summary>
        /// </summary>
        public int TickCount;

        /// <summary>
        /// </summary>
        public uint TickInterval;

        /// <summary>
        /// </summary>
        private Dictionary<string, string> FunctionSets = new Dictionary<string, string>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public HLFlatFunction()
        {
            this.LoadFunctionSets();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="rreqs">
        /// </param>
        /// <returns>
        /// </returns>
        public List<Requirements> ParseReqs(List<rawreqs> rreqs)
        {
            int numreqs = rreqs.Count;

            List<Requirements> output = new List<Requirements>();

            for (int i = 0; i < numreqs; i++)
            {
                rawreqs rr = rreqs[i];
                Requirements aor = new Requirements();

                aor.Target = ItemTarget.Self;
                aor.Statnumber = rr.stat;
                aor.Operator = (Operator)Enum.ToObject(typeof(Operator), rr.ops);
                aor.Value = rr.val;
                aor.ChildOperator = Operator.Unknown;

                if ((i < numreqs - 1)
                    && ((aor.Operator == Operator.OnTarget) 
                    || (aor.Operator == Operator.OnSelf) 
                    || (aor.Operator == Operator.OnUser)
                        || (aor.Operator == Operator.OnValidTarget) 
                        || (aor.Operator == Operator.OnInvalidUser) 
                        || (aor.Operator == Operator.OnValidUser)
                        || (aor.Operator == Operator.OnInvalidUser)
                        || (aor.Operator == Operator.OnGeneralBeholder) 
                        || (aor.Operator == Operator.OnCaster)
                        || (aor.Operator == Operator.Unknown3)))
                {
                    aor.Target = (ItemTarget)Enum.ToObject(typeof(ItemTarget), aor.Operator);
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

                    if ((((aop == 3) || (aop == 4)) || (aop == 0x2a)) && (anum == 0))
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
        /// </summary>
        /// <param name="stream">
        /// </param>
        public void ReadFromStream(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            this.FunctionType = IPAddress.NetworkToHostOrder(br.ReadInt32());

            // Skip 8
            br.ReadInt64();

            int reqCount = IPAddress.NetworkToHostOrder(br.ReadInt32());
            List<rawreqs> raws = new List<rawreqs>();
            while (reqCount > 0)
            {
                int stat = IPAddress.NetworkToHostOrder(br.ReadInt32());
                int val = IPAddress.NetworkToHostOrder(br.ReadInt32());
                int ops = IPAddress.NetworkToHostOrder(br.ReadInt32());
                rawreqs r = new rawreqs();
                r.ops = ops;
                r.stat = stat;
                r.val = val;
                raws.Add(r);
                reqCount--;
            }

            foreach (Requirements req in this.ParseReqs(raws))
            {
                this.Requirements.Add(req);
            }

            this.TickCount = IPAddress.NetworkToHostOrder(br.ReadInt32());
            this.TickInterval = (uint)IPAddress.NetworkToHostOrder(br.ReadInt32());
            this.Target = IPAddress.NetworkToHostOrder(br.ReadInt32());
            br.ReadInt32();
            bool bool1 = false;
            foreach (object oo in this.ParseArgs(this.FunctionType, ref bool1, br))
            {
                MessagePackObject x = MessagePackObject.FromObject(oo);
                this.Arguments.Values.Add(x);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="br">
        /// </param>
        /// <returns>
        /// </returns>
        public string ReadHash(BinaryReader br)
        {
            byte[] array = new byte[4];
            array = br.ReadBytes(4);
            Array.Reverse(array);
            return Encoding.ASCII.GetString(array);
        }

        /// <summary>
        /// </summary>
        /// <param name="br">
        /// </param>
        /// <returns>
        /// </returns>
        public string ReadString(BinaryReader br)
        {
            int count = IPAddress.NetworkToHostOrder(br.ReadInt32());
            count--;
            StringBuilder stringBuilder = new StringBuilder();
            while (count > 0)
            {
                byte b = br.ReadByte();
                stringBuilder.Append((char)b);
                count--;
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal Functions ToFunctions()
        {
            Functions f = new Functions();
            f.FunctionType = this.FunctionType;
            f.Target = this.Target;
            f.TickCount = this.TickCount;
            f.TickInterval = this.TickInterval;
            f.Arguments = this.Arguments;
            f.Requirements = this.Requirements;
            return f;
        }

        /// <summary>
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
        /// </summary>
        /// <param name="funcNum">
        /// </param>
        /// <param name="R">
        /// </param>
        /// <param name="br">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        private object[] ParseArgs(int funcNum, ref bool R, BinaryReader br)
        {
            bool flag = !this.FunctionSets.ContainsKey(funcNum.ToString());
            if (flag)
            {
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

                            int value = IPAddress.NetworkToHostOrder(br.ReadInt32());
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

                                string item = this.ReadHash(br);
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
                                    item2 = this.ReadString(br);

                                    br.ReadByte();
                                    list.Add(item2);
                                    num8++;
                                }
                            }
                            else
                            {
                                flag = left == "x";
                                if (flag)
                                {
                                    br.ReadBytes(num);
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