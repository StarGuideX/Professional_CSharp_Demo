using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.BarrierSample
{
    public class BarrierDemo
    {
        public string BarrierDemoStart()
        {
            var sb = new StringBuilder();

            const int numberTasks = 2;
            const int partitionSize = 1000000;
            const int loops = 5;

            var taskResults = new Dictionary<int, int[][]>();
            var data = new List<string>[loops];
            for (int i = 0; i < loops; i++)
            {
                data[i] = new List<string>(FillData(partitionSize * numberTasks));
            }

            var barrier = new Barrier(numberTasks + 1);
            sb.Append(LogBarrierInformation("在barrier中初始化Participant", barrier));

            for (int i = 0; i < numberTasks; i++)
            {
                barrier.AddParticipant();

                int jobNumber = 1;
                taskResults.Add(i, new int[loops][]);

                for (int loop = 0; loop < loops; loop++)
                {
                    taskResults[i][loop] = new int[26];
                }
                sb.Append($"BarrierDemoStart - 开始Task job{jobNumber}\r\n");
                Task.Run(() =>
                {
                    sb.Append(CalculationInTask(jobNumber, partitionSize, barrier, data, loops, taskResults[jobNumber]));
                });
            }

            for (int loop = 0; loop < 5; loop++)
            {
                sb.Append(LogBarrierInformation("BarrierDemoStart task,开始发信号和等待", barrier));
                barrier.SignalAndWait();
                sb.Append(LogBarrierInformation("BarrierDemoStart task,等待完成", barrier));

                int[][] resultCollection1 = taskResults[0];
                int[][] resultCollection2 = taskResults[1];

                var resultCollection = resultCollection1[loop].Zip(resultCollection2[loop], (c1, c2) => c1 + c2);

                char ch = 'a';
                int sum = 0;

                foreach (var x in resultCollection)
                {
                    sb.Append($"{ch++},count：{x}");
                    sum += x;
                }

                sb.Append(LogBarrierInformation($"BarrierDemoStart task已完成loop{loop}，sum：{sum}", barrier));
            }
            sb.Append("已完成所有迭代");
            return sb.ToString();
        }

        public IEnumerable<string> FillData(int size)
        {
            var r = new Random();
            return Enumerable.Range(0, size).Select(x => GetString(r));
        }

        private string GetString(Random r)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                sb.Append((char)(r.Next(26) + 97));
            }

            return sb.ToString();
        }

        private string LogBarrierInformation(string info, Barrier barrier)
        {
            return $"Task{Task.CurrentId}：{info} | 屏障中参与者总数：{barrier.ParticipantCount} | 屏障中未在当前阶段发出信号的参与者数{barrier.ParticipantsRemaining} | 屏障的当前阶段的编号：{barrier.CurrentPhaseNumber}\r\n";
        }

        private string CalculationInTask(int jobNumber, int partitionSize, Barrier barrier, IList<string>[] coll, int loops, int[][] results)
        {
            var sb = new StringBuilder();
            sb.Append(LogBarrierInformation("CalculationInTask 开始", barrier));

            for (int i = 0; i < loops; i++)
            {
                var data = new List<string>(coll[i]);

                int start = jobNumber * partitionSize;
                int end = start + partitionSize;
                sb.Append($"Task{Task.CurrentId}在loop{i}：从{start}到{end}分割\r\n");

                for (int j = start; j < end; j++)
                {
                    char c = data[j][0];
                    results[i][c - 97]++;
                }

                sb.Append($"计算已经完成，Task{Task.CurrentId}在loop{i}。{results[i][0]}次字符a，{results[i][25]}次字符z\r\n");
                sb.Append(LogBarrierInformation("发送信号并等待所有任务", barrier));
                barrier.SignalAndWait();
                sb.Append(LogBarrierInformation("等待完成", barrier));
            }
            barrier.RemoveParticipant();
            sb.Append(LogBarrierInformation("完成任务，删除Participant", barrier));

            return sb.ToString();
        }
    }
}
