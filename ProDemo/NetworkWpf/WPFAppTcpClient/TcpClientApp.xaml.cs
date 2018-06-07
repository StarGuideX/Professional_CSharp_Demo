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
    private readonly CustomProtocolCommand.CustomProtocolCommands _commands =
        new CustomProtocolCommand.CustomProtocolCommands();

    public TcpClientApp()
    {
        InitializeComponent();
    }

    #region Properties
        private string _remoteHost = "localhost";
        private int _serverPort = 8800;
        private string _sessionId;
        private CustomProtocolCommand _activeCommand;
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
        public int ServerPort
        {
            get
            {
                return _serverPort;
            }

            set
            {
                SetProperty(ref _serverPort, value);
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
        public CustomProtocolCommand ActiveCommand
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
    /// <summary>
    /// 当用户单击Connect按钮时，调用方法OnConnect。
    /// 建立到TCP服务器的连接，调用TcpClient类的ConnectAsync方法。
    /// 如果连接处于失效模式，且再次调用OnConnect方法，就抛出一个SocketException异常，
    /// 其中ErrorCode设置为0x2748。这里使用C# 6异常过滤器来处理SocketException，
    /// 创建一个新的TcpClient,所以再次调用Onconnect可能会成功：
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnConnect(object sender, RoutedEventArgs e)
    {
        try
        {
            await _client.ConnectAsync(RemoteHost, ServerPort);
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

    public IEnumerable<CustomProtocolCommand> Commands => _commands;
    /// <summary>
    /// 请求发送到TCP服务器是由OnsendCommand方法处理的。
    /// 这里的代码非常类似于服务器上的收发代码。
    /// Getstream方法返回一个NetworkStream，这用于把（writebuffer）数据写入服务器，
    /// 从服务器中读取ReadAsync数据：
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <summary>
    /// 为了建立可以发送到服务器的数据，从OnSendCommand内部调用GetCommand方法。
    /// GetCommand方法又调用方法GetSessionHeader来建立会话标识符，
    /// 然后提取ActiveCommand属性（其类型是CustomProtocolCommand)，其中包含选中的命令名称和输入的数据
    /// </summary>
    /// <returns></returns>
    private string GetCommand() => $"{GetSessionHeader()}{ActiveCommand?.Name}{ActiveCommand?.Action}";
    
    /// <summary>
    /// 建立会话标识符
    /// </summary>
    /// <returns></returns>
    private object GetSessionHeader()
    {
        if (string.IsNullOrEmpty(SessionId)) return string.Empty;
        return $"ID::{SessionId}::";
    }

    /// <summary>
    /// 从服务器接受数据后使用ParseMessage方法。这个方法拆分消息以设置Status和SessionId属性
    /// </summary>
    /// <param name="message"></param>
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
