using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    public static class StreamDemo
    {
        public static void CopyUsingStreams(string inputFile, string outputFile)
        {
            const int BUFFERSIZE = 4096;
            using (var inputStream = File.OpenRead(inputFile))
            using (var outputStream = File.OpenWrite(outputFile))
            {
                byte[] buffer = new byte[BUFFERSIZE];
                bool completed = false;
                do
                {
                    int nread = inputStream.Read(buffer, 0, BUFFERSIZE);
                    if (nread == 0) completed = true;
                    outputStream.Write(buffer, 0, nread);
                } while (!completed);
            }
        }

        public static void CopyUsingStreams2(string inputFile, string outputFile)
        {
            using (var inputStream = File.OpenRead(inputFile))
            using (var outputStream = File.OpenWrite(outputFile))
            {
                inputStream.CopyTo(outputStream);
            }
        }

        const int RECORDSIZE = 44;

        public static void RandomAccessSample()
        {
            try
            {
                string SampleFilePath = Environment.CurrentDirectory + @"\Sampledata.data";
                using (FileStream stream = File.OpenRead(SampleFilePath))
                {
                    byte[] buffer = new byte[RECORDSIZE];
                    do
                    {
                        try
                        {
                            Console.Write("record 数字(或者'bye' 到结束)：");
                            string line = Console.ReadLine();
                            if (line.ToUpper().CompareTo("BYE") == 0) break;

                            int record;
                            if (int.TryParse(line, out record))
                            {
                                stream.Seek((record - 1) * RECORDSIZE, SeekOrigin.Begin);
                                stream.Read(buffer, 0, RECORDSIZE);
                                string s = Encoding.UTF8.GetString(buffer);
                                Console.WriteLine($"record：{s}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    } while (true);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("需要创建示例文件");
            }
        }

        public static async Task CreateSampleFile(int nRecords)
        {
            string SampleFilePath = Environment.CurrentDirectory + @"\Sampledata.data";
            FileStream stream = File.Create(SampleFilePath);
            using (var writer = new StreamWriter(stream))
            {
                var r = new Random();

                var records = Enumerable.Range(0, nRecords).Select(x => new
                {
                    Number = x,
                    Text = $"Sample text {r.Next(200)}",
                    Data = new DateTime(Math.Abs((long)((r.NextDouble() * 2 - 1) * DateTime.MaxValue.Ticks)))
                });

                foreach (var rec in records)
                {
                    string date = rec.Data.ToString("d", CultureInfo.InvariantCulture);
                    string s = $"#{rec.Number,8};{rec.Text,20};{date}#{Environment.NewLine}";
                    await writer.WriteAsync(s);
                }
            }
        }
        /// <summary>
        /// 写入流
        /// </summary>
        public static void WriteTextFile()
        {
            string tempTextFileName = Path.ChangeExtension(Path.GetTempFileName(), "txt");
            using (FileStream stream = File.OpenWrite(tempTextFileName))
            {
                // 写入UTF-8文件时，需要把序言写入文件。为此，可以使用WriteByte()方法，给流发送3个字
                //stream.WriteByte(0xef);
                //stream.WriteByte(0xbb);
                //stream.WriteByte(0xbf);

                // 序言的替代方案：不需要记住指定编码的字节。Encoding类己经有这些信息了。
                // GetPreamble()方法返回一个字节数组，其中包含文件的序言。这个字节数组使用Stream类的Write()方法写入
                byte[] preamble = Encoding.UTF8.GetPreamble();
                stream.Write(preamble, 0, preamble.Length);

                // 现在可以写入文件的内容。Write()方法需要写入字节数组，所以需要转换字符串。
                // 将字符串转换为UTF-8的字节数组，可以使用Encoding.UTF8.GetBytes完成这个工作，之后写入字节数组
                string hello = "hello,world!";
                byte[] buffer = Encoding.UTF8.GetBytes(hello);
                stream.Write(buffer, 0, buffer.Length);
                Console.WriteLine($"文件 {stream.Name}已写入");
            }
        }

        /// <summary>
        /// 读取流
        /// </summary>
        /// <param name="fileName"></param>
        public static void ReadFileUsingFileStream(string fileName)
        {
            const int BUFFERSIZE = 256; ;
            // 除了使用Filestream类的构造函数来创建Filestream对象之外，
            // 还可以直接使用File类的OpenRead方法创建Filestream。
            // OpenRead方法打开一个文件（类似于FileMode.Open)，
            // 返回一个可以读取的流(FileAccess.Read)，也允许其他进程执行读取访问(FileShare.Read）：
            // using (var stream = File.OpenRead(fileName))，
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ShowStreamInfomation(stream);
                Encoding encoding = GetEncoding(stream);

                byte[] buffer = new byte[BUFFERSIZE];

                bool completed = false;
                do
                {
                    int nread = stream.Read(buffer, 0, BUFFERSIZE);
                    if (nread == 0) completed = true;
                    if (nread < BUFFERSIZE)
                    {
                        Array.Clear(buffer, nread, BUFFERSIZE - nread);
                    }

                    string s = encoding.GetString(buffer, 0, nread);
                    Console.WriteLine($"读取{nread}字节");
                    Console.WriteLine(s);
                } while (!completed);
            }
        }

        /// <summary>
        /// 分析文本文件的编码
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static Encoding GetEncoding(FileStream stream)
        {
            // 是否支持查找
            if (!stream.CanSeek) throw new ArgumentException("需要一个支持查找的Stream");
            Encoding encoding = Encoding.ASCII;

            byte[] bom = new byte[5];
            int nRead = stream.Read(bom, offset: 0, count: 5);
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
            {
                Console.WriteLine("UTF-32");
                stream.Seek(4, SeekOrigin.Begin);
                return Encoding.UTF32;
            }
            else if (bom[0] == 0xff && bom[1] == 0xfe)
            {
                Console.WriteLine("UTF-16, little endian");
                stream.Seek(2, SeekOrigin.Begin);
                return Encoding.Unicode;
            }
            else if (bom[0] == 0xfe && bom[1] == 0xff)
            {
                Console.WriteLine("UTF-16, big endian");
                stream.Seek(2, SeekOrigin.Begin);
                return Encoding.BigEndianUnicode;
            }
            else if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            {
                Console.WriteLine("UTF-8");
                stream.Seek(3, SeekOrigin.Begin);
                return Encoding.UTF8;
            }
            stream.Seek(0, SeekOrigin.Begin);
            return encoding;
        }

        /// <summary>
        /// 获取流的信息
        /// </summary>
        /// <param name="stream"></param>
        private static void ShowStreamInfomation(FileStream stream)
        {
            Console.WriteLine($"stream能够读取：{stream.CanRead}，能写入：{stream.CanWrite}，能查找：{stream.CanSeek}，能超时：{stream.CanTimeout}");
            Console.WriteLine($"长度：{stream.Length}，当前位置：{stream.Position}");
            if (stream.CanTimeout)
            {
                Console.WriteLine($"读取超时时间：{stream.ReadTimeout}，写入超时时间：{stream.WriteTimeout}");
            }
        }
    }
}
