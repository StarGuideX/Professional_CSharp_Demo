using System;
using System.Collections.Generic;
using System.Text;

namespace UICalculator.CalculatorUtils
{
    public enum Speed
    {
        Fast,
        Slow
    }

    public class SpeedMetadata
    {
        /// <summary>
        /// 为了利用入口访问元数据，定义SpeedMetadata类。
        /// SpeedMetadata定义了与SpeedMetadataAttribute相同的属性
        /// </summary>
        public Speed Speed { get; set; }
    }
}
