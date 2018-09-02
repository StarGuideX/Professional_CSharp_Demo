using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RelationUsingConventions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 显示加载
        /// 查询指定标题的一本书。
        /// 如果开始查询后访问Chapters和Author属性，则会为null。关系不会隐式加载。
        /// EF Core通过使用上下文的Entry方法（在参数中传递实体）返回EntityEntry对象支持显式加载。
        /// 使用EntityEntry的Collection（一对多）和Reference（一对一）方法进行显示加载。再使用Load()。
        /// </summary>

        private static void ExplicitLoading()
        {
            Console.WriteLine(nameof(ExplicitLoading));
            using (var context = new BooksContext())
            {
                var book = context.Books
                .Where(b => b.Title.StartsWith("Professional C# 7"))
                .FirstOrDefault();
                if (book != null)
                {
                    Console.WriteLine(book.Title);
                    context.Entry(book).Collection(b => b.Chapters).Load();
                    context.Entry(book).Reference(b => b.Author).Load();
                    Console.WriteLine(book.Author.Name);
                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}.{ chapter.Title}");
                    }
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 立即加载
        /// </summary>
        private static void EagerLoading()
        {
            Console.WriteLine(nameof(EagerLoading));
            using (var context = new BooksContext())
            {
                var book = context.Books
                .Include(b => b.Chapters)
                .Include(b => b.Author)
                .Where(b => b.Title.StartsWith("Professional C# 7"))
                .FirstOrDefault();
            if (book != null)
                {
                    Console.WriteLine(book.Title);
                    foreach (var chapter in book.Chapters)
                    {
                        Console.WriteLine($"{chapter.Number}.{ chapter.Title}");
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
