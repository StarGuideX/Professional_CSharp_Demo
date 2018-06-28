using CompositionConsoleApp.AttributeBaseSample.CalculatorContract;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace CompositionConsoleApp.AttributeBaseSample.SimpleCalculator
{
    /// <summary>
    /// 按照Export特性的定义，Calculator类导出为部件
    /// </summary>
    [Export(typeof(ICalculator))]
    public class Calculator : ICalculator
    {
        public IList<IOperation> GetOperations() =>
            new List<IOperation>
            {
                new Operation {Name="+",NumberOperands=2},
                new Operation {Name="-",NumberOperands=2},
                new Operation {Name="*",NumberOperands=2},
                new Operation {Name="/",NumberOperands=2},
            };

        public double Operate(IOperation operation, double[] operands)
        {
            double result = 0;
            switch (operation.Name)
            {
                case "+":
                    result = operands[0] + operands[1];
                    break;
                case "-":
                    result = operands[0] - operands[1];
                    break;
                case "*":
                    result = operands[0] * operands[1];
                    break;
                case "/":
                    result = operands[0] / operands[1];
                    break;
                default:
                    throw new InvalidOperationException($"不合法的操作{operation.Name}");
            }
            return result; 
        }
    }
}
