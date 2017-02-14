using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QDP2.Models
{
    /// <summary>
    /// 客户端详情
    /// </summary>
    public class ClientDetails
    {
        public string IP { get; set; }
        public string Port { get; set; }
        private IPEndPoint _IPEndPoint = null;
        public IPEndPoint IPEndPoint { get { return _IPEndPoint; } set { _IPEndPoint = value; } }
    }
}
