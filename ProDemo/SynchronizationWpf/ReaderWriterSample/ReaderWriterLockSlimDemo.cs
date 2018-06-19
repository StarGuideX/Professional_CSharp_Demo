using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.ReaderWriterSample
{
    public class ReaderWriterLockSlimDemo
    {
        private List<int> _items = new List<int>() { 0, 1, 2, 3, 4, 5 };
        private ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public string ReaderWriterLockSlimDemoStart()
        {
            var sb = new StringBuilder();
            var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            var tasks = new Task<string>[6];

            tasks[0] = taskFactory.StartNew(WriterMethod, 1);
            tasks[1] = taskFactory.StartNew(ReaderMethod, 1);
            tasks[3] = taskFactory.StartNew(ReaderMethod, 2);
            tasks[2] = taskFactory.StartNew(WriterMethod, 2);
            tasks[4] = taskFactory.StartNew(ReaderMethod, 3);
            tasks[5] = taskFactory.StartNew(ReaderMethod, 3);

            sb.Append(tasks[0].Result);
            sb.Append(tasks[1].Result);
            sb.Append(tasks[2].Result);
            sb.Append(tasks[3].Result);
            sb.Append(tasks[4].Result);
            sb.Append(tasks[5].Result);

            Task.WaitAll(tasks);

            return sb.ToString();
        }

        public String ReaderMethod(object reader)
        {
            var sb = new StringBuilder();
            try
            {
                _rwl.EnterReadLock();

                for (int i = 0; i < _items.Count; i++)
                {
                    sb.Append($"reader{reader},loop：{i},item：{_items[i]}\r\n");
                    Task.Delay(40).Wait();
                }
            }
            finally
            {
                _rwl.ExitReadLock();
            }

            return sb.ToString();
        }

        public String WriterMethod(object writer)
        {
            var sb = new StringBuilder();
            try
            {
                while (!_rwl.TryEnterWriteLock(50))
                {
                    sb.Append($"writer{writer}等待写入锁\r\n");
                    sb.Append($"读取模式锁定状态的独有线程的总数：{_rwl.CurrentReadCount}\r\n");
                }
                sb.Append($"writer{writer}需要锁\r\n");

                for (int i = 0; i < _items.Count; i++)
                {
                    _items[i]++;
                    Task.Delay(40).Wait();
                }

                sb.Append($"writer{writer}已完成\r\n");
            }
            finally
            {
                _rwl.EnterWriteLock();
            }

            return sb.ToString();
        }
    }


}
