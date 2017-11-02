using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    public enum SttpPointIDTypeCode
    {
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
        private readonly long m_valueInt64;

        [FieldOffset(0)]
        private readonly Guid m_valueGuid;

        [FieldOffset(16)]
        private readonly string m_stringValue;

        [FieldOffset(25)]
        public readonly SttpPointIDTypeCode TypeCode;

        /// <summary>
        /// Defines the mapping code for the provided time quality.
        /// </summary>
        [FieldOffset(26)]
        public readonly short TimeQualityMap;

        /// <summary>
        /// Defines the mapping code for the provided value quality.
        /// </summary>
        [FieldOffset(28)]
        public readonly short ValueQualityMap;

        /// <summary>
        /// Defines the value type code for every measurement sent on the wire. 
        /// Note: value can still be null. But if it is not null, it will be this type.
        /// </summary>
        [FieldOffset(30)]
        public readonly short ValueTypeCode;

        /// <summary>
        /// The runtimeID associated with the PointID. This PointID must either be globally defined (if positive) or session defined (if negative)
        /// </summary>
        [FieldOffset(32)]
        public readonly int RuntimeID;

        #endregion

        #region [ Constructors ]

        public SttpPointID(string pointID, short timeQualityMap, short valueQualityMap, short valueTypeCode, int runtimeID)
        {
            TypeCode = SttpPointIDTypeCode.String;
            m_stringValue = pointID;
            TimeQualityMap = timeQualityMap;
            ValueQualityMap = valueQualityMap;
            ValueTypeCode = valueTypeCode;
            RuntimeID = runtimeID;
        }

        public SttpPointID(Guid pointID, short timeQualityMap, short valueQualityMap, short valueTypeCode, int runtimeID)
        {
            TypeCode = SttpPointIDTypeCode.Guid;
            m_valueGuid = pointID;
            TimeQualityMap = timeQualityMap;
            ValueQualityMap = valueQualityMap;
            ValueTypeCode = valueTypeCode;
            RuntimeID = runtimeID;
        }

        public SttpPointID(long pointID, short timeQualityMap, short valueQualityMap, short valueTypeCode, int runtimeID)
        {
            TypeCode = SttpPointIDTypeCode.String;
            m_valueInt64 = pointID;
            TimeQualityMap = timeQualityMap;
            ValueQualityMap = valueQualityMap;
            ValueTypeCode = valueTypeCode;
            RuntimeID = runtimeID;
        }

        #endregion

        #region [ Properties ]

        public long AsInt64
        {
            get
            {
                if (TypeCode == SttpPointIDTypeCode.Int64)
                    return m_valueInt64;
                throw new NotSupportedException();
            }
        }

        public Guid AsGuid
        {
            get
            {
                if (TypeCode == SttpPointIDTypeCode.Guid)
                    return m_valueGuid;
                throw new NotSupportedException();
            }
        }

        public string AsString
        {
            get
            {
                if (TypeCode == SttpPointIDTypeCode.String)
                    return m_stringValue;
                throw new NotSupportedException();
            }
        }

        #endregion

    }
}
