using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Maintains information about a DataPoint that does not change. 
    /// </summary>
    /// <remarks>>
    /// If information needs to change, the DataPoint will need to be re-negotiated.
    /// </remarks>
    public class DataPointKey
    {
        /// <summary>
        /// Unique data point identifier
        /// </summary>
        public readonly Guid UniqueID;
        public readonly ValueType Type;   // Value type of `DataPoint`
        public readonly bool HasQuality;

        /// <summary>
        /// Contains the list of all attributes that are associated with this measurement. 
        /// </summary>
        public readonly AttributeList Attributes;
    }

    // 23-bytes
}
