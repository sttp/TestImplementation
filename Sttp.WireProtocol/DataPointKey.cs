using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Maintains information about a DataPoint that does not change. 
    /// (At least if it does, the DataPoint will need to be re-negotiated.)
    /// </summary>
    public class DataPointKey
    {
        /// <summary>
        /// Unique data point identifier - maps to metadata `Measurement.uniqueID`
        /// Note: This unique identifier maps to a record in the metadata tables of the publisher. 
        /// This identifier is not required to remain the same from connection to connection 
        /// since some publishers may not store this identifier. (Ex: A PMU). In this case,
        /// the provided metadata tables must be used to map the identifer on the subscriber's end.
        /// 
        /// Also note: for PDC to PDC connections, it's possible that GUIDs aren't the same in both systems,
        /// so an identifier mapping table might need to be created..
        /// </summary>
        public Guid UniqueID;
        /// <summary>
        /// The runtime identifier negotiated for this measurement. This is only valid for the current connection
        /// and is renegotiated every connection.
        /// </summary>
        public uint RuntimeID;
        public ValueType Type;   // Value type of `DataPoint`
        public StateFlags Flags; // State flags for `DataPoint`
    }
    // 23-bytes
}
