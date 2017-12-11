using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSttpNamedSet : SttpValue
    {
        public readonly SttpNamedSet Value;

        public SttpValueSttpNamedSet(SttpNamedSet value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSttpNamedSetMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSttpNamedSetMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSttpNamedSetMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueSttpNamedSetMethods.AsInt64(Value);
        public override ulong AsUInt64 => SttpValueSttpNamedSetMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSttpNamedSetMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSttpNamedSetMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSttpNamedSetMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSttpNamedSetMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSttpNamedSetMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSttpNamedSetMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSttpNamedSetMethods.AsGuid(Value);
        public override string AsString => SttpValueSttpNamedSetMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSttpNamedSetMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSttpNamedSetMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSttpNamedSetMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSttpNamedSetMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSttpNamedSetMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueSttpNamedSetMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SttpNamedSet;

        public static string ToTypeString(SttpNamedSet value)
        {
            return $"(SttpNamedSet){value.ToString()}";
        }

        public static object ToNativeType(SttpNamedSet value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static TimeSpan AsTimeSpan(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(SttpNamedSet value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(SttpNamedSet value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
