using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpValueMutable : SttpValue
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
        private SttpTime m_valueSttpTime;
        [FieldOffset(0)]
        private SttpTimeOffset m_valueSttpTimeOffset;
        [FieldOffset(0)]
        private DateTime m_valueDateTime;
        [FieldOffset(0)]
        private DateTimeOffset m_valueDateTimeOffset;
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

        public SttpValueMutable()
        {
            m_valueTypeCode = SttpValueTypeCode.Null;
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. 
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpValueMutable(SttpValueMutable value)
        {
            m_rawBytes0_7 = value.m_rawBytes0_7;
            m_rawBytes8_15 = value.m_rawBytes8_15;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.m_valueTypeCode;
        }

        public SttpValueMutable(PayloadReader rd)
        {
            switch ((SttpValueTypeCode)rd.ReadByte())
            {
                case SttpValueTypeCode.Null:
                    m_valueTypeCode = SttpValueTypeCode.Null;
                    break;
                case SttpValueTypeCode.SByte:
                    SetValue(rd.ReadSByte());
                    break;
                case SttpValueTypeCode.Int16:
                    SetValue(rd.ReadInt16());
                    break;
                case SttpValueTypeCode.Int32:
                    SetValue(rd.ReadInt32());
                    break;
                case SttpValueTypeCode.Int64:
                    SetValue(rd.ReadInt64());
                    break;
                case SttpValueTypeCode.Byte:
                    SetValue(rd.ReadByte());
                    break;
                case SttpValueTypeCode.UInt16:
                    SetValue(rd.ReadUInt16());
                    break;
                case SttpValueTypeCode.UInt32:
                    SetValue(rd.ReadUInt32());
                    break;
                case SttpValueTypeCode.UInt64:
                    SetValue(rd.ReadUInt64());
                    break;
                case SttpValueTypeCode.Single:
                    SetValue(rd.ReadSingle());
                    break;
                case SttpValueTypeCode.Double:
                    SetValue(rd.ReadDouble());
                    break;
                case SttpValueTypeCode.Decimal:
                    SetValue(rd.ReadDecimal());
                    break;
                case SttpValueTypeCode.DateTime:
                    SetValue(rd.ReadDateTime());
                    break;
                case SttpValueTypeCode.DateTimeOffset:
                    throw new NotImplementedException();
                    //AsDateTimeOffset = rd.ReadDateTimeOffset();
                    break;
                case SttpValueTypeCode.SttpTime:
                    throw new NotImplementedException();
                    // AsSttpTime = rd.ReadSttpTime();
                    break;
                case SttpValueTypeCode.SttpTimeOffset:
                    throw new NotImplementedException();
                    //AsSttpTimeOffset = rd.ReadSttpTimeOffset();
                    break;
                case SttpValueTypeCode.TimeSpan:
                    SetValue(new TimeSpan(rd.ReadInt64()));
                    break;
                case SttpValueTypeCode.Bool:
                    SetValue(rd.ReadBoolean());
                    break;
                case SttpValueTypeCode.Char:
                    SetValue(rd.ReadChar());
                    break;
                case SttpValueTypeCode.Guid:
                    SetValue(rd.ReadGuid());
                    break;
                case SttpValueTypeCode.String:
                    SetValue(rd.ReadString());
                    break;
                case SttpValueTypeCode.SttpBuffer:
                    throw new NotImplementedException();
                    //AsBuffer = rd.ReadBuffer();
                    break;
                case SttpValueTypeCode.SttpValueSet:
                    throw new NotImplementedException();
                    //AsValueSet = rd.ReadValueSet();
                    break;
                case SttpValueTypeCode.SttpNamedSet:
                    throw new NotImplementedException();
                    //AsNamedSet = rd.ReadNamedSet();
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    throw new NotImplementedException();
                    //AsConnectionString = rd.ReadConnectionString();
                    break;
                case SttpValueTypeCode.BulkTransportGuid:
                    throw new NotImplementedException();
                    //AsBulkTransportGuid = rd.ReadBulkTransportGuid();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void Load(Stream stream)
        {
            throw new NotImplementedException();
        }

        public SttpValueMutable(object x)
        {
            SetValue(x);
        }

        #endregion

        #region [ Properties ]

        public override sbyte AsSByte
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public override short AsInt16
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override int AsInt32
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override long AsInt64
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override byte AsByte
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override ushort AsUInt16
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override uint AsUInt32
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override ulong AsUInt64
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override decimal AsDecimal
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override double AsDouble
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override float AsSingle
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override DateTime AsDateTime
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
                            return m_valueSttpTime.Ticks;
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset.Ticks;
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return DateTime.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.DateTime:
                            return m_valueDateTime;
                        case SttpValueTypeCode.DateTimeOffset:
                            return m_valueDateTimeOffset.UtcDateTime;
                        case SttpValueTypeCode.SttpNamedSet:
                        case SttpValueTypeCode.SttpMarkup:
                        case SttpValueTypeCode.BulkTransportGuid:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override DateTimeOffset AsDateTimeOffset
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
                            throw new NotImplementedException();
                        case SttpValueTypeCode.SttpTimeOffset:
                            throw new NotImplementedException();
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return DateTimeOffset.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.DateTime:
                            return new DateTimeOffset(m_valueDateTime);
                        case SttpValueTypeCode.DateTimeOffset:
                            return m_valueDateTimeOffset.UtcDateTime;
                        case SttpValueTypeCode.SttpNamedSet:
                        case SttpValueTypeCode.SttpMarkup:
                        case SttpValueTypeCode.BulkTransportGuid:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpTime AsSttpTime
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
                            return m_valueSttpTime;
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset.ToSttpTimestamp();
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return SttpTime.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpTimeOffset AsSttpTimeOffset
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
                            return m_valueSttpTime.ToSttpTimestampOffset();
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset;
                        case SttpValueTypeCode.TimeSpan:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Char:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Bool:
                            throw new InvalidCastException("Cannot cast from TimeSpan");
                        case SttpValueTypeCode.Guid:
                            throw new InvalidCastException("Cannot cast from Guid");
                        case SttpValueTypeCode.String:
                            return SttpTimeOffset.Parse((string)m_valueObject);
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override TimeSpan AsTimeSpan
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override char AsChar
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override bool AsBool
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public override Guid AsGuid
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override string AsString
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
                            return m_valueSttpTime.ToString();
                        case SttpValueTypeCode.SttpTimeOffset:
                            return m_valueSttpTimeOffset.ToString();
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
                        case SttpValueTypeCode.SttpBuffer:
                            //ToDo: Return a byte string 0x292A78B402;
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override string ToTypeString
        {
            get
            {
                return $"({m_valueTypeCode}){AsString}";
            }
        }

        public override SttpBuffer AsBuffer
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
                        case SttpValueTypeCode.SttpBuffer:
                            return (SttpBuffer)m_valueObject;
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpValueSet AsSttpValueSet
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            return (SttpValueSet)m_valueObject;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpNamedSet AsSttpNamedSet
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
                        case SttpValueTypeCode.SttpBuffer:
                            throw new InvalidCastException("Cannot cast from Buffer");
                        case SttpValueTypeCode.SttpValueSet:
                            throw new InvalidCastException("Cannot cast from Set");
                        case SttpValueTypeCode.SttpNamedSet:
                            return (SttpNamedSet)m_valueObject;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        public override SttpMarkup AsSttpMarkup { get; }

        public override Guid AsBulkTransportGuid { get; }

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public override SttpValueTypeCode ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
        }

        #endregion

        #region [ Methods ] 

        public SttpValue CloneAsImmutable()
        {
            throw new NotImplementedException();
        }

        public void SetValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                m_valueTypeCode = SttpValueTypeCode.Null;
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
            else if (type == typeof(SttpTime))
            {
                SetValue((SttpTime)value);
            }
            else if (type == typeof(SttpTimeOffset))
            {
                SetValue((SttpTimeOffset)value);
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
            else if (type == typeof(SttpValueSet))
            {
                SetValue((SttpValueSet)value);
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
            m_valueTypeCode = SttpValueTypeCode.SByte;
            m_valueSByte = value;
        }
        public void SetValue(short value)
        {
            m_valueTypeCode = SttpValueTypeCode.Int16;
            m_valueInt16 = value;
        }
        public void SetValue(int value)
        {
            m_valueTypeCode = SttpValueTypeCode.Int32;
            m_valueInt32 = value;
        }
        public void SetValue(long value)
        {
            m_valueTypeCode = SttpValueTypeCode.Int64;
            m_valueInt64 = value;
        }
        public void SetValue(byte value)
        {
            m_valueTypeCode = SttpValueTypeCode.Byte;
            m_valueByte = value;
        }
        public void SetValue(ushort value)
        {
            m_valueTypeCode = SttpValueTypeCode.UInt16;
            m_valueUInt16 = value;
        }
        public void SetValue(uint value)
        {
            m_valueTypeCode = SttpValueTypeCode.UInt32;
            m_valueUInt32 = value;
        }
        public void SetValue(ulong value)
        {
            m_valueTypeCode = SttpValueTypeCode.UInt64;
            m_valueUInt64 = value;
        }
        public void SetValue(float value)
        {
            m_valueTypeCode = SttpValueTypeCode.Single;
            m_valueSingle = value;
        }
        public void SetValue(double value)
        {
            m_valueTypeCode = SttpValueTypeCode.Double;
            m_valueDouble = value;
        }
        public void SetValue(decimal value)
        {
            m_valueTypeCode = SttpValueTypeCode.Decimal;
            m_valueDecimal = value;
        }
        public void SetValue(SttpTime value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpTime;
            m_valueSttpTime = value;
        }
        public void SetValue(SttpTimeOffset value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpTimeOffset;
            m_valueSttpTimeOffset = value;
        }
        public void SetValue(DateTime value)
        {
            m_valueTypeCode = SttpValueTypeCode.DateTime;
            m_valueDateTime = value;
        }
        public void SetValue(DateTimeOffset value)
        {
            m_valueTypeCode = SttpValueTypeCode.DateTimeOffset;
            m_valueDateTimeOffset = value;
        }
        public void SetValue(TimeSpan value)
        {
            m_valueTypeCode = SttpValueTypeCode.TimeSpan;
            m_valueTimeSpan = value;
        }
        public void SetValue(bool value)
        {
            m_valueTypeCode = SttpValueTypeCode.Bool;
            m_valueBool = value;
        }
        public void SetValue(char value)
        {
            m_valueTypeCode = SttpValueTypeCode.Char;
            m_valueChar = value;
        }
        public void SetValue(Guid value)
        {
            m_valueTypeCode = SttpValueTypeCode.Guid;
            m_valueGuid = value;
        }
        public void SetValue(string value)
        {
            m_valueTypeCode = SttpValueTypeCode.String;
            m_valueObject = value;
        }
        public void SetValue(byte[] value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpBuffer;
            m_valueObject = value;
        }
        public void SetValue(SttpValueSet value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpValueSet;
            m_valueObject = value;
        }
        public void SetValue(SttpNamedSet value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpNamedSet;
            m_valueObject = value;
        }

        public static bool operator ==(SttpValueMutable a, SttpValueMutable b)
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

        public static bool operator !=(SttpValueMutable a, SttpValueMutable b)
        {
            return !(a == b);
        }

        #endregion

        public void Load(PayloadReader payloadReader)
        {
            throw new NotImplementedException();
        }

        public void Save(PayloadWriter payloadWriter, bool includeTypeCode)
        {
            throw new NotImplementedException();
        }


        public static explicit operator SttpValueMutable(double v)
        {
            var rv = new SttpValueMutable();
            rv.SetValue(v);
            return rv;
        }
        public static explicit operator SttpValueMutable(string v)
        {
            var rv = new SttpValueMutable();
            rv.SetValue(v);
            return rv;
        }

        public override object ToNativeType
        {
            get
            {
                switch (ValueTypeCode)
                {
                    case SttpValueTypeCode.Null:
                        return null;
                    case SttpValueTypeCode.SByte:
                        return AsSByte;
                    case SttpValueTypeCode.Int16:
                        return AsInt16;
                    case SttpValueTypeCode.Int32:
                        return AsInt32;
                    case SttpValueTypeCode.Int64:
                        return AsInt64;
                    case SttpValueTypeCode.Byte:
                        return AsByte;
                    case SttpValueTypeCode.UInt16:
                        return AsUInt16;
                    case SttpValueTypeCode.UInt32:
                        return AsUInt32;
                    case SttpValueTypeCode.UInt64:
                        return AsUInt64;
                    case SttpValueTypeCode.Single:
                        return AsSingle;
                    case SttpValueTypeCode.Double:
                        return AsDouble;
                    case SttpValueTypeCode.Decimal:
                        return AsDecimal;
                    case SttpValueTypeCode.DateTime:
                        return AsDateTime;
                    case SttpValueTypeCode.DateTimeOffset:
                        return AsDateTimeOffset;
                    case SttpValueTypeCode.SttpTime:
                        return AsSttpTime;
                    case SttpValueTypeCode.SttpTimeOffset:
                        return AsSttpTimeOffset;
                    case SttpValueTypeCode.TimeSpan:
                        return AsTimeSpan.Ticks;
                    case SttpValueTypeCode.Bool:
                        return AsBool;
                    case SttpValueTypeCode.Char:
                        return AsChar;
                    case SttpValueTypeCode.Guid:
                        return AsGuid;
                    case SttpValueTypeCode.String:
                        return AsString;
                    case SttpValueTypeCode.SttpBuffer:
                        return AsBuffer;
                    case SttpValueTypeCode.SttpValueSet:
                        return AsSttpValueSet;
                    case SttpValueTypeCode.SttpNamedSet:
                        return AsSttpNamedSet;
                    case SttpValueTypeCode.SttpMarkup:
                        throw new NotImplementedException();
                    //return AsConnectionString);
                    case SttpValueTypeCode.BulkTransportGuid:
                        throw new NotImplementedException();
                    //return AsBulkTransportGuid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SetNull()
        {
            m_valueTypeCode = SttpValueTypeCode.Null;
        }
    }
}
