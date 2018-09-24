using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MigrationsLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreMigrations
{
    public class MenusContextFactory : IDesignTimeDbContextFactory<MenusContext>
    {
        private const string ConnectionString = 
            @"server= (localdb)\mssqllocaldb;
            database=EFCoreMigrations;trusted_connection=true";
        public MenusContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<MenusContext>();
            optionBuilder.UseSqlServer(ConnectionString);
            return new MenusContext(optionBuilder.Options);
        }
    }
}
