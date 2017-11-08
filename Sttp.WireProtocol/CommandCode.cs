
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
        /// Requests the schema for the metadata. Since all I/O for metadata requests occurs 
        /// with Column/Table IDs, the response for this packet will provide all table and column
        /// for the database.
        /// 
        /// Layout:
        /// SubCommands: GetDatabaseSchema     - Will return all tables with all columns.
        /// SubCommands: GetDatabaseVersion    - Will return the version of the current database.
        /// 
        /// Response:
        /// IF (GetDatabaseSchema)
        /// Subcommands: DefineTable           
        /// Subcommands: DefineColumn
        /// Subcommands: DefineTableRelationship
        /// 
        /// 
        /// IF (GetDatabaseVerion)
        /// Subcommands: DatabaseVersion
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
        /// Subcommand: IsAugmentedSubscription         - Specifies that the subscription request is intended to augment the existing subscription.
        /// Subcommand: DownSamplingPerDay              - Sets the down-sampling mode for this subscription to be a certain number of samples per day.
        ///                                               This is valid for all data points defined after this point.
        /// Subcommand: DownSamplingSamplesPerSecond    - Sets the down-sampling mode for this subscription to be a value every 'X' number of seconds.
        ///                                               This is valid for all data points defined after this point.
        /// Subcommand: Priority                        - Sets the priority 
        ///                                               This is valid for all data points defined after this point.
        /// Subcommand: SubscribeToTheFollowing         - Indicates that all of the following data points should be added the subscription.
        /// Subcommand: UnsubscribeFromTheFollowing     - Indicates that all of the following data points should be removed from the subscription.
        /// Subcommand: AllDataPoints                   - Identifies all data points.
        /// Subcommand: TableDataPoints                 - Identifies all Data Points listed in a table
        /// Subcommand: DataPointByID                   - Specifies a DataPoint by ID
        /// Subcommand: StartTime                       - Sets the start time for a subscription for historical data.
        /// Subcommand: StopTime                        - Sets the stop time for a subscription for historical data.
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
        /// Capable of sending large blocks of data over STTP.
        /// </summary>
        BulkTransport,

        // TODO : assign values
        NegotiateSession,
        NegotiateSessionResponse,

        SecureDataChannel,
        NoOp = 0xFF,
    }
}