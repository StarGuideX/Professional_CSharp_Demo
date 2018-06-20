using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    public static class DriverInfoDemo
    {
        public static void DriverInfoDemoStart()
        {
            DriveInfo[] divers = DriveInfo.GetDrives();
            foreach (DriveInfo driver in divers)
            {
                if (driver.IsReady)
                {
                    Console.WriteLine($"驱动器名称：{driver.Name}");
                    Console.WriteLine($"文件系统名称：{driver.DriveFormat}");
                    Console.WriteLine($"驱动器类型：{driver.DriveType}");
                    Console.WriteLine($"根目录：{driver.RootDirectory}");
                    Console.WriteLine($"卷标：{driver.VolumeLabel}");
                    Console.WriteLine($"可用空间：{driver.TotalFreeSpace}");
                    Console.WriteLine($"空闲空间：{driver.AvailableFreeSpace}");
                    Console.WriteLine($"总存储空间：{driver.TotalSize}");
                    Console.WriteLine(string.Empty);
                }
            }
        }
    }
}
