using CompositionConsoleApp.AttributeBaseSample.CalculatorContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompositionConsoleApp.AttributeBaseSample.SimpleCalculator
{
    /// <summary>
    /// 实现协定定义的接口
    /// Operation类实现了IOperation接口。
    /// 这个类仅包含接口定义的两个属性。
    /// 该接口定义了属性的get访问器；内部的set访问器用于在程序集内部设置属性
    /// </summary>
    public class Operation : IOperation
    {
        public string Name { get; internal set; }

        public int NumberOperands { get; internal set; }
    }
}
