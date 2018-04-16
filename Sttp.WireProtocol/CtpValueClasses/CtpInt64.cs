using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CtpInt64 : CtpValue
    {
        public readonly long Value;

        public CtpInt64(long value)
        {
            Value = value;
        }

        public override CtpTypeCode ValueTypeCode => SttpValueInt64Methods.ValueTypeCode;
        public override string ToTypeString => SttpValueInt64Methods.ToTypeString(Value);
        public override object ToNativeType => SttpValueInt64Methods.ToNativeType(Value);
        public override long AsInt64 => SttpValueInt64Methods.AsInt64(Value);
        public override float AsSingle => SttpValueInt64Methods.AsSingle(Value);
        public override double AsDouble => SttpValueInt64Methods.AsDouble(Value);
        public override CtpTime AsCtpTime => SttpValueInt64Methods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueInt64Methods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueInt64Methods.AsGuid(Value);
        public override string AsString => SttpValueInt64Methods.AsString(Value);
        public override CtpBuffer AsSttpBuffer => SttpValueInt64Methods.AsSttpBuffer(Value);
        public override CtpMarkup AsSttpMarkup => SttpValueInt64Methods.AsSttpMarkup(Value);
    }

    internal static class SttpValueInt64Methods
    {
        public static CtpTypeCode ValueTypeCode => CtpTypeCode.Int64;
       
        public static string ToTypeString(long value)
        {
            return $"(long){value.ToString()}";
        }

        public static object ToNativeType(long value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(long value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static float AsSingle(long value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(long value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static CtpTime AsSttpTime(long value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(long value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(long value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(long value)
        {
            return value.ToString();
        }

        public static CtpBuffer AsSttpBuffer(long value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static CtpMarkup AsSttpMarkup(long value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        #endregion
    }
}
