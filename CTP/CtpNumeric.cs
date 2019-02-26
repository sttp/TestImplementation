using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF;

namespace CTP
{
    public struct CtpNumeric
    {
        public readonly byte Flags;
        public readonly int High;
        public readonly int Low;
        public readonly int Mid;

        public CtpNumeric(decimal value)
        {
            var bits = decimal.GetBits(value);
            Low = bits[0];
            Mid = bits[1];
            High = bits[2];
            Flags = (byte)((bits[3] >> 16) & 31);
            if (Flags > 28)
                throw new ArgumentException("Invalid Scale Factor");
            if (bits[3] < 0)
                Flags += 128;
        }

        public CtpNumeric(byte flags, int high, int mid, int low)
        {
            High = high;
            Mid = mid;
            Low = low;
            Flags = flags;
            if (Scale > 28)
                throw new ArgumentException("Invalid Scale Factor");
        }

        public CtpNumeric(bool isNegative, byte scale, int high, int mid, int low)
        {
            if (scale > 28)
                throw new ArgumentException("Invalid Scale Factor");
            Flags = scale;
            if (isNegative)
                Flags += 128;
            High = high;
            Mid = mid;
            Low = low;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">a signed value that is to be scaled.</param>
        /// <param name="scale">A scaling factor. From 0 to 28</param>
        public CtpNumeric(long value, byte scale = 0)
        {
            if (scale > 28)
                throw new ArgumentException("Invalid Scale Factor");
            Flags = scale;
            if (value < 0)
            {
                Flags += 128;
                value = -value; //ToDo: Check this conversion
            }
            Low = (int)value;
            Mid = (int)(value >> 32);
            High = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">a signed value that is to be scaled.</param>
        /// <param name="scale">A scaling factor. From 0 to 28</param>
        public CtpNumeric(ulong value, byte scale = 0)
        {
            if (scale > 28)
                throw new ArgumentException("Invalid Scale Factor");
            Flags = scale;
            Low = (int)value;
            Mid = (int)(value >> 32);
            High = 0;
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
            return a.IsNegative == b.IsNegative && a.High == b.High && a.Mid == b.Mid && a.Low == b.Low;
        }

        public static bool operator !=(CtpNumeric a, CtpNumeric b)
        {
            return !(a == b);
        }

        public byte Scale => (byte)(Flags & 31);
        public bool IsNegative => Flags > 127;
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
