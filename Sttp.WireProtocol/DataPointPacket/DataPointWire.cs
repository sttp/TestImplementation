namespace Sttp.WireProtocol
{
    /// <summary>
    /// This data point is what is used to communicating with the <see cref="WireEncoder"/>/<see cref="WireDecoder"/>. 
    /// </summary>
    public class DataPointWire
    {
        /// <summary>
        /// Maps to DataPointKeyWire.RuntimeID
        /// </summary>
        public uint ID;

        /// <summary>
        /// Contains the Value.
        /// </summary>
        public readonly byte[] Value = new byte[64];

        /// <summary>
        /// The ID assigned to the bulk data since this packet could not be sized in 64 bytes.
        /// </summary>
        public uint BulkDataValueID;
        
        /// <summary>
        /// The total size of all fragments.
        /// </summary>
        public uint Length;

        public SttpTimestamp Time;

        public TimeQualityFlags Flags;

        public DataQualityFlags QualityFlags;

    }
}
