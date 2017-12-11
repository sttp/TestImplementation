using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpBulkTransport : SttpValue
    {
        public readonly SttpBulkTransport Value;

        public SttpValueSttpBulkTransport(SttpBulkTransport value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpBulkTransportMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpBulkTransportMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpBulkTransportMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpBulkTransportMethods.AsInt64(Value);
        public override ulong AsUInt64 => SttpValueSttpBulkTransportMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSttpBulkTransportMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpBulkTransportMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpBulkTransportMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSttpBulkTransportMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpBulkTransportMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpBulkTransportMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpBulkTransportMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpBulkTransportMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpBulkTransportMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueSttpBulkTransportMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueSttpBulkTransportMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpBulkTransport;

        public static string ToTypeString(SttpBulkTransport value)
        {
            return $"(SttpBulkTransport){value.ToString()}";
        }

        public static object ToNativeType(SttpBulkTransport value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static ulong AsUInt64(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        
        public static SttpTime AsSttpTime(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

       

        public static bool AsBoolean(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        
        public static Guid AsGuid(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpBulkTransport value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(SttpBulkTransport value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(SttpBulkTransport value)
        {
            return value;
        }

        #endregion
    }
}
