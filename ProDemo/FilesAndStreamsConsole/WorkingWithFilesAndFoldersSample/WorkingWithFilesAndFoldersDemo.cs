using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    public static class WorkingWithFilesAndFoldersDemo
    {
        /// <summary>
        /// Environment类定义了一组特殊的文件夹，来访问.NET4.6的特殊文件夹。
        /// 下面的代码片段通过把枚举值SpecialFolder.MyDocuments传递给GetFolderPath方法，返回documents文件夹。
        /// Environment类的这个特性不可用于.NETCore；因此在以下代码中，使用环境变量HOMEDRIVE和HOMEPATH的值。
        /// </summary>
        /// <returns></returns>
        public static string GetDocumentsFolder()
        {
#if NET46
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#else
            string drive = Environment.GetEnvironmentVariable("HOMEDRIVE");
            string path = Environment.GetEnvironmentVariable("HOMEPATH");
            return Path.Combine(drive, path, "documents");
#endif
        }


        const string Sample1FileName = "Sample1.txt";
        const string Sample2FileName = "Sample2.txt";
        /// <summary>
        /// 创建文件
        /// </summary>
        public static void CreateAFile()
        {
            string fileName = Path.Combine(GetDocumentsFolder(), Sample1FileName);
            File.WriteAllText(fileName, "Hello,Word!");
        }

        /// <summary>
        /// 复制文件(FileInfo)
        /// </summary>
        public static void CopyFileByFileInfo()
        {
            string fileName1 = Path.Combine(GetDocumentsFolder(), Sample1FileName);
            string fileName2 = Path.Combine(GetDocumentsFolder(), Sample2FileName);

            var file1 = new FileInfo(fileName1);
            if (file1.Exists)
            {
                file1.CopyTo(fileName2);
            }
        }

        /// <summary>
        /// 复制文件(File)
        /// </summary>
        public static void CopyFileByFile()
        {
            string fileName1 = Path.Combine(GetDocumentsFolder(), Sample1FileName);
            string fileName2 = Path.Combine(GetDocumentsFolder(), Sample2FileName);

            if (File.Exists(fileName1))
            {
                File.Copy(fileName1, fileName2);
            }
        }

        public static void Filelnformation(string fileName)
        {
            var file = new FileInfo(fileName);
            Console.WriteLine($"名称{file.Name}");
            Console.WriteLine($"目录完整路径{file.DirectoryName}");
            Console.WriteLine($"是否只读{file.IsReadOnly}");
            Console.WriteLine($"扩展名{file.Extension}");
            Console.WriteLine($"文件大小(字节){file.Length}");
            Console.WriteLine($"创建时间{file.CreationTime: F}");
            Console.WriteLine($"上次访问时间{file.LastAccessTime}");
            Console.WriteLine($"目录特性{file.Attributes}");
        }

        public static void ChangeFileProperties()
        {
            string fileName = Path.Combine(GetDocumentsFolder(), Sample1FileName);
            var file = new FileInfo(fileName);
            if (!file.Exists)
            {
                Console.WriteLine($"请在使用此方法之前创建{Sample1FileName}");
                return;
            }
            else
            {
                Console.WriteLine($"创建时间{file.CreationTime:F}");
                file.CreationTime = new DateTime(2023, 12, 24, 15, 0, 0);
                Console.WriteLine($"创建时间{file.CreationTime:F}");
            }
        }
    }
}
