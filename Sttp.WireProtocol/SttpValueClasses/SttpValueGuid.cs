using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueGuid : SttpValue
    {
        public readonly Guid Value;

        public SttpValueGuid(Guid value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueGuidMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueGuidMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueGuidMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueGuidMethods.AsInt64(Value);
        public override float AsSingle => SttpValueGuidMethods.AsSingle(Value);
        public override double AsDouble => SttpValueGuidMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueGuidMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueGuidMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueGuidMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueGuidMethods.AsGuid(Value);
        public override string AsString => SttpValueGuidMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueGuidMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueGuidMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueGuidMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueGuidMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Guid;

        public static string ToTypeString(Guid value)
        {
            return $"(Guid){value.ToString()}";
        }

        public static object ToNativeType(Guid value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static SttpTime AsSttpTime(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(Guid value)
        {
            return value;
        }

        public static string AsString(Guid value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(Guid value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBulkTransport");
        }

        #endregion
    }
}
