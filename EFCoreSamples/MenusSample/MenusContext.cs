using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MenusSample
{
    public class MenusContext:DbContext
    {
        private const string ConnectionString = @"server=(localdb)\MSSQLLocalDb;" + "Database=MenuCards;Trusted_Connection=True";
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuCard> MenuCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 配置Schema
            modelBuilder.HasDefaultSchema("mc");
            // model(MenuCard)映射到表(MenuCards)定义键和索引
            modelBuilder.Entity<MenuCard>().ToTable("MenuCards").HasKey(c => c.MenuCardId);
            // Propert用来配置属性，返回PropertyBuilder。ValueGeneratedOnAdd自动生成值、HasMaxLength最大长度。
            modelBuilder.Entity<MenuCard>().Property(c => c.MenuCardId).ValueGeneratedOnAdd();
            modelBuilder.Entity<MenuCard>().Property(c => c.Title).HasMaxLength(50);

            modelBuilder.Entity<Menu>().ToTable("Menus").HasKey(m => m.MenuId);
            modelBuilder.Entity<Menu>().Property(m => m.MenuId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Menu>().Property(m => m.Text).HasMaxLength(120);
            modelBuilder.Entity<Menu>().Property(m => m.Price).HasColumnType("Money");

            modelBuilder.Entity<MenuCard>().HasMany(c => c.Menus).WithOne(m => m.MenuCard);
            modelBuilder.Entity<Menu>().HasOne(m => m.MenuCard).WithMany(c => c.Menus).HasForeignKey(m => m.MenuCardId);

            //modelBuilder.HasDefaultSchema("mc");
            //modelBuilder.ApplyConfiguration(new MenuCardConfiguration());
            //modelBuilder.ApplyConfiguration(new MenuConfiguration());
        }
    }
}
