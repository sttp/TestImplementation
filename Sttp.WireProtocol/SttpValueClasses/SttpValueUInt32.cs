using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueUInt32 : SttpValue
    {
        public readonly uint Value;

        public SttpValueUInt32(uint value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueUInt32Methods.ValueTypeCode;
        public override string ToTypeString => SttpValueUInt32Methods.ToTypeString(Value);
        public override object ToNativeType => SttpValueUInt32Methods.ToNativeType(Value);
        public override int AsInt32 => SttpValueUInt32Methods.AsInt32(Value);
        public override long AsInt64 => SttpValueUInt32Methods.AsInt64(Value);
        public override uint AsUInt32 => SttpValueUInt32Methods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueUInt32Methods.AsUInt64(Value);
        public override float AsSingle => SttpValueUInt32Methods.AsSingle(Value);
        public override double AsDouble => SttpValueUInt32Methods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueUInt32Methods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueUInt32Methods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueUInt32Methods.AsBoolean(Value);
        public override char AsChar => SttpValueUInt32Methods.AsChar(Value);
        public override Guid AsGuid => SttpValueUInt32Methods.AsGuid(Value);
        public override string AsString => SttpValueUInt32Methods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueUInt32Methods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueUInt32Methods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueUInt32Methods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueUInt32Methods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueUInt32Methods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueUInt32Methods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.UInt32;
       
        public static string ToTypeString(uint value)
        {
            return $"(uint){value.ToString()}";
        }

        public static object ToNativeType(uint value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(uint value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(uint value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(uint value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(uint value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(uint value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(uint value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(uint value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(uint value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(uint value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(uint value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(uint value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static TimeSpan AsTimeSpan(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(uint value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(uint value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(uint value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
