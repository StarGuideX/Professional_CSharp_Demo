using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(u => u.UserId);
            builder.Property(u => u.UserId).ValueGeneratedOnAdd();
            builder.Property(u=>u.Name).IsRequired();
            #region 多个List<Book>的写法
            builder.HasMany(u => u.AuthoredBooks).WithOne(b => b.Author);
            builder.HasMany(u => u.EditedBooks).WithOne(b => b.Editor);
            #endregion
            #region 表拆分，一个数据库表分为两个实体User和Address
            // 在上下文中，User和Address是两个DbSet属性。
            // 在OnModelCreating方法中，User类与Address使用HasOne和WithOne配置为一对一关系。
            // User和Address传递给ToTable方法的参数指定了相同的表名。 
            // User和Address都映射到同一个表Users。
            //builder.ToTable("Users").HasKey(u => u.UserId); //需要本段代码，可与不设置主键，因为第一行有，所以不重复
            builder.HasOne(u => u.Address).WithOne(a => a.User).HasForeignKey<Address>(a => a.AddressId);
            #endregion
        }
    }
}
