using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _config = Utility.Config.ConfigReadWrite;

namespace WebEngine
{
    using System.IO;
    using System.Net;

    public static class Checks
    {
        public static void CheckPhp()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostPhpPath) == false)
            {
                var request = (HttpWebRequest)WebRequest.Create("http://windows.php.net/downloads/releases/php-5.5.9-nts-Win32-VC11-x86.zip");
                request.Method = "GET";
                request.ContentType = "application/zip";
                try
                {

                    var res = (HttpWebResponse)request.GetResponse();
                    using (var sr = new StreamReader(res.GetResponseStream(), Encoding.Default))
                    {
                        var oWriter = new StreamWriter(@"php-5.5.9-nts-Win32-VC11-x86.zip");
                        oWriter.Write(sr.ReadToEnd());
                        oWriter.Close();
                    }
                    res.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
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
