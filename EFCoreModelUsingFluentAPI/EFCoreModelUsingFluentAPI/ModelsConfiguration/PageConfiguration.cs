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
        }
    }
}
