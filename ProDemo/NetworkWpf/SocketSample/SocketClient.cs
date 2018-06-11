using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkWpf.SocketSample
{
public class SocketClient
{
    /// <summary>
    /// 使用DNS名称解析，从主机名中获得IPHostEnfiy。
    /// 这个IPHostEnfiy用来得到主机的I4地址。
    /// 创建Socket实例后（其方式与为服务器创建代码相同），Connect方法使用该地址连接到服务器。
    /// 连接完成后，调用Sender和Receiver方法，创建不同的任务，这允许同时运行这些方法——接收方客户端可以同时读写服务器
    /// </summary>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public async Task SendAndRecieive(string hostname, int port)
    {
        try
        {
            IPHostEntry ipHost = await Dns.GetHostEntryAsync(hostname);
            IPAddress ipAddress = ipHost.AddressList.Where(address => address.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            if (ipAddress == null)
            {
                Console.WriteLine("no IPv4 address");
                return;
            }

            using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                client.Connect(ipAddress, port);

                Console.WriteLine("客户端已经连接");
                var stream = new NetworkStream(client);
                var cts = new CancellationTokenSource();

                Task tSender = Sender(stream, cts);
                Task tReceiver = Receiver(stream, cts.Token);
                await Task.WhenAll(tSender, tReceiver);
            }

        }
        catch (SocketException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    /// <summary>
    /// Sender方法要求用户输入数据，并使用WriteAsync方法将这些数据发送到网络流。
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task Sender(NetworkStream stream, CancellationTokenSource cts)
    {
        Console.WriteLine("Sender任务");
        while (true)
        {
            Console.WriteLine("输入一个字符串，此字符串用来发送(输入shutdown,可以退出)");
            string line = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes($"{line}\r\n");
            await stream.WriteAsync(buffer, 0, buffer.Length);
            await stream.FlushAsync();
            if (string.Compare(line, "shutdown", ignoreCase: true) == 0)
            {
                cts.Cancel();
                Console.WriteLine("Sender任务关闭");
                break;
            }
        }
    }

    /// <summary>
    /// Receiver方法用ReadAsync方法接收流中的数据。当用户进入终止字符串时，通过CancellationToken从sender任务中发送取消信息：
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cts"></param>
    /// <returns></returns>
    private async Task Receiver(NetworkStream stream, CancellationToken token)
    {
        try
        {
            stream.ReadTimeout = 5000;
            Console.WriteLine("Receiver 任务");

            byte[] readbuffer = new byte[1024];
            while (true)
            {
                Array.Clear(readbuffer,0,1024);
                int read = await stream.ReadAsync(readbuffer,0,readbuffer.Length);
                string receivedLine = Encoding.UTF8.GetString(readbuffer,0,read);
                Console.WriteLine($"received{receivedLine}");
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
}
