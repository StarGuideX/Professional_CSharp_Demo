using System;
using System.Threading.Tasks;

namespace Intro
{
    class Program
    {
        static async Task Main()
        {
            var p = new Program();
            await p.CreateTheDatabaseAsync();
            Console.Read();
        }

        private async Task CreateTheDatabaseAsync()
        {
            using (var context = new BooksContext())
            {
                bool created = await context.Database.EnsureCreatedAsync();
                string creationInfo = created ? "created" : "exists";
                Console.WriteLine($"数据库{creationInfo}");
            }
        }
    }
}
