using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationWpf.AsyncDelegateSample
{
    public class AsyncDelegate
    {
        public delegate int TakesAWhileDelegate(int x, int ms);

        public string StartDemo() {

            StringBuilder sb = new StringBuilder();

            try
            {
                TakesAWhileDelegate d1 = TakesAWhile;

                IAsyncResult ar = d1.BeginInvoke(1, 3000, null, null);
                while (true)
                {
                    sb.Append(".");
                    if (ar.AsyncWaitHandle.WaitOne(50))
                    {
                        sb.Append("现在不能返回结果\r\n");
                        break;
                    }
                }
                int result = d1.EndInvoke(ar);
                sb.Append($"结果: {result}\r\n");
                return sb.ToString();
            }
            catch (PlatformNotSupportedException)
            {
                return "PlatformNotSupported exception - with async delegates please use the full .NET Framework";
            }
        }

        private int TakesAWhile(int x, int ms)
        {
            Task.Delay(ms).Wait();
            return 42;
        }
    }
}
