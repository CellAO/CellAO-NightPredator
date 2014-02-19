using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _config = Utility.Config.ConfigReadWrite;

namespace WebEngine
{
    using System.IO;

    public static class Checks
    {
        public static void CheckPhp()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostPhpPath) == false)
            {

            }
            else { Console.WriteLine("Php exists.");}
        }

        public static void CheckWebCore()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostRoot) == false)
            {
                
            }
            else { Console.WriteLine("Webcore Exists.");}
        }
    }
}
