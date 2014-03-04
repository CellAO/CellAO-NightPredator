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
    using System.Collections.Generic;
    using WebEngine.ErrorHandlers;
    using WebEngine.Handlers;
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

        private void SendError400(ref Socket sockets)
        {
            try
            {
                var error = new Error400();
                SendData(error.getResponseHeader().getResponseHeaders(), ref sockets);
                sockets.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void SendError404(ref Socket sockets)
        {
            try
            {
                var error = new Error404();
                SendData(error.getResponseHeader().getResponseHeaders(), ref sockets);
                sockets.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="extention">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetMimeType(string extention)
        {
            var xElement1 = this.xdoc.Element("configuration");
            if (xElement1 != null)
            {
                var element1 = xElement1.Element("Mime");
                if (element1 != null)
                {
                    foreach (var xel in element1.Elements("Values"))
                    {
                        var xElement = xel.Element("Ext");
                        if (xElement != null && xElement.Value == extention)
                        {
                            var element = xel.Element("Type");
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
            StreamWriter logStream = null;
            string remoteAddress = string.Empty;
            string cookie = string.Empty;

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
                int startPos = sBuffer.IndexOf("HTTP", 1);
                if (startPos == -1)
                {
                    this.SendError400(ref sockets);
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
                    else if (param.Trim().StartsWith("Cookie: "))
                    {
                        cookie = param.Trim().Substring(8);
                    }
                }
                //string postData = @params[@params.Length - 1].Replace("\0", "");
                // Get request method
                REQUESTED_METHOD = sBuffer.Substring(0, sBuffer.IndexOf(" "));
                int lastPos = sBuffer.IndexOf('/') + 1;
                request = sBuffer.Substring(lastPos, startPos - lastPos - 1);

                switch (REQUESTED_METHOD)
                {
                    case "POST":
                        requestedFile = request.Replace("/", "\\").Trim();
                        queryString = @params[@params.Length - 1].Trim().Replace("\0", "");
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
                        this.SendError400(ref sockets);
                        return;
                }

                // Get the full name of the requested file
                if (requestedFile.EndsWith("\\") || String.IsNullOrEmpty(requestedFile))
                {
                    foreach(String fileName in new String[] {"index.php", "index.aspx", "index.html", "index.htm", "index.txt"}){
                       String tmpFileName = _config.Instance.CurrentConfig.WebHostRoot + "\\" + requestedFile + fileName;
                       if(File.Exists(tmpFileName)){
                           requestedFile = requestedFile + fileName;
                           break;
                       }
                    }
                    if (string.IsNullOrEmpty(requestedFile))
                    {
                        this.SendError404(ref sockets);
                        return;
                    }
                }

                filePath = this.serverRoot + "\\" + requestedFile;
                Console.WriteLine("Requested file : {0}", filePath);

                // If there is no such file send error message
                if (File.Exists(filePath) == false)
                {
                    this.SendError404(ref sockets);
                    return;
                }
                else
                {
                    string ext = new FileInfo(filePath).Extension.ToLower();
                    mimeType = this.GetMimeType(ext);

                    // process web pages
                    if (ext == ".aspx")
                    {
                        var requestOptions = new Dictionary<String, String>
                                             {
                                                 { "server_root", this.serverRoot },
                                                 { "query_string", queryString }
                                             };
                        var aspxHandler = new ASPXHandler(requestedFile, requestOptions);

                    }
                    else if (ext == ".php" || ext == ".exe")
                    {
                        var requestOptions = new Dictionary<string, string>();
                        requestOptions.Add("remote_addr", remoteAddress.ToString(CultureInfo.InvariantCulture));
                        requestOptions.Add("user_agent", userAgent);
                        requestOptions.Add("request_method", REQUESTED_METHOD);
                        requestOptions.Add("referer", referer);
                        requestOptions.Add("server_protocol", serverProtocol);
                        requestOptions.Add("query_string", queryString);
                        requestOptions.Add("cookie", cookie);
                        requestOptions.Add("post", queryString);
                        var phpHandler = new PHPHandler(filePath, requestOptions);
                        SendData(phpHandler.getResponseHeaders(), ref sockets);
                        SendData(phpHandler.getResponseBody(), ref sockets);
                    }
                    else
                    {
                        var fileHandler = new FileHandler(filePath);
                        SendData(fileHandler.getResponseHeader(), ref sockets);
                        SendData(fileHandler.getResponseBody(), ref sockets);
                    }
                }

                sockets.Close();

                Monitor.Enter(this.randObj);
                logStream = new StreamWriter("WebEngine-Server.log", true);

                // Output to the server log
                logStream.WriteLine(DateTime.Now.ToString());
                logStream.WriteLine("Connected to {0}", remoteAddress);
                logStream.WriteLine("Requested path {0}", request);
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
            SendData(Encoding.GetEncoding("windows-1252").GetBytes(data), ref sockets);
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
    }
}