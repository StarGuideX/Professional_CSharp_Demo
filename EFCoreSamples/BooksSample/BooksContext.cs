using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BooksSample.ColumnNames;

namespace BooksSample
{
    /// <summary>
    /// 为了避免多次使用这些字符串时出现拼写错误，定义了一个定义const字符串的类
    /// </summary>
    internal class ColumnNames
    {
        /// <summary>
        /// 阴影属性
        /// </summary>
        public const string LastUpdated = nameof(LastUpdated);
        /// <summary>
        /// 阴影属性
        /// </summary>
        public const string IsDeleted = nameof(IsDeleted);
        public const string BookId = nameof(BookId);
        public const string AuthorId = nameof(AuthorId);
    }


    class BooksContext:DbContext
    {
        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //过滤查询
            modelBuilder.Entity<Book>().HasQueryFilter(b => !EF.Property<bool>(b, IsDeleted));

            modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(50);
            // 使用HasField方法将Publisher属性映射到相应字段。
            modelBuilder.Entity<Book>().Property(b => b.Publisher).HasField("_publisher").IsRequired(false).HasMaxLength(30);
            // _bookId没有相应的属性,使用Property的重载方法，会将数据库中的BookId映射到字段_bookId
            modelBuilder.Entity<Book>().Property<int>(BookId).HasField("_bookId").IsRequired();
            modelBuilder.Entity<Book>().HasKey(BookId);

            // 阴影属性
            // LastUpdated：实体最后更新的时间。 
            // IsDeleted：逻辑删除还是物理删除。逻辑删除可以撤消以恢复实体并提供历史信息。
            modelBuilder.Entity<Book>().Property<bool>(IsDeleted);
            modelBuilder.Entity<Book>().Property<DateTime>(LastUpdated);

            }
        /// <summary>
        /// 重写方法 SaveChangesAsync 以自动更新阴影属性LastUpdated、并管理IsDeleted属性（同步方法SaveChanges也要重写）
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();

            // 如果状态显示添加、修改、删除，则会使用当前时间更新阴影属性LastUpdated
            // 
            foreach (var item in ChangeTracker.Entries<Book>()
                .Where(e => 
                e.State == EntityState.Added || 
                e.State == EntityState.Modified || 
                e.State == EntityState.Deleted))
            {
                //使用EntityEntry的CurrentValues索引器访问模型中没有的阴影属性
                item.CurrentValues[LastUpdated] = DateTime.Now;
                // 将实体的状态由删除状态改为修改状态，并将IsDeleted属性设为true
                if (item.State == EntityState.Deleted)
                {
                    item.State = EntityState.Modified;
                    item.CurrentValues[IsDeleted] = true;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges() => SaveChangesAsync().Result;
    }
}
