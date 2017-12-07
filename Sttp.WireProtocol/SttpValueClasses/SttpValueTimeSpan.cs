using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueTimeSpan : SttpValue
    {
        public readonly TimeSpan Value;

        public SttpValueTimeSpan(TimeSpan value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueTimeSpanMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueTimeSpanMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueTimeSpanMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueTimeSpanMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueTimeSpanMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueTimeSpanMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueTimeSpanMethods.AsInt64(Value);
        public override byte AsByte => SttpValueTimeSpanMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueTimeSpanMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueTimeSpanMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueTimeSpanMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueTimeSpanMethods.AsSingle(Value);
        public override double AsDouble => SttpValueTimeSpanMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueTimeSpanMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueTimeSpanMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueTimeSpanMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueTimeSpanMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueTimeSpanMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueTimeSpanMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueTimeSpanMethods.AsBoolean(Value);
        public override char AsChar => SttpValueTimeSpanMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueTimeSpanMethods.AsGuid(Value);
        public override string AsString => SttpValueTimeSpanMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueTimeSpanMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueTimeSpanMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueTimeSpanMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueTimeSpanMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueTimeSpanMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueTimeSpanMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.TimeSpan;
       
        public static string ToTypeString(TimeSpan value)
        {
            return $"(TimeSpan){value.ToString()}";
        }

        public static object ToNativeType(TimeSpan value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(TimeSpan value)
        {
            return value;
        }

        public static bool AsBoolean(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(TimeSpan value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(TimeSpan value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
