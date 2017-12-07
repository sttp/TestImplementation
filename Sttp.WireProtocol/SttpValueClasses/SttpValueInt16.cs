using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueInt16 : SttpValue
    {
        public readonly short Value;

        public SttpValueInt16(short value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueInt16Methods.ValueTypeCode;
        public override string ToTypeString => SttpValueInt16Methods.ToTypeString(Value);
        public override object ToNativeType => SttpValueInt16Methods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueInt16Methods.AsSByte(Value);
        public override short AsInt16 => SttpValueInt16Methods.AsInt16(Value);
        public override int AsInt32 => SttpValueInt16Methods.AsInt32(Value);
        public override long AsInt64 => SttpValueInt16Methods.AsInt64(Value);
        public override byte AsByte => SttpValueInt16Methods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueInt16Methods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueInt16Methods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueInt16Methods.AsUInt64(Value);
        public override float AsSingle => SttpValueInt16Methods.AsSingle(Value);
        public override double AsDouble => SttpValueInt16Methods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueInt16Methods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueInt16Methods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueInt16Methods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueInt16Methods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueInt16Methods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueInt16Methods.AsTimeSpan(Value);
        public override bool AsBoolean => SttpValueInt16Methods.AsBoolean(Value);
        public override char AsChar => SttpValueInt16Methods.AsChar(Value);
        public override Guid AsGuid => SttpValueInt16Methods.AsGuid(Value);
        public override string AsString => SttpValueInt16Methods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueInt16Methods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueInt16Methods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueInt16Methods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueInt16Methods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueInt16Methods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueInt16Methods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Int16;
       
        public static string ToTypeString(short value)
        {
            return $"(short){value.ToString()}";
        }

        public static object ToNativeType(short value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(short value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(short value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(short value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(short value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(short value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(short value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(short value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(short value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(short value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(short value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(short value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static SttpTimeOffset AsSttpTimeOffset(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTimeOffset");
        }

        public static TimeSpan AsTimeSpan(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(short value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(short value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsValueSet(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsNamedSet(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(short value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
