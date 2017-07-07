using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerCore.Core
{
    public class SyncBox
    {
        //0未完成,1已完成,2异常
        public Dictionary<Node, int> Nodes = new Dictionary<Node, int>();
        private MatchingAlgorithm Ma;
        public SyncBox(Node node1, Node node2,MatchingAlgorithm ma)
        {
            Ma = ma;
            Nodes.Add(node1, 0);
            Nodes.Add(node2, 0);
            node1.SyncFile(node2, this);
            node2.SyncFile(node1, this);
        }
        public void CompleteSync(Node node, bool isOK, string err = "")
        {
            if (isOK)
                Nodes[node] = 1;
            else
                Nodes[node] = 2;
            if(Nodes.First().Value!=0&&Nodes.Last().Value!=0)
            {
                //销毁当前盒子
                DestroyBox();
            }
        }
        /// <summary>
        /// 销毁盒子,暂时未处理异常节点
        /// </summary>
        /// <param name="box"></param>
        public void DestroyBox()
        {
            Ma.ReturnNode(Nodes.First().Key, Nodes.Last().Key, this);
            Nodes = null;
        }
    }
}
