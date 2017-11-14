
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
        /// Request the metadata schema if it has changed since the specified version.
        /// 
        /// Payload:
        /// Guid schemaVersion,                 - If a schema mismatch occurs, The entire schema is serialized. Specify Guid.Empty to get a full schema.
        /// long revision,                      - The revision that is cached on the client.
        /// 
        /// Response:
        /// MetadataSchema schema
        /// 
        /// </summary>
        GetMetadataSchema,
        GetMetadataSchemaResponse,

        /// <summary>
        /// Queries metadata from the server.
        /// 
        /// Payload:
        /// Guid schemaVersion,                 - Can be Guid.Empty. If not empty an error is returned if the schema has changed and the client is out of sync.
        /// long revision,                      - The revisionID that the database was on. Ignored if IsUpdateQuery is false.
        /// bool isUpdateQuery                  - Specifies that this query should only be run on rows that have been modified since the specified revision.
        /// SttpQueryExpression expression      - An sttp query expression.
        /// 
        /// Response:
        /// VersionNotCompatible                - If SchemaVersion does not match the current one, or if revision is too old to do an update query on.
        /// 
        /// OR
        /// 
        /// DefineResponseSchema                - Defines the response Table
        /// DefineRow                           - Defines a row of the data
        /// UndefineRow                         - For update queries, indicates this row should be removed if it exists.
        /// Finished                            - Indicates that the streaming of the table has completed.
        /// 
        /// Note: this response can span multiple packets
        /// 
        /// </summary>
        GetMetadata,
        GetMetadataResponse,

        /// <summary>
        /// Updates the subscription for new measurements. 
        /// 
        /// Subcommand: ConfigureOptions        - Defines options for the measurements about to be selected. Such as priority; dead-banding; start/stop times; sample resolution.
        /// Subcommand: AllDataPoints           - Subscribes to everything
        /// Subcommand: DataPointByID           - Specifies individual data points
        /// Subcommand: ByQuery                 - Specifies some kind of query to use to select the measurements.
        /// 
        /// Success/Failed
        /// 
        /// </summary>
        Subscribe,

        /// <summary>
        /// Sends a series of DataPoints as a single packet. 
        /// The encoding of this packet can be rather complex therefore the wire protocol 
        /// will only deal with a buffer
        /// 
        /// Payload:
        /// List<SttpDataPoint> Data;
        /// 
        /// </summary>
        SendDataPoints,

        /// <summary>
        /// Sends a series of DataPoints as a single packet. 
        /// The encoding of this packet can be rather complex therefore the wire protocol 
        /// will only deal with a buffer
        /// 
        /// Payload:
        /// byte encodingMethod 
        /// byte[] Data;
        /// 
        /// </summary>
        SendDataPointsCustom,


        /// <summary>
        /// Indicates that the Data Point Stream has completed. This occurs for historical queries to indicate that 
        /// the last point has been serialized.
        /// </summary>
        CompletedSendingDataPoints,

        /// <summary>
        /// Registers a new data point identifier. 
        /// 
        /// To minimize the size of SttpDataPackets, their identifiers should be converted into RuntimeIDs. There are a configurable number of 
        /// runtime IDs that can be mapped, therefore all identifiers do not have to be mapped.
        /// 
        /// Payload:
        /// List<SttpPointID> Points;
        /// 
        /// </summary>
        RuntimeIDMapping,

        /// <summary>
        /// Negotiates session variables and roles.
        /// 
        /// Subcommand: InitiateReverseConnection     - The connecting client desires to act as the server.
        /// Subcommand: GetAllInstances               - Request all named instances on the server.
        /// Subcommand: ChangeInstance                - Requests to change the instance connected to.
        /// Subcommand: SupportedFuncationality       - Tell the server what functions/versions are supported by your client
        /// Subcommand: ChangeUdpCipher               - Indicates that the UDP cipher needs to be changed.
        /// 
        /// 
        /// Response: 
        /// Subcommand: ChangeInstanceSuccess
        /// Subcommand: ChangeUdpCipherResponse       - Returns data and agrees to change the UDP cipher.
        /// Subcommand: DesiredOperation              - Reply to SupportedFunctionality, indicates the selected mode of operation.
        /// Subcommand: InstanceList                  - The list of all instances this server has.
        /// Subcommand: ReverseConnectionSuccess      - Transfers the role of server to the client.
        /// 
        /// </summary>
        NegotiateSession,
        NegotiateSessionResponse,

        /// <summary>
        /// The specified request failed. 
        /// 
        /// Payload: 
        /// CommandCode FailedCommand   - The command code that failed.
        /// bool TerminateConnection    - Indicates that the connection should be terminated for a failure.
        /// string Reason               - A user friendly message for the failure, can be null.
        /// string Details              - A not so friendly message more helpful for troubleshooters.
        /// </summary>
        RequestFailed,
        /// <summary>
        /// The specified request Succeeded. 
        /// 
        /// Payload: 
        /// CommandCode SuccessCommand  - The command code that succeeded.
        /// string Reason               - A user friendly message for the success, can be null.
        /// string Details              - A not so friendly message more helpful for troubleshooters.
        /// </summary>
        RequestSucceeded,

        /// <summary>
        /// Capable of sending large blocks of data over STTP.
        /// 
        /// Payload:
        /// Subcommand: BeginSend       - Indicates a new large block of data is on its way.
        /// Subcommand: SendFragment    - Indicates that a new fragment of data is being sent.
        /// Subcommand: CancelSend      - Indicates that a send operation is being canceled.
        /// 
        /// </summary>
        BulkTransport,

        /// <summary>
        /// A keep-alive packet.
        /// 
        /// Payload: 
        /// bool ShouldEcho              - Indicates if this packet should be echoed back. 
        /// 
        /// </summary>
        NoOp = 0xFF,
    }
}