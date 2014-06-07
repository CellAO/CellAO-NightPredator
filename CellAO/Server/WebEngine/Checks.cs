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

namespace WebEngine
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Net;

    using Ionic.Zip;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion

    public class Checks
    {
        public void CheckPhp()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostPhpPath) == false)
            {
                Console.WriteLine();
                var url = new WebClient();
                Console.WriteLine();
                Console.WriteLine("Downloading php...");
                url.DownloadFile(
                    "http://windows.php.net/downloads/releases/php-5.5.10-nts-Win32-VC11-x86.zip",
                    "php-5.5.10-nts-Win32-VC11-x86.zip");
                url.Dispose();
                this.UrlDownloadFileCompleted("php-5.5.10-nts-Win32-VC11-x86.zip");
            }
            else
            {
                Console.WriteLine("Php exists.");
            }
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
            using (ZipFile zip = ZipFile.Read(file))
            {
                foreach (ZipEntry ze in zip)
                {
                    ze.Extract(
                        _config.Instance.CurrentConfig.WebHostPhpPath,
                        ExtractExistingFileAction.OverwriteSilently);
                }
                Console.WriteLine("Done.");
                Console.WriteLine();
                Console.WriteLine("Deleting " + Convert.ToString(file) + "...");
                File.Delete(file);
                Console.WriteLine();
                var url = new WebClient();
                url.DownloadFile(
                    "http://www.aocell.info/php.ini",
                    _config.Instance.CurrentConfig.WebHostPhpPath + @"\php.ini");
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
                string[] coreDirectories = Directory.GetDirectories(_config.Instance.CurrentConfig.WebHostRoot);

                foreach (string coreDirectory in coreDirectories)
                {
                    string[] files = Directory.GetFiles(coreDirectory);
                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = Path.GetFileName(s);
                        string destFile = Path.Combine(_config.Instance.CurrentConfig.WebHostRoot, fileName);
                        File.Move(s, destFile);
                    }

                    files = Directory.GetDirectories(coreDirectory);
                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = Path.GetFileName(s);
                        string destFile = Path.Combine(_config.Instance.CurrentConfig.WebHostRoot, fileName);
                        Directory.Move(s, destFile);
                    }
                    Directory.Delete(coreDirectory);
                }
            }
            else
            {
                Console.WriteLine("Webcore Exists.");
            }
        }

        private void Unzip2(string file)
        {
            using (ZipFile zip = ZipFile.Read(file))
            {
                foreach (ZipEntry ze in zip)
                {
                    ze.Extract(_config.Instance.CurrentConfig.WebHostRoot, ExtractExistingFileAction.OverwriteSilently);
                }
                Console.WriteLine("Done.");
            }
        }
    }
}