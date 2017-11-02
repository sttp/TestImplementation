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
    public class SttpValue : ISttpValue
    {
        #region [ Members ]

        [FieldOffset(0)]
        private ulong m_bytes0to7; //Used for cloning data.

        [FieldOffset(0)]
        private ulong m_valueUInt64;
        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private float m_valueSingle;
        [FieldOffset(0)]
        private double m_valueDouble;
        [FieldOffset(8)]
        private byte[] m_bufferValue;

        [FieldOffset(16)]
        private SttpFundamentalTypeCode m_fundamentalTypeCode;

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
            m_bytes0to7 = value.m_bytes0to7;
            m_bufferValue = value.m_bufferValue;
            m_fundamentalTypeCode = value.m_fundamentalTypeCode;
        }

        #endregion

        #region [ Properties ]

        public long AsInt64
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Int64)
                    return m_valueInt64;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Int64;
                m_valueInt64 = value;
                m_bufferValue = null;
            }
        }

        public ulong AsUInt64
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.UInt64)
                    return m_valueUInt64;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.UInt64;
                m_valueUInt64 = value;
                m_bufferValue = null;
            }
        }

        public float AsSingle
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Single)
                    return m_valueSingle;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Single;
                m_bytes0to7 = 0;
                m_valueSingle = value;
                m_bufferValue = null;
            }
        }

        public double AsDouble
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Double)
                    return m_valueDouble;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Double;
                m_valueDouble = value;
                m_bufferValue = null;
            }
        }

        public byte[] AsBuffer
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Buffer)
                    return (byte[])m_bufferValue;
                throw new NotSupportedException();
            }
            set
            {
                if (value == null)
                {
                    IsNull = true;
                }
                else
                {
                    m_fundamentalTypeCode = SttpFundamentalTypeCode.Buffer;
                    m_bytes0to7 = 0;
                    m_bufferValue = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the native type of the STTP Wire Protocol.
        /// </summary>
        public object AsObject
        {
            get
            {
                switch (m_fundamentalTypeCode)
                {
                    case SttpFundamentalTypeCode.Null:
                        return null;
                    case SttpFundamentalTypeCode.Int64:
                        return AsInt64;
                    case SttpFundamentalTypeCode.UInt64:
                        return AsUInt64;
                    case SttpFundamentalTypeCode.Single:
                        return AsSingle;
                    case SttpFundamentalTypeCode.Double:
                        return AsDouble;
                    case SttpFundamentalTypeCode.Buffer:
                        return AsBuffer;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                if (value == null)
                {
                    IsNull = true;
                    return;
                }
                var type = value.GetType();
                if (type == typeof(long))
                {
                    AsInt64 = (long)value;
                }
                else if (type == typeof(ulong))
                {
                    AsUInt64 = (ulong)value;
                }
                else if (type == typeof(double))
                {
                    AsDouble = (double)value;
                }
                else if (type == typeof(float))
                {
                    AsSingle = (float)value;
                }
                else if (type == typeof(byte[]))
                {
                    AsBuffer = (byte[])value;
                }
                else
                {
                    throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
                }
            }
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by calling <see cref="SetValueToNull"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">When setting this value to true. Set a value by calling one of the <see cref="SetValue"/> methods.</exception>
        public bool IsNull
        {
            get
            {
                return m_fundamentalTypeCode == SttpFundamentalTypeCode.Null;
            }
            set
            {
                if (!value)
                    throw new InvalidOperationException("Can only set a value to null with this property, to set not null, use one of the other properties to set the value.");
                m_bufferValue = null;
                m_bytes0to7 = 0;
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Null;
            }
        }

        public void SetValue(SttpValue value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_bufferValue = value.m_bufferValue;
            m_fundamentalTypeCode = value.m_fundamentalTypeCode;
        }

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpFundamentalTypeCode FundamentalTypeCode
        {
            get
            {
                return m_fundamentalTypeCode;
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

        public void Save(PacketWriter writer)
        {
            writer.Write(m_fundamentalTypeCode);
            switch (m_fundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    break;
                case SttpFundamentalTypeCode.Int64:
                    writer.Write(AsInt64);
                    break;
                case SttpFundamentalTypeCode.UInt64:
                    writer.Write(AsUInt64);
                    break;
                case SttpFundamentalTypeCode.Single:
                    writer.Write(AsSingle);
                    break;
                case SttpFundamentalTypeCode.Double:
                    writer.Write(AsDouble);
                    break;
                case SttpFundamentalTypeCode.Buffer:
                    writer.Write(AsBuffer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Load(PacketReader reader)
        {
            switch (reader.Read<SttpFundamentalTypeCode>())
            {
                case SttpFundamentalTypeCode.Null:
                    IsNull = true;
                    break;
                case SttpFundamentalTypeCode.Int64:
                    AsInt64 = reader.ReadInt64();
                    break;
                case SttpFundamentalTypeCode.UInt64:
                    AsUInt64 = reader.ReadUInt64();
                    break;
                case SttpFundamentalTypeCode.Single:
                    AsSingle = reader.ReadSingle();
                    break;
                case SttpFundamentalTypeCode.Double:
                    AsDouble = reader.ReadDouble();
                    break;
                case SttpFundamentalTypeCode.Buffer:
                    AsBuffer = reader.ReadBytes();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool operator ==(SttpValue a, SttpValue b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (ReferenceEquals(a, null))
                return false;
            if (ReferenceEquals(b, null))
                return false;
            if (a.m_fundamentalTypeCode != b.m_fundamentalTypeCode)
                return false;
            switch (a.m_fundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    return true;
                case SttpFundamentalTypeCode.Single:
                case SttpFundamentalTypeCode.UInt64:
                case SttpFundamentalTypeCode.Int64:
                case SttpFundamentalTypeCode.Double:
                    return a.m_bytes0to7 == b.m_bytes0to7;
                case SttpFundamentalTypeCode.Buffer:
                    return a.AsBuffer.SequenceEqual(b.AsBuffer);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool operator !=(SttpValue a, SttpValue b)
        {
            return !(a == b);
        }

        #endregion
    }
}
