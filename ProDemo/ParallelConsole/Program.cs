using ParallelConsole.ParallelSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ParallelSampleClass psc = new ParallelSampleClass();
            //ParallelFor
            //psc.ParallelFor();
            //ParallelForWithAsync
            psc.ParallelForWithAsync();
            Console.Read();
        }
    }
}
