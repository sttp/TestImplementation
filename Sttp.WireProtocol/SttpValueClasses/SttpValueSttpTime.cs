using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpTime : SttpValue
    {
        public readonly SttpTime Value;

        public SttpValueSttpTime(SttpTime value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpTimeMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpTimeMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpTimeMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpTimeMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSttpTimeMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpTimeMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpTimeMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSttpTimeMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpTimeMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpTimeMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpTimeMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpTimeMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpTimeMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueSttpTimeMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueSttpTimeMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpTime;
       
        public static string ToTypeString(SttpTime value)
        {
            return $"(SttpTime){value.ToString()}";
        }

        public static object ToNativeType(SttpTime value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static SttpTime AsSttpTime(SttpTime value)
        {
            return value;
        }

        public static bool AsBoolean(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpTime value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(SttpTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
