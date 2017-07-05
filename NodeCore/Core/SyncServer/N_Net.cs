using System;
using System.Collections.Generic;
using System.Linq;
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
        public void GetClient()
        {
            TcpClient tcpClient = new TcpClient("192.168.1.66", 8088);
            NetworkStream stream = tcpClient.GetStream();//通过网络流进行数据的交换  
            //while (true)
            //{
                //read来读取数据，write用来写入数据就是发送数据  
                string message = "连接"; 
                byte[] data = new byte[1024];
                data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            //}
            //stream.Close();
            //tcpClient.Close();
            //Console.ReadKey();  
        }
    }
}
