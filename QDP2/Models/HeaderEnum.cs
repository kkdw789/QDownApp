using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2.Models
{
    /// <summary>
    /// 头信息（枚举）
    /// </summary>
    public enum HeaderEnum
    {
        Q0 = "回执",
        Q1 = "连接",
        Q2 = "数据",
        Q3 = "重传",
        Q4 = "完成",
        Q5 = "重连"
    }
}
