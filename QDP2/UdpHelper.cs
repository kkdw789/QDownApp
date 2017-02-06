using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 网络帮助类
    /// </summary>
    public static class UdpHelper
    {
        /// <summary>
        /// 初始化UDP客户端
        /// </summary>
        public static void InitCliend()
        {
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Parse(State.ClientInfo.IP), int.Parse(State.ClientInfo.Port));
            State.UDPClient = new UdpClient(localIPEndPoint);

            System.Console.Write("服务初始化");
        }
        /// <summary>
        /// 监听
        /// </summary>
        public static void Monitor()
        {
            //启动接受线程
            Thread threadReceive = new Thread(ReceiveMessages);
            threadReceive.IsBackground = true;
            threadReceive.Start();
            System.Console.Write("监听启动");
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public static void SendData(byte[] bytes)
        {
            //byte[] bytes = Helper.GetBytes(str);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(State.ServerInfo.IP), int.Parse(State.ServerInfo.Port));
            State.UDPClient.Send(bytes, bytes.Length, remoteIPEndPoint);
        }
        /// <summary>
        /// 清理
        /// </summary>
        public static void ClearCliend()
        {
            State.UDPClient.Close();
            System.Console.Write("服务停止");
        }
        /// <summary>
        /// 获取文件包总数
        /// </summary>
        public static long GetFilesNum(string path)
        {
            //62KB+1KB的头
            //byte[] buffer = new byte[63488];
            System.IO.FileInfo f = new FileInfo(path);
            long num = f.Length / State.DataPackageSize;
            return num;
        }
        public static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.Unicode.GetString(bytes, 0, bytes.Length);

            //char[] chars = new char[bytes.Length / sizeof(char)];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            //return new string(chars);
        }
        /// <summary>
        /// 合并数据包
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] MergePackage(string str)
        {
            //Client.Send(Helper.MergePackage("数据," + packID + "," + Client.CurrentConnectionID + "," + Client.FileName + "," + packData)); 
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }




        /// <summary>
        /// 监听信息
        /// </summary>
        private static void ReceiveMessages()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    //关闭QDPState.ServerUdpClient时此句会产生异常
                    //int sh = System.BitConverter.ToInt32(receiveBytes, 0);
                    //string message = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);

                    byte[] receiveBytes = State.UDPClient.Receive(ref remoteIPEndPoint);
                    string message = BytesToString(receiveBytes);
                    System.Console.Write("接收数据:" + message);
                    //AnalyticObj.AnalyticInfo(message);//解析
                }
                catch (Exception ex)
                {
                    System.Console.Write("服务异常:" + ex.Message);
                    break;
                }
            }
        }
    }
}
