
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
        /// Requests metadata from the server.
        /// </summary>
        GetMetadata,

        /// <summary>
        /// Supplies metadata. Can be solicited or unsolicited.
        /// </summary>
        Metadata,

        /// <summary>
        /// Commands for the real-time subscription data point stream. 
        /// </summary>
        Subscription,

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
        /// Response to a request/reply method of getting data.
        /// </summary>
        DataPoints,

        /// <summary>
        /// An out of bounds request for specific data. 
        /// This is formatted as request/reply as compared to streaming data.
        /// </summary>
        GetDataPoints,

        /// <summary>
        /// Negotiates session variables and roles.
        /// </summary>
        NegotiateSession,

        /// <summary>
        /// A unsolicited or feedback message for a recent command.
        /// </summary>
        Message,
      
        /// <summary>
        /// A request for data that would be considered as a large object
        /// </summary>
        GetLargeObject,

        /// <summary>
        /// Replies to large object requests
        /// </summary>
        LargeObject,

        /// <summary>
        /// A keep-alive packet that can also notify a client of state changes.
        /// </summary>
        Heartbeat = 0xFF,
    }
}