using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    public static class CompressFileDemo
    {
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="compressedFileName"></param>
        public static void CompressFile(string fileName, string compressedFileName)
        {
            using (var inputStream = File.OpenRead(fileName))
            using (var outputStream = File.OpenWrite(compressedFileName))
            using (var compressStream = new DeflateStream(outputStream, CompressionMode.Compress))
            {
                inputStream.CopyTo(compressStream);
            }
        }

        /// <summary>
        /// 解压缩流
        /// </summary>
        /// <param name="fileName"></param>
        public static void DecompressFile(string fileName)
        {
            FileStream inputStream = File.OpenRead(fileName);
            using (MemoryStream outputStream = new MemoryStream())
            using (var compressStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            {
                compressStream.CopyTo(outputStream);
                outputStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outputStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true))
                {
                    string result = reader.ReadLine();
                    Console.WriteLine(result);
                }
            }
        }

        /// <summary>
        /// 创建解压缩文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="zipFile"></param>
        public static void CreateZipFile(string directory, string zipFile)
        {
            string destDirectory = Path.GetDirectoryName(zipFile);
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }
            FileStream zipStream = File.Create(zipFile);
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(file));
                    using (FileStream inputStream = File.OpenRead(file))
                    using (Stream outputStream = entry.Open())
                    {
                        inputStream.CopyTo(outputStream);
                    }
                }
            }
        }
    }
}
