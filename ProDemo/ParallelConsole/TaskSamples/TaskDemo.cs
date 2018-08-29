using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelConsole.TaskSamples
{
    public class TaskDemo
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

        /// <summary>
        /// 使用线程池的任务
        /// </summary>
        public void TasksUsingThreadPool()
        {
            var tf = new TaskFactory();
            Task t1 = tf.StartNew(TaskMethod, "using a task factory");
            Task t2 = Task.Factory.StartNew(TaskMethod, "factory via a task");
            var t3 = new Task(TaskMethod, "using a task constructor and Start");
            t3.Start();
            Task t4 = Task.Run(() => TaskMethod("using the Run menthod"));
        }

        /// <summary>
        /// 同步任务
        /// </summary>
        public void RunSynchronousTask()
        {
            TaskMethod("Just the main thread");
            var t1 = new Task(TaskMethod, "run sync");
            t1.RunSynchronously();
        }

        /// <summary>
        /// 使用单独线程的任务——TaskCreationOptions
        /// </summary>
        public void LongRunningTask()
        {
            var t1 = new Task(TaskMethod, "long running", TaskCreationOptions.LongRunning);
            t1.Start();
        }

        /// <summary>
        /// 任务的结果
        /// </summary>
        public void TaskWithResultDemo()
        {
            var t1 = new Task<Tuple<int, int>>(TaskWithResult, Tuple.Create(8, 3));
            t1.Start();
            Console.WriteLine(t1.Result);
            t1.Wait();
            Console.WriteLine($"result from task:{t1.Result.Item1}{t1.Result.Item2}");
        }

        private Tuple<int, int> TaskWithResult(object division)
        {
            Tuple<int, int> div = (Tuple<int, int>)division;
            int result = div.Item1 / div.Item2;
            int remider = div.Item1 % div.Item2;
            Console.WriteLine("task creates  a result...");

            return Tuple.Create(result, remider);
        }

        public void ContinuationTasks()
        {
            Task t1 = new Task(DoOnFirst);
            Task t2 = t1.ContinueWith(DoOnSecond);
            Task t3 = t1.ContinueWith(DoOnSecond);
            Task t4 = t2.ContinueWith(DoOnSecond);
            t1.Start();
        }


        private void DoOnFirst()
        {
            Console.WriteLine($"做一些任务{Task.CurrentId}");
            Task.Delay(3000).Wait();
        }

        private void DoOnSecond(Task t)
        {
            Console.WriteLine($"任务{t.Id}已完成");
            Console.WriteLine($"本次任务ID：{Task.CurrentId}");
            Console.WriteLine("做一些清理工作");
            Task.Delay(3000).Wait();
        }

        public void ParentAndChild()
        {
            var parent = new Task(ParentTask);
            parent.Start();
            Task.Delay(2000).Wait();
            Console.WriteLine(parent.Status);
            Task.Delay(4000).Wait();
            Console.WriteLine(parent.Status);
        }

        private void ParentTask()
        {
            Console.WriteLine($"Task id{Task.CurrentId}");
            var child = new Task(ChildTask);
            child.Start();
            Task.Delay(1000).Wait();
            Console.WriteLine("父任务开始子任务");
        }

        private void ChildTask()
        {
            Console.WriteLine("子任务");
            Task.Delay(5000).Wait();
            Console.WriteLine("子任务已完成");
        }
    }
}