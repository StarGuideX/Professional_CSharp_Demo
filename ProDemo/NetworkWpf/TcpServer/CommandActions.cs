using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.TcpServer
{

    /// <summary>
    /// CommandActions类定义了简单的方法Echo和Reverse，返回操作字符串，或返回反向发送的字符串
    /// </summary>
    public class CommandActions
    {
        internal string Echo(string requestAction) => requestAction;

        internal string Reverse(string requestAction) => string.Join("", requestAction);
    }
}
