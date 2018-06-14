using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.SynchronizationSample
{
    public class Job
    {
        private ShareState _shareState;

        public Job(ShareState shareState)
        {
            _shareState = shareState;
        }

        /// <summary>
        /// ShareState和Job都不锁定
        /// </summary>
        public void DoTheJobByNoLockAll()
        {
            for (int i = 0; i < 50000; i++)
            {
                _shareState.State += 1;
            }
        }

        /// <summary>
        /// ShareState，不锁定
        /// Job中方法DoTheJobByJobMethodLock，锁定
        /// </summary>
        public void DoTheJobByJobMethodLock()
        {
            for (int i = 0; i < 50000; i++)
            {
                lock (_shareState)
                {
                    _shareState.State += 1;
                }
            }
        }


        private ShareStatePropertyLock _shareStatePropertyLock;
        public Job(ShareStatePropertyLock shareStatePropertyLock)
        {
            _shareStatePropertyLock = shareStatePropertyLock;
        }
        /// <summary>
        /// ShareStatePropertyLock中的属性，锁定
        /// Job方法DoTheJobByShareStatePropertyLock，不锁定
        /// </summary>
        public void DoTheJobByShareStatePropertyLock()
        {
            for (int i = 0; i < 50000; i++)
            {
                _shareStatePropertyLock.State += 1;
            }
        }
        /// <summary>
        ///  ShareStatePropertyLock中的属性，锁定
        /// Job方法DoTheJobByShareStatePropertyLockAndJobMethodLock，锁定
        /// </summary>
        public void DoTheJobByShareStatePropertyLockAndJobMethodLock()
        {
            for (int i = 0; i < 50000; i++)
            {
                lock (_shareStatePropertyLock)
                {
                    _shareStatePropertyLock.State += 1;
                }
            }
        }

        private ShareStateMethodLock _ShareStateMethodLock;
        public Job(ShareStateMethodLock shareStateMethodLock)
        {
            _ShareStateMethodLock = shareStateMethodLock;
        }

        /// <summary>
        /// ShareStateMethodLock中的方法，锁定
        /// Job方法DoTheJobByShareStateMethodLock，不锁定
        /// </summary>
        public void DoTheJobByShareStateMethodLock()
        {
            for (int i = 0; i < 50000; i++)
            {
                _ShareStateMethodLock.IncrementState();
            }

        }

        /// <summary>
        /// ShareStateMethodLock中的方法，锁定
        /// Job方法DoTheJobByShareStateMethodLockAndJobMethodLock，锁定
        /// </summary>
        public void DoTheJobByShareStateMethodLockAndJobMethodLock()
        {
            for (int i = 0; i < 50000; i++)
            {
                lock (_ShareStateMethodLock)
                {
                    _ShareStateMethodLock.IncrementState();
                }
            }
        }
    }
}
