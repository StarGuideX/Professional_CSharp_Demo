using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelConsole.ParallelSamples
{
    public class ParallelSampleClass
    {
        /// <summary>
        /// 线程和任务标识符
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public void Log(string prefix)
        {
            Console.WriteLine($"{prefix} task: {Task.CurrentId}, thread: {Thread.CurrentThread.ManagedThreadId}");
        }
        /// <summary>
        /// Parallel.For
        /// </summary>
        /// <returns></returns>
        public void ParallelFor()
        {
            ParallelLoopResult res =
                Parallel.For(0, 10, i =>
                {
                    Log($"S {i}");
                    Task.Delay(10).Wait();
                    Log($"E {i}");
                });
            Console.WriteLine($"Is Completed：{res.IsCompleted}");
        }

        public string ParallelForWithAsync()
        {
            StringBuilder sb = new StringBuilder();
            ParallelLoopResult res =
                Parallel.For(0, 10, async i =>
                {
                    Log($"S {i}");
                    await Task.Delay(10);
                    Log($"E {i}");
                });
            Console.WriteLine($"Is Completed：{res.IsCompleted}");
            return sb.ToString();
        }
    }
}
