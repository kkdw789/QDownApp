using NodeCore.Core.SyncServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NodeCore.Core
{
    public class SystemManager
    {
        private static N_Net netServer = new N_Net();

        public IPEndPoint ServerIP =new IPEndPoint(IPAddress.Parse("192.168.1.75"), 8888);
        private static object _lockOtherData = new object();//初始化锁
        private static SystemManager _instance = null;//自身对象
        /// <summary>
        /// 单例
        /// </summary>
        public static SystemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockOtherData)
                    {
                        if (_instance == null)
                        {
                            _instance = new SystemManager();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 与节点同步
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void SyncNode(string ip,string port)
        {
            netServer.client.ClientSendMessage("SyncEnd:Data_");//返回版本最新时间
        }
        public void StartNode()
        {
            netServer.StartNode();
        }
        public bool RegisterNode()
        {
            return netServer.RegisterNode();
        }
    }
}
