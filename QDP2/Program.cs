﻿using System;
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
            Console.WriteLine("输入自己端口：");
            State.ClientInfo.Port = Console.ReadLine();
            Console.WriteLine("输入对方IP");
            State.ServerInfo.Port = "8398";
            UdpHelper.InitCliend(true);
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
            State.ClientInfo.Port = "8398";
            Console.WriteLine("输入对方IP");
            //State.ServerInfo.IP = "14.155.227.171";
            State.ServerInfo.IP = Console.ReadLine();
            Console.WriteLine("输入对方端口：");
            State.ServerInfo.Port = Console.ReadLine();
            //State.ServerInfo.Port = "8399";
            UdpHelper.InitCliend(false);
            UdpHelper.Monitor();
            Console.WriteLine("");
            Console.WriteLine("输入传输文件路径：");
            string path = Console.ReadLine();
            Console.WriteLine("输入传输文件名：");
            string name = Console.ReadLine();
            Operation.CreateContainer(path, name);
            //Operation.CreateContainer(@"H:\OpenStudio-1.13.3.44ac130fa7-Win64.exe");
            //Operation.CreateContainer(@"H:\XX.png");
            //Operation.CreateContainer(@"H:\dotNetFx45.exe");
            //Operation.CreateContainer(@"H:\SQL.iso");
            //Operation.CreateContainer(@"H:\Maya_16.0.1312.exe", "Maya_16.0.1312.exe");
            Operation.ConnectOperation();
            Operation.StartTimeoutWait();
            
            System.Console.ReadLine();
        }
    }
}
