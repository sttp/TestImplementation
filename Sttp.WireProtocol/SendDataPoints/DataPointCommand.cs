namespace Sttp.WireProtocol.SendDataPoints
{
    public enum DataPointCommand : byte
    {
        /// <summary>
        /// Map a new runtime point id.
        /// </summary>
        RegisterDataPoint,

        /// <summary>
        /// Sends an individual data point.
        /// </summary>
        SendDataPoint,

        /// <summary>
        /// Sends a data point that is larger than 64 bytes in size.
        /// </summary>
        SendLargeDataPoint,
    }
}
