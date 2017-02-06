using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QDP.Core
{
    /// <summary>
    /// 解析返回数据
    /// </summary>
    public class AnalyticReceive
    {
        private UDPClient Client;
        public AnalyticReceive(UDPClient _client) { Client = _client; }
        /// <summary>
        /// 解析返回数据
        /// </summary>
        /// <param name="mess"></param>
        public void AnalyticInfo(string mess)
        {
            ClientReceive(mess);
            ServerReceive(mess);
        }
        /// <summary>
        /// 主动方
        /// </summary>
        /// <param name="mess"></param>
        private void ClientReceive(string mess)
        {
            //62KB+1KB的头
            byte[] bytes = new byte[QDPState.PackSize];
            if (!mess.Contains("回执,") && !mess.Contains("重传,") || mess.Contains("数据,"))
            {
                return;
            }
            System.Console.Write("客户端接收到数据：" + mess + "\n");
            if (mess.Contains("回执,") && mess.Contains("连接,"))
            {
                Client.CurrentConnectionID = mess.Split(',')[1].ToString();
                Client.SendPackNum += AnalyticFlieData();
                Client.Operation.StageComplete();
                return;
            }
            if (mess.Contains("回执") && mess.Contains("部成"))
            {
                System.Console.Write("部分传输完成\n");
                Client.CurrentConnectionID = mess.Split(',')[1].ToString();
                Client.SendPackNum += AnalyticFlieData();
                if (Client.SendPackNum >= Client.FilesNum)
                    Client.Operation.CompleteTransmit();
                else
                    Client.Operation.StageComplete();
                return;
            }
            if (mess.Contains("回执") && mess.Contains("完成"))
            {
                System.Console.Write("数据传输完成\n");
                return;
            }
            if (mess.Contains("重传,"))
            {
                System.Console.Write("开始重传数据包 \n");
                List<string> list = mess.Split(',')[2].Split('、').ToList();
                foreach (var item in list)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        Client.Operation.TransmitData(item, bytes);
                }
                Client.Operation.CompleteTransmit();
                return;
            }
        }

        /// <summary>
        /// 被动方接收
        /// </summary>
        private void ServerReceive(string mess)
        {
            if (mess.Contains("回执,") || mess.Contains("重传,"))
            {
                return;
            }
            System.Console.Write("服务端接收到数据：" + mess + "\n");
            if (mess.Contains("连接,"))
            {
                List<string> listStr = mess.Split(',').ToList();
                Client.FileName = listStr[5].ToString();
                Client.FilesNum = long.Parse(listStr[2].ToString());
                Client.Operation.ReplyConnect(mess);
                Client.ReceivePack.Clear();
                return;
            }
            if (mess.Contains("数据"))
            {
                List<string> str = mess.Split(',').ToList();

                if (!Client.ReceivePack.ContainsKey(Client.CurrentConnectionID))
                {
                    Client.ReceivePack.Add(Client.CurrentConnectionID, new List<string>());
                }
                if (!Client.ReceivePack[Client.CurrentConnectionID].Contains(str[1]))
                {
                    Client.ReceivePack[Client.CurrentConnectionID].Add(str[1]);
                    SaveFlieData(@"G:\QDP\", str[2]+"_"+str[1],Helper.GetBytes(str[4]));
                }
            }
            if (mess.Contains("部成"))
            {
                System.Console.Write("部分传输完成\n");
                Client.CurrentConnectionID = (int.Parse(mess.Split(',')[2].ToString()) + 1).ToString();
                Client.Operation.ReplyConnect(mess);
                //部成检测
                return;
            }
            if (mess.Contains("完成"))
            {
                Task task = Task.Factory.StartNew(() =>
                   {
                       //Thread.Sleep(5000);
                       //判断接收到的数据包数量
                       long num = 0;
                       foreach (var item in Client.ReceivePack)
                       {
                           num += item.Value.Count;
                       }
                       //完成检测
                       if (num >= Client.FilesNum)
                       {
                           Client.CurrentConnectionID = (int.Parse(mess.Split(',')[2].ToString()) + 1).ToString();
                           Client.Operation.ReplyConnect(mess);
                           System.Console.Write("数据接收完成\n");
                       }
                       else
                       {
                           List<string> list = new List<string>();
                           for (int i = 0; i < Client.FilesNum; i++)
                           {
                               if (!Client.ReceivePack[Client.CurrentConnectionID].Contains(i.ToString()))
                                   list.Add(i.ToString());
                           }
                           Client.Operation.RemedyUpload(list);
                       }
                   });
            }
        }

        /// <summary>
        /// 获取文件传输
        /// </summary>
        /// <returns></returns>
        private int AnalyticFlieData()
        {
            //62KB+1KB的头
            byte[] buffer = new byte[QDPState.PackSize];
            using (FileStream fs = new FileStream(Client.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int offest = 0;
                int offest2 = (int.Parse(Client.CurrentConnectionID) * 100 + offest) * buffer.Length;
                while ((fs.Read(buffer, offest2, buffer.Length)) > 0)
                {
                    Client.Operation.TransmitData(offest.ToString(), buffer);
                    offest++;
                    if (offest == 100 || int.Parse(Client.CurrentConnectionID) * 100 + offest == Client.FilesNum)
                        break;
                }

                fs.Close();
                fs.Dispose();
                return offest;
            }
        }

        /// <summary>
        /// 写入文件保存(ID+1)
        /// </summary>
        /// <returns></returns>
        private void SaveFlieData(string localFilePath, string connectionID_ID, byte[] buffer)
        {
            localFilePath = localFilePath + connectionID_ID + ".qk";
            using (FileStream fs = new FileStream(localFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                fs.Dispose();
            }
        }
    }
}