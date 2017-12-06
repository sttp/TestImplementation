using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    public class SttpValueInt32 : SttpValue
    {
        public readonly int Value;

        public SttpValueInt32(int value)
        {
            Value = value;
        }

        public override SttpValueTypeCode ValueTypeCode => SttpValueInt32Methods.ValueTypeCode;
        public override string ToTypeString => SttpValueInt32Methods.ToTypeString(Value);
        public override object ToNativeType => SttpValueInt32Methods.ToNativeType(Value);
        public override sbyte AsSByte => SttpValueInt32Methods.AsSByte(Value);
        public override short AsInt16 => SttpValueInt32Methods.AsInt16(Value);
        public override int AsInt32 => SttpValueInt32Methods.AsInt32(Value);
        public override long AsInt64 => SttpValueInt32Methods.AsInt64(Value);
        public override byte AsByte => SttpValueInt32Methods.AsByte(Value);
        public override ushort AsUInt16 => SttpValueInt32Methods.AsUInt16(Value);
        public override uint AsUInt32 => SttpValueInt32Methods.AsUInt32(Value);
        public override ulong AsUInt64 => SttpValueInt32Methods.AsUInt64(Value);
        public override float AsSingle => SttpValueInt32Methods.AsSingle(Value);
        public override double AsDouble => SttpValueInt32Methods.AsDouble(Value);
        public override decimal AsDecimal => SttpValueInt32Methods.AsDecimal(Value);
        public override DateTime AsDateTime => SttpValueInt32Methods.AsDateTime(Value);
        public override DateTimeOffset AsDateTimeOffset => SttpValueInt32Methods.AsDateTimeOffset(Value);
        public override SttpTime AsSttpTime => SttpValueInt32Methods.AsSttpTime(Value);
        public override SttpTimeOffset AsSttpTimeOffset => SttpValueInt32Methods.AsSttpTimeOffset(Value);
        public override TimeSpan AsTimeSpan => SttpValueInt32Methods.AsTimeSpan(Value);
        public override bool AsBool => SttpValueInt32Methods.AsBool(Value);
        public override char AsChar => SttpValueInt32Methods.AsChar(Value);
        public override Guid AsGuid => SttpValueInt32Methods.AsGuid(Value);
        public override string AsString => SttpValueInt32Methods.AsString(Value);
        public override SttpBuffer AsBuffer => SttpValueInt32Methods.AsBuffer(Value);
        public override SttpValueSet AsSttpValueSet => SttpValueInt32Methods.AsValueSet(Value);
        public override SttpNamedSet AsSttpNamedSet => SttpValueInt32Methods.AsNamedSet(Value);
        public override SttpMarkup AsSttpMarkup => SttpValueInt32Methods.AsSttpMarkup(Value);
        public override Guid AsBulkTransportGuid => SttpValueInt32Methods.AsBulkTransportGuid(Value);
    }



    internal static class SttpValueInt32Methods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Int32;
       
        public static string ToTypeString(int value)
        {
            return $"(int){value.ToString()}";
        }

        public static object ToNativeType(int value)
        {
            return value;
        }

        #region [ Type Casting ]

        public static sbyte AsSByte(int value)
        {
            checked
            {
                return (sbyte)value;
            }
        }
        public static short AsInt16(int value)
        {
            checked
            {
                return (short)value;
            }
        }

        public static int AsInt32(int value)
        {
            checked
            {
                return (int)value;
            }
        }

        public static long AsInt64(int value)
        {
            checked
            {
                return (long)value;
            }
        }

        public static byte AsByte(int value)
        {
            checked
            {
                return (byte)value;
            }
        }

        public static ushort AsUInt16(int value)
        {
            checked
            {
                return (ushort)value;
            }
        }

        public static uint AsUInt32(int value)
        {
            checked
            {
                return (uint)value;
            }
        }

        public static ulong AsUInt64(int value)
        {
            checked
            {
                return (ulong)value;
            }
        }

        public static float AsSingle(int value)
        {
            checked
            {
                return (float)value;
            }
        }

        public static double AsDouble(int value)
        {
            checked
            {
                return (double)value;
            }
        }

        public static decimal AsDecimal(int value)
        {
            checked
            {
                return (decimal)value;
            }
        }

        public static DateTime AsDateTime(int value)
        {
            throw new InvalidCastException();
        }

        public static DateTimeOffset AsDateTimeOffset(int value)
        {
            throw new InvalidCastException();
        }

        public static SttpTime AsSttpTime(int value)
        {
            throw new InvalidCastException();
        }

        public static SttpTimeOffset AsSttpTimeOffset(int value)
        {
            throw new InvalidCastException();
        }

        public static TimeSpan AsTimeSpan(int value)
        {
            throw new InvalidCastException();
        }

        public static bool AsBool(int value)
        {
            throw new InvalidCastException();
        }

        public static char AsChar(int value)
        {
            checked
            {
                return (char)value;
            }
        }

        public static Guid AsGuid(int value)
        {
            throw new InvalidCastException();
        }

        public static string AsString(int value)
        {
            return value.ToString();
        }

        public static SttpBuffer AsBuffer(int value)
        {
            throw new InvalidCastException();
        }

        public static SttpValueSet AsValueSet(int value)
        {
            throw new InvalidCastException();
        }

        public static SttpNamedSet AsNamedSet(int value)
        {
            throw new InvalidCastException();
        }

        public static SttpMarkup AsSttpMarkup(int value)
        {
            throw new InvalidCastException();
        }

        public static Guid AsBulkTransportGuid(int value)
        {
            throw new InvalidCastException();
        }

        #endregion
    }
}
