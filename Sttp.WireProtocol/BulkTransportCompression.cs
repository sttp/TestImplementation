namespace Sttp.WireProtocol
{
    public enum BulkTransportCompression : byte
    {
        /// <summary>
        /// No compression applied to packet content.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Stateless GZip applied to each packet.
        /// </summary>
        GZipPacket,

        /// <summary>
        /// GZip applied to entire message, must be reassembled on client side.
        /// </summary>
        GZipStream
    }
}