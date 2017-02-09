using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace QDP2
{
    /// <summary>
    /// 超时计数器
    /// </summary>
    public class Overtime
    {
        public delegate void MyEvent();
        public event MyEvent 超时事件委托;
        public Timer monitorTimerPing=new Timer();
        /// <summary>
        ///  超时时间
        /// </summary>
        public int OvertimeValue { get; set; }
        /// <summary>
        /// 开启和重置刷新
        /// </summary>
        public void OnStart()
        {

            monitorTimerPing.Interval = OvertimeValue;
            monitorTimerPing.Elapsed += monitorTimerPing_Tick;
            monitorTimerPing.Start();
        }

        void monitorTimerPing_Tick(object sender, EventArgs e)
        {
            monitorTimerPing.Stop();
            超时事件委托();
        }
        /// <summary>
        /// 停止并清理
        /// </summary>
        public void OnStop()
        {
            monitorTimerPing.Stop();
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void OnPause()
        {
            
        }
    }
   
}
