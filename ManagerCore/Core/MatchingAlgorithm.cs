using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace ManagerCore.Core
{
    /// <summary>
    /// 配对算法
    /// </summary>
    public class MatchingAlgorithm
    {
        private Group currentGroup;
        private Timer scanningTimer = new Timer();
        public void StartScanning(Group group)
        {
            currentGroup = group;
            Scanning();
        }
        /// <summary>
        /// 扫描对比(计时器)
        /// </summary>
        public void Scanning()
        {
            scanningTimer.Elapsed += delegate
            {
                scanningTimer.Stop();
                //优化：先匹配队列第二的，优化网络
                List<Node> nodes = currentGroup.Nodes.Where(c => c.State == 1).ToList();
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].State == 1)
                    {
                        if (nodes.Count < 2)
                            break;
                        if (i + 1 < nodes.Count)
                            ContrastNode(nodes[i], nodes[i + 1]);
                        else
                            ContrastNode(nodes[i], nodes[0]);
                        if (nodes.Count < 2 || nodes.Count <= i)
                            break;
                    }
                }
                scanningTimer.Start();
            };
            scanningTimer.Interval = 10000;
            scanningTimer.Start();
        }
        /// <summary>
        /// 停止扫描
        /// </summary>
        public void StopScanning()
        {
            scanningTimer.Stop();
        }
        /// <summary>
        /// 对比
        /// </summary>
        /// <returns></returns>
        private void ContrastNode(Node node1, Node node2)
        {
            //if (node1.LastSyncData != node2.LastSyncData)
            //{
                currentGroup.RemoveNode(node1);
                currentGroup.RemoveNode(node2);

                SyncBox box = new SyncBox(node1, node2, this);
                currentGroup.SyncBoxs.Add(box);
            //}
        }
        /// <summary>
        /// 回归队列
        /// </summary>
        public void ReturnNode(Node node1, Node node2,SyncBox box)
        {
            currentGroup.SyncBoxs.Remove(box);
            currentGroup.AddNode(node1);
            currentGroup.AddNode(node2);
        }
    }
}
