using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelWpf.ParallelSamples
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ParallelForWithAsync()
        {
            ParallelLoopResult res =
                Parallel.For(0, 10, async i =>
                {
                    Log($"S {i}");
                    await Task.Delay(10);
                    Log($"E {i}");
                });
            Console.WriteLine($"Is Completed：{res.IsCompleted}");
        }
        /// <summary>
        /// 停止Parallel.For
        /// </summary>
        public void StopParallelForEarly()
        {
            ParallelLoopResult result = Parallel.For(10, 40, (int i, ParallelLoopState pls) =>
            {
                Log($"S {i}");
                if (i > 12)
                {
                    pls.Break();
                    Log($"break now... {i}");
                }
                Task.Delay(10).Wait();
                Log($"E {i}");

            });

            Console.WriteLine($"Is Completed：{result.IsCompleted}");
            Console.WriteLine($"lowest break iteration:{result.LowestBreakIteration}");
        }

        public void ParallelForWithInit()
        {
            ParallelLoopResult result = Parallel.For<string>(0, 10, () =>
              {
                  //每个线程调用一次
                  Log($"init thread");
                  return $"{Thread.CurrentThread.ManagedThreadId}";
              },
              (i, pls, strl) =>
              {
                  //每个成员都会调用
                  Log($"body i {i} strl {strl}");
                  Task.Delay(10).Wait();
                  return $"i {i}";
              },
              (strl) =>
              {
                  //每个线程最后执行的操作
                  Log($"finally {strl}");
              });
        }

        public void ParallForEach()
        {
            string[] data = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            ParallelLoopResult result = Parallel.ForEach<string>(data, s =>
            {
                Console.WriteLine(s);
            });
            //Parallel.ForEach<string>(data, (s, pls, l) => {
            //    Console.WriteLine($"{s} {l}");
            //});
        }
    }
}
