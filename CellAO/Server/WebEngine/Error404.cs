using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEngine
{
    class Error404
    {
        private ResponseHeader responseHeader;
        private string responseBody = ""; 

        public Error404()
        {
            this.responseHeader = new ResponseHeader("Status: 404 Not Found\r\nContent-Type: text/html\r\n");
        }

        public string getResponseBody()
        {
            return this.responseBody;
        }

        public ResponseHeader getResponseHeader()
        {
            return this.responseHeader;
        }
    }
}
