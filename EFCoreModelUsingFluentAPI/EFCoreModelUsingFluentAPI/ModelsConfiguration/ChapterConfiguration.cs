using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters").HasKey(c => c.ChapterId);
            builder.Property(c => c.ChapterId).ValueGeneratedOnAdd();
            builder.Property(c => c.Title).IsRequired();
            //关联外键，级联删除和添加
            builder.HasOne(c => c.Book).WithMany(b => b.Chapters).HasForeignKey(c=>c.BookId);
        }
    }
}
