using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    public struct CtpNumeric
    {
        public readonly int Flags;
        public readonly int High;
        public readonly int Low;
        public readonly int Mid;

        public CtpNumeric(decimal value)
        {
            var bits = decimal.GetBits(value);
            Low = bits[0];
            Mid = bits[1];
            High = bits[2];
            Flags = bits[3];
        }

        public CtpNumeric(int flags, int high, int mid, int low)
        {
            Flags = flags;
            High = high;
            Mid = mid;
            Low = low;
        }

        public static explicit operator CtpNumeric(decimal value)
        {
            return new CtpNumeric(value);
        }

        public static explicit operator decimal(CtpNumeric value)
        {
            return new decimal(value.Low, value.Mid, value.High, value.IsNegative, value.Scale);
        }

        public static bool operator ==(CtpNumeric a, CtpNumeric b)
        {
            return a.Flags == b.Flags && a.High == b.High && a.Mid == b.Mid && a.Low == b.Low;
        }

        public static bool operator !=(CtpNumeric a, CtpNumeric b)
        {
            return !(a == b);
        }

        public byte Scale => (byte)(Flags >> 16);
        public bool IsNegative => Flags < 0;
        public bool IsDefault => Flags == 0 && High == 0 && Low == 0 && Mid == 0;

        public override bool Equals(object value)
        {
            if (value is CtpNumeric)
            {
                return this == (CtpNumeric)value;
            }
            return false;
        }

        public override string ToString()
        {
            return ((decimal)this).ToString();
        }

        public override int GetHashCode()
        {
            return Flags ^ High ^ Low ^ Mid;
        }

    }
}
