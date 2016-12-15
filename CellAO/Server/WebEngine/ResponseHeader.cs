#region License

// Copyright (c) 2005-2016, CellAO Team
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

    #endregion

    internal class ResponseHeader
    {
        private string responseHeaders;

        private int responseCode;

        private string responseStatus;

        private int contentLength;

        public ResponseHeader(String responseHeaders)
        {
            this.setResponseStatus(this.getKeyValue("Status: ", responseHeaders));
            this.setResponseCode("Status: " + this.getKeyValue("Status: ", responseHeaders));
            this.setResponseHeaders(responseHeaders);
        }

        public ResponseHeader(String responseHeaders, String scriptFilePath)
        {
            if (this.getKeyValue("Status: ", responseHeaders) == "")
            {
                if (File.Exists(scriptFilePath))
                {
                    this.setResponseStatus("200 OK");
                    this.setResponseCode("Status: 200 OK");
                }
                else
                {
                    this.setResponseCode("Status: 404");
                    this.setResponseStatus("404");
                }
            }
            else
            {
                this.setResponseStatus(this.getKeyValue("Status: ", responseHeaders));
                this.setResponseCode("Status: " + this.getKeyValue("Status: ", responseHeaders));
            }

            this.setResponseHeaders(responseHeaders);
        }

        public void setResponseHeaders(String responseHeaders)
        {
            this.responseHeaders = "HTTP/1.1 " + this.getResponseStatus() + "\r\n";
            this.responseHeaders += "Sever: CellAO WebEngine\r\n";
            this.responseHeaders += "Accept-Ranges: bytes\r\n";
            this.responseHeaders += responseHeaders;
        }

        public String getResponseHeaders()
        {
            string tmpHeaders = this.responseHeaders;
            if (this.getResponseCode() == 200 && this.contentLength > -1)
            {
                if (!tmpHeaders.EndsWith("\r\n"))
                {
                    tmpHeaders += "\r\n";
                }
                tmpHeaders += "Content-Length: " + this.contentLength;
            }
            tmpHeaders += "\r\n\r\n";
            return tmpHeaders;
        }

        public void setContentLength(int contentLength)
        {
            this.contentLength = contentLength;
        }

        public string getResponseStatus()
        {
            return this.responseStatus;
        }

        public int getResponseCode()
        {
            return this.responseCode;
        }

        public void setResponseCode(String responseHeaders)
        {
            string statusHeaderKey = "Status: ";
            try
            {
                this.setResponseCode(
                    Int32.Parse(
                        responseHeaders.Substring(responseHeaders.IndexOf(statusHeaderKey) + statusHeaderKey.Length, 3)));
            }
            catch (Exception)
            {
                this.setResponseCode(400);
            }
        }

        public void setResponseStatus(String responseStatus)
        {
            this.responseStatus = responseStatus;
        }

        public void setResponseCode(int responseCode)
        {
            this.responseCode = responseCode;
        }

        public string getHeaderValue(String key)
        {
            if (!key.EndsWith(": "))
            {
                if (key.EndsWith(":"))
                {
                    key = key + " ";
                }
                else
                {
                    key = key + ": ";
                }
            }
            return this.getKeyValue(key, this.responseHeaders);
        }

        public string getKeyValue(String needle, String haystack)
        {
            String value = "";
            if (haystack.Contains(needle))
            {
                value =
                    haystack.Substring(
                        haystack.IndexOf(needle) + needle.Length,
                        haystack.IndexOf("\r\n", haystack.IndexOf(needle)) - (haystack.IndexOf(needle) + needle.Length))
                        .Trim();
            }
            return value;
        }
    }
}