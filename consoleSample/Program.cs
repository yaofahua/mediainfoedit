/******************************************************************************
 * MediaInfo.NET - A fast, easy-to-use .NET wrapper for MediaInfo.
 * Use at your own risk, under the same license as MediaInfo itself.
 * Copyright (C) 2012 Charles N. Burns
 * 
 * Official source code: https://code.google.com/p/mediainfo-dot-net/
 * 
 ******************************************************************************
 * Program.cs
 * 
 * Demonstrates the use of MediaInfoDotNet by gathering data from a video.
 * (The video is of me after having "rescued" a honey bee from my pool)
 * 
 * To use in a Visual Studio project:
 * Put these files inside all the folders within your project's "/Bin" folder
 *		MediaInfo.dll, MediaInfoDotNet.dll, sample.mkv 
 * 
 * e.g. if your project is named "test", those files would be in: 
 *  "c:\projects\test\test\Bin\Debug\" and "c:\projects\test\test\Bin\Release\"
 * or similar, depending on where you store your projects.
 * 
 ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaInfoDotNet;
using System.IO;
using ExifLibrary;

namespace consoleSample
{
    enum FileType
    {
        Image,
        Video,
        Other
    };

    class Program
	{
        //
        // 目标文件夹args[0]内包含有多级子文件夹，子文件夹内可能有各种不同
        // 格式的视频和图片以及其它文件，本程序的目的就是要把所有的视频和
        // 图片取出来，保存到args[0]上级目录的target文件夹，文件名变更为拍摄
        // 时间
        // target
        //    video
        //        2015
        //            2015-08-01 18-02-32.mp4
        //            2015-08-01 18-02-32.mov
        //            2015-08-01 18-02-32.mpg
        //        2016
        //            2015-08-01 18-02-32.mp4
        //            2015-08-01 18-02-32.mp4
        //        2017
        //            2015-08-01 18-02-32.mp4
        //            2015-08-01 18-02-32.mp4
        //            2015-08-01 18-02-32.mp4
        //    image
        //        2015
        //            2015-08-01 18-02-32.png
        //            2015-08-01 18-02-32.jpg
        //            2015-08-01 18-02-32.png
        //        2016
        //            2015-08-01 18-02-32.png
        //            2015-08-01 18-02-32.jpg
        //        2017
        //            2015-08-01 18-02-32.png
        //            2015-08-01 18-02-32.png
        //            2015-08-01 18-02-32.jpg
        //    other
        //        a.txt
        //        b.doc
        //        ...

        // 将yfh和lsw手机中的照片及视频同步到电视u盘的方法
        // 1、使用公司电脑同步yfh 手机至D:\\Priv\\iphone同步
        // 2、使用家里笔记本电脑同步lsw手机至家里笔记本，再U盘copy到公司电脑D:\\Priv\\lswiphone sync
        // 3、将1和2的增量图片和视频文件放置D:\\Priv\\tmp\\20190624\\source（文件随意放置，为避免文件重名，建议将lsw和yfh单独设置文件夹）
        // 4、运行此程序（结果保存在D:\\Priv\\tmp\\20190624\\target）
        // 5、将D:\\Priv\\tmp\\20190624\\target目录下的所有内容拷贝至电脑u盘
        static void Main(string[] args) {

            //// 检查参数
            //if (args.Length != 1){
            //    PrintUsage();
            //    return;
            //}

            //// 检查源文件夹
            //string srcPath = args[0];
            //if (!CheckSrcPath(srcPath)) {
            //    return;
            //}
            //string srcPath = "D:\\Priv\\lswiphone sync";  // lsw的手机同步到20190625(家里笔记本同步，U盘copy到公司电脑)
            //string srcPath = "D:\\Priv\\iphone同步";  // yfh的手机同步到20190625
            string srcPath = "D:\\Priv\\tmp\\20190624\\source";

            // 获取目标文件夹
            //string tarPath = GetTargetPath(srcPath);
            string tarPath = "D:\\Priv\\tmp\\20190624\\target";
            string imagePath = tarPath + "\\image";
            string videoPath = tarPath + "\\video";
            string otherPath = tarPath + "\\other";

            // 获取源文件夹下的所有文件列表
            List<string> allFiles = GetAllFiles(srcPath);
            if (allFiles.Count() <= 0) {
                Console.WriteLine("Err: 没有文件");
                return;
            }

            // 遍历文件
            int imageCnt = 0;
            int videoCnt = 0;
            int otherCnt = 0;
            for (int i = 0; i < allFiles.Count(); i++) {
                string file = allFiles[i];
                FileType type = GetFileType(file);

                // 消息
                Console.WriteLine("{0}: {1}", i, file);

                // 不处理非视频图片文件
                if (type == FileType.Other)
                {
                    CopyOtherFile(file, otherPath);
                    otherCnt++;
                    continue;
                }
                else if (type == FileType.Image)
                    imageCnt++;
                else// if (type == FileType.Video)
                    videoCnt++;

                // 处理图片或视频
                string path = (type == FileType.Image) ? imagePath : videoPath;
                DateTime encodedDate = GetEncodedDate(file, type);
                CopyImageOrVideo(file, path, encodedDate);
            }

            // 消息
            Console.WriteLine("Image:{0}, Video:{1}, Other:{2}.", imageCnt, videoCnt, otherCnt);
        }

        /// <summary>
        /// 打印帮助
        /// </summary>
        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("      请指定文件夹路径");
        }

        /// <summary>
        /// 拷贝视频或图片
        /// </summary>
        /// <param name="file">原始路径</param>
        /// <param name="path">imagePath or videoPath</param>
        /// <param name="encodedDate">视频或图片的拍摄时间</param>
        static void CopyImageOrVideo(string file, string path, DateTime encodedDate)
        {
            FileInfo info = new FileInfo(file);

            string newFile = String.Format("{0}\\{1}\\{2}{3}", path, encodedDate.ToLocalTime().ToString("yyyy"), 
                encodedDate.ToLocalTime().ToString("yyyy-MM-dd HH-mm-ss"), info.Extension);

            string newDir = new FileInfo(newFile).Directory.FullName;
            if (!Directory.Exists(newDir))
                Directory.CreateDirectory(newDir);
            
            File.Copy(file, GetNewPath(newFile,false));

            
        }

        /// <summary>
        /// 获取视频或者图片的拍摄时间
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type">图片或视频</param>
        /// <returns></returns>
        static DateTime GetEncodedDate(string file, FileType type)
        {
            if (type == FileType.Video)
            {
                var mf = new MediaFile(file);
                return mf.encodedDate;
            }
            else if (type == FileType.Image)
            {
                ExifFile data = ExifFile.Read(file);
                foreach (ExifProperty item in data.Properties.Values)
                {
                    if (item.Tag == ExifTag.DateTimeOriginal) {
                        return (DateTime)item.Value;
                    }
                    if (item.Tag == ExifTag.DateTimeDigitized)
                    {
                        return (DateTime)item.Value;
                    }
                }

                return DateTime.Now;
            }
            else
                return DateTime.Now;
        }

        /// <summary>
        /// 将非视频图片文件拷贝到other文件夹
        /// </summary>
        /// <param name="file"></param>
        /// <param name="otherPath"></param>
        static void CopyOtherFile(string file, string otherPath)
        {
            FileInfo info = new FileInfo(file);
            string newFile = GetNewPath(otherPath + "\\" + info.Name, false);

            string newDir = new FileInfo(newFile).Directory.FullName;
            if (!Directory.Exists(newDir))
                Directory.CreateDirectory(newDir);

            File.Copy(file, newFile);
        }

        /// <summary>
        /// old文件夹或文件不存在则返回old，如存在则
        /// 加数字后缀直到新路径不存在
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        static string GetNewPath(string old, bool dirOrFile)
        {
            int cnt = 1;
            string newPath = old;
            while(true)
            {
                if (dirOrFile && !Directory.Exists(newPath)) // 文件夹
                    return newPath;
                if (!dirOrFile && !File.Exists(newPath))     // 文件
                    return newPath;

                // new name
                if (dirOrFile)
                {
                    newPath = old + "_" + (cnt++);
                }
                else {
                    FileInfo info = new FileInfo(old);
                    newPath = string.Format("{0}\\{1}_{2}{3}", info.Directory.FullName, 
                        info.Name.Substring(0, info.Name.LastIndexOf(".")), cnt++, info.Extension);
                }
            }
        }

        public static bool ExtContains(string fileName, string strFilter)
        {
            char[] separtor = { '|' };
            string[] tempFileds = StringSplit(strFilter, separtor);
            foreach (string str in tempFileds)
            {
                if (str.ToUpper() == fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf(".")).ToUpper()) { return true; }
            }
            return false;
        }
        // 通过字符串，分隔符返回string[]数组 
        public static string[] StringSplit(string s, char[] separtor)
        {
            string[] tempFileds = s.Trim().Split(separtor); return tempFileds;
        }

        /// <summary>
        /// 返回文件类型
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static FileType GetFileType(string file)
        {
            if (ExtContains(file, ".jpeg|.gif|.jpg|.png|.bmp|.pic|.tiff|.ico|.iff|.lbm|.mag|.mac|.mpt|.opt|"))
            {
                ExifFile data;
                try
                {
                    data = ExifFile.Read(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(", read exif error.");
                    return FileType.Other;
                }

                foreach (ExifProperty item in data.Properties.Values)
                {
                    if (item.Tag == ExifTag.DateTimeOriginal || item.Tag == ExifTag.DateTimeDigitized)
                    {
                        DateTime dt = (DateTime)item.Value;
                        // 没有拍摄时间
                        if (dt.Year <= 1900)
                            return FileType.Other;
                        else
                            return FileType.Image;
                    }
                }
                return FileType.Other;
            }
            if (ExtContains(file, ".mov|.avi|.rmvb|.rm|.asf|.divx|.mpg|.mpeg|.mpe|.wmv|.mp4|.mkv|.vob"))
            {
                var mf = new MediaFile(file);
                // 没有拍摄时间
                if (mf.encodedDate.Year <= 1900)
                    return FileType.Other;
                else
                    return FileType.Video;
            }
            else
                return FileType.Other;
        }

        /// <summary>
        /// 存在该文件夹且文件夹下存在视频或图片文件返回true，
        /// 否则返回false
        /// </summary>
        /// <param name="path">原始文件夹路径</param>
        /// <returns></returns>
        static bool CheckSrcPath(string path) {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 获取目标文件夹，如果target不存在或者为空文件夹返回target,
        /// 如果target不为空则返回target2...
        /// 例如：
        ///     path：D:\abc\bbb
        ///     target: D:\abc\target
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        static string GetTargetPath(string path) {
            DirectoryInfo dir = new DirectoryInfo(path);
            return GetNewPath(dir.Parent.FullName + "\\target", true);
        }

        /// <summary>
        /// 获取path文件下包括所有子文件夹的所有文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        static List<string> GetAllFiles(string path) {
            List<string> all = new List<string>();

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                long size = f.Length;
                all.Add(f.FullName);
            }
            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo d in dii)
            {
                foreach (string f in GetAllFiles(d.FullName))
                {
                    all.Add(f);
                }
            }

            return all;
        }
    }
}
