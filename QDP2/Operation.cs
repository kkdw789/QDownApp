using QDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 操作
    /// </summary>
    public static class Operation
    {
        /// <summary>
        /// 连接请求
        /// </summary>
        public static void ConnectOperation()
        {
            UdpHelper.SendData(Analytic.BuildDataPackage(HeaderEnum.连接,0,""));
        }
        /// <summary>
        /// 回执
        /// </summary>
        public static void ReceiptOperation(byte[] str)
        {
            UdpHelper.SendData(Analytic.BuildDataPackage(HeaderEnum.回执, 0, str));
        }
        /// <summary>
        /// 建立容器
        /// </summary>
        public static void CreateContainer(string FilePath)
        {
            SendContainer container = new SendContainer();
            State.ContainerStatus = container;
            container.BoxWarnNum = State.BoxWarnNum;
            container.BoxAnomalyNum = State.BoxAnomalyNum;
            container.FilePath = FilePath;
        }
        /// <summary>
        /// 销毁容器
        /// </summary>
        public static void ClearContainer()
        {
            State.ContainerStatus.ContainerDispose();
        }
        /// <summary>
        /// 开启超时等待
        /// </summary>
        public static void StartTimeoutWait()
        {

        }
        /// <summary>
        /// 关闭超时等待
        /// </summary>
        public static void StopTimeoutWait()
        {

        }
        /// <summary>
        /// 建立蜂窝
        /// </summary>
        public static void CreateFileBeehive()
        {
            State.FileBeehiveObj = new FileBeehive();
            State.FileBeehiveObj.CreateBeehive();

        }
        /// <summary>
        /// 重连
        /// </summary>
        public static void Chonglian()
        {

        }
        private static int cc = 1;
        /// <summary>
        /// 接收判断与响应
        /// </summary>
        public static void ResponseLogic(DataPackage data)
        {
            switch (data.HeaderStr)
            {
                //System.Console.Write("接收数据:" + data.HeaderStr);
                case HeaderEnum.回执:
                    ResponseLogic2(Analytic.AnalyticDataPackage(data.Data));
                    break;
                case HeaderEnum.连接://建立连接并且服务端完成
                    System.Console.WriteLine("建立连接！");
                    State.IsConn = true;
                    ReceiptOperation(data.SendData);
                    break;
                case HeaderEnum.数据://把储存起来
                    //System.Console.Write("接收数据！" + data.ID + "\n");
                    if (State.FileBeehiveObj == null)
                        CreateFileBeehive();
                    State.FileBeehiveObj.AddBox(data);
                    ReceiptOperation(Analytic.BuildDataPackage(HeaderEnum.数据, data.ID, "").SendData);
                    cc++;
                    break;
                case HeaderEnum.完成://组装文件
                    System.Console.WriteLine("接收数据完成！" + data.ID);
                    System.Console.WriteLine("接收数据总数！" + cc);
                    State.FileBeehiveObj.CompositeFile();
                    ReceiptOperation(Analytic.BuildDataPackage(HeaderEnum.完成, data.ID, "").SendData);
                    break;
                case HeaderEnum.重连:
                    break;
                default:
                    break;
            }

        }
        public static DateTime datetime;
        /// <summary>
        /// 接收回执处理（不用返回数据包）
        /// </summary>
        /// <param name="data"></param>
        private static void ResponseLogic2(DataPackage data)
        {
            switch (data.HeaderStr)
            {
                case HeaderEnum.连接://建立连接并且客户端完成
                    System.Console.WriteLine("建立连接！");
                    State.IsConn = true;
                    Console.WriteLine("开始传输");
                    datetime = DateTime.Now;
                    State.ContainerStatus.BeginSend();
                    break;
                case HeaderEnum.数据://删除容器块，并重新加载
                    //返回给块的自主程序，让其自行销毁
                    //System.Console.Write("接收数据回执！" + data.ID + "\n");
                    State.ContainerStatus.ReceiptOK(data.ID);
                    break;
                case HeaderEnum.完成://结束传输
                    System.Console.WriteLine("接收数据回执完成！" + data.ID);
                     System.Console.WriteLine(DateTime.Now.Subtract(datetime));
                    State.ContainerStatus.ReceiptOK(data.ID);
                    ClearContainer();
                    break;
                case HeaderEnum.重连:
                    break;
                default:
                    System.Console.WriteLine("接收数据有误！");
                    break;
            }
        }
    }
}
