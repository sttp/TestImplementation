namespace Sttp.WireProtocol
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
}