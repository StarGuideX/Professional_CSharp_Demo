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
            while (true)
            {
                PickDemo();
            }
        }

        private static void PickDemo()
        {
            Console.WriteLine("1-DriverInfo-驱动器信息");
            Console.WriteLine("2-Path");
            Console.WriteLine("3-CreateAndCopyFile(path输入2获得)");
            Console.WriteLine("4-Filelnfo-文件信息");
            Console.WriteLine("5-Filelnfo-修改文件属性");
            Console.WriteLine("5-Filelnfo-修改文件属性");
            Console.WriteLine("7-Filelnfo-删除重复文件");
            Console.WriteLine("8-FileStream-读取流，并获取流的信息");
            Console.WriteLine("9-FileStream-写入流");
            Console.WriteLine("10-FileStream-复制流");
            Console.WriteLine("11-暂无-随机访问流");
            #region ReaderWriterDemo
            Console.WriteLine("12-StreamReader-使用StreamReader读取文件");
            Console.WriteLine("13-StreamWriter-使用StreamWriter写入文件");
            Console.WriteLine("14-BinaryWriterr-使用BinaryWriter写入二进制文件");
            

            #endregion
            string testFileName = Environment.CurrentDirectory + @"\Test.txt";

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
                case "6":
                    WorkingWithFilesAndFoldersDemo.ReadingAFileLineByLine("filename");
                    WorkingWithFilesAndFoldersDemo.WriteFile();
                    break;
                case "7":
                    WorkingWithFilesAndFoldersDemo.DeleteDuplicateFiles(directory: @"C:\Users\Stone\Desktop\Test", checkOnly: false);
                    break;
                case "8":
                    StreamDemo.ReadFileUsingFileStream(fileName: @"C:\Users\stone\Desktop\sql update.txt");
                    break;
                case "9":
                    StreamDemo.WriteTextFile();
                    break;
                case "10":
                    StreamDemo.CopyUsingStreams(@"C:\Users\stone\Desktop\sql update.txt", @"C:\Users\stone\Desktop\sql update2.txt");
                    StreamDemo.CopyUsingStreams2(@"C:\Users\stone\Desktop\sql update.txt", @"C:\Users\stone\Desktop\sql update3.txt");
                    break;
                case "12":
                    ReaderWriterDemo.ReadFileUsingReader(testFileName);
                    break;
                case "13":
                    string writeFileUsingWriterTestFileName = Environment.CurrentDirectory + @"\Test(WriteFileUsingWriter).txt";
                    string[] lines = new string[] { "WriteFileUsingWriterText1", "WriteFileUsingWriterText2", "WriteFileUsingWriterText3", "WriteFileUsingWriterText4", "WriteFileUsingWriterText5" };
                    ReaderWriterDemo.WriteFileUsingWriter(writeFileUsingWriterTestFileName, lines);
                    break;
                case "14":
                    string binaryWriterTestFileName = Environment.CurrentDirectory + @"\Test(binaryWriter).txt";
                    ReaderWriterDemo.WriteFileUsingBinaryWriter(binaryWriterTestFileName);
                    break;

                    
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }
    }
}
