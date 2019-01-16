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
        public static CtpObject Null => default(CtpObject);

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
            if (ValueTypeCode == other.ValueTypeCode)
            {
                switch (ValueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return true; //Null == Null, should that return false?
                    case CtpTypeCode.Int64:
                        return AsInt64 == other.AsInt64;
                    case CtpTypeCode.Single:
                        return AsSingle == other.AsSingle;
                    case CtpTypeCode.Double:
                        return AsDouble == other.AsDouble;
                    case CtpTypeCode.CtpTime:
                        return AsCtpTime == other.AsCtpTime;
                    case CtpTypeCode.Boolean:
                        return AsBoolean == other.AsBoolean;
                    case CtpTypeCode.Guid:
                        return AsGuid == other.AsGuid;
                    case CtpTypeCode.String:
                        return AsString == other.AsString;
                    case CtpTypeCode.CtpBuffer:
                        return AsCtpBuffer == other.AsCtpBuffer;
                    case CtpTypeCode.CtpCommand:
                        return AsCtpCommand == other.AsCtpCommand;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (ValueTypeCode == CtpTypeCode.Null || other.ValueTypeCode == CtpTypeCode.Null)
                return false;

            switch (ValueTypeCode)
            {
                case CtpTypeCode.Int64:
                    {
                        var value = AsInt64;
                        switch (other.ValueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.AsInt64;
                            case CtpTypeCode.Single:
                                return value == other.AsSingle;
                            case CtpTypeCode.Double:
                                return value == other.AsDouble;
                        }
                        break;
                    }
                case CtpTypeCode.Single:
                    {
                        var value = AsSingle;
                        switch (other.ValueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.AsInt64;
                            case CtpTypeCode.Single:
                                return value == other.AsSingle;
                            case CtpTypeCode.Double:
                                return value == other.AsDouble;
                        }
                        break;
                    }
                case CtpTypeCode.Double:
                    {
                        var value = AsDouble;
                        switch (other.ValueTypeCode)
                        {
                            case CtpTypeCode.Int64:
                                return value == other.AsInt64;
                            case CtpTypeCode.Single:
                                return value == other.AsSingle;
                            case CtpTypeCode.Double:
                                return value == other.AsDouble;
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



        #endregion

    }
}
