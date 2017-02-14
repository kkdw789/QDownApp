using QDP2.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 文件蜂窝
    /// </summary>
    public class FileBeehive
    {
        //private ConcurrentQueue<SendBox> ThumBoxList = new ConcurrentQueue<SendBox>();
        private ConcurrentDictionary<int, DataPackage> ThumBoxList = new ConcurrentDictionary<int, DataPackage>();
        public string FilePath { get; set; }
        public string FileInfo { get; set; }
        private bool isBegin = true;
        private int currentWriteID = 1;
        /// <summary>
        /// 创建蜂窝
        /// </summary>
        public void CreateBeehive()
        {
            Task.Factory.StartNew(() =>
            {
                FilePath = Environment.CurrentDirectory + @"\test.ls";
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
                State.FS = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                while (isBegin || ThumBoxList.Count > 0)
                {
                    //DataPackage item = ThumBoxList.FirstOrDefault(f => f.ID == currentWriteID);
                    DataPackage item;
                    ThumBoxList.TryRemove(currentWriteID,out item);
                    if (item != null)
                    {
                        currentWriteID++;
                        Analytic.WriteFlieData(item.Data);
                    }
                    if (ThumBoxList.Count(c => c.Key >= currentWriteID) == 0 && !isBegin)//在完成接收后，如果没有大于的就跳出循环
                    {
                        ThumBoxList = null;
                        break;
                    }
                }
                State.FS.Close();
                State.FS.Dispose();
            });
        }
        /// <summary>
        /// 写入块
        /// </summary>
        public void AddBox(DataPackage item)
        {
            ThumBoxList.AddOrUpdate(item.ID, item, (key, value) => { return value = item; });  
        }
        /// <summary>
        /// 合成文件
        /// </summary>
        public void CompositeFile()
        {
            isBegin = false;
        }
    }
}
