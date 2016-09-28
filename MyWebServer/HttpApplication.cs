using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    public class HttpApplication
    {
        Socket newSocket = null;
        DGShowMsg dgShowMsg = null;
        /// <summary>
        /// Finish client side sent data processing
        /// </summary>
        public HttpApplication(Socket newSocket, DGShowMsg dgShowMsg)
        {
            this.newSocket = newSocket;
            this.dgShowMsg = dgShowMsg;
            //receive client side sent data
            byte[] buffer = new byte[1024*1024*2];
            int receiveLength;
            receiveLength = newSocket.Receive(buffer);//return the actual length of the received data
            if (receiveLength >0) { 
                string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveLength);
                HttpRequest request = new HttpRequest(msg);
                ProcessRequest(request);
                dgShowMsg(msg);
            }

        }

        public void ProcessRequest(HttpRequest request)
        {
            string filePath = request.Filepath;
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;//obtain current server running directory
            if (dataDir.EndsWith(@"\bin\Debug\") || dataDir.EndsWith(@"\bin\Release\"))
            {
                dataDir = System.IO.Directory.GetParent(dataDir).Parent.Parent.FullName;
            }
            string fullDir = dataDir + filePath;//obtain file complete path
            using (FileStream fileStream = new FileStream(fullDir,FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                //create response datagram
                HttpResponse response = new HttpResponse(buffer,filePath);
                newSocket.Send(response.GetHeaderResponse());
                newSocket.Send(buffer);
            }

        }
    }
}
