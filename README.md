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