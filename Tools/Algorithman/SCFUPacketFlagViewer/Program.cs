using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCFUPacketFlagViewer
{
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Write("Input packet flags (as hex):");
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                line = line.Replace(" ", "");
                uint packFlags;
                try
                {
                    packFlags=uint.Parse(line, System.Globalization.NumberStyles.HexNumber);

                }
                catch (Exception)
                {
                    packFlags = 0;
                    Console.WriteLine("Invalid hex number... tsktsktsk");                
                }

                SimpleCharFullUpdateFlags fl = (SimpleCharFullUpdateFlags)packFlags;

                foreach (
                    SimpleCharFullUpdateFlags flag in
                        (SimpleCharFullUpdateFlags[])Enum.GetValues(typeof(SimpleCharFullUpdateFlags)))
                {
                    if (fl.HasFlag(flag))
                    {
                        Console.WriteLine(((int)flag).ToString("X8")+" "+flag);
                    }
                }
                Console.WriteLine();
            }


        }
    }
}
