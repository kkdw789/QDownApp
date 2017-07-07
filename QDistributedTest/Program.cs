using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QDistributedTest
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        public void StateReceive()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.66"), 8088);
            TcpListener server = new TcpListener(remoteIPEndPoint);
            while (true)
            {
                server.Start(100);
                tcpClientConnected.Reset();
                IAsyncResult result = server.BeginAcceptTcpClient(new AsyncCallback(Acceptor), server);
                tcpClientConnected.WaitOne();
            }
        }
        private void Acceptor(IAsyncResult o)
        {
            TcpListener server = o.AsyncState as TcpListener;
            Debug.Assert(server != null);
            TcpClient node = null;

            node = server.EndAcceptTcpClient(o);
            System.Threading.Interlocked.Increment(ref currentLinked);
            tcpNodes.Add(node);

            IAsyncResult result = server.BeginAcceptTcpClient(new AsyncCallback(Acceptor), server);
            if (node == null)
            {
                return;
            }
            else
            {
                //node.GetStream().ReadByte
                Thread.CurrentThread.Join();
            }
            //Close(node);
        }
    }
}


//static int socketCount;  //已经开的线程
//static object threadFlag; //多线程的锁旗标
//const int MAX_SOCKET_COUNT = 3;  //最大接受客户端数量
//public static void StateReceive2(int post = 8888)
//{
//    threadFlag = new object();
//    socketCount = 0;

//    Socket serverScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//    IPEndPoint serverPoint = new IPEndPoint(IPAddress.Any, post);

//    serverScoket.Bind(serverPoint);
//    serverScoket.Listen(10);

//    Thread t = null;
//    Socket clientSocket = null;
//    try
//    {
//        while (true)
//        {

//            //判断当前已开的线程数是否超出最大值，超出了则要等待
//            while (socketCount >= MAX_SOCKET_COUNT) Thread.Sleep(1000);
//            clientSocket = serverScoket.Accept();
//            //累计变量自增
//            socketCount++;
//            //IPEndPoint clientPoint = clientSocket.RemoteEndPoint as IPEndPoint;
//            //Console.WriteLine("client {0}:{1} connect", clientPoint.Address, clientPoint.Port);
//            t = new Thread(new ParameterizedThreadStart(ThreadConnect));
//            t.Start(clientSocket);
//        }
//    }
//    finally
//    {
//        serverScoket.Close();
//        serverScoket.Dispose();
//        Environment.Exit(0);

//    }
//}
//static void ThreadConnect(object clientObj)
//{
//    Socket clientSocket = clientObj as Socket;
//    if (clientSocket == null)
//    {
//        lock (threadFlag)
//        {
//            socketCount--;
//        }
//        return;
//    }
//    IPEndPoint clientPoint = clientSocket.RemoteEndPoint as IPEndPoint;
//    tcpNodes.Add(clientSocket.RemoteEndPoint.ToString(), clientSocket);
//    SystemManager.Instance.SyncCompleteInfoCallback(clientSocket.LocalEndPoint.ToString(), clientSocket, "q1");


//    clientSocket.Send(Encoding.ASCII.GetBytes("Hello world"), SocketFlags.None);
//    byte[] datas;
//    int rec;
//    while (true)
//    {
//        datas = new byte[1024];
//        rec = clientSocket.Receive(datas);
//        //当客户端发来的消息长度为0时，表明结束通信
//        if (rec == 0) break;

//        string msg = "Msg has been receive length is " + rec;
//        clientSocket.Send(Encoding.ASCII.GetBytes(msg), SocketFlags.None);
//    }
//    Console.WriteLine("client {0}:{1} disconnect", clientPoint.Address, clientPoint.Port);
//    lock (threadFlag)
//    {
//        //减少当前已开的线程数
//        socketCount--;
//        clientSocket.Close();
//        clientSocket.Dispose();
//    }
//}