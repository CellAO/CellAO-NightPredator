using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace ReadingPacketsFromADumpFile
{
  class Program
  {
    static List<object> objects = new List<object>();

    static void Main(string[] args)
    {
      // Check command line
      if (args.Length != 1)
      {
        Console.WriteLine("usage: " + Environment.GetCommandLineArgs()[0] + " <filename>");
        return;
      }

      // Create the offline device
      OfflinePacketDevice selectedDevice = new OfflinePacketDevice(args[0]);

      // Open the capture file
      using (PacketCommunicator communicator =
          selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                      // 65536 guarantees that the whole packet will be captured on all the link layers
                              PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                              1000))                                  // read timeout
      {
        // Read and dispatch packets until EOF is reached
        communicator.ReceivePackets(0, DispatcherHandler);
      }
      System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objects));
      
    }

    private static void DispatcherHandler(Packet packet)
    {      
      SmokeLounge.AOtomation.Messaging.Serialization.MessageSerializer serializer = new SmokeLounge.AOtomation.Messaging.Serialization.MessageSerializer();
      Byte[] bytes = packet.Buffer;

      Byte[] trimmedBytes = new Byte[bytes.Length - 40];
      Array.Copy(bytes, 40, trimmedBytes, 0, trimmedBytes.Length);
      if (trimmedBytes.Length == 0) return;


      var message = serializer.Deserialize(new System.IO.MemoryStream(trimmedBytes));
      if (message == null)
      {
        objects.Add(new
        {
          Info = "COULD NOT DESERIALIZE AGAINST KNOWN TYPES",
          Data = trimmedBytes,
          TotalLength = bytes.Length,
          DataLength = trimmedBytes.Length
        });
        //System.Console.WriteLine(String.Format("Unknown packet: {0}/{1}", trimmedBytes.Length, bytes.Length));
      }
      else
      {
        //System.Console.WriteLine(String.Format("Known packet: {0}", message.Body.GetType()));
        objects.Add(message);
        
      }
      Console.WriteLine();
    }
  }
}