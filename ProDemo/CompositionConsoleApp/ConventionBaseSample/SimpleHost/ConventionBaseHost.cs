using CompositionConsoleApp.AttributeBaseSample.CalculatorContract;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Text;


namespace CompositionConsoleApp.ConventionBaseSample.SimpleHost
{
    public class ConventionBaseHost
    {
        public ICalculator Calculator { get; set; }

        public void ConventionBaseStart()
        {
            Bootstrap();
            Run();
        }

        public void Bootstrap()
        {
            var conventions = new ConventionBuilder();
            //使用ForTypesDerivedFrom<ICalculator>().Export<ICalculator>()方法来代替Export特性 导出ICalculator
            conventions.ForTypesDerivedFrom<ICalculator>().Export<ICalculator>();
            //使用ConventionBaseHost类的约定规则 代替Impor特性 导入ICalculator类型的属性。属性使用lambda表达式定义
            conventions.ForType<ConventionBaseHost>().ImportProperty<ICalculator>(p => p.Calculator);

            // 通过ContainerConfiguration.WithDefaultConventions 配置使用ConventionBuilder定义的约定
            // 在定义了要使用的约定后，可以像之前那样使用WithPart方法，指定部件中应当应用约定的部分。
            // 为了使之比以前更加灵活，现在WithAssemblies方法用于指定应该应用的程序集。
            // 过滤传递给这个方法的所有程序集，得到派生自ICalculator接口的类型，来应用出口。
            var configuration = new ContainerConfiguration().WithDefaultConventions(conventions).WithAssemblies(GetAssemblies("C:/addins"));

            using (CompositionHost host = configuration.CreateContainer())
            {
                host.SatisfyImports(this, conventions);
            }
        }

        /// <summary>
        /// 从给定的目录中加载所有的程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<Assembly> GetAssemblies(string path)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(path, "*.dll");
            var assemblies = new List<Assembly>();
            foreach (var file in files)
            {
                Assembly assembly = Assembly.LoadFile(file);
                assemblies.Add(assembly);
            }

            return assemblies;
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
