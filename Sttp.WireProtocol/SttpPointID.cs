using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public enum SttpPointIDTypeCode
    {
        RuntimeID, // A 32 bit value that defines a runtime id. Positive numbers have a 1-to-1 distinction for a static PointID, Negative numbers correspond to a session derived type.
        Int64,     // A 64-bit Basic Identifier.
        Guid,      // A 128-bit GUID
        String,    // A variable length string. Can be used for on the fly calculations such as: MW('ID:1234','ID:4562') 
    }

    /// <summary>
    /// This class contains the allowable PointID types for the STTP protocol. All pointIDs will be mapped to a 32-bit RuntimeID.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpPointID
    {
        #region [ Members ]

        [FieldOffset(0)]
        private ulong m_bytes0to7; //Used for cloning data.
        [FieldOffset(8)]
        private ulong m_bytes8to15; //Used for cloning data.

        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private Guid m_valueGuid;

        [FieldOffset(16)]
        private string m_stringValue;

        [FieldOffset(23)]
        private SttpPointIDTypeCode m_typeCode;

        #endregion

        #region [ Constructors ]

        public SttpPointID()
        {
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. 
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpPointID(SttpPointID value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_bytes8to15 = value.m_bytes8to15;
            m_stringValue = value.m_stringValue;
            m_typeCode = value.m_typeCode;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpPointIDTypeCode TypeCode
        {
            get
            {
                return m_typeCode;
            }
        }

        public long AsInt64
        {
            get
            {
                if (m_typeCode == SttpPointIDTypeCode.Int64)
                    return m_valueInt64;
                throw new NotSupportedException();
            }
            set
            {
                m_typeCode = SttpPointIDTypeCode.Int64;
                m_valueInt64 = value;
                m_stringValue = null;
            }
        }

        public Guid AsGuid
        {
            get
            {
                if (m_typeCode == SttpPointIDTypeCode.Guid)
                    return m_valueGuid;
                throw new NotSupportedException();
            }
            set
            {
                m_typeCode = SttpPointIDTypeCode.Guid;
                m_valueGuid = value;
                m_stringValue = null;
            }
        }

        public string AsString
        {
            get
            {
                if (m_typeCode == SttpPointIDTypeCode.String)
                    return m_stringValue;
                throw new NotSupportedException();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                m_typeCode = SttpPointIDTypeCode.String;
                m_bytes0to7 = 0;
                m_bytes8to15 = m_bytes0to7;
                m_stringValue = value;
            }
        }

        public int AsRuntimeID
        {
            get
            {
                if (m_typeCode == SttpPointIDTypeCode.RuntimeID)
                    return (int)m_valueInt64;
                throw new NotSupportedException();
            }
            set
            {
                m_typeCode = SttpPointIDTypeCode.RuntimeID;
                m_valueInt64 = value;
                m_stringValue = null;
            }
        }

        #endregion

    }
}
