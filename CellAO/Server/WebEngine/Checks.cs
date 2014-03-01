
namespace WebEngine
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Mime;
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
                //url.DownloadFile("http://windows.php.net/downloads/releases/archives/php-5.3.3-Win32-VC9-x86.zip", "php-5.3.3-Win32-VC9-x86.zip");
                url.Dispose();
                this.UrlDownloadFileCompleted("php-5.5.9-nts-Win32-VC11-x86.zip");
                //this.UrlDownloadFileCompleted("php-5.3.3-Win32-VC9-x86.zip");
            }
            else { Console.WriteLine("Php exists.");}
        }

        private void UrlDownloadFileCompleted(string file)
        {
            
            Console.WriteLine("Download Complete.");
            Console.WriteLine();
            Console.WriteLine("Unzipping File...");
            this.Unzip(file);
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
                var url = new WebClient();
                url.DownloadFile("http://www.aocell.info/php.ini", _config.Instance.CurrentConfig.WebHostPhpPath + @"\php.ini");
                //File.Copy(_config.Instance.CurrentConfig.WebHostPhpPath+@"\php.ini-production",_config.Instance.CurrentConfig.WebHostPhpPath+@"\php.ini");
                Directory.CreateDirectory(@"c:\temp");
                Console.WriteLine("Done.");
            }
        }

        public void CheckWebCore()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostRoot) == false)
            {
                var url = new WebClient();
                Console.WriteLine("Downloading WebCore...");
                url.DownloadFile(_config.Instance.CurrentConfig.WebCoreRepo, "WebCore.zip");
                Console.WriteLine("Download Complete.");
                Console.WriteLine();
                Console.WriteLine("Unzipping File...");
                this.Unzip2("WebCore.zip");
                string[] coreDirectories = System.IO.Directory.GetDirectories(_config.Instance.CurrentConfig.WebHostRoot);

                foreach (string coreDirectory in coreDirectories)
                {
                    string[] files = System.IO.Directory.GetFiles(coreDirectory);
                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(s);
                        string destFile = System.IO.Path.Combine(_config.Instance.CurrentConfig.WebHostRoot, fileName);
                        System.IO.File.Move(s, destFile);
                    }

                    files = System.IO.Directory.GetDirectories(coreDirectory);
                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(s);
                        string destFile = System.IO.Path.Combine(_config.Instance.CurrentConfig.WebHostRoot, fileName);
                        System.IO.Directory.Move(s, destFile);
                    }
                    System.IO.Directory.Delete(coreDirectory);
                }
            }
            else { Console.WriteLine("Webcore Exists.");}
        }


        private void Unzip2(string file)
        {
            using (var zip = ZipFile.Read(file))
            {
                foreach (ZipEntry ze in zip)
                {
                    ze.Extract(_config.Instance.CurrentConfig.WebHostRoot, ExtractExistingFileAction.OverwriteSilently);
                }
                zip.Dispose();
                Console.WriteLine("Done.");
            }
        }
    }
}
