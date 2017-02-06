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
        回执 = 0,
        连接 = 1,
        数据 = 2,
        //重传 = 3,
        完成 = 3,
        重连 = 4
    }
}
