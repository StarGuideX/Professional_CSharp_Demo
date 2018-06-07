using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkWpf.TcpServer
{
    public class TcpServerMain
    {
        private const int portNumber = 8800;
        private SessionManager _sessionManager = new SessionManager();
        private CommandActions _commandActions = new CommandActions();

        private enum ParseResponse
        {
            OK,
            CLOSE,
            ERROR,
            TIMEOUT
        }

        /// <summary>
        /// Run方法，启动一个计时器，每分钟清理一次所有的会话状态。主要功能是通过调用RunServerAsync方法来启动服务器
        /// </summary>
        public void run()
        {
            using (var timer = new Timer(TimerSessionCleanup, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)))
            {
                RunServerAsync().Wait();
            }
        }

        /// <summary>
        /// 对于TcpListener类，服务器最重要的部分在RunServerAsync方法中。
        /// TcpListener使用IP地址和端口号的构造函数实例化，在IP地址和端口号上可以访问侦听器。
        /// 调用Start()方法，侦听器开始侦听客户端连接。AcceptTcpClientAsync等待客户机连接。
        /// 一旦客户端连接，就返回TcpClient实例，允许与客户沟通。这个实例传递给RunClientRequest方法，以处理请求。
        /// </summary>
        /// <returns></returns>
        private async Task RunServerAsync()
        {
            try
            {
                var listener = new TcpListener(IPAddress.Any, portNumber);
                listener.Start();
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Task t = RunClientRequest(client);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 为了在客户端上读写，TcpClient的Getstream方法返回NetworkStream。
        /// 首先需要读取来自客户机的请求。为此，可以使用ReadAsync方法。
        /// ReadAsync方法填充一个字节数组。这个字节数组使用Encoding类转换为字符串。
        /// 收到的信息写入控制台，传递到ParseRequest辅助方法。
        /// 根据ParseRequest方法的结果，创建客户端的回应，使用WriteAsync方法返回给客户端。
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private Task RunClientRequest(TcpClient client)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using (client)
                    {
                        Console.WriteLine("与客户端已建立连接");
                        using (NetworkStream stream = client.GetStream())
                        {
                            bool completed = false;
                            do
                            {
                                byte[] readBuffer = new byte[1024];
                                int read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);

                                string request = Encoding.ASCII.GetString(readBuffer, 0, read);
                                Console.WriteLine($"已接收{request}");
                                string sessionId;
                                string result = string.Empty;
                                byte[] wiriteBuffer = null;
                                string response = string.Empty;

                                ParseResponse resp = ParseRequest(request, out sessionId, out response);
                                switch (resp)
                                {
                                    case ParseResponse.OK:
                                        string content = $"{CustomProtocol.STATUSOK}::{CustomProtocol.SESSIONID}::{sessionId}";
                                        if (!string.IsNullOrEmpty(result))
                                        {
                                            content += $"{CustomProtocol.SEPARATOR}{result}";
                                        }
                                        response = $"{CustomProtocol.STATUSOK}{CustomProtocol.SEPARATOR}{CustomProtocol.SESSIONID}{CustomProtocol.SEPARATOR}" +
                                        $"{sessionId}{CustomProtocol.SEPARATOR}{content}";
                                        break;
                                    case ParseResponse.CLOSE:
                                        response = $"{CustomProtocol.STATUSCLOSED}";
                                        completed = true;
                                        break;
                                    case ParseResponse.TIMEOUT:
                                        response = $"{CustomProtocol.STATUSTIMEOUT}";
                                        break;
                                    case ParseResponse.ERROR:
                                        response = $"{CustomProtocol.STATUSINVALID}";
                                        break;
                                    default:
                                        break;
                                }
                                wiriteBuffer = Encoding.ASCII.GetBytes(response);
                                await stream.WriteAsync(wiriteBuffer, 0, wiriteBuffer.Length);
                                await stream.FlushAsync();
                                Console.WriteLine($"返回给客户端{Encoding.ASCII.GetString(wiriteBuffer, 0, wiriteBuffer.Length)}");
                            } while (!completed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"客户端请求异常 {ex.GetType().Name}，message{ex.Message}");
                }
                Console.WriteLine("与客户端断开连接");
            });
        }

        /// <summary>
        /// ParseRequest方法解析请求，并过滤掉会话标识符。
        /// server(HELO)的第一个调用是不从客户端传递会话标识符的唯一调用，它是使用SessionManager创建的。
        /// 在第二个和后来的请求中，requesColl[0]必须包含ID，requesColl[1]必须包含会话标识符。
        /// 使用这个标识符，如果会话仍然是有效的，TouchSession方法就更新会话标识符的当前时间。
        /// 如果无效，就返回超时。对于服务的功能，调用ProcessRequest方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sessionId"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private ParseResponse ParseRequest(string request, out string sessionId, out string response)
        {
            sessionId = string.Empty;
            response = string.Empty;
            string[] requestColl = request.Split(new string[] { CustomProtocol.SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            if (requestColl[0] == CustomProtocol.COMMANDHELO)
            {
                sessionId = _sessionManager.CreateSession();
            }
            else if (requestColl[0] == CustomProtocol.SESSIONID)
            {
                sessionId = requestColl[1];

                if (!_sessionManager.TouchSession(sessionId))
                {
                    return ParseResponse.TIMEOUT;
                }

                if (requestColl[2] == CustomProtocol.COMMANDBYE)
                {
                    return ParseResponse.CLOSE;
                }

                if (requestColl.Length >= 4)
                {
                    response = ProcessRequest(requestColl);
                }
            }
            else
            {
                return ParseResponse.ERROR;
            }
            return ParseResponse.OK;
        }
        /// <summary>
        /// ProcessRequest方法包含一个switch语句，来处理不同的请求。这个方法使用CommandActions类来回应或反向传递收到的消息。为了存储和检索会话状态，使用SessionManager
        /// </summary>
        /// <param name="requestColl"></param>
        /// <returns></returns>
        private string ProcessRequest(string[] requestColl)
        {
            if (requestColl.Length < 4)
                throw new ArgumentException("requestColl 长度不合格");

            string sessionId = requestColl[1];
            string response = string.Empty;
            string requestCommand = requestColl[2];

            string requestAction = requestColl[3];

            switch (requestCommand)
            {
                case CustomProtocol.COMMANDECHO:
                    response = _commandActions.Echo(requestAction);
                    break;
                case CustomProtocol.COMMANDREV:
                    response = _commandActions.Reverse(requestAction);
                    break;
                case CustomProtocol.COMMANDSET:
                    response = _sessionManager.ParseSessionData(sessionId, requestAction);
                    break;
                case CustomProtocol.COMMANDGET:
                    response = $"{_sessionManager.GetSessionData(sessionId, requestAction)}";
                    break;
                default:
                    response = CustomProtocol.STATUSUNKNOWN;
                    break;
            }
            return response;
        }

        private void TimerSessionCleanup(object o)
        {
            _sessionManager.CleanupAllSessions();
        }
    }
}
