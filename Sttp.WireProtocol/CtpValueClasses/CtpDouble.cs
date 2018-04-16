using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpDouble : CtpValue
    {
        public readonly double Value;

        public CtpDouble(double value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueDoubleMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueDoubleMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueDoubleMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueDoubleMethods.AsInt64(Value);
        public override float AsSingle => SttpValueDoubleMethods.AsSingle(Value);
        public override double AsDouble => SttpValueDoubleMethods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueDoubleMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueDoubleMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueDoubleMethods.AsGuid(Value);
        public override string AsString => SttpValueDoubleMethods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueDoubleMethods.AsSttpBuffer(Value);
        public override CtpMarkup AsSttpMarkup => SttpValueDoubleMethods.AsSttpMarkup(Value);
    }

    internal static class SttpValueDoubleMethods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.Double;
       
        public static string ToTypeString(double value)
        {
            return $"(double){value.ToString()}";
        }

        public static object ToNativeType(double value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(double value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static float AsSingle(double value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(double value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static CtpTime AsSttpTime(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(double value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpMarkup AsSttpMarkup(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}
