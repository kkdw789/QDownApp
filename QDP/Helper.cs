using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP
{
    public static class Helper
    {
        /// <summary>
        /// 合并数据包
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] MergePackage(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// 获取文件的包数
        /// </summary>
        /// <returns></returns>
        public static long GetFilesNum(string path)
        {
            //62KB+1KB的头
            //byte[] buffer = new byte[63488];
            System.IO.FileInfo f = new FileInfo(path);
            long num = f.Length / QDPState.PackSize;
            return num;
        }
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

    }
}
