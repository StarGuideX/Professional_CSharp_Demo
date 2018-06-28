using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace UICalculator.CalculatorUtils
{
    /// <summary>
    /// 创建一个特性类，应用MetadataAttribute，可以定义元数据。
    /// 这个特性应用于一个部件，如AddOperation和SlowAddOperation类型
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class SpeedMetadataAttribute :Attribute
    {
        public Speed Speed { get; set; }
    }
}
