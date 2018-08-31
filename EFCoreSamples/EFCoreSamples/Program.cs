using System;
using System.Threading.Tasks;
using UsingDependencyInjection;

namespace EFCoreSamples
{
    class Program
    {
        static async Task MainAsync(string[] args)
        {
            UsingDependencyInjectionMain usingDependencyInjectionMain = new UsingDependencyInjectionMain();
            await usingDependencyInjectionMain.StartAsync();
            Console.ReadLine();
        }
    }
}
