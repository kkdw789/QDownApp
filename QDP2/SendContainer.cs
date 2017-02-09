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
        private ConcurrentDictionary<long, SendBox> BoxList = new ConcurrentDictionary<long, SendBox>();//待发送队列
        private BlockingCollection<SendBox> SendList = new BlockingCollection<SendBox>();//排队发送队列

        /// <summary>
        /// 警戒块数，暂时不用
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
                LoadBoxs(true);//第一批，回执后继续添加
                while (true)
                {
                    if (SendList.Count > 0)
                    {
                        SendBox item;
                        SendList.TryTake(out item);
                        if (item != null)
                        {
                            UdpHelper.SendData(item);
                            SendList.TryTake(out item);
                        }
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
        /// <param name="IsAnomaly">是否极限添加</param>
        public void LoadBoxs(bool IsAnomaly)
        {
            if (BoxList.Count < BoxAnomalyNum)
            {
                if (BoxList.Count < BoxWarnNum)
                {
                    for (int i = 0; i < BoxWarnNum - BoxList.Count; i++)
                    {
                        SendBox box = new SendBox();
                        BoxList.TryAdd(box.ID, box);
                        box.ActivityStart();
                    }
                }
                else
                {
                    //创建块
                    SendBox box = new SendBox();
                    //添加块至列表
                    BoxList.TryAdd(box.ID, box);
                    //开启块自主传输活动
                    box.ActivityStart();
                }
            }
        }
        /// <summary>
        /// 进入待发送列表
        /// </summary>
        public void AddBox(SendBox box)
        {
            SendList.Add(box);
        }
        /// <summary>
        /// 完成销毁块(块收到回执后 由块自主调用销毁)
        /// </summary>
        public void RemoveBox(SendBox box)
        {
            //SendList.TryTake(out box);
            BoxList.TryRemove(box.ID,out box);
            box.ActivityRemove();
            LoadBoxs(true);//销毁添加
        }
        /// <summary>
        /// 获取回执
        /// </summary>
        public void ReceiptOK(long id)
        {
            try
            {
                SendBox box;
                //找到块
                BoxList.TryGetValue(id, out box);
                if (box != null)
                    //销毁块
                    RemoveBox(box);
                else
                    //记录日志
                    State.Loger = "无法找到数据块";
            }
            catch (Exception ex)
            {
                //记录日志
                State.Loger = ex.Message;
            }
        }
    }
}
