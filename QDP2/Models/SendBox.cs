using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2.Models
{
    /// <summary>
    /// 数据块
    /// </summary>
    public class SendBox
    {
        public SendBox(string fileName)
        {
            ID = State.GetID();
            //加载数据
            var thum = Analytic.AnalyticFlieData(ID);
            if (thum != null)
            {
                Data = Analytic.BuildDataPackage(HeaderEnum.数据, ID, thum);
                BoxStatez = BoxState.NotStart;
                OvertimeObj.OvertimeValue = State.BoxOverTime;
                OvertimeObj.超时事件委托 += OvertimeObj_超时事件委托;
            }
            else
            {
                Data = Analytic.BuildDataPackage(HeaderEnum.完成, ID, Analytic.StringToBytes(fileName));
                State.ContainerStatus.IsCompleted = true;
                BoxStatez = BoxState.Completed;
                OvertimeObj.OvertimeValue = State.CompleteOverTime;
                OvertimeObj.超时事件委托 += OvertimeObj_检查完成事件委托;
                OvertimeObj.OnStart();
            }

        }

        void OvertimeObj_超时事件委托()
        {
            if (OvertimeObj == null)
                return;
            OvertimeObj.OnStop();
            //重新发送
            OvertimeObj.OvertimeValue = State.BoxOverTime + SendNum * State.OvertimeIncreaseNum;
            SendNum++;
            //if (SendNum == 5)
            //{
            //    BoxStatez = BoxState.FailStop;
            //    return;
            //}
            JustSendTime = DateTime.Now;
            State.ContainerStatus.AddBox(this);
            if (OvertimeObj != null)
                OvertimeObj.OnStart();
        }
        void OvertimeObj_检查完成事件委托()
        {
            OvertimeObj.OnStop();
            //重新发送
            OvertimeObj.OvertimeValue = State.CompleteOverTime;
            SendNum++;
            JustSendTime = DateTime.Now;
            if (State.ContainerStatus.BoxList.Count==1)
            State.ContainerStatus.AddBox(this);
            if (OvertimeObj != null)
                OvertimeObj.OnStart();
        }
        public Int64 ID { get; set; }
        /// <summary>
        /// 已经发送次数
        /// </summary>
        public int SendNum { get; set; }
        /// <summary>
        /// 块数据
        /// </summary>
        public DataPackage Data { get; set; }
        /// <summary>
        /// 超时计数器
        /// </summary>
        private Overtime OvertimeObj = new Overtime();
        /// <summary>
        /// 块状态
        /// </summary>
        public BoxState BoxStatez { get; set; }
        /// <summary>
        /// 首次发送时间
        /// </summary>
        public DateTime FirstSendTime { get; set; }
        /// <summary>
        /// 最近发送时间
        /// </summary>
        public DateTime JustSendTime { get; set; }
        public void ActivityStart()
        {
            SendNum++;
            FirstSendTime = DateTime.Now;
            JustSendTime = DateTime.Now;
            if (this.BoxStatez == BoxState.Completed)
                return;
            State.ContainerStatus.AddBox(this);
            OvertimeObj.OnStart();
        }
        public void ActivityRemove()
        {
            OvertimeObj.OnStop();
            OvertimeObj = null;
            this.Data.Data = null;
            this.Data = null;
        }
    }
    public enum BoxState
    {
        NotStart,
        WaitReply,
        OvertimeSend,
        FailStop,
        Completed
    }

}
