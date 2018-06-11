using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.ThreadingIssues
{
    public class SampleTask
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
