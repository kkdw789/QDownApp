using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ManagerCore.Core
{
    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager
    {

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

        private List<Group> Groups = new List<Group>();
        public void StartService()
        {
            M_Net.StateReceive();
            Group group = new Group("q1", new MatchingAlgorithm());

            //测试
            if (Groups.Find(f => f.GroupId == "q1") == null)
                Groups.Add(group);
        }
        private void AddNode(string groupName,Node newNode)
        {
            Groups.FirstOrDefault(f => f.GroupId == groupName).Nodes.Add(newNode);
        }
        /// <summary>
        /// 同步信息接收回调
        /// </summary>
        public void SyncCompleteInfoCallback(string id,DateTime lastTime, bool isOK, string err)
        {
            foreach (var item in Groups)
            {
                Node n=item.QueryNode(id);
                if (n != null)
                    n.SyncEnd(isOK, err, lastTime);
            }
        }
        /// <summary>
        /// 节点连接信息接收回调
        /// </summary>
        public void SyncCompleteInfoCallback(string id,Socket tcpClient,string addGroup)
        {
            Node node = new Node();
            node.NewNode(id,tcpClient);
            Groups.Find(f=>f.GroupId=="q1").AddNode(node);
        }
    }
}
