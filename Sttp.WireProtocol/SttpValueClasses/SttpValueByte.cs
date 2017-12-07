using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueByte : SttpValue
    {
        public readonly byte Value;

        public SttpValueByte(byte value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueByteMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueByteMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueByteMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueByteMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueByteMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueByteMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueByteMethods.AsInt64(Value);
        public override byte AsByte => SttpValueByteMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueByteMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueByteMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueByteMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueByteMethods.AsSingle(Value);
        public override double AsDouble => SttpValueByteMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueByteMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueByteMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueByteMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueByteMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueByteMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueByteMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueByteMethods.AsBoolean(Value);
        public override char AsChar => SttpValueByteMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueByteMethods.AsGuid(Value);
        public override string AsString => SttpValueByteMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueByteMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueByteMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueByteMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueByteMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueByteMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueByteMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Byte;
       
        public static string ToTypeString(byte value)
        {
            return $"(byte){value.ToString()}";
        }

        public static object ToNativeType(byte value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(byte value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(byte value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(byte value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(byte value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(byte value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(byte value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(byte value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(byte value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(byte value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(byte value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(byte value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(byte value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(byte value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(byte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
