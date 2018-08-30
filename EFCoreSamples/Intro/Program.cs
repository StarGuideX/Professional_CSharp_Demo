using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro
{
    class Program
    {
        static async Task Main()
        {
            var p = new Program();
            await p.CreateTheDatabaseAsync();
            Console.Read();
        }

        private async Task CreateTheDatabaseAsync()
        {
            using (var context = new BooksContext())
            {
                bool created = await context.Database.EnsureCreatedAsync();
                string creationInfo = created ? "created" : "exists";
                Console.WriteLine($"数据库{creationInfo}");
            }
        }

        private async Task DeleteDatabaseAsync()
        {
            Console.WriteLine("删除数据库？");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using (var context = new BooksContext())
                {
                    bool deleted = await context.Database.EnsureDeletedAsync();
                    string deletedInfo = deleted ? "deleted" : "not deleted";
                    Console.WriteLine($"数据库{deletedInfo}");
                }
            }
        }

        private async Task AddBookAsync(string title, string publisher)
        {
            using (var context = new BooksContext())
            {
                var book = new Book
                {
                    Title = title,
                    Publisher = publisher
                };
                // 只是将对象添加进山下文中，并没有写入数据库
                await context.Books.AddAsync(book);
                // SaveChangesAsync才会写入数据库
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"添加了{records}条");
            }
        }

        private async Task AddBooksAsync()
        {
            using (var context = new BooksContext())
            {
                var book1 = new Book
                {
                    Title = "title1",
                    Publisher = "publisher1"
                };
                var book2 = new Book
                {
                    Title = "title2",
                    Publisher = "publisher2"
                };
                var book3 = new Book
                {
                    Title = "title3",
                    Publisher = "publisher3"
                };

                await context.Books.AddRangeAsync(book1, book2, book3);
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"添加了{records}条");
            }
        }

        private async Task QueryBooksAsync()
        {
            using (var context = new BooksContext())
            {
                List<Book> books = await context.Books.Where(b => b.Publisher == "publisher1").ToListAsync();
                // 使用声明式Linq 查询语法，和上一个语法会生成相同的sql语句
                // var books = await (from b in context.Books where b.Publisher == "publisher1" select b).ToListAsync();

                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title}{b.Publisher}");
                }
            }
        }

        private async Task UpdateBookAsync()
        {
            using (var context = new BooksContext())
            {
                int records = 0;
                Book book = await context.Books.Where(b => b.Title == "title1").FirstOrDefaultAsync();
                if (book != null)
                {
                    book.Title = "title111";
                    records = await context.SaveChangesAsync();
                }
                Console.WriteLine($"更新了{records}条");
            }
        }

        private async Task DeleteBooksAsync()
        {
            using (var context = new BooksContext())
            {
                var books = context.Books;
                context.Books.RemoveRange(books);
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"删除了{records}条");
            }
        }

        /// <summary>
        /// 使用BooksContext注册新的logger
        /// </summary>
        private void AddLogging() {
            using (var context = new BooksContext())
            {
                // 使用GetInfrastructure检索IServiceProvider
                IServiceProvider provider = context.GetInfrastructure<IServiceProvider>();
                // 使用IServiceProvider可以检索在容器中注册的服务，如ILoggerFactory
                ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
                // 使用ILoggerFactory，可以添加log provider，例如Console log provider
                loggerFactory.AddConsole(LogLevel.Information);
            }
        }


    }
}
