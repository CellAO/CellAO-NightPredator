using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEngine.ErrorHandlers;

namespace WebEngine.Handlers
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Xml.Linq;

    using Utility;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion


    class FileHandler
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
            contentTypes.Add(".txt", "text/plain");
            contentTypes.Add(".html", "text/html");
            contentTypes.Add(".htm", "text/html");
            contentTypes.Add(".json", "application/json");
            contentTypes.Add(".xml", "application/xml");
            contentTypes.Add(".png", "image/png");
            contentTypes.Add(".gif", "image/gif");
            contentTypes.Add(".jpeg", "image/jpg");
            contentTypes.Add(".jpg", "image/jpg");
            contentTypes.Add(".css", "text/css");
            contentTypes.Add(".js", "application/javascript");

            this.fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            this.fullFilePath = fileName;
            this.fileExtension = this.fileName.Substring(this.fileName.LastIndexOf('.'));

            if (File.Exists(fileName))
            {
                var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                this.responseBody = new byte[fs.Length + 1];
                this.responseBodyLength = this.responseBody.Length;
                fs.Read(this.responseBody, 0, this.responseBodyLength);
                fs.Close();

                var tmpHeader = "Status: 200 OK\r\n";
                if (contentTypes.ContainsKey(this.fileExtension))
                {
                    tmpHeader += "Content-Type: " + contentTypes[this.fileExtension] + "\r\n";
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
