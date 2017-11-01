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
    /// A class that can hold all of the fundamental types of the openECA framework. This class can only hold one 
    /// value at a time, regardless of the type, and will automatically type cast to any supported type.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpDataPoint
    {
        private class NestedObjects
        {
            public object DataPointKeyToken;
            public object ValueObject;
            public SttpLongTimestamp LongTimestamp;
        }

        #region [ Members ]

        [FieldOffset(0)]
        private ulong m_bytes0to7; //Used for cloning data.

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
        private DateTime m_valueDateTime;
        [FieldOffset(0)]
        private TimeSpan m_valueTimeSpan;
        [FieldOffset(0)]
        private char m_valueChar;
        [FieldOffset(0)]
        private bool m_valueBool;

        [FieldOffset(8)]
        private object m_valueObject; //Holds any reference type or type not defined above.

        [FieldOffset(16)]
        private long m_timestamp;

        [FieldOffset(24)]
        private uint m_dataPointID;

        [FieldOffset(28)]
        private uint m_timeQuality;

        [FieldOffset(32)]
        private uint m_valueQuality;

        [FieldOffset(36)]
        private SttpValueTypeCode m_valueTypeCode;

        [FieldOffset(37)]
        private bool m_isImmutable;

        [FieldOffset(38)]
        private bool m_nestedObject;

        [FieldOffset(39)]
        private bool m_advTime;


        #endregion

        #region [ Constructors ]

        public SttpDataPoint()
        {
            SetValueToNull();
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. The cloned value is mutable
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpDataPoint(SttpDataPoint value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.ValueTypeCode;
            m_isImmutable = m_isImmutable = false;
        }

        #endregion

        #region [ Properties ]

        #region [ Timestamp casting ] 

        public SttpTimestamp Timestamp
        {
            get
            {
                if (!m_advTime)
                {
                    return new SttpTimestamp(m_timestamp);
                }
                if (!m_nestedObject)
                {
                    return ((SttpLongTimestamp)m_valueObject).ToSttpTimestamp();
                }
                return (m_valueObject as NestedObjects).LongTimestamp.ToSttpTimestamp();
            }
            set
            {
                CheckImmutable();
                m_advTime = false;
                m_timestamp = value.Ticks;
            }
        }

        public SttpLongTimestamp LongTimestamp
        {
            get
            {
                if (!m_advTime)
                {
                    return new SttpLongTimestamp(m_timestamp, 0);
                }
                if (!m_nestedObject)
                {
                    return (SttpLongTimestamp)m_valueObject;
                }
                return (m_valueObject as NestedObjects).LongTimestamp;
            }
            set
            {
                CheckImmutable();
                m_advTime = true;
                if (!m_nestedObject && m_valueObject == null)
                {
                    m_valueObject = value;
                    return;
                }
                if (!m_nestedObject)
                {
                    var nested = new NestedObjects();
                    nested.ValueObject = m_valueObject;
                    nested.LongTimestamp = value;
                    m_valueObject = nested;
                    m_nestedObject = true;
                    return;
                }
                (m_valueObject as NestedObjects).LongTimestamp = value;

            }
        }

        #endregion

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
        /// Gets if this class has a value. Clear this by calling <see cref="SetValueToNull"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">When setting this value to true. Set a value by calling one of the <see cref="SetValue"/> methods.</exception>
        public bool HasValue
        {
            get
            {
                return m_valueTypeCode != SttpValueTypeCode.Null;
            }
        }

        /// <summary>
        /// Sets the <see cref="HasValue"/> property to false;
        /// </summary>
        public void SetValueToNull()
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Null;
            if (m_valueObject != null) m_valueObject = null; //assigning reference fields are function calls. So this avoids a call if not necessary.
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

        public void SetValue(object value)
        {
            CheckImmutable();
            if (value == null || value == DBNull.Value)
            {
                SetValueToNull();
                return;
            }

            var type = value.GetType();
            if (type == typeof(byte))
            {
                SetValue((byte)value);
            }
            else if (type == typeof(byte))
            {
                SetValue((byte)value);
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
            else if (type == typeof(decimal))
            {
                SetValue((decimal)value);
            }
            else if (type == typeof(double))
            {
                SetValue((double)value);
            }
            else if (type == typeof(float))
            {
                SetValue((float)value);
            }
            else if (type == typeof(DateTime))
            {
                SetValue((DateTime)value);
            }
            else if (type == typeof(TimeSpan))
            {
                SetValue((TimeSpan)value);
            }
            else if (type == typeof(char))
            {
                SetValue((char)value);
            }
            else if (type == typeof(bool))
            {
                SetValue((bool)value);
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
            else if (typeof(ISttpUdfType).IsAssignableFrom(type))
            {
                SetValue((ISttpUdfType)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

        public void SetValue(byte value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Byte;
            m_valueByte = value;
            //Since class assignments cost a function call when not on the stack, check for null before assigning it.
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(short value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Int16;
            m_valueInt16 = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(int value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Int32;
            m_valueInt32 = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(long value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Int64;
            m_valueInt64 = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(ushort value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.UInt16;
            m_valueUInt16 = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(uint value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.UInt32;
            m_valueUInt32 = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(ulong value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.UInt64;
            m_valueUInt64 = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(decimal value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Decimal;
            m_valueObject = value;
        }

        public void SetValue(double value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Double;
            m_valueDouble = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(float value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Single;
            m_valueSingle = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(DateTime value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.DateTime;
            m_valueDateTime = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(TimeSpan value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.TimeSpan;
            m_valueTimeSpan = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(char value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Char;
            m_valueChar = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(bool value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Bool;
            m_valueBool = value;
            if (m_valueObject != null)
                m_valueObject = null;
        }

        public void SetValue(Guid value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Guid;
            m_valueObject = value;
        }

        public void SetValue(ISttpUdfType value)
        {
            m_valueTypeCode = SttpValueTypeCode.UDFType;
            m_valueObject = value;
        }

        public void SetValue(string value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.String;
            m_valueObject = value;
        }


        public void SetValue(byte[] value)
        {
            CheckImmutable();
            m_valueTypeCode = SttpValueTypeCode.Buffer;
            m_valueObject = value;
        }

        public void SetValue(SttpDataPoint value)
        {
            CheckImmutable();
            m_bytes0to7 = value.m_bytes0to7;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.ValueTypeCode;
        }

        public void SetRawValue(SttpValueTypeCode typeCode, uint value)
        {
            CheckImmutable();
            if (typeCode == SttpValueTypeCode.Int32 || typeCode == SttpValueTypeCode.UInt32 || typeCode == SttpValueTypeCode.Single)
            {
                m_valueTypeCode = typeCode;
                m_valueUInt32 = value;
                if (m_valueObject != null)
                    m_valueObject = null;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
            }
        }

        public void SetRawValue(SttpValueTypeCode typeCode, ulong value)
        {
            CheckImmutable();
            if (typeCode == SttpValueTypeCode.Int64 || typeCode == SttpValueTypeCode.UInt64 || typeCode == SttpValueTypeCode.Double || typeCode == SttpValueTypeCode.DateTime || typeCode == SttpValueTypeCode.TimeSpan)
            {
                m_valueTypeCode = typeCode;
                m_valueUInt64 = value;
                if (m_valueObject != null)
                    m_valueObject = null;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
            }
        }

        public override string ToString()
        {
            return AsString();
        }

        /// <summary>
        /// returns the boxed native type of the value. Returns <see cref="DBNull"/> for null values. 
        /// This is useful if you want to use other conversion methods such as those present in <see cref="DataTable"/>
        /// </summary>
        /// <returns></returns>
        public object ToNativeType()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    return DBNull.Value;
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return m_valueDouble;
                case SttpValueTypeCode.Single:
                    return m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    return m_valueDateTime;
                case SttpValueTypeCode.TimeSpan:
                    return m_valueTimeSpan;
                case SttpValueTypeCode.Char:
                    return m_valueChar;
                case SttpValueTypeCode.Bool:
                    return m_valueBool;
                case SttpValueTypeCode.Guid:
                    return (Guid)m_valueObject;
                case SttpValueTypeCode.String:
                    return m_valueObject;
                case SttpValueTypeCode.Buffer:
                    return m_valueObject;
                case SttpValueTypeCode.UDFType:
                    return m_valueObject;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region [ implicit operators Cast From EcaMeasurementValue ]

        public byte ToByte()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to byte");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (byte)m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (byte)m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (byte)m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return (byte)m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (byte)m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (byte)m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (byte)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (byte)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (byte)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to byte");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to byte");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to byte");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to byte");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to byte");
                case SttpValueTypeCode.String:
                    return byte.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public short ToInt16()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to short");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (short)m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (short)m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return (short)m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (short)m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (short)m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (short)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (short)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (short)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to short");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to short");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to short");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to short");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to short");
                case SttpValueTypeCode.String:
                    return short.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int ToInt32()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to int");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (int)m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (int)m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (int)m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (int)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (int)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (int)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to int");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to int");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to int");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to int");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to int");
                case SttpValueTypeCode.String:
                    return int.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public long ToInt64()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to long");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (long)m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (long)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (long)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (long)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to long");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to long");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to long");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to long");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to long");
                case SttpValueTypeCode.String:
                    return long.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ushort ToUInt16()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to ushort");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (ushort)m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (ushort)m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (ushort)m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (ushort)m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (ushort)m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (ushort)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (ushort)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (ushort)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to ushort");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to ushort");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to ushort");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to ushort");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to ushort");
                case SttpValueTypeCode.String:
                    return ushort.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public uint ToUInt32()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to long");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (uint)m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (uint)m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (uint)m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (uint)m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (uint)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (uint)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (uint)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to uint");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to uint");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to uint");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to uint");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to uint");
                case SttpValueTypeCode.String:
                    return uint.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ulong ToUInt64()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to long");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (ulong)m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (ulong)m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (ulong)m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (ulong)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (ulong)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (ulong)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to long");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to long");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to long");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to long");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to long");
                case SttpValueTypeCode.String:
                    return ulong.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public decimal ToDecimal()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to decimal");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (decimal)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (decimal)m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to decimal");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to decimal");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to decimal");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to decimal");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to decimal");
                case SttpValueTypeCode.String:
                    return decimal.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double ToDouble()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to double");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (double)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return m_valueDouble;
                case SttpValueTypeCode.Single:
                    return m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to double");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to double");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to double");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to double");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to double");
                case SttpValueTypeCode.String:
                    return double.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public float ToSingle()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to float");
                case SttpValueTypeCode.Byte:
                    return m_valueByte;
                case SttpValueTypeCode.Int16:
                    return m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (float)(Decimal)m_valueObject;
                case SttpValueTypeCode.Double:
                    return (float)m_valueDouble;
                case SttpValueTypeCode.Single:
                    return m_valueSingle;
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to float");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to float");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to float");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to float");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to float");
                case SttpValueTypeCode.String:
                    return float.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DateTime ToDateTime()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to DateTime");
                case SttpValueTypeCode.Byte:
                    throw new InvalidCastException("Cannot convert from Byte to DateTime");
                case SttpValueTypeCode.Int16:
                    throw new InvalidCastException("Cannot convert from Int16 to DateTime");
                case SttpValueTypeCode.Int32:
                    throw new InvalidCastException("Cannot convert from Int32 to DateTime");
                case SttpValueTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64 to DateTime");
                case SttpValueTypeCode.UInt16:
                    throw new InvalidCastException("Cannot convert from UInt16 to DateTime");
                case SttpValueTypeCode.UInt32:
                    throw new InvalidCastException("Cannot convert from UInt32 to DateTime");
                case SttpValueTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64 to DateTime");
                case SttpValueTypeCode.Decimal:
                    throw new InvalidCastException("Cannot convert from Decimal to DateTime");
                case SttpValueTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double to DateTime");
                case SttpValueTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single to DateTime");
                case SttpValueTypeCode.DateTime:
                    return m_valueDateTime;
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to DateTime");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to DateTime");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to DateTime");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to DateTime");
                case SttpValueTypeCode.String:
                    return DateTime.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TimeSpan ToTimeSpan()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to TimeSpan");
                case SttpValueTypeCode.Byte:
                    throw new InvalidCastException("Cannot convert from Byte to TimeSpan");
                case SttpValueTypeCode.Int16:
                    throw new InvalidCastException("Cannot convert from Int16 to TimeSpan");
                case SttpValueTypeCode.Int32:
                    throw new InvalidCastException("Cannot convert from Int32 to TimeSpan");
                case SttpValueTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64 to TimeSpan");
                case SttpValueTypeCode.UInt16:
                    throw new InvalidCastException("Cannot convert from UInt16 to TimeSpan");
                case SttpValueTypeCode.UInt32:
                    throw new InvalidCastException("Cannot convert from UInt32 to TimeSpan");
                case SttpValueTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64 to TimeSpan");
                case SttpValueTypeCode.Decimal:
                    throw new InvalidCastException("Cannot convert from Decimal to TimeSpan");
                case SttpValueTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double to TimeSpan");
                case SttpValueTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single to TimeSpan");
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to TimeSpan");
                case SttpValueTypeCode.TimeSpan:
                    return m_valueTimeSpan;
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to TimeSpan");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to TimeSpan");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to TimeSpan");
                case SttpValueTypeCode.String:
                    return TimeSpan.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public char ToChar()
        {
            //ToDo: Allow char conversion from '2' to 2 and vice versa
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to char");
                case SttpValueTypeCode.Byte:
                    throw new InvalidCastException("Cannot convert from Byte to TimeSpan");
                case SttpValueTypeCode.Int16:
                    throw new InvalidCastException("Cannot convert from Int16 to TimeSpan");
                case SttpValueTypeCode.Int32:
                    throw new InvalidCastException("Cannot convert from Int32 to TimeSpan");
                case SttpValueTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64 to TimeSpan");
                case SttpValueTypeCode.UInt16:
                    throw new InvalidCastException("Cannot convert from UInt16 to TimeSpan");
                case SttpValueTypeCode.UInt32:
                    throw new InvalidCastException("Cannot convert from UInt32 to TimeSpan");
                case SttpValueTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64 to TimeSpan");
                case SttpValueTypeCode.Decimal:
                    throw new InvalidCastException("Cannot convert from Decimal to TimeSpan");
                case SttpValueTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double to TimeSpan");
                case SttpValueTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single to TimeSpan");
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to TimeSpan");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from DateTime to TimeSpan");
                case SttpValueTypeCode.Char:
                    return m_valueChar;
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to TimeSpan");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to TimeSpan");
                case SttpValueTypeCode.String:
                    return char.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool ToBool()
        {
            //ToDo: use GSF.ParseBoolean extension to do a bool conversion.
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to bool");
                case SttpValueTypeCode.Byte:
                    throw new InvalidCastException("Cannot convert from Byte to bool");
                case SttpValueTypeCode.Int16:
                    throw new InvalidCastException("Cannot convert from Int16 to bool");
                case SttpValueTypeCode.Int32:
                    throw new InvalidCastException("Cannot convert from Int32 to bool");
                case SttpValueTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64 to bool");
                case SttpValueTypeCode.UInt16:
                    throw new InvalidCastException("Cannot convert from UInt16 to bool");
                case SttpValueTypeCode.UInt32:
                    throw new InvalidCastException("Cannot convert from UInt32 to bool");
                case SttpValueTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64 to bool");
                case SttpValueTypeCode.Decimal:
                    throw new InvalidCastException("Cannot convert from Decimal to bool");
                case SttpValueTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double to bool");
                case SttpValueTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single to bool");
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to bool");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from TimeSpan to bool");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to bool");
                case SttpValueTypeCode.Bool:
                    return m_valueBool;
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to bool");
                case SttpValueTypeCode.String:
                    return bool.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Guid ToGuid()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to Guid");
                case SttpValueTypeCode.Byte:
                    throw new InvalidCastException("Cannot convert from Byte to Guid");
                case SttpValueTypeCode.Int16:
                    throw new InvalidCastException("Cannot convert from Int16 to Guid");
                case SttpValueTypeCode.Int32:
                    throw new InvalidCastException("Cannot convert from Int32 to Guid");
                case SttpValueTypeCode.Int64:
                    throw new InvalidCastException("Cannot convert from Int64 to Guid");
                case SttpValueTypeCode.UInt16:
                    throw new InvalidCastException("Cannot convert from UInt16 to Guid");
                case SttpValueTypeCode.UInt32:
                    throw new InvalidCastException("Cannot convert from UInt32 to Guid");
                case SttpValueTypeCode.UInt64:
                    throw new InvalidCastException("Cannot convert from UInt64 to Guid");
                case SttpValueTypeCode.Decimal:
                    throw new InvalidCastException("Cannot convert from Decimal to Guid");
                case SttpValueTypeCode.Double:
                    throw new InvalidCastException("Cannot convert from Double to Guid");
                case SttpValueTypeCode.Single:
                    throw new InvalidCastException("Cannot convert from Single to Guid");
                case SttpValueTypeCode.DateTime:
                    throw new InvalidCastException("Cannot convert from DateTime to Guid");
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from TimeSpan to Guid");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to Guid");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to Guid");
                case SttpValueTypeCode.Guid:
                    return (Guid)m_valueObject;
                case SttpValueTypeCode.String:
                    return Guid.Parse((string)m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string AsString()
        {
            switch (m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    return null;
                case SttpValueTypeCode.Byte:
                    return m_valueByte.ToString();
                case SttpValueTypeCode.Int16:
                    return m_valueInt16.ToString();
                case SttpValueTypeCode.Int32:
                    return m_valueInt32.ToString();
                case SttpValueTypeCode.Int64:
                    return m_valueInt64.ToString();
                case SttpValueTypeCode.UInt16:
                    return m_valueUInt16.ToString();
                case SttpValueTypeCode.UInt32:
                    return m_valueUInt32.ToString();
                case SttpValueTypeCode.UInt64:
                    return m_valueUInt64.ToString();
                case SttpValueTypeCode.Decimal:
                    return ((Decimal)m_valueObject).ToString();
                case SttpValueTypeCode.Double:
                    return m_valueDouble.ToString();
                case SttpValueTypeCode.Single:
                    return m_valueSingle.ToString();
                case SttpValueTypeCode.DateTime:
                    return m_valueDateTime.ToString();
                case SttpValueTypeCode.TimeSpan:
                    return m_valueTimeSpan.ToString();
                case SttpValueTypeCode.Char:
                    return m_valueChar.ToString();
                case SttpValueTypeCode.Bool:
                    return m_valueBool.ToString();
                case SttpValueTypeCode.Guid:
                    return ((Guid)m_valueObject).ToString();
                case SttpValueTypeCode.String:
                    return (string)m_valueObject;
                case SttpValueTypeCode.UDFType:
                    return m_valueObject?.ToString() ?? string.Empty;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        public byte[] ToByteArray()
        {
            return null;
        }
    }
}
