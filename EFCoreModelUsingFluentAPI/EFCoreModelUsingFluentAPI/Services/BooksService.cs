using EFCoreModelUsingFluentAPI.Contexts;
using EFCoreModelUsingFluentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EFCoreModelUsingFluentAPI.Models.PageColumnNames;

namespace EFCoreModelUsingFluentAPI.Services
{
    public class BooksService
    {
        #region 使用依赖注入
        private BooksContext _booksContext;
        public BooksService(BooksContext context) => _booksContext = context;
        #endregion


        public async Task CreateDatabaseAsync()
        {
            bool isCreated = await _booksContext.Database.EnsureCreatedAsync();
            string res = isCreated ? "创建完毕" : "已创建";
            Console.WriteLine($"数据库创建：{res}");
        }

        public async Task DeleteDatabaseAsync()
        {
            bool isDeleted = await _booksContext.Database.EnsureDeletedAsync();
            string res = isDeleted ? "删除完毕" : "无此数据库";
            Console.WriteLine($"数据库删除：{res}");
        }

        public async Task AddBooksAsync(IEnumerable<Book> books)
        {

            // 只是将对象添加进上下文中，并没有写入数据库
            await _booksContext.Books.AddRangeAsync(books);
            // SaveChangesAsync才会写入数据库
            int records = await _booksContext.SaveChangesAsync();
        }

        /// <summary>
        /// 基本查询
        /// 根据Id查询book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Book> QueryBookAsync(int id)
        {
            return await _booksContext.FindAsync<Book>(id);
            //return await _booksContext.Books.FirstOrDefaultAsync(b=>b.BookId == id);
        }
        /// <summary>
        /// 基本查询
        /// 查询所有Book
        /// </summary>
        /// <returns></returns>
        public async Task QueryAllBooksAsync()
        {
            List<Book> books = await _booksContext.Books.ToListAsync();
            foreach (var b in books)
            {
                Console.WriteLine(b);
            }
            // 使用异步API时，可以使用从ToAsyncEnumerable方法返回的IAsyncEnumerable，并使用ForEachAsync
            //await context.Books.ToAsyncEnumerable().ForEachAsync(b =>
            //{
            //    Console.WriteLine(b);
            //});
        }

        /// <summary>
        /// 原始Sql查询
        /// </summary>
        /// <param name="publisher"></param>
        /// <returns></returns>
        public async Task RawSqlQuery(string title)
        {
            IList<Book> books = await _booksContext.Books.FromSql(
            $"SELECT * FROM fluent.Books WHERE Title = {title}")
            .ToListAsync();
            foreach (var b in books)
            {
                Console.WriteLine(b.ToString());
            }
        }

        /// <summary>
        /// 编译查询
        /// </summary>
        public void CompiledQuery(string qTitle)
        {
            Func<BooksContext, string, IEnumerable<Book>> query =
                EF.CompileQuery<BooksContext, string, Book>((context, title) =>
                context.Books.Where(b => b.Title == title));

            IEnumerable<Book> books = query(_booksContext, qTitle);
            foreach (var b in books)
            {
                Console.WriteLine(b.ToString());
            }
        }

        /// <summary>
        /// EF.Functions
        /// 通过使用EF.Functions.Like增强了Where方法的查询，并提供包含参数titleSegment的表达式。 
        /// 参数titleSegment嵌入在两个％字符内
        /// </summary>
        /// <param name="titleSegment"></param>
        /// <returns></returns>
        public async Task UseEFCunctions(string titleSegment)
        {
            string likeExpression = $"%{titleSegment}%";
            IList<Book> books = await _booksContext.Books.Where(
            b => EF.Functions.Like(b.Title,
            likeExpression)).ToListAsync();
            foreach (var b in books)
            {
                Console.WriteLine(b.ToString());
            }
        }

        #region 阴影属性和从属实体
        /// <summary>
        /// 验证阴影属性
        /// </summary>
        /// <returns></returns>
        public async Task AddShadowPageBooksAsync(IEnumerable<Book> books)
        {
            await _booksContext.Books.AddRangeAsync(books);
            await _booksContext.SaveChangesAsync();
            Console.WriteLine($"ShadowPageBooks添加完毕");
            Console.WriteLine();
        }

        /// <summary>
        /// 删除、有阴影属性isDeleted会设为true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePageAsync(int id)
        {
            Page p = await _booksContext.Pages.FindAsync(id);
            if (p == null) return;
            _booksContext.Pages.Remove(p);
            await _booksContext.SaveChangesAsync();
            Console.WriteLine("运行DeletePageAsync完毕");
            Console.WriteLine();
        }

        /// <summary>
        /// 验证DeleteBookAsync方法
        /// </summary>
        /// <returns></returns>
        public async Task QueryDeletedPagesAsync()
        {
            IEnumerable<Page> deletedPages =
            await _booksContext.Pages
            .Where(b => EF.Property<bool>(b, IsDeleted))
            .ToListAsync();
            foreach (var page in deletedPages)
            {
                Console.WriteLine($"deleted: {page}");
            }
        }
        #endregion

        /// <summary>
        /// 使用BooksContext注册新的logger
        /// </summary>
        public void AddLogging()
        {
            // 使用GetInfrastructure检索IServiceProvider
            IServiceProvider provider = _booksContext.GetInfrastructure<IServiceProvider>();
            // 使用IServiceProvider可以检索在容器中注册的服务，如ILoggerFactory
            ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
            // 使用ILoggerFactory，可以添加log provider，例如Console log provider
            loggerFactory.AddConsole(LogLevel.Information);
        }
    }
}
