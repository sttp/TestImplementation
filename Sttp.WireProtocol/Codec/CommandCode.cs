
namespace Sttp.Codec
{
    public enum CommandCode : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Indicates that a fragmented packet is being sent. Fragmented packets 
        /// are for the wire protocol to ensure every packet fits the desired transport size. 
        /// This functionality is handled internally and not exposed for use. To send large packets
        /// over the wire, the user must send their data with the BulkTransport command.
        /// 
        /// Fragmented packets must be sent one at a time in sequence and cannot be 
        /// interwoven with any other kind of packet.
        /// 
        /// Fragments are limited to a size configure in the SessionDetails, but is on the order 
        /// of MB's.
        /// 
        /// Fragmented packets can have a compression=none.
        /// 
        /// If TotalFragmentSize == [Length of first fragment], this first fragment is the only fragment.
        /// This occurs when the packet is compressed, but not fragmented.
        /// 
        /// Layout:
        /// int TotalFragmentSize     - The size of all fragments.
        /// int TotalRawSize          - The uncompressed data size.
        /// byte CommandCode          - The Command of the data that is encapsulated.
        /// byte CompressionMode      - The algorithm that is used to compress the data.
        /// (Implied) Length of first fragment
        /// byte[] firstFragment
        /// 
        /// </summary>
        BeginFragment,

        /// <summary>
        /// Specifies the next fragment of data. When Offset + Length of Fragment == TotalFragmentSize, the fragment is completed.
        /// Since fragments are sequential, the offset is known, and the length is computed from the packet overhead.
        /// 
        /// Layout:
        /// (Implied) Offset position in Fragment
        /// (Implied) Length of fragment
        /// byte[] Fragment               
        /// </summary>
        NextFragment,

        /// <summary>
        /// Streaming of real-time data. This command is extremely simplified since 
        /// the payload for this kind of data is small, so overhead is more costly.
        /// 
        /// Payload:
        /// byte encodingMethod 
        /// byte[] Data;
        /// 
        /// </summary>
        SubscriptionStream,

        /// <summary>
        /// Streaming of real-time data. This command is extremely simplified since 
        /// the payload for this kind of data is small, so overhead is more costly.
        /// 
        /// Payload:
        /// string CommandName 
        /// SttpMarkup Data;
        /// </summary>
        MarkupCommand,

    }
}