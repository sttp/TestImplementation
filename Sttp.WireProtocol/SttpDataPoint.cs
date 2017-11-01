using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// This class contains the fundamental unit of transmission for STTP.
    /// The intention of this class is to be reusable. In order to prevent reuse of the class,
    /// it must be marked as Immutable. This will indicate that a new class must be created if
    /// reuse is the normal case.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpDataPoint : ISttpValue
    {
        // Since 99% of the class instances Data Points going to be small 32 or 64 bit values
        // this class has been optimized to contain an object field that gets expanded 
        // only during the edge case scenarios
        private class NestedObjects
        {
            public object DataPointKeyToken;
            public object ValueObject; //Can be a string or a byte[]
            public SttpScientificTime Time;

            public NestedObjects Clone()
            {
                return (NestedObjects)MemberwiseClone();
            }
        }

        //This class will store
        // Value68
        // Token
        // DataPointID
        // TimeQuality
        // ValueQuality
        // FundamentalTypeCode

        #region [ Members ]

        [FieldOffset(0)]
        private ulong m_bytes0to7; //Used for cloning data.
        [FieldOffset(16)]
        private ulong m_bytes16to23; //Used for cloning data.
        [FieldOffset(24)]
        private ulong m_bytes24to31; //Used for cloning data.
        [FieldOffset(32)]
        private ulong m_bytes32to39; //Used for cloning data.

        [FieldOffset(0)]
        private int m_valueInt32;
        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private float m_valueSingle;
        [FieldOffset(0)]
        private double m_valueDouble;

        //A user defined token that can be assigned to each point at the wire protocol level to reduce the lookup penalty in mapping.
        //Note: If the fundamental data type is Buffer, this object will be NestedObjects to represent this rare data.
        [FieldOffset(8)]
        private object m_dataPointKeyToken;

        // The default SttpTimestamp. SttpScientificTime will be stored in (m_dataPointKeyToken as NestedObjects)
        [FieldOffset(16)]
        private long m_timestamp;

        [FieldOffset(24)]
        private uint m_dataPointID;

        [FieldOffset(28)]
        private uint m_timeQuality;

        [FieldOffset(32)]
        private uint m_valueQuality;

        [FieldOffset(36)]
        private SttpFundamentalTypeCode m_fundamentalTypeCode;

        [FieldOffset(37)]
        private bool m_isImmutable;

        [FieldOffset(38)]
        private bool m_isNestedObject;

        [FieldOffset(39)]
        private bool m_advTime;

        #endregion

        #region [ Constructors ]

        public SttpDataPoint()
        {
            IsNull = true;
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. The cloned value is mutable
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpDataPoint(SttpDataPoint value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_bytes16to23 = value.m_bytes16to23;
            m_bytes24to31 = value.m_bytes24to31;
            m_bytes32to39 = value.m_bytes32to39;

            if (value.m_isNestedObject)
                m_dataPointKeyToken = ((NestedObjects)value.m_dataPointKeyToken).Clone();
            else
                m_dataPointKeyToken = value.m_dataPointKeyToken;


            m_isImmutable = false;
        }

        #endregion

        #region [ Properties ]

        public SttpTimestamp Timestamp
        {
            get
            {
                return new SttpTimestamp(m_timestamp);
            }
            set
            {
                CheckImmutable();
                m_advTime = false;
                m_timestamp = value.Ticks;
                if (m_isNestedObject)
                {
                    ((NestedObjects)m_dataPointKeyToken).Time = null;
                }
            }
        }

        public SttpScientificTime ScientificTime
        {
            get
            {
                if (m_advTime)
                    return ((NestedObjects)m_dataPointKeyToken).Time;
                return new SttpScientificTime(new SttpTimestamp(m_timestamp));
            }
            set
            {
                CheckImmutable();
                m_advTime = true;
                m_timestamp = value.ToSttpTimestamp().Ticks;

                if (m_isNestedObject)
                {
                    ((NestedObjects)m_dataPointKeyToken).Time = null;
                }
                else
                {
                    var nested = new NestedObjects();
                    nested.Time = value;
                    nested.DataPointKeyToken = m_dataPointKeyToken;
                    m_dataPointKeyToken = nested;
                    m_isNestedObject = true;
                }
            }
        }

        public uint TimeQuality
        {
            get
            {
                return m_timeQuality;
            }
            set
            {
                CheckImmutable();
                m_timeQuality = value;
            }
        }

        public uint ValueQuality
        {
            get
            {
                return m_valueQuality;
            }
            set
            {
                CheckImmutable();
                m_valueQuality = value;
            }
        }

        public uint DataPointID
        {
            get
            {
                return m_dataPointID;
            }
            set
            {
                CheckImmutable();
                m_dataPointID = value;
            }
        }

        /// <summary>
        /// A user defined token that can be assigned to each point at the wire protocol level to reduce the lookup penalty in mapping.
        /// </summary>
        public object DataPointKeyToken
        {
            get
            {
                if (m_isNestedObject)
                    return ((NestedObjects)m_dataPointKeyToken).DataPointKeyToken;
                return m_dataPointKeyToken;
            }
            set
            {
                CheckImmutable();
                if (m_isNestedObject)
                    ((NestedObjects)m_dataPointKeyToken).DataPointKeyToken = value;
                else
                    m_dataPointKeyToken = value;
            }
        }

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
                CheckImmutable();
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Int32;
                m_valueInt32 = value;
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
                CheckImmutable();
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Int64;
                m_valueInt64 = value;
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
                CheckImmutable();
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Single;
                m_valueSingle = value;
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
                CheckImmutable();
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Double;
                m_valueDouble = value;
            }
        }

        public byte[] AsBuffer
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Buffer)
                    return (byte[])((NestedObjects)m_dataPointKeyToken).ValueObject;
                throw new NotSupportedException();
            }
            set
            {
                CheckImmutable();
                if (value == null)
                {
                    IsNull = true;
                }
                else
                {
                    m_fundamentalTypeCode = SttpFundamentalTypeCode.Buffer;
                    if (!m_isNestedObject)
                    {
                        m_isNestedObject = true;
                        var obj = new NestedObjects();
                        obj.ValueObject = value;
                        obj.DataPointKeyToken = m_dataPointKeyToken;
                        m_dataPointKeyToken = obj;
                        return;
                    }
                    ((NestedObjects)m_dataPointKeyToken).ValueObject = value;
                }
            }
        }

        public string AsString
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.String)
                    return (string)((NestedObjects)m_dataPointKeyToken).ValueObject;
                throw new NotSupportedException();
            }
            set
            {
                CheckImmutable();
                if (value == null)
                {
                    IsNull = true;
                }
                else
                {
                    m_fundamentalTypeCode = SttpFundamentalTypeCode.String;
                    if (!m_isNestedObject)
                    {
                        m_isNestedObject = true;
                        var obj = new NestedObjects();
                        obj.ValueObject = value;
                        obj.DataPointKeyToken = m_dataPointKeyToken;
                        m_dataPointKeyToken = obj;
                        return;
                    }
                    ((NestedObjects)m_dataPointKeyToken).ValueObject = value;
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
                CheckImmutable();
                if (!value)
                    throw new InvalidOperationException("Can only set a value to null with this property, to set not null, use one of the other properties to set the value.");
                if (m_isNestedObject)
                {
                    ((NestedObjects)m_dataPointKeyToken).ValueObject = null;
                }
            }
        }

        /// <summary>
        /// Gets/Sets this class as Immutable. Once set, this field cannot be cleared.
        /// </summary>
        public bool IsImmutable
        {
            get
            {
                return m_isImmutable;
            }
            set
            {
                //This flag can only be set and cannot be reverted.
                if (value)
                {
                    //Even if the class is immutable, setting this flag again should not throw an exception.
                    m_isImmutable = true;
                }
                else
                {
                    //If trying to set this to false, a simple check will suffice. If the check succeeds, then it's already false.
                    CheckImmutable();
                }
            }
        }

        public void SetValue(SttpDataPoint value)
        {
            CheckImmutable();
            m_bytes0to7 = value.m_bytes0to7;
            m_dataPointKeyToken = value.m_dataPointKeyToken;
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

        private void CheckImmutable()
        {
            if (m_isImmutable)
                ThrowImmutableException();
        }

        private void ThrowImmutableException()
        {
            throw new InvalidOperationException("Value has been marked as immutable and cannot be modified.");
        }

        /// <summary>
        /// Clones the value
        /// </summary>
        /// <returns></returns>
        public SttpDataPoint Clone()
        {
            return new SttpDataPoint(this);
        }

        #endregion
    }
}
