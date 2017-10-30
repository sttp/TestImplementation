
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
        /// Fragmented packets can have a compression=none.
        /// 
        /// Layout:
        /// byte CommandCode.BeginFragment 
        /// short PacketLength
        /// int TotalFragmentSize     - The size of all fragments.
        /// int TotalRawSize          - The uncompressed data size.
        /// byte CommandCode          - The Command of the data that is encapsulated.
        /// byte CompressionMode      - The algorithm that is used to compress the data.
        /// (Implied) Length of first fragment
        /// byte[] firstFragment
        /// </summary>
        BeginFragment,

        /// <summary>
        /// Specifies the next fragment of data. When Offset + Length of data == TotalFragmentSize, the fragment is completed.
        /// 
        /// Layout:
        /// byte CommandCode.NextFragment 
        /// short PacketLength
        /// (Implied) Length of fragment
        /// byte[] Fragment               
        /// </summary>
        NextFragment,

        /// <summary>
        /// Indicates a packet that is compressed but not fragmented. This will decrease the overhead 
        /// 
        /// Layout:
        /// byte CommandCode.CompressedPacket 
        /// short PacketLength
        /// int TotalRawSize          - The uncompressed data size.
        /// byte CommandCode,         - The Command of the data that is encapsulated.
        /// byte CompressionMode      - The algorithm that is used to compress the data.
        /// (Implied) Length of compressed data.
        /// byte[] CompressedData
        /// </summary>
        CompressedPacket,

        // TODO : assign values
        NegotiateSession,
        Subscribe,
        SecureDataChannel,
        RuntimeIDMapping,
        DataPointPacket,

        NoOp = 0xFF,
    }
}