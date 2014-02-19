using System;
using _config = Utility.Config.ConfigReadWrite;

namespace WebEngine
{
    using System.ComponentModel;
    using System.IO;
    using System.Net;

    public  class Checks
    {
        public void CheckPhp()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostPhpPath) == false)
            {
                 var url = new WebClient();

                 url.DownloadFileCompleted += new AsyncCompletedEventHandler(this.UrlDownloadFileCompleted);
                 url.DownloadFile("http://windows.php.net/downloads/releases/php-5.5.9-nts-Win32-VC11-x86.zip", "php-5.5.9-nts-Win32-VC11-x86.zip");
                 
            }
            else { Console.WriteLine("Php exists.");}
        }

        private  void UrlDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null) return;

            this.Unzip("php-5.5.9-nts-Win32-VC11-x86.zip");

        }

        private void Unzip(string file)
        {
            
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
