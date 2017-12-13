using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpValueSingle : SttpValue
    {
        public readonly float Value;

        public SttpValueSingle(float value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSingleMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSingleMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSingleMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSingleMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSingleMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSingleMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSingleMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSingleMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSingleMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSingleMethods.AsGuid(Value);
        public override string AsString => SttpValueSingleMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSingleMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSingleMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueSingleMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueSingleMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Single;
       
        public static string ToTypeString(float value)
        {
            return $"(float){value.ToString()}";
        }

        public static object ToNativeType(float value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(float value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static float AsSingle(float value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(float value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(float value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static SttpTime AsSttpTime(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(float value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
