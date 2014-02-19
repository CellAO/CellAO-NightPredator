using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Text;
using _config = Utility.Config.ConfigReadWrite;
//using ASPX;

namespace WebEngine
{
    using System.Globalization;
    using System.Xml.Linq;

    using WebEngine.ASPX;

    public class HttpServer
    {
        private readonly TcpListener myListener;

        readonly string serverRoot;

        readonly string errorMessage;

        readonly string badRequest;

        readonly object randObj = new object();

        string serverName;

        readonly XDocument xdoc;

        public HttpServer()
        {
            try
            {
                xdoc = XDocument.Load("MimeTypes.xml");
                //two messages about errors
                errorMessage = "<html><body><h2>Requested file not found</h2></body></html>";
                badRequest = "<html><body><h2>Bad Request</h2></body></html>";
                //define the port
                var port = Convert.ToInt32(_config.Instance.CurrentConfig.WebHostPort);

                serverName = _config.Instance.CurrentConfig.WebHostName;
                //define the directory of the web pages
                serverRoot = _config.Instance.CurrentConfig.WebHostRoot;

                myListener = new TcpListener(IPAddress.Any, port);
                myListener.Start();
                var thread = new Thread(new ThreadStart(StartListen));
                thread.Start();
                Console.WriteLine("WebEngine runs...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        //Get MIME of the file
        private string GetMimeType(string extention)
        {
            var xElement1 = this.xdoc.Element("configuration");
            if (xElement1 != null)
            {
                var element1 = xElement1.Element("Mime");
                if (element1 != null)
                {
                    foreach (XElement xel in element1.Elements("Values"))
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

        //Get default web pages
        private string GetDefaultPage(string serverFolder)
        {
            if  (File.Exists(serverFolder + "\\" + _config.Instance.CurrentConfig.WebHostDefaultPage))
            {
                return _config.Instance.CurrentConfig.WebHostDefaultPage;
            }
            return "";
        }
        //Send content
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

        //Overloaded method
        private void SendData(string data, ref Socket sockets)
        {
            SendData(Encoding.Default.GetBytes(data), ref sockets);
        }
        //Send the headers

        private void SendHeader(string serverProtocol, string mimeType, int totalBytes, string statusCode, ref Socket sockets)
        {
            var ss = new StringBuilder();

            if (string.IsNullOrEmpty(mimeType))
            mimeType = "text/html";
            ss.Append(serverProtocol);
            ss.Append(statusCode).AppendLine();
            ss.AppendLine("Sever: EugeneServer");
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

        private string GetCgiData(string cgiFile, string queryString, string ext, string remoteAddress, string serverProtocol, string referer, string requestedMethod, string userAgent, string request)
        {
            var proc = new Process();

            //indicate the executable to get stdout
            if (ext == ".php")
            {
                proc.StartInfo.FileName = _config.Instance.CurrentConfig.WebHostPhpPath + "\\\\php-cgi.exe";
                //if path to the php is not defined
                if (!File.Exists(proc.StartInfo.FileName))
                {
                    return errorMessage;
                }
                proc.StartInfo.Arguments = " -q " + cgiFile + " " + queryString;
            }
            else
            {
                proc.StartInfo.FileName = cgiFile;
                proc.StartInfo.Arguments = queryString;
            }
            string scriptName = cgiFile.Substring(cgiFile.LastIndexOf('\\') + 1);
            //Set some global variables and output parameters
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
            var str = "";

            proc.Start();
            str = proc.StandardOutput.ReadToEnd();
            proc.Close();
            proc.Dispose();

            return str;
        }

        //Listen incoming connections
        private void StartListen()
        {
            while (true)
            {
                Socket sockets = this.myListener.AcceptSocket();
                var listening = new Thread(() => HttpThread(sockets));

                listening.Start();
            }
        }
        //Process the requests
        private void HttpThread(Socket sockets)
        {
            string request = null;
            string requestedFile = "";
            string mimeType = "";
            string filePath = "";
            string queryString = "";
            string REQUESTED_METHOD = "";
            string referer = "";
            string userAgent = "";
            string serverProtocol = "HTTP/1.1";
            int erMesLen = errorMessage.Length;
            int badMesLen = badRequest.Length;
            StreamWriter logStream = null;
            string remoteAddress = "";

            if (sockets.Connected == true)
            {
                remoteAddress = sockets.RemoteEndPoint.ToString();
                Console.WriteLine("Connected to {0}", remoteAddress);
                //get request from the client and decode it
                var received = new byte[1025];
                int i = sockets.Receive(received, received.Length, 0);
                string sBuffer = Encoding.ASCII.GetString(received);
                if (string.IsNullOrEmpty(sBuffer))
                {
                    sockets.Close();
                    return;
                }

                //Sure that is HTTP -request and get its version
                int startPos = sBuffer.IndexOf("HTTP", 1, System.StringComparison.Ordinal);
                if (startPos == -1)
                {
                    SendHeader(serverProtocol, "", badMesLen, "400 Bad Request", ref sockets);
                    SendData(badRequest, ref sockets);
                    sockets.Close();
                    return;
                }
                else
                {
                    serverProtocol = sBuffer.Substring(startPos, 8);
                }

                //Get other request parameters
                //string[] @params = sBuffer.Split(new char[] { Constants.vbNewLine });
                var @params = sBuffer.Split(
                    new char[] { Convert.ToChar(Environment.NewLine) });

                foreach (var param in @params)
                {
                    //Get User-Agent
                    if (param.Trim().StartsWith("User-Agent"))
                    {
                        userAgent = param.Substring(12);
                        //Get Refferer
                    }
                    else if (param.Trim().StartsWith("Referer"))
                    {
                        referer = param.Trim().Substring(9);
                    }
                }

                //Get request method
                REQUESTED_METHOD = sBuffer.Substring(0, sBuffer.IndexOf(" "));
                var lastPos = sBuffer.IndexOf('/') + 1;
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
                        SendHeader(serverProtocol, "", badMesLen, "400 Bad Request", ref sockets);
                        SendData(badRequest, ref sockets);
                        sockets.Close();
                        return;

                }

                //Get the full name of the requested file
                if (requestedFile.Length == 0)
                {
                    requestedFile = this.GetDefaultPage(serverRoot);
                    if (string.IsNullOrEmpty(requestedFile))
                    {
                        SendHeader(serverProtocol, "", erMesLen, "404 Not Found", ref sockets);
                        SendData(errorMessage, ref sockets);
                    }
                }

                filePath = serverRoot + "\\" + requestedFile;
                Console.WriteLine("Requested file : {0}", filePath);

                //If the file among forbidden files send the error message
                var xElement = this.xdoc.Element("configuration");
                if (xElement != null)
                {
                    var element = xElement.Element("Forbidden");
                    if (element != null)
                    {
                        foreach (XElement forbidden in element.Elements("Path"))
                        {
                            if (filePath.StartsWith(forbidden.Value))
                            {
                                this.SendHeader(serverProtocol, "", erMesLen, "404 Not Found", ref sockets);
                                this.SendData(this.errorMessage, ref sockets);
                                sockets.Close();
                                return;
                            }
                        }
                    }
                }

                //If there is no such file send error message
                if (File.Exists(filePath) == false)
                {
                    SendHeader(serverProtocol, "", erMesLen, "404 Not Found", ref sockets);
                    SendData(errorMessage, ref sockets);
                }
                else
                {
                    string ext = new FileInfo(filePath).Extension.ToLower();
                    mimeType = GetMimeType(ext);

                    //process web pages
                    if (ext == ".aspx")
                    {
                        //Create an instance of Host class
                        var aspxHost = new Host();
                        //Pass to it filename and query string
                        var htmlOut = aspxHost.CreateHost(requestedFile, serverRoot, queryString);
                        erMesLen = htmlOut.Length;
                        SendHeader(serverProtocol, mimeType, erMesLen, " 200 OK", ref sockets);
                        SendData(htmlOut, ref sockets);
                    }
                    else if (ext == ".php" || ext == ".exe")
                    {
                        string cgi2html = GetCgiData(filePath, queryString, ext, remoteAddress, serverProtocol, referer, REQUESTED_METHOD, userAgent, request);
                        if (cgi2html == errorMessage)
                        {
                            SendHeader(serverProtocol, "", erMesLen, "404 Not Found", ref sockets);
                            SendData(errorMessage, ref sockets);
                        }
                        else
                        {
                            erMesLen = cgi2html.Length;
                            SendHeader(serverProtocol, mimeType, erMesLen, " 200 OK", ref sockets);
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
                        SendHeader(serverProtocol, mimeType, erMesLen, "200 OK", ref sockets);
                        SendData(bytes, ref sockets);
                    }
                }
                sockets.Close();

                Monitor.Enter(randObj);
                logStream = new StreamWriter("Server.log", true);
                //Output to the server log
                logStream.WriteLine(System.DateTime.Now.ToString());
                logStream.WriteLine("Connected to {0}", remoteAddress);
                logStream.WriteLine("Requested path {0}", request);
                logStream.WriteLine("Total bytes {0}", erMesLen);
                logStream.Flush();
                logStream.Close();
                Monitor.Exit(randObj);
            }
        }
    }
}
