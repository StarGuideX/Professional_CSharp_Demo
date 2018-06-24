using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole
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
            #region StreamReader、StreamWriter、BinaryWrite、BinaryReader
            Console.WriteLine("1-NamedPipeServerStream-命名管道服务器");
            #endregion

            Console.WriteLine("**********选项**********");

            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    PipesReaderDemo.PipesReader("");
                    break;
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }
    }
}
