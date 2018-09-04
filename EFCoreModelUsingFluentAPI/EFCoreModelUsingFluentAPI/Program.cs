using EFCoreModelUsingFluentAPI.Contexts;
using EFCoreModelUsingFluentAPI.Services;
using System;
using System.Threading.Tasks;

namespace EFCoreModelUsingFluentAPI
{
    class Program
    {
        
        static async Task Main()
        {
            BooksContext bookContext = new BooksContext();
            BooksService service = new BooksService(bookContext);
            service.AddLogging();
            while (true)
            {
                await PickMethodAsync(service, bookContext);
            }
        }

        static async Task PickMethodAsync(BooksService service,BooksContext context)
        {
            Console.WriteLine("请选择");
            Console.WriteLine("1—检查是否创建数据库");
            Console.WriteLine("2—删除数据库");
            //Console.WriteLine("3—添加一本");
            //Console.WriteLine("4—添加多本书");
            //Console.WriteLine("5—根据title查询(请先执行4)");

            string index = Console.ReadLine();
            switch (index)
            {
                case "1":
                    await service.CreateDatabaseAsync();
                    break;
                case "2":
                    await service.DeleteDatabaseAsync();
                    break;
                default:
                    context.Dispose();
                    Console.WriteLine("已关闭连接，请重新启动");
                    break;
            }
        }

    }
}
