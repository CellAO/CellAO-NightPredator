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

namespace WebEngine
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Xml.Linq;

    using Utility;

    using WebEngine.ASPX;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion

    /// <summary>
    /// </summary>
    public class HttpServer
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static HttpServer instance = null;

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        public bool isRunning = false;

        /// <summary>
        /// </summary>
        private readonly string badRequest;

        /// <summary>
        /// </summary>
        private readonly string errorMessage;

        /// <summary>
        /// </summary>
        private readonly TcpListener myListener;

        /// <summary>
        /// </summary>
        private readonly object randObj = new object();

        /// <summary>
        /// </summary>
        private readonly string serverRoot;

        /// <summary>
        /// </summary>
        private readonly XDocument xdoc;

        /// <summary>
        /// </summary>
        private string serverName;

        /// <summary>
        /// </summary>
        private bool stopServer = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public HttpServer()
        {
            try
            {
                this.xdoc = XDocument.Load("MimeTypes.xml");

                // two messages about errors
                this.errorMessage = "<html><body><h2>Requested file not found</h2></body></html>";
                this.badRequest = "<html><body><h2>Bad Request</h2></body></html>";

                // define the port
                int port = Convert.ToInt32(_config.Instance.CurrentConfig.WebHostPort);

                this.serverName = _config.Instance.CurrentConfig.WebHostName;

                // define the directory of the web pages
                this.serverRoot = _config.Instance.CurrentConfig.WebHostRoot;

                this.myListener = new TcpListener(IPAddress.Any, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void StartServer()
        {
            this.stopServer = false;
            Thread mainLoop = new Thread(this.StartListen);
            mainLoop.Start();
        }

        /// <summary>
        /// </summary>
        public void StopServer()
        {
            this.stopServer = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="cgiFile">
        /// </param>
        /// <param name="queryString">
        /// </param>
        /// <param name="ext">
        /// </param>
        /// <param name="remoteAddress">
        /// </param>
        /// <param name="serverProtocol">
        /// </param>
        /// <param name="referer">
        /// </param>
        /// <param name="requestedMethod">
        /// </param>
        /// <param name="userAgent">
        /// </param>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetCgiData(
            string cgiFile, 
            string queryString, 
            string ext, 
            string remoteAddress, 
            string serverProtocol, 
            string referer, 
            string requestedMethod, 
            string userAgent, 
            string request)
        {
            var proc = new Process();

            // indicate the executable to get stdout
            if (ext == ".php")
            {
                proc.StartInfo.FileName = _config.Instance.CurrentConfig.WebHostPhpPath + "\\\\php-cgi.exe";

                // if path to the php is not defined
                if (!File.Exists(proc.StartInfo.FileName))
                {
                    return this.errorMessage;
                }

                proc.StartInfo.Arguments = " -q " + cgiFile + " " + queryString;
            }
            else
            {
                proc.StartInfo.FileName = cgiFile;
                proc.StartInfo.Arguments = queryString;
            }

            string scriptName = cgiFile.Substring(cgiFile.LastIndexOf('\\') + 1);

            // Set some global variables and output parameters
            proc.StartInfo.EnvironmentVariables.Add("REMOTE_ADDR", remoteAddress.ToString(CultureInfo.InvariantCulture));
            proc.StartInfo.EnvironmentVariables.Add("SCRIPT_NAME", scriptName);
            proc.StartInfo.EnvironmentVariables.Add("USER_AGENT", userAgent);
            proc.StartInfo.EnvironmentVariables.Add("REQUESTED_METHOD", requestedMethod);
            proc.StartInfo.EnvironmentVariables.Add("REFERER", referer);
            proc.StartInfo.EnvironmentVariables.Add("SERVER_PROTOCOL", serverProtocol);
            proc.StartInfo.EnvironmentVariables.Add("QUERY_STRING", request);

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.CreateNoWindow = true;
            string str = string.Empty;

            proc.Start();
            str = proc.StandardOutput.ReadToEnd();
            proc.Close();
            proc.Dispose();

            return str;
        }

        // Get MIME of the file

        // Get default web pages
        /// <summary>
        /// </summary>
        /// <param name="serverFolder">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetDefaultPage(string serverFolder)
        {
            if (File.Exists(serverFolder + "\\" + _config.Instance.CurrentConfig.WebHostDefaultPage))
            {
                return _config.Instance.CurrentConfig.WebHostDefaultPage;
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="extention">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetMimeType(string extention)
        {
            XElement xElement1 = this.xdoc.Element("configuration");
            if (xElement1 != null)
            {
                XElement element1 = xElement1.Element("Mime");
                if (element1 != null)
                {
                    foreach (XElement xel in element1.Elements("Values"))
                    {
                        XElement xElement = xel.Element("Ext");
                        if (xElement != null && xElement.Value == extention)
                        {
                            XElement element = xel.Element("Type");
                            if (element != null)
                            {
                                return element.Value;
                            }
                        }
                    }
                }
            }

            return "text/html";
        }

        /// <summary>
        /// </summary>
        /// <param name="sockets">
        /// </param>
        private void HttpThread(Socket sockets)
        {
            string request = null;
            string requestedFile = string.Empty;
            string mimeType = string.Empty;
            string filePath = string.Empty;
            string queryString = string.Empty;
            string REQUESTED_METHOD = string.Empty;
            string referer = string.Empty;
            string userAgent = string.Empty;
            string serverProtocol = "HTTP/1.1";
            int erMesLen = this.errorMessage.Length;
            int badMesLen = this.badRequest.Length;
            StreamWriter logStream = null;
            string remoteAddress = string.Empty;

            if (sockets.Connected == true)
            {
                remoteAddress = sockets.RemoteEndPoint.ToString();
                Console.WriteLine("Connected to {0}", remoteAddress);

                // get request from the client and decode it
                var received = new byte[1025];
                int i = sockets.Receive(received, received.Length, 0);
                string sBuffer = Encoding.ASCII.GetString(received);
                if (string.IsNullOrEmpty(sBuffer))
                {
                    sockets.Close();
                    return;
                }

                // Sure that is HTTP -request and get its version
                int startPos = sBuffer.IndexOf("HTTP", 1, StringComparison.Ordinal);
                if (startPos == -1)
                {
                    this.SendHeader(serverProtocol, string.Empty, badMesLen, "400 Bad Request", ref sockets);
                    SendData(this.badRequest, ref sockets);
                    sockets.Close();
                    return;
                }
                else
                {
                    serverProtocol = sBuffer.Substring(startPos, 8);
                }

                // Get other request parameters
                // string[] @params = sBuffer.Split(new char[] { Constants.vbNewLine });
                string[] @params = sBuffer.Replace("\r\n", "\n").Split('\n');

                foreach (string param in @params)
                {
                    // Get User-Agent
                    if (param.Trim().StartsWith("User-Agent"))
                    {
                        userAgent = param.Substring(12);

                        // Get Refferer
                    }
                    else if (param.Trim().StartsWith("Referer"))
                    {
                        referer = param.Trim().Substring(9);
                    }
                }

                // Get request method
                REQUESTED_METHOD = sBuffer.Substring(0, sBuffer.IndexOf(" ", StringComparison.Ordinal));
                int lastPos = sBuffer.IndexOf('/') + 1;
                request = sBuffer.Substring(lastPos, startPos - lastPos - 1);

                switch (REQUESTED_METHOD)
                {
                    case "POST":
                        requestedFile = request.Replace("/", "\\").Trim();
                        queryString = @params[@params.Length - 1].Trim();
                        break;
                    case "GET":
                        lastPos = request.IndexOf('?');
                        if (lastPos > 0)
                        {
                            requestedFile = request.Substring(0, lastPos).Replace("/", "\\");
                            queryString = request.Substring(lastPos + 1);
                        }
                        else
                        {
                            requestedFile = request.Substring(0).Replace("/", "\\");
                        }

                        break;
                    case "HEAD":
                        break;
                    default:
                        this.SendHeader(serverProtocol, string.Empty, badMesLen, "400 Bad Request", ref sockets);
                        SendData(this.badRequest, ref sockets);
                        sockets.Close();
                        return;
                }

                // Get the full name of the requested file
                if (requestedFile.Length == 0)
                {
                    requestedFile = this.GetDefaultPage(this.serverRoot);
                    if (string.IsNullOrEmpty(requestedFile))
                    {
                        this.SendHeader(serverProtocol, string.Empty, erMesLen, "404 Not Found", ref sockets);
                        SendData(this.errorMessage, ref sockets);
                    }
                }

                filePath = this.serverRoot + "\\" + requestedFile;
                Console.WriteLine("Requested file : {0}", filePath);

                // If the file among forbidden files send the error message
                XElement xElement = this.xdoc.Element("configuration");
                if (xElement != null)
                {
                    XElement element = xElement.Element("Forbidden");
                    if (element != null)
                    {
                        foreach (XElement forbidden in element.Elements("Path"))
                        {
                            if (filePath.StartsWith(forbidden.Value))
                            {
                                this.SendHeader(serverProtocol, string.Empty, erMesLen, "404 Not Found", ref sockets);
                                this.SendData(this.errorMessage, ref sockets);
                                sockets.Close();
                                return;
                            }
                        }
                    }
                }

                // If there is no such file send error message
                if (File.Exists(filePath) == false)
                {
                    this.SendHeader(serverProtocol, string.Empty, erMesLen, "404 Not Found", ref sockets);
                    SendData(this.errorMessage, ref sockets);
                }
                else
                {
                    string ext = new FileInfo(filePath).Extension.ToLower();
                    mimeType = this.GetMimeType(ext);

                    // process web pages
                    if (ext == ".aspx")
                    {
                        // Create an instance of Host class
                        var aspxHost = new Host();

                        // Pass to it filename and query string
                        string htmlOut = aspxHost.CreateHost(requestedFile, this.serverRoot, queryString);
                        erMesLen = htmlOut.Length;
                        this.SendHeader(serverProtocol, mimeType, erMesLen, " 200 OK", ref sockets);
                        SendData(htmlOut, ref sockets);
                    }
                    else if (ext == ".php" || ext == ".exe")
                    {
                        string cgi2html = this.GetCgiData(
                            filePath, 
                            queryString, 
                            ext, 
                            remoteAddress, 
                            serverProtocol, 
                            referer, 
                            REQUESTED_METHOD, 
                            userAgent, 
                            request);
                        if (cgi2html == this.errorMessage)
                        {
                            this.SendHeader(serverProtocol, string.Empty, erMesLen, "404 Not Found", ref sockets);
                            SendData(this.errorMessage, ref sockets);
                        }
                        else
                        {
                            erMesLen = cgi2html.Length;
                            this.SendHeader(serverProtocol, mimeType, erMesLen, " 200 OK", ref sockets);
                            SendData(cgi2html, ref sockets);
                        }
                    }
                    else
                    {
                        var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        var bytes = new byte[fs.Length + 1];
                        erMesLen = bytes.Length;
                        fs.Read(bytes, 0, erMesLen);
                        fs.Close();
                        this.SendHeader(serverProtocol, mimeType, erMesLen, "200 OK", ref sockets);
                        SendData(bytes, ref sockets);
                    }
                }

                sockets.Close();

                Monitor.Enter(this.randObj);
                logStream = new StreamWriter("Server.log", true);

                // Output to the server log
                logStream.WriteLine(DateTime.Now.ToString());
                logStream.WriteLine("Connected to {0}", remoteAddress);
                logStream.WriteLine("Requested path {0}", request);
                logStream.WriteLine("Total bytes {0}", erMesLen);
                logStream.Flush();
                logStream.Close();
                Monitor.Exit(this.randObj);
            }
        }

        // Send content
        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <param name="sockets">
        /// </param>
        private void SendData(byte[] data, ref Socket sockets)
        {
            const int NumberOfBytes = 0;
            try
            {
                if (sockets.Connected == true)
                {
                    if (NumberOfBytes + sockets.Send(data, data.Length, SocketFlags.None) == -1)
                    {
                        Console.WriteLine("Error");
                    }
                }
                else
                {
                    Console.WriteLine("Connection is closed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Overloaded method
        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <param name="sockets">
        /// </param>
        private void SendData(string data, ref Socket sockets)
        {
            SendData(Encoding.Default.GetBytes(data), ref sockets);
        }

        // Send the headers

        /// <summary>
        /// </summary>
        /// <param name="serverProtocol">
        /// </param>
        /// <param name="mimeType">
        /// </param>
        /// <param name="totalBytes">
        /// </param>
        /// <param name="statusCode">
        /// </param>
        /// <param name="sockets">
        /// </param>
        private void SendHeader(
            string serverProtocol, 
            string mimeType, 
            int totalBytes, 
            string statusCode, 
            ref Socket sockets)
        {
            var ss = new StringBuilder();

            if (string.IsNullOrEmpty(mimeType))
            {
                mimeType = "text/html";
            }

            ss.Append(serverProtocol);
            ss.Append(statusCode).AppendLine();
            ss.AppendLine("Sever: CellAO WebEngine");
            ss.Append("Content-Type: ");
            ss.Append(mimeType).AppendLine();
            ss.Append("Accept-Ranges: bytes").AppendLine();
            ss.Append("Content-Length: ");
            ss.Append(totalBytes).AppendLine().AppendLine();

            byte[] dataToSend = Encoding.Default.GetBytes(ss.ToString());
            ss.Clear();
            SendData(dataToSend, ref sockets);
            Console.WriteLine("{0} bytes have been sent", totalBytes.ToString(CultureInfo.InvariantCulture));
        }

        // Listen incoming connections
        /// <summary>
        /// </summary>
        private void StartListen()
        {
            this.myListener.Start();
            this.isRunning = true;
            try
            {
                while (!this.stopServer)
                {
                    Socket sockets = this.myListener.AcceptSocket();
                    var listening = new Thread(() => this.HttpThread(sockets));

                    listening.Start();
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }

            this.isRunning = false;
        }

        #endregion

        // Process the requests
    }
}