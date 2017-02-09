﻿using System;
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
        public SendBox()
        {
            ID = State.GetID();
            //加载数据
            var thum = Analytic.AnalyticFlieData(ID);
            if (thum != null)
            {
                Data = Analytic.BuildDataPackage(HeaderEnum.数据, ID, thum);
                BoxStatez = BoxState.NotStart;
            }
            else
            {
                Data = Analytic.BuildDataPackage(HeaderEnum.完成, ID, new byte[0]);
                State.ContainerStatus.IsCompleted = true;
                BoxStatez = BoxState.Completed;
            }
            OvertimeObj.OvertimeValue = 1000;
            OvertimeObj.超时事件委托 += OvertimeObj_超时事件委托;
        }

        void OvertimeObj_超时事件委托()
        {
            //重新发送
            SendNum++;
            JustSendTime = DateTime.Now;
            State.ContainerStatus.AddBox(this);
            OvertimeObj.OnStart();
        }
        public int ID { get; set; }
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
