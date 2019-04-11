using System;
using System.Collections.Generic;
using CTP;

namespace Sttp
{
    /// <summary>
    /// An ID that is assigned for each <see cref="SttpDataPoint"/>
    /// </summary>
    public class SttpDataPointID : IEquatable<SttpDataPointID>
    {
        /// <summary>
        /// The unique identifier for this PointID. This will typically be a GUID, but may also be a string or integer.
        /// </summary>
        public readonly CtpObject ID;
        private readonly int m_hashCode;

        /// <summary>
        /// An optional token that can be defined by the API layer. The primary purpose of this token is to speed up
        /// mapping since a dictionary lookup on ID is somewhat expensive.
        /// </summary>
        public object MetadataToken;

        public SttpDataPointID(CtpObject id)
        {
            ID = id;
            m_hashCode = ID.GetHashCode();
        }

        public bool Equals(SttpDataPointID other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SttpDataPointID)obj);
        }

        public override int GetHashCode()
        {
            return m_hashCode;
        }

    }
}