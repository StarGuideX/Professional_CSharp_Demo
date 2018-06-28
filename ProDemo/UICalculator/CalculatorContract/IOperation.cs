using System;
using System.Collections.Generic;
using System.Text;

namespace UICalculator.CalculatorContract
{
    /// <summary>
    /// IOperation接口定义了只读属性Name和NumberOperands
    /// </summary>
    public interface IOperation
    {
        string Name { get; }
        int NumberOperands { get; }
    }
}
