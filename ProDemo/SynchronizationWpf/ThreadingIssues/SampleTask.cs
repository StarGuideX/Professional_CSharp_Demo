using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.ThreadingIssues
{
    public class StateObject
    {
        private int _state = 5;
        private object _sync = new object();
        public void ChangeState(int loop)
        {
            lock (_sync)// 如想发生争用，请注释掉
            {
                if (_state == 5)
                {
                    _state++;
                    Trace.Assert(_state == 6, $"在循环{loop}了，发生了争用条件");
                }
            }
        }
    }

    public class SampleTask
    {
        public SampleTask()
        {

        }

        /// <summary>
        /// 将一个StateObject类作为其参数。在一个无限while循环中，调用ChangeState()方法。
        /// </summary>
        /// <param name="o"></param>
        public void RaceCondition(object o)
        {
            Trace.Assert(o is StateObject, "o必须是StateObject类型");
            StateObject state = o as StateObject;

            int i = 0;
            while (true)
            {
                lock (state) // 如想发生争用，请注释掉
                {
                    state.ChangeState(i++);
                }
            }
        }

        private StateObject _s1;
        private StateObject _s2;

        public SampleTask(StateObject s1, StateObject s2)
        {
            _s1 = s1;
            _s2 = s2;
        }

        public void Deallock1()
        {
            int i = 0;
            while (true)
            {
                lock (_s1)
                {
                    lock (_s2)
                    {
                        _s1.ChangeState(i);
                        _s2.ChangeState(i++);
                        Console.WriteLine($"正在运行{i}");
                    }
                }
            }
        }

        public void Deallock2()
        {
            int i = 0;
            while (true)
            {
                lock (_s2)
                {
                    lock (_s1)
                    {
                        _s1.ChangeState(i);
                        _s2.ChangeState(i++);
                        Console.WriteLine($"正在运行{i}");
                    }
                }
            }
        }
    }
}
