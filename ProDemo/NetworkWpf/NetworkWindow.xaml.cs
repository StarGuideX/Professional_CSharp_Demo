using NetworkWpf.HttpClientSample;
using NetworkWpf.SomeTools;
using NetworkWpf.TcpServer;
using NetworkWpf.UdpSample;
using NetworkWpf.WPFAppTcpClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetworkWpf
{
    /// <summary>
    /// NetworkWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NetworkWindow : Window
    {
        private HttpClientSampleClass httpClientSample = new HttpClientSampleClass();
        private NetWorkTools netWorkTools = new NetWorkTools();
        public NetworkWindow()
        {
            InitializeComponent();
        }

        private async void HttpClientBtn_Click(object sender, RoutedEventArgs e)
        {
            await httpClientSample.GetDataSimpleAsync();
        }

        private async void GetDataWithHeadersBtn_Click(object sender, RoutedEventArgs e)
        {
            await httpClientSample.GetDataWithHeaders();
        }

        private void UriBtn_Click(object sender, RoutedEventArgs e)
        {
            netWorkTools.UriTool();
        }
        private void UribuilderBtn_Click(object sender, RoutedEventArgs e)
        {
            netWorkTools.UriBuilderTool();
        }

        private void IpAddressBtn_Click(object sender, RoutedEventArgs e)
        {
            netWorkTools.IPAddressTool();
        }
        private async void DnsBtn_Click(object sender, RoutedEventArgs e)
        {
            await netWorkTools.DNSTool_OnLookupAsync();
        }
        private async void HttpClientUsingTCPBtn_Click(object sender, RoutedEventArgs e)
        {
            await netWorkTools.HttpClientUsingTCP();
        }

        private void TcpServerBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                TcpServerMain tsm = new TcpServerMain();
                tsm.run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

        }

        private void TcpClientBtn_Click(object sender, RoutedEventArgs e)
        {
            TcpClientApp tc = new TcpClientApp();
            tc.Show();
        }

        private void UdpSenderBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(()=> {
                UdpSender udpSender = new UdpSender();
                udpSender.SenderStart(new string[] { "-p", "9400", "-h", "localhost" });
            });
            t.SetApartmentState(ApartmentState.MTA);
            t.Start();
        }

        private void UdpReceiverBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(()=> {
                UdpReceiver receiver = new UdpReceiver();
                receiver.ReveiverStart(new string[] { "-p", "9400" });
            });
            t.SetApartmentState(ApartmentState.MTA);
            t.Start();
        }
    }
}
