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
        public Guid UniqueID;
        public ValueType Type;   // Value type of `DataPoint`
        public bool HasQuality;
    }
    // 23-bytes
}
