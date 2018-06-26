using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsole
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
            Console.WriteLine("1-Windows Principal-验证用户信息");
            Console.WriteLine("2-SHA512-创建和验证签名");
            Console.WriteLine("3-ECDiffieHellmanP521-安全的数据交换");
            Console.WriteLine("**********选项**********");

            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    WindowsPrincipalDemo.WindowsPrincipalStart();
                    break;
                case "2":
                    SigningDemo.SigningStart();
                    break;
                case "3":
                    SecureTransferDemo.SecureTransferStart();
                    break;
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }

    }
}
