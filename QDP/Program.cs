using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.服务启动,2.开始连接");
            string str =Console.ReadLine();
            if(str=="1")
            {
                Server();
            }else
            {
                Client();
            }
        }
        private static void Server()
        {
            UDPClient client = new UDPClient();
            client.InitConnect("127.0.0.1", 8399, "127.0.0.1", 8398);
            System.Console.ReadLine();
        }
        private static void Client()
        {
            UDPClient client = new UDPClient();
            client.InitConnect("127.0.0.1", 8398, "127.0.0.1", 8399);
            client.TransferFile(@"H:\12.exe","12.exe");
            System.Console.ReadLine();
        }
    }
}
