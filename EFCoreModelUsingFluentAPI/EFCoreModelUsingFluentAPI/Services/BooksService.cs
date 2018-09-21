using EFCoreModelUsingFluentAPI.Contexts;
using EFCoreModelUsingFluentAPI.Extensions;
using EFCoreModelUsingFluentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <summary>
        /// 对象的更新状态变化
        /// </summary>
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

        /// <summary>
        /// 更新未跟踪的对象
        /// </summary>
        public void ChangeUntracked()
        {
            Book GetBook()
            {
                return _booksContext.Books.Skip(2).FirstOrDefault();
            }

            Book b = GetBook();
            b.Title += "ChangeUntracked";
            UpdateUntracked(b);

        }
        /// <summary>
        /// 更新未跟踪的对象
        /// </summary>
        /// <param name="b"></param>
        private void UpdateUntracked(Book b)
        {
            ShowState();
            // UpdateUntracked方法接收更新的对象，将其与context关联
            // 第一种方式Attach对象，并设置EntityState
            // EntityEntry<Book> entity = _booksContext.Books.Attach(b);
            // entity.State = EntityState.Modified;
            // 使用Update可以自动完成以上注释的语句
            _booksContext.Books.Update(b);
            ShowState();
            _booksContext.SaveChanges();
        }

        /// <summary>
        /// 创建了100个Book对象,并写入数据库
        /// </summary>
        public void AddHundredRecords()
        {

            var books = Enumerable.Range(1, 100).Select(x =>
             new Book
             {
                 Title = "AddHundredRecordsssss",
             });
            _booksContext.Books.AddRange(books);
            Stopwatch stopwatch = Stopwatch.StartNew();
            int records = _booksContext.SaveChanges();
            stopwatch.Stop();
            Console.WriteLine($"{records} records added after " + $"{stopwatch.ElapsedMilliseconds} milliseconds");
        }
        #endregion

        #region 冲突处理
        private const string BookTitle = "sample book";
        private const string ConnectionString = @"server=(localdb)\MSSQLLocalDb;database=EFCoreDemoFluentAPI;trusted_connection=true";

        #region 保留最后一条
        /// <summary>
        /// 最后一条更改为最终更改（数据库数据更改为最后一条语句）—默认
        /// </summary>
        public void ConflictHandling()
        {
            var options = new DbContextOptionsBuilder<BooksContext>();
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(ConnectionString);
            // 准备初始数据
            void PrepareBook()
            {
                using (var context = new BooksContext(options.Options))
                {
                    context.Books.Add(new Book() { Title = BookTitle });
                    context.SaveChanges();
                }
            }

            PrepareBook();

            // user 1
            var tuple1 = PrepareUpdate();
            tuple1.book.Title = "用户1更新了这条";

            // user 2
            var tuple2 = PrepareUpdate();
            tuple2.book.Title = "用户2更新了这条";

            Update(tuple1.context, tuple1.book, "用户1");
            Update(tuple2.context, tuple2.book, "用户2");

            tuple1.context.Dispose();
            tuple2.context.Dispose();

            CheckUpdate(tuple1.book.BookId);
        }

        /// <summary>
        /// 返回一个元组，包括context和book。
        /// 此方法被调用两次，并返回与不同上下文对象关联的不同Book对象
        /// </summary>
        /// <returns></returns>
        private (BooksContext context, Book book) PrepareUpdate()
        {
            var options = new DbContextOptionsBuilder<BooksContext>();
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(ConnectionString);
            var context = new BooksContext(options.Options);
            Book book = context.Books.Where(b => b.Title == BookTitle).FirstOrDefault();
            return (context, book);
        }

        /// <summary>
        /// 将具有指定ID的书籍写入控制台
        /// </summary>
        /// <param name="id"></param>
        private void CheckUpdate(int id)
        {
            var options = new DbContextOptionsBuilder<BooksContext>();
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(ConnectionString);

            using (var context = new BooksContext(options.Options))
            {
                Book book = context.Books.Find(id);
                Console.WriteLine($"this is the updated state: {book.Title}");
            }
        }
        /// <summary>
        /// 接受已打开的BooksContext和更新状态的Book，并将book保存至数据库。
        /// 此方法也会被调用两次
        /// </summary>
        /// <param name="context"></param>
        /// <param name="book"></param>
        /// <param name="user"></param>
        private void Update(BooksContext context, Book book, string user)
        {
            int records = context.SaveChanges();
            Console.WriteLine($"{user}: {records} record updated from {user}");
        }

        //private void ShowChanges(int id, EntityEntry entity)
        //{
        //    void ShowChange(PropertyEntry propertyEntry) =>
        //        Console.WriteLine($"id: {id}, current: {propertyEntry.CurrentValue}, original: {propertyEntry.OriginalValue}, modified: {propertyEntry.IsModified}");

        //    ShowChange(entity.Property("Title"));
        //    ShowChange(entity.Property("Publisher"));
        //}
        #endregion

        #region 保留第一条
        /// <summary>
        /// 第一条更改为最终更改
        /// </summary>
        public void ConflictHandingFirst()
        {
            var options = new DbContextOptionsBuilder<BooksContext>();
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(ConnectionString);
            // 准备初始数据
            void PrepareBook()
            {
                using (var context = new BooksContext(options.Options))
                {
                    context.Books.Add(new Book() { Title = BookTitle });
                    context.SaveChanges();
                }
            }
            PrepareBook();


            (BooksContext context, Book book) PrepareUpdateFirst()
            {

                var pOptions = new DbContextOptionsBuilder<BooksContext>();
                pOptions.EnableSensitiveDataLogging();
                pOptions.UseSqlServer(ConnectionString);
                var context = new BooksContext(pOptions.Options);
                Book book = context.Books.Where(b => b.Title == BookTitle).FirstOrDefault();
                return (context, book);
            }

            // user 1
            var tuple1 = PrepareUpdateFirst();
            tuple1.book.Title = "用户1更新了这条";

            // user 2
            var tuple2 = PrepareUpdateFirst();
            tuple2.book.Title = "用户2更新了这条";

            UpdateFirst(tuple1.context, tuple1.book, "用户1");
            UpdateFirst(tuple2.context, tuple2.book, "用户2");

            tuple1.context.Dispose();
            tuple2.context.Dispose();

            CheckUpdateFirst(tuple1.book.BookId);
        }


        private void UpdateFirst(BooksContext context, Book book, string user)
        {
            try
            {
                Console.WriteLine($"{user}: 更新中: id {book.BookId}, timestamp {book.TimeStamp.StringOutput()}");
                ShowChangesFirst(book.BookId, context.Entry(book));
                int records = context.SaveChanges();
                Console.WriteLine($"{user}: 已更新 {book.TimeStamp.StringOutput()}");
                Console.WriteLine($"{user}: 已更新 {records} 条 updated while updating {book.Title}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"{user}: 使用{book.Title}更新失败");
                Console.WriteLine($"{user}: error: {ex.Message}");
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Book b)
                    {
                        Console.WriteLine($"{b.Title} {b.TimeStamp.StringOutput()}");
                        ShowChangesFirst(book.BookId, context.Entry(book));
                    }
                }
            }

        }

        private void ShowChangesFirst(int id, EntityEntry entity)
        {
            //多次调用
            void ShowChange(PropertyEntry propertyEntry) =>
                Console.WriteLine($"id:{id},CurrentValue：{propertyEntry.CurrentValue}," +
                $"original: {propertyEntry.OriginalValue}, modified: {propertyEntry.IsModified}");

            ShowChange(entity.Property("Title"));
        }

        private static void CheckUpdateFirst(int id)
        {
            var options = new DbContextOptionsBuilder<BooksContext>();
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(ConnectionString);

            using (var context = new BooksContext(options.Options))
            {
                Book book = context.Books.Find(id);
                Console.WriteLine($"已更新状态: {book.Title}");
            }
        }
        #endregion

        #endregion

        #region 事务
        /// <summary>
        /// 隐式事务，添加失败
        /// </summary>
        public void AddTwoRecordsWithOneTx()
        {
            try
            {
                Book book = _booksContext.Books.First();

                var chapter = new Chapter()
                {
                    BookId = book.BookId,
                    Title = "TxChapterAdded",
                    Number = 1
                };
                // 取得现有book表中最后一条id
                int hightestId = _booksContext.Books.Max(c => c.BookId);
                var pInvalid = new Chapter
                {
                    //BookId赋予了一个无效的id
                    BookId = ++hightestId,
                    Title = "invalid"
                };
                _booksContext.Chapters.AddRange(chapter, pInvalid);
                int records = _booksContext.SaveChanges();
                Console.WriteLine($"添加了{records}条");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 多次调用savechanges
        /// </summary>
        public void AddTwoRecordsWithTwoTx()
        {
            try
            {
                Book book = _booksContext.Books.First();
                var chapter = new Chapter()
                {
                    BookId = book.BookId,
                    Title = "TxChapterAdded",
                    Number = 1
                };
                _booksContext.Chapters.Add(chapter);
                int records = _booksContext.SaveChanges();
                Console.WriteLine($"添加了{records}条");
                // 取得现有book表中最后一条id
                int hightestId = _booksContext.Books.Max(c => c.BookId);
                var pInvalid = new Chapter
                {
                    //BookId赋予了一个无效的id
                    BookId = ++hightestId,
                    Title = "invalid"
                };
                _booksContext.Chapters.Add(pInvalid);
                records = _booksContext.SaveChanges();
                Console.WriteLine($"添加了{records}条");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 显示事务
        /// </summary>
        /// <returns></returns>
        public void  TwoSaveChangesWithOneTx()
        {
            IDbContextTransaction tx = null;
            try
            {
                using (tx =  _booksContext.Database.BeginTransaction())
                {
                    Book book = _booksContext.Books.First();
                    var chapter = new Chapter()
                    {
                        BookId = book.BookId,
                        Title = "TxChapterAdded",
                        Number = 1
                    };
                    _booksContext.Chapters.Add(chapter);
                    int records = _booksContext.SaveChanges();
                    Console.WriteLine($"添加了{records}条");
                    // 取得现有book表中最后一条id
                    int hightestId = _booksContext.Books.Max(c => c.BookId);
                    var pInvalid = new Chapter
                    {
                        //BookId赋予了一个无效的id
                        BookId = ++hightestId,
                        Title = "invalid"
                    };
                    _booksContext.Chapters.Add(pInvalid);
                    records = _booksContext.SaveChanges();
                    Console.WriteLine($"添加了{records}条");
                    tx.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
                Console.WriteLine("rolling back…");
                tx.Rollback();
            }
            Console.WriteLine();
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
