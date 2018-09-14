using EFCoreModelUsingFluentAPI.Contexts;
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
            Console.WriteLine("5-原始Sql查询-查询所有Title为Book的Book(先执行3)");
            Console.WriteLine("6-编译查询-查询所有Title为Book的Book(先执行3)");
            Console.WriteLine("7-阴影属性和从属实体");
            Console.WriteLine("8-EFCunctions-重复执行的sql语句");
            Console.WriteLine("9-显示加载");
            Console.WriteLine("10-立即加载（急切加载）");
            

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
                case "7":
                    IList<Book> shadowPageBooks = new List<Book>()
                    {
                       new Book()
                       {
                           Title = "shadowPageBook1",
                           Pages = new List<Page>()
                           {
                               new Page("Remark1_1")
                               {
                                   Content ="Content1_1",
                                   TitleFont = new TextFont(){
                                       FontName = "TitleFontName1_1",
                                       FontColor = new FontColor(){ FontColorName = "TitleFontColorName1_1" }
                                   },
                                   TextFont = new TextFont()
                                   {
                                       FontName = "TextFontName1_1",
                                       FontColor = new FontColor(){ FontColorName = "TextFontColorName1_1" }
                                   }
                               },
                               new Page("Remark1_2")
                               {
                                   Content ="Content1_2",
                                   TitleFont = new TextFont(){
                                       FontName = "TitleFontName1_2",
                                       FontColor = new FontColor(){ FontColorName = "TitleFontColorName1_2" }
                                   },
                                   TextFont = new TextFont()
                                   {
                                       FontName = "TextFontName1_2",
                                       FontColor = new FontColor(){ FontColorName = "TextFontColorName1_2" }
                                   }
                               },
                           },
                           Author = new User()
                           {
                             Name="Author",
                             Address=new Address()
                             {
                                  AddressDetail = "AddressDetailAddressDetailAddressDetail"
                             }
                           }
                       }
                    };

                    Task t1 = service.AddShadowPageBooksAsync(shadowPageBooks);
                    t1.Wait();
                    Task t2 = service.DeletePageAsync(1);
                    t2.Wait();
                    Task t3 = service.QueryDeletedPagesAsync();
                    t3.Wait();
                    break;
                case "8":
                    await service.UseEFCunctions("Book");
                    break;
                case "9":
                    IList<Book> explicitBooks = new List<Book>()
                    {
                       new Book()
                       {
                           Title = "ExplicitBook1",
                           Pages = new List<Page>()
                           {
                               new Page("Remark1_1")
                               {
                                   Content ="Content1_1",
                                   TitleFont = new TextFont(){
                                       FontName = "TitleFontName1_1",
                                       FontColor = new FontColor(){ FontColorName = "TitleFontColorName1_1" }
                                   },
                                   TextFont = new TextFont()
                                   {
                                       FontName = "TextFontName1_1",
                                       FontColor = new FontColor(){ FontColorName = "TextFontColorName1_1" }
                                   }
                               },
                               new Page("Remark1_2")
                               {
                                   Content ="Content1_2",
                                   TitleFont = new TextFont(){
                                       FontName = "TitleFontName1_2",
                                       FontColor = new FontColor(){ FontColorName = "TitleFontColorName1_2" }
                                   },
                                   TextFont = new TextFont()
                                   {
                                       FontName = "TextFontName1_2",
                                       FontColor = new FontColor(){ FontColorName = "TextFontColorName1_2" }
                                   }
                               },
                           },
                           Author = new User()
                           {
                             Name="Author",
                             Address=new Address()
                             {
                                  AddressDetail = "AddressDetailAddressDetailAddressDetail"
                             }
                           }
                       }
                    };
                    service.AddShadowPageBooksAsync(explicitBooks).Wait();
                    Console.WriteLine("数据添加完毕");
                    service.ExplicitLoading("Explicit");
                    break;
                case "10":
                    IList<Book> eagerBooks = new List<Book>()
                    {
                       new Book()
                       {
                           Title = "EagerBook1",
                           Pages = new List<Page>()
                           {
                               new Page("Remark1_1")
                               {
                                   Content ="Content1_1",
                                   TitleFont = new TextFont(){
                                       FontName = "TitleFontName1_1",
                                       FontColor = new FontColor(){ FontColorName = "TitleFontColorName1_1" }
                                   },
                                   TextFont = new TextFont()
                                   {
                                       FontName = "TextFontName1_1",
                                       FontColor = new FontColor(){ FontColorName = "TextFontColorName1_1" }
                                   }
                               },
                               new Page("Remark1_2")
                               {
                                   Content ="Content1_2",
                                   TitleFont = new TextFont(){
                                       FontName = "TitleFontName1_2",
                                       FontColor = new FontColor(){ FontColorName = "TitleFontColorName1_2" }
                                   },
                                   TextFont = new TextFont()
                                   {
                                       FontName = "TextFontName1_2",
                                       FontColor = new FontColor(){ FontColorName = "TextFontColorName1_2" }
                                   }
                               },
                           },
                           Author = new User()
                           {
                             Name="Author",
                             Address=new Address()
                             {
                                  AddressDetail = "AddressDetailAddressDetailAddressDetail"
                             }
                           }
                       }
                    };
                    service.AddShadowPageBooksAsync(eagerBooks).Wait();
                    Console.WriteLine("数据添加完毕");
                    service.EagerLoading("Eager");
                    break;
                case "11":
                    service.AddRecords();
                    break;
                case "12":
                    service.ObjectTracking();
                    break;
                case "13":
                    service.UpdateRecords();
                    break;
                case "14":
                    service.ChangeUntracked();
                    break;
                case "15":
                    service.AddHundredRecords();
                    break;
                case "16":
                    service.ConflictHandling();
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
                (options => {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(ConnectionString); }
                );

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
