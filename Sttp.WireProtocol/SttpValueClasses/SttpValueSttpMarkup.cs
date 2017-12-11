using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpMarkup : SttpValue
    {
        public readonly SttpMarkup Value;

        public SttpValueSttpMarkup(SttpMarkup value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpMarkupMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpMarkupMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpMarkupMethods.ToNativeType(Value);
        public override int AsInt32 => SttpValueSttpMarkupMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueSttpMarkupMethods.AsInt64(Value);
        public override uint AsUInt32 => SttpValueSttpMarkupMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueSttpMarkupMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSttpMarkupMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpMarkupMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpMarkupMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSttpMarkupMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpMarkupMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSttpMarkupMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSttpMarkupMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpMarkupMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpMarkupMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSttpMarkupMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSttpMarkupMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpMarkupMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSttpMarkupMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueSttpMarkupMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpMarkup;

        public static string ToTypeString(SttpMarkup value)
        {
            return $"(SttpMarkup){value.ToString()}";
        }

        public static object ToNativeType(SttpMarkup value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static TimeSpan AsTimeSpan(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpMarkup value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(SttpMarkup value)
        {
            return value;
        }

        public static Guid AsBulkTransportGuid(SttpMarkup value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
