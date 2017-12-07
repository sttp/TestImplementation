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
        public override sbyte AsSByte => SttpValueSttpBufferMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueSttpBufferMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueSttpBufferMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueSttpBufferMethods.AsInt64(Value);
        public override byte AsByte => SttpValueSttpBufferMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueSttpBufferMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueSttpBufferMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueSttpBufferMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSttpBufferMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpBufferMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpBufferMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueSttpBufferMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueSttpBufferMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueSttpBufferMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueSttpBufferMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueSttpBufferMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueSttpBufferMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSttpBufferMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSttpBufferMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpBufferMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpBufferMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSttpBufferMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSttpBufferMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpBufferMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSttpBufferMethods.AsBulkTransportGuid(Value);
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

        public static sbyte AsSByte(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
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

        public static DateTime AsDateTime(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
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

        public static SttpValueSet AsSttpValueSet(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(SttpBuffer value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
