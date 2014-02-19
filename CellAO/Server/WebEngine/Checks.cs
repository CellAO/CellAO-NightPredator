#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
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

#endregion

#region Usings ...

using _config = Utility.Config.ConfigReadWrite;

#endregion

namespace WebEngine
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    #endregion

    /// <summary>
    /// </summary>
    public static class Checks
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public static void CheckPhp()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostPhpPath) == false)
            {
                var request =
                    (HttpWebRequest)
                        WebRequest.Create("http://windows.php.net/downloads/releases/php-5.5.9-nts-Win32-VC11-x86.zip");
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
            else
            {
                Console.WriteLine("Php exists.");
            }
        }

        /// <summary>
        /// </summary>
        public static void CheckWebCore()
        {
            if (Directory.Exists(_config.Instance.CurrentConfig.WebHostRoot) == false)
            {
            }
            else
            {
                Console.WriteLine("Webcore Exists.");
            }
        }

        #endregion
    }
}