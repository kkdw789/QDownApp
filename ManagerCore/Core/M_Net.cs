using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagerCore.Core
{
    //完成一次交互后及时关闭连接，节约资源
    //发送数据前要提前检查连接后发布
    /// <summary>
    /// 与节点通讯组件
    /// </summary>
    public class M_Net
    {
        //开启心跳状态接收

        private static Dictionary<string, Socket> tcpNodes = new Dictionary<string, Socket>();
        private int currentLinked;
        private ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        /// <summary>
        /// 申请加入（首次）
        /// </summary>
        /// <param name="post"></param>
        public static void StateReceive(int post = 8888)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, post);
                socket.Bind(iep);
                socket.Listen(10);
            socket.BeginAccept(new AsyncCallback(CallbackAccept), socket);
        }
        static void CallbackAccept(IAsyncResult iar)
        {
            Socket server = (Socket)iar.AsyncState;
            Socket client = server.EndAccept(iar);
            tcpNodes.Add(client.RemoteEndPoint.ToString(), client);
            SystemManager.Instance.SyncCompleteInfoCallback(client.LocalEndPoint.ToString(), client, "q1");
            new Thread(new ParameterizedThreadStart(ReceiveInfo)).Start(client);
            server.BeginAccept(new AsyncCallback(CallbackAccept), server);
        }
        private void Close(TcpClient client)
        {
            if (client.Connected)
            {
                client.Client.Shutdown(SocketShutdown.Both);
            }
            client.Client.Close();
            client.Close();

            System.Threading.Interlocked.Decrement(ref currentLinked);
        }
        //重连
        public static void ConnectNode(Node node)
        {
            //成功后改变节点状态
            node.State = 1;
        }

        //监听接收消息
        static void ReceiveInfo(object clientObj)
        {
            Node node = clientObj as Node;
            if (node == null)
                return;
            byte[] datas;
            while (true)
            {
                datas = new byte[1024*64];
                int rec = node.TcpNodeSocket.Receive(datas);
                //当客户端发来的消息长度为0时，表明结束通信
                if (rec == 0)
                    break;
            }
            string ss=Encoding.ASCII.GetString(datas,0,datas.Length);
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="node"></param>
        public void CloseConnection(Node node)
        {
            node.TcpNodeSocket.DisconnectAsync(new SocketAsyncEventArgs());
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="node"></param>
        public static bool SendStr(string str, Node node)
        {
            if (node.TcpNodeSocket.Connected)
            {
                int c=node.TcpNodeSocket.Send(Encoding.ASCII.GetBytes(str), SocketFlags.None);
                if (c != 0)
                    return true;
            }
            else
            {
                ConnectNode(node);
                int c = node.TcpNodeSocket.Send(Encoding.ASCII.GetBytes(str), SocketFlags.None);
                if (c != 0)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 掉线节点处理
        /// </summary>
        /// <param name="node"></param>
        public static void DroppedNode(Node node)
        {
            //改变节点状态
            node.State = 2;
        }

        /// <summary>
        /// 同步节点
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public static bool TrySync(Node node1, Node node2)
        {
            if (SendStr("Sync:IP_" + node2.IP + "Port_" + node2.Port, node1))
                return true;
            node1.State = 2;
            return false;
        }
        public void Analysis(string str)
        {

        }
        //新版本提交，ID 节点时间
        public void SubNewVar(string id,DateTime time)
        {

        }

    }
}
