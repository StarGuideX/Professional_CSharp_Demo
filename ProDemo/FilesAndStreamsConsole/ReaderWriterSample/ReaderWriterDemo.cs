
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    public class ReaderWriterDemo
    {
        /// <summary>
        /// 使用StreamReader读取文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void ReadFileUsingReader(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            using (var reader = new StreamReader(stream))
            {
                // EndOfStream属性可以检查文件的末尾，
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Console.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// 使用StreamWriter写入文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void WriteFileUsingWriter(string fileName, string[] lines)
        {
            var outputStream = File.OpenWrite(fileName);

            using (var writer = new StreamWriter(outputStream))
            {
                byte[] preamble = Encoding.UTF8.GetPreamble();
                outputStream.Write(preamble, 0, preamble.Length);
                writer.Write(lines);
            }
        }

        /// <summary>
        /// 使用BinaryWriter写入二进制文件
        /// </summary>
        /// <param name="binFile"></param>
        public static void WriteFileUsingBinaryWriter(string binFile)
        {
            var outputStream = File.Create(binFile);

            using (var writer = new BinaryWriter(outputStream))
            {
                double d = 47.47;
                int i = 42;
                long l = 987654321;
                string s = "simple";
                writer.Write(d);
                writer.Write(i);
                writer.Write(l);
                writer.Write(s);
            }
        }

        /// <summary>
        /// 使用BinaryReader读取二进制文件
        /// </summary>
        /// <param name="binFile"></param>
        public static void ReadFileUsingBinaryReader(string binFile)
        {
            var inputStream = File.Open(binFile, FileMode.Open);

            using (var reader = new BinaryReader(inputStream))
            {
                double d = reader.ReadDouble();
                int i = reader.ReadInt32();
                long l = reader.ReadInt64();
                string s = reader.ReadString();
                Console.WriteLine($"d：{d}，i：{i}，l：{l}，s：{s}");
            }
        }
    }
}
