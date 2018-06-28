using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace UICalculator.CalculatorUtils
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class CalculatorExtensionMetadataAttribute:Attribute
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }

    }
}
