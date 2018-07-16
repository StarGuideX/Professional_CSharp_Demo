using DynamicConsole.DecompileSample;
using DynamicConsole.DynamicSample;
using System;

namespace DynamicConsole
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
            Console.WriteLine("1-Dynamic-将Dynamic可以在运行时转为任何类型（也可以多次转换）");
            Console.WriteLine("2-Dynamic-在获得具体类型之前所需要的时间很多");

            Console.WriteLine("**********选项**********");

            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    new DynamicDemo().DynamicDemoStart();
                    break;
                case "2":
                    new DecompileDemo().DecompileStart();
                    break;

                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }
    }
}
