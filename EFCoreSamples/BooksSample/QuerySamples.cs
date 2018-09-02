using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSample
{
    public class QuerySamples
    {
        private async Task QueryAllBooksAsync()
        {
            Console.WriteLine(nameof(QueryAllBooksAsync));
            using (var context = new BooksContext())
            {
                List<Book> books = await context.Books.ToListAsync();              
                foreach (var b in books)
                {
                    Console.WriteLine(b);
                }
                
                // 使用异步API时，可以使用从ToAsyncEnumerable方法返回的IAsyncEnumerable，并使用ForEachAsync
                //await context.Books.ToAsyncEnumerable().ForEachAsync(b =>
                //{
                //    Console.WriteLine(b);
                //});
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 原始Sql查询
        /// </summary>
        /// <param name="publisher"></param>
        /// <returns></returns>
        private async Task RawSqlQuery(string publisher)
        {
            Console.WriteLine(nameof(RawSqlQuery));
            using (var context = new BooksContext())
            {
                IList<Book> books = await context.Books.FromSql(
                $"SELECT * FROM Books WHERE Publisher = {publisher}")
                .ToListAsync();
                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 编译查询
        /// </summary>
        private void CompiledQuery()
        {
            Console.WriteLine(nameof(CompiledQuery));
            Func<BooksContext, string, IEnumerable<Book>> query = 
                EF.CompileQuery<BooksContext, string, Book>((context,publisher) =>
                context.Books.Where(b => b.Publisher == publisher));
            using (var context = new BooksContext())
            {
                IEnumerable<Book> books = query(context, "Wrox Press");
                foreach (var b in books){
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// EF.Functions
        /// 通过使用EF.Functions.Like增强了Where方法的查询，并提供包含参数titleSegment的表达式。 
        /// 参数titleSegment嵌入在两个％字符内
        /// </summary>
        /// <param name="titleSegment"></param>
        /// <returns></returns>
        public static async Task UseEFCunctions(string titleSegment)
        {
            Console.WriteLine(nameof(UseEFCunctions));
            using (var context = new BooksContext())
            {
                string likeExpression = $"%{titleSegment}%";
                IList<Book> books = await context.Books.Where(
                b => EF.Functions.Like(b.Title,
                likeExpression)).ToListAsync();
                foreach (var b in books)
                {
                    Console.WriteLine($"{b.Title} {b.Publisher}");
                }
            }
            Console.WriteLine();
        }
    }
}
