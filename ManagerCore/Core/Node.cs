using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ManagerCore.Core
{
    /// <summary>
    /// 节点行为器
    /// </summary>
    public class Node
    {
        //ID
        public string ID { get; set; }
        //IP
        public string IP { get; set; }
        //Port
        public string Port { get; set; }
        //State
        public long State { get; set; }
        //有效同步最新时间
        public DateTime LastSyncData { get; set; }
        //有效同步节点来源（暂时不用，用于区别最新时间相同的节点无同步问题）
        public DateTime LastSyncFromNode { get; set; }
        //PerformScore性能评分
        public long PerformScore { get; set; }

        //文件列表（暂定）
        public List<string> FileList { get; set; }
        public Group NodeGroup { get; set; }
        public SyncBox NodeBox { get; set; }
        public Socket TcpNodeSocket { get; set; }

        //开启
        public void TryStartNode()
        {
            //通过Net发送消息给Node
        }
        //停止
        public void TryStopNode()
        {
            //通过Net发送消息给Node
        }
        //退出
        public void ExitNode()
        {
            //将节点移除

            //通过Net发送消息给Node
        }
        //创建Node
        public void NewNode(string id, Socket tcpClient)
        {
            ID = id;
            IP = id;
            Port = id;
            LastSyncData = DateTime.MinValue;
            TcpNodeSocket = tcpClient;
        }

        //配置（速度限制、同步时间、卡断链路）
        public void SetterNode()
        {
            //通过Net发送消息给Node
        }
        //同步
        public void SyncFile(Node comNode,SyncBox nodeBox)
        {
            NodeBox = nodeBox;
            //通过Net发送消息给Node
            M_Net.TrySync(this, comNode);
        }
        //同步完成回调
        public void SyncEnd(bool isOK,string err,DateTime lastTime)
        {
            if (isOK)
            {
                LastSyncData = lastTime;
                NodeBox.CompleteSync(this, isOK);
            }
        }
        /// <summary>
        /// 节点变化(在同步的时候变化怎么办)
        /// </summary>
        public void NodeChange()
        {
            
        }
    }
}
