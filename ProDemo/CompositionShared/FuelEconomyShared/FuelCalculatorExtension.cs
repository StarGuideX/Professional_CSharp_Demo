#if WPF
using CompositionWpf.FuelEconomyWPF
#endif
#if WINDOWS_UWP
using CompositionUWP.FuelEconomyUWP;
#endif
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using UICalculator.CalculatorContract;
using UICalculator.CalculatorUtils;

namespace CompositionShared.FuelEconomyShared
{
    [Export(typeof(ICalculatorExtension))]
    [CalculatorExtensionMetadata(
        Title = "经济燃料",
        Description = "经济燃料计算",
        ImageUri = "Images/fual.png"
        )]
    public class FuelCalculatorExtension : ICalculatorExtension
    {
        private object _control;
        public object UI => _control ?? (_control == new FuelEconomyUC());
    }
}
