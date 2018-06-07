using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.UdpSample
{
    public class UdpReceiver
    {
        /// <summary>
        /// 从接收应用程序开始。该应用程序使用命令行参数来控制应用程序的不同功能。
        /// 所需的命令行参数是-p，它指定接收器可以接收数据的端口号。
        /// 可选参数-g与一个组地址用于多播。
        /// </summary>
        /// <param name="args"></param>
        public void ReveiverStart(string[] args)
        {
            int port;
            string groupAddress;
            if (!ParseCommandLine(args, out port, out groupAddress))
            {
                ShowUsage();
                return;
            }
            ReadAsync(port, groupAddress).Wait();
            Console.ReadLine();
        }

        private void ShowUsage()
        {
            Console.WriteLine("Usage：UdpReceiver -p port [-g groupAddress]");
        }

        /// <summary>
        /// ParseCommandLine方法解析命令行参数，并将结果放入变量port和groupAddress中
        /// </summary>
        /// <param name="args"></param>
        /// <param name="port"></param>
        /// <param name="groupAddress"></param>
        /// <returns></returns>
        private bool ParseCommandLine(string[] args, out int port, out string groupAddress)
        {
            port = 0;
            groupAddress = string.Empty;
            if (args.Length < 2 || args.Length > 5)
            {
                return false;
            }
            if (args.SingleOrDefault(a => a == "-p") == null)
            {
                Console.WriteLine("-p required");
                return false;
            }

            // 端口号
            string port1 = GetValueForKey(args, "-p");
            if (port1 == null || !int.TryParse(port1, out port))
            {
                return false;
            }

            // 群组地址
            groupAddress = GetValueForKey(args, "-g");
            return true;
        }

        private static string GetValueForKey(string[] args, string key)
        {
            int? nextIndex = args.Select((a, i) => new { Arg = a, Index = i }).SingleOrDefault(a => a.Arg == key)?.Index + 1;
            if (!nextIndex.HasValue)
            {
                return null;
            }
            return args[nextIndex.Value];
        }

        private async Task ReadAsync(int port, string groupAddress)
        {
            using (var client = new UdpClient())
            {
                if (groupAddress != null)
                {
                    client.JoinMulticastGroup(IPAddress.Parse(groupAddress));
                    Console.WriteLine($"{IPAddress.Parse(groupAddress)}已添加到多播广播组");
                }

                bool completed = false;

                do
                {
                    Console.WriteLine("UDP开始接受");
                    UdpReceiveResult result = await client.ReceiveAsync();
                    byte[] datagram = result.Buffer;
                    string received = Encoding.UTF8.GetString(datagram);
                    Console.WriteLine($"已接受{received}");
                    if (received == "bye")
                    {
                        completed = true;
                    }
                } while (!completed);

                Console.WriteLine("receiver closing");

                if (groupAddress != null)
                {
                    client.DropMulticastGroup(IPAddress.Parse(groupAddress));
                }
            }
        }
    }
}
