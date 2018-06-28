using System;
using System.Collections.Generic;
using System.Text;

namespace UICalculator.CalculatorContract
{
    public interface IBinaryOperation
    {
        double Operation(double x, double y);
    }
}
