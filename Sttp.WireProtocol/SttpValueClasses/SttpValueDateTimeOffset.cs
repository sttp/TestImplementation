using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueDateTimeOffset : SttpValue
    {
        public readonly DateTimeOffset Value;

        public SttpValueDateTimeOffset(DateTimeOffset value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueDateTimeOffsetMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueDateTimeOffsetMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueDateTimeOffsetMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueDateTimeOffsetMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueDateTimeOffsetMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueDateTimeOffsetMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueDateTimeOffsetMethods.AsInt64(Value);
        public override byte AsByte => SttpValueDateTimeOffsetMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueDateTimeOffsetMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueDateTimeOffsetMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueDateTimeOffsetMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueDateTimeOffsetMethods.AsSingle(Value);
        public override double AsDouble => SttpValueDateTimeOffsetMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueDateTimeOffsetMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueDateTimeOffsetMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueDateTimeOffsetMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueDateTimeOffsetMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueDateTimeOffsetMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueDateTimeOffsetMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueDateTimeOffsetMethods.AsBoolean(Value);
        public override char AsChar => SttpValueDateTimeOffsetMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueDateTimeOffsetMethods.AsGuid(Value);
        public override string AsString => SttpValueDateTimeOffsetMethods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueDateTimeOffsetMethods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueDateTimeOffsetMethods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueDateTimeOffsetMethods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueDateTimeOffsetMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueDateTimeOffsetMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueDateTimeOffsetMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.DateTimeOffset;
       
        public static string ToTypeString(DateTimeOffset value)
        {
            return $"(DateTimeOffset){value.ToString()}";
        }

        public static object ToNativeType(DateTimeOffset value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(DateTimeOffset value)
        {
            return value.UtcDateTime;
        }

        public static DateTimeOffset AsDateTimeOffset(DateTimeOffset value)
        {
            return (DateTimeOffset)value;
        }

        public static SttpTime AsSttpTime(DateTimeOffset value)
        {
            return new SttpTime(value.UtcDateTime);
        }

        public static SttpTimeOffset AsSttpTimeOffset(DateTimeOffset value)
        {
            return new SttpTimeOffset(value);
        }

        public static TimeSpan AsTimeSpan(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(DateTimeOffset value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(DateTimeOffset value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
