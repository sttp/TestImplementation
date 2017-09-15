namespace Sttp.WireProtocol
{
    /// <summary>
    /// Represents a single point of data.
    /// 
    /// Note: this should be turned into a reusable class and therefore the initial size of 
    /// Value and State should be the maximum supported size. It's not necessary to clear
    /// the unused bytes on Value and State.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Maps to DataPointKey.RuntimeID
        /// </summary>
        public uint ID;
        /// <summary>
        /// Contains the Value.
        /// </summary>
        public byte[] Value;
        /// <summary>
        /// Contains the Timestamp and Quality flags. For variable length types, it can also include length and sequence numbers.
        /// </summary>
        public byte[] State;
    }
}
