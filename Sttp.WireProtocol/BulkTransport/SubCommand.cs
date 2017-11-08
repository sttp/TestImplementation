namespace Sttp.WireProtocol.BulkTransport
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
        /// Notifies that a new bulk transport packet is pending to be sent over the wire.
        /// 
        /// Payload:
        /// Guid ID,
        /// BulkTransportMode mode,
        /// BulkTransportCompression compression,
        /// ulong OrigionalSize,
        /// byte[] Data
        /// </summary>
        BeginSend,

        /// <summary>
        /// Sends a fragment for the previously defined bulk transport. 
        /// 
        /// Payload: 
        /// Guid ID,
        /// ulong bytesRemaining 
        /// byte[] Data
        /// </summary>
        SendFragment,

        /// <summary>
        /// Indicates that the pending bulk transfer is to be canceled. Can be sent in either direction.
        /// 
        /// Payload: 
        /// Guid ID,
        /// </summary>
        CancelSend,
    }

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