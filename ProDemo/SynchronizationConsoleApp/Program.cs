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


    }
}
