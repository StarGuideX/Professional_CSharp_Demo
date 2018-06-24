using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
    public class MemoryMappedFilesDemo
    {
        private static ManualResetEventSlim _mapCreated = new ManualResetEventSlim(initialState: false);
        private static ManualResetEventSlim _dataWrittenEvent = new ManualResetEventSlim(initialState: false);
        private const string MAPNAME = "SampleMap";

        public static void MemoryMappedFilesStart()
        {
            Task.Run(() => WriterAsync());
            Task.Run(() => Reader());
            Console.WriteLine("任务开始");
        }

        /// <summary>
        /// 创建内存映射文件后，给事件_mapCreated发出信号，给其他任务提供信息，
        /// 说明己经创建了内存映射文件，可以打开它了。
        /// 调用方法CreateViewAccessor，返回一个MemoryMappedViewAccessor以访问共享的内存。
        /// 使用视图访问器，可以定义这一任务使用的偏移量和大小。
        /// 当然，可以使用的最大大小是内存映射文件的大小。
        /// 这个视图用于写入，因此文件访问设置为MemoryMappedFileAccess.Write。
        /// 接下来，使用MemoryMappedViewAccessor的重载Write方法，可以将原始数据类型写入共享内存。
        /// Write方法总是需要位置信息，来指定数据应该写入的位置。
        /// 写入所有的数据之后，给一个事件发出信号，通知读取器，现在可以开始读取了
        /// </summary>
        /// <returns></returns>
        private static async Task WriterAsync()
        {
            try
            {
                using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateOrOpen(MAPNAME, 10000, MemoryMappedFileAccess.ReadWrite))
                {
                    _mapCreated.Set();
                    Console.WriteLine("共享内存字符段已创建");
                    using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(0, 10000, MemoryMappedFileAccess.Write))
                    {
                        for (int i = 0, pos = 0; i < 100; i++, pos += 4)
                        {
                            // Write方法总是需要位置信息，来指定数据应该写入的位置。
                            accessor.Write(pos, i);
                            Console.WriteLine($"写入{i}在{pos}位置上");
                            await Task.Delay(10);
                        }
                    }

                    _dataWrittenEvent.Set();
                    Console.WriteLine("数据已写入");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 读取器首先等待创建内存映射文件，再使用MemoryMappedFile.0penExisting打开它。
        /// 读取器只需要映射的读取权限。之后，与前面的写入器类似，创建一个视图访问器。
        /// 在读取数据之前，等待设置_dataWrittenEvent()读取类似于写入，
        /// 因为也要提供应该访问数据的位置，但是不同的Read方法，如Readlnt32，用于读取不同的数据类型
        /// </summary>
        private static void Reader()
        {
            try
            {
                Console.WriteLine("reader");
                _mapCreated.Wait();
                Console.WriteLine("reader开始");

                using (MemoryMappedFile mappedFile = MemoryMappedFile.OpenExisting(MAPNAME, MemoryMappedFileRights.Read))
                {
                    using (MemoryMappedViewAccessor accessor = mappedFile.CreateViewAccessor(0, 10000, MemoryMappedFileAccess.Read))
                    {
                        _dataWrittenEvent.Wait();
                        Console.WriteLine("现在可以开始读取");

                        for (int i = 0; i < 400; i += 4)
                        {
                            int result = accessor.ReadInt32(i);
                            Console.WriteLine($"从位置{i}上读取了{result}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public static void MemoryMappedFilesStartUsingStreams()
        {
            Task.Run(() => WriterUsingStreams());
            Task.Run(() => ReaderUsingStreams());
            Console.WriteLine("任务开始(UsingStreams)");
        }

        /// <summary>
        /// 现在创建一个写入器，使用StreamWriter。
        /// MemoryMappedFile中的方法CreateViewStream()返回MemoryMappedViewStream。
        /// 这个方法非常类似于前面使用的CreateViewAccessor()方法，也是在映射内定义一个视图，
        /// 有了偏移量和大小，可以方便地使用流的所有特性。 
        /// 然后使用WriteLineAsync()方法把一个字符串写到流中。
        /// Streamwriter缓存写入操作，所以流的位置不是在每个写入操作中都更新，只在写入器写入块时才更新。
        /// 为了用每次写入的内容刷新缓存，要把StreamWriter的AutoFlush属性设置true
        /// </summary>
        /// <returns></returns>
        private static async Task WriterUsingStreams()
        {
            try
            {
                using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateOrOpen(MAPNAME, 10000, MemoryMappedFileAccess.ReadWrite))
                {
                    _mapCreated.Set();
                    Console.WriteLine("共享内存字符段已创建（WriterUsingStreams）");

                    MemoryMappedViewStream stream = mappedFile.CreateViewStream(0, 10000, MemoryMappedFileAccess.Write);

                    using (var writer = new StreamWriter(stream))
                    {
                        writer.AutoFlush = true;
                        for (int i = 0; i < 100; i++)
                        {
                            string s = $"一些数据{i}";
                            Console.WriteLine($"在{stream.Position}位置上写入{s}");
                            await writer.WriteLineAsync(s);
                        }
                    }
                }

                _dataWrittenEvent.Set();
                Console.WriteLine("数据已写入（WriterUsingStreams）");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 读取器同样用CreateViewStream创建了一个映射视图流，但这次需要读取权限。
        /// 现在可以使用StreamReader()方法从共享内存中读取内容
        /// </summary>
        /// <returns></returns>
        private static async Task ReaderUsingStreams()
        {
            try
            {
                Console.WriteLine("读取器（ReaderUsingStreams）");
                _mapCreated.Wait();
                Console.WriteLine("读取器开始（ReaderUsingStreams）");

                using (MemoryMappedFile mappedFile = MemoryMappedFile.OpenExisting(MAPNAME, MemoryMappedFileRights.Read))
                {
                    MemoryMappedViewStream stream = mappedFile.CreateViewStream(0, 10000, MemoryMappedFileAccess.Read);
                    using (var reader = new StreamReader(stream))
                    {
                        _dataWrittenEvent.Wait();
                        Console.WriteLine("现在可以开始读取（ReaderUsingStreams）");

                        for (int i = 0; i < 100; i++)
                        {
                            long pos = stream.Position;
                            string s = await reader.ReadLineAsync();
                            Console.WriteLine($"从位置{pos}上读取了{s}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
