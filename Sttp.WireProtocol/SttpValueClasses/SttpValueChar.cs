using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueChar : SttpValue
    {
        public readonly char Value;

        public SttpValueChar(char value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueCharMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueCharMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueCharMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueCharMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueCharMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueCharMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueCharMethods.AsInt64(Value);
        public override byte AsByte => SttpValueCharMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueCharMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueCharMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueCharMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueCharMethods.AsSingle(Value);
        public override double AsDouble => SttpValueCharMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueCharMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueCharMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueCharMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueCharMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueCharMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueCharMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueCharMethods.AsBoolean(Value);
        public override char AsChar => SttpValueCharMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueCharMethods.AsGuid(Value);
        public override string AsString => SttpValueCharMethods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueCharMethods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueCharMethods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueCharMethods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueCharMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueCharMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueCharMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Char;

        public static string ToTypeString(char value)
        {
            return $"(char){value.ToString()}";
        }

        public static object ToNativeType(char value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(char value)
        {
            return value;
        }

        public static Guid AsGuid(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(char value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(char value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
