using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.TcpServer
{
    public class CommandActions
    {
        internal string Echo(string requestAction) => requestAction;

        internal string Reverse(string requestAction) => string.Join("", requestAction);
    }
}
