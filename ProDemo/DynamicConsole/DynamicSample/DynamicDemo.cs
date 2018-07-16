using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicConsole.DynamicSample
{
    public class DynamicDemo
    {
        public void DynamicDemoStart()
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
        }
    }
}
