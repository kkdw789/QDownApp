using QDP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QDP
{
    /// <summary>
    /// 一个传输添加一个client，一个client可以一对多
    /// </summary>
    public class UDPClient
    {

        public OperationProtocol Operation;
        public AnalyticReceive AnalyticObj;
        /// <summary>
        /// 本次连接的ID
        /// </summary>
        public string CurrentConnectionID = "0";
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName;
        /// <summary>
        /// 当前传输的文件路径
        /// </summary>
        public string FilePath;
        /// <summary>
        /// 文件分割的包数
        /// </summary>
        public long FilesNum;
        /// <summary>
        /// 已发送的包数
        /// </summary>
        public long SendPackNum=0;
        /// <summary>
        /// 已经接收到的数据包
        /// </summary>
        public Dictionary<string, List<string>> ReceivePack = new Dictionary<string, List<string>>();

        /// <summary>
        ///  初始化连接
        /// </summary>
        /// <param name="severIP"></param>
        /// <param name="severPort"></param>
        public void InitConnect(string localIP, int localPort, string severIP, int severPort)
        {
            Operation = new OperationProtocol(this);
            AnalyticObj = new AnalyticReceive(this);

            QDPState.RemoteIP = severIP;
            QDPState.RemotePort = severPort;
            QDPState.LocalIP = localIP;
            QDPState.LocalPort = localPort;
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Parse(localIP), localPort);
            QDPState.ClientUdpClient = new UdpClient(localIPEndPoint);

            //启动接受线程
            Thread threadReceive = new Thread(ReceiveMessages);
            threadReceive.IsBackground = true;
            threadReceive.Start();
            System.Console.Write("服务启动");
        }
        /// <summary>
        /// 传输文件
        /// </summary>
        /// <param name="path"></param>
        public void TransferFile(string path,string name)
        {
            FileName = name;
            FilePath=path;
            FilesNum = Helper.GetFilesNum(path);
            Operation.Connect();
        }
        /// <summary>
        /// 发送信息(通用)
        /// </summary>
        public void Send(byte[] bytes)
        {
            //byte[] bytes = Helper.GetBytes(str);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(QDPState.RemoteIP), QDPState.RemotePort);
            QDPState.ClientUdpClient.Send(bytes, bytes.Length, remoteIPEndPoint);
        }
        /// <summary>
        /// 停止信息
        /// </summary>
        public void CloseReceiveUdpClient()
        {
            System.Console.Write("服务停止");
            QDPState.ClientUdpClient.Close();
        }

        //打包信息
        //解析信息
        //保存至临时文件
        //临时文件验证和整合

        /// <summary>
        /// 监听信息
        /// </summary>
        private void ReceiveMessages()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    //关闭QDPState.ServerUdpClient时此句会产生异常
                    //int sh = System.BitConverter.ToInt32(receiveBytes, 0);
                    //string message = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);
                    byte[] receiveBytes = QDPState.ClientUdpClient.Receive(ref remoteIPEndPoint);
                    string message = Encoding.Unicode.GetString(receiveBytes, 0, receiveBytes.Length);
                    AnalyticObj.AnalyticInfo(message);
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
