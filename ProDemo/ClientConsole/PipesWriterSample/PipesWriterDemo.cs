using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsole.PipesWriterSample
{
    public class PipesWriterDemo
    {
        /// <summary>
        /// 通过实例化一个NamedPipeClientStream对象来创建客户端。
        /// 因为命名管道可以在网络上通信，所以需要服务器名称、管道的名称和管道的方向。
        /// 客户端通过调用Connect()方法来连接。连接成功后，
        /// 就在Streamwriter上调用WriteLine，把消息发送给服务器。
        /// 默认情况下，消息不立即发送；而是缓存起来。调用Flush()方法把消息推到服务器上。
        /// 也可以立即传送所有的消息，而不调用Flush()方法。
        /// 为此，必须配置选项，在创建管道时遍历缓存文件
        /// </summary>
        /// <param name="pipeName"></param>
        public static void PipesWriter(string pipeName)
        {
            try
            {
                var pipeWriter = new NamedPipeClientStream("TheRocks", pipeName, PipeDirection.Out);
                using (var writer = new StreamWriter(pipeWriter))
                {
                    pipeWriter.Connect();
                    Console.WriteLine("写入器已连接");

                    bool completed = false;
                    while (!completed)
                    {
                        string input = Console.ReadLine();
                        if (input == "bye") completed = true;

                        writer.WriteLine(input);
                        writer.Flush();
                    }
                }
                Console.WriteLine("写入已完成");
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
