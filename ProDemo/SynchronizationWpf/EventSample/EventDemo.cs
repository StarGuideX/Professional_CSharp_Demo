using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationWpf.EventSample
{
    public class EventDemo
    {
        /// <summary>
        /// 程序的EventDemoStart()方法定义了包含4个ManualResetEventSlim对象的数组和包含4个Calculator对象的数组。
        /// 每个Calculator在构造函数中用一个ManualResetEventSlim对象初始化，这样每个任务在完成时都有自己的事件对象来发信号。
        /// 现在使用Task类，让不同的任务执行计算任务。
        /// </summary>
        public string EventDemoStart()
        {
            const int taskCount = 4;

            var mEvents = new ManualResetEventSlim[taskCount];
            var waitHandles = new WaitHandle[taskCount];
            var calcs = new Calculator[taskCount];

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < taskCount; i++)
            {
                int j = i;
                mEvents[i] = new ManualResetEventSlim(false);
                waitHandles[i] = mEvents[i].WaitHandle;
                calcs[i] = new Calculator(mEvents[i]);
                Task.Run(() => sb.Append(calcs[j].Calculation(j + 1, j + 3)));
            }

            // WaitHandle类现在用于等待数组中的任意一个事件。
            // WaitAny()方法等待向任意一个事件发信号。
            // 与ManualResetEvent对象不同，ManualResetEventSlim对象不派生自WaitHandle类。
            // 因此有一个WaitHandle对象的集合，它在ManualResetEventSlim类的WaitHandle属性中填充。
            // 从WaitAny()方法返回的index值匹配传递给Wainy()方法的事件数组的索引，
            // 以提供发信号的事件的相关信息，使用该索引可以从这个事件中读取结果。

            for (int i = 0; i < taskCount; i++)
            {
                int index = WaitHandle.WaitAny(waitHandles);
                if (index == WaitHandle.WaitTimeout)
                {
                    sb.Append("超时\r\n");
                }
                else
                {
                    mEvents[index].Reset();
                    sb.Append($"索引{index}任务已完成，结果：{calcs[index].Result}\r\n");
                }
            }
            return sb.ToString();
        }
    }
}
