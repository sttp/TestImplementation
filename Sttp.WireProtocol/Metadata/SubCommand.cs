namespace Sttp.WireProtocol.Metadata
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
        /// Specified schema or revision is not compatible. Recompile query with the latest schema.
        /// </summary>
        VersionNotCompatible,

        /// <summary>
        /// Defines the table structure of the requested table
        /// 
        /// Payload:
        /// bool isUpdateQuery, - Indicates if the results are a complete set or an update set. 
        ///                       Note: if the SchemaVersion matches, but the data cannot be patched for some reason, a full set of data is serialized regardless of the client's request.
        /// long updatedFromRevision - If isUpdatedQuery, this is the revision number that was supplied in the GetMetadata request.
        /// Guid schemaVersion, 
        /// long revision, 
        /// string tableName,
        /// Array columns{string columnName, SttpValueTypeCode columnType}
        /// 
        /// </summary>
        DefineResponse,

        /// <summary>
        /// Defines a row to an existing table. This will either be a new row, or replacing an exiting one.
        /// 
        /// Payload: 
        /// SttpValue primaryKey,  -The primary key for the data being parsed.
        /// SttpValueSet fields    -All of the fields for this row.
        /// 
        /// </summary>
        DefineRow,

        /// <summary>
        /// Indicates that a row of data has been removed. It's possible that the client may not have this row. In which case, ignore it.
        /// This is only valid for queries that are specified as update queries.
        /// 
        /// Payload: 
        /// SttpValue primaryKey,
        /// 
        /// </summary>
        UndefineRow,

        /// <summary>
        /// Indicates that the requested metadata has finished streaming.
        /// </summary>
        Finished,
    }
}