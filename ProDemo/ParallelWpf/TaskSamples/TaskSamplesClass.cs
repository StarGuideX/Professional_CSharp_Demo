using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelWpf.TaskSamples
{
    public class TaskSamplesClass
    {
        public void TaskMethod(object o)
        {
            Log(o?.ToString());
        }
        private object s_logLock = new object();
        public void Log(string title)
        {
            lock (s_logLock)
            {
                Console.WriteLine(title);
                Console.WriteLine($"Task id:{Task.CurrentId?.ToString() ?? "no task"}," + $"thread:{Thread.CurrentThread.ManagedThreadId}");
#if (!DNXCORE)
                Console.WriteLine($"is pooled thread:{Thread.CurrentThread.IsThreadPoolThread}");
#endif
                Console.WriteLine($"is background thread:{Thread.CurrentThread.IsBackground}");
                Console.WriteLine();
            }
        }
    }
}
