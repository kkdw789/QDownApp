using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using QDP2.Models;
namespace QDP2
{
    /// <summary>
    /// 容器
    /// </summary>
    public class SendContainer
    {
        private ConcurrentDictionary<string, SendBox> BoxList = new ConcurrentDictionary<string, SendBox>();
        private BlockingCollection<SendBox> SendList = new BlockingCollection<SendBox>();

        /// <summary>
        /// 警戒块数
        /// </summary>
        public int BoxWarnNum { get; set; }
        /// <summary>
        /// 极限块数
        /// </summary>
        public int BoxAnomalyNum { get; set; }
        /// <summary>
        /// 开始发送
        /// </summary>
        public void BeginSend()
        {

        }
        /// <summary>
        /// 暂停发送
        /// </summary>
        public void StopSend()
        {

        }
        /// <summary>
        /// 速度限制
        /// </summary>
        public void MaxSpeed()
        {

        }
        /// <summary>
        /// 添加块
        /// </summary>
        public void AddBox()
        {

        }
        /// <summary>
        /// 移除块
        /// </summary>
        public void RemoveBox()
        {

        }
    }
}
