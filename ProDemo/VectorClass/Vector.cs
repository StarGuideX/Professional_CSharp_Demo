using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WhatsNewAttributes;

[assembly:SupportsWhatsNew]
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

        public override bool Equals(object obj) => this == obj as Vector;

        public override int GetHashCode() => (int)X | (int)Y | (int)Z;

        [LastModified("2010年2月10日", "方法加入格式化")]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                return ToString();
            }

            switch (format.ToUpper())
            {
                case "N":
                    return "|| " + Norm().ToString() + " ||";
                case "VE":
                    return $"( {X:E}, {Y:E}, {Z:E} )";
                case "IJK":
                    var sb = new StringBuilder(X.ToString(), 30);
                    sb.Append(" i + ");
                    sb.Append(Y.ToString());
                    sb.Append(" j + ");
                    sb.Append(Z.ToString());
                    sb.Append(" k");
                    return sb.ToString();
                default:
                    return ToString();
            }
        }

        [LastModified("6 Jun 2015", "added to implement IEnumerable<T>")]
        public IEnumerator<double> GetEnumerator() => new VectorEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 

        public override string ToString() => $"({X} , {Y}, {Z}";

        public double this[uint i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new IndexOutOfRangeException(
                            "Attempt to retrieve Vector element" + i);
                }
            }
        }

        public static bool operator ==(Vector left, Vector right) =>
           Math.Abs(left.X - right.X) < double.Epsilon &&
           Math.Abs(left.Y - right.Y) < double.Epsilon &&
           Math.Abs(left.Z - right.Z) < double.Epsilon;


        public static bool operator !=(Vector left, Vector right) => !(left == right);

        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public static Vector operator *(double left, Vector right) =>
            new Vector(left * right.X, left * right.Y, left * right.Z);

        public static Vector operator *(Vector left, double right) => left * right;

        public static double operator *(Vector left, Vector right) =>
            left.X * right.X + left.Y + right.Y + left.Z * right.Z;

        public double Norm() => X * X + Y * Y + Z * Z;

        [LastModified("2015年6月10日", "实现泛型接口IEnumerable<T>")]
        [LastModified("2010年12月14日", "创建集合的item")]
        private class VectorEnumerator : IEnumerator<double>
        {
            readonly Vector _theVector;      // Vector object that this enumerato refers to 
            int _location;   // which element of _theVector the enumerator is currently referring to 

            public VectorEnumerator(Vector theVector)
            {
                _theVector = theVector;
                _location = -1;
            }

            public bool MoveNext()
            {
                ++_location;
                return (_location <= 2);
            }

            public object Current => Current;

            double IEnumerator<double>.Current
            {
                get
                {
                    if (_location < 0 || _location > 2)
                        throw new InvalidOperationException(
                            "The enumerator is either before the first element or " +
                            "after the last element of the Vector");
                    return _theVector[(uint)_location];
                }
            }

            public void Reset()
            {
                _location = -1;
            }

            public void Dispose()
            {
                // nothing to cleanup
            }
        }
    }
}
