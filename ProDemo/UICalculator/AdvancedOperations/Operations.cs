using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using UICalculator.CalculatorContract;

namespace UICalculator.AdvancedOperations
{
    [Export("Add", typeof(IBinaryOperation))]
    public class AddOperation : IBinaryOperation
    {
        public double Operation(double x, double y) => x + y;
    }


    [Export("Subtract", typeof(IBinaryOperation))]
    public class SubtractOperation : IBinaryOperation
    {
        public double Operation(double x, double y) => x - y;
    }



    public class Operations
    {

    }
}
