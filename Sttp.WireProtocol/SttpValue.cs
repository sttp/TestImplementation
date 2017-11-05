using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpValue
    {
        #region [ Members ]

        [FieldOffset(0)]
        private long m_rawBytes0_7;
        [FieldOffset(8)]
        private long m_rawBytes8_15;

        [FieldOffset(0)]
        private sbyte m_valueSByte;
        [FieldOffset(0)]
        private byte m_valueByte;
        [FieldOffset(0)]
        private short m_valueInt16;
        [FieldOffset(0)]
        private int m_valueInt32;
        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private ushort m_valueUInt16;
        [FieldOffset(0)]
        private uint m_valueUInt32;
        [FieldOffset(0)]
        private ulong m_valueUInt64;
        [FieldOffset(0)]
        private double m_valueDouble;
        [FieldOffset(0)]
        private float m_valueSingle;
        [FieldOffset(0)]
        private TimeSpan m_valueTimeSpan;
        [FieldOffset(0)]
        private char m_valueChar;
        [FieldOffset(0)]
        private bool m_valueBool;
        [FieldOffset(0)]
        private SttpTimestamp m_valueSttpTimestamp;
        [FieldOffset(0)]
        private SttpTimestampOffset m_valueSttpTimestampOffset;
        [FieldOffset(0)]
        private decimal m_valueDecimal;
        [FieldOffset(0)]
        private Guid m_valueGuid;

        [FieldOffset(16)]
        private object m_valueObject;

        [FieldOffset(33)]
        private SttpValueTypeCode m_valueTypeCode;

        #endregion

        #region [ Constructors ]

        public SttpValue()
        {
            IsNull = true;
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. 
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpValue(SttpValue value)
        {
            m_rawBytes0_7 = value.m_rawBytes0_7;
            m_rawBytes8_15 = value.m_rawBytes8_15;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.m_valueTypeCode;
        }

        #endregion

        #region [ Properties ]

        public sbyte AsSByte
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (sbyte)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (sbyte)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (sbyte)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (sbyte)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (sbyte)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (sbyte)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (sbyte)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (sbyte)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (sbyte)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (sbyte)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (sbyte)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (sbyte)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return sbyte.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.SByte;
                m_valueSByte = value;
            }
        }

        public short AsInt16
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (short)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (short)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (short)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (short)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (short)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (short)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (short)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (short)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (short)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (short)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (short)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (short)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return short.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Int16;
                m_valueInt16 = value;

            }
        }

        public int AsInt32
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (int)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (int)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (int)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (int)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (int)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (int)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (int)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (int)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (int)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (int)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (int)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (int)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return int.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Int32;
                m_valueInt32 = value;

            }
        }

        public long AsInt64
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (long)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (long)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (long)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (long)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (long)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (long)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (long)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (long)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (long)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (long)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (long)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (long)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return long.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Int64;
                m_valueInt64 = value;
            }
        }

        public byte AsByte
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (byte)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (byte)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (byte)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (byte)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (byte)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (byte)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (byte)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (byte)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (byte)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (byte)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (byte)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (byte)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (byte)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return byte.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Byte;
                m_valueByte = value;
            }
        }

        public ushort AsUInt16
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (ushort)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (ushort)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (ushort)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (ushort)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (ushort)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (ushort)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (ushort)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (ushort)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (ushort)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (ushort)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (ushort)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (ushort)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (ushort)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return ushort.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.UInt16;
                m_valueUInt16 = value;
            }
        }

        public uint AsUInt32
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (uint)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (uint)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (uint)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (uint)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (uint)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (uint)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (uint)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (uint)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (uint)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (uint)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (uint)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (uint)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (uint)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return uint.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.UInt32;
                m_valueUInt32 = value;
            }
        }

        public ulong AsUInt64
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (ulong)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (ulong)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (ulong)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (ulong)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (ulong)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (ulong)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (ulong)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (ulong)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (ulong)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (ulong)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (ulong)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (ulong)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (ulong)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return ulong.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.UInt64;
                m_valueUInt64 = value;
            }
        }

        public decimal AsDecimal
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (decimal)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (decimal)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (decimal)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (decimal)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (decimal)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (decimal)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (decimal)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (decimal)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (decimal)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (decimal)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (decimal)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (decimal)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (decimal)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return decimal.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Decimal;
                m_valueDecimal = value;
            }
        }

        public double AsDouble
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (double)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (double)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (double)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (double)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (double)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (double)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (double)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (double)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (double)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (double)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (double)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (double)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (double)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return double.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Double;
                m_valueDouble = value;
            }
        }

        public float AsSingle
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (float)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (float)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (float)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (float)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (float)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (float)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (float)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (float)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (float)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (float)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (float)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (float)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (float)(m_valueBool ? 0 : 1);
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return float.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Single;
                m_valueSingle = value;
            }
        }

        public SttpTimestamp AsSttpTimestamp
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTimestamp;
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimestampOffset.ToSttpTimestamp();
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return SttpTimestamp.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.SttpTime;
                m_valueSttpTimestamp = value;
            }
        }

        public SttpTimestampOffset AsSttpTimestampOffset
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTimestamp.ToSttpTimestampOffset();
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimestampOffset;
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return SttpTimestampOffset.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.SttpTimeOffset;
                m_valueSttpTimestampOffset = value;
            }
        }

        public TimeSpan AsTimeSpan
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            return m_valueTimeSpan;
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return TimeSpan.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.TimeSpan;
                m_valueTimeSpan = value;
            }
        }

        public char AsChar
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return (char)m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return (char)m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return (char)m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return (char)m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return (char)m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return (char)m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return (char)m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return (char)m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return (char)m_valueDecimal;
                        case SttpValueTypeCode.Double:
                            return (char)m_valueDouble;
                        case SttpValueTypeCode.Single:
                            return (char)m_valueSingle;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            return (char)m_valueChar;
                        case SttpValueTypeCode.Bool:
                            return (char)(m_valueBool ? 'T' : 'F');
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return char.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Char;
                m_valueChar = value;
            }
        }

        public bool AsBool
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            return 0 != m_valueSByte;
                        case SttpValueTypeCode.Int16:
                            return 0 != m_valueInt16;
                        case SttpValueTypeCode.Int32:
                            return 0 != m_valueInt32;
                        case SttpValueTypeCode.Int64:
                            return 0 != m_valueInt64;
                        case SttpValueTypeCode.Byte:
                            return 0 != m_valueByte;
                        case SttpValueTypeCode.UInt16:
                            return 0 != m_valueUInt16;
                        case SttpValueTypeCode.UInt32:
                            return 0 != m_valueUInt32;
                        case SttpValueTypeCode.UInt64:
                            return 0 != m_valueUInt64;
                        case SttpValueTypeCode.Decimal:
                            return Math.Abs(m_valueDecimal) > 0.75M;
                        case SttpValueTypeCode.Double:
                            return Math.Abs(m_valueDouble) > 0.75;
                        case SttpValueTypeCode.Single:
                            return Math.Abs(m_valueSingle) > 0.75;
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            switch (m_valueChar)
                            {
                                case '1':
                                case 'T':
                                    return true;
                                case '0':
                                case 'F':
                                    return false;
                            }
                            throw new InvalidCastException("Cannot cast from Char");
                        case SttpValueTypeCode.Bool:
                            return m_valueBool;
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return bool.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Bool;
                m_valueBool = value;
            }
        }

        public Guid AsGuid
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            return m_valueGuid;
                        case SttpValueTypeCode.String:
                            return Guid.Parse((string)m_valueObject);
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Guid;
                m_valueGuid = value;
            }
        }

        public string AsString
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            return null;
                        case SttpValueTypeCode.SByte:
                            return m_valueSByte.ToString();
                        case SttpValueTypeCode.Int16:
                            return m_valueInt16.ToString();
                        case SttpValueTypeCode.Int32:
                            return m_valueInt32.ToString();
                        case SttpValueTypeCode.Int64:
                            return m_valueInt64.ToString();
                        case SttpValueTypeCode.Byte:
                            return m_valueByte.ToString();
                        case SttpValueTypeCode.UInt16:
                            return m_valueUInt16.ToString();
                        case SttpValueTypeCode.UInt32:
                            return m_valueUInt32.ToString();
                        case SttpValueTypeCode.UInt64:
                            return m_valueUInt64.ToString();
                        case SttpValueTypeCode.Decimal:
                            return m_valueDecimal.ToString();
                        case SttpValueTypeCode.Double:
                            return m_valueDouble.ToString();
                        case SttpValueTypeCode.Single:
                            return m_valueSingle.ToString();
                        case SttpValueTypeCode.SttpTime:
                            return m_valueSttpTimestamp.ToString();
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimestampOffset.ToString();
                        case SttpValueTypeCode.TimeSpan:
                            return m_valueTimeSpan.ToString();
                        case SttpValueTypeCode.Char:
                            return m_valueChar.ToString();
                        case SttpValueTypeCode.Bool:
                            return m_valueBool.ToString();
                        case SttpValueTypeCode.Guid:
                            return m_valueGuid.ToString();
                        case SttpValueTypeCode.String:
                            return (string)m_valueObject;
                        case SttpValueTypeCode.Buffer:
                            //ToDo: Return a byte string 0x292A78B402;
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.String;
                m_valueObject = value;
            }
        }

        public byte[] AsBuffer
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            throw new InvalidCastException("Cannot cast from String");
                        case SttpValueTypeCode.Buffer:
                            return (byte[])m_valueObject;
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Buffer;
                m_valueObject = value;
            }
        }

        public SttpSet AsSet
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            throw new InvalidCastException("Cannot cast from String");
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            return (SttpSet)m_valueObject;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.Set;
                m_valueObject = value;
            }
        }

        public SttpNamedSet AsNamedSet
        {
            get
            {
                checked
                {
                    switch (m_valueTypeCode)
                    {
                        case SttpValueTypeCode.Null:
                            throw new InvalidCastException("Cannot cast from Null");
                        case SttpValueTypeCode.SByte:
                            throw new InvalidCastException("Cannot cast from SByte");
                        case SttpValueTypeCode.Int16:
                            throw new InvalidCastException("Cannot cast from Int16");
                        case SttpValueTypeCode.Int32:
                            throw new InvalidCastException("Cannot cast from Int32");
                        case SttpValueTypeCode.Int64:
                            throw new InvalidCastException("Cannot cast from Int64");
                        case SttpValueTypeCode.Byte:
                            throw new InvalidCastException("Cannot cast from Byte");
                        case SttpValueTypeCode.UInt16:
                            throw new InvalidCastException("Cannot cast from UInt16");
                        case SttpValueTypeCode.UInt32:
                            throw new InvalidCastException("Cannot cast from UInt32");
                        case SttpValueTypeCode.UInt64:
                            throw new InvalidCastException("Cannot cast from UInt64");
                        case SttpValueTypeCode.Decimal:
                            throw new InvalidCastException("Cannot cast from Decimal");
                        case SttpValueTypeCode.Double:
                            throw new InvalidCastException("Cannot cast from Double");
                        case SttpValueTypeCode.Single:
                            throw new InvalidCastException("Cannot cast from Single");
                        case SttpValueTypeCode.SttpTime:
                            throw new InvalidCastException("Cannot cast from SttpTime");
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new InvalidCastException("Cannot cast from SttpTimeOffset");
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            throw new InvalidCastException("Cannot cast from String");
                        case SttpValueTypeCode.Buffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.Set:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.NamedSet:
                            return (SttpNamedSet)m_valueObject;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            set
            {
                m_valueTypeCode = SttpValueTypeCode.NamedSet;
                m_valueObject = value;
            }
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        /// <exception cref="InvalidOperationException">When setting this value to true. Set a value by calling one of the <see cref="SetValue"/> methods.</exception>
        public bool IsNull
        {
            get
            {
                return m_valueTypeCode == SttpValueTypeCode.Null;
            }
            set
            {
                if (!value)
                    throw new InvalidOperationException("Can only set a value to null with this property, to set not null, use one of the other properties to set the value.");
                m_valueObject = null;
                m_valueTypeCode = SttpValueTypeCode.Null;
            }
        }


        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpValueTypeCode ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
        }

        #endregion

        #region [ Methods ] 

        /// <summary>
        /// Clones the value
        /// </summary>
        /// <returns></returns>
        public SttpValue Clone()
        {
            return new SttpValue(this);
        }

        public void SetValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                IsNull = true;
                return;
            }

            var type = value.GetType();
            if (type == typeof(sbyte))
            {
                SetValue((sbyte)value);
            }
            else if (type == typeof(short))
            {
                SetValue((short)value);
            }
            else if (type == typeof(int))
            {
                SetValue((int)value);
            }
            else if (type == typeof(long))
            {
                SetValue((long)value);
            }
            else if (type == typeof(byte))
            {
                SetValue((byte)value);
            }
            else if (type == typeof(ushort))
            {
                SetValue((ushort)value);
            }
            else if (type == typeof(uint))
            {
                SetValue((uint)value);
            }
            else if (type == typeof(ulong))
            {
                SetValue((ulong)value);
            }
            else if (type == typeof(float))
            {
                SetValue((float)value);
            }
            else if (type == typeof(double))
            {
                SetValue((double)value);
            }
            else if (type == typeof(decimal))
            {
                SetValue((decimal)value);
            }
            else if (type == typeof(SttpTimestamp))
            {
                SetValue((SttpTimestamp)value);
            }
            else if (type == typeof(SttpTimestampOffset))
            {
                SetValue((SttpTimestampOffset)value);
            }
            else if (type == typeof(DateTime))
            {
                SetValue((DateTime)value);
            }
            else if (type == typeof(DateTimeOffset))
            {
                SetValue((DateTimeOffset)value);
            }
            else if (type == typeof(TimeSpan))
            {
                SetValue((TimeSpan)value);
            }
            else if (type == typeof(bool))
            {
                SetValue((bool)value);
            }
            else if (type == typeof(char))
            {
                SetValue((char)value);
            }
            else if (type == typeof(Guid))
            {
                SetValue((Guid)value);
            }
            else if (type == typeof(string))
            {
                SetValue((string)value);
            }
            else if (type == typeof(byte[]))
            {
                SetValue((byte[])value);
            }
            else if (type == typeof(SttpSet))
            {
                SetValue((SttpSet)value);
            }
            else if (type == typeof(SttpNamedSet))
            {
                SetValue((SttpNamedSet)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

        public void SetValue(sbyte value)
        {
            AsSByte = value;
        }
        public void SetValue(short value)
        {
            AsInt16 = value;
        }
        public void SetValue(int value)
        {
            AsInt32 = value;
        }
        public void SetValue(long value)
        {
            AsInt64 = value;
        }
        public void SetValue(byte value)
        {
            AsByte = value;
        }
        public void SetValue(ushort value)
        {
            AsUInt16 = value;
        }
        public void SetValue(uint value)
        {
            AsUInt32 = value;
        }
        public void SetValue(ulong value)
        {
            AsUInt64 = value;
        }
        public void SetValue(float value)
        {
            AsSingle = value;
        }
        public void SetValue(double value)
        {
            AsDouble = value;
        }
        public void SetValue(decimal value)
        {
            AsDecimal = value;
        }
        public void SetValue(SttpTimestamp value)
        {
            AsSttpTimestamp = value;
        }
        public void SetValue(SttpTimestampOffset value)
        {
            AsSttpTimestampOffset = value;
        }
        public void SetValue(DateTime value)
        {
            AsSttpTimestamp = new SttpTimestamp(value);
        }
        public void SetValue(DateTimeOffset value)
        {
            AsSttpTimestampOffset = new SttpTimestampOffset(value);
        }
        public void SetValue(TimeSpan value)
        {
            AsTimeSpan = value;
        }
        public void SetValue(bool value)
        {
            AsBool = value;
        }
        public void SetValue(char value)
        {
            AsChar = value;
        }
        public void SetValue(Guid value)
        {
            AsGuid = value;
        }
        public void SetValue(string value)
        {
            AsString = value;
        }
        public void SetValue(byte[] value)
        {
            AsBuffer = value;
        }
        public void SetValue(SttpSet value)
        {
            AsSet = value;
        }
        public void SetValue(SttpNamedSet value)
        {
            AsNamedSet = value;
        }

        public static bool operator ==(SttpValue a, SttpValue b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (ReferenceEquals(a, null))
                return false;
            if (ReferenceEquals(b, null))
                return false;
            if (a.m_valueTypeCode != b.m_valueTypeCode)
                return false;
            switch (a.m_valueTypeCode)
            {
               //ToDo: Finish.
            }
            return true;
        }

        public static bool operator !=(SttpValue a, SttpValue b)
        {
            return !(a == b);
        }
        
        #endregion

        public void Load(PacketReader packetReader)
        {
            throw new NotImplementedException();
        }

        

        public object ToNativeType(SttpValueTypeCode typeCode)
        {
            throw new NotImplementedException();
        }

        public void Save(PacketWriter packetWriter)
        {
            throw new NotImplementedException();
        }
    }
}
