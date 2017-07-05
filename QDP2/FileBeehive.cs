using QDP2.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QDP2
{
    /// <summary>
    /// 文件蜂窝
    /// </summary>
    public class FileBeehive
    {
        //private ConcurrentQueue<SendBox> ThumBoxList = new ConcurrentQueue<SendBox>();
        private ConcurrentDictionary<Int64, DataPackage> ThumBoxList = new ConcurrentDictionary<Int64, DataPackage>();
        public string FilePath { get; set; }
        public string FileInfo { get; set; }
        public string FileName { get; set; }
        private bool isBegin = true;
        private Int64 currentWriteID = 1;
        /// <summary>
        /// 创建蜂窝
        /// </summary>
        public void CreateBeehive()
        {
            Task.Factory.StartNew(() =>
            {
                FileName = "Thum.ls";
                FilePath = Environment.CurrentDirectory + @"\Thum.ls";
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
                //State.FS = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //while (isBegin || ThumBoxList.Count > 0)
                //{
                    //using (State.FS=new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    //{
                        State.FS=new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        while (isBegin || ThumBoxList.Count > 0)
                        {
                            //Thread.Sleep(10);
                            DataPackage item;
                            ThumBoxList.TryRemove(currentWriteID, out item);
                            if (item != null)
                            {
                                System.Console.WriteLine("写入数据！" + currentWriteID);
                                currentWriteID++;
                                Analytic.WriteFlieData(item.Data);
                            }
                            if (ThumBoxList.Count(c => c.Key >= currentWriteID) == 0 && !isBegin)//在完成接收后，如果没有大于的就跳出循环
                            {
                                System.Console.WriteLine("写入跳出完成！" + ThumBoxList.Count + " " + currentWriteID);
                                ThumBoxList = null;
                                break;
                            }
                        //}
                    }

                    //lock (State.FS)
                    //{
                    //    Thread.Sleep(10);
                    //    //DataPackage item = ThumBoxList.FirstOrDefault(f => f.ID == currentWriteID);
                    //    DataPackage item;
                    //    ThumBoxList.TryRemove(currentWriteID, out item);
                    //    if (item != null)
                    //    {
                    //        //System.Console.WriteLine("写入数据！" + currentWriteID);
                    //        currentWriteID++;
                    //        Analytic.WriteFlieData(item.Data);
                    //    }
                    //    if (ThumBoxList.Count(c => c.Key >= currentWriteID) == 0 && !isBegin)//在完成接收后，如果没有大于的就跳出循环
                    //    {
                    //        ThumBoxList = null;
                    //        break;
                    //    }
                    //}
                //}
                System.Console.WriteLine("写入数据完成！" + currentWriteID);
                State.FS.Close();
                State.FS.Dispose();

                FileInfo fileInfo = new FileInfo(FilePath);
                fileInfo.MoveTo(Environment.CurrentDirectory +@"\"+ FileName);
                FilePath = Environment.CurrentDirectory + FileName;
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
        public void CompositeFile(string fileName)
        {
            FileName = fileName;
            isBegin = false;
        }
    }
}
