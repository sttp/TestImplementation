using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public enum SttpPointIDTypeCode
    {
        /// <summary>
        /// A 32-bit Runtime ID, Realistically, it will be closer to 20 bits or less.
        /// </summary>
        RuntimeID,
        /// <summary>
        /// ID is a Guid, or 128-bit integer. It doesn't have to be a true GUID.
        /// </summary>
        Guid,
        /// <summary>
        /// ID is a string.
        /// </summary>
        String,
        /// <summary>
        /// ID is a named set, Example: PMU-ID: 29384, Station: Shelby, PointName: PM1, Type: Analog, Position: 4
        /// </summary>
        NamedSet,
        /// <summary>
        /// This value is invalid and cannot be serialized, but is present to identify when it's unassigned.
        /// </summary>
        Invalid,
    }

    /// <summary>
    /// This class contains all the variations of representing the PointID.
    /// 
    /// Ideally, all measurements will be mapped to a runtime ID, however, 
    /// for systems that contain millions of measurements, this is not a practical expectation.
    /// 
    /// I system could potentially have an unlimited number of PointIDs, and the metadata must be stored with the point. 
    /// Like financial institutions with billion of identifiers, but very low transactional count per identifier.
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpPointID
    {
        #region [ Members ]

        [FieldOffset(0)]
        private int m_valueInt32;
        [FieldOffset(0)]
        private Guid m_valueGuid;
        [FieldOffset(16)]
        private object m_valueObject;
        [FieldOffset(25)]
        private SttpPointIDTypeCode m_valueTypeCode;

        #endregion

        #region [ Constructors ]

        public SttpPointID()
        {
            m_valueTypeCode = SttpPointIDTypeCode.Invalid;
        }

        #endregion

        #region [ Properties ]

        public int AsRuntimeID
        {
            get
            {
                if (m_valueTypeCode == SttpPointIDTypeCode.RuntimeID)
                {
                    return m_valueInt32;
                }
                throw new InvalidCastException("Value is not a runtime ID");
            }
            set
            {
                m_valueTypeCode = SttpPointIDTypeCode.RuntimeID;
                m_valueInt32 = value;

            }
        }

        public Guid AsGuid
        {
            get
            {
                if (m_valueTypeCode == SttpPointIDTypeCode.Guid)
                {
                    return m_valueGuid;
                }
                throw new InvalidCastException("Value is not a Guid");
            }
            set
            {
                m_valueTypeCode = SttpPointIDTypeCode.Guid;
                m_valueGuid = value;
            }
        }

        public string AsString
        {
            get
            {
                if (m_valueTypeCode == SttpPointIDTypeCode.String)
                {
                    return m_valueObject as string;
                }
                throw new InvalidCastException("Value is not a String");
            }
            set
            {
                if (value == null)
                {
                    m_valueTypeCode = SttpPointIDTypeCode.Invalid;
                    m_valueObject = null;
                    return;
                }
                m_valueTypeCode = SttpPointIDTypeCode.String;
                m_valueObject = value;
            }
        }

        public SttpNamedSet AsNamedSet
        {
            get
            {
                if (m_valueTypeCode == SttpPointIDTypeCode.NamedSet)
                {
                    return m_valueObject as SttpNamedSet;
                }
                throw new InvalidCastException("Value is not a Named Set");
            }
            set
            {
                if (value == null)
                {
                    m_valueTypeCode = SttpPointIDTypeCode.Invalid;
                    m_valueObject = null;
                    return;
                }
                m_valueTypeCode = SttpPointIDTypeCode.NamedSet;
                m_valueObject = value;
            }
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        /// <exception cref="InvalidOperationException">When setting this value to true. Set a value by calling one of the <see cref="SetValue"/> methods.</exception>
        public bool IsNull
        {
            get
            {
                return m_valueTypeCode == SttpPointIDTypeCode.Invalid;
            }
            set
            {
                if (!value)
                    throw new InvalidOperationException("Can only set a value to null with this property, to set not null, use one of the other properties to set the value.");
                m_valueObject = null;
                m_valueTypeCode = SttpPointIDTypeCode.Invalid;
            }
        }

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpPointIDTypeCode ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
        }

        #endregion
    }
}
