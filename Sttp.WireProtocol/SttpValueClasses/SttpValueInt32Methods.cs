using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.SttpValueClasses
{
    internal static class SttpValueInt32Methods
    {
        public static SttpValueTypeCode ValueTypeCode => SttpValueTypeCode.Int32;

        public static bool IsNull(int value)
        {
            return false;
        }

        public static string ToTypeString(int value)
        {
            return $"(int){value.ToString()}";
        }

        public static object ToNativeType(int value)
        {
            return value;
        }

        public static int GetHashCode(int value)
        {
            return (SttpValueTypeCode.Int32.GetHashCode() * 397) ^ value.GetHashCode();
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

        public static byte[] AsBuffer(int value)
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
