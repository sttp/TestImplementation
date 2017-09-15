using System;

namespace Sttp.WireProtocol
{
    public class DataPointKey
    {
        public Guid UniqueID;    // Unique data point identifier - maps to metadata `Measurement.uniqueID`
        public uint RuntimeID;   // Runtime identifier as referenced by `DataPoint`
        public ValueType Type;   // Value type of `DataPoint`
        public StateFlags Flags; // State flags for `DataPoint`
    }
    // 23-bytes
}
