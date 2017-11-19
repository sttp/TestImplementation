namespace Sttp.WireProtocol.GetMetadata
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

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
        Schema,

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
        Query,
    }
}