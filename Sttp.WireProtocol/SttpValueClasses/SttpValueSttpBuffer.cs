using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpBuffer : SttpValue
    {
        public readonly SttpBuffer Value;

        public SttpValueSttpBuffer(SttpBuffer value)
        {
            Value = value;
        }

        public SttpValueSttpBuffer(byte[] value)
        {
            Value = new SttpBuffer(value);
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpBufferMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpBufferMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpBufferMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpBufferMethods.AsInt64(Value);
        public override float AsSingle => SttpValueSttpBufferMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpBufferMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpBufferMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSttpBufferMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpBufferMethods.AsBoolean(Value);
        public override Guid AsGuid => SttpValueSttpBufferMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpBufferMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpBufferMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpBufferMethods.AsSttpMarkup(Value);
        public override SttpBulkTransport AsSttpBulkTransport => SttpValueSttpBufferMethods.AsSttpBulkTransport(Value);
    }

    internal static class SttpValueSttpBufferMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpBuffer;

        public static string ToTypeString(SttpBuffer value)
        {
            return $"(SttpBuffer){value.ToString()}";
        }

        public static object ToNativeType(SttpBuffer value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static long AsInt64(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static float AsSingle(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static SttpTime AsSttpTime(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static bool AsBoolean(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static Guid AsGuid(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpBuffer value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpBuffer value)
        {
            return value;
        }

        public static SttpMarkup AsSttpMarkup(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static SttpBulkTransport AsSttpBulkTransport(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
