
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
                Console.WriteLine("Downloading SVN...");
                url.DownloadFile("http://downloads.sourceforge.net/project/win32svn/1.7.5/svn-win32-1.7.5.zip?r=http%3A%2F%2Fsourceforge.net%2Fprojects%2Fwin32svn%2Ffiles%2F1.7.5%2F&ts=1392843117&use_mirror=softlayer-dal", "svn.zip");
                UrlDownloadFileCompleted2("svn.zip");
            }
            else { Console.WriteLine("Webcore Exists.");}
        }

        private void UrlDownloadFileCompleted2(string file)
        {

            Console.WriteLine("Download Complete.");
            Console.WriteLine();
            Console.WriteLine("Unzipping File...");
            this.Unzip2(file);
        }

        private void Unzip2(string file)
        {
            using (var zip = ZipFile.Read(file))
            {
                foreach (ZipEntry ze in zip)
                {
                    ze.Extract(ExtractExistingFileAction.OverwriteSilently);
                }
                zip.Dispose();
                Console.WriteLine("Done.");
                Console.WriteLine();
                Console.WriteLine("Deleting " + Convert.ToString(file) + "...");
                File.Delete(file);
                Console.WriteLine();
                Console.WriteLine("Done.");
                Checkoutsvn();
            }
        }

        private void Checkoutsvn()
        {
            Console.WriteLine();
            Console.WriteLine("Checking out SVN...");
            var proc = Process.Start(@"svn-win32-1.7.5\bin\svn.exe", "co https://simplesqlcellaowebcore.googlecode.com/svn/trunk/webcore" + " " + _config.Instance.CurrentConfig.WebHostRoot);
            if (proc != null)
            {
                proc.WaitForExit();
            }
            Console.WriteLine();
            Console.WriteLine("Done.");
            Directory.Delete("svn-win32-1.7.5", true);
        }
    }
}
