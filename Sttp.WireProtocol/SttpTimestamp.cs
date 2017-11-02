using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public enum SttpTimestampTypeCode
    {
        SttpTicks,       // A 64-bit UTC timestamp that holds leap second data.
        SttpTicksPlus,   // A 128-bit value that contains an extra 64-bits to be defined by the API level protocol.
    }

    /// <summary>
    /// This class contains the allowable PointID types for the STTP protocol. All pointIDs will be mapped to a 32-bit RuntimeID.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpTimestamp
    {
        #region [ Members ]

        [FieldOffset(0)]
        private long m_valueTicks;

        [FieldOffset(8)]
        private long m_valueFlags;

        [FieldOffset(17)]
        private SttpTimestampTypeCode m_typeCode;

        #endregion

        #region [ Constructors ]

        public SttpTimestamp()
        {
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. 
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpTimestamp(SttpTimestamp value)
        {
            m_valueTicks = value.m_valueTicks;
            m_valueFlags = value.m_valueFlags;
            m_typeCode = value.m_typeCode;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpTimestampTypeCode TypeCode
        {
            get
            {
                return m_typeCode;
            }
        }

        public long Ticks
        {
            get
            {
                return m_valueTicks;
            }
            set
            {
                m_typeCode = SttpTimestampTypeCode.SttpTicks;
                m_valueTicks = value;
            }
        }

        public long Flags
        {
            get
            {
                if (m_typeCode == SttpTimestampTypeCode.SttpTicks)
                    return m_valueFlags;
                throw new NotSupportedException();
            }
            set
            {
                m_typeCode = SttpTimestampTypeCode.SttpTicks;
                m_valueFlags = value;
            }
        }



        #endregion

    }
}
