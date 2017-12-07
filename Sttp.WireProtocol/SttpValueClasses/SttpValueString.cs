using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueString : SttpValue
    {
        public readonly string Value;

        public SttpValueString(string value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueStringMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueStringMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueStringMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueStringMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueStringMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueStringMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueStringMethods.AsInt64(Value);
        public override byte AsByte => SttpValueStringMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueStringMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueStringMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueStringMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueStringMethods.AsSingle(Value);
        public override double AsDouble => SttpValueStringMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueStringMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueStringMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueStringMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueStringMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueStringMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueStringMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueStringMethods.AsBoolean(Value);
        public override char AsChar => SttpValueStringMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueStringMethods.AsGuid(Value);
        public override string AsString => SttpValueStringMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueStringMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueStringMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueStringMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueStringMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueStringMethods.AsBulkTransportGuid(Value);
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

        public static sbyte AsSByte(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
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

        public static SttpValueSet AsSttpValueSet(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(string value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
