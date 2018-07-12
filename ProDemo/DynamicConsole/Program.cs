using DynamicConsole.DecompileSample;
using System;

namespace DynamicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic dyn;

            dyn = 100;
            Console.WriteLine(dyn.GetType());
            Console.WriteLine(dyn);

            dyn = "这是个字符串";
            Console.WriteLine(dyn.GetType());
            Console.WriteLine(dyn);

            dyn = new Person() { FirstName = "名", LastName = "姓" };
            Console.WriteLine(dyn.GetType());
            Console.WriteLine($"{dyn.LastName}{dyn.FirstName}");


            new DecompileDemo().DecompileStart();

            Console.ReadLine();
        }
    }
}
