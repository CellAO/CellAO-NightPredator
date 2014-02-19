
namespace WebEngine
{
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Security.Policy;

    using Ionic.Zip;
    using System;
    using _config = Utility.Config.ConfigReadWrite;

    public  class Checks
    {
        public void CheckPhp()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostPhpPath) == false)
            {
                Console.WriteLine();
                 var url = new WebClient();
                 Console.WriteLine();
                Console.WriteLine("Downloading php..." );
                 url.DownloadFile("http://windows.php.net/downloads/releases/php-5.5.9-nts-Win32-VC11-x86.zip", "php-5.5.9-nts-Win32-VC11-x86.zip");
                url.Dispose();
                this.UrlDownloadFileCompleted();
            }
            else { Console.WriteLine("Php exists.");}
        }

        private void UrlDownloadFileCompleted()
        {
            
            Console.WriteLine("Download Complete.");
            Console.WriteLine();
            Console.WriteLine("Unzipping File...");
            this.Unzip("php-5.5.9-nts-Win32-VC11-x86.zip");
        }

        private void Unzip(string file)
        {
            using (var zip = ZipFile.Read(file))
            {
                foreach (ZipEntry ze in zip)
                {
                    ze.Extract(_config.Instance.CurrentConfig.WebHostPhpPath, ExtractExistingFileAction.OverwriteSilently);
                }
                zip.Dispose();
                Console.WriteLine("Done.");
                Console.WriteLine();
                Console.WriteLine("Deleting "+ Convert.ToString(file) + "...");
                File.Delete(file);
                Console.WriteLine();
                Console.WriteLine("Done.");
            }
        }

        public void CheckWebCore()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostRoot) == false)
            {
                
            }
            else { Console.WriteLine("Webcore Exists.");}
        }
    }
}
