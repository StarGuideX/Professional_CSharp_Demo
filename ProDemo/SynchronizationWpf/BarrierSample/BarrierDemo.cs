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
            return string.Empty;
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
    }
}
