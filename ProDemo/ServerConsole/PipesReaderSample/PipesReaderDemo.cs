using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole
{
    public static class PipesReaderDemo
    {
        /// <summary>
        /// 用PipeDirection可以定义了管道的方向。
        /// 服务器流用于读取，因此将方向设置为PipeDirection.In。
        /// 命名管道也可以是双向的，用于读写，此时使用PipeDirection.InOut。
        /// 匿名管道只能是单向的。接下来，调用WaitForConnection()方法，命名管道等待写入方的连接。
        /// 然后，在一个循环中（直到收到消息"bye"），管道服务器把消息读入缓冲区数组，把消息写到控制台
        /// </summary>
        /// <param name="pipeName"></param>
        public static void PipesReader(string pipeName)
        {
            try
            {
                using (var pipeReader = new NamedPipeServerStream(pipeName, PipeDirection.In))
                {
                    pipeReader.WaitForConnection();
                    Console.WriteLine("读取器已连接");

                    const int BUFFERSIZE = 256;

                    bool completed = false;
                    while (!completed)
                    {
                        byte[] buffer = new byte[BUFFERSIZE];
                        int nRead = pipeReader.Read(buffer, 0, BUFFERSIZE);
                        string line = Encoding.UTF8.GetString(buffer, 0, nRead);
                        Console.WriteLine(line);
                        if (line == "bye") completed = true;
                    }
                }
                Console.WriteLine("完成读取");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// 因为NamedPipeServerStream是一个流，
        /// 所以可以使用StreamReader，而不是读取字节数组，该方法简化了代码
        /// </summary>
        /// <param name="pipeName"></param>
        public static void PipesReaderSlimUsingStreamReader(string pipeName)
        {
            try
            {
                var pipeReader = new NamedPipeServerStream(pipeName, PipeDirection.In);
                using (var reader = new StreamReader(pipeName))
                {
                    pipeReader.WaitForConnection();
                    Console.WriteLine("读取器已连接");

                    bool completed = false;
                    while (!completed)
                    {
                        string line = reader.ReadLine();
                        Console.WriteLine(line);
                        if (line == "bye") completed = true;
                    }
                }
                Console.WriteLine("完成读取");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
