using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// A class that can hold all of the fundamental types of the openECA framework. This class can only hold one 
    /// value at a time, regardless of the type, and will automatically type cast to any supported type.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpValue
    {
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
        private SttpValueTypeCode m_valueTypeCode;
        [FieldOffset(17)]
        private bool m_isImmutable;

        #endregion

        #region [ Constructors ]

        public SttpValue()
        {
            SetValueToNull();
        }

        public SttpValue(byte value)
        {
            SetValue(value);
        }
        public SttpValue(short value)
        {
            SetValue(value);
        }
        public SttpValue(int value)
        {
            SetValue(value);
        }
        public SttpValue(long value)
        {
            SetValue(value);
        }
        public SttpValue(ushort value)
        {
            SetValue(value);
        }
        public SttpValue(uint value)
        {
            SetValue(value);
        }
        public SttpValue(ulong value)
        {
            SetValue(value);
        }
        public SttpValue(decimal value)
        {
            SetValue(value);
        }
        public SttpValue(double value)
        {
            SetValue(value);
        }
        public SttpValue(float value)
        {
            SetValue(value);
        }
        public SttpValue(DateTime value)
        {
            SetValue(value);
        }
        public SttpValue(TimeSpan value)
        {
            SetValue(value);
        }
        public SttpValue(char value)
        {
            SetValue(value);
        }
        public SttpValue(bool value)
        {
            SetValue(value);
        }
        public SttpValue(Guid value)
        {
            SetValue(value);
        }
        public SttpValue(string value)
        {
            SetValue(value);
        }
        public SttpValue(byte[] value)
        {
            SetValue(value);
        }
        public SttpValue(ISttpUdfType value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. The cloned value is mutable
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpValue(SttpValue value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.ValueTypeCode;
            m_isImmutable = m_isImmutable = false;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets bytes 0-0 of this structure;
        /// This is where Byte and Boolean are stored.
        /// </summary>
        public ushort RawValue0_0
        {
            get
            {
                return m_valueByte;
            }
        }

        /// <summary>
        /// Gets bytes 0-1 of this structure;
        /// This is where Char, Short, UShort are stored.
        /// </summary>
        public ushort RawValue0_1
        {
            get
            {
                return m_valueUInt16;
            }
        }

        /// <summary>
        /// Gets bytes 0-3 of this structure;
        /// This is where all 4 byte objects are stored.
        /// </summary>
        public uint RawValue0_3
        {
            get
            {
                return m_valueUInt32;
            }
        }
        /// <summary>
        /// Gets bytes 0-7 of this structure.
        /// This is where all 8 byte objects are stored.
        /// </summary>
        public ulong RawValue0_7
        {
            get
            {
                return m_bytes0to7;
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
        public SttpValue Clone()
        {
            return new SttpValue(this);
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

        public void SetValue(SttpValue value)
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
            return (string)this;
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

        public static implicit operator byte(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to byte");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (byte)value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (byte)value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (byte)value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return (byte)value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (byte)value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (byte)value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (byte)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (byte)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (byte)value.m_valueSingle;
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
                    return byte.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator short(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to short");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (short)value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (short)value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return (short)value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (short)value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (short)value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (short)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (short)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (short)value.m_valueSingle;
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
                    return short.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator int(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to int");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (int)value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (int)value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (int)value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (int)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (int)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (int)value.m_valueSingle;
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
                    return int.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator long(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to long");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (long)value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (long)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (long)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (long)value.m_valueSingle;
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
                    return long.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator ushort(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to ushort");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (ushort)value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (ushort)value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (ushort)value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return (ushort)value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (ushort)value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (ushort)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (ushort)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (ushort)value.m_valueSingle;
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
                    return ushort.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator uint(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to long");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (uint)value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (uint)value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (uint)value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return (uint)value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (uint)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (uint)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (uint)value.m_valueSingle;
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
                    return uint.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator ulong(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to long");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return (ulong)value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return (ulong)value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return (ulong)value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (ulong)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (ulong)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (ulong)value.m_valueSingle;
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
                    return ulong.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator decimal(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to decimal");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (decimal)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return (decimal)value.m_valueSingle;
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
                    return decimal.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator double(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to double");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (double)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return value.m_valueSingle;
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
                    return double.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator float(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    throw new InvalidCastException("Cannot convert from Null to float");
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte;
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16;
                case SttpValueTypeCode.Int32:
                    return value.m_valueInt32;
                case SttpValueTypeCode.Int64:
                    return value.m_valueInt64;
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16;
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32;
                case SttpValueTypeCode.UInt64:
                    return value.m_valueUInt64;
                case SttpValueTypeCode.Decimal:
                    return (float)(Decimal)value.m_valueObject;
                case SttpValueTypeCode.Double:
                    return (float)value.m_valueDouble;
                case SttpValueTypeCode.Single:
                    return value.m_valueSingle;
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
                    return float.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator DateTime(SttpValue value)
        {
            switch (value.m_valueTypeCode)
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
                    return value.m_valueDateTime;
                case SttpValueTypeCode.TimeSpan:
                    throw new InvalidCastException("Cannot convert from Timespan to DateTime");
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to DateTime");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to DateTime");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to DateTime");
                case SttpValueTypeCode.String:
                    return DateTime.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator TimeSpan(SttpValue value)
        {
            switch (value.m_valueTypeCode)
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
                    return value.m_valueTimeSpan;
                case SttpValueTypeCode.Char:
                    throw new InvalidCastException("Cannot convert from Char to TimeSpan");
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to TimeSpan");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to TimeSpan");
                case SttpValueTypeCode.String:
                    return TimeSpan.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator char(SttpValue value)
        {
            //ToDo: Allow char conversion from '2' to 2 and vice versa
            switch (value.m_valueTypeCode)
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
                    return value.m_valueChar;
                case SttpValueTypeCode.Bool:
                    throw new InvalidCastException("Cannot convert from Bool to TimeSpan");
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to TimeSpan");
                case SttpValueTypeCode.String:
                    return char.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator bool(SttpValue value)
        {
            //ToDo: use GSF.ParseBoolean extension to do a bool conversion.
            switch (value.m_valueTypeCode)
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
                    return value.m_valueBool;
                case SttpValueTypeCode.Guid:
                    throw new InvalidCastException("Cannot convert from Guid to bool");
                case SttpValueTypeCode.String:
                    return bool.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator Guid(SttpValue value)
        {
            switch (value.m_valueTypeCode)
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
                    return (Guid)value.m_valueObject;
                case SttpValueTypeCode.String:
                    return Guid.Parse((string)value.m_valueObject);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator string(SttpValue value)
        {
            switch (value.m_valueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    return null;
                case SttpValueTypeCode.Byte:
                    return value.m_valueByte.ToString();
                case SttpValueTypeCode.Int16:
                    return value.m_valueInt16.ToString();
                case SttpValueTypeCode.Int32:
                    return value.m_valueInt32.ToString();
                case SttpValueTypeCode.Int64:
                    return value.m_valueInt64.ToString();
                case SttpValueTypeCode.UInt16:
                    return value.m_valueUInt16.ToString();
                case SttpValueTypeCode.UInt32:
                    return value.m_valueUInt32.ToString();
                case SttpValueTypeCode.UInt64:
                    return value.m_valueUInt64.ToString();
                case SttpValueTypeCode.Decimal:
                    return ((Decimal)value.m_valueObject).ToString();
                case SttpValueTypeCode.Double:
                    return value.m_valueDouble.ToString();
                case SttpValueTypeCode.Single:
                    return value.m_valueSingle.ToString();
                case SttpValueTypeCode.DateTime:
                    return value.m_valueDateTime.ToString();
                case SttpValueTypeCode.TimeSpan:
                    return value.m_valueTimeSpan.ToString();
                case SttpValueTypeCode.Char:
                    return value.m_valueChar.ToString();
                case SttpValueTypeCode.Bool:
                    return value.m_valueBool.ToString();
                case SttpValueTypeCode.Guid:
                    return ((Guid)value.m_valueObject).ToString();
                case SttpValueTypeCode.String:
                    return (string)value.m_valueObject;
                case SttpValueTypeCode.UDFType:
                    return value.m_valueObject?.ToString() ?? string.Empty;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region [ implicit operators Cast To EcaMeasurementValue ]

        public static implicit operator SttpValue(byte value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(short value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(int value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(long value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(ushort value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(uint value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(ulong value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(decimal value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(double value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(float value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(DateTime value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(TimeSpan value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(char value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(bool value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(Guid value)
        {
            return new SttpValue(value);
        }

        public static implicit operator SttpValue(string value)
        {
            return new SttpValue(value);
        }
        public static implicit operator SttpValue(byte[] value)
        {
            return new SttpValue(value);
        }

        #endregion

        /// <summary>
        /// A immutable class of an <see cref="SttpValue"/> that is marked as null
        /// </summary>
        public static readonly SttpValue NullValue;

        static SttpValue()
        {
            NullValue = new SttpValue();
            NullValue.IsImmutable = true;
        }

    }
}
