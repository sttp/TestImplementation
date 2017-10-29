
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

        /// <summary>
        /// Indicates that a fragmented packet is being sent. Fragmented packets 
        /// are for the wire protocol to ensure every packet fits the MSS size. Users
        /// must send their bulk data over BulkTransport.
        /// 
        /// Fragmented packets must be sent one at a time in sequence and cannot be 
        /// interwoven with any other kind of packet.
        /// 
        /// Fragments are limited to a size configure in the SessionDetails, but is on the order 
        /// of MB's.
        /// 
        /// Payload:
        /// int FragmentID     - A sequential counter to ensure that fragments are not interwoven.
        /// int OrigionalSize  - The size of the original packet.
        /// int Offset         - The offset of the incoming data in respects to the original data.
        /// short fragmentSize - MSS cannot exceed 32KB
        /// byte[] data
        /// 
        /// </summary>
        Fragment,

        // TODO : assign values
        NegotiateSession,
        Subscribe,
        SecureDataChannel,
        RuntimeIDMapping,
        DataPointPacket,
        

        

        NoOp = 0xFF,
    }
}