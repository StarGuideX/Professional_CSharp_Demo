using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicConsole.DecompileSample
{
    public class DecompileDemo
    {
        public void DecompileStart()
        {
            StaticClass staticClass = new StaticClass();
            DynamicClass dynamicClass = new DynamicClass();
            Console.WriteLine(staticClass.IntValue);
            Console.WriteLine(dynamicClass.DynValue);
        }

        class StaticClass
        {
            public int IntValue = 100;
        }

        class DynamicClass
        {
            public dynamic DynValue = 100;
        }


    }
}
