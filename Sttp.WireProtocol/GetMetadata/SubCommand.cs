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
        /// Gets a query request from the server.
        /// 
        /// Payload:
        /// Guid schemaVersion,                 - Can be Guid.Empty. If not empty an error is returned if the schema has changed.
        /// long revision,                      - The revisionID that the schema was on. Ignored if IsUpdateQuery is false.
        /// bool isUpdateQuery                  - Specifies that this query should only be run on rows that has been modified since the specified revision.
        /// SttpQueryExpression expression      - An sttp query expression.
        /// 
        /// 
        /// </summary>
        RequestQuery,
    }
}