
namespace Sttp.WireProtocol
{
    public enum CommandCode : byte
    {
        //Finalized Names;

        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Contains Metadata related commands.
        /// </summary>
        Metadata = 0x02,

        /// <summary>
        /// Capable of sending large blocks of data over STTP.
        /// </summary>
        BulkTransport = 0x03,

        /// <summary>
        /// Indicates that this packet is a deflate encapsulated packet.
        /// 
        /// Payload:
        /// int Length,
        /// byte[] CompressedData
        /// </summary>
        DeflatePacket = 0x04,

        // TODO : assign values
        NegotiateSession,
        Subscribe,
        SecureDataChannel,
        RuntimeIDMapping,
        DataPointPacket,
        /// <summary>
        /// Indicates that a fragmented packet is being sent
        /// </summary>
        Fragment,

        

        NoOp = 0xFF,
    }
}