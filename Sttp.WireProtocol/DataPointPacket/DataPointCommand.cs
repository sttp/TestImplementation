namespace Sttp.WireProtocol.Codec.DataPointPacket
{
    public enum DataPointCommand : byte
    {
        /// <summary>
        /// Map a new runtime point id.
        /// </summary>
        MapRuntimeID,

        /// <summary>
        /// Sends a data point of the normal size.
        /// </summary>
        SendDataPoint,

        /// <summary>
        /// Sends a data point that is larger than 64bytes in size.
        /// </summary>
        SendLargeDataPoint,
    }
}
