using EFCoreModelUsingConvention.Contexts;
using EFCoreModelUsingConvention.Models;
using EFCoreModelUsingConvention.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCoreModelUsingConvention
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
                await PickMethodAsync(service);
            }
        }

        static async Task PickMethodAsync(BooksService service) {
            Console.WriteLine("请选择");
            Console.WriteLine("1—检查是否创建数据库");
            Console.WriteLine("2—删除数据库");
            Console.WriteLine("3—添加一本");
            Console.WriteLine("4—添加多本书");
            Console.WriteLine("5—根据title查询(请先执行4)");
            


            string index = Console.ReadLine();
            switch (index)
            {
                case "1":
                    await service.CreateTheDatabaseAsync();
                    break;
                case "2":
                    await service.DeleteDatabaseAsync();
                    break;
                case "3":
                    var book = new Book() {
                        Title = "AddBookAsyncTitle",
                    };
                    await service.AddBookAsync(book);
                    break;
                case "4":
                    var books = new List<Book>() {
                        new Book()
                        {
                            Title = "AddBooksAsyncTitle",
                        },
                        new Book()
                        {
                            Title = "AddBooksAsyncTitle",
                        },
                        new Book()
                        {
                            Title = "AddBooksAsyncTitle",
                        },
                        new Book()
                        {
                            Title = "AddBooksAsyncTitle",
                        },
                    };
                    await service.AddBooksAsync(books);
                    break;
                case "5":
                    await service.QueryBooksAsync("AddBooksAsyncTitle");
                    break;
                default:
                    service.Dispose();
                    Console.WriteLine("已关闭连接，请重新启动");
                    break;
            }
        }



    }
}
