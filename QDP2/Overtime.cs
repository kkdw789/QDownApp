using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 超时计数器
    /// </summary>
    public class Overtime
    {
        public string 超时事件委托 { get; set; }
        /// <summary>
        ///  超时时间
        /// </summary>
        public int OvertimeValue { get; set; }
        /// <summary>
        /// 开启
        /// </summary>
        public void OnStart()
        {

        }
        /// <summary>
        /// 停止并清理
        /// </summary>
        public void OnStop()
        {

        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void OnPause()
        {

        }
    }
   
}
