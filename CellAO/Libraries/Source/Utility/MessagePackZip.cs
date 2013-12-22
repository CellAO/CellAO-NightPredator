using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    using System.IO;

    using MsgPack.Serialization;

    using zlib;

    public static class MessagePackZip
    {
        public static void CompressData<T>(string filename, string version, List<T> dataList, int packCount = 500)
        {
            Console.WriteLine("Compressing " + typeof(T).Name + "s");

            if (packCount == 0)
            {
                throw new Exception("Dont use 0 as packCount!!");
            }
            Stream fileStream = new FileStream(filename, FileMode.Create);
            var zipStream = new ZOutputStream(fileStream, zlibConst.Z_BEST_COMPRESSION);
            var bufferStream = new MemoryStream();

            byte[] versionbuffer = Encoding.ASCII.GetBytes(version);
            bufferStream.WriteByte((byte)versionbuffer.Length);
            bufferStream.Write(versionbuffer, 0, versionbuffer.Length);

            byte[] buffer = BitConverter.GetBytes(packCount);
            bufferStream.Write(buffer, 0, buffer.Length);
            MessagePackSerializer<List<T>> bf = MessagePackSerializer.Create<List<T>>();
            List<T> temp = new List<T>();
            int counter = 0;
            foreach (T dataEntry in dataList)
            {
                temp.Add(dataEntry);
                if (temp.Count == packCount)
                {
                    bf.Pack(bufferStream, temp);
                    temp.Clear();
                    bufferStream.Flush();
                    counter++;
                    Console.Write(((counter * packCount) * 100 / dataList.Count).ToString().PadLeft(3) + "% serialized\r");
                }
            }
            if (temp.Count != 0)
            {
                bf.Pack(bufferStream, temp);
            }
            Console.WriteLine("100% serialized");
            bufferStream.Seek(0, SeekOrigin.Begin);
            CopyStream(bufferStream, zipStream);
            bufferStream.Close();
            zipStream.Close();
            fileStream.Close();
        }
        const int CopyBufferLength = 512 * 1024;
        public static void CopyStream(Stream input, Stream output, string text = "inflated")
        {
            var buffer = new byte[CopyBufferLength];
            int len;
            int done = 0;
            while ((len = input.Read(buffer, 0, CopyBufferLength)) > 0)
            {
                output.Write(buffer, 0, len);
                done += len;
                Console.Write("\r" + (done * 100 / input.Length).ToString().PadLeft(3) + "% " + text);
            }

            output.Flush();
            Console.WriteLine("\r100% "+text);
        }


        public static List<T> UncompressData<T>(string fname)
        {
            var tempList = new List<T>();
            Stream fileStream = new FileStream(fname, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream();

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

            while (true)
            {
                try
                {
                    List<T> temp1 = messagePackSerializer.Unpack(memoryStream);
                    tempList.AddRange(temp1);
                }
                catch (Exception)
                {
                    break;
                }
            }
            return tempList;
        }

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
            var zipStream = new ZOutputStream(fileStream, zlibConst.Z_BEST_COMPRESSION);
            var bufferStream = new MemoryStream();

            byte[] versionbuffer = Encoding.ASCII.GetBytes(version);
            bufferStream.WriteByte((byte)versionbuffer.Length);
            bufferStream.Write(versionbuffer, 0, versionbuffer.Length);

            byte[] buffer = BitConverter.GetBytes(packCount);
            bufferStream.Write(buffer, 0, buffer.Length);
            MessagePackSerializer<Dictionary<T, TU>> bf = MessagePackSerializer.Create<Dictionary<T, TU>>();
            Dictionary<T,TU> temp = new Dictionary<T,TU>();
            int counter = 0;
            foreach (KeyValuePair<T,TU> dataEntry in dataList)
            {
                temp.Add(dataEntry.Key,dataEntry.Value);
                if (temp.Count == packCount)
                {
                    bf.Pack(bufferStream, temp);
                    temp.Clear();
                    bufferStream.Flush();
                    counter++;
                    Console.Write(((counter * packCount) * 100 / dataList.Count).ToString().PadLeft(3) + "% serialized\r");
                }
            }
            if (temp.Count != 0)
            {
                bf.Pack(bufferStream, temp);
            }
            Console.WriteLine("100% serialized");
            bufferStream.Seek(0, SeekOrigin.Begin);
            CopyStream(bufferStream, zipStream);
            bufferStream.Close();
            zipStream.Close();
            fileStream.Close();
        }
        public static Dictionary<T,TU> UncompressData<T,TU>(string fname)
        {
            var tempList = new Dictionary<T,TU>();
            Stream fileStream = new FileStream(fname, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream();

            ZOutputStream zOutputStream = new ZOutputStream(memoryStream);
            CopyStream(fileStream, zOutputStream, "deflated");

            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            byte versionlength = (byte)memoryStream.ReadByte();
            char[] version = new char[versionlength];
            version = binaryReader.ReadChars(versionlength);

            // TODO: Check version and print a warning if not same as config.xml's
            MessagePackSerializer<Dictionary<T, TU>> messagePackSerializer = MessagePackSerializer.Create<Dictionary<T, TU>>();

            var buffer = new byte[4];
            memoryStream.Read(buffer, 0, 4);
            int packaged = BitConverter.ToInt32(buffer, 0);

            while (true)
            {
                try
                {
                    Dictionary<T,TU> temp1 = messagePackSerializer.Unpack(memoryStream);
                    foreach (var tempEntry in temp1)
                    {
                        tempList.Add(tempEntry.Key,tempEntry.Value);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return tempList;
        }

    }
}
