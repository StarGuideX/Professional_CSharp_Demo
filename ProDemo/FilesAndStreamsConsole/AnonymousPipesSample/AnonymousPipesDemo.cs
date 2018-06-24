using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole
{
public static class AnonymousPipesDemo
{
    private static string _pipeHandle;
    private static ManualResetEventSlim _pipeHandleSet;

    public static void AnonymousPipesStart()
    {
        _pipeHandleSet = new ManualResetEventSlim(initialState: false);

        Task.Run(() => Reader());
        Task.Run(() => Writer());
    }

    /// <summary>
    /// 创建一个AnonymousPipeServerStream，定义PipeDirection.In,把服务器端充当读取器。
    /// 通信的另一端需要知道管道的客户端句柄。这个句柄在GetClientHandleAsString方法中转换为一个字符串，
    /// 赋予_pipeHandele变量。这个变量以后由充当写入器的客户端使用。
    /// 在最初的处理后，管道服务器可以作为一个流，因为它本来就是一个流
    /// </summary>
    private static void Reader()
    {
        try
        {
            var pipeReader = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.None);
            using (var reader = new StreamReader(pipeReader))
            {
                _pipeHandle = pipeReader.GetClientHandleAsString();
                Console.WriteLine($"管道句柄：{_pipeHandle}");
                _pipeHandleSet.Set();

                bool end = false;
                while (!end)
                {
                    string line = reader.ReadLine();
                    Console.WriteLine(line);
                    if (line == "end") end = true;
                }
                Console.WriteLine("读取已完成");
            }
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// 客户端代码等到变量_pipeHandleSet发出信号，就打开_pipeHandele变量引用的管道句柄。
    /// 后来的处理用StreamWriter继续
    /// </summary>
    private static void Writer()
    {
        Console.WriteLine("匿名管道写入器");
        _pipeHandleSet.Wait();

        var pipeWriter = new AnonymousPipeClientStream(PipeDirection.Out, _pipeHandle);
        using (var writer = new StreamWriter(pipeWriter))
        {
            writer.AutoFlush = true;
            Console.WriteLine("开始写入");
            for (int i = 0; i < 5; i++)
            {
                writer.WriteLine($"信息{i}");
                Task.Delay(500).Wait(); 
            }
        }
        Console.WriteLine("结束");
    }

    
}
}
