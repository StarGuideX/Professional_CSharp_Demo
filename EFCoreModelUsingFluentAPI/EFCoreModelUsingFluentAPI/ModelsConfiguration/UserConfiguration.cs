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
        }
    }
}
