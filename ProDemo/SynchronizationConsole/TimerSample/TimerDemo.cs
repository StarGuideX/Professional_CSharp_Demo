using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationConsole.TimerSample
{
    public class TimerDemo
    {
        private static void ThreadingTimer() {
            using (var t1 = new Timer(TimeAction, null, TimeSpan.FromSeconds(2),TimeSpan.FromSeconds(3)))
            {
                Task.Delay(1500).Wait();
            }
        }

        private static void TimeAction(object o)
        {
            Console.WriteLine($"System.Threading.Timer {DateTime.Now:T}");
        }
    }
}
