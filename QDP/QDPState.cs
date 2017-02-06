using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QDP
{
    public static class QDPState
    {
        public static String LocalIP { get; set; }
        public static int LocalPort { get; set; }

        public static String RemoteIP { get; set; }
        public static int RemotePort { get; set; }

        public static UdpClient ClientUdpClient { get; set; }

        public static int PackSize = 63488;
    }
}
