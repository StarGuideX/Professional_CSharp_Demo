using CompositionConsoleApp.AttributeBaseSample.SimpleCalculator;
using CompositionConsoleApp.AttributeBaseSample.SimpleHost;
using CompositionConsoleApp.ConventionBaseSample.SimpleHost;
using System;
using System.Composition.Hosting;

namespace CompositionConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                PickDemo();
            }
        }

        private static void PickDemo()
        {
            Console.WriteLine("**********选项**********");
            Console.WriteLine("1-[Import] [Export]-使用特性的Composition");
            Console.WriteLine("2-ConventionBuilder-基于约定的部件注册");
            Console.WriteLine("3-ECDiffieHellmanP521-安全的数据交换");
            Console.WriteLine("4-FileSecurity-资源的访问控制");

            Console.WriteLine("**********选项**********");

            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    new AttributeBaseHost().AttributeBaseStart();
                    break;
                case "2":
                    new ConventionBaseHost().ConventionBaseStart();
                    break;
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }

    }
}
