using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEngine
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

    using WebEngine.ASPX;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion

    class ASPXHandler
    {
        private String fileName; 
        private String fullFilePath;
        private String fileExtension;

        private String responseBody;
        private ResponseHeader responseHeader;

        public ASPXHandler(String fileName, Dictionary <String, String> envVariables)
        {
            this.fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            this.fullFilePath = fileName;
            this.fileExtension = this.fileName.Substring(this.fileName.IndexOf('.'));

            if (File.Exists(fileName))
            {
                // Create an instance of Host class
                var aspxHost = new Host();
                // Pass to it filename and query string
                this.responseBody = aspxHost.CreateHost(fileName, envVariables["server_root"], envVariables["query_string"]);
                this.responseHeader = new ResponseHeader("Status: 200 OK\r\nContent-Type: text/html\r\n");
                this.responseHeader.setContentLength(this.responseBody.Length);
            }
            else
            {
                Error404 error = new Error404();
                this.responseHeader = error.getResponseHeader();
                this.responseBody = error.getResponseBody();
            }
        }


    }
}
