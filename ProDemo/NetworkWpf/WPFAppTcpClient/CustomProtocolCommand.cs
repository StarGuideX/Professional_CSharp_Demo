using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.WPFAppTcpClient
{
public class CustomProtocolCommand
{
    /// <summary>
    /// 命令名称
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 用户输入的、与命令一起发送的数据
    /// </summary>
    public string Action { get; set; }
    public override string ToString() => Name;

    public CustomProtocolCommand(string name) : this(name, null)
    {

    }

    public CustomProtocolCommand(string name, string action)
    {
        Name = name;
        Action = action;
    }

    public class CustomProtocolCommands : IEnumerable<CustomProtocolCommand>
    {
        private readonly List<CustomProtocolCommand> _commands =
            new List<CustomProtocolCommand>();

        public CustomProtocolCommands()
        {
            string[] commands = { "HELO", "BYE", "SET", "GET", "ECO", "REV" };
            foreach (var command in commands)
            {
                _commands.Add(new CustomProtocolCommand(command));
            }
            _commands.Single(c => c.Name == "HELO").Action = "v1.0";
        }

        public IEnumerator<CustomProtocolCommand> GetEnumerator() => _commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _commands.GetEnumerator();
    }
}
}
