using System;
using System.Web;
using System.Web.Hosting;
using System.IO;

namespace WebEngine.ASPX
{
    public class Host : MarshalByRefObject
    {
        private string ProcessFile(string filename, string queryString)
        {
            var sw = new StringWriter();
            var simpleWorker = new SimpleWorkerRequest(filename, queryString, sw);
            HttpRuntime.ProcessRequest(simpleWorker);
            return sw.ToString();
        }
        public string CreateHost(string filename, string serverRoot, string queryString)
        {
            var myHost = (Host)ApplicationHost.CreateApplicationHost(typeof(Host), "/", serverRoot);
            return myHost.ProcessFile(filename, queryString);
        }
    }
}
