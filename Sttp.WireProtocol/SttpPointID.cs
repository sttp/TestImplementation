using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{

    /// <summary>
    /// This class contains required metadata for each STTP point. 
    /// </summary>
    public class SttpPointID
    {
        #region [ Members ]

        private object m_pointIDToken;
        private long m_pointID;
        private Guid m_pointGuid;
        private string m_pointString;
        private short m_timeQualityMap;
        private short m_valueQualityMap;
        private short m_valueTypeCode;
        private int m_runtimeID;
        private bool m_hasPointID;
        private bool m_hasPointGuid;
        private bool m_hasPointString;
        private bool m_hasRuntimeID;
        private bool m_isImmutable;

        #endregion


        /// <summary>
        /// An API definable token that can be used to cache the lookup information for this DataPoint's metadata.
        /// </summary>
        public object PointIDToken
        {
            get
            {
                return m_pointIDToken;
            }
            set
            {
                m_pointIDToken = value;
            }
        }

        /// <summary>
        /// A 64-bit Basic Identifier.
        /// </summary>
        public long PointID
        {
            get
            {
                return m_pointID;
            }
            set
            {
                CheckImmutable();
                m_hasPointID = true;
                m_pointID = value;
            }
        }

        /// <summary>
        /// A 128-bit GUID
        /// </summary>
        public Guid PointGuid
        {
            get
            {
                return m_pointGuid;
            }
            set
            {
                CheckImmutable();
                m_pointGuid = value;
                m_hasPointGuid = true;
            }
        }

        /// <summary>
        /// A variable length string. Can be used for on the fly calculations such as: MW('ID:1234','ID:4562')
        /// </summary>
        public string PointString
        {
            get
            {
                return m_pointString;
            }
            set
            {
                CheckImmutable();
                m_pointString = value;
                m_hasPointString = (value != null);
            }
        }
        /// <summary>
        /// The runtimeID associated with the PointID. This PointID must either be globally defined (if positive) or session defined (if negative).
        /// </summary>
        public int RuntimeID
        {
            get
            {
                return m_runtimeID;
            }
            set
            {
                CheckImmutable();
                m_runtimeID = value;
                m_hasRuntimeID = true;
            }
        }

        /// <summary>
        /// Defines the mapping code for the provided time quality.
        /// </summary>
        public short TimeQualityMap
        {
            get
            {
                return m_timeQualityMap;
            }
            set
            {
                CheckImmutable();
                m_timeQualityMap = value;
            }
        }

        /// <summary>
        /// Defines the mapping code for the provided value quality.
        /// </summary>
        public short ValueQualityMap
        {
            get
            {
                return m_valueQualityMap;
            }
            set
            {
                CheckImmutable();
                m_valueQualityMap = value;
            }
        }

        /// <summary>
        /// Defines the value type code for every measurement sent on the wire. 
        /// Note: value can still be null. But if it is not null, it will be this type.
        /// </summary>
        public short ValueTypeCode
        {
            get
            {
                return m_valueTypeCode;
            }
            set
            {
                CheckImmutable();
                m_valueTypeCode = value;
            }
        }

        public bool HasPointID
        {
            get
            {
                return m_hasPointID;
            }
            set
            {
                CheckImmutable();
                m_hasPointID = value;
            }
        }

        public bool HasPointGuid
        {
            get
            {
                return m_hasPointGuid;
            }
            set
            {
                CheckImmutable();
                m_hasPointGuid = value;
            }
        }

        public bool HasPointString
        {
            get
            {
                return m_hasPointString;
            }
            set
            {
                CheckImmutable();
                m_hasPointString = value;
            }
        }

        public bool HasRuntimeID
        {
            get
            {
                return m_hasRuntimeID;
            }
            set
            {
                CheckImmutable();
                m_hasRuntimeID = value;
            }
        }

        /// <summary>
        /// Gets/Sets this class as Immutable. Once set, this field cannot be cleared.
        /// </summary>
        public bool IsImmutable
        {
            get
            {
                return m_isImmutable;
            }
            set
            {
                //This flag can only be set and cannot be reverted.
                if (value)
                {
                    //Even if the class is immutable, setting this flag again should not throw an exception.
                    m_isImmutable = true;
                }
                else
                {
                    //If trying to set this to false, a simple check will suffice. If the check succeeds, then it's already false.
                    CheckImmutable();
                }
            }
        }

        private void CheckImmutable()
        {
            if (m_isImmutable)
                ThrowImmutableException();
        }

        private void ThrowImmutableException()
        {
            throw new InvalidOperationException("Value has been marked as immutable and cannot be modified.");
        }



    }
}
