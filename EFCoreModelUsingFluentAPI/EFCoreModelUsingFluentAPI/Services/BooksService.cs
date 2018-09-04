using EFCoreModelUsingFluentAPI.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreModelUsingFluentAPI.Services
{
    public class BooksService
    {
        private BooksContext _booksContext;
        public BooksService(BooksContext context) => _booksContext = context;

        public async Task CreateDatabaseAsync()
        {
            bool isCreated = await _booksContext.Database.EnsureCreatedAsync();
            string res = isCreated ? "创建完毕":"已创建";
            Console.WriteLine($"数据库创建：{res}");
        }

        public async Task DeleteDatabaseAsync() {
            bool isDeleted = await _booksContext.Database.EnsureDeletedAsync();
            string res = isDeleted ? "删除完毕" : "无此数据库";
            Console.WriteLine($"数据库删除：{res}");
        }

        //public async Task AddBookAsync() {
        //    _booksContext.Database

        //}
        //RelationUsingAnnotations----
        //MenusWithDataAnnotations----
        //intro
        //TableSplitting
        //OwnedEntities

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
