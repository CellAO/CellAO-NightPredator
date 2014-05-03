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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MsgPack.Serialization;

    using Ionic.Zlib;

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

            // Need to build the serializer/deserializer prior to Task invocations
            MessagePackSerializer<List<T>> constructor = MessagePackSerializer.Create<List<T>>();

            Console.WriteLine("Compressing " + typeof(T).Name + "s");

            if (packCount == 0)
            {
                throw new Exception("Dont use 0 as packCount!!");
            }


            Stream fileStream = new FileStream(filename, FileMode.Create);
            var zlibStream = new ZlibStream(fileStream, CompressionMode.Compress, CompressionLevel.BestCompression);

            BinaryWriter binaryWriter = new BinaryWriter(zlibStream);

            byte[] versionbuffer = Encoding.ASCII.GetBytes(version);
            binaryWriter.Write((byte)versionbuffer.Length);
            binaryWriter.Write(versionbuffer, 0, versionbuffer.Length);

            binaryWriter.Write(packCount);
            int tempCapacity = dataList.Count;
            binaryWriter.Write(tempCapacity);

            int maxCount = dataList.Count;

            // Write number of slices
            int slices = Convert.ToInt32(Math.Ceiling((double)maxCount / packCount));
            binaryWriter.Write(slices);

            TaskedSerializer<T>[] taskData = new TaskedSerializer<T>[slices];

            Task[] tasks = new Task[taskData.Length];
            for (int i = 0; i < taskData.Count(); i++)
            {
                taskData[i] = new TaskedSerializer<T>(dataList.Skip(packCount * i).Take(packCount).ToList());
                int i1 = i;
                tasks[i] = new Task(() => taskData[i1].Serialize());
                tasks[i].Start();
            }

            // Wait for all serialization to finish
            Task.WaitAll(tasks);

            Console.WriteLine("100% serialized");

            // Write data streams
            foreach (TaskedSerializer<T> task in taskData)
            {
                task.Stream.Position = 0;
                int size = (int)task.Stream.Length;
                binaryWriter.Write(size);
                task.Stream.CopyTo(zlibStream);
                task.Dispose();
            }

            for (int i = 0; i < slices; i++)
            {
                taskData[i] = null;
                tasks[i].Dispose();
            }
            zlibStream.Close();
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
            var zipStream = new ZlibStream(fileStream, CompressionMode.Compress, CompressionLevel.BestCompression);
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
            bufferStream.CopyTo(zipStream);
            bufferStream.Close();
            zipStream.Close();
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
            // Need to build the serializer/deserializer prior to Task invocations
            MessagePackSerializer<List<T>> constructor = MessagePackSerializer.Create<List<T>>();

            Stream fileStream = new FileStream(fname, FileMode.Open);

            ZlibStream inputStream = new ZlibStream(fileStream, CompressionMode.Decompress);

            // CopyStream(fileStream, zOutputStream, "deflated");

            // memoryStream.Seek(0, SeekOrigin.Begin);

            BinaryReader binaryReader = new BinaryReader(inputStream);


            byte versionlength = (byte)inputStream.ReadByte();
            char[] version = new char[versionlength];
            version = binaryReader.ReadChars(versionlength);
            string versionString = "";
            foreach (char c in version)
            {
                versionString += c;
            }

            Console.WriteLine("Loading data for client version " + versionString);
            // TODO: Check version and print a warning if not same as config.xml's

            // packaged is unused here
            int packaged = binaryReader.ReadInt32();

            int capacity = binaryReader.ReadInt32();

            int slices = binaryReader.ReadInt32();

            TaskedSerializer<T>[] tasked = new TaskedSerializer<T>[slices];

            Task[] tasks = new Task[slices];

            for (int i = 0; i < slices; i++)
            {

                int size = binaryReader.ReadInt32();
                byte[] tempBuffer = new byte[size];
                inputStream.Read(tempBuffer, 0, size);
                using (MemoryStream tempStream = new MemoryStream(tempBuffer))
                {
                    tasked[i] = new TaskedSerializer<T>(tempStream);
                }
                int i1 = i;
                tasks[i] = new Task(() => tasked[i1].Deserialize());
                tasks[i].Start();
            }

            binaryReader.Close();
            Task.WaitAll(tasks);

            List<T> resultList = new List<T>(capacity);
            for (int i = 0; i < slices; i++)
            {
                resultList.AddRange(tasked[i].DataSlice);
                tasked[i].DataSlice.Clear();
                tasks[i].Dispose();
                tasked[i].Dispose();
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

            ZlibStream inputStream = new ZlibStream(fileStream, CompressionMode.Decompress);

            inputStream.Seek(0, SeekOrigin.Begin);
            BinaryReader binaryReader = new BinaryReader(inputStream);
            byte versionlength = binaryReader.ReadByte();
            char[] version = new char[versionlength];
            version = binaryReader.ReadChars(versionlength);

            // TODO: Check version and print a warning if not same as config.xml's
            MessagePackSerializer<Dictionary<T, TU>> messagePackSerializer =
                MessagePackSerializer.Create<Dictionary<T, TU>>();

            var buffer = new byte[4];
            inputStream.Read(buffer, 0, 4);

            inputStream.Read(buffer, 0, 4);

            return messagePackSerializer.Unpack(inputStream);
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

    internal class TaskedSerializer<T> : IDisposable
    {
        public MemoryStream Stream = new MemoryStream();

        public List<T> DataSlice = new List<T>();

        public TaskedSerializer(List<T> data)
        {
            this.DataSlice = data;
        }

        public TaskedSerializer(byte[] dataBytes)
        {
            this.Stream = new MemoryStream(dataBytes);
            this.Stream.Position = 0;
        }

        public TaskedSerializer(MemoryStream ms)
        {
            ms.CopyTo(this.Stream, (int)ms.Length);
            this.Stream.Position = 0;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        internal void Serialize()
        {
            MessagePackSerializer<List<T>> bf = MessagePackSerializer.Create<List<T>>();
            bf.Pack(this.Stream, this.DataSlice);
        }

        internal void Deserialize()
        {
            MessagePackSerializer<List<T>> messagePackSerializer = MessagePackSerializer.Create<List<T>>();
            this.DataSlice = messagePackSerializer.Unpack(this.Stream);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DataSlice.Clear();
                this.Stream.Dispose();
            }
        }
    }
}