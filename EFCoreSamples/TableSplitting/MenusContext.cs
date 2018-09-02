using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableSplitting
{
    public static class SchemaNames
    {
        public const string Menus = nameof(Menus);
    }

    /// <summary>
    /// 在上下文中，Menus和MenuDetails是两个DbSet属性。
    /// 在OnModelCreating方法中，Menu类与MenuDetails使用HasOne和WithOne配置为一对一关系。
    /// Menu和MenuDetails传递给ToTable方法的参数指定了相同的表名。 
    /// Menu和MenuDetails都映射到同一个表Menus。
    /// </summary>
    public class MenusContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasOne<MenuDetails>(m => m.Details)
                .WithOne(d => d.Menu)
                .HasForeignKey<MenuDetails>(d => d.MenuDetailsId);
            modelBuilder.Entity<Menu>().ToTable(SchemaNames.Menus);
            modelBuilder.Entity<MenuDetails>
            ().ToTable(SchemaNames.Menus);
        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuDetails> MenuDetails { get; set; }
    }
}
