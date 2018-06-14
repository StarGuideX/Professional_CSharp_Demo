using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.SynchronizationSample
{
    /// <summary>
    ///  无锁定
    /// </summary>
    public class ShareState
    {
        public int State { get; set; }
    }
    /// <summary>
    /// ShareStateThreadSafe中State，属性的锁定
    /// </summary>
    public class ShareStatePropertyLock
    {
        private int _state = 0;
        private object _syncRoot = new object();

        public int State
        {
            get { lock (_syncRoot) { return _state; } }
            set { lock (_syncRoot) { _state = value; } }
        }
    }
    /// <summary>
    /// ShareStateMethodThreadSafe中IncrementState，方法的锁定
    /// </summary>
    public class ShareStateMethodLock
    {
        private int _state = 0;
        private object _syncRoot = new object();

        public int State => _state;

        public int IncrementState()
        {
            lock (_syncRoot)
            {
                return ++_state;
            }
        }
    }
}
