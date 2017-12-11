using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueSingle : SttpValue
    {
        public readonly float Value;

        public SttpValueSingle(float value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueSingleMethods.ValueTypeCode;
        public override string ToTypeString => SttpValueSingleMethods.ToTypeString(Value);
        public override object ToNativeType => SttpValueSingleMethods.ToNativeType(Value);
        public override int AsInt32 => SttpValueSingleMethods.AsInt32(Value);
        public override long AsInt64 => SttpValueSingleMethods.AsInt64(Value);
        public override uint AsUInt32 => SttpValueSingleMethods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueSingleMethods.AsUInt64(Value);
        public override float AsSingle => SttpValueSingleMethods.AsSingle(Value);
        public override double AsDouble => SttpValueSingleMethods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueSingleMethods.AsDecimal(Value);
        public override SttpTime AsSttpTime => SttpValueSingleMethods.AsSttpTime(Value);
        public override bool AsBoolean => SttpValueSingleMethods.AsBoolean(Value);
        public override char AsChar => SttpValueSingleMethods.AsChar(Value);
        public override Guid AsGuid => SttpValueSingleMethods.AsGuid(Value);
        public override string AsString => SttpValueSingleMethods.AsString(Value);
        public override SttpBuffer AsSttpBuffer => SttpValueSingleMethods.AsSttpBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueSingleMethods.AsSttpValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueSingleMethods.AsSttpNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueSingleMethods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueSingleMethods.AsBulkTransportGuid(Value);
    }

    internal static class SttpValueSingleMethods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Single;
       
        public static string ToTypeString(float value)
        {
            return $"(float){value.ToString()}";
        }

        public static object ToNativeType(float value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(float value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(float value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(float value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(float value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(float value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(float value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(float value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(float value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(float value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(float value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(float value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTime");
        }

        public static DateTimeOffset AsDateTimeOffset(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to DateTimeOffset");
        }

        public static SttpTime AsSttpTime(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpTime");
        }

        public static TimeSpan AsTimeSpan(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to TimeSpan");
        }

        public static bool AsBoolean(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Boolean");
        }

        public static char AsChar(float value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to Guid");
        }

        public static string AsString(float value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsSttpBuffer(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpBuffer");
        }

        public static SttpValueSet AsSttpValueSet(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpValueSet");
        }

        public static SttpNamedSet AsSttpNamedSet(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpNamedSet");
        }

        public static SttpMarkup AsSttpMarkup(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to SttpMarkup");
        }

        public static Guid AsBulkTransportGuid(float value)
        {
            throw new InvalidCastException($"Cannot cast from {ToTypeString(value)} to BulkTransportGuid");
        }

        #endregion
    }
}
