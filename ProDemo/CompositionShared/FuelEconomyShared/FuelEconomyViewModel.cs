using System;
using System.Collections.Generic;
using System.Text;
using UICalculator.CalculatorUtils;

namespace CompositionShared.FuelEconomyShared
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

        private FuelEconomyType _selectedFuelEcoType;

        public FuelEconomyType SelectedFuelEcoType
        {
            get { return _selectedFuelEcoType; }
            set { SetProperty(ref _selectedFuelEcoType, value); }
        }

        private string _fuel;
        public string Fuel
        {
            get { return _fuel; }
            set { SetProperty(ref _fuel, value); }
        }

        private string _distance;
        public string Distance
        {
            get { return _distance; }
            set { SetProperty(ref _distance, value); }
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        public void OnCalculate()
        {
            double fual = double.Parse(Fuel);
            double distance = double.Parse(Distance);
            FuelEconomyType ecoType = SelectedFuelEcoType;
            double result = 0;
            switch (ecoType.Id)
            {
                case "lpk":
                    result = fual / (distance / 100);
                    break;
                case "mpg":
                    result = distance / fual;
                    break;
                default:
                    break;
            }
            Result = result.ToString();
        }
    }
}
