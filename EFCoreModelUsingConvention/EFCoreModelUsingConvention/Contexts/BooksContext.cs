using EFCoreModelUsingConvention.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingConvention.Contexts
{
    public class BooksContext :DbContext
    {
        private const string ConnectionString = @"server=(localdb)\MSSQLLocalDb;database=EFCoreDemo;trusted_connection=true";

        /// <summary>
        /// get:允许查询
        /// set:允许添加
        /// </summary>
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User> Pages { get; set; }
        /// <summary>
        /// 重写DbContext的OnConfiguring方法可以定义连接字符串。
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            //UseSqlServer扩展方法将上下文映射到SQL Server数据库。
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
