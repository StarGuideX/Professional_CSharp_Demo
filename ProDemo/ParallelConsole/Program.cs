using ParallelConsole.ParallelSamples;
using ParallelConsole.TaskSamples;
using System;

namespace ParallelConsole
{
    class Program
    {
        private static ParallelDemo parallelDemo = new ParallelDemo();
        private static TaskDemo taskDemo = new TaskDemo();
        
        static void Main(string[] args)
        {
            while (true)
            {
                PickDemo();
            }
        }

        private static void PickDemo()
        {
            Console.WriteLine("**********选项**********");
            Console.WriteLine("1-Parallel.For-使用Task.Delay(10).Wait()控制");
            Console.WriteLine("2-Parallel.For-使用await关键字和Task.Delay()");
            Console.WriteLine("3-Parallel.For-提前停止Parallel.For");
            Console.WriteLine("4-Parallel.For-Parallel.For的初始化-这个方法完美地累加了大量数据集合的结果");
            Console.WriteLine("5-Parallel.ForEach-简单示例");
            Console.WriteLine("6-Parallel.ForEach-Invoke()-通过lnvoke调用多个方法");
            Console.WriteLine("7-Task-启动任务-使用线程池的任务");
            Console.WriteLine("8-Task-启动任务-同步任务");
            Console.WriteLine("9-Task-启动任务-使用单独线程的任务（TaskCreation0ptions）");
            Console.WriteLine("10-Task-任务的结果-Task(Func<object, TResult> function, object state)");
            Console.WriteLine("**********选项**********");

            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    parallelDemo.ParallelFor();
                    break;
                case "2":
                    parallelDemo.ParallelForWithAsync();
                    break;
                case "3":
                    parallelDemo.StopParallelForEarly();
                    break;
                case "4":
                    parallelDemo.ParallelForWithInit();
                    break;
                case "5":
                    parallelDemo.ParallForEach();
                    break;
                case "6":
                    parallelDemo.ParallelInvoke();
                    break;
                case "7":
                    taskDemo.TasksUsingThreadPool();
                    break;
                case "8":
                    taskDemo.RunSynchronousTask();
                    break;
                case "9":
                    taskDemo.LongRunningTask();
                    break;
                case "10":
                    taskDemo.TaskWithResultDemo();
                    break;
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }
    }
}
