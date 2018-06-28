using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using UICalculator.CalculatorContract;
using UICalculator.CalculatorUtils;

namespace UICalculator.SimpleCalcutor
{
    public class Calculator : ICalculator
    {
        [Import("Add")]
        public Lazy<IBinaryOperation, SpeedMetadata> AddMethods { get; set; }

        [Import("Subtract")]
        public IBinaryOperation SubtractMethod { get; set; }

        public IList<IOperation> GetOperations()
        {
            throw new NotImplementedException();
        }

        public double Operate(IOperation operation, double[] operands)
        {
            double result = 0;
            switch (operation.Name)
            {
                case "+":
                    //foreach (var addMethods in AddMethods)
                    //{
                    //    if (addMethods.)
                    //    {

                    //    }
                    //}
                    break;
                case "-":
                    result = SubtractMethod.Operation(operands[0], operands[1]);
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
