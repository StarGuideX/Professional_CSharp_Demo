using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.SemaphoreSample
{
    public class SemaphoreDemo
    {
        public string SemaphoreStart() {
            int taskCount = 6;
            int SemaphoreCount = 3;
            var Semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
            StringBuilder sb = new StringBuilder();

            var tasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() => sb.Append(TaskMain(Semaphore)));
            }
            Task.WaitAll(tasks);
            sb.Append("所有任务已完成");
            return sb.ToString();
        }

        private string TaskMain(SemaphoreSlim semaphore)
        {
            StringBuilder sb = new StringBuilder();
            bool isCompleted = false;
            while (!isCompleted)
            {
                if (semaphore.Wait(600))
                {
                    try
                    {
                        sb.Append($"Task{Task.CurrentId}锁住了semaphore\r\n");
                        Task.Delay(2000).Wait();
                    }
                    finally
                    {
                        sb.Append($"Task{Task.CurrentId}释放了semaphore\r\n");
                        semaphore.Release();
                        isCompleted = true;
                    }
                }
                else {
                    sb.Append($"Task{Task.CurrentId}超时，请等待\r\n");
                }
            }

            return sb.ToString();
        }
    }
}
