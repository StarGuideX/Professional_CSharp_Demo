using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelConsole.CancellationSamples
{
    public class CancellationDemo
    {
        /// <summary>
        ///  Parallel.For()方法的取消
        /// </summary>
        public void CancelParallelFor()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("*** token cancelled"));
            //500毫秒后取消标记
            cts.CancelAfter(500);
            try
            {
                ParallelLoopResult result =
                    Parallel.For(0, 100, new ParallelOptions
                    {
                        CancellationToken = cts.Token
                    },
                    x =>
                    {
                        Console.WriteLine($"Loop{x}开始");
                        int sum = 0;
                        for (int i = 0; i < 100; i++)
                        {
                            Task.Delay(2).Wait();
                            sum += i;
                        }
                        Console.WriteLine($"Loop{x}完成");
                    });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 任务的取消
        /// </summary>
        public void CancelTask()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("*** Task 取消"));
            cts.CancelAfter(500);
            Task t1 = Task.Run(() => {
                Console.WriteLine("进入Task");
                for (int i = 0; i < 20; i++)
                {
                    Task.Delay(100).Wait();
                    CancellationToken token = cts.Token;
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("请求取消操作，任务取消");
                        token.ThrowIfCancellationRequested();
                        break;
                    }
                    Console.WriteLine("在Task中的循环中");
                }
                Console.WriteLine("Task已完成，且没有被取消");
            }, cts.Token);
            try
            {
                t1.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"异常：{ex.GetType().Name},{ex.Message}");
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine($"内部异常：{ex.InnerException.GetType()},{ex.InnerException.Message}");
                }
            }
        }
    }
}
