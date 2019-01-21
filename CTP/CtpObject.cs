using System;
using System.Runtime.InteropServices;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for CTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public partial struct CtpObject : IEquatable<CtpObject>
    {
        public static readonly CtpObject Null = default(CtpObject);

        #region [ Members ]

        [FieldOffset(0)]
        private readonly long m_valueInt64;
        [FieldOffset(0)]
        private readonly double m_valueDouble;
        [FieldOffset(0)]
        private readonly float m_valueSingle;
        [FieldOffset(0)]
        private readonly bool m_valueBoolean;
        [FieldOffset(0)]
        private readonly CtpTime m_valueCtpTime;
        [FieldOffset(0)]
        private readonly Guid m_valueGuid;

        [FieldOffset(16)]
        private readonly object m_valueObject;

        [FieldOffset(24)]
        private readonly CtpTypeCode m_valueTypeCode;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The type code of the value.
        /// </summary>
        public CtpTypeCode ValueTypeCode => m_valueTypeCode;

        public long IsInt64
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.Int64)
                    ThrowHelper(CtpTypeCode.Int64);
                return m_valueInt64;
            }
        }

        public float IsSingle
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.Single)
                    ThrowHelper(CtpTypeCode.Single);
                return m_valueSingle;
            }
        }

        public double IsDouble
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.Double)
                    ThrowHelper(CtpTypeCode.Double);
                return m_valueDouble;
            }
        }

        public CtpTime IsCtpTime
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.CtpTime)
                    ThrowHelper(CtpTypeCode.CtpTime);
                return m_valueCtpTime;
            }
        }

        public bool IsBoolean
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.Boolean)
                    ThrowHelper(CtpTypeCode.Boolean);
                return m_valueBoolean;
            }
        }

        public Guid IsGuid
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.Guid)
                    ThrowHelper(CtpTypeCode.Guid);
                return m_valueGuid;
            }
        }

        public string IsString
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.String)
                    ThrowHelper(CtpTypeCode.String);
                return (string)m_valueObject;
            }
        }

        public CtpBuffer IsCtpBuffer
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.CtpBuffer)
                    ThrowHelper(CtpTypeCode.CtpBuffer);
                return (CtpBuffer)m_valueObject;
            }
        }

        public CtpCommand IsCtpCommand
        {
            get
            {
                if (m_valueTypeCode != CtpTypeCode.CtpCommand)
                    ThrowHelper(CtpTypeCode.CtpCommand);
                return (CtpCommand)m_valueObject;
            }
        }

        private void ThrowHelper(CtpTypeCode code)
        {
            throw new InvalidOperationException($"The internal value is {m_valueTypeCode}, not {code}");
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        public bool IsNull => ValueTypeCode == CtpTypeCode.Null;

        #endregion

        #region [ Methods ] 

        public CtpObject Clone()
        {
            return (CtpObject)MemberwiseClone();
        }

        public override string ToString()
        {
            return ToTypeString;
        }

        public bool Equals(CtpObject other)
        {
            if (m_valueTypeCode == other.m_valueTypeCode)
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return true; //Null == Null, should that return false?
                    case CtpTypeCode.Int64:
                        return m_valueInt64 == other.m_valueInt64;
                    case CtpTypeCode.Single:
                        return m_valueSingle == other.m_valueSingle;
                    case CtpTypeCode.Double:
                        return m_valueDouble == other.m_valueDouble;
                    case CtpTypeCode.CtpTime:
                        return m_valueCtpTime == other.m_valueCtpTime;
                    case CtpTypeCode.Boolean:
                        return m_valueBoolean == other.m_valueBoolean;
                    case CtpTypeCode.Guid:
                        return m_valueGuid == other.m_valueGuid;
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

            if (m_valueTypeCode == CtpTypeCode.Null || other.m_valueTypeCode == CtpTypeCode.Null)
                return false;

            switch (m_valueTypeCode)
            {
                case CtpTypeCode.Int64:
                    {
                        var value = m_valueInt64;
                        switch (other.m_valueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.m_valueInt64;
                            case CtpTypeCode.Single:
                                return value == other.m_valueSingle;
                            case CtpTypeCode.Double:
                                return value == other.m_valueDouble;
                        }
                        break;
                    }
                case CtpTypeCode.Single:
                    {
                        var value = m_valueSingle;
                        switch (other.m_valueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.m_valueInt64;
                            case CtpTypeCode.Single:
                                return value == other.m_valueSingle;
                            case CtpTypeCode.Double:
                                return value == other.m_valueDouble;
                        }
                        break;
                    }
                case CtpTypeCode.Double:
                    {
                        var value = m_valueDouble;
                        switch (other.m_valueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.m_valueInt64;
                            case CtpTypeCode.Single:
                                return value == other.m_valueSingle;
                            case CtpTypeCode.Double:
                                return value == other.m_valueDouble;
                        }
                        break;
                    }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpObject)obj);
        }

        public static bool operator ==(CtpObject a, CtpObject b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CtpObject a, CtpObject b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            switch (m_valueTypeCode)
            {
                case CtpTypeCode.Null:
                    return (int)m_valueTypeCode;
                case CtpTypeCode.Int64:
                    return (m_valueInt64.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.Single:
                    return (m_valueSingle.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.Double:
                    return (m_valueDouble.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.CtpTime:
                    return (m_valueCtpTime.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.Boolean:
                    return (m_valueBoolean.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.Guid:
                    return (m_valueGuid.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.String:
                    return (m_valueObject.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.CtpBuffer:
                    return (m_valueObject.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                case CtpTypeCode.CtpCommand:
                    return (m_valueObject.GetHashCode() << 3) ^ m_valueTypeCode.GetHashCode();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        #endregion

    }
}
