using EFCoreModelUsingFluentAPI.Contexts;
using EFCoreModelUsingFluentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        /// 显示加载，每load一次，对应的表就会进行一次查询。
        /// </summary>
        /// <param name="startsWithTitle"></param>
        public void ExplicitLoading(string startsWithTitle)
        {
            var book = _booksContext.Books.Where(b => b.Title.StartsWith(startsWithTitle)).FirstOrDefault();
            if (book != null)
            {
                _booksContext.Entry(book).Collection(b => b.Pages).Load();
                _booksContext.Entry(book).Reference(b => b.Author).Load();
                Console.WriteLine(book.Author.Name);
                foreach (var page in book.Pages)
                {
                    Console.WriteLine(page.Content);
                }
            }
        }

        /// <summary>
        /// 立即加载（急切加载）
        /// </summary>
        /// <param name="startsWithTitle"></param>
        public void EagerLoading(string startsWithTitle)
        {
            var book = _booksContext.Books.Include(b => b.Author)
                .Include(b => b.Pages).Where(b => b.Title.StartsWith(startsWithTitle)).FirstOrDefault();
            if (book != null)
            {
                Console.WriteLine(book.Author.Name);
                foreach (var page in book.Pages)
                {
                    Console.WriteLine(page.Content);
                }
            }
        }
        public void UpdateRecords()
        {
            Book book = _booksContext.Books.Skip(1).FirstOrDefault();
            ShowState();
            book.Title += "UpdateRecords";
            ShowState();
            int records = _booksContext.SaveChanges();
            Console.WriteLine($"{records} updated");
            ShowState();
        }
        #region 保存

        /// <summary>
        /// 关联表的保存
        /// </summary>
        public void AddRecords()
        {
            var book = new Book()
            {
                Title = "SaveBook1(Tracker)",
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
                    Name = "SaveAuthor",
                    Address = new Address()
                    {
                        AddressDetail = "SaveAddressDetail"
                    }
                }
            };

            _booksContext.Books.Add(book);
            ShowState();
            int records = _booksContext.SaveChanges();
            Console.WriteLine($"{records} added");
        }

        /// <summary>
        /// DbContext.ChangeTracker.Entries 返回所有更改追踪器知道的所有对象
        /// </summary>
        /// <param name="context"></param>
        private void ShowState()
        {
            //ChangeTracker.Entries,returns all the objects the change tracker knows about. 
            foreach (EntityEntry entry in _booksContext.ChangeTracker.Entries())
            {
                Console.WriteLine($"type: {entry.Entity.GetType().Name}," +
                $"state: {entry.State}, {entry.Entity}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 对象追踪
        /// </summary>
        public void ObjectTracking()
        {
            var b1 = (from b in _booksContext.Books
                      where b.Title.StartsWith("Save")
                      select b).FirstOrDefault();
            var b2 = (from b in _booksContext.Books
                      where b.Title.Contains("(")
                      select b).FirstOrDefault();
            if (object.ReferenceEquals(b1, b2))
            {
                Console.WriteLine("相同对象");
            }
            else
            {
                Console.WriteLine("不同对象");

            }
            ShowState();
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
