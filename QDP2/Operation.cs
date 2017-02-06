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
            UdpHelper.SendData(Analytic.BuildDataPackage(HeaderEnum.Q1,0,""));
        }
        /// <summary>
        /// 回执
        /// </summary>
        public static void ReceiptOperation()
        {

        }
        /// <summary>
        /// 建立容器
        /// </summary>
        public static void CreateContainer()
        {

        }
        /// <summary>
        /// 销毁容器
        /// </summary>
        public static void ClearContainer()
        {

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

        }
        /// <summary>
        /// 重连
        /// </summary>
        public static void Chonglian()
        {

        }
    }
}
