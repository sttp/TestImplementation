using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpValueSet : SttpValue
    {
        public readonly SttpValueSet Value;

        public SttpValueSttpValueSet(SttpValueSet value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpValueSetMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpValueSetMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpValueSetMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueSttpValueSetMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueSttpValueSetMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueSttpValueSetMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueSttpValueSetMethods.AsInt64(Value);
        public override byte AsByte => SttpValueSttpValueSetMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueSttpValueSetMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueSttpValueSetMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueSttpValueSetMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSttpValueSetMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpValueSetMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpValueSetMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueSttpValueSetMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueSttpValueSetMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueSttpValueSetMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueSttpValueSetMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueSttpValueSetMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueSttpValueSetMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSttpValueSetMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSttpValueSetMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpValueSetMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpValueSetMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSttpValueSetMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSttpValueSetMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpValueSetMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSttpValueSetMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueSttpValueSetMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpValueSet;

        public static string ToTypeString(SttpValueSet value)
        {
            return $"(SttpValueSet){value.ToString()}";
        }

        public static object ToNativeType(SttpValueSet value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpValueSet value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(SttpValueSet value)
        {
            return value;
        }

        public static SttpNamedSet AsSttpNamedSet(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(SttpValueSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
