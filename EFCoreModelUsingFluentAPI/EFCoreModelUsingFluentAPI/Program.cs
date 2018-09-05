﻿using EFCoreModelUsingFluentAPI.Contexts;
using EFCoreModelUsingFluentAPI.Models;
using EFCoreModelUsingFluentAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCoreModelUsingFluentAPI
{
    class Program
    {

        static async Task Main()
        {
            var p = new Program();
            p.InitializeServices();
            p.ConfigureLogging();
            var service = p.Container.GetService<BooksService>();
            while (true)
            {
                await PickMethodAsync(service);
            }
        }

        static async Task PickMethodAsync(BooksService service)
        {
            Console.WriteLine("请选择");
            Console.WriteLine("1—检查是否创建数据库");
            Console.WriteLine("2-删除数据库");
            Console.WriteLine("3-添加多本书");
            Console.WriteLine("4-基本查询-查询所有Book");
            Console.WriteLine("5-原始Sql查询-查询所有Book(先执行3)");
            Console.WriteLine("6-编译查询-")
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
                case "3":
                    IList<Book> books = new List<Book>() {
                        new Book
                        {
                            Title = "Book"
                        },
                        new AdultBook
                        {
                            Title = "AdultBook"
                        },
                        new MinorBook
                        {
                            Title = "AdultBook"
                        },
                        new Book
                        {
                            Title = "Book"
                        },
                        new Book
                        {
                            Title = "Book"
                        }
                    };
                    await service.AddBooksAsync(books);
                    break;
                case "4":
                    await service.QueryAllBooksAsync();
                    break;
                case "5":
                    await service.RawSqlQuery("Book");
                    break;
                case "6":
                    service.CompiledQuery("Book");
                    break;
                default:
                    Console.WriteLine("已关闭连接，请重新启动");
                    break;
            }
        }


        /// <summary>
        /// 使用依赖注入
        /// </summary>
        private void InitializeServices()
        {
            const string ConnectionString = @"server=(localdb)\MSSQLLocalDb;database=EFCoreDemoFluentAPI;trusted_connection=true";
            var services = new ServiceCollection();
            services.AddTransient<BooksService>()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<BooksContext>
                (options => options.UseSqlServer(ConnectionString));

            Container = services.BuildServiceProvider();
        }
        public ServiceProvider Container { get; private set; }
        /// <summary>
        /// 添加日志，输出到Console
        /// </summary>
        private void ConfigureLogging()
        {
            ILoggerFactory loggerFactory = Container.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Information);
        }
    }
}