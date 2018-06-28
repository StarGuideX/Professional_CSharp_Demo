using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using UICalculator.CalculatorContract;
using UICalculator.CalculatorUtils;

namespace UICalculator.CalculatorViewModels
{
    public class CalculatorImport
    {
        public event EventHandler<ImportEventArgs> ImportsSatisfied;
        [Import]
        public Lazy<ICalculator> Calculator { get; set; }
        [OnImportsSatisfied]
        public void OnImportsSatisfied()
        {
            ImportsSatisfied?.Invoke(this, new ImportEventArgs { StatusMessage = "ICalculator导入成功" });

        }
    }
}
