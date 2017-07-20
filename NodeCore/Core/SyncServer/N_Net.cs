using NodeCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NodeCore.Core.SyncServer
{
    /// <summary>
    /// 通讯
    /// </summary>
    public class N_Net
    {
        public SocketClient client = new SocketClient();
        public void StartNode()
        {

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Parse("192.168.1.75");
            //IPEndPoint iep = new IPEndPoint(ip, 8888);
            //socket.BeginConnect(iep, new AsyncCallback(requestCallBack), socket);
        }
        public bool RegisterNode()
        {
            if (client.clientSocket == null)
            {
                client.StopSocket();
                client = new SocketClient();
                client.OnReceiveMessage += client_OnReceiveMessage;
                client.ConnectSocket();
                return true;
            }
            return true;

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Parse("192.168.1.75");
            //IPEndPoint iep = new IPEndPoint(ip, 8888);
            //socket.BeginConnect(iep, new AsyncCallback(requestCallBack), socket);
        }
        //"Sync:IP_" + node2.IP + "Port_" + node2.Port, node1
        void client_OnReceiveMessage(string message)
        {
            if(message.Contains("Sync"))
            {
                string ip = message.Split('_')[1].Replace("Port", "");
                string port = message.Split('_')[2];
                SystemManager.Instance.SyncNode(ip, port);
            }

        }
        //private void requestCallBack(IAsyncResult iar)
        //{
        //    try
        //    {
        //        //还原原始的TcpClient对象
        //        TcpClient client = (TcpClient)iar.AsyncState;
        //        //
        //        client.EndConnect(iar);
        //        //Console.WriteLine("与服务器{0}连接成功", client.Client.RemoteEndPoint);
        //    }
        //    catch (Exception e)
        //    {
        //        //Console.WriteLine(e.ToString());
        //    }
        //    finally
        //    {

        //    }
        //}
    }
}
