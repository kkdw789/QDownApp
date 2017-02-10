using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2
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
            //设置配置
            //初始化UDP
            //初始化蜂窝
            //运行蜂窝（传输）
            //初始化系统超时
            Console.WriteLine("输入自己IP");
            State.ClientInfo.IP = "127.0.0.1";
            State.ClientInfo.Port = "8399";
            Console.WriteLine("输入对方IP");
            State.ServerInfo.IP = "127.0.0.1";
            State.ServerInfo.Port = "8398";
            UdpHelper.InitCliend();
            UdpHelper.Monitor();
            System.Console.ReadLine();
        }
        private static void Client()
        {
            //设置配置
            //初始化UDP
            //初始化容器
            //运行容器（传输）
            //初始化系统超时
            Console.WriteLine("输入自己IP");
            State.ClientInfo.IP = "127.0.0.1";
            State.ClientInfo.Port = "8398";
            Console.WriteLine("输入对方IP");
            State.ServerInfo.IP = "127.0.0.1";
            State.ServerInfo.Port = "8399";
            UdpHelper.InitCliend();
            UdpHelper.Monitor();
            Operation.ConnectOperation();
            Operation.StartTimeoutWait();
            //Operation.CreateContainer(@"H:\xf-adsk2016_x64.exe");
            Operation.CreateContainer(@"H:\SQL.iso");
            System.Console.ReadLine();
        }
    }
}
