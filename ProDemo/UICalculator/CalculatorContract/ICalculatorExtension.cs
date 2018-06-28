using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UICalculator.CalculatorContract
{
    /// <summary>
    /// UI属性允许插件返回派生自FrameworkElement的任何用户界面元素，并在宿主应用程序中显示为用户界面
    /// </summary>
    public interface ICalculatorExtension
    {
        object UI { get; }
    }
}
