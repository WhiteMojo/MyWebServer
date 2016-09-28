using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    public class HttpRequest
    {
        public string Filepath { get; set;}
        public HttpRequest(string msg)
        {
            string[] msgs = msg.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);//split the request datagram by \r \n
            Filepath = msgs[0].Split(' ')[1];//obtain requested file name from request datagram
        }
    }
}
