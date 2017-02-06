using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP.Core
{
    /// <summary>
    /// 单次连接操作
    /// </summary>
    public class OperationProtocol
    {
        private UDPClient Client;
        public OperationProtocol(UDPClient _client){ Client = _client; }
        
        /// <summary>
        /// 连接（需要回执，用于申请对被动方的连接和传输相关信息）{线程数量，文件包数，文件MD5，文件大小，文件名，最后修改时间}
        /// </summary>
        public void Connect() 
        {
            Client.Send(Helper.MergePackage(@"连接,1," + Client.FilesNum + ",0,10," + Client.FileName + "," + DateTime.Now)); 
        }
        /// <summary>
        /// 数据（包含数据的纯包）{包含包ID、数据本次传输ID、文件名、数据}
        /// </summary>
        /// <param name="packID">包ID</param>
        /// <param name="packData">包数据</param>
        public void TransmitData(string packID,byte[] packData) 
        {
            Client.Send(Helper.MergePackage("数据," + packID + "," + Client.CurrentConnectionID + "," + Client.FileName + "," + packData)); 
        }
        /// <summary>
        /// 完成（需要回执，用于告诉传输完成）{文件名、数据本次传输ID}
        /// </summary>
        public void CompleteTransmit() 
        {
            Client.Send(Helper.MergePackage("完成," + Client.FileName + "," + Client.CurrentConnectionID));
        }

        /// <summary>
        /// 部成（完成100个包的传输）{文件名、数据本次传输ID}
        /// </summary>
        public void StageComplete()
        {
            Client.Send(Helper.MergePackage("部成," + Client.FileName + "," + Client.CurrentConnectionID));
        }


        /// <summary>
        /// 回执（确认主动方信息）{回执、数据本次传输ID、请求信息}传输ID由服务端生成
        /// </summary>
        /// <param name="str">请求信息</param>
        public void ReplyConnect(string str) 
        {
            Client.Send(Helper.MergePackage("回执," + Client.CurrentConnectionID + "," + str));
        }
        /// <summary>
        /// 重传（用于反馈需要重传的包信息）{重传，数据本次传输ID，包ID}，重传是否需要重新生成一个连接ID？
        /// </summary>
        /// <param name="Idlist">需要重新上传的列表</param>
        public void RemedyUpload(List<string> Idlist) 
        {
            string list = "" ;
            foreach (var item in Idlist)
            {
                list += item+"、";
            }
            Client.Send(Helper.MergePackage("重传," + Client.CurrentConnectionID + "," + list));
        }

    }
}
