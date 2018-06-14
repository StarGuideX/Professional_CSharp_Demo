﻿using System;
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

        public void ThreadUsingThreadPool()
        {
            var tf = new TaskFactory();
            Task t1 = tf.StartNew(TaskMethod, "using a task factory");
            Task t2 = Task.Factory.StartNew(TaskMethod, "factory via a task");
            var t3 = new Task(TaskMethod, "using a task constructor and Start");
            t3.Start();
            Task t4 = Task.Run(() => TaskMethod("using the Run menthod"));
        }

        public void RunSynchronousTask()
        {
            TaskMethod("Just the main thread");
            var t1 = new Task(TaskMethod, "run sync");
            t1.RunSynchronously();
        }

        public void LongRunningTask()
        {
            var t1 = new Task(TaskMethod, "long running", TaskCreationOptions.LongRunning);
            t1.Start();
        }

        private Tuple<int, int> TaskWithResult(object division)
        {
            Tuple<int, int> div = (Tuple<int, int>)division;
            int result = div.Item1 / div.Item2;
            int remider = div.Item1 % div.Item2;
            Console.WriteLine("task creates  a result...");

            return Tuple.Create(result, remider);
        }

        public void TaskWithResultDemo()
        {
            var t1 = new Task<Tuple<int, int>>(TaskWithResult, Tuple.Create(8, 3));
            t1.Start();
            Console.WriteLine(t1.Result);
            t1.Wait();
            Console.WriteLine($"result from task:{t1.Result.Item1}{t1.Result.Item2}");
        }
    }
}