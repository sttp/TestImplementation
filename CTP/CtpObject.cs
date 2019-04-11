using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for CTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct CtpObject : IEquatable<CtpObject>
    {
        public static readonly CtpObject Null = default(CtpObject);

        #region [ Members ]

        /// <summary>
        /// Extracts a value without type checking. Never use this unless a type check has already occurred.
        /// Use the corresponding 'Is' method to direct cast or throw an exception, or 'As' method to attempt to cast to they specified type.
        /// </summary>
        [FieldOffset(0)]
        public readonly long UnsafeInteger;
        [FieldOffset(0)]
        public readonly double UnsafeDouble;
        [FieldOffset(0)]
        public readonly float UnsafeSingle;
        [FieldOffset(0)]
        public readonly bool UnsafeBoolean;
        [FieldOffset(0)]
        public readonly CtpTime UnsafeCtpTime;
        [FieldOffset(0)]
        public readonly Guid UnsafeGuid;
        [FieldOffset(0)]
        public readonly CtpNumeric UnsafeNumeric;

        [FieldOffset(16)]
        private readonly object m_valueObject;


        /// <summary>
        /// The type code of the value.
        /// </summary>
        [FieldOffset(24)]
        public readonly CtpTypeCode ValueTypeCode;

        /// <summary>
        /// This extracts the first 8 bytes of a value.
        /// Important Note: primitives that are not exactly 8 bytes will
        /// exhibit an erratic behavior depending on the endian of the processor.
        /// </summary>
        [FieldOffset(0)]
        public readonly ulong UnsafeRawUInt64;

        /// <summary>
        /// This extracts the first 4 bytes of a value.
        /// Important Note: primitives that are not exactly 4 bytes will
        /// exhibit an erratic behavior depending on the endian of the processor.
        /// </summary>
        [FieldOffset(0)]
        public readonly uint UnsafeRawUInt32;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets if the specified value is considered the default value for that type.
        /// Default values for object types are zero length objects since null is a predefined data type.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                switch (ValueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return true;
                    case CtpTypeCode.Integer:
                    case CtpTypeCode.Single:
                    case CtpTypeCode.Double:
                    case CtpTypeCode.CtpTime:
                    case CtpTypeCode.Boolean:
                        return UnsafeInteger == 0;
                    case CtpTypeCode.Numeric:
                        return UnsafeNumeric.IsDefault;
                    case CtpTypeCode.Guid:
                        return UnsafeGuid == Guid.Empty;
                    case CtpTypeCode.String:
                        return ((string)m_valueObject).Length == 0;
                    case CtpTypeCode.CtpBuffer:
                        return ((CtpBuffer)m_valueObject).Length == 0;
                    case CtpTypeCode.CtpCommand:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public long IsInteger
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.Integer)
                    ThrowHelper(CtpTypeCode.Integer);
                return UnsafeInteger;
            }
        }

        public float IsSingle
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.Single)
                    ThrowHelper(CtpTypeCode.Single);
                return UnsafeSingle;
            }
        }

        public double IsDouble
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.Double)
                    ThrowHelper(CtpTypeCode.Double);
                return UnsafeDouble;
            }
        }

        public CtpNumeric IsNumeric
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.Numeric)
                    ThrowHelper(CtpTypeCode.Numeric);
                return UnsafeNumeric;
            }
        }

        public CtpTime IsCtpTime
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.CtpTime)
                    ThrowHelper(CtpTypeCode.CtpTime);
                return UnsafeCtpTime;
            }
        }

        public bool IsBoolean
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.Boolean)
                    ThrowHelper(CtpTypeCode.Boolean);
                return UnsafeBoolean;
            }
        }

        public Guid IsGuid
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.Guid)
                    ThrowHelper(CtpTypeCode.Guid);
                return UnsafeGuid;
            }
        }

        public string IsString
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.String)
                    ThrowHelper(CtpTypeCode.String);
                return UnsafeString;
            }
        }

        public string UnsafeString => (string)m_valueObject;

        public CtpBuffer IsCtpBuffer
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.CtpBuffer)
                    ThrowHelper(CtpTypeCode.CtpBuffer);
                return UnsafeCtpBuffer;
            }
        }

        public CtpBuffer UnsafeCtpBuffer => (CtpBuffer)m_valueObject;

        public CtpCommand IsCtpCommand
        {
            get
            {
                if (ValueTypeCode != CtpTypeCode.CtpCommand)
                    ThrowHelper(CtpTypeCode.CtpCommand);
                return UnsafeCtpCommand;
            }
        }

        public CtpCommand UnsafeCtpCommand => (CtpCommand)m_valueObject;


        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowHelper(CtpTypeCode code)
        {
            throw new InvalidOperationException($"The internal value is {ValueTypeCode}, not {code}");
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        public bool IsNull => ValueTypeCode == CtpTypeCode.Null;

        #endregion

        #region [ Methods ] 

        public override string ToString()
        {
            return ToTypeString;
        }

        public bool Equals(CtpObject other)
        {
            if (ValueTypeCode != other.ValueTypeCode)
                return false;

            switch (ValueTypeCode)
            {
                case CtpTypeCode.Boolean:
                case CtpTypeCode.Null:
                case CtpTypeCode.Integer:
                case CtpTypeCode.Single:
                case CtpTypeCode.Double:
                case CtpTypeCode.CtpTime:
                    return UnsafeInteger == other.UnsafeInteger;
                case CtpTypeCode.Numeric:
                    return UnsafeNumeric == other.UnsafeNumeric;
                case CtpTypeCode.Guid:
                    return UnsafeGuid == other.UnsafeGuid;
                case CtpTypeCode.String:
                    return (string)m_valueObject == (string)other.m_valueObject;
                case CtpTypeCode.CtpBuffer:
                    return (CtpBuffer)m_valueObject == (CtpBuffer)other.m_valueObject;
                case CtpTypeCode.CtpCommand:
                    return (CtpCommand)m_valueObject == (CtpCommand)other.m_valueObject;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpObject)obj);
        }

        public override int GetHashCode()
        {
            switch (ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    return (int)ValueTypeCode;
                case CtpTypeCode.Integer:
                    return (UnsafeInteger.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.Single:
                    return (UnsafeSingle.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.Double:
                    return (UnsafeDouble.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.Numeric:
                    return (UnsafeNumeric.GetHashCode() << 3) ^ UnsafeNumeric.GetHashCode();
                case CtpTypeCode.CtpTime:
                    return (UnsafeCtpTime.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.Boolean:
                    return (UnsafeBoolean.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.Guid:
                    return (UnsafeGuid.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.String:
                    return (m_valueObject.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.CtpBuffer:
                    return (m_valueObject.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                case CtpTypeCode.CtpCommand:
                    return (m_valueObject.GetHashCode() << 3) ^ ValueTypeCode.GetHashCode();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        public static CtpObject CreateDefault(CtpTypeCode typeCode)
        {
            switch (typeCode)
            {
                case CtpTypeCode.Null:
                    return CtpObject.Null;
                case CtpTypeCode.Integer:
                    return new CtpObject(0);
                case CtpTypeCode.Single:
                    return new CtpObject(0f);
                case CtpTypeCode.Double:
                    return new CtpObject(0d);
                case CtpTypeCode.Numeric:
                    return new CtpObject(default(CtpNumeric));
                case CtpTypeCode.CtpTime:
                    return new CtpTime(0);
                case CtpTypeCode.Boolean:
                    return new CtpObject(false);
                case CtpTypeCode.Guid:
                    return new CtpObject(Guid.Empty);
                case CtpTypeCode.String:
                    return new CtpObject(string.Empty);
                case CtpTypeCode.CtpBuffer:
                    return new CtpObject(CtpBuffer.Empty);
                case CtpTypeCode.CtpCommand:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, null);
            }
        }
    }
}
