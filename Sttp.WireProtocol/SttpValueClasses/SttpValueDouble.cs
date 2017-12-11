using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueDouble : SttpValue
    {
        public readonly double Value;

        public SttpValueDouble(double value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueDoubleMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueDoubleMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueDoubleMethods.ToNativeType(Value);
        public override long AsInt64 => SttpValueDoubleMethods.AsInt64(Value);
        public override ulong AsUInt64 => SttpValueDoubleMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueDoubleMethods.AsSingle(Value);
        public override double AsDouble => SttpValueDoubleMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueDoubleMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueDoubleMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueDoubleMethods.AsBoolean(Value);
        public override char AsChar => SttpValueDoubleMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueDoubleMethods.AsGuid(Value);
        public override string AsString => SttpValueDoubleMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueDoubleMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueDoubleMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueDoubleMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueDoubleMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueDoubleMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueDoubleMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Double;
       
        public static string ToTypeString(double value)
        {
            return $"(double){value.ToString()}";
        }

        public static object ToNativeType(double value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(double value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(double value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(double value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(double value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(double value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(double value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(double value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(double value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(double value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(double value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(double value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static TimeSpan AsTimeSpan(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(double value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(double value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(double value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
