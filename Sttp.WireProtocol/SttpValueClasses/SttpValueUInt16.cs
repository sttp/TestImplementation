using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueUInt16 : SttpValue
    {
        public readonly ushort Value;

        public SttpValueUInt16(ushort value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueUInt16Methods.ValueTypeCode;
        public override string ToTypeString => SttpValueUInt16Methods.ToTypeString(Value);
        public override object ToNativeType => SttpValueUInt16Methods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueUInt16Methods.AsSByte(Value);
        public override short AsInt16 => SttpValueUInt16Methods.AsInt16(Value);
        public override int AsInt32 => SttpValueUInt16Methods.AsInt32(Value);
        public override long AsInt64 => SttpValueUInt16Methods.AsInt64(Value);
        public override byte AsByte => SttpValueUInt16Methods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueUInt16Methods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueUInt16Methods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueUInt16Methods.AsUInt64(Value);
        public override float AsSingle => SttpValueUInt16Methods.AsSingle(Value);
        public override double AsDouble => SttpValueUInt16Methods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueUInt16Methods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueUInt16Methods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueUInt16Methods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueUInt16Methods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueUInt16Methods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueUInt16Methods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueUInt16Methods.AsBoolean(Value);
        public override char AsChar => SttpValueUInt16Methods.AsChar(Value);
        public override Guid AsGuid => SttpValueUInt16Methods.AsGuid(Value);
        public override string AsString => SttpValueUInt16Methods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueUInt16Methods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueUInt16Methods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueUInt16Methods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueUInt16Methods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueUInt16Methods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueUInt16Methods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.UInt16;
       
        public static string ToTypeString(ushort value)
        {
            return $"(ushort){value.ToString()}";
        }

        public static object ToNativeType(ushort value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(ushort value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(ushort value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(ushort value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(ushort value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(ushort value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(ushort value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(ushort value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(ushort value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(ushort value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(ushort value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(ushort value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(ushort value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(ushort value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(ushort value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
