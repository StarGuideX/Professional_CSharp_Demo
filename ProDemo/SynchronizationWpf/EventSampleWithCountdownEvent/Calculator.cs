using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.EventSampleWithCountdownEvent
{
    public class Calculator
    {
        private CountdownEvent _cEvent;

        public int Result { get; private set; }

        public Calculator(CountdownEvent cv)
        {
            this._cEvent = cv;
        }

        public String Calculation(int x, int y)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Task{Task.CurrentId}开始计算\r\n");
            Task.Delay(new Random().Next(3000)).Wait();
            Result = x + y;

            //信号完成事件
            sb.Append($"Task{Task.CurrentId}已经准备好\r\n");
            _cEvent.Signal();

            return sb.ToString();
        }
    }
}
