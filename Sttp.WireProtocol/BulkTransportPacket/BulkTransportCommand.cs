using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransportPacket
{
    public enum BulkTransportCommand : byte
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
        /// ulong OrigionalSize,
        /// Enum Mode: {Data Packet; Metadata packet; Internal Fragment; User Defined}
        /// bool isGzip
        /// byte[] Data
        /// </summary>
        BeginBulkTransport,

        /// <summary>
        /// Indicates that the pending bulk transfer is to be canceled. Can be sent in either direction.
        /// 
        /// Payload: 
        /// Guid ID,
        /// </summary>
        CancelBulkTransport,

        /// <summary>
        /// Sends a fragment for the previously defined bulk transport. Maximum bytes sent per command is 1000 bytes
        /// 
        /// Payload: 
        /// Guid ID,
        /// ulong Offset 
        /// byte[] Data
        /// </summary>
        SendFragment

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
