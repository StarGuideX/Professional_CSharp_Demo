using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.TcpServer
{
    public static class CustomProtocol
    {
        public const string SESSIONID = "ID";
        /// <summary>
        /// 命令HELO，启动连接后，这个命令需要发送。其他命令将不被接受
        /// </summary>
        public const string COMMANDHELO = "HELO";
        /// <summary>
        /// 命令ECHO，ECHO命令向调用者返回消息
        /// </summary>
        public const string COMMANDECHO = "ECO";
        /// <summary>
        /// 命令REV，REV命令保留消息并返回给调用者
        /// </summary>
        public const string COMMANDREV = "REV";
        /// <summary>
        /// 命令BYE，BYE命令关闭连接
        /// </summary>
        public const string COMMANDBYE = "BYE";
        /// <summary>
        /// 命令SET，SET命令设置服务器端状态，可以用GET命令检索
        /// </summary>
        public const string COMMANDSET = "SET";
        /// <summary>
        /// 命令GET，SET命令设置服务器端状态，可以用GET命令检索
        /// </summary>
        public const string COMMANDGET = "GET";
        /// <summary>
        /// 状态：OK
        /// </summary>
        public const string STATUSOK = "OK";
        /// <summary>
        /// 状态：关闭
        /// </summary>
        public const string STATUSCLOSED = "CLOSED";
        /// <summary>
        /// 状态：无效
        /// </summary>
        public const string STATUSINVALID = "INV";
        /// <summary>
        /// 状态：未知
        /// </summary>
        public const string STATUSUNKNOWN = "UNK";
        /// <summary>
        /// 状态：未找到
        /// </summary>
        public const string STATUSNOTFOUND = "NOTFOUND";
        /// <summary>
        /// 状态：超时
        /// </summary>
        public const string STATUSTIMEOUT = "TIMOUT";
        /// <summary>
        /// 分割字符::
        /// </summary>
        public const string SEPARATOR = "::";
        /// <summary>
        /// 超时时间
        /// </summary>
        public static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(2);
    }
}
