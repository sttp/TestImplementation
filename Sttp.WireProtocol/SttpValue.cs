using System;
using System.Runtime.InteropServices;
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
        private int m_valueInt32;
        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private float m_valueSingle;
        [FieldOffset(0)]
        private double m_valueDouble;

        [FieldOffset(8)]
        private object m_objectValue;

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
            m_objectValue = value.m_objectValue;
            m_fundamentalTypeCode = value.m_fundamentalTypeCode;
        }

        #endregion

        #region [ Properties ]

        public int AsInt32
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Int32)
                    return m_valueInt32;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Int32;
                m_valueInt32 = value;
                m_objectValue = null;
            }
        }

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
                m_objectValue = null;
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
                m_valueSingle = value;
                m_objectValue = null;
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
                m_objectValue = null;
            }
        }

        public byte[] AsBuffer
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Buffer)
                    return (byte[])m_objectValue;
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
                    m_objectValue = value;
                }
            }
        }

        public string AsString
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.String)
                    return (String)m_objectValue;
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
                    m_fundamentalTypeCode = SttpFundamentalTypeCode.String;
                    m_objectValue = value;
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
                m_objectValue = null;
            }
        }

        public void SetValue(SttpValue value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_objectValue = value.m_objectValue;
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

        #endregion
    }
}
