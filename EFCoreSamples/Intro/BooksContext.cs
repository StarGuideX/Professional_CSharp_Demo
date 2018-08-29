using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intro
{
public class BooksContext : DbContext
{
    private const string ConnectionString = 
        @"server=(localdb)\MSSQLLocalDb;database=WroxBooks;" + 
        @"trusted_connection=true";
    /// <summary>
    /// 此类型允许查询和添加
    /// </summary>
    public DbSet<Book> Books { get; set; }

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
