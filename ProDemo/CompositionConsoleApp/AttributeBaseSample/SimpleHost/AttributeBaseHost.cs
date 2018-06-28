using CompositionConsoleApp.AttributeBaseSample.CalculatorContract;
using CompositionConsoleApp.AttributeBaseSample.SimpleCalculator;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Text;

namespace CompositionConsoleApp.AttributeBaseSample.SimpleHost
{

    /// <summary>
    /// 宿主应用程序是一个简单的控制台应用程序（包）。
    /// 部件使用Export特性定义导出的内容，对于宿主应用程序，Import特性定义了所使用的信息
    /// </summary>
    public class AttributeBaseHost
    {
        /// <summary>
        /// Import特性注解了Calculator属性，以设置和获取实现ICalculator接口的对象。
        /// 因此实现了这个接口的任意计算器插件都可以在这里使用。
        /// </summary>
        [Import]
        public ICalculator Calculator { get; set; }

        /// <summary>
        /// 创建了一个ContainerConfiguration，有了ContainerConfiguration，就可以使用流利的API配置这个对象。
        /// 方法WithPart<Calculator> 导出Calculator类，可以用于CompositionHost。
        /// CompositionHost实例使用ContainerConfiguration的CreateContainer方法创建
        /// </summary>
        public void AttributeBaseStart()
        {
            Bootstrapper();
            Run();
        }

        private void Bootstrapper()
        {
            var configuration = new ContainerConfiguration().WithPart<Calculator>();
            using (CompositionHost host = configuration.CreateContainer())
            {
                //Calculator = host.GetExport<ICalculator>();
                host.SatisfyImports(this);
            }
        }

        private void Run()
        {
            var operations = Calculator.GetOperations();
            var operationsDict = new SortedList<string, IOperation>();
            foreach (var item in operations)
            {
                Console.WriteLine($"Name：{item.Name},number opsrands：{item.NumberOperands}");
                operationsDict.Add(item.Name, item);
            }
            Console.WriteLine();
            string selectedOp = null;
            do
            {
                try
                {
                    Console.Write("Operation? ");
                    selectedOp = Console.ReadLine();
                    if (selectedOp.ToLower() == "exit" || !operationsDict.ContainsKey(selectedOp))
                        continue;
                    var operation = operationsDict[selectedOp];
                    double[] operands = new double[operation.NumberOperands];
                    for (int i = 0; i < operation.NumberOperands; i++)
                    {
                        Console.Write($"\t operand{i + 1} ? ");
                        string selectedOperand = Console.ReadLine();
                        operands[i] = double.Parse(selectedOperand);
                    }
                    Console.WriteLine("调用计算器");
                    double result = Calculator.Operate(operation, operands);
                    Console.WriteLine("result:{0}", result);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    continue;
                }

            } while (selectedOp != "exit");

        }
    }
}
