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
        /// long Size,
        /// long OrigionalSize,
        /// Enum Mode: {Data Packet; Metadata packet; Internal Fragment; User Defined}
        /// bool isGzip
        /// </summary>
        BeginBulkTransport,

        /// <summary>
        /// Indicates that the pending bulk transer is to be canceled. Can be sent in either direction.
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
        /// long Offset,
        /// int Length,
        /// </summary>
        SendFragment

    }
}
