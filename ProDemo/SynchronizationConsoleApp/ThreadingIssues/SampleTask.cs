using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationConsoleApp.ThreadingIssues
{
    public class StateObject
    {
        private int _state = 5;

        public void ChangeState(int loop)
        {
            if (_state == 5)
            {
                _state++;
                Trace.Assert(_state == 6, $"在循环{loop}了，发生了争用条件");
            }
        }
    }

    public class SampleTask
    {
        private StateObject _s1;
        private StateObject _s2;

        public SampleTask() {

        }

        public SampleTask(StateObject s1, StateObject s2)
        {
            _s1 = s1;
            _s2 = s2;
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
                state.ChangeState(i++);
            }
        }
    }
}
