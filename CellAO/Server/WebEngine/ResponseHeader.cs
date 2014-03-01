using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEngine
{
    class ResponseHeader
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

        public void setResponseHeaders(String responseHeaders)
        {
            this.responseHeaders = "HTTP/1.1 " + this.getResponseStatus() + "\r\n";
            this.responseHeaders += "Sever: CellAO WebEngine\r\n";
            this.responseHeaders += "Accept-Ranges: bytes\r\n";
            this.responseHeaders += responseHeaders;
        }

        public String getResponseHeaders()
        {
            var tmpHeaders = this.responseHeaders;
            if (this.getResponseCode() == 200 && this.contentLength > -1)
            {
                tmpHeaders += "Content-Length: " + this.contentLength;
            }
            tmpHeaders +=  "\r\n\r\n";
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
                this.setResponseCode(Int32.Parse(responseHeaders.Substring(responseHeaders.IndexOf(statusHeaderKey) + statusHeaderKey.Length, 3)));
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
                var one = haystack.IndexOf(needle) + needle.Length;
                var two = haystack.IndexOf("\r\n", haystack.IndexOf(needle));

                value = haystack.Substring(haystack.IndexOf(needle) + needle.Length, haystack.IndexOf("\r\n", haystack.IndexOf(needle)) - needle.Length).Trim();
            }
            return value;
        }
    }
}
