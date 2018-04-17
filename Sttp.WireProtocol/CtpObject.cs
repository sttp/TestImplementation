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

        #endregion

        #region [ Constructors ]

        public CtpObject()
        {
            SetNull();
        }

        public CtpObject(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                m_valueTypeCode = CtpTypeCode.Null;
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
                SetValue((double)(decimal)value);
            }
            else if (type == typeof(DateTime))
            {
                SetValue((DateTime)value);
            }
            else if (type == typeof(CtpTime))
            {
                SetValue((CtpTime)value);
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
            else if (type == typeof(CtpBuffer))
            {
                SetValue((CtpBuffer)value);
            }
            else if (type == typeof(byte[]))
            {
                SetValue((byte[])value);
            }
            else if (value is CtpObject)
            {
                SetValue((CtpObject)value);
            }
            else if (type == typeof(CtpDocument))
            {
                SetValue((CtpDocument)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

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

        public sbyte AsSByte
        {
            get
            {
                checked
                {
                    return (sbyte)AsInt32;
                }
            }
        }

        public ulong AsUInt64
        {
            get
            {
                checked
                {
                    return (ulong)AsInt64;
                }
            }
        }

        public int AsInt32
        {
            get
            {
                checked
                {
                    return (int)AsInt64;
                }
            }
        }

        public short AsInt16
        {
            get
            {
                checked
                {
                    return (short)AsInt32;
                }
            }
        }

        public byte AsByte
        {
            get
            {
                checked
                {
                    return (byte)AsInt32;
                }
            }
        }

        public ushort AsUInt16
        {
            get
            {
                checked
                {
                    return (ushort)AsInt32;
                }
            }
        }

        public uint AsUInt32
        {
            get
            {
                checked
                {
                    return (uint)AsInt64;
                }
            }
        }

        public DateTime AsDateTime => AsCtpTime.AsDateTime;

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
                        return AsDocument == other.AsDocument;
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
            if (a.m_valueTypeCode != b.m_valueTypeCode)
                return false;
            switch (a.m_valueTypeCode)
            {
                //ToDo: Finish.
            }
            return true;
        }

        public static bool operator !=(CtpObject a, CtpObject b)
        {
            return !(a == b);
        }

        #endregion

    }
}
