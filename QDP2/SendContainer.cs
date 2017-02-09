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
        private ConcurrentDictionary<string, SendBox> BoxList = new ConcurrentDictionary<string, SendBox>();//待发送队列
        private BlockingCollection<SendBox> SendList = new BlockingCollection<SendBox>();//排队发送队列

        /// <summary>
        /// 警戒块数
        /// </summary>
        public int BoxWarnNum { get; set; }
        /// <summary>
        /// 极限块数
        /// </summary>
        public int BoxAnomalyNum { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 开始发送
        /// </summary>
        public void BeginSend(string filePath)
        {
            FilePath = filePath;
            Task.Factory.StartNew(() =>
            {
                LoadBoxs();//第一批，回执后继续添加
                while (true)
                {
                    if (SendList.Count > 0)
                    {
                        SendBox item;
                        SendList.TryTake(out item);
                        if (item != null)
                            UdpHelper.SendData(item);
                    }
                }

            });
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
        /// 加载数据块
        /// </summary>
        public void LoadBoxs()
        {
             
        }
        /// <summary>
        /// 进入待发送列表
        /// </summary>
        public void AddBox()
        {

        }
        /// <summary>
        /// 完成销毁块
        /// </summary>
        public void RemoveBox()
        {

        }
    }
}
