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

        public void DoTheJob()
        {
            for (int i = 0; i < 50000; i++)
            {
                _shareState.State += 1;
            }
        }
    }
}
