using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelConsole.ParallelSamples
{
    public class ParallelDemo
    {
        /// <summary>
        /// 线程和任务标识符
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public void Log(string prefix)
        {
            Console.WriteLine($"{prefix} 任务: {Task.CurrentId}, 线程: {Thread.CurrentThread.ManagedThreadId}");
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
        /// Parallel.For异步
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
        /// <summary>
        /// Parallel.For()的初始化,这个方法完美地累加了大量数据集合的结果
        /// </summary>
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

        /// <summary>
        /// Parallel.ForEach的简单示例
        /// </summary>
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

        /// <summary>
        /// 通过lnvoke调用多个方法
        /// </summary>
        public void ParallelInvoke()
        {
            Parallel.Invoke(Foo, Bar, Bar);
        }

        private void Foo()
        {
            Console.WriteLine("Foo");
        }
        private void Bar()
        {
            Console.WriteLine("Bar");
        }

    }
}
