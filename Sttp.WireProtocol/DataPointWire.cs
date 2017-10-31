using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    /// <summary>
    /// This data point is what is used to communicating with the <see cref="WireEncoder"/>/<see cref="WireDecoder"/>. 
    /// </summary>
    public class DataPointWire
    {
        /// <summary>
        /// Maps to DataPointKeyWire.RuntimeID
        /// </summary>
        public uint DataPointID;

        public SttpTimestamp Time;

        /// <summary>
        /// The type of the Value field.
        /// </summary>
        public ValueType ValueType;

        /// <summary>
        /// Contains the Value.
        /// </summary>
        public readonly byte[] Value = new byte[64];

        /// <summary>
        /// The length of the value field
        /// </summary>
        public uint ValueLength;

        /// <summary>
        /// The ID assigned to the bulk data since this packet could not be sized in 64 bytes.
        /// </summary>
        public Guid BulkDataValueID;

        public TimeQualityFlags TimeQualityFlags;

        public DataQualityFlags DataQualityFlags;

    }
}
