using System;
using System.Collections.Generic;
using System.Text;

namespace UICalculator.CalculatorContract
{
    /// <summary>
    /// ICalculator接口使用IOperation接口返回操作列表，并调用一个操作。
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// GetOperations()方法返回插件计算器支持的所有操作对应的列表，Operate()方法可调用操作。
        /// 这个接口很灵活，因为计算器可以支持不同的操作。如果该接口定义了Add()和Subtract()方法，
        /// 而不是灵活的Operate()方法，就需要一个新版本的接口来支持Divide()和Multiply()方法。
        /// </summary>
        /// <returns></returns>
        IList<IOperation> GetOperations();
        double Operate(IOperation operation, double[] operands);
    }
}
