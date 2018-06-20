using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationConsole
{
public static class BarrierDemo
{
    /// <summary>
    /// BarrierDemoStart()方法创建一个Barrier实例。在构造函数中，可以指定参与者的数量。
    /// 在该示例中，这个数量是3(numberTasks+1)，因为该示例创建了两个任务，BarrierDemoStart()方法本身也是一个参与者。
    /// 使用Task.Run创建两个任务，把遍历集合的任务分为两个部分。
    /// 启动该任务后，使用SignalAndWait()方法，BarrierDemoStart()方法在完成时发出信号，
    /// 并等待所有其他参与者或者发出完成的信号，或者从Barrier类中删除它们。
    /// 一旦所有的参与者都准备好，就提取任务的结果，并使用Zip()扩展方法把它们合并起来。接着进行下一次迭代，等待任务的下一个结果。
    /// </summary>
    public static void BarrierDemoStart()
        {
            const int numberTasks = 2;
            const int partitionSize = 1000000;
            const int loops = 5;
            var taskResults = new Dictionary<int, int[][]>();
            var data = new List<string>[loops];
            for (int i = 0; i < loops; i++)
            {
                data[i] = new List<string>(FillData(partitionSize * numberTasks));
            }

            var barrier = new Barrier(1);
            LogBarrierInformation("在barrier中初始化Participant", barrier);

            for (int i = 0; i < numberTasks; i++)
            {
                barrier.AddParticipant();

                int jobNumber = i;
                taskResults.Add(i, new int[loops][]);
                for (int loop = 0; loop < loops; loop++)
                {
                    taskResults[i][loop] = new int[26];
                }
                Console.WriteLine($"BarrierDemoStart - 开始Task job{jobNumber}");
                Task.Run(() => CalculationInTask(jobNumber, partitionSize, barrier, data, loops, taskResults[jobNumber]));
            }

            for (int loop = 0; loop < 5; loop++)
            {
                LogBarrierInformation("BarrierDemoStart task,开始发信号和等待", barrier);
                barrier.SignalAndWait();
                LogBarrierInformation("BarrierDemoStart task,等待完成", barrier);
                //                var resultCollection = tasks[0].Result.Zip(tasks[1].Result, (c1, c2) => c1 + c2);
                int[][] resultCollection1 = taskResults[0];
                int[][] resultCollection2 = taskResults[1];
                var resultCollection = resultCollection1[loop].Zip(resultCollection2[loop], (c1, c2) => c1 + c2);

                char ch = 'a';
                int sum = 0;
                foreach (var x in resultCollection)
                {
                    Console.WriteLine($"{ch++},count：{x}");
                    sum += x;
                }

                LogBarrierInformation($"BarrierDemoStart task已完成loop{loop}，sum：{sum}", barrier);
            }

            Console.WriteLine("已完成所有迭代");
            Console.ReadLine();
        }

    /// <summary>
    /// 创建一个集合，并用随机字符串填充它。
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static IEnumerable<string> FillData(int size)
    {
        var r = new Random();
        return Enumerable.Range(0, size).Select(x => GetString(r));
    }

    private static string GetString(Random r)
        {
            var sb = new StringBuilder(6);
            for (int i = 0; i < 6; i++)
            {
                sb.Append((char)(r.Next(26) + 97));
            }
            return sb.ToString();
        }

    /// <summary>
    /// 义一个辅助方法，来显示Barrier的信息：
    /// </summary>
    /// <param name="info"></param>
    /// <param name="barrier"></param>
    private static void LogBarrierInformation(string info, Barrier barrier)
        {
            Console.WriteLine($"Task{Task.CurrentId}：{info} | 屏障中参与者总数：{barrier.ParticipantCount} | 屏障中未在当前阶段发出信号的参与者数{barrier.ParticipantsRemaining} | 屏障的当前阶段的编号：{barrier.CurrentPhaseNumber}");
        }

    /// <summary>
    /// 定义了任务执行的作业。通过参数接收一个包含4项的元组。
    /// 第3个参数是对Barrier实例的引用。用于计算的数据是数组IList<string>。
    /// 最后一个参数是int锯齿数组，用于在任务执行过程中写出结果。
    /// 任务把处理放在一个循环中。每一次循环中，都处理化IList<string>[] 的数组元素。每个循环完成
    /// 后，任务通过调用SignalAndWait方法，发出做好了准备的信号，并等待，直到所有的其他任务也准备好处理为止。
    /// 这个循环会继续执行，直到任务完全完成为止。
    /// 接着，任务就会使用RemoveParticipant()方法从Barrier类中删除它自己。
    /// </summary>
    /// <param name="jobNumber"></param>
    /// <param name="partitionSize"></param>
    /// <param name="barrier"></param>
    /// <param name="coll"></param>
    /// <param name="loops"></param>
    /// <param name="results"></param>
    private static void CalculationInTask(int jobNumber, int partitionSize, Barrier barrier, IList<string>[] coll, int loops, int[][] results)
        {
            LogBarrierInformation("CalculationInTask 开始", barrier);

            for (int i = 0; i < loops; i++)
            {
                var data = new List<string>(coll[i]);

                int start = jobNumber * partitionSize;
                int end = start + partitionSize;
                Console.WriteLine($"Task{Task.CurrentId}在loop{i}：从{start}到{end}分割");

                for (int j = start; j < end; j++)
                {
                    char c = data[j][0];
                    results[i][c - 97]++;
                }

                Console.WriteLine($"计算已经完成，Task{Task.CurrentId}在loop{i}。{results[i][0]}次字符a，{results[i][25]}次字符z");

                LogBarrierInformation("发送信号并等待所有任务", barrier);
                barrier.SignalAndWait();
                LogBarrierInformation("等待完成", barrier);
            }

            barrier.RemoveParticipant();
            LogBarrierInformation("完成任务，删除Participant", barrier);
        }
}
}