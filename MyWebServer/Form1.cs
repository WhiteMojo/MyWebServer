using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWebServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// Start service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Socket listenSocket = null;
        private void button1_Click(object sender, EventArgs e)
        {
            listenSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);//establish a socket in charge of listening 
            IPAddress ipAddress = IPAddress.Parse(this.txtIpAddress.Text);//IP Address
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(this.txtPort.Text));//Establish a IPENdpoint
            listenSocket.Bind(ipEndPoint);//listened socket was binded to a communication endpoint
            listenSocket.Listen(10);//listen the port for data, 10 means listen socket can handle 10 requests same time 
            Thread myThread = new Thread(BeginAccept);
            myThread.IsBackground = true;
            myThread.Start();
        }
        private void BeginAccept()
        {
            while (true)//dead infinite loop,responsibile for waiting socket to continue waiting for clients data requests
            {
                Socket newSocket = listenSocket.Accept();
                HttpApplication httpApplication = new HttpApplication(newSocket,ShowMsg);  
            }
        }
        private void ShowMsg(string msg)
        {
            this.txtContent.AppendText(msg+"\r\n");
        }
    }
}
