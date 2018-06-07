using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.UdpSample
{
    public class UdpSender
    {
        public void SenderStart(string[] args)
        {
            int port;
            string hostname;
            bool broadcast;
            string groupAddress;
            bool ipv6;
            if (!ParseCommandLine(args, out port, out hostname, out broadcast, out groupAddress, out ipv6))
            {
                ShowUsage();
                Console.ReadLine();
                return;
            }
            IPEndPoint endpoint = GetIPEndPoint(port, hostname, broadcast, groupAddress, ipv6).Result;
            SenderStart(endpoint, broadcast, groupAddress);
            Console.WriteLine("请按任意键退出");
            Console.ReadLine();
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

        private static bool ParseCommandLine(string[] args, out int port, out string hostname, out bool broadcast, out string groupAddress, out bool ipv6)
        {
            port = 0;
            hostname = string.Empty;
            broadcast = false;
            groupAddress = string.Empty;
            ipv6 = false;
            if (args.Length < 2 || args.Length > 5)
            {
                return false;
            }
            if (args.SingleOrDefault(a => a == "-p") == null)
            {
                Console.WriteLine("-p required");
                return false;
            }
            string[] requiredOneOf = { "-h", "-b", "-g" };
            if (args.Intersect(requiredOneOf).Count() != 1)
            {
                Console.WriteLine("either one (and only one) of -h -b -g required");
                return false;
            }

            // get port number
            string port1 = GetValueForKey(args, "-p");
            if (port1 == null || !int.TryParse(port1, out port))
            {
                return false;
            }

            // get optional host name
            hostname = GetValueForKey(args, "-h");

            broadcast = args.Contains("-b");

            ipv6 = args.Contains("-ipv6");

            // get optional group address
            groupAddress = GetValueForKey(args, "-g");
            return true;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage: UdpSender -p port [-g groupaddress | -b | -h hostname] [-ipv6]");
            Console.WriteLine("\t-p port number\tEnter a port number for the sender");
            Console.WriteLine("\t-g group address\tGroup address in the range 224.0.0.0 to 239.255.255.255");
            Console.WriteLine("\t-b\tFor a broadcast");
            Console.WriteLine("\t-h hostname\tUse the hostname option if the message should be sent to a single host");
        }
        /// <summary>
        /// 发送数据时，需要一个IPEndPoint0根据程序参数，以不同的方式创建它。
        /// 对于广播，IPv4定义了从IPAddress.Broadcast返回的地址255.255.255.255。
        /// 没有用于广播的IPv6地址，因为IPv6不支持广播。IPv6用多播替代广播。多播也添加到IPv4中。
        /// 传递主机名时，主机名使用DNS查找功能和Dns类来解析。
        /// GetHostEntryAsync方法返回一个IPHostEntry，其中IPAddress可以从AddressList属性中检索。
        /// 根据使用IPv4还是IPv6，从这个列表中提取不同的IPAddress。根据网络环境，只有一个地址类型是有效的。
        /// 如果把一个组地址传递给方法，就使用IPAddress.Parse解析地址
        /// </summary>
        /// <param name="port"></param>
        /// <param name="hostname"></param>
        /// <param name="broadcast"></param>
        /// <param name="groupAddress"></param>
        /// <param name="ipv6"></param>
        /// <returns></returns>
        private async Task<IPEndPoint> GetIPEndPoint(int port, string hostname, bool broadcast, string groupAddress, bool ipv6)
        {
            IPEndPoint endpoint = null;
            try
            {
                if (broadcast)
                {
                    endpoint = new IPEndPoint(IPAddress.Broadcast, port);
                }
                else if (hostname != null)
                {
                    IPHostEntry hostEntry = await Dns.GetHostEntryAsync(hostname);
                    IPAddress address;
                    if (ipv6)
                    {
                        address = hostEntry.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6).FirstOrDefault();
                    }
                    else
                    {
                        address = hostEntry.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                    }

                    if (address != null)
                    {
                        Func<string> ipversion = () => ipv6 ? "IPv6" : "IPv4";
                        Console.WriteLine($"no {ipversion()} address for {hostname}");
                        return null;
                    }

                    endpoint = new IPEndPoint(address, port);
                }
                else if (groupAddress != null)
                {
                    endpoint = new IPEndPoint(IPAddress.Parse(groupAddress), port);
                }
                else
                {
                    throw new InvalidOperationException($"需要设置 {nameof(hostname)}、{nameof(broadcast)} 或者 {nameof(groupAddress)}");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return endpoint;
        }

        /// <summary>
        /// 在创建一个UdpClient实例，并将字符串转换为字节数组后，就使用SendAsync方法发送数据。
        /// 请注意接收器不需要侦听，发送方也不需要连接。UDP是很简单的。
        /// 然而，如果发送方把数据发送到未知的地方一一一无人接收数据，也不会得到任何错误消息
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="broadcast"></param>
        /// <param name="groupAddress"></param>
        private void SenderStart(IPEndPoint endpoint, bool broadcast, string groupAddress)
        {
            try
            {
                string locahost = Dns.GetHostName();
                using (var client = new UdpClient())
                {
                    client.EnableBroadcast = broadcast;
                    if (groupAddress != null)
                    {
                        client.JoinMulticastGroup(IPAddress.Parse(groupAddress));
                    }

                    bool completed = false;

                    do
                    {
                        Console.WriteLine("请输入信息或者输入bye退出");
                        string input = Console.ReadLine();
                        Console.WriteLine();
                        completed = input == "bye";
                        byte[] datagram = Encoding.UTF8.GetBytes($"{input} from {locahost}");
                    } while (!completed);

                    if (groupAddress !=null)
                    {
                        client.DropMulticastGroup(IPAddress.Parse(groupAddress));
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
