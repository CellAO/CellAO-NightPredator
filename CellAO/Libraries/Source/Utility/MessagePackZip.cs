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
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using MsgPack.Serialization;

    using zlib;

    #endregion

    /// <summary>
    /// </summary>
    public static class MessagePackZip
    {
        #region Constants

        /// <summary>
        /// </summary>
        private const int CopyBufferLength = 2 * 1024 * 1024;

        #endregion

        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly byte[] copyBuffer = new byte[CopyBufferLength];

        #endregion

        #region Public Methods and Operators

        public static byte[] SerializeData<T>(List<T> dataList)
        {
            byte[] temp = null;
            MessagePackSerializer<List<T>> bf = MessagePackSerializer.Create<List<T>>();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Pack(ms, dataList);
                temp = ms.GetBuffer();
            }
            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="filename">
        /// </param>
        /// <param name="version">
        /// </param>
        /// <param name="dataList">
        /// </param>
        /// <param name="packCount">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="Exception">
        /// </exception>
        public static void CompressData<T>(string filename, string version, List<T> dataList, int packCount = 500)
        {
            Console.WriteLine("Compressing " + typeof(T).Name + "s");

            if (packCount == 0)
            {
                throw new Exception("Dont use 0 as packCount!!");
            }

            Stream fileStream = new FileStream(filename, FileMode.Create);
            var zipStream = new ZOutputStream(fileStream, zlibConst.Z_BEST_COMPRESSION);
            var bufferStream = new MemoryStream(20 * 1024 * 1024);

            byte[] versionbuffer = Encoding.ASCII.GetBytes(version);
            bufferStream.WriteByte((byte)versionbuffer.Length);
            bufferStream.Write(versionbuffer, 0, versionbuffer.Length);

            byte[] buffer = BitConverter.GetBytes(packCount);
            bufferStream.Write(buffer, 0, buffer.Length);
            int tempCapacity = dataList.Count;
            buffer = BitConverter.GetBytes(tempCapacity);
            bufferStream.Write(buffer, 0, buffer.Length);
            MessagePackSerializer<List<T>> bf = MessagePackSerializer.Create<List<T>>();

            int counter = 0;
            int maxCount = dataList.Count;
            List<T> tempList = new List<T>(tempCapacity);
            while (counter < maxCount)
            {
                tempList.Add(dataList[counter]);
                counter++;

                if (counter % packCount == 0)
                {
                    bf.Pack(bufferStream, tempList);
                    bufferStream.Flush();
                    tempList.Clear();
                }
            }

            if (tempList.Count > 0)
            {
                bf.Pack(bufferStream, tempList);
                bufferStream.Flush();
            }

            // bf.Pack(bufferStream, dataList);

            Console.WriteLine("100% serialized");
            bufferStream.Seek(0, SeekOrigin.Begin);
            CopyStream(bufferStream, zipStream);
            bufferStream.Close();
            zipStream.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="filename">
        /// </param>
        /// <param name="version">
        /// </param>
        /// <param name="dataList">
        /// </param>
        /// <param name="packCount">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TU">
        /// </typeparam>
        /// <exception cref="Exception">
        /// </exception>
        public static void CompressData<T, TU>(
            string filename,
            string version,
            Dictionary<T, TU> dataList,
            int packCount = 500)
        {
            Console.WriteLine("Compressing " + typeof(TU).Name + "s");

            if (packCount == 0)
            {
                throw new Exception("Dont use 0 as packCount!!");
            }

            Stream fileStream = new FileStream(filename, FileMode.Create);
            var zipStream = new ZOutputStream(fileStream, zlibConst.Z_HUFFMAN_ONLY);
            var bufferStream = new MemoryStream(20 * 1024 * 1024);

            byte[] versionbuffer = Encoding.ASCII.GetBytes(version);
            bufferStream.WriteByte((byte)versionbuffer.Length);
            bufferStream.Write(versionbuffer, 0, versionbuffer.Length);

            byte[] buffer = BitConverter.GetBytes(packCount);
            bufferStream.Write(buffer, 0, buffer.Length);
            packCount = dataList.Count;
            buffer = BitConverter.GetBytes(packCount);
            bufferStream.Write(buffer, 0, buffer.Length);

            MessagePackSerializer<Dictionary<T, TU>> bf = MessagePackSerializer.Create<Dictionary<T, TU>>();

            bf.Pack(bufferStream, dataList);

            Console.WriteLine("100% serialized");
            bufferStream.Seek(0, SeekOrigin.Begin);
            CopyStream(bufferStream, zipStream);
            bufferStream.Close();
            zipStream.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="input">
        /// </param>
        /// <param name="output">
        /// </param>
        /// <param name="text">
        /// </param>
        public static void CopyStream(Stream input, Stream output, string text = "inflated")
        {
            int len;
            int done = 0;
            while ((len = input.Read(copyBuffer, 0, CopyBufferLength)) > 0)
            {
                output.Write(copyBuffer, 0, len);

                // output.Flush();
                done += len;
                Console.Write("\r" + (done * 100 / input.Length).ToString().PadLeft(3) + "% " + text);
            }

            output.Flush();
            Console.WriteLine("\r100% " + text);
        }

        /// <summary>
        /// </summary>
        /// <param name="fname">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static List<T> UncompressData<T>(string fname)
        {
            Stream fileStream = new FileStream(fname, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream(4 * 1024 * 1024);

            ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
            CopyStream(fileStream, zOutputStream, "deflated");

            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            byte versionlength = (byte)memoryStream.ReadByte();
            char[] version = new char[versionlength];
            version = binaryReader.ReadChars(versionlength);

            // TODO: Check version and print a warning if not same as config.xml's
            MessagePackSerializer<List<T>> messagePackSerializer = MessagePackSerializer.Create<List<T>>();

            var buffer = new byte[4];
            memoryStream.Read(buffer, 0, 4);
            int packaged = BitConverter.ToInt32(buffer, 0);
            memoryStream.Read(buffer, 0, 4);

            int capacity = BitConverter.ToInt32(buffer, 0);

            List<T> resultList = new List<T>(capacity);
            while (true)
            {
                try
                {
                    resultList.AddRange(messagePackSerializer.Unpack(memoryStream));
                }
                catch (Exception)
                {
                    break;
                }
            }

            return resultList;
        }

        /// <summary>
        /// </summary>
        /// <param name="fname">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TU">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static Dictionary<T, TU> UncompressData<T, TU>(string fname)
        {
            var tempList = new Dictionary<T, TU>();
            Stream fileStream = new FileStream(fname, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream(20 * 1024 * 1024);

            ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
            CopyStream(fileStream, zOutputStream, "deflated");

            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            byte versionlength = (byte)memoryStream.ReadByte();
            char[] version = new char[versionlength];
            version = binaryReader.ReadChars(versionlength);

            // TODO: Check version and print a warning if not same as config.xml's
            MessagePackSerializer<Dictionary<T, TU>> messagePackSerializer =
                MessagePackSerializer.Create<Dictionary<T, TU>>();

            var buffer = new byte[4];
            memoryStream.Read(buffer, 0, 4);

            memoryStream.Read(buffer, 0, 4);

            return messagePackSerializer.Unpack(memoryStream);
        }

        #endregion

        public static List<T> DeserializeData<T>(byte[] data)
        {
            if (data.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    ms.Position = 0;
                    MessagePackSerializer<List<T>> bf = MessagePackSerializer.Create<List<T>>();
                    return bf.Unpack(ms);
                }
            }
            return new List<T>();
        }
    }
}