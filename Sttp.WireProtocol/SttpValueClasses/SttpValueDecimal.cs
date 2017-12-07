using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueDecimal : SttpValue
    {
        public readonly decimal Value;

        public SttpValueDecimal(decimal value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueDecimalMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueDecimalMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueDecimalMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueDecimalMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueDecimalMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueDecimalMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueDecimalMethods.AsInt64(Value);
        public override byte AsByte => SttpValueDecimalMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueDecimalMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueDecimalMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueDecimalMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueDecimalMethods.AsSingle(Value);
        public override double AsDouble => SttpValueDecimalMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueDecimalMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueDecimalMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueDecimalMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueDecimalMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueDecimalMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueDecimalMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueDecimalMethods.AsBoolean(Value);
        public override char AsChar => SttpValueDecimalMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueDecimalMethods.AsGuid(Value);
        public override string AsString => SttpValueDecimalMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueDecimalMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueDecimalMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueDecimalMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueDecimalMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueDecimalMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueDecimalMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Decimal;
       
        public static string ToTypeString(decimal value)
        {
            return $"(decimal){value.ToString()}";
        }

        public static object ToNativeType(decimal value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(decimal value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(decimal value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(decimal value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(decimal value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(decimal value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(decimal value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(decimal value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(decimal value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(decimal value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(decimal value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(decimal value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(decimal value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(decimal value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(decimal value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
