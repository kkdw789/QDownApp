using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerCore.Core
{
    /// <summary>
    /// 组
    /// </summary>
    public class Group
    {
        public Group(string id,MatchingAlgorithm ma)
        {
            GroupId = id;
            Algorithm = ma;
            ma.StartScanning(this);
        }

        public string GroupId { get; set; }
        private List<Node> _nodes = new List<Node>();
        /// <summary>
        /// 待命节点列表
        /// </summary>
        public List<Node> Nodes { get { return _nodes; }}
        private List<SyncBox> _syncBoxs = new List<SyncBox>();
        /// <summary>
        /// 新加入节点
        /// </summary>
        public void AddNode(Node node)
        {
            Nodes.Add(node);
            SystemManager.Instance.GroupChange(this);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
            SystemManager.Instance.GroupChange(this);
        }
        /// <summary>
        /// 同步中节点列表
        /// </summary>
        public List<SyncBox> SyncBoxs { get { return _syncBoxs; } }
        /// <summary>
        /// 匹配算法
        /// </summary>
        public MatchingAlgorithm Algorithm { get; set; }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node QueryNode(string id)
        {
            foreach (var box in _syncBoxs)
            {
                Node node=box.Nodes.FirstOrDefault(f => f.Key.ID.ToString() == id).Key;
                if (node != null)
                    return node;
            }
            return null;
        }
    }
}
