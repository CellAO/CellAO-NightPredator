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

namespace WebEngine.Handlers
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using WebEngine.ErrorHandlers;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion

    internal class PHPHandler
    {
        private string fullFilePath;

        private string scriptName;

        private string phpOutput;

        private string phpErrorOutput;

        private string responseBody;

        private readonly ResponseHeader responseHeaders;

        public PHPHandler(String fileName, Dictionary<String, String> envVariables)
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
                if (proc.StartInfo.EnvironmentVariables.ContainsKey("PHPRC"))
                {
                    proc.StartInfo.EnvironmentVariables["PHPRC"] = _config.Instance.CurrentConfig.WebHostPhpPath;
                }
                else
                {
                    proc.StartInfo.EnvironmentVariables.Add("PHPRC", _config.Instance.CurrentConfig.WebHostPhpPath);
                }
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

                this.responseHeaders =
                    new ResponseHeader(
                        this.phpOutput.Substring(0, this.phpOutput.IndexOf("\r\n\r\n")),
                        this.fullFilePath);
                this.setResponseBody(this.phpOutput.Substring(this.phpOutput.IndexOf("\r\n\r\n")));
                this.responseHeaders.setContentLength(this.getContentLength());
            }
            else
            {
                var error = new Error404();
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