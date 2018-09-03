using EFCoreModelUsingConvention.Contexts;
using EFCoreModelUsingConvention.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreModelUsingConvention.Services
{
    /// <summary>
    /// 本例为了关闭连接，所有实现了IDisposable，正常有别的方法
    /// </summary>
    public class BooksService:IDisposable
    {
        private readonly BooksContext _booksContext;
        public BooksService(BooksContext context) => _booksContext = context;

        /// <summary>
        /// 检查是否创建数据库
        /// </summary>
        /// <returns></returns>
        public async Task CreateTheDatabaseAsync()
        {
            bool created = await _booksContext.Database.EnsureCreatedAsync();
            string creationInfo = created ? "已创建数据库" : "已存在数据库";
            Console.WriteLine($"数据库：{creationInfo}");
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <returns></returns>
        public async Task DeleteDatabaseAsync()
        {
            Console.WriteLine("删除数据库？");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                bool deleted = await _booksContext.Database.EnsureDeletedAsync();
                string deletedInfo = deleted ? "已删除数据库" : "数据库不存在";
                Console.WriteLine($"数据库：{deletedInfo}");
            }
        }

        /// <summary>
        /// 添加一本
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task AddBookAsync(Book book)
        {
            // 只是将对象添加进上下文中，并没有写入数据库
            await _booksContext.Books.AddAsync(book);
            // SaveChangesAsync才会写入数据库
            int records = await _booksContext.SaveChangesAsync();
            Console.WriteLine($"添加了{records}条");
        }

        /// <summary>
        /// 添加多本书
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        public async Task AddBooksAsync(IEnumerable<Book> books)
        {
            await _booksContext.Books.AddRangeAsync(books);
            int records = await _booksContext.SaveChangesAsync();
            Console.WriteLine($"添加了{records}条");
        }

        /// <summary>
        /// 根据title查询
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task QueryBooksAsync(string title)
        {
            List<Book> books = await _booksContext.Books.Where(b => b.Title.Equals(title)).ToListAsync();
            // 使用声明式Linq查询语法，和上一个语法会生成相同的sql语句
            // var books = await (from b in context.Books where b.Publisher == "publisher1" select b).ToListAsync();
            foreach (var b in books)
            {
                Console.WriteLine($"{b.Title}");
            }
        }

        /// <summary>
        /// 更新书籍
        /// </summary>
        /// <returns></returns>
        public async Task UpdateBookAsync(Book book)
        {
            int records = 0;
            Book queryBook = await _booksContext.Books.Where(b => b.BookId.Equals(book.BookId)).FirstOrDefaultAsync();
            if (queryBook != null)
            {
                queryBook = book;
                records = await _booksContext.SaveChangesAsync();
            }
            Console.WriteLine($"更新了{records}条");
        }

        /// <summary>
        /// 删除所有书
        /// </summary>
        /// <returns></returns>
        public async Task DeleteBooksAsync()
        {
            var books = _booksContext.Books;
            _booksContext.Books.RemoveRange(books);
            int records = await _booksContext.SaveChangesAsync();
            Console.WriteLine($"删除了{records}条");
        }


        public static void NoImplicitLoadingWithEFCore2()
        {
            Console.WriteLine(nameof(NoImplicitLoadingWithEFCore2));
            using (var context = new BooksContext())
            {
                var book1 = (from b in context.Books
                             from c in b.Chapters
                             where c.Title.StartsWith("Entity")
                             select b).FirstOrDefault();

                var book = context.Books
                    .SelectMany(b => b.Chapters, (b, chapters) => new { Book = b, Chapters = chapters })  // defining expression trees does not support tuples (yet)
                    .Where(bc => bc.Chapters.Title.StartsWith("Entity"))
                    .Select(bc => bc.Book).FirstOrDefault();
                //var book = context.Books
                //    .SelectMany(b => b.Chapters, (b, chapters) => (Book: b, Chapters: chapters))  // defining expression trees does not support tuples (yet) - see https://github.com/dotnet/roslyn/issues/12897
                //    .Where(bc => bc.Chapters.Title.StartsWith("Entity"))
                //    .Select(bc => bc.Book).FirstOrDefault();

                if (book != null && book.Chapters == null)
                {
                    Console.WriteLine("Chapters are not implicitly loaded with EF Core, this is different from Entity Framework");
                }


                if (book != null)
                {
                    Console.WriteLine(book.Title);
                    if (!context.Entry(book).Collection(b => b.Chapters).IsLoaded)
                    {
                        context.Entry(book).Collection(b => b.Chapters).Load();
                    }

                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}. {chapter.Title}");
                    }
                }
            }
            Console.WriteLine();
        }

        public static void BooksForAuthor()
        {
            using (var context = new BooksContext())
            {
                var author = context.Users.Include(u => u.AuthoredBooks).Where(u => u.Name == "Christian Nagel").FirstOrDefault();
                if (author != null)
                {
                    Console.WriteLine(author.Name);
                    foreach (var b in author.AuthoredBooks)
                    {
                        Console.WriteLine(b.Title);
                    }
                }
            }
        }

        public static void ExplicitLoading()
        {
            Console.WriteLine(nameof(ExplicitLoading));
            using (var context = new BooksContext())
            {
                var book = context.Books.Where(b => b.Title.StartsWith("Professional C# 7")).FirstOrDefault();
                if (book != null)
                {
                    Console.WriteLine(book.Title);
                    context.Entry(book).Collection(b => b.Chapters).Load();
                    context.Entry(book).Reference(b => b.Author).Load();
                    Console.WriteLine(book.Author.Name);
                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}. {chapter.Title}");
                    }
                }
            }
            Console.WriteLine();
        }

        public static void EagerLoading()
        {
            Console.WriteLine(nameof(EagerLoading));
            using (var context = new BooksContext())
            {
                var book = context.Books
                    .Include(b => b.Chapters)
                    .Include(b => b.Author)
                    .Where(b => b.Title.StartsWith("Professional C# 7"))
                    .FirstOrDefault();
                if (book != null)
                {
                    Console.WriteLine(book.Title);

                    Console.WriteLine(book.Author.Name);
                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}. {chapter.Title}");
                    }
                }
            }
            Console.WriteLine();
        }


        /// <summary>
        /// 使用BooksContext注册新的logger
        /// </summary>
        public void AddLogging()
        {
            using (var context = new BooksContext())
            {
                // 使用GetInfrastructure检索IServiceProvider
                IServiceProvider provider = _booksContext.GetInfrastructure<IServiceProvider>();
                // 使用IServiceProvider可以检索在容器中注册的服务，如ILoggerFactory
                ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
                // 使用ILoggerFactory，可以添加log provider，例如Console log provider
                loggerFactory.AddConsole(LogLevel.Information);
            }
        }

        public void Dispose()
        {
            _booksContext.Dispose();
        }
    }
}
