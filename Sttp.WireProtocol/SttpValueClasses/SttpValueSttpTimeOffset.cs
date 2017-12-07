using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpTimeOffset : SttpValue
    {
        public readonly SttpTimeOffset Value;

        public SttpValueSttpTimeOffset(SttpTimeOffset value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpTimeOffsetMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpTimeOffsetMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpTimeOffsetMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueSttpTimeOffsetMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueSttpTimeOffsetMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueSttpTimeOffsetMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueSttpTimeOffsetMethods.AsInt64(Value);
        public override byte AsByte => SttpValueSttpTimeOffsetMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueSttpTimeOffsetMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueSttpTimeOffsetMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueSttpTimeOffsetMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSttpTimeOffsetMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpTimeOffsetMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpTimeOffsetMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueSttpTimeOffsetMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueSttpTimeOffsetMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueSttpTimeOffsetMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueSttpTimeOffsetMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueSttpTimeOffsetMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueSttpTimeOffsetMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSttpTimeOffsetMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSttpTimeOffsetMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpTimeOffsetMethods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueSttpTimeOffsetMethods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSttpTimeOffsetMethods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSttpTimeOffsetMethods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpTimeOffsetMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSttpTimeOffsetMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueSttpTimeOffsetMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpTimeOffset;
       
        public static string ToTypeString(SttpTimeOffset value)
        {
            return $"(SttpTimeOffset){value.ToString()}";
        }

        public static object ToNativeType(SttpTimeOffset value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(SttpTimeOffset value)
        {
            return value.Ticks;
        }

        public static DateTimeOffset AsDateTimeOffset(SttpTimeOffset value)
        {
            return (DateTimeOffset)value.Ticks;
        }

        public static SttpTime AsSttpTime(SttpTimeOffset value)
        {
            return value.ToSttpTimestamp();
        }

        public static SttpTimeOffset AsSttpTimeOffset(SttpTimeOffset value)
        {
            return value;
        }

        public static TimeSpan AsTimeSpan(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpTimeOffset value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(SttpTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
