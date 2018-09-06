using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using static EFCoreModelUsingFluentAPI.Models.PageColumnNames;

namespace EFCoreModelUsingFluentAPI.Models
{
    internal class PageColumnNames
    {
        public const string LastUpdated = nameof(LastUpdated);
        public const string IsDeleted = nameof(IsDeleted);
        public const string PageId = nameof(PageId);
        //public const string AuthorId = nameof(AuthorId);
    }
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("Pages");
            // _pageId没有相应的属性,使用Property的重载方法，会将数据库中的PageId映射到字段_pageId
            builder.Property<int>(PageId).HasField("_pageId").IsRequired();
            builder.HasKey(PageId);
            builder.Property<int>(PageId).ValueGeneratedOnAdd();
            // 使用HasField方法将Remark属性映射到相应字段。
            builder.Property(b => b.Remark).HasField("_remark").IsRequired(false).HasMaxLength(100);
            builder.HasOne(p=>p.Book).WithMany(b => b.Pages);


            // 阴影属性
            // LastUpdated：实体最后更新的时间。 
            // IsDeleted：逻辑删除还是物理删除。逻辑删除可以撤消以恢复实体并提供历史信息。
            builder.Property<bool>(IsDeleted);
            builder.Property<DateTime>(LastUpdated);


            builder.OwnsOne(p => p.TitleFont).OwnsOne<FontColor>(t => t.FontColor, tbuilder =>
            {
                tbuilder.Property(p => p.FontColorName).HasColumnName("TitleFontColorName");
            });
            builder.OwnsOne(p => p.TextFont).ToTable("TextFont").OwnsOne(a => a.FontColor);
        }
    }
}
