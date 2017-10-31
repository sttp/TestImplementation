namespace Sttp.WireProtocol.RegisterDataPoint
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Identifies new data point to send
        /// 
        /// Payload:
        /// uint DataPointID
        /// uint RuntimeID
        /// ValueType Type
        /// StateFlags Flags
        /// </summary>
        NewPoint,
    }
}