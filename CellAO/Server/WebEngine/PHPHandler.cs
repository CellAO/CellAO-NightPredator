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

    using _config = Utility.Config.ConfigReadWrite;

    #endregion
    class PHPHandler
    {
        private string fullFilePath;
        private string scriptName;
        private string phpOutput;
        private string phpErrorOutput; 
        private string responseBody;
        private ResponseHeader responseHeaders;

        public PHPHandler(String fileName, Dictionary <String, String> envVariables)
        {
            this.scriptName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            this.fullFilePath = fileName;
            if (File.Exists(fileName))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = _config.Instance.CurrentConfig.WebHostPhpPath + "\\\\php-cgi.exe";
                proc.StartInfo.EnvironmentVariables.Add("REMOTE_ADDR", envVariables["remote_addr"]);
                proc.StartInfo.EnvironmentVariables.Add("SCRIPT_NAME", this.scriptName);
                proc.StartInfo.EnvironmentVariables.Add("USER_AGENT", envVariables["user_agent"]);
                proc.StartInfo.EnvironmentVariables.Add("REQUEST_METHOD", envVariables["request_method"]);
                proc.StartInfo.EnvironmentVariables.Add("REFERER", envVariables["referer"]);
                proc.StartInfo.EnvironmentVariables.Add("SERVER_PROTOCOL", envVariables["server_protocol"]);
                proc.StartInfo.EnvironmentVariables.Add("QUERY_STRING", envVariables["query_string"]);
                proc.StartInfo.EnvironmentVariables.Add("HTTP_COOKIE", envVariables["cookie"]);
                proc.StartInfo.EnvironmentVariables.Add("SCRIPT_FILENAME", this.fullFilePath);
                proc.StartInfo.EnvironmentVariables.Add("REDIRECT_STATUS", "200");
                proc.StartInfo.EnvironmentVariables.Add("CONTENT_LENGTH", envVariables["post"].Length.ToString());
                proc.StartInfo.EnvironmentVariables.Add("CONTENT_TYPE", "application/x-www-form-urlencoded");
                proc.StartInfo.EnvironmentVariables.Add("HTTP_RAW_POST_DATA", envVariables["post"]);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.StandardInput.WriteLine(envVariables["post"]);
                this.phpOutput = proc.StandardOutput.ReadToEnd();
                this.phpErrorOutput = proc.StandardError.ReadToEnd();
                proc.Close();
                proc.Dispose();

                this.responseHeaders = new ResponseHeader(this.phpOutput.Substring(0, this.phpOutput.IndexOf("\r\n\r\n")));
                this.setResponseBody(phpOutput.Substring(phpOutput.IndexOf("\r\n\r\n")));
                this.responseHeaders.setContentLength(this.getContentLength());
            }
            else
            {
                Error404 error = new Error404();
                this.responseHeaders = error.getResponseHeader();
                this.responseBody = error.getResponseBody();
            }
        }

        public string getResponseHeaders()
        {
            return this.responseHeaders.getResponseHeaders();
        }

        private void setResponseBody(string responseBody)
        {
            this.responseBody = responseBody;
        }

        public string getResponseBody()
        {
            return this.responseBody;
        }

        public int getContentLength()
        {
            return this.getResponseBody().Length;
        }

























    }
}
