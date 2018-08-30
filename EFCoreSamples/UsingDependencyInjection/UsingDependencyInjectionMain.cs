using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsingDependencyInjection
{
    public class UsingDependencyInjectionMain
    {
        public ServiceProvider Container { get; private set; }

        public void InitializeServices() {
            const string ConnectionString = @"server=(localdb)\MSSQLLocalDb;database=Books;trusted_connection=true";

            var services = new ServiceCollection();
            services.AddTransient<BooksService>().AddEntityFrameworkSqlServer()
                .AddDbContext<BooksContext>(options => options.UseSqlServer(ConnectionString));

            Container = services.BuildServiceProvider();
        }

    }
}
