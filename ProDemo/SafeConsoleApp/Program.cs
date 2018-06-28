using System;

namespace SafeConsoleApp
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
            Console.WriteLine("2-RSA-使用RSA签名和散列");
            Console.WriteLine("3-ECDiffieHellmanP521-安全的数据交换");
            Console.WriteLine("4-FileSecurity-资源的访问控制");

            Console.WriteLine("**********选项**********");

            var read = Console.ReadLine();
            switch (read)
            {
                case "1":
                    WindowsPrincipalDemo.WindowsPrincipalStart();
                    break;
                case "2":
                    RSADemo.RSAStart();
                    break;
                case "3":
                    DataProtectionDemo.DataProtectionStart(new string[] { "-w", Environment.CurrentDirectory + @"\Test.txt" });
                    //DataProtectionDemo.DataProtectionStart(new string[] { "-r", Environment.CurrentDirectory + @"\Test.txt" });
                    break;
                case "4":
                    FileAccessControlDemo.FileAccessControlStart(Environment.CurrentDirectory + @"\Test.txt");
                    break;
                    
                default:
                    break;
            }
            Console.WriteLine($"已完成{read}");
        }
    }
}
