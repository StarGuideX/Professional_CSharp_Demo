using System;
using System.Collections.Generic;
using System.Text;
using UICalculator.CalculatorUtils;

namespace UICalculator.FuelEconomyShared
{
    public class FuelEconomyViewModel : BindableBase
    {
        public FuelEconomyViewModel()
        {
            InitializeFuelEcoTypes();

        }

        public List<FuelEconomyType> FuelEcoTypes { get; } = new List<FuelEconomyType>();

        private void InitializeFuelEcoTypes()
        {
            var t1 = new FuelEconomyType()
            {
                Id = "lpk",
                Text = "L/100 km",
                DistanceText = "距离(千米)",
                FuelText = "用于燃料(升)"
            };
            var t2 = new FuelEconomyType()
            {
                Id = "mpg",
                Text = "Miles per gallon",
                DistanceText = "距离（米）",
                FuelText = "用于燃料(加仑)"
            };
            FuelEcoTypes.AddRange(new FuelEconomyType[] { t1, t2 });
        }

       
    }
}
