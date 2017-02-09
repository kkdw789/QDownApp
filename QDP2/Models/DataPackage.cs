using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2.Models
{
    /// <summary>
    /// 数据包
    /// </summary>
    public class DataPackage
    {
        public DataPackage()
        {
        }
        //public DataPackage(bool isInit)
        //{
        //    Data = new byte[State.DataPackageSize];
        //    SendData = new byte[State.DataPackageSize];
        //}
        public int ID { get; set; }
        /// <summary>
        /// 头信息
        /// </summary>
        public HeaderEnum HeaderStr { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Byte[] Data { get; set; }
        /// <summary>
        /// 发送数据
        /// </summary>
        public Byte[] SendData { get; set; }
        /// <summary>
        /// 是否是回执
        /// </summary>
        public bool IsReceiptInfo { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}
