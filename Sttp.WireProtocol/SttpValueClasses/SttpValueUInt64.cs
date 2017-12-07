using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueUInt64 : SttpValue
    {
        public readonly ulong Value;

        public SttpValueUInt64(ulong value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueUInt64Methods.ValueTypeCode;
        public override string ToTypeString => SttpValueUInt64Methods.ToTypeString(Value);
        public override object ToNativeType => SttpValueUInt64Methods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueUInt64Methods.AsSByte(Value);
        public override short AsInt16 => SttpValueUInt64Methods.AsInt16(Value);
        public override int AsInt32 => SttpValueUInt64Methods.AsInt32(Value);
        public override long AsInt64 => SttpValueUInt64Methods.AsInt64(Value);
        public override byte AsByte => SttpValueUInt64Methods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueUInt64Methods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueUInt64Methods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueUInt64Methods.AsUInt64(Value);
        public override float AsSingle => SttpValueUInt64Methods.AsSingle(Value);
        public override double AsDouble => SttpValueUInt64Methods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueUInt64Methods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueUInt64Methods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueUInt64Methods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueUInt64Methods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueUInt64Methods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueUInt64Methods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueUInt64Methods.AsBoolean(Value);
        public override char AsChar => SttpValueUInt64Methods.AsChar(Value);
        public override Guid AsGuid => SttpValueUInt64Methods.AsGuid(Value);
        public override string AsString => SttpValueUInt64Methods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueUInt64Methods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueUInt64Methods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueUInt64Methods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueUInt64Methods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueUInt64Methods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueUInt64Methods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.UInt64;
       
        public static string ToTypeString(ulong value)
        {
            return $"(ulong){value.ToString()}";
        }

        public static object ToNativeType(ulong value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(ulong value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(ulong value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(ulong value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(ulong value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(ulong value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(ulong value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(ulong value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(ulong value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(ulong value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(ulong value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(ulong value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(ulong value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(ulong value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(ulong value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}