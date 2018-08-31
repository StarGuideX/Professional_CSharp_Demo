using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UsingDependencyInjection
{
    public class BooksService
    {
        private readonly BooksContext _booksContext;
        public BooksService(BooksContext context) => _booksContext = context;

        public async Task AddBooksAsync()
        {
            var book1 = new Book
            {
                Title = "title1",
                Publisher = "publisher1"
            };
            var book2 = new Book
            {
                Title = "title2",
                Publisher = "publisher2"
            };
            var book3 = new Book
            {
                Title = "title3",
                Publisher = "publisher3"
            };

            await _booksContext.Books.AddRangeAsync(book1, book2, book3);
            int records = await _booksContext.SaveChangesAsync();
            Console.WriteLine($"添加了{records}条");
        }

        public async Task ReadBooksAsync()
        {
            List<Book> books = await _booksContext.Books.ToListAsync();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var b in books)
            {
                Console.WriteLine($"{b.Title} {b.Publisher}");
            }
        }
    }
}
