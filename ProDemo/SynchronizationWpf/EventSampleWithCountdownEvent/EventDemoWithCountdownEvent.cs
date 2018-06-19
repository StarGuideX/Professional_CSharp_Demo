using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.EventSampleWithCountdownEvent
{
    public class EventDemoWithCountdownEvent
    {
        /// <summary>
        /// 简化的EventDemo，使它只需要一个等待。如果不像EventDemo那样单独处理结果，这个版本就可以了。
        /// </summary>
        public string EventDemoWithCountdownEventStart()
        {
            const int taskCount = 4;

            var cEvent = new CountdownEvent(taskCount);
            var calcs = new Calculator[taskCount];

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < taskCount; i++)
            {
                calcs[i] = new Calculator(cEvent);

                int j = i;

                Task.Run(() => sb.Append(calcs[j].Calculation(j + 1, j + 3)));
            }

            cEvent.Wait();
            sb.Append($"所有已完成\r\n");

            for (int i = 0; i < taskCount; i++)
            {
                sb.Append($"任务{i}，结果：{calcs[i].Result}\r\n");
            }
            return sb.ToString();
        }
    }
}
