using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PickDemo(); 
        }

        private static void PickDemo()
        {
            Console.WriteLine("1-DriverInfo-驱动器信息");
            Console.WriteLine("2-Path");
            Console.WriteLine("3-CreateAndCopyFile(path输入2获得)");
            Console.WriteLine("4-Filelnfo-文件信息");
            Console.WriteLine("5-Filelnfo-修改文件属性");
            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    DriverInfoDemo.DriverInfoDemoStart();
                    break;
                case "2":
                    Console.WriteLine(WorkingWithFilesAndFoldersDemo.GetDocumentsFolder());
                    break;
                case "3":
                    WorkingWithFilesAndFoldersDemo.CreateAFile();
                    WorkingWithFilesAndFoldersDemo.CopyFileByFileInfo();
                    WorkingWithFilesAndFoldersDemo.CopyFileByFile();
                    break;
                case "4":
                    string air = Environment.CurrentDirectory;
                    WorkingWithFilesAndFoldersDemo.Filelnformation($"./FilesAndStreamsConsole.exe");
                    break;
                case "5":
                    WorkingWithFilesAndFoldersDemo.ChangeFileProperties();
                    break;
                default:
                    break;
            }

            Console.Read();
        }
    }
}
