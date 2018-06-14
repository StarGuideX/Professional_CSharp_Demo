using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.SynchronizationSample
{
    public class SynchronizationSampleMain
    {
        public string DealLockSample()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var state = new ShareState();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(state).DoTheJob());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{state.State}\r\n");
            return sb.ToString();
        }
    }
}
