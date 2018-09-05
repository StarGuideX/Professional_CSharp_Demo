using EFCoreModelUsingFluentAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EFCoreModelUsingFluentAPI.Models.PageColumnNames;

namespace EFCoreModelUsingFluentAPI.Contexts
{
    public class BooksContext : DbContext
    {
        /// <summary>
        /// 使用依赖注入
        /// </summary>
        /// <param name="options"></param>
        public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Book> Books { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("fluent");
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new PageConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
        }

        /// <summary>
        /// 重写方法 SaveChangesAsync 以自动更新阴影属性LastUpdated、并管理IsDeleted属性（同步方法SaveChanges也要重写）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();
            if (ChangeTracker.Entries().FirstOrDefault().GetType().Equals(typeof(Page)))
            {
                // 如果状态显示添加、修改、删除，则会使用当前时间更新阴影属性LastUpdated
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
            }
           

            
            

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
