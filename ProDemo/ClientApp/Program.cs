using System;
using System.Reflection;
using System.Runtime.Loader;

namespace ClientApp
{
    class Program
    {
        /// <summary>
        /// 库的路径
        /// </summary>
        private const string CalculatorLibPath = @"C:/addins/CalculatorLib.dll";
        /// <summary>
        /// 程序集名称
        /// </summary>
        private const string CalculatorLibName = "CalculatorLib";
        /// <summary>
        /// Caculator类的名称
        /// </summary>
        private const string CalculatorTypeName = "CalculatorLib.Calculator";

        static void Main(string[] args)
        {
            ReflectionOld();
            ReflectionNew();

            Console.ReadLine();
        }

        private static void ReflectionNew()
        {
            double x = 3;
            double y = 4;
            dynamic calc = GetCalculator(CalculatorLibPath);
            double result = calc.Add(x,y);
            Console.WriteLine($"{x}加{y}结果:{result}—dynamic");

        }

        private static void ReflectionOld()
        {
            double x = 3;
            double y = 4;
            object calc = GetCalculator(CalculatorLibPath);
            object result = calc.GetType().GetMethod("Add").Invoke(calc, new object[] { x, y });

            Console.WriteLine($"{x}加{y}结果:{result}—object");
        }

#if NET461
        private static object GetCalculator(string addinPath)
        {
            Assembly assembly = Assembly.LoadFile(addinPath);
            return assembly.CreateInstance(CalculatorTypeName);
        }
#else
        /// <summary>
        /// 实例化Calculator
        /// </summary>
        /// <param name="addinPath"></param>
        /// <returns></returns>
        private static object GetCalculator(string addinPath)
        {
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(addinPath);
            Type type = assembly.GetType(CalculatorTypeName);
            return Activator.CreateInstance(type);
        }
#endif
    }
}
