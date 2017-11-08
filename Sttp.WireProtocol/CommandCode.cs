
namespace Sttp.WireProtocol
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
        /// </summary>
        BeginFragment,

        /// <summary>
        /// Specifies the next fragment of data. When Offset + Length of data == TotalFragmentSize, the fragment is completed.
        /// 
        /// Layout:
        /// (Implied) Length of fragment
        /// byte[] Fragment               
        /// </summary>
        NextFragment,

        /// <summary>
        /// Requests the version of the current database, along with the schema. 
        /// 
        /// Payload:
        /// bool IncludeSchema   - Indicates if only the current version of the database will be returned. Not the schema.
        /// 
        /// Response:
        /// MetadataSchema schema
        /// 
        /// </summary>
        GetMetadataSchema,
        GetMetadataSchemaResponse,

        /// <summary>
        /// Gets metadata from the server.
        /// 
        /// Payload:
        /// Guid schemaVersion,                 - Can be Guid.Empty. If not empty an error is returned if the schema has changed.
        /// long revision,                      - The revisionID that the schema was on. Ignored if IsUpdateQuery is false.
        /// bool isUpdateQuery                  - Specifies that this query should only be run on rows that has been modified since the specified revision.
        /// SttpQueryExpression expression      - An sttp query expression.
        /// 
        /// </summary>
        GetMetadata,
        GetMetadataResponse,

        /// <summary>
        /// Updates the subscription for new measurements. This subscription can be realtime or historical.
        /// 
        /// Layout:
        /// 
        /// </summary>
        Subscribe,
        SubscribeResponse,

        /// <summary>
        /// Sends a series of DataPoints as a single packet. 
        /// The encoding of this packet can be rather complex depending on the 
        /// advance encoding algorithm that is specified.
        /// </summary>
        SendDataPoints,

        /// <summary>
        /// Registers a new data point identifier. 
        /// 
        /// To minimize the size of SttpDataPackets, their identifiers should be converted into RuntimeIDs. There are a configurable number of 
        /// runtime IDs that can be mapped, therefore all identifiers do not have to be mapped.
        /// 
        /// Payload:
        /// int32 RuntimeID
        /// SttpPointID ID;
        /// </summary>
        RegisterDataPointRuntimeIdentifier,

        /// <summary>
        /// Negotiates session variables and roles.
        /// </summary>
        NegotiateSession,
        NegotiateSessionResponse,

        /// <summary>
        /// The specified request failed. 
        /// 
        /// Payload: 
        /// CommandCode FailedCommand   - The command code that failed.
        /// string Reason      - A user friendly message for the failure, can be null.
        /// string Details     - A not so friendly message more helpful for troubleshooters.
        /// </summary>
        RequestFailed,


        /// <summary>
        /// Capable of sending large blocks of data over STTP.
        /// </summary>
        BulkTransport,

        NoOp = 0xFF,
    }
}