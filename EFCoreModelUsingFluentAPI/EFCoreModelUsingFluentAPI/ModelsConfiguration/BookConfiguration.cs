using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    /// <summary>
    /// 鉴别器列的类型
    /// </summary>
    public static class ColumnNames
    {
        public const string Type = nameof(Type);
    }
    /// <summary>
    /// 鉴别器列的值
    /// </summary>
    public static class ColumnValues
    {
        public const string MinorBook = nameof(MinorBook);
        public const string AdultBook = nameof(AdultBook);
    }


    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            //主键
            builder.ToTable("Books").HasKey(b => b.BookId);
            //自增
            builder.Property(b => b.BookId).ValueGeneratedOnAdd();
            builder.Property(b => b.Title).HasMaxLength(50);

            //关系
            builder.HasMany(b => b.Chapters).WithOne(c => c.Book);
            builder.HasMany(b => b.Pages).WithOne(p => p.Book);

            #region 多个User类型的写法
            builder.HasOne(b => b.Author).WithMany(u => u.AuthoredBooks);
            builder.HasOne(b => b.Editor).WithMany(u => u.EditedBooks);
            #endregion

            #region TPH(Table per Hierarchy)形成多种书籍
            builder.Property<string>(ColumnNames.Type); //鉴别器的阴影状态
            // 为鉴别器添加值
            builder.HasDiscriminator<string>(ColumnNames.Type)
                .HasValue<AdultBook>(ColumnValues.AdultBook)
                .HasValue<MinorBook>(ColumnValues.MinorBook);
            #endregion


            //builder.Property<DateTime>("LastUpdated");
            //builder.Property<bool>("IsDeleted");
        }
    }
}
