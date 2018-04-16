using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public partial class CtpValueMutable : CtpValue
    {
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

        [FieldOffset(33)]
        private CtpTypeCode m_valueTypeCode;

        #endregion

        #region [ Constructors ]

        public CtpValueMutable()
        {
            SetNull();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public override CtpTypeCode ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
        }

        #endregion

        #region [ Methods ] 

        public CtpValue CloneAsImmutable()
        {
            switch (ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    return CtpValue.Null;
                case CtpTypeCode.Int64:
                    return (CtpValue)AsInt64;
                case CtpTypeCode.Single:
                    return (CtpValue)AsSingle;
                case CtpTypeCode.Double:
                    return (CtpValue)AsDouble;
                case CtpTypeCode.CtpTime:
                    return (CtpValue)AsCtpTime;
                case CtpTypeCode.Boolean:
                    return (CtpValue)AsBoolean;
                case CtpTypeCode.Guid:
                    return (CtpValue)AsGuid;
                case CtpTypeCode.String:
                    return (CtpValue)AsString;
                case CtpTypeCode.CtpBuffer:
                    return (CtpValue)AsSttpBuffer;
                case CtpTypeCode.CtpDocument:
                    return (CtpValue)AsDocument;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool operator ==(CtpValueMutable a, CtpValueMutable b)
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

        public static bool operator !=(CtpValueMutable a, CtpValueMutable b)
        {
            return !(a == b);
        }

        #endregion

    }
}
