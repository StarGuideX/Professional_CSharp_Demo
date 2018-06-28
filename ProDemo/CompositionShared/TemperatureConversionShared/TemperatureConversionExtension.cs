using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using UICalculator.CalculatorContract;

namespace CompositionShared.TemperatureConversionShared
{
    [Export(typeof(ICalculatorExtension))]
    //[ICalculatorExtensionMetadata]
    public class TemperatureConversionExtension : ICalculatorExtension
    {
        private object _control;
        public object UI => _control ?? (_control = new TemperatureConversionExtension());
    }
}
