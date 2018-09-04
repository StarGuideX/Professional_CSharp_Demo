using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("Pages").HasKey(p=>p.PageId);
            builder.Property(p=>p.PageId).ValueGeneratedOnAdd();
            builder.HasOne(p=>p.Book).WithMany(b => b.Pages);

            builder.OwnsOne(p => p.TitleFont).OwnsOne<FontColor>(a => a.FontColor, tbuilder =>
            {
                tbuilder.Property(p => p.FontColorName).HasColumnName("TitleFontColorName");
            });
            builder.OwnsOne(p => p.TextFont).ToTable("TextFont").OwnsOne(a => a.FontColor);
        }
    }
}
