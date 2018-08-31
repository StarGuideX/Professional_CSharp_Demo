using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UsingDependencyInjection
{
    public class UsingDependencyInjectionMain
    {
        public async Task StartAsync()
        {
            this.InitializeServices();
            this.ConfigureLogging();
            var service = this.Container.GetService<BooksService>();
            await service.AddBooksAsync();
            await service.ReadBooksAsync();
        }


        public ServiceProvider Container { get; private set; }

        /// <summary>
        /// 初始化依赖注入的Container
        /// </summary>
        public void InitializeServices() {
            const string ConnectionString = @"server=(localdb)\MSSQLLocalDb;database=Books;trusted_connection=true";

            var services = new ServiceCollection();
            // AddTransient：每次请求服务时都会实例化BooksService，并添加至ServiceCollection
            // AddEntityFrameworkSqlServer：注册EF和Sqlserver
            services.AddTransient<BooksService>().AddEntityFrameworkSqlServer()
                .AddDbContext<BooksContext>(options => options.UseSqlServer(ConnectionString));
            // 对应用程序配置logging,通过注入服务使用logging
            services.AddLogging();

            Container = services.BuildServiceProvider();
        }

        /// <summary>
        /// 配置logging，将信息输出在控制台
        /// </summary>
        public void ConfigureLogging()
        {
            ILoggerFactory loggerFactory = Container.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Information);
        }
    }
}
