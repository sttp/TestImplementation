using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSByte : SttpValue
    {
        public readonly sbyte Value;

        public SttpValueSByte(sbyte value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSByteMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSByteMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSByteMethods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueSByteMethods.AsSByte(Value);
        public override short AsInt16 => SttpValueSByteMethods.AsInt16(Value);
        public override int AsInt32 => SttpValueSByteMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueSByteMethods.AsInt64(Value);
        public override byte AsByte => SttpValueSByteMethods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueSByteMethods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueSByteMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueSByteMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSByteMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSByteMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSByteMethods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueSByteMethods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueSByteMethods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueSByteMethods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueSByteMethods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueSByteMethods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueSByteMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSByteMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSByteMethods.AsGuid(Value);
        public override string AsString => SttpValueSByteMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSByteMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSByteMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSByteMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSByteMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSByteMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueSByteMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.SByte;
       
        public static string ToTypeString(sbyte value)
        {
            return $"(sbyte){value.ToString()}";
        }

        public static object ToNativeType(sbyte value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(sbyte value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(sbyte value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(sbyte value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(sbyte value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(sbyte value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(sbyte value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(sbyte value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(sbyte value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(sbyte value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(sbyte value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(sbyte value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(sbyte value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(sbyte value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(sbyte value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
