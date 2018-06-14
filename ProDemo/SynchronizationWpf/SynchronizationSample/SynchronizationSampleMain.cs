using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.SynchronizationSample
{
    public class SynchronizationSampleMain
    {
        /// <summary>
        /// ShareState和Job都不锁定
        /// </summary>
        public string DoTheJobByNoLockAll()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var state = new ShareState();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(state).DoTheJobByNoLockAll());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{state.State}\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// ShareState，不锁定
        /// Job中方法DoTheJobByJobMethodLock，锁定
        /// </summary>
        public string DoTheJobByJobMethodLock()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var state = new ShareState();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(state).DoTheJobByJobMethodLock());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{state.State}\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// ShareStatePropertyLock中的属性，锁定
        /// Job方法DoTheJobByShareStatePropertyLock，不锁定
        /// </summary>
        public string DoTheJobByShareStatePropertyLock()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var shareStatePropertyLock = new ShareStatePropertyLock();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(shareStatePropertyLock).DoTheJobByShareStatePropertyLock());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{shareStatePropertyLock.State}\r\n");
            return sb.ToString();
        }

        /// <summary>
        ///  ShareStatePropertyLock中的属性，锁定
        /// Job方法DoTheJobByShareStatePropertyLockAndJobMethodLock，锁定
        /// </summary>
        public string DoTheJobByShareStatePropertyLockAndJobMethodLock()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var shareStatePropertyLock = new ShareStatePropertyLock();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(shareStatePropertyLock).DoTheJobByShareStatePropertyLockAndJobMethodLock());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{shareStatePropertyLock.State}\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// ShareStateMethodLock中的方法，锁定
        /// Job方法DoTheJobByShareStateMethodLock，不锁定
        /// </summary>
        public string DoTheJobByShareStateMethodLock()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var shareStateMethodLock = new ShareStateMethodLock();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(shareStateMethodLock).DoTheJobByShareStateMethodLock());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{shareStateMethodLock.State}\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// ShareStateMethodLock中的方法，锁定
        /// Job方法DoTheJobByShareStateMethodLockAndJobMethodLock，锁定
        /// </summary>
        public string DoTheJobByShareStateMethodLockAndJobMethodLock()
        {
            StringBuilder sb = new StringBuilder();
            int numTasks = 20;
            var shareStateMethodLock = new ShareStateMethodLock();
            var tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => new Job(shareStateMethodLock).DoTheJobByShareStateMethodLockAndJobMethodLock());
            }
            Task.WaitAll(tasks);

            sb.Append($"总共循环次数{shareStateMethodLock.State}\r\n");
            return sb.ToString();
        }
    }
}
