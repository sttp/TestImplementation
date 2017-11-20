namespace Sttp.WireProtocol.BulkTransportBeginSend
{
    public enum BulkTransportMode : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Contents are TODO document this
        /// </summary>
        DataPacket,

        /// <summary>
        /// Contents are metadata.
        /// </summary>
        MetadataPacket,

        /// <summary>
        /// Contents are custom user defined data.
        /// </summary>
        UserDefined
    }

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