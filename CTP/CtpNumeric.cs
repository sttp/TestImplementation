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
        public readonly uint High;
        public readonly uint Low;
        public readonly uint Mid;

        public CtpNumeric(decimal value)
        {
            var bits = decimal.GetBits(value);
            Low = (uint)bits[0];
            Mid = (uint)bits[1];
            High = (uint)bits[2];
            Flags = (byte)((bits[3] >> 16) & 31);
            if (Flags > 28)
                throw new ArgumentException("Invalid Scale Factor");
            if (bits[3] < 0)
                Flags += 32;
        }

        public CtpNumeric(byte flags, uint high, uint mid, uint low)
        {
            High = (uint)high;
            Mid = (uint)mid;
            Low = (uint)low;
            Flags = flags;
            if (Scale > 28)
                throw new ArgumentException("Invalid Scale Factor");
            if (Flags > 63)
                throw new ArgumentException("Flags");

        }

        public CtpNumeric(bool isNegative, byte scale, uint high, uint mid, uint low)
        {
            if (scale > 28)
                throw new ArgumentException("Invalid Scale Factor");
            Flags = scale;
            if (isNegative)
                Flags += 32;
            High = (uint)high;
            Mid = (uint)mid;
            Low = (uint)low;
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
                Flags += 32;
                value = -value; //ToDo: Check this conversion
            }
            Low = (uint)value;
            Mid = (uint)(value >> 32);
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
            Low = (uint)value;
            Mid = (uint)(value >> 32);
            High = 0;
        }

        public static explicit operator CtpNumeric(decimal value)
        {
            return new CtpNumeric(value);
        }

        public static explicit operator decimal(CtpNumeric value)
        {
            return new decimal((int)value.Low, (int)value.Mid, (int)value.High, value.IsNegative, value.Scale);
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
        public bool IsNegative => Flags >= 32;
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
            return (int)(Flags ^ High ^ Low ^ Mid);
        }

    }
}
