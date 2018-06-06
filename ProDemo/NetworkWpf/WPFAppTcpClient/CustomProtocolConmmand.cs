using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.WPFAppTcpClient
{
    public class CustomProtocolConmmand
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

        public CustomProtocolConmmand(string name) : this(name, null)
        {

        }

        public CustomProtocolConmmand(string name, string action)
        {
            Name = name;
            Action = action;
        }

        public class CustomProtocolConmmands : IEnumerable<CustomProtocolConmmand>
        {
            private readonly List<CustomProtocolConmmand> _commands =
                new List<CustomProtocolConmmand>();

            public CustomProtocolConmmands()
            {
                string[] commands = { "HELO", "BYE", "SET", "GET", "ECO", "REV" };
                foreach (var command in commands)
                {
                    _commands.Add(new CustomProtocolConmmand(command));
                }
                _commands.Single(c => c.Name == "HELO").Action = "v1.0";
            }

            public IEnumerator<CustomProtocolConmmand> GetEnumerator() => _commands.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _commands.GetEnumerator();
        }

    }
}
