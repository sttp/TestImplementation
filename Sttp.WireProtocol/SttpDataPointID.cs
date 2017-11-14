using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public enum SttpDataPointIDTypeCode
    {
        /// <summary>
        /// The pointID is null
        /// </summary>
        Null = 0,
        /// <summary>
        /// ID is a Guid, or 128-bit integer. It doesn't have to be a true GUID.
        /// </summary>
        Guid = 1,
        /// <summary>
        /// ID is a string.
        /// </summary>
        String = 2,
        /// <summary>
        /// ID is a named set, Example: PMU-ID: 29384, Station: Shelby, PointName: PM1, Type: Analog, Position: 4
        /// </summary>
        NamedSet = 3,
    }

    /// <summary>
    /// This class contains all the variations of representing the PointID.
    /// 
    /// Ideally, all measurements will be mapped to a runtime ID, however, 
    /// for systems that contain millions of measurements, this is not a practical expectation.
    /// 
    /// For a system that could potentially have an unlimited number of PointIDs, the metadata must be stored with the point.
    /// This is the general use case for NamedSet. 
    /// Like financial institutions with billion of identifiers, but very low transactional count per identifier.
    /// 
    /// </summary>
    public class SttpDataPointID
    {
        #region [ Members ]

        private Guid m_valueGuid;
        private object m_valueObject;
        private SttpDataPointIDTypeCode m_valueTypeCode;
        public int RuntimeID { get; set; } = -1;

        #endregion

        #region [ Constructors ]

        public SttpDataPointID()
        {
            m_valueTypeCode = SttpDataPointIDTypeCode.Null;
        }

        #endregion

        #region [ Properties ]


        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpDataPointIDTypeCode ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
        }

        public Guid AsGuid
        {
            get
            {
                if (m_valueTypeCode == SttpDataPointIDTypeCode.Guid)
                {
                    return m_valueGuid;
                }
                throw new InvalidCastException("Value is not a Guid");
            }
            set
            {
                m_valueTypeCode = SttpDataPointIDTypeCode.Guid;
                m_valueGuid = value;
            }
        }

        public string AsString
        {
            get
            {
                if (m_valueTypeCode == SttpDataPointIDTypeCode.String)
                {
                    return m_valueObject as string;
                }
                throw new InvalidCastException("Value is not a String");
            }
            set
            {
                if (value == null)
                {
                    m_valueTypeCode = SttpDataPointIDTypeCode.Null;
                    m_valueObject = null;
                    return;
                }
                m_valueTypeCode = SttpDataPointIDTypeCode.String;
                m_valueObject = value;
            }
        }

        public SttpNamedSet AsNamedSet
        {
            get
            {
                if (m_valueTypeCode == SttpDataPointIDTypeCode.NamedSet)
                {
                    return m_valueObject as SttpNamedSet;
                }
                throw new InvalidCastException("Value is not a Named Set");
            }
            set
            {
                if (value == null)
                {
                    m_valueTypeCode = SttpDataPointIDTypeCode.Null;
                    m_valueObject = null;
                    return;
                }
                m_valueTypeCode = SttpDataPointIDTypeCode.NamedSet;
                m_valueObject = value;
            }
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by setting a value.
        /// </summary>
        /// <exception cref="InvalidOperationException">When setting this value to true.</exception>
        public bool IsNull
        {
            get
            {
                return m_valueTypeCode == SttpDataPointIDTypeCode.Null;
            }
            set
            {
                if (!value)
                    throw new InvalidOperationException("Can only set a value to null with this property, to set not null, use one of the other properties to set the value.");
                m_valueObject = null;
                m_valueTypeCode = SttpDataPointIDTypeCode.Null;
            }
        }



        #endregion
    }
}
