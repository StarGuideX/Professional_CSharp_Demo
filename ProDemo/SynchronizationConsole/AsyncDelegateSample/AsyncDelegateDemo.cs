using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationConsole
{
    public static class AsyncDelegateDemo
    {
        public delegate int TakesAWhileDelegate(int x, int ms);

        public static void AsyncDelegateDemoStart()
        {
            try
            {
                TakesAWhileDelegate d1 = TakesAWhile;

                IAsyncResult ar = d1.BeginInvoke(1, 3000, null, null);
                while (true)
                {
                    Console.Write(".");
                    if (ar.AsyncWaitHandle.WaitOne(50))
                    {
                        Console.WriteLine("现在不能返回结果");
                        break;
                    }
                }
                int result = d1.EndInvoke(ar);
                Console.WriteLine($"结果: {result}");
            }
            catch (PlatformNotSupportedException)
            {
                Console.WriteLine("PlatformNotSupported exception - with async delegates please use the full .NET Framework");
            }
        }

        private static int TakesAWhile(int x, int ms)
        {
            Task.Delay(ms).Wait();
            return 42;
        }
    }
}
