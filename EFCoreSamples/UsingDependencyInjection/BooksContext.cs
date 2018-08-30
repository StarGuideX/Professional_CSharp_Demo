using Microsoft.EntityFrameworkCore;
using System;

namespace UsingDependencyInjection
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
