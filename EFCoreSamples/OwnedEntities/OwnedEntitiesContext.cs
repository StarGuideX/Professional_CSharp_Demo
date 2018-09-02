using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OwnedEntities
{
    /// <summary>
    /// 
    /// </summary>
    public class OwnedEntitiesContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .OwnsOne(p => p.CompanyAddress)//本次调用OwnsOne，指定CompanyAddress为Person的从属实体。
                .OwnsOne<Location>(a => a.Location, builder =>
                {
                    builder.Property(p => p.City).HasColumnName("BusinessCity");
                    builder.Property(p => p.Country).HasColumnName("BusinessCountry");
                });// 本次OwnsOne的调用者为第一次OwnsOne调用完返回的ReferenceOwnershipBuilder<Person,Address>,
                   // 本次OwnsOne调用时，给Address的Location的两个属性映射为数据库中的BusinessCity和BusinessCountry。
            modelBuilder.Entity<Person>()
                .OwnsOne(p => p.PrivateAddress)//指定PrivateAddress为Person的从属实体。
                .ToTable("Addr") //映射到表名为Addr的表
                .OwnsOne(a => a.Location);//包含Location类中默认的列
        }
        public DbSet<Person> People { get; set; }
    }
}
