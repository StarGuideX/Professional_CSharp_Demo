using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TransactionsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 隐式事务，添加失败
        /// </summary>
        private static void AddTwoRecordsWithOneTx()
        {
            Console.WriteLine(nameof(AddTwoRecordsWithOneTx));
            try
            {
                using (var context = new MenusContext())
                {
                    var card = context.MenuCards.First();
                    var m1 = new Menu
                    {
                        MenuCardId = card.MenuCardId,
                        Text = "added",
                        Price = 99.99m
                    };
                    // hightestCardId为数据库可用的、最大的ID
                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    var mInvalid = new Menu
                    {
                        //比最大的ID还+1，就引用了无效的MenuCard
                        MenuCardId = ++hightestCardId,
                        Text = "invalid",
                        Price = 999.99m
                    };
                    context.Menus.AddRange(m1, mInvalid);
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }

        private static void AddTwoRecordsWithTwoTx()
        {
            Console.WriteLine(nameof(AddTwoRecordsWithTwoTx));
            try
            {
                using (var context = new MenusContext())
                {
                    var card = context.MenuCards.First();
                    var m1 = new Menu
                    {
                        MenuCardId = card.MenuCardId,
                        Text = "added",
                        Price = 99.99m
                    };
                    context.Menus.Add(m1);
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");
                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    var mInvalid = new Menu
                    {
                        MenuCardId = ++hightestCardId,
                        Text = "invalid",
                        Price = 999.99m
                    };
                    context.Menus.Add(mInvalid);
                    records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }

        private static async Task TwoSaveChangesWithOneTxAsync()
        {
            Console.WriteLine(nameof(TwoSaveChangesWithOneTxAsync));
            IDbContextTransaction tx = null;
            try
            {
                using (var context = new MenusContext())
                using (tx = await context.Database.BeginTransactionAsync())
                {
                    var card = context.MenuCards.First();
                var m1 = new Menu
                {
                    MenuCardId = card.MenuCardId,
                    Text = "added with explicit tx",
                    Price = 99.99m
                };
                    context.Menus.Add(m1);
                    int records = await context.SaveChangesAsync();
                    Console.WriteLine($"{records} records added");
                    int hightestCardId = context.MenuCards.Max(c =>
                    c.MenuCardId);
                    var mInvalid = new Menu
                    {
                        MenuCardId = ++hightestCardId,
                        Text = "invalid",
                        Price = 999.99m
                    };
                    context.Menus.Add(mInvalid);
                    records = await context.SaveChangesAsync();
                    Console.WriteLine($"{records} records added");
                    tx.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
                Console.WriteLine("rolling back…");
                tx.Rollback();
            }
            Console.WriteLine();
        }
    }
}
