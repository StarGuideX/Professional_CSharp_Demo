using ParallelConsole.CancellationSamples;
using ParallelConsole.ParallelSamples;
using ParallelConsole.SimpleDataFlowSamples;
using ParallelConsole.TaskSamples;
using System;

namespace ParallelConsole
{
    class Program
    {
        private static ParallelDemo parallelDemo = new ParallelDemo();
        private static TaskDemo taskDemo = new TaskDemo();
        private static CancellationDemo cancellationDemo = new CancellationDemo();
        private static SimpleDataFlowDemo simpleDataFlowDemo = new SimpleDataFlowDemo();
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
            Console.WriteLine("11-Task-连续的任务-Task.ContinueWith)");
            Console.WriteLine("12-Task-任务层次结构-父子任务)");
            Console.WriteLine("13-CancellationTokenSource-Parallel.For()方法的取消)");
            Console.WriteLine("14-CancellationTokenSource-任务的取消)");
            Console.WriteLine("15-ActionBlock");
            Console.WriteLine("16-BufferBlock");
            Console.WriteLine("17-连接块");
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
                case "11":
                    taskDemo.ContinuationTasks();
                    break;
                case "12":
                    taskDemo.ParentAndChild();
                    break;
                case "13":
                    cancellationDemo.CancelParallelFor();
                    break;
                case "14":
                    cancellationDemo.CancelTask();
                    break;
                case "15":
                    simpleDataFlowDemo.SimpleDataFlowDemoUsingActionBlock();
                    break;
                case "16":
                    simpleDataFlowDemo.SimpleDataFlowDemoUsingBufferBlock();
                    break;
                case "17":
                    simpleDataFlowDemo.ContectBlockDemoStart();
                    break;
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }
    }
}
