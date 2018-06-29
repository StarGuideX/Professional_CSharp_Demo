using System;
using System.Collections;
using System.Collections.Generic;
using WhatsNewAttributes;

namespace VectorClass
{
    [LastModified("2015年6月10日", "updated for C# 6 and .NET Core")]
    [LastModified("2010年12月14日", "IEnumerable interface implemented:" + "Vector可以作为一个集合来对待")]
    [LastModified("2010年2月10日", "IFormattable interface implemented" + "Vector接受N和VE格式的说明符")]
    public class Vector : IFormattable, IEnumerable<double>
    {
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(Vector vector) : this(vector.X, vector.Y, vector.Z)
        {

        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }


        

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
        public IEnumerator<double> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
