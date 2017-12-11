using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueBoolean : SttpValue
    {
        public readonly bool Value;

        public SttpValueBoolean(bool value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueBooleanMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueBooleanMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueBooleanMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueBooleanMethods.AsInt64(Value);
        public override ulong AsUInt64 => SttpValueBooleanMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueBooleanMethods.AsSingle(Value);
        public override double AsDouble => SttpValueBooleanMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueBooleanMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueBooleanMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueBooleanMethods.AsBoolean(Value);
        public override char AsChar => SttpValueBooleanMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueBooleanMethods.AsGuid(Value);
        public override string AsString => SttpValueBooleanMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueBooleanMethods.AsSttpBuffer(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueBooleanMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueBooleanMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueBooleanMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Boolean;

        public static string ToTypeString(bool value)
        {
            return $"(bool){value.ToString()}";
        }

        public static object ToNativeType(bool value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SByte");
        }
        public static short AsInt16(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int16");
        }

        public static int AsInt32(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int32");
        }

        public static long AsInt64(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Int64");
        }

        public static byte AsByte(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Byte");
        }

        public static ushort AsUInt16(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt16");
        }

        public static uint AsUInt32(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt32");
        }

        public static ulong AsUInt64(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to UInt64");
        }

        public static float AsSingle(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Single");
        }

        public static double AsDouble(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Double");
        }

        public static decimal AsDecimal(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Decimal");
        }

        public static DateTime AsDateTime(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static TimeSpan AsTimeSpan(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(bool value)
        {
            return value;
        }

        public static char AsChar(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Char");
        }

        public static Guid AsGuid(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(bool value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpMarkup AsSttpMarkup(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(bool value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}