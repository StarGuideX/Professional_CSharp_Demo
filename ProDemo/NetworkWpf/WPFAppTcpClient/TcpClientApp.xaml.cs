using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetworkWpf.WPFAppTcpClient
{
    /// <summary>
    /// TcpClientApp.xaml 的交互逻辑
    /// </summary>
    public partial class TcpClientApp : Window, INotifyPropertyChanged, IDisposable
    {
        private TcpClient _client = new TcpClient();
        private readonly CustomProtocolConmmand.CustomProtocolConmmands _commands =
            new CustomProtocolConmmand.CustomProtocolConmmands();

        public TcpClientApp()
        {
            InitializeComponent();
        }

        #region Properties
        private string _remoteHost = "localhost";
        private int _serverPory = 8800;
        private string _sessionId;
        private CustomProtocolConmmand _activeCommand;
        private string _log;
        private string _status;

        public string RemoteHost
        {
            get
            {
                return _remoteHost;
            }

            set
            {
                SetProperty(ref _remoteHost, value);
            }
        }
        public int ServerPory
        {
            get
            {
                return _serverPory;
            }

            set
            {
                SetProperty(ref _serverPory, value);
            }
        }
        public string SessionId
        {
            get
            {
                return _sessionId;
            }

            set
            {
                SetProperty(ref _sessionId, value);
            }
        }
        public CustomProtocolConmmand ActiveCommand
        {
            get
            {
                return _activeCommand;
            }

            set
            {
                SetProperty(ref _activeCommand, value);
            }
        }
        public string Log
        {
            get
            {
                return _log;
            }

            set
            {
                SetProperty(ref _log, value);
            }
        }
        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                SetProperty(ref _status, value);
            }
        }
        #endregion

        private bool VerifyIsConnected()
        {
            if (!_client.Connected)
            {
                MessageBox.Show("connect first");
                return false;
            }
            return true;
        }

        private async void OnConnect(object sender, RoutedEventArgs e)
        {
            try
            {
                await _client.ConnectAsync(RemoteHost, ServerPory);
            }
            catch (SocketException ex) when (ex.ErrorCode == 0x2748)
            {
                _client.Close();
                _client = new TcpClient();
                MessageBox.Show("请重新连接");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private async void OnSendCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!VerifyIsConnected()) return;

                NetworkStream stream = _client.GetStream();
                byte[] writebuffer = Encoding.ASCII.GetBytes(GetCommand());
                await stream.WriteAsync(writebuffer, 0, writebuffer.Length);
                await stream.FlushAsync();
                byte[] readbuffer = new byte[1024];
                int read = await stream.ReadAsync(readbuffer, 0, readbuffer.Length);
                string messageRead = Encoding.ASCII.GetString(readbuffer, 0, read);
                Log += messageRead + Environment.NewLine;
                ParseMessage(messageRead);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetCommand() => $"{GetSessionHeader()}{ActiveCommand?.Name}{ActiveCommand?.Action}";

        private object GetSessionHeader()
        {
            if (string.IsNullOrEmpty(SessionId)) return string.Empty;
            return $"ID::{SessionId}::";
        }

        private void ParseMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            string[] messageColl = message.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

            Status = messageColl[0];
            SessionId = GetSessionId(messageColl);
        }
        private string GetSessionId(string[] messageColl) =>
           messageColl.Length >= 2 && messageColl[1] == "ID" ? messageColl[2] : string.Empty;


        #region INotifyPropertyChanged

        private bool SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return false;

            item = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void OnClearLog(object sender, RoutedEventArgs e)
        {
            Log = string.Empty;
        }
    }
}
