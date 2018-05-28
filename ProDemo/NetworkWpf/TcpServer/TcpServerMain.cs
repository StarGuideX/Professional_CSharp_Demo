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

        public void run()
        {
            using (var timer = new Timer(TimerSessionCleanup, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
            {
                RunServerAssync().Wait();
            )
        }



        private async Task RunServerAssync()
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

        private Task RunClientRequest(TcpClient client)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using (client)
                    {
                        using (NetworkStream stream = client.GetStream())
                        {
                            bool completed = false;
                            do
                            {
                                byte[] readBuffer = new byte[1024];
                                int read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);

                                string request = Encoding.ASCII.GetString(readBuffer, 0, read);

                                string sessionId;
                                string result;
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
                                        //reponse
                                        break;
                                    case ParseResponse.CLOSE:
                                        break;
                                    case ParseResponse.ERROR:
                                        break;
                                    case ParseResponse.TIMEOUT:
                                        break;
                                    default:
                                        break;
                                }
                            } while (true);
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            });
        }

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
                    response = _sessionManager.ParseSessionData(sessionId,requestAction);
                    break;
                case CustomProtocol.COMMANDGET:
                    response = $"{_sessionManager.GetSessionData(sessionId,requestAction)}";
                    break;
                default:
                    response = CustomProtocol.STATUSUNKNOWN;
                    break;
            }
        }

        private void TimerSessionCleanup(object o)
        {
            _sessionManager.CleanupAllSessions();
        }
    }


}
