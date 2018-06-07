using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.SomeTools
{
    public class NetWorkTools
    {
        public void UriTool()
        {
            string uri = "https://item.jd.com/12179822.html";
            var page = new Uri(uri);
            StringBuilder sb = new StringBuilder();
            //请求头
            sb.Append($"host:{page.Host},type:{page.HostNameType}\r\n");
            sb.Append($"port:{page.Port}\r\n");
            sb.Append($"path:{page.AbsolutePath}\r\n");
            sb.Append($"query:{page.Query}\r\n");
            foreach (var item in page.Segments)
            {
                sb.Append($"segment:{item}\r\n");
            }
            Console.WriteLine(sb.ToString());
        }
        public void UriBuilderTool()
        {
            var builder = new UriBuilder();
            builder.Host = "www.cninnovation.com";
            builder.Port = 80;
            builder.Path = "training/MVC";
            Uri uri = builder.Uri;
            Console.WriteLine(uri.ToString());
        }
        public void IPAddressTool()
        {
            IPAddress address;
            IPAddress.TryParse("65.52.128.33", out address);
            StringBuilder sb = new StringBuilder();
            foreach (byte item in address.GetAddressBytes())
            {
                int i = 1;
                sb.Append($"byte{i}：{item:X}\r\n");
            }
            sb.Append($"AddressFamily：{address.AddressFamily}\r\n");
            sb.Append($"MapToIPv6：{address.MapToIPv6()}\r\n");
            sb.Append($"静态属性\r\n");
            sb.Append($"IPv4 loopback address：{IPAddress.Loopback}\r\n");
            sb.Append($"IPv6 loopback address：{IPAddress.IPv6Loopback}\r\n");
            sb.Append($"IPv4 broadcast address：{IPAddress.Broadcast}\r\n");
            sb.Append($"IPv4 any address：{IPAddress.Any}\r\n");
            sb.Append($"IPv6 any address：{IPAddress.IPv6Any}\r\n");
            Console.WriteLine(sb.ToString());
        }

        public async Task DNSTool_OnLookupAsync()
        {

            IPHostEntry ipHost = await Dns.GetHostEntryAsync("www.orf.at");
            StringBuilder sb = new StringBuilder();
            sb.Append($"Hostname：{ipHost.HostName}\r\n");

            foreach (var item in ipHost.AddressList)
            {
                sb.Append($"Address Family：{item.AddressFamily}\r\n");
                sb.Append($"Address：{item.MapToIPv4()}\r\n");
            }
            Console.WriteLine(sb.ToString());
        }

        public async Task<string>  HttpClientUsingTCP()
        {
           return await RequestHtmlAsync("10.1.1.11");
        }

        private const int ReadBufferSize = 1024;
        private async Task<string> RequestHtmlAsync(string hostName)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(hostName, 80);
                    NetworkStream stream = client.GetStream();
                    string header = "GET / HTTP/1.1\r\n" +
                        $"Host：{hostName}:80\r\n" +
                        "Connection：close\r\n" +
                        "\r\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(header);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    await stream.FlushAsync();

                    var ms = new MemoryStream();
                    int read = 0;
                    do
                    {
                        read = await stream.ReadAsync(buffer, 0, ReadBufferSize);
                        ms.Write(buffer, 0, read);
                        Array.Clear(buffer, 0, buffer.Length);

                    } while (read > 0);
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
