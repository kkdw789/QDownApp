using QDP2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 状态
    /// </summary>
    public static class State
    {
        public static UdpClient UDPClient;
        /// <summary>
        /// 容器
        /// </summary>
        public static SendContainer ContainerStatus;
        /// <summary>
        /// 蜂窝
        /// </summary>
        public static FileBeehive FileBeehiveObj;
        /// <summary>
        /// 客户端信息(自己)
        /// </summary>
        public static ClientDetails ClientInfo=new ClientDetails();
        /// <summary>
        /// 服务端信息(对方)
        /// </summary>
        public static ClientDetails ServerInfo=new ClientDetails();
        public static FileStream FS;
        /// <summary>
        /// 配置信息
        /// </summary>
        public static SetterDetails SetterDetails=new SetterDetails();
        /// <summary>
        /// 日志
        /// </summary>
        public static string Loger;
        /// <summary>
        /// 系统超时器
        /// </summary>
        public static Overtime SystemOvertime=new Overtime();
        public static int BoxOverTime = 800;
        public static int CompleteOverTime = 1000;
        public static int BoxWarnNum = 5;
        public static int BoxAnomalyNum = 15;
        /// <summary>
        /// 是否连接
        /// </summary>
        public static bool IsConn = false;
        /// <summary>
        /// 块大小
        /// </summary>
        public static int BoxSize;
        /// <summary>
        /// 包大小
        /// </summary>
        public static int DataPackageSize = 10000;//或2000
        //public static int DataPackageSize = 63488;
        public static int IDvalue = 0;
        public static int GetID()
        {
            lock (UDPClient)
            {
                IDvalue++;
                return IDvalue;
            }
        }
    }
}
