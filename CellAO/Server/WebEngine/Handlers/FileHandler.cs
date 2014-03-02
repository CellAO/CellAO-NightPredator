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

namespace WebEngine.Handlers
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using WebEngine.ErrorHandlers;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion

    internal class FileHandler
    {
        private String fileName;

        private String fullFilePath;

        private String fileExtension;

        private byte[] responseBody;

        private int responseBodyLength;

        private ResponseHeader responseHeader;

        private Dictionary<String, String> contentTypes = new Dictionary<String, String>();

        public FileHandler(String fileName)
        {
            this.contentTypes.Add(".txt", "text/plain");
            this.contentTypes.Add(".html", "text/html");
            this.contentTypes.Add(".htm", "text/html");
            this.contentTypes.Add(".json", "application/json");
            this.contentTypes.Add(".xml", "application/xml");
            this.contentTypes.Add(".png", "image/png");
            this.contentTypes.Add(".gif", "image/gif");
            this.contentTypes.Add(".jpeg", "image/jpg");
            this.contentTypes.Add(".jpg", "image/jpg");
            this.contentTypes.Add(".css", "text/css");
            this.contentTypes.Add(".js", "application/javascript");

            this.fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            this.fullFilePath = fileName;
            this.fileExtension = this.fileName.Substring(this.fileName.LastIndexOf('.'));

            if (File.Exists(fileName))
            {
                var fs = new FileStream(this.fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                this.responseBody = new byte[fs.Length + 1];
                this.responseBodyLength = this.responseBody.Length;
                fs.Read(this.responseBody, 0, this.responseBodyLength);
                fs.Close();

                string tmpHeader = "Status: 200 OK\r\n";
                if (this.contentTypes.ContainsKey(this.fileExtension))
                {
                    tmpHeader += "Content-Type: " + this.contentTypes[this.fileExtension] + "\r\n";
                }

                this.responseHeader = new ResponseHeader(tmpHeader);
                this.responseHeader.setContentLength(this.responseBodyLength);
            }
            else
            {
                var error = new Error404();
                this.responseHeader = error.getResponseHeader();
                this.responseBody = Encoding.ASCII.GetBytes(error.getResponseBody());
            }
        }

        public byte[] getResponseBody()
        {
            return this.responseBody;
        }

        public string getResponseHeader()
        {
            return this.responseHeader.getResponseHeaders();
        }
    }
}