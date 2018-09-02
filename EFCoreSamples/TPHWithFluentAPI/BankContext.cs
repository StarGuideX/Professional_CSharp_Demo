using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TPHWithFluentAPI
{
    public class BankContext : DbContext
    {
        /// <summary>
        /// discriminator的新名称是Type。 
        /// 这应该是Payment表中的一列，但它不应显示在Payment类型中。
        /// 字符串Cash和Creditcard应用于区分model类型。 
        /// 为了便于对字符串进行管理，定义了ColumnNames和ColumnValues类
        /// </summary>

        public static class ColumnNames
        {
            public const string Type = nameof(Type);
        }
        public static class ColumnValues
        {
            public const string Cash = nameof(Cash);
            public const string Creditcard = nameof(Creditcard);
        }

        /// <summary>
        /// 仅仅为了所有Payment类型定义一个Payments属性
        /// 使用HasDiscriminator方法指定TPH层次结构。
        /// discriminator的名称是Type，它也被指定为阴影属性。
        /// 派生类型的区别是使用HasValue方法指定的。 
        /// HasValue是从HasDiscriminator方法返回的DiscriminatorBuilder的方法。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 指定必须字段和Amount在数据库映射为Money类型
            modelBuilder.Entity<Payment>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("Money");
            // 为discriminator定义的阴影属性
            modelBuilder.Entity<Payment>().Property<string>(ColumnNames.Type);
            modelBuilder.Entity<Payment>()
            .HasDiscriminator<string>(ColumnNames.Type)
            .HasValue<CashPayment>(ColumnValues.Cash)
            .HasValue<CreditcardPayment>(ColumnValues.Creditcard);
        }
        public DbSet<Payment> Payments { get; set; }
    }
}
