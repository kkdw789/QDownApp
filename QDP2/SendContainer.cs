using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using QDP2.Models;
using System.IO;
using System.Threading;
namespace QDP2
{
    /// <summary>
    /// 容器
    /// </summary>
    public class SendContainer
    {
        public ConcurrentDictionary<long, SendBox> BoxList = new ConcurrentDictionary<long, SendBox>();//待发送队列
        private ConcurrentQueue<SendBox> SendList = new ConcurrentQueue<SendBox>();//排队发送队列

        /// <summary>
        /// 警戒块数，暂时不用
        /// </summary>
        public int BoxWarnNum { get; set; }
        /// <summary>
        /// 极限块数
        /// </summary>
        public int BoxAnomalyNum { get; set; }
        /// <summary>
        /// 文件总包数
        /// </summary>
        public Int64 BoxNum = 0;
        /// <summary>
        /// 文件尾包大小
        /// </summary>
        public long LastBoxSize = 0;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件名字
        /// </summary>
        public string FileName { get; set; }
        private bool isBegin = true;
        /// <summary>
        /// 开始发送
        /// </summary>
        public void BeginSend()
        {
            State.FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BoxNum = Analytic.GetFilesNum(FilePath, out LastBoxSize);
            Task.Factory.StartNew(() =>
            {
                LoadBoxs();//第一批，回执后继续添加
                while (isBegin)
                {
                    lock (State.FS)
                    {


                        if (SendList.Count > 0)
                        {
                            if (State.SystemOvertime != null && DateTime.Now.Subtract((DateTime)State.SystemOvertime) > new TimeSpan(0, 0, 3))
                            {
                                State.IsConn = false;
                                isBegin = false;
                                return;
                            }
                            SendBox item;
                            bool isGET = SendList.TryDequeue(out item);
                            //bool isTake=SendList.TryTake(out item);
                            if (item != null && isGET)
                            {
                                //Thread.Sleep(10);
                                UdpHelper.SendData(item);

                            }

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
        public bool IsCompleted=false;
        private static object obj = new object();
        /// <summary>
        /// 加载数据块
        /// </summary>
        /// <param name="IsAnomaly">是否添加</param>
        public void LoadBoxs()
        {
            Task.Factory.StartNew(() =>
                {
                    //此处这样写可以获得超高速度，可以对比研究下
                    int num = BoxList.Count;
                    while (num < BoxAnomalyNum)
                    {
                        lock (obj)
                        {

                            if (IsCompleted)
                                return;
                            if (num < BoxWarnNum)
                            {
                                //for (int i = 0; i < BoxWarnNum - num; i++)
                                //{
                                SendBox box = new SendBox(FileName);
                                BoxList.TryAdd(box.ID, box);
                                box.ActivityStart();
                                if (box.BoxStatez == BoxState.Completed)
                                {
                                    IsCompleted = true;
                                    return;
                                }
                                //}
                            }
                            else
                            {
                                //减速
                                //Thread.Sleep(1);
                                //创建块
                                SendBox box = new SendBox(FileName);
                                //添加块至列表
                                BoxList.TryAdd(box.ID, box);
                                //开启块自主传输活动
                                box.ActivityStart();
                                if (box.BoxStatez == BoxState.Completed)
                                {
                                    IsCompleted = true;
                                    return;

                                }
                            }
                        }
                    }
                });
        }
        //public void LoadBoxs()
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        while (isBegin)
        //        {
        //            if (IsCompleted)
        //                return;
        //            lock (obj)
        //            {
        //                int num = BoxList.Count;
        //                if (num < BoxAnomalyNum)
        //                {
        //                    if (num < BoxWarnNum)
        //                    {
        //                        //for (int i = 0; i < BoxWarnNum - num; i++)
        //                        //{
        //                        SendBox box = new SendBox(FileName);
        //                        BoxList.TryAdd(box.ID, box);
        //                        box.ActivityStart();
        //                        if (box.BoxStatez == BoxState.Completed)
        //                        {
        //                            IsCompleted = true;
        //                            return;
        //                        }
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        //System.Console.WriteLine("到达警戒值");
        //                        ////降低速度
        //                        //Thread.Sleep(1);
        //                        //创建块
        //                        SendBox box = new SendBox(FileName);
        //                        //添加块至列表
        //                        BoxList.TryAdd(box.ID, box);
        //                        //开启块自主传输活动
        //                        box.ActivityStart();
        //                        if (box.BoxStatez == BoxState.Completed)
        //                        {
        //                            IsCompleted = true;
        //                            return;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //System.Console.WriteLine("到达极限值");
        //                }
        //            }
        //        }
        //    });
        //}
        /// <summary>
        /// 进入待发送列表
        /// </summary>
        public void AddBox(SendBox box)
        {
            //if (SendList.Count!=0&&SendList.FirstOrDefault(f => f.ID == box.ID) != null)
            //    return;
            SendList.Enqueue(box);
        }
        /// <summary>
        /// 完成销毁块(块收到回执后 由块自主调用销毁)
        /// </summary>
        public void RemoveBox(SendBox box)
        {
            
            //SendList.TryTake(out box);
            if (BoxList.TryRemove(box.ID, out box))
            {
                box.ActivityRemove();
                //LoadBoxs(true);//销毁添加
            }
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
        /// <summary>
        /// 销毁容器（所有块完成时或异常时）
        /// </summary>
        public void ContainerDispose()
        {
            //isBegin = false;
            State.FS.Close();
            State.FS.Dispose();
        }
    }
}
