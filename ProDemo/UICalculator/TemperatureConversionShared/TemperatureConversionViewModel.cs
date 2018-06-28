﻿using System;
using System.Collections.Generic;
using System.Text;
using UICalculator.CalculatorUtils;

namespace UICalculator.TemperatureConversionShared
{
    /// <summary>
    /// 该用户控件为温度转换提供了一个简单的实现方式。
    /// TempConversionType枚举定义了这个控件可能进行的不同转换。
    /// </summary>
    public enum TempConversionType
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }
    /// <summary>
    /// 
    /// </summary>
    public class TemperatureConversionViewModel : BindableBase
    {
        public TemperatureConversionViewModel()
        {
            //CalculateCommand = new DelegateCommand(OnCalculate);       
        }

        //public DelegateCommand CalculateCommand { get; }

        public IEnumerable<string> TemperatureConversionTypes => Enum.GetNames(typeof(TempConversionType));

        /// <summary>
        /// 将t从其原始值转换为摄氏值。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="conv"></param>
        /// <returns></returns>
        private double ToCelsiusFrom(double t, TempConversionType conv)
        {
            switch (conv)
            {
                case TempConversionType.Celsius:
                    return t;
                case TempConversionType.Fahrenheit:
                    return (t - 32) / 1.8;
                case TempConversionType.Kelvin:
                    return (t - 273.15);
                default:
                    throw new ArgumentException("不合规的枚举值");
            }
        }

        /// <summary>
        /// 将摄氏值转换为所选的温度值
        /// </summary>
        /// <param name="t"></param>
        /// <param name="conv"></param>
        /// <returns></returns>
        private double FromCelsiusTo(double t, TempConversionType conv)
        {
            switch (conv)
            {
                case TempConversionType.Celsius:
                    return t;
                case TempConversionType.Fahrenheit:
                    return t * 1.8 + 32;
                case TempConversionType.Kelvin:
                    return t + 273.15;
                default:
                    throw new ArgumentException("不合规的枚举值");
            }
        }

        private string _fromValue;
        public string FromValue
        {
            get { return _fromValue; }
            set { SetProperty(ref _fromValue, value); }
        }

        private string _toValue;
        public string ToValue
        {
            get { return _toValue; }
            set { SetProperty(ref _toValue, value); }
        }

        private TempConversionType _fromType;
        public TempConversionType FromTypee
        {
            get { return _fromType; }
            set { SetProperty(ref _fromType, value); }
        }

        private TempConversionType _toType;
        public TempConversionType ToTypee
        {
            get { return _toType; }
            set { SetProperty(ref _toType, value); }
        }

        /// <summary>
        /// 根据用户选择的转换类型执行转换
        /// </summary>
        public void OnCalculate() {
            double result = FromCelsiusTo(ToCelsiusFrom(double.Parse(FromValue),FromTypee),ToTypee);
            ToValue = result.ToString();
        }
    }
}
