using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PickDemo();
        }

        private static void PickDemo()
        {
            Console.WriteLine("11-AsyncDelegateDemo");
            Console.WriteLine("11-BarrierDemo");
            Console.WriteLine("12-ReaderWriterLockSlimDemo");
            var read = Console.ReadLine();
            switch (read)
            {
                case "01":
                    AsyncDelegateDemo.AsyncDelegateDemoStart();
                    break;
                case "11":
                    BarrierDemo.BarrierDemoStart();
                    break;
                case "12":
                    ReaderWriterLockSlimDemo.ReaderWriterLockSlimDemoStart();
                    break;
                    
                default:
                    break;
            }
        }
    }
}
