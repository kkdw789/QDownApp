using QDP2.Models;
using System;
using System.Collections.Generic;
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
        /// 客户端信息(自己)
        /// </summary>
        public static ClientDetails ClientInfo=new ClientDetails();
        /// <summary>
        /// 服务端信息(对方)
        /// </summary>
        public static ClientDetails ServerInfo=new ClientDetails();
        /// <summary>
        /// 文件或蜂窝信息
        /// </summary>
        public static FileDetails FileDetails=new FileDetails();
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
        public static int DataPackageSize;
    }
}
