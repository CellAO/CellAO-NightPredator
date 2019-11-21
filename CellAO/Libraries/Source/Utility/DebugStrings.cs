#region License

// Copyright (c) 2005-2014, CellAO Team
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

    public static class DebugStrings
    {
        private const int columnWidth = 25;

        public static string DebugString(this UInt16 v)
        {
            return v.ToString().PadLeft(columnWidth) + " " + v.ToString("X4").PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this UInt16? v)
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(UInt16?).Name;
            }
            else
            {
                return ((UInt16)v).ToString().PadLeft(columnWidth) + " " + ((UInt16)v).ToString("X4").PadRight(columnWidth)
                       + v.GetType().Name;
            }
        }

        public static string DebugString(this Int16 v)
        {
            return v.ToString().PadLeft(columnWidth) + " " + v.ToString("X4").PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this Int16? v)
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(Int16?).Name;
            }
            else
            {
                return ((Int16)v).ToString().PadLeft(columnWidth) + " " + ((Int16)v).ToString("X4").PadRight(columnWidth)
                       + v.GetType().Name;
            }
        }

        public static string DebugString(this UInt32 v)
        {
            return v.ToString().PadLeft(columnWidth) + " " + v.ToString("X8").PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this UInt32? v)
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(UInt32?).Name;
            }
            else
            {
                return ((UInt32)v).ToString().PadLeft(columnWidth) + " " + ((UInt32)v).ToString("X8").PadRight(columnWidth)
                       + v.GetType().Name;
            }
        }

        public static string DebugString(this Int32 v)
        {
            return v.ToString().PadLeft(columnWidth) + " " + v.ToString("X8").PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this Int32? v)
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(Int32?).Name;
            }
            else
            {
                return ((Int32)v).ToString().PadLeft(columnWidth) + " " + ((Int32)v).ToString("X8").PadRight(columnWidth)
                       + v.GetType().Name;
            }
        }

        public static string DebugString(this string v)
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(string).Name;
            }
            return v.PadRight(columnWidth * 2) + v.GetType().Name;
        }

        public static string DebugString(this byte v)
        {
            return v.ToString().PadLeft(columnWidth) + " " + v.ToString("X2").PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this byte? v)
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(byte?).Name;
            }
            return v.ToString().PadLeft(columnWidth) + " " + ((byte)v).ToString("X2").PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this Single v)
        {
            return v.ToString("F").PadLeft(columnWidth) + " " + "".PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this Identity v)
        {
            return v.Type.ToString().PadLeft(columnWidth) + " " + v.ToString().PadRight(columnWidth) + v.GetType().Name;
        }

        public static string DebugString(this Enum v)
        {
            Type t = Enum.GetUnderlyingType(v.GetType());
            if (t == typeof(byte))
            {
                return v.ToString().PadLeft(columnWidth) + " " + ((byte)Enum.Parse(v.GetType(), v.ToString())).ToString("X2").PadRight(columnWidth)
                       + v.GetType().Name;
            }
            else if (t == typeof(Int16))
            {
                return v.ToString().PadLeft(columnWidth) + " "
                       + ((Int16)Enum.Parse(v.GetType(), v.ToString())).ToString("X4").PadRight(columnWidth) + v.GetType().Name;
            }
            else if (t == typeof(Int32))
            {
                return v.ToString().PadLeft(columnWidth) + " "
                       + ((Int32)Enum.Parse(v.GetType(), v.ToString())).ToString("X8").PadRight(columnWidth) + v.GetType().Name;
            }
            else if (t == typeof(UInt16))
            {
                return v.ToString().PadLeft(columnWidth) + " "
                       + ((UInt16)Enum.Parse(v.GetType(), v.ToString())).ToString("X4").PadRight(columnWidth) + v.GetType().Name;
            }
            else if (t == typeof(UInt32))
            {
                return v.ToString().PadLeft(columnWidth) + " "
                       + ((UInt32)Enum.Parse(v.GetType(), v.ToString())).ToString("X8").PadRight(columnWidth) + v.GetType().Name;
            }
            else
            {
                return v.ToString().PadLeft(columnWidth * 2) + v.GetType().Name;
            }
        }

        public static string DebugString<T>(this T v) where T : class
        {
            if (v == null)
            {
                return "null".PadRight(columnWidth * 2) + typeof(T).Name;
            }

            Stack<Type> typeStack = new Stack<Type>();
            Type baseType = v.GetType();
            do
            {
                typeStack.Push(baseType);
                baseType = baseType.BaseType;
            }
            while (baseType != null);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Class: " + v.GetType().Name);

            while (typeStack.Count > 0)
            {
                Type t = typeStack.Pop();
                IEnumerable<PropertyInfo> p =
                    from property in t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    let memberAttribute =
                        property.GetCustomAttributes(typeof(AoMemberAttribute), false)
                            .Cast<AoMemberAttribute>()
                            .FirstOrDefault()
                    where property.CanWrite && memberAttribute != null && property.DeclaringType == t
                    orderby memberAttribute.Order ascending
                    select property;

                foreach (PropertyInfo pi in p)
                {
                    sb.AppendLine(pi.Name.PadRight(columnWidth) + Switch(pi.PropertyType, pi.GetValue(v, null)));
                }
            }
            return sb.ToString();
        }



        private static string Switch(Type t, object v)
        {
            if (t.IsArray)
            {
                if (v == null)
                {
                    return "null".PadRight(columnWidth) + t.Name;
                }
                StringBuilder sb = new StringBuilder(v.GetType().Name);
                sb.AppendLine();

                if (v.GetType().GetElementType() == typeof(Int32))
                {
                    foreach (var u in (Int32[])v)
                    {
                        sb.AppendLine(u.DebugString());
                    }
                }
                else
                {
                    if (v.GetType().GetElementType() == typeof(byte))
                    {
                        foreach (var u in (byte[])v)
                        {
                            sb.AppendLine(u.DebugString());
                        }
                    }
                    else
                    {
                        if (v.GetType().GetElementType() == typeof(Int16))
                        {
                            foreach (var u in (Int16[])v)
                            {
                                sb.AppendLine(u.DebugString());
                            }
                        }
                        else
                        {
                            if (v.GetType().GetElementType() == typeof(Identity))
                            {
                                foreach (var u in (Identity[])v)
                                {
                                    sb.AppendLine(u.DebugString());
                                }
                            }
                            else
                            {
                                foreach (var u in (object[])v)
                                {
                                    sb.AppendLine(u.DebugString());
                                }
                            }
                        }
                    }
                }


                return sb.ToString();
            }

            if ((v is ValueType) || (v is string))
            {
                if (t == typeof(byte))
                {
                    return ((byte)v).DebugString();
                }
                if (t == typeof(byte?))
                {
                    return ((byte?)v).DebugString();
                }
                if (t == typeof(Int16))
                {
                    return ((Int16)v).DebugString();
                }
                if (t == typeof(Int16?))
                {
                    return ((Int16?)v).DebugString();
                }
                if (t == typeof(Int32?))
                {
                    return ((Int32?)v).DebugString();
                }
                if (t == typeof(Int32))
                {
                    return ((Int32)v).DebugString();
                }
                if (t == typeof(UInt16?))
                {
                    return ((UInt16?)v).DebugString();
                }
                if (t == typeof(UInt32?))
                {
                    return ((UInt32?)v).DebugString();
                }
                if (t == typeof(UInt16))
                {
                    return ((UInt16)v).DebugString();
                }
                if (t == typeof(UInt32))
                {
                    return ((UInt32)v).DebugString();
                }
                if (t == typeof(Single))
                {
                    return ((Single)v).DebugString();
                }
                if (t.IsEnum)
                {
                    return ((Enum)v).DebugString();
                }
                if (t == typeof(string))
                {
                    return ((string)v).DebugString();
                }
            }
            if (v is Identity)
            {
                return ((Identity)v).DebugString();
            }
            return Environment.NewLine + v.DebugString();
        }
    }
}