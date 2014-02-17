using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text;

namespace WebEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var myServer = default(HttpServer);
            myServer = new HttpServer();
        }
    }
}
