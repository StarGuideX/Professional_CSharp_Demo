using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsoleApp
{
    public static class DataProtectionDemo
    {
        private const string readOption = "-r";
        private const string writeOption = "-w";
        private static readonly string[] options = { readOption, writeOption };

        /// <summary>
        /// 使用-r和-w命令行参数，可以启动控制台应用程序，读写存储器。
        /// 此外，需要使用命令行，设置一个文件名来读写。
        /// 检查命令行参数后，通过调用InitProtection辅助方法来初始化数据保护。
        /// 这个方法返回一个MySafe类型的对象，嵌入IDataProtector。
        /// 之后，根据命令行参数，调用Write或Read方法
        /// </summary>
        /// <param name="arr"></param>
        public static void DataProtectionStart(string[] arr)
        {
            if (arr.Length != 2 || arr.Intersect(options).Count() != 1)
            {
                ShowUsage();
                return;
            }

            string fileName = arr[1];

            MySafe safe = InitProtection();

            switch (arr[0])
            {

                case writeOption:
                    Write(safe, fileName);
                    break;
                case readOption:
                    Read(safe, fileName);
                    break;
                default:
                    ShowUsage();
                    break;
            }
        }

        /// <summary>
        /// 通过InitProtection方法调用AddDataProtection和ConfigureDataProtection扩展方法，
        /// 通过依赖注入添加数据保护，并配置它。
        /// AddDataProtection方法通过调用DataProtectionServices.GetDefaultServices静态方法，注册默认服务。
        /// ConfigureDataProtection方法包含一个有趣的特殊部分。在这里，它定义了密钥应该如何保存。
        /// 示例代码把Directorylnfo实例传递给PersistKeysToFileSystem方法，把密钥保存在实际的目录中。
        /// 另一个选择是把密钥保存到注册表（PersistKeysToRegistry）中， 可以创建自己的方法，把密钥保存在定制的存储中。
        /// 所创建密钥的生命周期由SetDefaultKeyLifetime方法定义。
        /// 接下来，密钥通过调用ProtectKeysWithDpapi来保护。
        /// 这个方法使用DPAPI保护密钥，加密与当前用户一起存储的密钥。
        /// ProtectKeysWithCeritificate允许使用证书保护密钥。
        /// API还定义了UseEphemeralDataProtectionProvider方法，把密钥存储在内存中。
        /// 再次启动应用程序时，需要生成新密钥。这个功能非常适合于单元测试
        /// </summary>
        /// <returns></returns>
        private static MySafe InitProtection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("."))
                .SetDefaultKeyLifetime(TimeSpan.FromDays(20))
                .ProtectKeysWithDpapi();
            IServiceProvider services = serviceCollection.BuildServiceProvider();

            return ActivatorUtilities.CreateInstance<MySafe>(services);
        }

        private static void Write(MySafe safe, string fileName)
        {
            Console.WriteLine("输入内容，开始write方法");
            string content = Console.ReadLine();
            string encrypted = safe.Encrypt(content);
            File.WriteAllText(fileName, encrypted);
            Console.WriteLine($"内容已写入{fileName}");
        }

        private static void Read(MySafe safe, string fileName)
        {
            string encrypted = File.ReadAllText(fileName);
            string decrypted = safe.Decrypt(encrypted);
            Console.WriteLine(decrypted);
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage: DataProtectionSample options filename");
            Console.WriteLine("Options:");
            Console.WriteLine("\t-r Read");
            Console.WriteLine("\t-w Write");
            Console.WriteLine();
        }
    }
}
