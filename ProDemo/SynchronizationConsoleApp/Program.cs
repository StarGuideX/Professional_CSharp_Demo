using SynchronizationConsoleApp.ThreadingIssues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入r演示争用，输入d演示死锁");
            string line = Console.ReadLine();

            switch (line)
            {
                case "r":
                    RaceCondition();
                    break;
                default:
                    ShowUsage();
                    break;
            }

            Console.ReadLine();
        }

        private static void ShowUsage()
        {
            //Console.WriteLine($"Usage: ThreadingIssues options");
            //Console.WriteLine();
            //Console.WriteLine("options:");
            //Console.WriteLine("\t-d\tCreate a deadlock");
            //Console.WriteLine("\t-r\tCreate a race condition");
        }

        /// <summary>
        /// 此方法新建了一个StateObject对象，它由所有任务共享。
        /// 通过使用传递给Task的Run方法的lambda表达式调用Racecondition方法来创建Task对象。
        /// 然后，主线程等待用户输入。但是，因为可能出现争用，所以程序很有可能在读取用户输入前就挂起：
        /// </summary>
        public static void RaceCondition()
        {
            var state = new StateObject();
            for (int i = 0; i < 50000; i++)
            {
                Task.Run(() => new SampleTask().RaceCondition(state));
            }
        }
    }
}
