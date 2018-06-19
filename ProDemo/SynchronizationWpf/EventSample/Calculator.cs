using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.EventSample
{
    /// <summary>
    /// Calculation()方法，是任务的入口点。
    /// 在这个方法中，该任务接收用于计算的输入数据，将结果写入变量result，该变量可以通过Result属性来访问。
    /// 只要完成了计算（在随机的一段时间过后），就调用ManualResetEventSlim类的Set方法，向事件发信号。
    /// </summary>
    public class Calculator
    {
        private ManualResetEventSlim _mEvent;

        public int Result { get; private set; }

        public Calculator(ManualResetEventSlim ev)
        {
            this._mEvent = ev;
        }

        public String Calculation(int x, int y)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Task{Task.CurrentId}开始计算\r\n");
            Task.Delay(new Random().Next(3000)).Wait();
            Result = x + y;

            //信号完成事件
            sb.Append($"Task{Task.CurrentId}已经准备好\r\n");
            _mEvent.Set();

            return sb.ToString();
        }
    }
}
