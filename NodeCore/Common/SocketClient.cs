using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NodeCore.Common
{
    public class SocketClient
    {
        #region 委托方法
        public delegate void ReceiveMessageDelegate(string message);
        public delegate void SocketCloseDelegate();

        public event ReceiveMessageDelegate OnReceiveMessage;
        public event SocketCloseDelegate OnSocketClose;
        #endregion
        byte[] result = new byte[1024];

        public SocketClient(string _ip, int _port)
        {
            ip = _ip;
            port = _port;
        }
        public SocketClient()
        {
        }
        private bool closeSocket = false;
        private Socket socket;
        private string ip = "192.168.1.75";
        private int port = 8888;
        public Socket clientSocket;


        #region 服务端

        public bool StartSocket()
        {
            if (socket == null)
            {
                try
                {

                    IPAddress ipAddress = IPAddress.Parse(ip);
                    IPEndPoint ipe = new IPEndPoint(ipAddress, port);

                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(ipe);
                    socket.Listen(10);
                    Console.WriteLine("启动监听{0}成功", socket.LocalEndPoint.ToString());
                    //通过Clientsoket发送数据  

                    Thread receiveThread = new Thread(SeverReceiveMessage);
                    receiveThread.Start();
                }
                catch (Exception ex)
                {
                    try
                    {
                        socket.Dispose();
                    }
                    catch { }
                    socket = null;
                }
            }
            return socket != null;
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private void SeverReceiveMessage()
        {
            try
            {
                clientSocket = socket.Accept();
                DataHandle(clientSocket);
            }
            catch (Exception)
            {

            }

        }
        public bool SeverSendMessage(string message)
        {
            if (clientSocket != null)
            {
                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                    clientSocket.Send(sendBytes);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return false;
        }
        #endregion




        #region 客户端
        public void StopSocket()
        {
            closeSocket = true;
            if (socket != null)
            {
                socket.Dispose();
                socket = null;
            }
            if (clientSocket != null)
            {
                clientSocket.Dispose();
                clientSocket = null;
            }
        }

        public bool ConnectSocket()
        {
            if (socket == null)
            {
                try
                {

                    IPAddress ipAddress = IPAddress.Parse(ip);
                    IPEndPoint ipe = new IPEndPoint(ipAddress, port);

                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipe);

                    Thread receiveThread = new Thread(ClientReceiveMessage);
                    receiveThread.Start();
                }
                catch
                {
                    try
                    {
                        socket.Dispose();
                    }
                    catch { }
                    socket = null;
                }
            }
            return socket != null;
        }
        private bool SocketConnected()
        {
            try
            {
                return !socket.Poll(1, SelectMode.SelectRead) && (socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
        private void ClientReceiveMessage()
        {
            DataHandle(socket);
        }
        public bool ClientSendMessage(string message)
        {
            if (socket == null)
            {
                StartSocket();
            }
            if (socket != null && socket.Connected)
            {
                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                    socket.Send(sendBytes);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return false;
        }
        #endregion


        private void DataHandle(Socket mySocket)
        {
            while (!closeSocket && socket != null)
            {
                try
                {
                    int receiveLen = 1;
                    string receiveStr = "";
                    int receivedLen = 0;
                    while (receiveLen > 0)
                    {
                        receiveLen = mySocket.Receive(result, result.Length, 0);

                        int needReceiveLen = receiveLen - receivedLen;
                        if (needReceiveLen > 1024)
                        {
                            receiveStr += Encoding.ASCII.GetString(result, 0, 1024);
                            receivedLen += 1024;
                        }
                        else
                        {
                            receiveStr += Encoding.ASCII.GetString(result, 0, needReceiveLen);
                            receivedLen += needReceiveLen;
                        }

                        if (receiveLen == receivedLen)
                        {
                            string temp = receiveStr;
                            try
                            {
                                if (OnReceiveMessage != null)
                                {
                                    OnReceiveMessage(temp);
                                }
                            }
                            catch { }

                        }

                        receivedLen = 0;
                        receiveStr = "";
                    }
                }
                catch
                {
                    try
                    {
                        mySocket.Shutdown(SocketShutdown.Both);
                        mySocket.Close();
                        mySocket.Dispose();
                        mySocket = null;
                    }
                    catch { }
                    try
                    {
                        if (OnSocketClose != null)
                        {
                            OnSocketClose();
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
