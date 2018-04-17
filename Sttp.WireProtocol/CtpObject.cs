using System;
using System.Runtime.InteropServices;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public partial class CtpObject : IEquatable<CtpObject>
    {
        public static CtpObject Null => new CtpObject();

        #region [ Members ]

        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private double m_valueDouble;

        [FieldOffset(0)]
        private float m_valueSingle;
        [FieldOffset(0)]
        private bool m_valueBoolean;
        [FieldOffset(0)]
        private CtpTime m_valueCtpTime;
        [FieldOffset(0)]
        private Guid m_valueGuid;

        [FieldOffset(16)]
        private object m_valueObject;

        [FieldOffset(24)]
        private CtpTypeCode m_valueTypeCode;

        [FieldOffset(0)]
        private long m_raw0;
        [FieldOffset(8)]
        private long m_raw1; 

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public CtpTypeCode ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
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
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

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
                    case CtpTypeCode.CtpDocument:
                        return AsCtpDocument == other.AsCtpDocument;
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
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CtpObject)obj);
        }

        public static bool operator ==(CtpObject a, CtpObject b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (ReferenceEquals(a, null))
                return false;
            if (ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(CtpObject a, CtpObject b)
        {
            return !(a == b);
        }

        #endregion

    }
}
