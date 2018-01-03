using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueDouble : SttpValue
    {
        public readonly double Value;

        public SttpValueDouble(double value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueDoubleMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueDoubleMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueDoubleMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueDoubleMethods.AsInt64(Value);
        public override float AsSingle => SttpValueDoubleMethods.AsSingle(Value);
        public override double AsDouble => SttpValueDoubleMethods.AsDouble(Value);
        public override SttpTime AsSttpTime => SttpValueDoubleMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueDoubleMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueDoubleMethods.AsGuid(Value);
        public override string AsString => SttpValueDoubleMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueDoubleMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueDoubleMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueDoubleMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueDoubleMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Double;
       
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

        public static SttpTime AsSttpTime(double value)
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

        public static SttpBuffer AsSttpBuffer(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
