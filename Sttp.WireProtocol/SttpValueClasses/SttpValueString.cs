using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueString : SttpValue
    {
        public static readonly SttpValue EmptyString = new SttpValueString(string.Empty);

        public readonly string Value;

        public SttpValueString(string value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueStringMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueStringMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueStringMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueStringMethods.AsInt64(Value);
        public override float AsSingle => SttpValueStringMethods.AsSingle(Value);
        public override double AsDouble => SttpValueStringMethods.AsDouble(Value);
        public override SttpTime AsSttpTime => SttpValueStringMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueStringMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueStringMethods.AsGuid(Value);
        public override string AsString => SttpValueStringMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueStringMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueStringMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueStringMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueStringMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.String;

        public static string ToTypeString(string value)
        {
            return $"(String){value.ToString()}";
        }

        public static object ToNativeType(string value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static SttpTime AsSttpTime(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(string value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
