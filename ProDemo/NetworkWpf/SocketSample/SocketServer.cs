using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkWpf.SocketSample
{
    public class SocketServer
    {
        /// <summary>
        /// 侦听器创建一个新的Socket对象。
        /// 例如，用TCP与IPv4，地址系列就必须是InterNetwork，流套接字类型Stream、协议类型TCP。
        /// 要使用IPv4创建一个UDP通信，地址系列就需要设置为InterNetwork、套接字类型Dgram和协议类型Udp。
        /// </summary>
        /// <param name="port"></param>
        public void Listener(int port)
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.ReceiveTimeout = 5000;
            listener.SendTimeout = 5000;

            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            // 定义了服务器的缓冲区队列的大小一一在处理连接之前，可以同时连接多少客户端
            listener.Listen(backlog: 15);
            Console.WriteLine($"侦听器在端口{port}正在侦听");


            var cts = new CancellationTokenSource();

            var tf = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            tf.StartNew(() =>
            {
                Console.WriteLine("侦听任务开始");
                while (true)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        break;
                    }
                    Console.WriteLine("等待接受");
                    // 等待客户端连接在Socket类的方法Accept中进行。这个方法阻塞线程，直到客户机连接为止。
                    // 客户端连接后，需要再次调用这个方法，来满足其他客户端的请求；所以在while循环中调用此方法。
                    Socket client = listener.Accept();
                    if (!client.Connected)
                    {
                        Console.WriteLine("没有连接上");
                        continue;
                    }

                    Console.WriteLine($"客户端已连接当前地址 {((IPEndPoint)client.LocalEndPoint).Address} 端口号 {((IPEndPoint)client.LocalEndPoint).Port}" +
                        $"路由地址{((IPEndPoint)client.RemoteEndPoint).Address} 路由端口{((IPEndPoint)client.RemoteEndPoint).Port}");
                    // 为了进行侦听，启动一个单独的任务，该任务可以在调用线程中取消。在方法CommunicateWithClientUsingSocketAsync中执行使用套接字读写的任务。
                    // 这个方法接收绑定到客户端的Socket实例，进行读写：
                    Task t = CommunicateWithClientUsingSocketAsync(client);
                }
                listener.Dispose();
                Console.WriteLine("侦听任务关闭");
            }, cts.Token);

            Console.WriteLine("请按任意键退出");
            Console.ReadLine();
            cts.Cancel();
        }
        /// <summary>
        /// 为了与客户端沟通，创建一个新任务。
        /// 这会释放侦听器任务，立即进行下一次迭代，等待下一个客户端连接。
        /// Socket类的Receive方法接受一个缓冲，其中的数据和标志可以读取，用于套接字。
        /// 这个字节数组转换为字符串，使用Send方法，连同一个小变化一起发送回客户机
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private Task CommunicateWithClientUsingSocketAsync(Socket socket)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (socket)
                    {
                        bool completed = false;
                        do
                        {
                            byte[] readbuffer = new byte[1024];
                            int read = socket.Receive(readbuffer, 0, readbuffer.Length, SocketFlags.None);
                            string fromClient = Encoding.UTF8.GetString(readbuffer, 0, read);
                            Console.WriteLine($"read:{read} bytes:{fromClient}");
                            if (string.Compare(fromClient, "shutdown", ignoreCase: true) == 0)
                            {
                                completed = true;
                            }

                            byte[] writebuffer = Encoding.UTF8.GetBytes($"echo{fromClient}");

                            int send = socket.Send(writebuffer);
                            Console.WriteLine($"send:{send} bytes");
                        } while (!completed);
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        /// <summary>
        /// 使用NetWorkStream和套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private async Task CommunicateWithClientUsingNetworkStreamAsync(Socket socket)
        {
            try
            {
                using (var stream = new NetworkStream(socket, ownsSocket: true))
                {

                    bool completed = false;
                    do
                    {
                        byte[] readBuffer = new byte[1024];
                        int read = await stream.ReadAsync(readBuffer, 0, 1024);
                        string fromClient = Encoding.UTF8.GetString(readBuffer, 0, read);
                        Console.WriteLine($"read {read} bytes: {fromClient}");
                        if (string.Compare(fromClient, "shutdown", ignoreCase: true) == 0)
                        {
                            completed = true;
                        }

                        byte[] writeBuffer = Encoding.UTF8.GetBytes($"echo {fromClient}");

                        await stream.WriteAsync(writeBuffer, 0, writeBuffer.Length);

                    } while (!completed);
                }
                Console.WriteLine("closed stream and client socket");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 通过套接字使用读取器和写入器
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private async Task CommunicateWithClientUsingReadersAndWritersAsync(Socket socket)
        {
            try
            {
                // 因为NetworkStream派生于stream类，还可以使用读取器和写入器访问套接字。
                // 只需要注意读取器和写入器的生存期。调用读取器和与入器的Dispose方法，还会销毁底层的流。
                // 所以要选择StreamReader和Streamwriter的构造函数，其中leaveOption参数可以设置为true。
                // 之后，在销毁读取器和写入器时，就不会销毁底层的流了。NetworkStream在外层using语句的最后销毁，这又会关闭套接字，因为它拥有套接字。
                using (var stream = new NetworkStream(socket, ownsSocket: true))
                using (var reader = new StreamReader(stream, Encoding.UTF8, false, 8192, leaveOpen: true))
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 8192, leaveOpen: true))
                {
                    // 通过套接字使用写入器时，默认情况下，写入器不新数据，所以它们保存在缓存中，直到缓存己满。
                    // 使用网络流，可能需要更快的回应。这里可以把AutoFIush属性设置为true也可以调用FlushAsync方法
                    writer.AutoFlush = true;

                    bool completed = true;
                    do
                    {
                        string fromClient = await reader.ReadLineAsync();
                        Console.WriteLine($"read{fromClient}");
                        if (string.Compare(fromClient, "shutdown", ignoreCase: true) == 0)
                        {
                            completed = true;
                        }
                        await writer.WriteLineAsync($"echo {fromClient}");
                    } while (!completed);
                }
                Console.WriteLine($"关闭流和socket客户端");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
