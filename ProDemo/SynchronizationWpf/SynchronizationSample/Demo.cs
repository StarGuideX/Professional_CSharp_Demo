using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.SynchronizationSample
{
    public class Demo
    {
        public virtual bool IsSynchronized => false;

        public static Demo Synchronized(Demo d)
        {
            if (!d.IsSynchronized)
            {
                return new SynchronizationDemo(d);
            }
            return d;
        }

        public virtual void DoThis()
        {
        }

        public virtual void DoThat()
        {
        }

        private class SynchronizationDemo : Demo
        {
            private Object _syncRoot = new object();
            private Demo _d;
            public override bool IsSynchronized => true;

            public SynchronizationDemo(Demo d)
            {
                _d = d;
            }

            public override void DoThis()
            {
                lock (_syncRoot)
                {
                    _d.DoThis();
                }
            }

            public override void DoThat()
            {
                lock (_syncRoot)
                {
                    _d.DoThat();
                }
            }
        }
    }
}
