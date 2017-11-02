using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Extension methods for SttpDataPoint to ease usability.
    /// </summary>
    public static class ISttpValueExtensions
    {
        public static void SetValue(this ISttpValue dataPoint, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                dataPoint.IsNull = true;
                return;
            }

            var type = value.GetType();
            if (type == typeof(byte))
            {
                dataPoint.SetValue((byte)value);
            }
            else if (type == typeof(byte))
            {
                dataPoint.SetValue((byte)value);
            }
            else if (type == typeof(short))
            {
                dataPoint.SetValue((short)value);
            }
            else if (type == typeof(int))
            {
                dataPoint.SetValue((int)value);
            }
            else if (type == typeof(long))
            {
                dataPoint.SetValue((long)value);
            }
            else if (type == typeof(ushort))
            {
                dataPoint.SetValue((ushort)value);
            }
            else if (type == typeof(uint))
            {
                dataPoint.SetValue((uint)value);
            }
            else if (type == typeof(ulong))
            {
                dataPoint.SetValue((ulong)value);
            }
            else if (type == typeof(decimal))
            {
                dataPoint.SetValue((decimal)value);
            }
            else if (type == typeof(double))
            {
                dataPoint.SetValue((double)value);
            }
            else if (type == typeof(float))
            {
                dataPoint.SetValue((float)value);
            }
            else if (type == typeof(DateTime))
            {
                dataPoint.SetValue((DateTime)value);
            }
            else if (type == typeof(TimeSpan))
            {
                dataPoint.SetValue((TimeSpan)value);
            }
            else if (type == typeof(char))
            {
                dataPoint.SetValue((char)value);
            }
            else if (type == typeof(bool))
            {
                dataPoint.SetValue((bool)value);
            }
            else if (type == typeof(Guid))
            {
                dataPoint.SetValue((Guid)value);
            }
            else if (type == typeof(string))
            {
                dataPoint.SetValue((string)value);
            }
            else if (type == typeof(byte[]))
            {
                dataPoint.SetValue((byte[])value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

        public static void SetValue(this ISttpValue dataPoint, byte value)
        {
            dataPoint.AsUInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, short value)
        {
            dataPoint.AsInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, int value)
        {
            dataPoint.AsInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, long value)
        {
            dataPoint.AsInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, ushort value)
        {
            dataPoint.AsUInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, uint value)
        {
            dataPoint.AsUInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, ulong value)
        {
            dataPoint.AsUInt64 = (ulong)value;
        }

        public static void SetValue(this ISttpValue dataPoint, decimal value)
        {
            dataPoint.AsBuffer = BigEndian.GetBytes(value);
        }

        public static void SetValue(this ISttpValue dataPoint, double value)
        {
            dataPoint.AsDouble = value;
        }

        public static void SetValue(this ISttpValue dataPoint, float value)
        {
            dataPoint.AsSingle = value;
        }

        public static void SetValue(this ISttpValue dataPoint, DateTime value)
        {
            dataPoint.AsBuffer = BigEndian.GetBytes(value);
        }

        public static void SetValue(this ISttpValue dataPoint, TimeSpan value)
        {
            dataPoint.AsBuffer = BigEndian.GetBytes(value);
        }

        public static void SetValue(this ISttpValue dataPoint, char value)
        {
            dataPoint.AsUInt64 = value;
        }

        public static void SetValue(this ISttpValue dataPoint, bool value)
        {
            dataPoint.AsUInt64 = value ? 1u : 0u;
        }

        public static void SetValue(this ISttpValue dataPoint, Guid value)
        {
            dataPoint.AsBuffer = BigEndian.GetBytes(value);
        }

        public static void SetValue(this ISttpValue dataPoint, string value)
        {
            dataPoint.AsBuffer = Encoding.UTF8.GetBytes(value);
        }

        public static void SetValue(this ISttpValue dataPoint, byte[] value)
        {
            dataPoint.AsBuffer = value;
        }

        /// <summary>
        /// returns the boxed native type of the value. 
        /// </summary>
        /// <returns></returns>
        public static object ToNativeType(this ISttpValue dataPoint, SttpValueTypeCode nativeType)
        {
            if (dataPoint.IsNull)
                return null;

            switch (nativeType)
            {
                case SttpValueTypeCode.Null:
                    return null;
                case SttpValueTypeCode.Byte:
                    return dataPoint.ToByte();
                case SttpValueTypeCode.Int16:
                    return dataPoint.ToInt16();
                case SttpValueTypeCode.Int32:
                    return dataPoint.ToInt32();
                case SttpValueTypeCode.Int64:
                    return dataPoint.ToInt64();
                case SttpValueTypeCode.UInt16:
                    return dataPoint.ToUInt16();
                case SttpValueTypeCode.UInt32:
                    return dataPoint.ToUInt32();
                case SttpValueTypeCode.UInt64:
                    return dataPoint.ToUInt64();
                case SttpValueTypeCode.Decimal:
                    return dataPoint.ToDecimal();
                case SttpValueTypeCode.Double:
                    return dataPoint.ToDouble();
                case SttpValueTypeCode.Single:
                    return dataPoint.ToSingle();
                case SttpValueTypeCode.DateTime:
                    return dataPoint.ToDateTime();
                case SttpValueTypeCode.TimeSpan:
                    return dataPoint.ToTimeSpan();
                case SttpValueTypeCode.Char:
                    return dataPoint.ToChar();
                case SttpValueTypeCode.Bool:
                    return dataPoint.ToBool();
                case SttpValueTypeCode.Guid:
                    return dataPoint.ToGuid();
                case SttpValueTypeCode.String:
                    return dataPoint.ToString();
                case SttpValueTypeCode.Buffer:
                    return dataPoint.ToBuffer();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region [ implicit operators Cast From EcaMeasurementValue ]

        //ToDo: Consider bounds checking for all of these conversions.
        public static byte ToByte(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (byte)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (byte)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (byte)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (byte)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static short ToInt16(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (short)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (short)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (short)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (short)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int ToInt32(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (int)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (int)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (int)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (int)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static long ToInt64(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (long)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (long)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (long)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (long)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static ushort ToUInt16(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (ushort)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (ushort)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (ushort)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (ushort)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static uint ToUInt32(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (uint)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (uint)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (uint)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (uint)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static ulong ToUInt64(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (ulong)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (ulong)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (ulong)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (ulong)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static decimal ToDecimal(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (decimal)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (decimal)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (decimal)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (decimal)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static double ToDouble(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (double)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (double)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (double)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (double)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static float ToSingle(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (float)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (float)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (float)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (float)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static DateTime ToDateTime(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64");
                case SttpFundamentalTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64");
                case SttpFundamentalTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single");
                case SttpFundamentalTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double");
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static TimeSpan ToTimeSpan(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64");
                case SttpFundamentalTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64");
                case SttpFundamentalTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single");
                case SttpFundamentalTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double");
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static char ToChar(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return (char)dataPoint.AsInt64;
                case SttpFundamentalTypeCode.UInt64:
                    return (char)dataPoint.AsUInt64;
                case SttpFundamentalTypeCode.Single:
                    return (char)dataPoint.AsSingle;
                case SttpFundamentalTypeCode.Double:
                    return (char)dataPoint.AsDouble;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool ToBool(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.Int64:
                    return dataPoint.AsInt64 != 0;
                case SttpFundamentalTypeCode.UInt64:
                    return dataPoint.AsUInt64 != 0;
                case SttpFundamentalTypeCode.Single:
                    return Math.Abs(dataPoint.AsSingle) > 0.75;
                case SttpFundamentalTypeCode.Double:
                    return Math.Abs(dataPoint.AsDouble) > 0.75;
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Guid ToGuid(this ISttpValue dataPoint)
        {
            switch (dataPoint.FundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null");
                case SttpFundamentalTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64");
                case SttpFundamentalTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64");
                case SttpFundamentalTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single");
                case SttpFundamentalTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double");
                case SttpFundamentalTypeCode.Buffer:
                    throw new InvalidCastException("Cannot convert from Buffer");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static byte[] ToBuffer(this ISttpValue dataPoint)
        {
            return null;
        }

        #endregion


    }
}
