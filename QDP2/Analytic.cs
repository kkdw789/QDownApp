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
            lock (obj)
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
                string str = "";
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 1 && i != list.Count - 1)
                        str += list[i] + ",";
                    else if (i > 1 && i == list.Count - 1)
                        str += list[i];
                }
                dataPackage.Data = StringToBytes(str);
                dataPackage.SendData = data;
                return dataPackage;
            }
        }
        /// <summary>
        /// 建立包
        /// </summary>
        public static DataPackage BuildDataPackage(HeaderEnum headerEnum, Int64 id, byte[] str)
        {
            lock (obj)
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.HeaderStr = headerEnum;
                dataPackage.ID = id;
                dataPackage.IsReceiptInfo = false;
                if (headerEnum == HeaderEnum.回执)
                    dataPackage.IsReceiptInfo = true;

                dataPackage.SendTime = DateTime.Now;
                dataPackage.Data = str;
                dataPackage.SendData = MergePackage(dataPackage);

                var sad = Analytic.AnalyticDataPackage(dataPackage.SendData).Data;
                //var sad = Analytic.StringToBytes(Analytic.BytesToString(str));
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] != sad[i] || sad.Length != str.Length)
                        Console.WriteLine("解析结果不一致！");
                }
                //if (!str.Equals(sad))
                //    Console.WriteLine("解析结果不一致！");

                return dataPackage;
            }
        }
        public static DataPackage BuildDataPackage(HeaderEnum headerEnum, Int64 id, string str)
        {
            lock (obj)
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
            //byte[] bytes = new byte[str.Length * sizeof(char)];
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return Analytic.StringToBytes(str);
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
            lock (obj)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return new byte[0];
                //return Encoding.ASCII.GetBytes(str);

                byte[] bytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }
        }
        public static string BytesToString(byte[] bytes)
        {
            lock (obj)
            {
                //return Encoding.ASCII.GetString(bytes);

                char[] chars = new char[bytes.Length / sizeof(char)];
                if (bytes.Length % sizeof(char) > 0)
                    Console.WriteLine("数据转换有问题：");
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }
        }
        /// <summary>
        /// 获取文件传输
        /// </summary>
        /// <returns></returns>
        public static byte[] AnalyticFlieData(Int64 offest)
        {
            lock (obj)
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
        private static object obj = new object();
        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <returns></returns>
        public static void WriteFlieData(Byte[] data)
        {
            lock (obj)
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
