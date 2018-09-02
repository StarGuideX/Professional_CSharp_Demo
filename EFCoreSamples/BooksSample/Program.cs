using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BooksSample.ColumnNames;

namespace BooksSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        private async Task AddBooksAsync()
        {
            using (var context = new BooksContext())
            {
                var b1 = new Book("Professional C# 6 and .NET Core 1.0","Wrox Press");
                var b2 = new Book("Professional C# 5 and .NET 4.5.1", "Wrox Press");
                var b3 = new Book("JavaScript for Kids", "Wrox Press");
                var b4 = new Book("Web Design with HTML and CSS", "For Dummies");
                await context.Books.AddRangeAsync(b1, b2, b3, b4);
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"{records} records added");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 删除、有阴影属性isdeleted会设为true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task DeleteBookAsync(int id)
        {
            using (var context = new BooksContext())
            {
                Book b = await context.Books.FindAsync(id);
                if (b == null) return;
                context.Books.Remove(b);
                int records = await context.SaveChangesAsync();
                Console.WriteLine($"{records} books deleted");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 验证DeleteBookAsync方法
        /// </summary>
        /// <returns></returns>
        private async Task QueryDeletedBooksAsync()
        {
            using (var context = new BooksContext())
            {
                IEnumerable<Book> deletedBooks =
                await context.Books
                .Where(b => EF.Property<bool>(b, IsDeleted))
                .ToListAsync();
                foreach (var book in deletedBooks)
            {
                    Console.WriteLine($"deleted: {book}");
                }
            }
        }
    }
}
