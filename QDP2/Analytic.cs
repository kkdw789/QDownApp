using QDP2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 解析
    /// </summary>
    public static class Analytic
    {
        /// <summary>
        /// 解析包
        /// </summary>
        public static DataPackage AnalyticDataPackage(byte[] data)
        {
            List<string> list = BytesToString(data).Split(',').ToList();
            DataPackage dataPackage = new DataPackage();
            if (list.Count <= 2)
                return dataPackage;

            dataPackage.HeaderStr = (HeaderEnum)Enum.Parse(typeof(HeaderEnum), list[0]);
            dataPackage.ID = int.Parse(list[1]);
            dataPackage.IsReceiptInfo = false;
            if (dataPackage.HeaderStr == HeaderEnum.回执)
                dataPackage.IsReceiptInfo = true;

            //dataPackage.SendTime = DateTime.Now;
            string str="";
            for (int i = 0; i < list.Count; i++)
			{
                if (i > 1 && i!= list.Count-1)
                    str += list[i]+",";
                else if (i > 1 && i == list.Count - 1)
                    str += list[i];
			}
            dataPackage.Data = StringToBytes(str);
            dataPackage.SendData = data;
            return dataPackage;
        }
        /// <summary>
        /// 建立包
        /// </summary>
        public static DataPackage BuildDataPackage(HeaderEnum headerEnum, Int64 id, byte[] str)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.HeaderStr=headerEnum;
            dataPackage.ID=id;
            dataPackage.IsReceiptInfo=false;
            if(headerEnum==HeaderEnum.回执)
            dataPackage.IsReceiptInfo=true;

            dataPackage.SendTime=DateTime.Now;
            dataPackage.Data=str;
            dataPackage.SendData = MergePackage(dataPackage);
            return dataPackage;
        }
        public static DataPackage BuildDataPackage(HeaderEnum headerEnum, Int64 id, string str)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.HeaderStr = headerEnum;
            dataPackage.ID = id;
            dataPackage.IsReceiptInfo = false;
            if (headerEnum == HeaderEnum.回执)
                dataPackage.IsReceiptInfo = true;

            dataPackage.SendTime = DateTime.Now;
            dataPackage.Data = StringToBytes(str);
            dataPackage.SendData = MergePackage(dataPackage);
            return dataPackage;
        }




        /// <summary>
        /// 合并数据包
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] MergePackage(DataPackage dataPackage)
        {
            //Client.Send(Helper.MergePackage("数据," + packID + "," + Client.CurrentConnectionID + "," + Client.FileName + "," + packData)); 
            string str = dataPackage.HeaderStr + "," + dataPackage.ID + "," + BytesToString(dataPackage.Data);
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// 获取文件包总数
        /// </summary>
        public static long GetFilesNum(string path, out long lastBoxSize)
        {
            //62KB+1KB的头
            //byte[] buffer = new byte[63488];
            System.IO.FileInfo f = new FileInfo(path);
            long num = f.Length / State.DataPackageSize;
            lastBoxSize = f.Length % State.DataPackageSize;
            return num;
        }
        public static byte[] StringToBytes(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return new byte[0];
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static string BytesToString(byte[] bytes)
        {
            //return Encoding.Unicode.GetString(bytes, 0, bytes.Length);

            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        /// <summary>
        /// 获取文件传输
        /// </summary>
        /// <returns></returns>
        public static byte[] AnalyticFlieData(Int64 offest)
        {
            lock (State.FS)
            {
                //62KB+1KB的头
                byte[] buffer = new byte[State.DataPackageSize];
                if (offest == State.ContainerStatus.BoxNum + 1)
                {
                    buffer = new byte[State.ContainerStatus.LastBoxSize];
                }
                if (offest > State.ContainerStatus.BoxNum + 1)
                {
                    return null;
                }
                //Console.WriteLine(State.ContainerStatus.BoxNum + " " + offest);
                try
                {
                    if ((State.FS.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        return buffer;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                    //完成判断或者异常判断
                    return null;
                }
            }
        }
        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <returns></returns>
        public static void WriteFlieData(Byte[] data)
        {
            lock (State.FS)
            {

            try
            {
                State.FS.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }

            }
        }
    }
}
