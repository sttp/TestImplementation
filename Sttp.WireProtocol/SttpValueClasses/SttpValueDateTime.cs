using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueDateTime : SttpValue
    {
        public readonly DateTime Value;

        public SttpValueDateTime(DateTime value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueDateTimeMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueDateTimeMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueDateTimeMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueDateTimeMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueDateTimeMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueDateTimeMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueDateTimeMethods.AsInt64(Value);
        public override byte AsByte => SttpValueDateTimeMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueDateTimeMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueDateTimeMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueDateTimeMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueDateTimeMethods.AsSingle(Value);
        public override double AsDouble => SttpValueDateTimeMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueDateTimeMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueDateTimeMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueDateTimeMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueDateTimeMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueDateTimeMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueDateTimeMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueDateTimeMethods.AsBoolean(Value);
        public override char AsChar => SttpValueDateTimeMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueDateTimeMethods.AsGuid(Value);
        public override string AsString => SttpValueDateTimeMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueDateTimeMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueDateTimeMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueDateTimeMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueDateTimeMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueDateTimeMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueDateTimeMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.DateTime;
       
        public static string ToTypeString(DateTime value)
        {
            return $"(DateTime){value.ToString()}";
        }

        public static object ToNativeType(DateTime value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(DateTime value)
        {
            return value;
        }

        public static DateTimeOffset AsDateTimeOffset(DateTime value)
        {
            return (DateTimeOffset)value;
        }

        public static SttpTime AsSttpTime(DateTime value)
        {
            return new SttpTime(value);
        }

        public static SttpTimeOffset AsSttpTimeOffset(DateTime value)
        {
            return new SttpTimeOffset(value);
        }

        public static TimeSpan AsTimeSpan(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(DateTime value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(DateTime value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
